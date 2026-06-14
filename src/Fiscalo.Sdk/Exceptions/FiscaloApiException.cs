using Fiscalo.Sdk.Models;

namespace Fiscalo.Sdk.Exceptions;

public class FiscaloApiException : Exception
{
    public FiscaloApiException(
        string message,
        string errorCode = "api_error",
        int statusCode = 0,
        string? requestId = null,
        IReadOnlyDictionary<string, IReadOnlyList<string>>? details = null,
        IReadOnlyDictionary<string, IReadOnlyList<FiscaloValidationError>>? fieldErrors = null,
        string? suggestion = null) : base(message)
    {
        ErrorCode = errorCode;
        StatusCode = statusCode;
        RequestId = requestId;
        Details = details;
        FieldErrors = fieldErrors;
        Suggestion = suggestion;
    }

    public string ErrorCode { get; }

    public string Code => ErrorCode;

    public int StatusCode { get; }

    public string? RequestId { get; }

    public IReadOnlyDictionary<string, IReadOnlyList<string>>? Details { get; }

    public IReadOnlyDictionary<string, IReadOnlyList<FiscaloValidationError>>? FieldErrors { get; }

    public string? Suggestion { get; }
}
