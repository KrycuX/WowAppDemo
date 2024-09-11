using System.Text.Json.Serialization;
#nullable disable
namespace WowApi.Infrastructure.BlizzardApi.ExternalApiModels.Character;
// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
public class Achievements : IBaseResponse
{
	[JsonPropertyName("href")]
	public string Href { get; set; }
}

public class AchievementsStatistics : IBaseResponse
{
	[JsonPropertyName("href")]
	public string Href { get; set; }
}

public class ActiveSpec
{
	[JsonPropertyName("key")]
	public Key Key { get; set; }

	[JsonPropertyName("name")]
	public string Name { get; set; }

	[JsonPropertyName("id")]
	public int? Id { get; set; }
}

public class ActiveTitle
{
	[JsonPropertyName("key")]
	public Key Key { get; set; }

	[JsonPropertyName("name")]
	public string Name { get; set; }

	[JsonPropertyName("id")]
	public int? Id { get; set; }

	[JsonPropertyName("display_string")]
	public string DisplayString { get; set; }
}

public class Appearance : IBaseResponse
{
	[JsonPropertyName("href")]
	public string Href { get; set; }
}

public class CharacterClass
{
	[JsonPropertyName("key")]
	public Key Key { get; set; }

	[JsonPropertyName("name")]
	public string Name { get; set; }

	[JsonPropertyName("id")]
	public int? Id { get; set; }
}

public class Collections : IBaseResponse
{
	[JsonPropertyName("href")]
	public string Href { get; set; }
}

public class Encounters : IBaseResponse
{
	[JsonPropertyName("href")]
	public string Href { get; set; }
}

public class Equipment : IBaseResponse
{
	[JsonPropertyName("href")]
	public string Href { get; set; }
}

public class Faction
{
	[JsonPropertyName("type")]
	public string Type { get; set; }

	[JsonPropertyName("name")]
	public string Name { get; set; }
}

public class Gender
{
	[JsonPropertyName("type")]
	public string Type { get; set; }

	[JsonPropertyName("name")]
	public string Name { get; set; }
}

public class Guild
{
	[JsonPropertyName("key")]
	public Key Key { get; set; }

	[JsonPropertyName("name")]
	public string Name { get; set; }

	[JsonPropertyName("id")]
	public int? Id { get; set; }

	[JsonPropertyName("realm")]
	public Realm Realm { get; set; }

	[JsonPropertyName("faction")]
	public Faction Faction { get; set; }
}

public class Key : IBaseResponse
{
	[JsonPropertyName("href")]
	public string Href { get; set; }
}

public class Media : IBaseResponse
{
	[JsonPropertyName("href")]
	public string Href { get; set; }
}

public class MythicKeystoneProfile : IBaseResponse
{
	[JsonPropertyName("href")]
	public string Href { get; set; }
}

public class Professions : IBaseResponse
{
	[JsonPropertyName("href")]
	public string Href { get; set; }
}

public class PvpSummary : IBaseResponse
{
	[JsonPropertyName("href")]
	public string Href { get; set; }
}

public class Quests : IBaseResponse
{
	[JsonPropertyName("href")]
	public string Href { get; set; }
}

public class CharacterRace
{
	[JsonPropertyName("key")]
	public Key Key { get; set; }

	[JsonPropertyName("name")]
	public string Name { get; set; }

	[JsonPropertyName("id")]
	public int? Id { get; set; }
}

public class Realm
{
	[JsonPropertyName("key")]
	public Key Key { get; set; }

	[JsonPropertyName("name")]
	public string Name { get; set; }

	[JsonPropertyName("id")]
	public int? Id { get; set; }

	[JsonPropertyName("slug")]
	public string Slug { get; set; }
}

public class Reputations
{
	[JsonPropertyName("href")]
	public string Href { get; set; }
}

public class CharacterProfile
{

	[JsonPropertyName("id")]
	public int? Id { get; set; }

	[JsonPropertyName("name")]
	public string Name { get; set; }

	[JsonPropertyName("gender")]
	public Gender Gender { get; set; }

	[JsonPropertyName("faction")]
	public Faction Faction { get; set; }

	[JsonPropertyName("race")]
	public CharacterRace Race { get; set; }

	[JsonPropertyName("character_class")]
	public CharacterClass CharacterClass { get; set; }

	[JsonPropertyName("active_spec")]
	public ActiveSpec ActiveSpec { get; set; }

	[JsonPropertyName("realm")]
	public Realm Realm { get; set; }

	[JsonPropertyName("guild")]
	public Guild Guild { get; set; }

	[JsonPropertyName("level")]
	public int? Level { get; set; }

	[JsonPropertyName("experience")]
	public int? Experience { get; set; }

	[JsonPropertyName("achievement_points")]
	public int? AchievementPoints { get; set; }

	[JsonPropertyName("achievements")]
	public Achievements Achievements { get; set; }

	[JsonPropertyName("titles")]
	public Titles Titles { get; set; }

	[JsonPropertyName("pvp_summary")]
	public PvpSummary PvpSummary { get; set; }

	[JsonPropertyName("encounters")]
	public Encounters Encounters { get; set; }

	[JsonPropertyName("media")]
	public Media Media { get; set; }

	[JsonPropertyName("last_login_timestamp")]
	public long? LastLoginTimestamp { get; set; }

	[JsonPropertyName("average_item_level")]
	public int? AverageItemLevel { get; set; }

	[JsonPropertyName("equipped_item_level")]
	public int? EquippedItemLevel { get; set; }

	[JsonPropertyName("specializations")]
	public Specializations Specializations { get; set; }

	[JsonPropertyName("statistics")]
	public Statistics Statistics { get; set; }

	[JsonPropertyName("mythic_keystone_profile")]
	public MythicKeystoneProfile MythicKeystoneProfile { get; set; }

	[JsonPropertyName("equipment")]
	public Equipment Equipment { get; set; }

	[JsonPropertyName("appearance")]
	public Appearance Appearance { get; set; }

	[JsonPropertyName("collections")]
	public Collections Collections { get; set; }

	[JsonPropertyName("active_title")]
	public ActiveTitle ActiveTitle { get; set; }

	[JsonPropertyName("reputations")]
	public Reputations Reputations { get; set; }

	[JsonPropertyName("quests")]
	public Quests Quests { get; set; }

	[JsonPropertyName("achievements_statistics")]
	public AchievementsStatistics AchievementsStatistics { get; set; }

	[JsonPropertyName("professions")]
	public Professions Professions { get; set; }

	[JsonPropertyName("name_search")]
	public string NameSearch { get; set; }
}



public class Specializations : IBaseResponse
{
	[JsonPropertyName("href")]
	public string Href { get; set; }
}

public class Statistics : IBaseResponse
{
	[JsonPropertyName("href")]
	public string Href { get; set; }
}

public class Titles : IBaseResponse
{
	[JsonPropertyName("href")]
	public string Href { get; set; }
}


