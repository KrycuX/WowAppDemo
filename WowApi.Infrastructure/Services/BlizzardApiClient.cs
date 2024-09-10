using System.Net.Http.Headers;
using System.Text.Json;
using WowApi.Infrastructure.ExternalApiModels;

namespace WowApi.Infrastructure.Services;
public interface IBlizzardApiClient
{
	Task<T?> FetchDataAsync<T>(string endpoint, CancellationToken cancellationToken);

}
public class BlizzardApiClient : IBlizzardApiClient
{
	private readonly HttpClient _httpClient;
	public BlizzardApiClient(HttpClient httpClient) => _httpClient = httpClient;

	public async Task<T?> FetchDataAsync<T>(string endpoint,CancellationToken cancellationToken)
	{
		var response = await _httpClient.GetAsync(endpoint, cancellationToken);

		if (!response.IsSuccessStatusCode)
		{
			throw new HttpRequestException($"Blizzard API request failed with status code {response.StatusCode}.");
		}

		var content = await response.Content.ReadAsStringAsync();
		return JsonSerializer.Deserialize<T>(content);
	}
	

}

	

