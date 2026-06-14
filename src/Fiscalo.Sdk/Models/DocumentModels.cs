namespace Fiscalo.Sdk.Models;

public sealed class DocumentSeries
{
    public string? Id { get; init; }
    public string? DocumentType { get; init; }
    public string? Code { get; init; }
    public string? Status { get; init; }
    public string? ValidationCode { get; init; }
    public string? ExternalId { get; init; }
    public DateTimeOffset? CreatedAt { get; init; }
}

public sealed class CreateDocumentSeriesRequest
{
    public string DocumentType { get; init; } = "invoice";
    public string Code { get; init; } = string.Empty;
    public string? Description { get; init; }
}

public sealed class Document
{
    public string? Id { get; init; }
    public string? DocumentNumber { get; init; }
    public string? Status { get; init; }
    public string? DocumentType { get; init; }
    public string? CustomerId { get; init; }
    public string? SeriesId { get; init; }
    public string? Currency { get; init; }
    public decimal? Subtotal { get; init; }
    public decimal? TaxTotal { get; init; }
    public decimal? Total { get; init; }
    public IReadOnlyList<DocumentLine> Lines { get; init; } = Array.Empty<DocumentLine>();
    public DateTimeOffset? IssuedAt { get; init; }
    public DateTimeOffset? CreatedAt { get; init; }
    public DateTimeOffset? UpdatedAt { get; init; }
}

public sealed class CreateDocumentRequest
{
    public string? CustomerId { get; init; }
    public string? SeriesId { get; init; }
    public string DocumentType { get; init; } = "invoice";
    public string Source { get; init; } = "api";
    public string Currency { get; init; } = "EUR";
    public string? ExternalId { get; init; }
    public string? Notes { get; init; }
}

public sealed class DocumentLine
{
    public string? Id { get; init; }
    public string? ItemId { get; init; }
    public string? Description { get; init; }
    public decimal? Quantity { get; init; }
    public string? Unit { get; init; }
    public decimal? UnitPrice { get; init; }
    public decimal? DiscountRate { get; init; }
    public decimal? Total { get; init; }
}

public class CreateDocumentLineRequest
{
    public string? ItemId { get; init; }
    public string? Description { get; init; }
    public decimal Quantity { get; init; }
    public string? Unit { get; init; }
    public decimal? UnitPrice { get; init; }
    public decimal DiscountRate { get; init; }
}

public sealed class AddDocumentLineRequest : CreateDocumentLineRequest;

public sealed class IssueDocumentRequest
{
    public string? ActorType { get; init; }
    public string? ActorId { get; init; }
    public Dictionary<string, object?> Metadata { get; init; } = new();
}
