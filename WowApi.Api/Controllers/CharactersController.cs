using MediatR;
using Microsoft.AspNetCore.Mvc;
using WowApi.Application.Dtos;
using WowApi.Application.Handlers;

namespace WowApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CharactersController : ControllerBase
    {
 
        private readonly ILogger<CharactersController> _logger;

        private readonly IMediator _mediator;

        public CharactersController(ILogger<CharactersController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet("character",Name = "GetCharacter")]
        [ProducesResponseType(typeof(CharacterProfileDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<CharacterProfileDto>> GetCharacter(string characterName,string realm)
        {
            _logger.LogInformation("Received request to get character profile for {CharacterName} on {Realm}", characterName, realm);

            var query = new GetCharacterByNameQuery(characterName, realm);
            var result = await _mediator.Send(query);
            if (result == null)
            {
                _logger.LogWarning("Character profile not found for {CharacterName} on {Realm}", characterName, realm);
                return NotFound();
            }

            _logger.LogInformation("Successfully retrieved character profile for {CharacterName} on {Realm}", characterName, realm);
            return Ok(result);

        }

    }
}
