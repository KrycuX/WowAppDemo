// W YourApp.Application.Tests/Queries/GetCharacterProfileQueryHandlerTests.cs
using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;
using WowApi.Application.Handlers;
using WowApi.Application.Mapping;
using WowApi.Infrastructure.Configuration;
using WowApi.Infrastructure.ExternalApiModels;
using WowApi.Infrastructure.Services;


namespace WowApi.Application.Tests.Queries;

public class GetCharacterProfileQueryHandlerTests
{
	private readonly Mock<IBlizzardApiClient> _blizzardApiClientMock;
	private readonly Mock<IOptions<BlizzardApiSettings>> _blizzardApiSettingsMock;
	private readonly IMapper _mapper;
	private readonly GetCharacterByNameQueryHandler _handler;

	public GetCharacterProfileQueryHandlerTests()
	{
		// Init mapper
		var mapperConfig = new MapperConfiguration(cfg => cfg.AddProfile(new MappingProfile()));
		_mapper = mapperConfig.CreateMapper();

		// Init mockq
		_blizzardApiClientMock = new Mock<IBlizzardApiClient>();
		_blizzardApiSettingsMock = new Mock<IOptions<BlizzardApiSettings>>();
		_blizzardApiSettingsMock.Setup(opt => opt.Value).Returns(new BlizzardApiSettings
		{
			Endpoint = "https://api.blizzard.com",
			Region = "us"
		});

		// Init handler
		_handler = new GetCharacterByNameQueryHandler(
			_blizzardApiClientMock.Object,
			_blizzardApiSettingsMock.Object,
			_mapper);
	}

	[Fact]
	public async Task Handle_ShouldReturnCharacterProfileDto_WhenApiReturnsValidData()
	{
		// Arrange
		var characterProfileName = "krycuu";
		var realm = "silvermoon";
		var query = new GetCharacterByNameQuery(characterProfileName, realm);

		var characterData = new CharacterProfile
		{
			Name = "Test Character",
			Race = new CharacterRace { Id = 1 },
			Class = new CharacterClass { Id = 2 },
			Level = 60
		};

		_blizzardApiClientMock
			.Setup(client => client.FetchDataAsync<CharacterProfile>(It.IsAny<string>(), It.IsAny<CancellationToken>()))
			.ReturnsAsync(characterData);

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
	public async Task Handle_ShouldThrowException_WhenApiReturnsNull()
	{
		// Arrange
		var characterProfileName = "test-character";
		var realm = "test-realm";
		var query = new GetCharacterByNameQuery(characterProfileName, realm);

		_blizzardApiClientMock
			.Setup(client => client.FetchDataAsync<CharacterProfile>(It.IsAny<string>(), It.IsAny<CancellationToken>()))
			.ReturnsAsync((CharacterProfile)null);

		// Act
		Func<Task> act = async () => await _handler.Handle(query, CancellationToken.None);

		// Assert
		await act.Should().ThrowAsync<ApplicationException>().WithMessage("Character not found.");
	}
}
