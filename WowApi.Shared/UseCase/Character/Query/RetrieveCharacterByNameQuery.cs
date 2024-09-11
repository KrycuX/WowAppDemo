using MediatR;
using WowApi.Shared.Dtos.Character;


namespace WowApi.Shared.UseCase.Character.Query;

public record RetrieveCharacterByNameQuery(string CharacterProfileName, string Realm, string Region) : IRequest<RetrieveCharacterByNameQuery.Response>
{
    public record Response(CharacterProfileDto Character);

}
