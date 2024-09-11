using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace WowApi.Shared;

public static class Bootstrapper
{

	public static IServiceCollection AddSharedLayer(this IServiceCollection services)
	{
		services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));

		return services;
	}

}
