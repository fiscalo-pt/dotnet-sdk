using Fiscalo.Sdk.Resources;

namespace Fiscalo.Sdk.Abstractions;

public interface IFiscaloClient
{
    string ApiKey { get; }

    string BaseUrl { get; }

    TimeSpan Timeout { get; }

    CustomersResource Customers { get; }

    ItemsResource Items { get; }

    DocumentSeriesResource DocumentSeries { get; }

    DocumentsResource Documents { get; }

    DocumentLinesResource DocumentLines { get; }

    PdfResource Pdf { get; }
}
