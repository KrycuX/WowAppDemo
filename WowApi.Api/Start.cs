using Microsoft.OpenApi.Models;
using WowApi.Infrastructure.BlizzardApi;
using WowApi.Shared;


namespace WowApi.Api;
public class Start
{
	private readonly IConfiguration _configuration;

	public Start(IConfiguration configuration)
	{
		_configuration = configuration;
	}

	public void ConfigureServices(IServiceCollection services)
	{
		services.AddControllers();
		services.AddSwaggerGen(options =>
		{
			options.SwaggerDoc(
				"v1",
				new OpenApiInfo
				{
					Title = "WowApi API",
					Version = "v1"
				});
		});

		services.AddInfrastructure(_configuration);

	}

	public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
	{
		if (env.IsDevelopment())
		{
			app.UseDeveloperExceptionPage();
		}

		app.UseRouting();

		app.UseEndpoints(endpoints => { endpoints.MapControllers(); })
			.UseSwagger()
			.UseSwaggerUI(c =>
			{
				c.SwaggerEndpoint("/swagger/v1/swagger.json", "WowApi API v1");
				c.RoutePrefix = string.Empty;
			});
	}
}
