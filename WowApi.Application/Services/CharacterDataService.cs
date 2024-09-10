
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System.Net.Http;
using System.Net.NetworkInformation;
using WowApi.Application.Dtos;
using WowApi.Application.Handlers;
using WowApi.Infrastructure.BlizzardApi.Configuration;
using WowApi.Infrastructure.BlizzardApi.ExternalApiModels;
using WowApi.Infrastructure.BlizzardApi.ExternalApiModels.Resposne;
using WowApi.Infrastructure.BlizzardApi.Services;

namespace WowApi.Application.Services;
public interface ICharacterDataService
{
    
    Task<FullCharacterInfoDto> GetFullData(GetCharacterByNameQuery request, CancellationToken cancellationToken);
}
public class CharacterDataService(IBlizzardApiClient blizzardApiClient, IOptions<BlizzardApiSettings> blizzardApiSettings, IMemoryCache memoryCache) : ICharacterDataService
{
    private readonly IBlizzardApiClient _blizzardApiClient = blizzardApiClient;
    private readonly IOptions<BlizzardApiSettings> _blizzardApiSettings = blizzardApiSettings;
    private readonly IMemoryCache _memoryCache = memoryCache;


    public async Task<FullCharacterInfoDto> GetFullData(GetCharacterByNameQuery request, CancellationToken cancellationToken)
    {
        var cacheKey = $"CharacterData-{request.Realm}-{request.CharacterProfileName}";
        if (!_memoryCache.TryGetValue(cacheKey, out CharacterProfile? _cachedProfile))
        {
            string url = $"/profile/wow/character/{request.Realm.ToLower()}/{request.CharacterProfileName.ToLower()}?namespace=profile-{_blizzardApiSettings.Value.Region}&locale=en_US";
            _cachedProfile = await _blizzardApiClient.FetchDataAsync<CharacterProfile>(url, cancellationToken);

            // Cache the profile with an expiration time
            _memoryCache.Set(cacheKey, _cachedProfile, TimeSpan.FromMinutes(30));
        }

        if (_cachedProfile != null)
        {
            string content = string.Empty;
            if (!string.IsNullOrEmpty(_cachedProfile.Collections?.Href))
                 content = await _blizzardApiClient.FetchDataAsync(_cachedProfile.Collections.Href,cancellationToken);

            if (!string.IsNullOrEmpty(content))
            {

            }
   
        }
        return new() { CharacterProfile = new()};
    }
}
