
using Microsoft.AspNetCore.Mvc;
using School.Api.Middleware;
using School.Application.Service;
using School.Domain.Exceptions;
using System.Net;

namespace School.Api.Middleware
{
    public class ExceptionHandlingMiddleware(RequestDelegate request, ILogger<ExceptionHandlingMiddleware> logger)
    {
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await request(context);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error while processing {Path}", context.Request.Path);

                var statusCode = ResolveStatusCode(ex);
                try
                {
                    var exceptionLogService = context.RequestServices.GetService<ExceptionLogService>();
                    if (exceptionLogService is not null)
                        await exceptionLogService.LogAsync(ex, (int)statusCode, context.Request.Path, context.Request.Method);
                }
                catch (Exception logException)
                {
                    logger.LogError(logException, "Failed to save exception log to MongoDB.");
                }
                if (context.Response.HasStarted)
                    throw;
                await WriteProblemAsync(context, statusCode, ResolveDetail(ex, statusCode));
            }
        }
        private static string ResolveDetail(Exception ex, HttpStatusCode statusCode) =>
            statusCode == HttpStatusCode.InternalServerError ?
            "An unexpected error occurred." : ex.Message;

        private static HttpStatusCode ResolveStatusCode(Exception ex) =>
            ex switch
            {
                EntityNotFoundException => HttpStatusCode.NotFound,
                DuplicateUserNameException => HttpStatusCode.Conflict,
                DuplicateStudentException => HttpStatusCode.Conflict,
                ExistsDayException => HttpStatusCode.Conflict,
                DuplicateLogForStudentInDateException => HttpStatusCode.Conflict,
                InvalidOtpException => HttpStatusCode.BadRequest,
                OtpRateLimitException => HttpStatusCode.TooManyRequests,
                OtpDeliveryException => HttpStatusCode.ServiceUnavailable,
                _ => HttpStatusCode.InternalServerError
            };
        private static Task WriteProblemAsync(HttpContext context,HttpStatusCode statusCode,string detail)
        {
            context.Response.ContentType = "application/problem+json";
            context.Response.StatusCode = (int)statusCode;
            var problem = new ProblemDetails
            {
                Status = (int)statusCode,
                Title = statusCode.ToString(),
                Detail = detail
            };
            return context.Response.WriteAsJsonAsync(problem);
        }
    }
}
