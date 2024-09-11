using MediatR;
using static WowApi.Shared.UseCase.Querry.RetrieveCharacterByNameQuery;

namespace WowApi.Shared.UseCase.Querry;

public record RetrieveCharacterByNameQuery(string CharacterProfileName, string Realm, string Region) : IRequest<Response>
{
	public record Response(CharacterResponse Character);
	public record CharacterResponse( );
}
