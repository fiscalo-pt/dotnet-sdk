namespace Fiscalo.Sdk.Models;

public sealed class ApiErrorEnvelope
{
    public ApiErrorBody? Error { get; init; }

    public ResponseMeta? Meta { get; init; }
}

public sealed class ApiErrorBody
{
    public string? Code { get; init; }

    public string? Message { get; init; }

    public string? Suggestion { get; init; }

    public Dictionary<string, IReadOnlyList<string>>? Details { get; init; }

    public Dictionary<string, IReadOnlyList<FiscaloValidationError>>? Fields { get; init; }
}

public sealed class ResponseMeta
{
    public string? RequestId { get; init; }

    public string? Environment { get; init; }

    public PaginationMeta? Pagination { get; init; }
}

public sealed class PaginationMeta
{
    public int? CurrentPage { get; init; }

    public int? LastPage { get; init; }

    public int? PerPage { get; init; }

    public int? Total { get; init; }
}

public sealed class ApiLinks
{
    public string? First { get; init; }

    public string? Last { get; init; }

    public string? Previous { get; init; }

    public string? Next { get; init; }
}

public sealed class ApiEnvelope<T>
{
    public T? Data { get; init; }

    public ResponseMeta? Meta { get; init; }

    public ApiLinks? Links { get; init; }
}

public sealed class ApiCollectionEnvelope<T>
{
    public IReadOnlyList<T> Data { get; init; } = Array.Empty<T>();

    public ResponseMeta? Meta { get; init; }

    public ApiLinks? Links { get; init; }
}

public sealed class FiscaloError
{
    public string? Code { get; init; }

    public string? Message { get; init; }

    public string? Suggestion { get; init; }

    public IReadOnlyDictionary<string, IReadOnlyList<string>>? Details { get; init; }
}

public sealed class FiscaloValidationError
{
    public string? Code { get; init; }

    public string? Message { get; init; }

    public string? Suggestion { get; init; }
}
