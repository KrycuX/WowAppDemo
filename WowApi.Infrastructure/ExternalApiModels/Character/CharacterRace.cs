using System.Text.Json.Serialization;

namespace WowApi.Infrastructure.ExternalApiModels;

public class CharacterRace
{
    [JsonPropertyName("id")] public int Id { get; init; }
    [JsonPropertyName("name")]  public string Name { get; init; } = string.Empty;
    
}
