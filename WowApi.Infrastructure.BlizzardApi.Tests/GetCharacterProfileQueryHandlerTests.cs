using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;
using WowApi.Infrastructure.BlizzardApi.Configuration;
using WowApi.Infrastructure.BlizzardApi.Dtos.Character;
using WowApi.Infrastructure.BlizzardApi.ExternalApiModels.Character;
using WowApi.Infrastructure.BlizzardApi.Handlers.Character.Queries;
using WowApi.Infrastructure.BlizzardApi.Mapping;
using WowApi.Infrastructure.BlizzardApi.Services;
using WowApi.Infrastructure.BlizzardApi.Services.ExternalApiServices;
using WowApi.Shared.UseCase.Querry;

namespace WowApi.Application.Tests.Queries;

public class GetCharacterProfileQueryHandlerTests
{
	private readonly Mock<IBlizzardApiClient> _blizzardApiClientMock;
	private readonly Mock<IOptions<BlizzardApiSettings>> _blizzardApiSettingsMock;
	private readonly Mock<ICharacterDataService> _characterDataService;
	private readonly IMapper _mapper;
	private readonly RetrieveCharacterByNameQueryHandler _handler;

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
		});
		_characterDataService = new Mock<ICharacterDataService>(_blizzardApiClientMock.Object,
			_blizzardApiSettingsMock.Object);

		// Init handler
		_handler = new RetrieveCharacterByNameQueryHandler(
			_characterDataService.Object,
			_mapper	
			);
	}

	[Fact]
	public async Task Handle_ShouldReturnCharacterProfileDto_WhenApiReturnsValidData()
	{
		// Arrange
		var characterProfileName = "krycuu";
		var realm = "silvermoon";
		var region = "eu";
		var query = new RetrieveCharacterByNameQuery(characterProfileName, realm, region);

		var characterData = new CharacterProfile
		{
			Name = "Test Character",
			Race = new CharacterRace { Id = 1 },
			CharacterClass = new CharacterClass { Id = 2 },
			Level = 60
		};
		var full = new FullCharacterInfoDto() { CharacterProfile = new() };
		_blizzardApiClientMock
			.Setup(client => client.FetchDataAsync<CharacterProfile>(It.IsAny<string>(), It.IsAny<CancellationToken>()))
			.ReturnsAsync(characterData);

		_characterDataService
			.Setup(service => service.GetFullData(It.IsAny<RetrieveCharacterByNameQuery>(), It.IsAny<CancellationToken>()))
			.ReturnsAsync(full);

		// Act
		var result = await _handler.Handle(query, CancellationToken.None);

		// Assert
		//result.Should().NotBeNull();
		//result.Name.Should().Be("Test Character");
		//result.Race?.Id.Should().Be(1);
		//result.Class?.Id.Should().Be(2);
		//result.Level.Should().Be(60);
	}

	[Fact]
	public async Task Handle_ShouldThrowException_WhenApiReturnsNull()
	{
		// Arrange
		var characterProfileName = "test-character";
		var realm = "test-realm";
		var region = "eu";
		var query = new RetrieveCharacterByNameQuery(characterProfileName, realm, region);

		_blizzardApiClientMock
			.Setup(client => client.FetchDataAsync<CharacterProfile>(It.IsAny<string>(), It.IsAny<CancellationToken>()))
			.ReturnsAsync((CharacterProfile)null);

		// Act
		Func<Task> act = async () => await _handler.Handle(query, CancellationToken.None);

		// Assert
		await act.Should().ThrowAsync<ApplicationException>().WithMessage("Character not found.");
	}
}
