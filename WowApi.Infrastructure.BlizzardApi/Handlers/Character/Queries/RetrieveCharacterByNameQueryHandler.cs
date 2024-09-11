using AutoMapper;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using System.Text.Json;
using WowApi.Infrastructure.BlizzardApi.ExternalApiModels.Character;
using WowApi.Infrastructure.BlizzardApi.Services.ExternalApiServices;
using WowApi.Shared.Dtos.Character;
using WowApi.Shared.UseCase.Character.Query;

namespace WowApi.Infrastructure.BlizzardApi.Handlers.Character.Queries;
public class RetrieveCharacterByNameQueryHandler(IBlizzardApiClient blizzardApiClient, IMapper mapper, IMemoryCache memoryCache)
		: IRequestHandler<RetrieveCharacterByNameQuery, RetrieveCharacterByNameQuery.Response>
{
	private readonly IBlizzardApiClient _blizzardApiClient = blizzardApiClient;
	private readonly IMapper _mapper = mapper;
	private readonly IMemoryCache _memoryCache = memoryCache;

	public async Task<RetrieveCharacterByNameQuery.Response> Handle(RetrieveCharacterByNameQuery request, CancellationToken cancellationToken)
	{
		try
		{
			var cacheKey = $"CharacterData-{request.Region}-{request.Realm}-{request.CharacterProfileName}";
			if (!_memoryCache.TryGetValue(cacheKey, out CharacterProfile? _cachedProfile))
			{
				string url = $"/profile/wow/character/{request.Realm.ToLower()}/{request.CharacterProfileName.ToLower()}?namespace=profile-{request.Region}&locale=en_US";
				_cachedProfile = await _blizzardApiClient.FetchDataAsync<CharacterProfile>(url, cancellationToken);

				// Cache the profile with an expiration time
				_memoryCache.Set(cacheKey, _cachedProfile, TimeSpan.FromMinutes(30));
			}

			RetrieveCharacterByNameQuery.Response response = new(_mapper.Map<CharacterProfileDto>(_cachedProfile));
			return response;
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
