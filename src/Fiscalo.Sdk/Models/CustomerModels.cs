namespace Fiscalo.Sdk.Models;

public sealed class Customer
{
    public string? Id { get; init; }
    public string? Name { get; init; }
    public string? TaxNumber { get; init; }
    public string? ExternalId { get; init; }
    public string? Email { get; init; }
    public string? Phone { get; init; }
    public string? CustomerType { get; init; }
    public string? Country { get; init; }
    public string? AddressLine1 { get; init; }
    public string? AddressLine2 { get; init; }
    public string? PostalCode { get; init; }
    public string? City { get; init; }
    public DateTimeOffset? CreatedAt { get; init; }
    public DateTimeOffset? UpdatedAt { get; init; }
}

public sealed class CreateCustomerRequest
{
    public string Name { get; init; } = string.Empty;
    public string? TaxNumber { get; init; }
    public string? Email { get; init; }
    public string? Phone { get; init; }
    public string CustomerType { get; init; } = "company";
    public string Country { get; init; } = "PT";
    public string? AddressLine1 { get; init; }
    public string? AddressLine2 { get; init; }
    public string? PostalCode { get; init; }
    public string? City { get; init; }
    public string? ExternalId { get; init; }
}

public sealed class UpdateCustomerRequest
{
    public string? Name { get; init; }
    public string? TaxNumber { get; init; }
    public string? Email { get; init; }
    public string? Phone { get; init; }
    public string? CustomerType { get; init; }
    public string? Country { get; init; }
    public string? AddressLine1 { get; init; }
    public string? AddressLine2 { get; init; }
    public string? PostalCode { get; init; }
    public string? City { get; init; }
    public string? ExternalId { get; init; }
}
