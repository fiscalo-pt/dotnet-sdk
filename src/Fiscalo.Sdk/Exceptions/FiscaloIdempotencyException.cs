using Fiscalo.Sdk.Models;

namespace Fiscalo.Sdk.Exceptions;

public sealed class FiscaloIdempotencyException : FiscaloApiException
{
    public FiscaloIdempotencyException(
        string message,
        string errorCode = "idempotency_conflict",
        int statusCode = 409,
        string? requestId = null,
        IReadOnlyDictionary<string, IReadOnlyList<string>>? details = null,
        IReadOnlyDictionary<string, IReadOnlyList<FiscaloValidationError>>? fieldErrors = null,
        string? suggestion = null)
        : base(message, errorCode, statusCode, requestId, details, fieldErrors, suggestion)
    {
    }
}
