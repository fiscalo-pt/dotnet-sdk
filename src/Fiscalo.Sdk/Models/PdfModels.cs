namespace Fiscalo.Sdk.Models;

public sealed class DocumentPdf
{
    public string? DocumentId { get; init; }
    public string? DownloadUrl { get; init; }
    public string? FileName { get; init; }
    public string? ContentType { get; init; }
    public string? Template { get; init; }
    public DateTimeOffset? GeneratedAt { get; init; }
}
