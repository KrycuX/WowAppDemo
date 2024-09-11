

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Text.Json;
using WowApi.Infrastructure.BlizzardApi.Configuration;
using WowApi.Infrastructure.BlizzardApi.ExternalApiModels;


namespace WowApi.Infrastructure.BlizzardApi.Services.ExternalApiServices;

public class TokenManager : IHostedService
{
    private readonly ILogger<TokenManager> _logger;
    private readonly BlizzardApiSettings _blizzardApiSettings;
    private readonly HttpClient _httpClient;

    private Timer? _timer;
    private DateTime _tokenExpiry;
    private TokenResponse? _tokenResponse;

    public TokenManager(
          HttpClient httpClient
        , IOptions<BlizzardApiSettings> blizzardApiSettings
        , ILogger<TokenManager> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
        _blizzardApiSettings = blizzardApiSettings.Value;
    }


    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting TokenManager...");

        // Initial token refresh
        return RefreshTokenAsync();

    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Stopping TokenManager...");
        _timer?.Change(Timeout.Infinite, 0);
        return Task.CompletedTask;
    }
    public void Dispose()
    {
        _timer?.Dispose();
    }

    public TokenResponse? GetCurrentToken()
    {

        return _tokenResponse;
    }
    private async Task RefreshTokenAsync()
    {
        try
        {
            _tokenResponse = await GetAccessTokenAsync(_blizzardApiSettings.ClientId, _blizzardApiSettings.ClientSecret);

            _tokenExpiry = DateTime.UtcNow.AddSeconds(_tokenResponse.ExpiresIn - 60); // Refresh 1 minute before expiry

            _logger.LogInformation("Token refreshed successfully.");
            ScheduleTokenRefresh(); // Schedule the next refresh
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error refreshing token.");
        }
    }
    private void ScheduleTokenRefresh()
    {
        // Calculate the time to wait before the token expires
        var timeToWait = _tokenExpiry - DateTime.UtcNow;

        if (timeToWait < TimeSpan.Zero)
        {
            // Token has already expired or is about to expire, refresh immediately
            timeToWait = TimeSpan.Zero;
        }

        _timer = new Timer(async _ => await RefreshTokenAsync(), null, timeToWait, Timeout.InfiniteTimeSpan);
    }
    private async Task<TokenResponse> GetAccessTokenAsync(string clientId, string clientSecret)
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
            var _cachedToken = JsonSerializer.Deserialize<TokenResponse>(authContent);

            if (_cachedToken == null)
            {
                throw new HttpRequestException("Authentication failed.");
            }

            return _cachedToken;
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Failed to retrieve access token.", ex);
        }

    }
}
