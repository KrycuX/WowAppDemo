using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Text.Json;
using WowApi.Application.Configuration;
using WowApi.Infrastructure.ExternalApiModels;

namespace WowApi.Application.Services;
public interface IOAuthService
{
    Task<string> GetAccessTokenAsync();
}

public class OAuthService : IOAuthService
{
    private readonly HttpClient _httpClient;
    private readonly BlizzardApiSettings _blizzardApiSettings;

    public OAuthService(HttpClient httpClient, IOptions<BlizzardApiSettings> blizzardApiSettings)
    {
        _httpClient = httpClient;
        _blizzardApiSettings = blizzardApiSettings.Value;
    }

    public async Task<string> GetAccessTokenAsync()
    {
        string toReturn = string.Empty;
        try
        {
            var request = new HttpRequestMessage(HttpMethod.Post, _blizzardApiSettings.TokenEndpoint);
            var byteArray = System.Text.Encoding.ASCII.GetBytes($"{_blizzardApiSettings.ClientId}:{_blizzardApiSettings.ClientSecret}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
            request.Content = new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    { "grant_type", "client_credentials" }
                });

            HttpResponseMessage response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            string responseBody = await response.Content.ReadAsStringAsync();
            var tokenResponse = JsonSerializer.Deserialize<TokenResponse>(responseBody);

            if (tokenResponse == null || string.IsNullOrEmpty(tokenResponse.AccessToken))
            {
                throw new InvalidOperationException("Invalid token response received.");
            }

            return tokenResponse.AccessToken;
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Failed to retrieve access token.", ex);
        }

    }

}
