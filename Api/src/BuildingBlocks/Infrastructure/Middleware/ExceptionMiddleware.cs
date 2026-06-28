using BuildingBlocks.Application.Exceptions;
using BuildingBlocks.Application.Results;
using FluentValidation;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace BuildingBlocks.Infrastructure.Middleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        Result result;

        switch (exception)
        {
            case AppException appException:
                result = Result.Factory.Error(appException.Errors.ToList(), appException.StatusCode);
                break;

            case ValidationException validationException:
                var validationErrors = validationException.Errors.Select(e => e.ErrorMessage).ToList();
                result = Result.Factory.Error(validationErrors, StatusCodes.Status422UnprocessableEntity);
                break;

            case ArgumentException:
                result = Result.Factory.Error(exception.Message, StatusCodes.Status400BadRequest);
                break;

            case InvalidOperationException:
                result = Result.Factory.Error(exception.Message, StatusCodes.Status400BadRequest);
                break;

            case KeyNotFoundException:
                result = Result.Factory.Error(exception.Message, StatusCodes.Status404NotFound);
                break;

            case UnauthorizedAccessException:
                result = Result.Factory.Error(exception.Message, StatusCodes.Status401Unauthorized);
                break;

            default:
                result = Result.Factory.Error("An internal server error occurred.", StatusCodes.Status500InternalServerError);
                break;
        }

        context.Response.StatusCode = result.StatusCode;

        var json = JsonSerializer.Serialize(result, JsonOptions);
        await context.Response.WriteAsync(json);
    }
}

