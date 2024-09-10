

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using WowApi.Infrastructure.Configuration;
using WowApi.Infrastructure.ExternalApiModels;


namespace WowApi.Infrastructure.Services;

public class TokenManager : IHostedService
{
	private readonly BlizzardApiClient _blizzardApiClient;
	private readonly ILogger<TokenManager> _logger;
	private readonly BlizzardApiSettings _blizzardApiSettings;
	private Timer? _timer;
	private DateTime _tokenExpiry;
	private TokenResponse? _tokenResponse;

	public TokenManager(
		  BlizzardApiClient blizzardApiClient
		, IOptions<BlizzardApiSettings> blizzardApiSettings
		, ILogger<TokenManager> logger)
	{
		_blizzardApiClient = blizzardApiClient;
		_logger = logger;
		_blizzardApiSettings = blizzardApiSettings.Value;
	}


	public Task StartAsync(CancellationToken cancellationToken)
	{
		_logger.LogInformation("Starting TokenManager...");

		// Initial token refresh
		return RefreshTokenAsync();

	}
	private async Task RefreshTokenAsync()
	{
		try
		{
			_tokenResponse = await _blizzardApiClient.GetAccessTokenAsync(_blizzardApiSettings.ClientId, _blizzardApiSettings.ClientSecret);

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
}
