using System.Net;
using System.Text;
using Fiscalo.Sdk.Models;

namespace Fiscalo.Sdk.Tests;

public sealed class CustomersResourceTests
{
    [Fact]
    public async Task CreateCustomerUsesCustomersEndpoint()
    {
        var handler = new StubHttpMessageHandler((request, _) =>
        {
            Assert.Equal(HttpMethod.Post, request.Method);
            Assert.Equal("https://api.fiscalo.pt/api/v1/customers", request.RequestUri?.ToString());

            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("""{"data":{"id":"cus_123","name":"Cliente .NET"}}""", Encoding.UTF8, "application/json"),
            });
        });

        var fiscalo = new Fiscalo.Sdk.FiscaloClient(
            apiKey: "fisc_test_dotnet",
            baseUrl: "https://api.fiscalo.pt/api/v1",
            httpClient: new HttpClient(handler));

        var response = await fiscalo.Customers.CreateAsync(new CreateCustomerRequest
        {
            Name = "Cliente .NET",
            CustomerType = "company",
            Country = "PT",
        });

        Assert.Equal("cus_123", response.Data?.Id);
    }
}
