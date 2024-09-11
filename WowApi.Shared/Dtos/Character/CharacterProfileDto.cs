namespace WowApi.Shared.Dtos.Character;
public class CharacterProfileDto
{
	public int Id { get; set; }
	public string Name { get; set; } = default!;
	public CharacterRaceDto? Race { get; set; }
	public CharacterClassDto? Class { get; set; }
	public int Level { get; set; }
}
public class CharacterClassDto
{
	public int Id { get; init; }
	public string Name { get; init; } = string.Empty;
}
public class CharacterRaceDto
{
	public int Id { get; init; }
	public string Name { get; init; } = string.Empty;

}
