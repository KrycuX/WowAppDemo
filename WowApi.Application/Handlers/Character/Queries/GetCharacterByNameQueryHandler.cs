using AutoMapper;
using MediatR;
using Microsoft.Extensions.Options;
using System.Text.Json;
using WowApi.Application.Dtos;
using WowApi.Application.Services;
using WowApi.Infrastructure.BlizzardApi.Configuration;
using WowApi.Infrastructure.BlizzardApi.ExternalApiModels;
using WowApi.Infrastructure.BlizzardApi.Services;

namespace WowApi.Application.Handlers;
public class GetCharacterByNameQueryHandler : IRequestHandler<GetCharacterByNameQuery, CharacterProfileDto>
{
	private readonly IBlizzardApiClient _blizzardApiClient;
	private readonly IOptions<BlizzardApiSettings> _blizzardApiSettings;
	private readonly IMapper _mapper;
    private readonly ICharacterDataService _characterDataService;

    public GetCharacterByNameQueryHandler
		(
		IBlizzardApiClient blizzardApiClient,
		IOptions<BlizzardApiSettings> options,
		IMapper mapper,
		ICharacterDataService characterDataService
		)
	{
		_blizzardApiClient = blizzardApiClient;
		_blizzardApiSettings = options;
		_mapper = mapper;
        _characterDataService = characterDataService;
    }

	public async Task<CharacterProfileDto> Handle(GetCharacterByNameQuery request, CancellationToken cancellationToken)
	{
		try
		{
			var data =	await _characterDataService.GetFullData(request,cancellationToken);

            string url = $"/profile/wow/character/{request.Realm.ToLower()}/{request.CharacterProfileName.ToLower()}?namespace=profile-{_blizzardApiSettings.Value.Region}&locale=en_US";
			var response = await _blizzardApiClient.FetchDataAsync<CharacterProfile>(url, cancellationToken);
			if (response == null)
				throw new ApplicationException("Character not found.");

			var dto = _mapper.Map<CharacterProfileDto>(response);

			return dto;
		}
		catch (HttpRequestException ex)
		{
			throw new ApplicationException("Error occurred while fetching character profile.", ex);
		}
		catch (JsonException ex)
		{
			throw new ApplicationException("Error occurred while deserializing character profile.", ex);
		}
		catch (Exception ex)
		{
			throw new ApplicationException("Character not found.", ex);
		}
	}
}
