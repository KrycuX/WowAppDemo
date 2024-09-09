// W YourApp.Application.Tests/Queries/GetCharacterProfileQueryHandlerTests.cs
using AutoMapper;
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using WowApi.Application.Configuration;
using WowApi.Application.CQRS.Queries;
using WowApi.Application.Mapping;
using WowApi.Application.Services;
using Xunit;


namespace YourApp.Application.Tests.Queries
{
    public class GetCharacterProfileQueryHandlerTests
    {
        private readonly Mock<IHttpClientFactory> _httpClientFactoryMock;
        private readonly Mock<IOAuthService> _oAuthServiceMock;
        private readonly Mock<IOptions<BlizzardApiSettings>> _blizzardApiSettingsMock;
        private readonly IMapper _mapper;
        private readonly GetCharacterByNameQueryHandler _handler;

        public GetCharacterProfileQueryHandlerTests()
        {
            // Ustawienie mappera
            var mapperConfig = new MapperConfiguration(cfg => cfg.AddProfile(new MappingProfile()));
            _mapper = mapperConfig.CreateMapper();

            // Inicjalizacja mocków
            _httpClientFactoryMock = new Mock<IHttpClientFactory>();
            _oAuthServiceMock = new Mock<IOAuthService>();
            _blizzardApiSettingsMock = new Mock<IOptions<BlizzardApiSettings>>();

            _blizzardApiSettingsMock.Setup(opt => opt.Value).Returns(new BlizzardApiSettings
            {
                Endpoint = "https://api.blizzard.com",
                Region = "us"
            });

            // Inicjalizacja handlera
            _handler = new GetCharacterByNameQueryHandler(
                _httpClientFactoryMock.Object,
                _oAuthServiceMock.Object,
                _blizzardApiSettingsMock.Object,
                _mapper);
        }

        [Fact]
        public async Task Handle_ShouldReturnCharacterProfileDto_WhenApiReturnsValidResponse()
        {
            // Arrange
            var characterProfileName = "krycuu";
            var realm = "silvermoon";
            var query = new GetCharacterByNameQuery(characterProfileName, realm);

            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Content = new StringContent("{\"name\":\"Test Character\", \"race\":{\"id\":1}, \"character_class\":{\"id\":2}, \"level\":60}")
                });

            var httpClient = new HttpClient(mockHttpMessageHandler.Object)
            {
                BaseAddress = new(_blizzardApiSettingsMock.Object.Value.Endpoint)
            };
            _httpClientFactoryMock.Setup(x => x.CreateClient("BlizzardApiClient")).Returns(httpClient);

            _oAuthServiceMock.Setup(x => x.GetAccessTokenAsync()).ReturnsAsync("fake-token");

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Name.Should().Be("Test Character");
            result.Race?.Id.Should().Be(1);
            result.Class?.Id.Should().Be(2);
            result.Level.Should().Be(60);
        }

        [Fact]
        public async Task Handle_ShouldThrowException_WhenApiReturnsError()
        {
            // Arrange
            var characterProfileName = "test-character";
            var realm = "test-realm";
            var query = new GetCharacterByNameQuery(characterProfileName, realm);

            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                });

            var httpClient = new HttpClient(mockHttpMessageHandler.Object);
            _httpClientFactoryMock.Setup(x => x.CreateClient("BlizzardApiClient")).Returns(httpClient);

            _oAuthServiceMock.Setup(x => x.GetAccessTokenAsync()).ReturnsAsync("fake-token");

            // Act
            Func<Task> act = async () => await _handler.Handle(query, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<ApplicationException>();
        }
    }
}
