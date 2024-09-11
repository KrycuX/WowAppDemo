using MediatR;
using WowApi.Shared.Dtos.Mount;


namespace WowApi.Shared.UseCase.Mounts.Query;

public record RetrieveMountsByCharacterQuery(string CharacterProfileName, string Realm, string Region)  : IRequest<RetrieveMountsByCharacterQuery.Response>
{
	public record Response(List<MountDto> Mounts);
	
}
