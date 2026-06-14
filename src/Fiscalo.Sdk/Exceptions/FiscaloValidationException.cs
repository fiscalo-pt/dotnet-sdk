using Fiscalo.Sdk.Models;

namespace Fiscalo.Sdk.Exceptions;

public sealed class FiscaloValidationException : FiscaloApiException
{
    public FiscaloValidationException(
        string message,
        string errorCode = "validation_error",
        int statusCode = 422,
        string? requestId = null,
        IReadOnlyDictionary<string, IReadOnlyList<string>>? details = null,
        IReadOnlyDictionary<string, IReadOnlyList<FiscaloValidationError>>? fieldErrors = null,
        string? suggestion = null)
        : base(message, errorCode, statusCode, requestId, details, fieldErrors, suggestion)
    {
    }
}
