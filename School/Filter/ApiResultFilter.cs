using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using School.Api.Contracts;

namespace School.Api.Filter
{
    public class ApiResultFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (context.Result is ApiResult or ApiResult<object>)
            {
                await next();
                return;
            }
            int statusCode;
            switch (context.Result)
            {
                case OkObjectResult okResult:
                    statusCode = StatusCodes.Status200OK;
                    WarpObjectResult(okResult, statusCode);
                    break;
                case CreatedResult createdResult:
                    statusCode = StatusCodes.Status201Created;
                    WarpObjectResult(createdResult, statusCode);
                    break;
                case AcceptedResult acceptedResult:
                    statusCode = StatusCodes.Status202Accepted;
                    WarpObjectResult(acceptedResult, statusCode);
                    break;
                case NotFoundResult:
                    statusCode = StatusCodes.Status404NotFound;
                    context.Result = new ObjectResult(ApiResult.Failed(null, statusCode))
                    { StatusCode = statusCode };
                    break;
                case NoContentResult:
                    statusCode = StatusCodes.Status204NoContent;
                    context.Result = new ObjectResult(ApiResult.NoContent())
                    { StatusCode = statusCode };
                    break;
                case ObjectResult { StatusCode: not null } objectResult:
                    statusCode = objectResult.StatusCode.Value;
                    WarpObjectResult(objectResult, statusCode);
                    break;
            }
            await next();
        }

        private static void WarpObjectResult(ObjectResult objectResult, int statusCode)
        {
            var apiResult = statusCode is >= 200 and <= 300 ?
                ApiResult<object?>.Succeeded(objectResult.Value, statusCode) :
                ApiResult.Failed(objectResult.Value, statusCode);
            objectResult.Value = apiResult;
            objectResult.StatusCode = statusCode;
        }
    }
}
