using BuildingBlocks.Application.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Api.Filters;

public class FluentValidationFilter : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
        if (!context.ModelState.IsValid)
        {
            var errorMessages = context.ModelState
                .Where(x => x.Value?.Errors.Count > 0)
                .SelectMany(x => x.Value!.Errors.Select(e => e.ErrorMessage))
                .ToList();

            if (errorMessages.Any())
            {
                var result = Result.Factory.Error(errorMessages, StatusCodes.Status422UnprocessableEntity);
                context.Result = new ObjectResult(result)
                {
                    StatusCode = StatusCodes.Status422UnprocessableEntity
                };
            }
        }
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
    }
}

