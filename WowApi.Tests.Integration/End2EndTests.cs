using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using WowApi.Api;

namespace WowApi.Tests.Integration
{
    public class End2EndTests : IClassFixture<WebApplicationFactory<Start>>
    {
        private readonly WebApplicationFactory<Start> _factory;

        public End2EndTests(WebApplicationFactory<Start> factory)
        {
            _factory = factory;
        }
        [Fact]
        public async Task CanReachWowApi()
        {
            // Arrange
            var client = _factory.CreateClient();
            var characterName = "krycuu";
            var realm = "silvermoon";
            var url = $"api/Characters/character?characterName={characterName}&realm={realm}";

            // Act
            var response = await client.GetAsync(url);

            // Assert
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Request failed with status code {response.StatusCode} and message: {errorContent}");
                Assert.Fail($"Request failed with status code {response.StatusCode} and message: {errorContent}");
            }

            var content = await response.Content.ReadAsStringAsync();
            content.Should().NotBeNullOrEmpty();
        }
    }
}