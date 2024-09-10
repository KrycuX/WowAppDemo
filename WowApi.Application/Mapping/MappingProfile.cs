using AutoMapper;
using WowApi.Application.Dtos;
using WowApi.Infrastructure.BlizzardApi.ExternalApiModels;

namespace WowApi.Application.Mapping;

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
