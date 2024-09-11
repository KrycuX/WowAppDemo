using MediatR;
using Microsoft.AspNetCore.Mvc;
using WowApi.Api.Controllers.Characters;
using WowApi.Shared.Dtos.Character;
using WowApi.Shared.Dtos.Mount;
using WowApi.Shared.UseCase.Character.Query;
using WowApi.Shared.UseCase.Mounts.Query;

namespace WowApi.Api.Controllers.Mounts;

[ApiController]
[Route("api/[controller]")]
public class MountsController(ILogger<CharactersController> logger, IMediator mediator) : ControllerBase
{
	private readonly ILogger<CharactersController> _logger = logger;
	private readonly IMediator _mediator = mediator;

	[HttpGet("ownedMounts", Name = "GetOwnedMounts")]
	[ProducesResponseType(typeof(MountDto), StatusCodes.Status200OK)]
	public async Task<ActionResult<List<MountDto>>> GetOwnedMounts(string characterName, string realm, string region)
	{
		_logger.LogInformation("Received request to get Mounts collection for {CharacterName} on {Region} - {Realm} ", characterName, region, realm);

		var query = new RetrieveMountsByCharacterQuery(characterName, realm, region);
		var result = await _mediator.Send(query);
		if (result == null)
		{
			_logger.LogWarning("Mounts collection not found for {CharacterName} on {Region} - {Realm}", characterName, region, realm);
			return NotFound();
		}

		_logger.LogInformation("Successfully retrieved Mounts collection for {CharacterName} on {Region} - {Realm}", characterName, region, realm);
		return Ok(result);

	}
}


