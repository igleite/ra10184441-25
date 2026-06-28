using BuildingBlocks.Application.Enums;
using Microsoft.AspNetCore.Http;

namespace BuildingBlocks.Application.Results;

public class Result<T> : Result
{
    public T Data { get; internal set; } = default!;

    internal Result() : base()
    {
    }
}

public class Result
{
    public ResultTypeEnum TypeEnum { get; private set; }
    public List<string> Message { get; private set; } = new List<string>();
    public string Timestamp { get; private set; } = string.Empty;
    public int StatusCode { get; private set; } = StatusCodes.Status200OK;

    protected Result()
    {
    }

    public static class Factory<T>
    {
        public static Result<TData> Success<TData>(TData data, int statusCode = StatusCodes.Status200OK)
        {
            if (statusCode < StatusCodes.Status200OK || statusCode >= StatusCodes.Status300MultipleChoices)
                throw new ArgumentException("Status code must be a success status (2xx).", nameof(statusCode));

            var timestamp = DateTime.Now.ToString("O");
            var result = new Result<TData>();
            result.TypeEnum = ResultTypeEnum.Success;
            result.Timestamp = timestamp;
            result.Data = data;
            result.StatusCode = statusCode;
            return result;
        }

        public static Result<T> Error(List<string> message, int statusCode)
        {
            if (statusCode < StatusCodes.Status400BadRequest)
                throw new Exception("Status code must be 4xx (client error) or 5xx (server error)");

            var timestamp = DateTime.Now.ToString("O");
            var result = new Result<T>();
            result.TypeEnum = ResultTypeEnum.Error;
            result.Timestamp = timestamp;
            result.Message = message;
            result.StatusCode = statusCode;
            return result;
        }

        public static Result<T> Error(string message, int statusCode)
        {
            return Error(new List<string> { message }, statusCode);
        }

        public static Result<T> Warning(List<string> message, int statusCode, T? data = default)
        {
            var timestamp = DateTime.Now.ToString("O");
            var result = new Result<T>();
            result.TypeEnum = ResultTypeEnum.Warning;
            result.Timestamp = timestamp;
            result.Message = message;
            result.StatusCode = statusCode;
            if (data != null)
            {
                result.Data = data;
            }
            return result;
        }

        public static Result<T> Warning(string message, int statusCode, T data = default)
        {
            return Warning(new List<string> { message }, statusCode, data);
        }
    }

    public static class Factory
    {
        public static Result Success(int statusCode = StatusCodes.Status200OK)
        {
            if (statusCode < StatusCodes.Status200OK || statusCode >= StatusCodes.Status300MultipleChoices)
                throw new ArgumentException("Status code must be a success status (2xx).", nameof(statusCode));

            var timestamp = DateTime.Now.ToString("O");
            var result = new Result();
            result.TypeEnum = ResultTypeEnum.Success;
            result.Timestamp = timestamp;
            result.StatusCode = statusCode;
            return result;
        }

        public static Result Error(List<string> message, int statusCode)
        {
            if (statusCode < StatusCodes.Status400BadRequest)
                throw new Exception("Status code must be 4xx (client error) or 5xx (server error)");

            var timestamp = DateTime.Now.ToString("O");
            var result = new Result();
            result.TypeEnum = ResultTypeEnum.Error;
            result.Timestamp = timestamp;
            result.Message = message;
            result.StatusCode = statusCode;
            return result;
        }

        public static Result Error(string message, int statusCode)
        {
            return Error(new List<string> { message }, statusCode);
        }

        public static Result Warning(List<string> message, int statusCode)
        {
            if (statusCode < StatusCodes.Status200OK || statusCode >= StatusCodes.Status300MultipleChoices)
                throw new ArgumentException("Warning must use a success HTTP status code (2xx).", nameof(statusCode));

            var timestamp = DateTime.Now.ToString("O");
            var result = new Result();
            result.TypeEnum = ResultTypeEnum.Warning;
            result.Timestamp = timestamp;
            result.Message = message;
            result.StatusCode = statusCode;
            return result;
        }

        public static Result Warning(string message, int statusCode)
        {
            return Warning(new List<string> { message }, statusCode);
        }
    }
}