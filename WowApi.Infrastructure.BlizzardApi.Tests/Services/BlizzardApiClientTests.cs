using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using System.Net;
using System.Text.Json;
using WowApi.Infrastructure.BlizzardApi.Services.ExternalApiServices;


namespace WowApi.Application.Tests.Queries.Services
{
    public class BlizzardApiClientTests
    {
        private readonly Mock<HttpMessageHandler> _httpMessageHandlerMock;
        private readonly HttpClient _httpClient;
        private readonly BlizzardApiClient _blizzardApiClient;

        public BlizzardApiClientTests()
        {
			_httpMessageHandlerMock = new Mock<HttpMessageHandler>();

			// Initialize HttpClient with mocked handler
			_httpClient = new HttpClient(_httpMessageHandlerMock.Object)
			{
				BaseAddress = new Uri("https://api.blizzard.com")
			};

			// Initialize BlizzardApiClient with mocked HttpClient
			_blizzardApiClient = new BlizzardApiClient(_httpClient);
		}

        [Fact]
        public async Task FetchDataAsync_ShouldReturnData_WhenApiReturnsValidResponse()
        {
            // Arrange
            var endpoint = "/test-endpoint";
            var responseData = new { Name = "Test Character", Level = 60 };
            var jsonResponse = JsonSerializer.Serialize(responseData);

            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(jsonResponse)
                });

			// Act
			var result = await _blizzardApiClient.FetchDataAsync<JsonElement>(endpoint, CancellationToken.None);

			result.Should().NotBeNull();
			var name = result.GetProperty("Name").GetString();
			var level = result.GetProperty("Level").GetInt32();

			// Assert
			
			name.Should().Be("Test Character");
			level.Should().Be(60);
        }

        [Fact]
        public async Task FetchDataAsync_ShouldThrowException_WhenApiReturnsError()
        {
            // Arrange
            var endpoint = "/test-endpoint";

            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.BadRequest
                });

            // Act
            Func<Task> act = async () => await _blizzardApiClient.FetchDataAsync<dynamic>(endpoint, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<HttpRequestException>()
                .WithMessage("Blizzard API request failed with status code BadRequest.");
        }

      
    }
}
