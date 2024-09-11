using MediatR;

namespace WowApi.Infrastructure.BlizzardApi.Events.Character;

public record MountsEvent(string url): INotification;

