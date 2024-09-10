using System.Net.Http.Headers;

namespace WowApi.Infrastructure.BlizzardApi.Services;

public class BlizzardApiAuthHandler : DelegatingHandler
{
	private readonly TokenManager _tokenManager;

	public BlizzardApiAuthHandler(TokenManager tokenManager)
	{
		_tokenManager = tokenManager;
	}
	protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
	{
		var token = _tokenManager.GetCurrentToken();
		if (token == null)
			throw new Exception("Authentication error");

		request.Headers.Authorization = new AuthenticationHeaderValue(token.TokenType, token.AccessToken);
		return await base.SendAsync(request, cancellationToken);
	}
}
