using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Reflection;
using WowApi.Infrastructure.BlizzardApi.Configuration;
using WowApi.Infrastructure.BlizzardApi.Services;
using WowApi.Infrastructure.BlizzardApi.Services.ExternalApiServices;

namespace WowApi.Infrastructure.BlizzardApi;

public static class Bootstraper
{
	public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
	{
		services.Configure<BlizzardApiSettings>(configuration.GetSection("BlizzardApi"));
		services.AddAutoMapper(Assembly.GetExecutingAssembly());
		services.AddMediatR(c => c.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
		services.AddHttpClient<IBlizzardApiClient, BlizzardApiClient>((serviceProvider, client) =>
		{
			var settings = serviceProvider.GetRequiredService<IOptions<BlizzardApiSettings>>().Value;
			client.BaseAddress = new Uri(settings.Endpoint);
			client.DefaultRequestHeaders.Add("Accept", "application/json");

		}).AddHttpMessageHandler<BlizzardApiAuthHandler>();

		services.AddMemoryCache();
		services.AddTransient<TokenManager>();
		services.AddTransient<BlizzardApiAuthHandler>();
		services.AddTransient<ICharacterDataService, CharacterDataService>();
		services.AddHostedService(provider => provider.GetRequiredService<TokenManager>());
		return services;
	}

}
