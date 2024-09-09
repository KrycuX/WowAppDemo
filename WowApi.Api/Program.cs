using Microsoft.AspNetCore;
using WowApi.Api;

public class Program
{
    public static void Main(string[] args) =>
        CreateWebHostBuilder(args).Build().Run();

    public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
        WebHost
            .CreateDefaultBuilder(args)
            .UseStartup<Start>();
}
