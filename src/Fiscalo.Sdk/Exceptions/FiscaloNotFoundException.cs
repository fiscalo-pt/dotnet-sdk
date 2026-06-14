using Fiscalo.Sdk.Models;

namespace Fiscalo.Sdk.Exceptions;

public sealed class FiscaloNotFoundException : FiscaloApiException
{
    public FiscaloNotFoundException(
        string message,
        string errorCode = "resource_not_found",
        int statusCode = 404,
        string? requestId = null,
        IReadOnlyDictionary<string, IReadOnlyList<string>>? details = null,
        IReadOnlyDictionary<string, IReadOnlyList<FiscaloValidationError>>? fieldErrors = null,
        string? suggestion = null)
        : base(message, errorCode, statusCode, requestId, details, fieldErrors, suggestion)
    {
    }
}
