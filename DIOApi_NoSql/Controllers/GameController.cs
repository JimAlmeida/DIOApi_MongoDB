using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTO;
using API.Repository;
using MongoDB.Bson;
using System.Diagnostics.CodeAnalysis;

namespace API.Controllers
{

    [Route("api/v1/[controller]")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private readonly IGameRepository repository;

        public GameController(IGameRepository gameRepository)
        {
            repository = gameRepository;
        }

        // GET: api/GameItems
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Game))]
        public async Task<IActionResult> GetGames()
        {
            var games = await repository.Get();
            return Ok(games);
        }

        [HttpGet("search")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Game))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Search([FromQuery] string name)
        {
            if (name is not null)
            {
                var queryResults = await repository.GetByName(name);
                return (queryResults.Count() > 0)? Ok(queryResults) : NoContent();
            }
            else
            {
                return BadRequest("There were no parameters in this query!");
            }
        }
        
        // GET: api/GameItems/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Game))]
        public async Task<IActionResult> GetGameItem(string id)
        {
            var gameItem = await repository.Get(id);

            if (gameItem == null)
            {
                return NotFound();
            }

            return Ok(gameItem);
        }

        // PUT: api/GameItems/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGameItem(string id, Game gameItem)
        {
            if (id != gameItem.Id)
            {
                return BadRequest();
            }

            var gameOld = await repository.Get(id);

            if (gameOld == null)
            {
                return NotFound();
            }

            await repository.Update(id, gameItem);

            return NoContent();
        }

        // POST: api/GameItems
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<IActionResult> PostGameItem(Game gameItem)
        {
            bool gameDoesExist = await GameItemExistsByName(gameItem.Name);
            if (gameDoesExist)
            {
                return UnprocessableEntity("There's already a game with this name");
            }
            else
            {
                var gameId = await repository.Create(gameItem);
                return CreatedAtAction("GetGameItem", new { id = gameId.ToString() }, gameItem);
            }
        }

        // DELETE: api/GameItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGameItem(string id)
        {
            var gameOld = await repository.Get(id);

            if (gameOld == null)
            {
                return NotFound();
            }

            await repository.Delete(id);

            return NoContent();
        }

        private async Task<bool> GameItemExistsById(string id)
        {
            var gameFromRepository = await repository.Get(id);
            return gameFromRepository is not null;
        }

        private async Task<bool> GameItemExistsByName(string name)
        {
            var queryResults = await repository.GetByName(name);
            var gameFromRepository = queryResults.FirstOrDefault();
            return gameFromRepository is not null;
        }
    }
 }

