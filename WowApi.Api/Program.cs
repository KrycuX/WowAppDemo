using Microsoft.AspNetCore;
using System.Reflection;
using WowApi.Api;

public class Program
{
    public static void Main(string[] args) =>
        CreateWebHostBuilder(args).Build().Run();

    public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
        WebHost
            .CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((ctx,builder) => { builder.AddUserSecrets(Assembly.GetExecutingAssembly(), true); })
            .UseStartup<Start>();
        
}
