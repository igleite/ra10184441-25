using Microsoft.AspNetCore.Http;

namespace BuildingBlocks.Application.Exceptions;

public sealed class AppException : Exception
{
    public IReadOnlyList<string> Errors { get; }
    public int StatusCode { get; }

    private AppException(IEnumerable<string> messages, int statusCode)
        : base(messages.FirstOrDefault() ?? "An error occurred.")
    {
        Errors = messages.ToList().AsReadOnly();
        StatusCode = statusCode;
    }

    private AppException(IEnumerable<string> messages, int statusCode, Exception innerException)
        : base(messages.FirstOrDefault() ?? "An error occurred.", innerException)
    {
        Errors = messages.ToList().AsReadOnly();
        StatusCode = statusCode;
    }

    private static AppException Create(IEnumerable<string> messages, string defaultMessage, int statusCode)
    {
        var errorList = messages?.ToList() ?? new List<string> { defaultMessage };
        return new AppException(errorList, statusCode);
    }

    private static AppException Create(string message, string defaultMessage, int statusCode)
        => Create(message != null ? new[] { message } : null, defaultMessage, statusCode);

    private static AppException Create(IEnumerable<string> messages, string defaultMessage, int statusCode, Exception innerException)
    {
        var errorList = messages?.ToList() ?? new List<string> { defaultMessage };
        return new AppException(errorList, statusCode, innerException);
    }

    private static AppException Create(string message, string defaultMessage, int statusCode, Exception innerException)
        => Create(message != null ? new[] { message } : null, defaultMessage, statusCode, innerException);

    public static AppException NotFound(string message = null) 
        => Create(message, "Resource was not found.", StatusCodes.Status404NotFound);

    public static AppException NotFound(IEnumerable<string> messages) 
        => Create(messages, "Resource was not found.", StatusCodes.Status404NotFound);

    public static AppException BadRequest(string message = null) 
        => Create(message, "The request is invalid or contains incorrect data.", StatusCodes.Status400BadRequest);

    public static AppException BadRequest(IEnumerable<string> messages) 
        => Create(messages, "The request is invalid or contains incorrect data.", StatusCodes.Status400BadRequest);

    public static AppException Unauthorized(string message = null) 
        => Create(message, "You are not authorized to perform this action.", StatusCodes.Status401Unauthorized);

    public static AppException Unauthorized(IEnumerable<string> messages) 
        => Create(messages, "You are not authorized to perform this action.", StatusCodes.Status401Unauthorized);

    public static AppException Forbidden(string message = null) 
        => Create(message, "You do not have permission to access this resource.", StatusCodes.Status403Forbidden);

    public static AppException Forbidden(IEnumerable<string> messages) 
        => Create(messages, "You do not have permission to access this resource.", StatusCodes.Status403Forbidden);

    public static AppException Conflict(string message = null) 
        => Create(message, "There is a conflict with the current state of the resource.", StatusCodes.Status409Conflict);

    public static AppException Conflict(IEnumerable<string> messages) 
        => Create(messages, "There is a conflict with the current state of the resource.", StatusCodes.Status409Conflict);

    public static AppException UnprocessableEntity(string message = null) 
        => Create(message, "The entity could not be processed due to validation errors.", StatusCodes.Status422UnprocessableEntity);

    public static AppException UnprocessableEntity(IEnumerable<string> messages) 
        => Create(messages, "The entity could not be processed due to validation errors.", StatusCodes.Status422UnprocessableEntity);

    public static AppException InternalServerError(string message = null) 
        => Create(message, "An unexpected error occurred in the system.", StatusCodes.Status500InternalServerError);

    public static AppException InternalServerError(IEnumerable<string> messages) 
        => Create(messages, "An unexpected error occurred in the system.", StatusCodes.Status500InternalServerError);

    public static AppException ServiceUnavailable(string message = null) 
        => Create(message, "The service is currently unavailable. Please try again later.", StatusCodes.Status503ServiceUnavailable);

    public static AppException ServiceUnavailable(IEnumerable<string> messages) 
        => Create(messages, "The service is currently unavailable. Please try again later.", StatusCodes.Status503ServiceUnavailable);

    public static AppException RequestTimeout(string message = null) 
        => Create(message, "A requisição expirou. Tente novamente mais tarde.", StatusCodes.Status408RequestTimeout);

    public static AppException RequestTimeout(IEnumerable<string> messages) 
        => Create(messages, "A requisição expirou. Tente novamente mais tarde.", StatusCodes.Status408RequestTimeout);

    public static AppException TooManyRequests(string message = null) 
        => Create(message, "Muitas requisições. Tente novamente mais tarde.", StatusCodes.Status429TooManyRequests);

    public static AppException TooManyRequests(IEnumerable<string> messages) 
        => Create(messages, "Muitas requisições. Tente novamente mais tarde.", StatusCodes.Status429TooManyRequests);

    public static AppException Wrap(string message, Exception innerException) 
        => Create(message, "An error occurred.", StatusCodes.Status500InternalServerError, innerException);

    public static AppException Wrap(IEnumerable<string> messages, Exception innerException) 
        => Create(messages, "An error occurred.", StatusCodes.Status500InternalServerError, innerException);
}