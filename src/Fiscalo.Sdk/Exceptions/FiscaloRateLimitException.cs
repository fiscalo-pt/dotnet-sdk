using Fiscalo.Sdk.Models;

namespace Fiscalo.Sdk.Exceptions;

public sealed class FiscaloRateLimitException : FiscaloApiException
{
    public FiscaloRateLimitException(
        string message,
        string errorCode = "rate_limit_exceeded",
        int statusCode = 429,
        string? requestId = null,
        string? retryAfter = null,
        IReadOnlyDictionary<string, IReadOnlyList<string>>? details = null,
        IReadOnlyDictionary<string, IReadOnlyList<FiscaloValidationError>>? fieldErrors = null,
        string? suggestion = null)
        : base(message, errorCode, statusCode, requestId, details, fieldErrors, suggestion)
    {
        RetryAfter = retryAfter;
    }

    public string? RetryAfter { get; }
}
