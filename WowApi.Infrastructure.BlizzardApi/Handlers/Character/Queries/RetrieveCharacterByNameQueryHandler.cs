using AutoMapper;
using MediatR;
using System.Text.Json;
using WowApi.Infrastructure.BlizzardApi.Services;
using WowApi.Shared.UseCase.Querry;

namespace WowApi.Infrastructure.BlizzardApi.Handlers.Character.Queries;
public class RetrieveCharacterByNameQueryHandler(ICharacterDataService characterDataService, IMapper mapper)
		: IRequestHandler<RetrieveCharacterByNameQuery, RetrieveCharacterByNameQuery.Response>
{
	private readonly IMapper _mapper = mapper;
	private readonly ICharacterDataService _characterDataService = characterDataService;

	public async Task<RetrieveCharacterByNameQuery.Response> Handle(RetrieveCharacterByNameQuery request, CancellationToken cancellationToken)
	{
		try
		{
			var data = await _characterDataService.GetFullData(request, cancellationToken);
			return _mapper.Map<RetrieveCharacterByNameQuery.Response>(data);
		}
		catch (HttpRequestException ex)
		{
			throw new ApplicationException("Error occurred while fetching character profile.", ex);
		}
		catch (JsonException ex)
		{
			throw new ApplicationException("Error occurred while deserializing character profile.", ex);
		}
		catch (Exception ex)
		{
			throw new ApplicationException("Character not found.", ex);
		}
	}

}
