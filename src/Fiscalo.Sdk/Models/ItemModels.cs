namespace Fiscalo.Sdk.Models;

public sealed class Item
{
    public string? Id { get; init; }
    public string? Type { get; init; }
    public string? Sku { get; init; }
    public string? Name { get; init; }
    public string? Description { get; init; }
    public string? Unit { get; init; }
    public decimal? UnitPrice { get; init; }
    public decimal? VatRate { get; init; }
    public DateTimeOffset? CreatedAt { get; init; }
    public DateTimeOffset? UpdatedAt { get; init; }
}

public sealed class CreateItemRequest
{
    public string Type { get; init; } = "service";
    public string Name { get; init; } = string.Empty;
    public string? Sku { get; init; }
    public string? Description { get; init; }
    public string Unit { get; init; } = "un";
    public decimal UnitPrice { get; init; }
    public decimal? VatRate { get; init; }
}

public sealed class UpdateItemRequest
{
    public string? Type { get; init; }
    public string? Name { get; init; }
    public string? Sku { get; init; }
    public string? Description { get; init; }
    public string? Unit { get; init; }
    public decimal? UnitPrice { get; init; }
    public decimal? VatRate { get; init; }
}
