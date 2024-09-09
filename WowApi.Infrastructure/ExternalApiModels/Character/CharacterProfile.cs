using System.Text.Json.Serialization;

namespace WowApi.Infrastructure.ExternalApiModels;
public class CharacterProfile
{
    [JsonPropertyName("id")] public int Id { get; init; }
    [JsonPropertyName("name")] public string Name { get; init; } = default!;
    [JsonPropertyName("race")] public CharacterRace? Race { get; init; }
    [JsonPropertyName("character_class")] public CharacterClass? Class { get; init; }
    [JsonPropertyName("level")] public int Level { get; init; }
}
