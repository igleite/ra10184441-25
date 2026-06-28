using BuildingBlocks.Application.Exceptions;
using BuildingBlocks.Application.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text.Json;

namespace Api.Filters;

public class ResultFilter : IAsyncResultFilter
{
    private static readonly JsonSerializerOptions JsonOptions = new JsonSerializerOptions
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
    {
        if (context.Result is ObjectResult objectResult)
        {
            var value = objectResult.Value;

            if (value is Result result)
            {
                context.HttpContext.Response.StatusCode = result.StatusCode;
                
                if (result.StatusCode == StatusCodes.Status204NoContent)
                {
                    context.Result = new EmptyResult();
                }
                else
                {
                    context.Result = new JsonResult(result, JsonOptions);
                }
            }
            else if (value != null)
            {
                throw AppException.InternalServerError("The endpoint must return a Result or Result<T>.");
            }
        }

        await next();
    }
}

