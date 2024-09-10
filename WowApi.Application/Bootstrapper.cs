using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Reflection;
using WowApi.Infrastructure.Configuration;
using WowApi.Infrastructure.Services;

namespace WowApi.Application;

public static class Bootstrapper
{
	public static IServiceCollection AddConfiguration(this IServiceCollection services, IConfiguration configuration)
	{
		services.Configure<BlizzardApiSettings>(configuration.GetSection("BlizzardApi"));
		return services;
	}
	public static IServiceCollection AddWowApiApplication(this IServiceCollection services)
	{
		services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));
		services.AddAutoMapper(Assembly.GetExecutingAssembly());

		services.AddHttpClient<BlizzardApiClient>((serviceProvider, client) =>
		{
			var settings = serviceProvider.GetRequiredService<IOptions<BlizzardApiSettings>>().Value;
			client.BaseAddress = new Uri(settings.Endpoint);
			client.DefaultRequestHeaders.Add("Accept", "application/json");
		}).AddHttpMessageHandler<BlizzardApiAuthHandler>();


		services.AddSingleton<TokenManager>();
		services.AddSingleton<BlizzardApiAuthHandler>();
		services.AddHostedService(provider => provider.GetRequiredService<TokenManager>());

		return services;
	}

}
