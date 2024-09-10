
using System.Text.Json;

namespace WowApi.Infrastructure.BlizzardApi.Services;
public interface IBlizzardApiClient
{
	Task<T?> FetchDataAsync<T>(string endpoint, CancellationToken cancellationToken);
	Task<string> FetchDataAsync(string endpoint, CancellationToken cancellationToken);
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
    public async Task<string> FetchDataAsync(string endpoint, CancellationToken cancellationToken)
    {
        var response = await _httpClient.GetAsync(endpoint, cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"Blizzard API request failed with status code {response.StatusCode}.");
        }

        return await response.Content.ReadAsStringAsync();
    }

}

	

