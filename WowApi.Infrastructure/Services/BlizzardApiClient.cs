using System.Net.Http.Headers;
using System.Text.Json;
using WowApi.Infrastructure.ExternalApiModels;

namespace WowApi.Infrastructure.Services;
public interface IBlizzardApiClient
{
	Task<T?> FetchDataAsync<T>(string endpoint, CancellationToken cancellationToken);
	Task<TokenResponse> GetAccessTokenAsync(string clientId, string clientSecret);
}
public class BlizzardApiClient : IBlizzardApiClient
{
	private readonly HttpClient _httpClient;
	private TokenResponse? _cachedToken;

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
	public async Task<TokenResponse> GetAccessTokenAsync(string clientId, string clientSecret)
	{
		string toReturn = string.Empty;
		try
		{
			var authResponse = await _httpClient.PostAsync("https://oauth.battle.net/token",
			new FormUrlEncodedContent(new[]
			{
					new KeyValuePair<string, string>("grant_type", "client_credentials"),
					new KeyValuePair<string, string>("client_id", clientId),
					new KeyValuePair<string, string>("client_secret", clientSecret),
			 }));

			if (!authResponse.IsSuccessStatusCode)
			{
				throw new HttpRequestException("Authentication failed.");
			}

			string authContent = await authResponse.Content.ReadAsStringAsync();
			_cachedToken = JsonSerializer.Deserialize<TokenResponse>(authContent);
			if( _cachedToken == null )
			{
				throw new HttpRequestException("Authentication failed.");
			}

			_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _cachedToken.AccessToken);

			return _cachedToken;
		}
		catch (Exception ex)
		{
			throw new ApplicationException("Failed to retrieve access token.", ex);
		}

	}

}
