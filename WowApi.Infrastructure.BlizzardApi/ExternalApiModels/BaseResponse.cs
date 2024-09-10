using System.Text.Json.Serialization;

namespace WowApi.Infrastructure.BlizzardApi.ExternalApiModels;
public interface IBaseResponse
{
    [JsonPropertyName("href")]
    string? Href { get; set; }
}
public class BaseResponse : IBaseResponse
{
    [JsonPropertyName("href")]
    public string? Href { get; set; }
}
