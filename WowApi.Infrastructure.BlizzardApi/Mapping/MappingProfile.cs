using AutoMapper;
using WowApi.Infrastructure.BlizzardApi.Dtos.Character;
using WowApi.Infrastructure.BlizzardApi.ExternalApiModels.Character;

namespace WowApi.Infrastructure.BlizzardApi.Mapping;

public class MappingProfile : Profile
{
	public MappingProfile()
	{
		CreateMap<CharacterClassDto, CharacterClass>();
		CreateMap<CharacterProfileDto, CharacterProfile>();
		CreateMap<CharacterRaceDto, CharacterRace>();

		CreateMap<CharacterClass, CharacterClassDto>();
		CreateMap<CharacterProfile, CharacterProfileDto>();
		CreateMap<CharacterRace, CharacterRaceDto>();
	}
}
