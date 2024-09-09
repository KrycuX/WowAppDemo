using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Moq;

using WowApi.Services;

namespace WowApi.Tests.Unit
{
    public class StartTest : IClassFixture<WebApplicationFactory<Start>>
    {
        private readonly WebApplicationFactory<Start> _factory;

        public StartTest(WebApplicationFactory<Start> factory)
        {
            _factory = factory;
        }
        [Fact]
        public void ShouldResolveOAuthService()
        {
            _factory.CreateClient(); 
            using (var scope = _factory.Server.Host.Services.CreateScope())
            {
                var service = scope.ServiceProvider.GetService<IOAuthService>();
                service.Should().NotBeNull();
            }
        }
        [Fact]
        public void ShouldResolveWowApiService()
        {
            _factory.CreateClient(); 
            using (var scope = _factory.Server.Host.Services.CreateScope())
            {
                var service = scope.ServiceProvider.GetService<IWowApiService>();
                service.Should().NotBeNull();
            }
        }
        [Fact]
        public void ShouldResolveHttpClient()
        {
            _factory.CreateClient(); 
            var mockFactory = new Mock<IHttpClientFactory>();
            var client = new HttpClient();
            mockFactory.Setup(_ => _.CreateClient("BlizzardApiClient")).Returns(client);

            client.Should().NotBeNull();
        }
    }
}