using System.Text.Json.Serialization;

namespace UserRegistration.WebApi.Contracts.Shared;

public sealed class CloudEvent<T> where T : class
{
    [JsonPropertyName("specversion")] 
    public string SpecVersion { get; init; } = "1.0";

    [JsonPropertyName("id")] 
    public string Id { get; init; } = Guid.NewGuid().ToString();

    [JsonPropertyName("type")] 
    public string Type { get; init; } = string.Empty;

    [JsonPropertyName("source")] 
    public string Source { get; init; } = string.Empty;

    [JsonPropertyName("subject")]
    public string? Subject { get; init; }

    [JsonPropertyName("time")]
    public DateTime Time { get; init; }

    [JsonPropertyName("datacontenttype")]
    public string DataContentType { get; init; } = "application/json; charset=utf-8";

    [JsonPropertyName("dataschema")] 
    public string? DataSchema { get; init; }

    [JsonPropertyName("data")] 
    public T Data { get; init; } = default!;
}
