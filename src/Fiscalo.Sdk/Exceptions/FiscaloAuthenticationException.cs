using Fiscalo.Sdk.Models;

namespace Fiscalo.Sdk.Exceptions;

public sealed class FiscaloAuthenticationException : FiscaloApiException
{
    public FiscaloAuthenticationException(
        string message,
        string errorCode = "authentication_required",
        int statusCode = 401,
        string? requestId = null,
        IReadOnlyDictionary<string, IReadOnlyList<string>>? details = null,
        IReadOnlyDictionary<string, IReadOnlyList<FiscaloValidationError>>? fieldErrors = null,
        string? suggestion = null)
        : base(message, errorCode, statusCode, requestId, details, fieldErrors, suggestion)
    {
    }
}
