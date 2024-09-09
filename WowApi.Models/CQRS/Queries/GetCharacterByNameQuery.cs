using MediatR;
using WowApi.Application.Dtos;

namespace WowApi.Application.CQRS.Queries;

public class GetCharacterByNameQuery : IRequest<CharacterProfileDto>
{
    public string CharacterProfileName { get; }
    public string Realm { get; }

    public GetCharacterByNameQuery(string characterProfileName, string realm)
    {
        CharacterProfileName = characterProfileName;
        Realm = realm;

    }
}
