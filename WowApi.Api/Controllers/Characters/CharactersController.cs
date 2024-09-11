using MediatR;
using Microsoft.AspNetCore.Mvc;
using WowApi.Shared.Dtos.Character;
using WowApi.Shared.UseCase.Character.Query;

namespace WowApi.Api.Controllers.Characters
{
    [ApiController]
    [Route("api/[controller]")]
    public class CharactersController(ILogger<CharactersController> logger , IMediator mediator) : ControllerBase
    {
        private readonly ILogger<CharactersController> _logger = logger;
        private readonly IMediator _mediator = mediator;

        [HttpGet("character", Name = "GetCharacter")]
        [ProducesResponseType(typeof(CharacterProfileDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<CharacterProfileDto>> GetCharacter(string characterName, string realm, string region)
        {
            _logger.LogInformation("Received request to get character profile for {CharacterName} on {Region} - {Realm} ", characterName, region, realm);

            var query = new RetrieveCharacterByNameQuery(characterName, realm, region);
            var result = await _mediator.Send(query);
            if (result == null)
            {
                _logger.LogWarning("Character profile not found for {CharacterName} on {Region} - {Realm}", characterName, region, realm);
                return NotFound();
            }

            _logger.LogInformation("Successfully retrieved character profile for {CharacterName} on {Region} - {Realm}", characterName, region, realm);
            return Ok(result);

        }


    }
}
