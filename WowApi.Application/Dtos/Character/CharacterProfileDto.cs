

namespace WowApi.Application.Dtos;
public class CharacterProfileDto
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public CharacterRaceDto? Race { get; set; }
    public CharacterClassDto? Class { get; set; }
    public int Level { get; set; }
}
