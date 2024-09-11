using MediatR;
using WowApi.Shared.UseCase.Mounts.Query;


namespace WowApi.Infrastructure.BlizzardApi.Handlers.Mounts.Query;

public record RetrieveMountsByCharacterQueryHandler: IRequestHandler<RetrieveMountsByCharacterQuery, RetrieveMountsByCharacterQuery.Response>
{
	public Task<RetrieveMountsByCharacterQuery.Response> Handle(RetrieveMountsByCharacterQuery request, CancellationToken cancellationToken)
	{
		throw new NotImplementedException();
	}
}
