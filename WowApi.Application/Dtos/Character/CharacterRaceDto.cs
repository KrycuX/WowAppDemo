using System.Text.Json.Serialization;

namespace WowApi.Application.Dtos;

public class CharacterRaceDto
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    
}
