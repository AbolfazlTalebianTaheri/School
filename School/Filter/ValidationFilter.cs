using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace School.Api.Filter
{
    public class ValidationFilter(IServiceProvider serviceProvider) : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var errors = new Dictionary<string, string[]>();
            foreach (var paramters in context.ActionDescriptor.Parameters)
            {
                if (!context.ActionArguments.TryGetValue(paramters.Name, out var argument) || argument is null)
                    continue;
                var validator = serviceProvider.GetService(typeof(IValidator<>).MakeGenericType(argument.GetType())) as IValidator;
                if (validator is null)
                    continue;
                var validationContext = new ValidationContext<object>(argument);
                var result = await validator.ValidateAsync(validationContext, context.HttpContext.RequestAborted);
                if (result.IsValid)
                    continue;
                foreach (var gruop in result.Errors.GroupBy(x => x.PropertyName))
                    errors[gruop.Key] = gruop.Select(x => x.ErrorMessage).ToArray();
            }
            if (errors.Count > 0)
            {
                context.Result = new BadRequestObjectResult(new ValidationProblemDetails(errors)
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "BadRequest",
                    Detail = "One or more validation errors occurred."
                });
                return;
            }
            await next();
        }
    }
}
