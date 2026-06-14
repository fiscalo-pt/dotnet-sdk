using Fiscalo.Sdk.Models;
using Fiscalo.Sdk.Support;

namespace Fiscalo.Sdk.Resources;

/// <summary>
/// Provides access to Fiscalo customer endpoints.
/// </summary>
public sealed class CustomersResource
{
    private readonly FiscaloHttpClient httpClient;

    /// <summary>
    /// Creates a customer resource wrapper over the shared Fiscalo transport.
    /// </summary>
    public CustomersResource(FiscaloHttpClient httpClient) => this.httpClient = httpClient;

    /// <summary>
    /// Lists customers available in the current Fiscalo environment.
    /// </summary>
    public Task<FiscaloListResponse<Customer>> ListAsync(CancellationToken cancellationToken = default) =>
        httpClient.SendCollectionAsync<Customer>(HttpMethod.Get, "customers", cancellationToken: cancellationToken);

    /// <summary>
    /// Creates a new customer in Fiscalo.
    /// </summary>
    public Task<FiscaloResponse<Customer>> CreateAsync(CreateCustomerRequest payload, CancellationToken cancellationToken = default) =>
        httpClient.SendEnvelopeAsync<Customer>(HttpMethod.Post, "customers", payload, cancellationToken: cancellationToken);

    /// <summary>
    /// Retrieves a single customer by identifier.
    /// </summary>
    public Task<FiscaloResponse<Customer>> GetAsync(string id, CancellationToken cancellationToken = default) =>
        httpClient.SendEnvelopeAsync<Customer>(HttpMethod.Get, $"customers/{id}", cancellationToken: cancellationToken);

    /// <summary>
    /// Applies a partial update to an existing customer.
    /// </summary>
    public Task<FiscaloResponse<Customer>> UpdateAsync(string id, UpdateCustomerRequest payload, CancellationToken cancellationToken = default) =>
        httpClient.SendEnvelopeAsync<Customer>(HttpMethod.Patch, $"customers/{id}", payload, cancellationToken: cancellationToken);
}
