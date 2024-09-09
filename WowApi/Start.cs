using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using WowApi.Application.Configuration;
using WowApi.Application.CQRS.Queries;
using WowApi.Application.Mapping;
using WowApi.Application.Services;

namespace WowApi;
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
        services.AddAutoMapper(typeof(MappingProfile).Assembly);
        ConfigureMediatR(services);

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

        services.Configure<BlizzardApiSettings>(_configuration.GetSection("BlizzardApi"));
        services.AddHttpClient("BlizzardApiClient", (servicePrvider, client) =>
        {
            var settings = servicePrvider.GetRequiredService<IOptions<BlizzardApiSettings>>().Value;
            client.BaseAddress = new Uri(settings.Endpoint);
        });

        services.AddSingleton<IOAuthService, OAuthService>();

    }

    private static void ConfigureMediatR(IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(

            typeof(Program).Assembly,
            typeof(GetCharacterByNameQuery).Assembly

            ));
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
