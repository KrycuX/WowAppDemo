using AutoMapper;
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Moq;
using WowApi.Infrastructure.BlizzardApi.Configuration;
using WowApi.Infrastructure.BlizzardApi.ExternalApiModels.Character;
using WowApi.Infrastructure.BlizzardApi.Handlers.Character.Queries;
using WowApi.Infrastructure.BlizzardApi.Mapping;
using WowApi.Infrastructure.BlizzardApi.Services.ExternalApiServices;
using WowApi.Shared.Dtos.Character;
using WowApi.Shared.UseCase.Character.Query;

namespace WowApi.Application.Tests.Queries;

public class GetCharacterProfileQueryHandlerTests
{
	private readonly Mock<IBlizzardApiClient> _blizzardApiClientMock;
	private readonly Mock<IOptions<BlizzardApiSettings>> _blizzardApiSettingsMock;
	private readonly Mock<IMemoryCache> _memoryCacheMock;

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

		_memoryCacheMock = new Mock<IMemoryCache>();

		// Init handler
		_handler = new RetrieveCharacterByNameQueryHandler(
			_blizzardApiClientMock.Object,
			_mapper,
			_memoryCacheMock.Object
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
		var full = new CharacterProfileDto();
		_blizzardApiClientMock
			.Setup(client => client.FetchDataAsync<CharacterProfile>(It.IsAny<string>(), It.IsAny<CancellationToken>()))
			.ReturnsAsync(characterData);

		CharacterProfile? outValue = new();
		object whatever;
		_memoryCacheMock
			.Setup(mc => mc.TryGetValue(It.IsAny<object>(), out whatever))
			.Callback(new OutDelegate<object, object>((object k, out object v) =>
				v = new object())) // mocked value here (and/or breakpoint)
			.Returns(false);

		_memoryCacheMock
			.Setup(x => x.CreateEntry(It.IsAny<object>()))
			.Returns(Mock.Of<ICacheEntry>);


		// Act
		var result = await _handler.Handle(query, CancellationToken.None);

		// Assert
		result.Should().NotBeNull();
		result.Character.Should().NotBeNull();
		result.Character.Name.Should().Be("Test Character");
		result.Character.Race?.Id.Should().Be(1);
		result.Character.Class?.Id.Should().Be(2);
		result.Character.Level.Should().Be(60);
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
		object whatever;

		// Act
		Func<Task> act = async () => await _handler.Handle(query, CancellationToken.None);

		// Assert
		await act.Should().ThrowAsync<ApplicationException>().WithMessage("Character not found.");
	}
}
delegate void OutDelegate<TIn, TOut>(TIn input, out TOut output);
delegate T helpDelegate<TIn,T,TInn>(TIn inputKey, T inout, TInn input);
