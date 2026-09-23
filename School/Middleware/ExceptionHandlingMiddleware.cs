using Microsoft.AspNetCore.Mvc.Filters;
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

                var statusCode = ResolveStatusCode(ex);
                await WriteProblemAsync(context, statusCode, ResolveDetail(ex, statusCode));

            }
        }

        private static string ResolveDetail(Exception ex, HttpStatusCode statusCode) =>
            statusCode == HttpStatusCode.InternalServerError ? "An unexpected error occurred." : ex.Message;
        private static HttpStatusCode ResolveStatusCode(Exception ex) => ex
            switch
                {
                    EntityNotFoundException => HttpStatusCode.NotFound,
                    DuplicateUserNameException => HttpStatusCode.Conflict,
                    ExistsDayException => HttpStatusCode.Conflict,

                };
        private static Task WriteProblemAsync(HttpContext context, HttpStatusCode statusCode, string detail)
        {
            context.Response.ContentType = "application/problem+json";
            context.Response.StatusCode = (int)statusCode;
            var problem = new
            {
                Status = (int)statusCode,
                title = statusCode.ToString(),
                detail
            };
            return context.Response.WriteAsJsonAsync(problem);
        }
    }
}
