using BuildingBlocks.Application.Interfaces.Uow;
using Microsoft.AspNetCore.Http;

namespace BuildingBlocks.Infrastructure.Middleware;

public class TransactionMiddleware(RequestDelegate _next)
{
    private static readonly HashSet<string> WriteMethods = ["POST", "PUT", "DELETE", "PATCH"];

    public async Task InvokeAsync(HttpContext context, IUnitOfWork unitOfWork)
    {
        if (!ShouldOpenTransaction(context))
        {
            await _next(context);
            return;
        }

        await unitOfWork.SdpDpNew.BeginTransactionAsync(context.RequestAborted);

        try
        {
            await _next(context);

            if (IsSuccessStatusCode(context.Response.StatusCode))
                await unitOfWork.SdpDpNew.CommitAsync(context.RequestAborted);
            else
                await unitOfWork.SdpDpNew.RollbackAsync(context.RequestAborted);
        }
        catch
        {
            await unitOfWork.SdpDpNew.RollbackAsync(context.RequestAborted);
            throw;
        }
    }

    private static bool ShouldOpenTransaction(HttpContext context) =>
        WriteMethods.Contains(context.Request.Method, StringComparer.OrdinalIgnoreCase);

    private static bool IsSuccessStatusCode(int statusCode) =>
        statusCode >= StatusCodes.Status200OK && statusCode < StatusCodes.Status300MultipleChoices;
}

