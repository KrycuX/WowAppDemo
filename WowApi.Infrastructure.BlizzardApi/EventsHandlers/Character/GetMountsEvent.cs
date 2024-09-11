using MediatR;
using WowApi.Infrastructure.BlizzardApi.Events.Character;
using WowApi.Infrastructure.BlizzardApi.Services;

namespace WowApi.Infrastructure.BlizzardApi.EventsHandlers.Character;

public class GetMountsEvent(ICharacterDataService characterDataService) : INotificationHandler<MountsEvent>
{
	public Task Handle(MountsEvent notification, CancellationToken cancellationToken)
	{
		throw new NotImplementedException();
	}
}
