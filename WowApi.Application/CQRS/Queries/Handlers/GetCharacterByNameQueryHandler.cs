using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WowApi.Application.Configuration;
using WowApi.Application.Dtos;
using WowApi.Infrastructure.ExternalApiModels;
using System.Text.Json;
using WowApi.Application.Services;
using Microsoft.Extensions.Options;
using AutoMapper;

namespace WowApi.Application.CQRS.Queries;
public class GetCharacterByNameQueryHandler : IRequestHandler<GetCharacterByNameQuery,CharacterProfileDto>
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IOAuthService _oAuthService;
    private readonly IOptions<BlizzardApiSettings> _blizzardApiSettings;
    private readonly IMapper _mapper;

    public GetCharacterByNameQueryHandler
        (
        IHttpClientFactory httpClientFactory,
        IOAuthService oAuthService,
        IOptions<BlizzardApiSettings> options,
        IMapper mapper
        )
    { 
        _httpClientFactory = httpClientFactory;
        _oAuthService = oAuthService;
        _blizzardApiSettings = options;
        _mapper = mapper;
    }

    public async Task<CharacterProfileDto> Handle(GetCharacterByNameQuery request, CancellationToken cancellationToken)
    {
        try
        {
            string token = await _oAuthService.GetAccessTokenAsync();
            var client = _httpClientFactory.CreateClient("BlizzardApiClient");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            string url = $"/profile/wow/character/{request.Realm.ToLower()}/{request.CharacterProfileName.ToLower()}?namespace=profile-{_blizzardApiSettings.Value.Region}&locale=en_US";
            var response = await client.GetAsync(url, cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Request to Blizzard API failed with status code {response.StatusCode}.");
            }

            string jsonString = await response.Content.ReadAsStringAsync(cancellationToken);
            if (string.IsNullOrEmpty(jsonString))
            {
                throw new InvalidOperationException("Received empty response from Blizzard API.");
            }

            var character = JsonSerializer.Deserialize<CharacterProfile>(jsonString);
            if (character == null)
            {
                throw new InvalidOperationException("Failed to deserialize character profile.");
            }
            var dto = _mapper.Map<CharacterProfileDto>(character);
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
            throw new ApplicationException("An unexpected error occurred.", ex);
        }
    }
}
