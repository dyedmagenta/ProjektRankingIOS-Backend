using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApp.Model;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApp.Controllers
{
    [Route("api/[controller]")]
    public class GameController : Controller
    {
        private readonly RankingContext _context;

        public GameController(RankingContext context)
        {
            _context = context;

            if (_context.GameItems.Count() == 0)
            {
                _context.FillGames();
            }

        }

        [HttpGet]
        public IEnumerable<Game> GetAll()
        {
            return _context.GameItems.ToList();
        }

        [HttpGet("{id}", Name = "GetGame")]
        public IActionResult GetById(long id)
        {
            var item = _context.GameItems.FirstOrDefault(t => t.Id == id);
            if (item == null)
            {
                return NotFound();
            }
            return new ObjectResult(item);
        }

        [HttpPost]
        public IActionResult Create([FromBody] Game item)
        {
            if (item == null)
            {
                return BadRequest();
            }

            if (item.WhitePlayer == null)
            {
                item.WhitePlayer = _context.PlayerItems.FirstOrDefault(e => e.Id == item.WhitePlayerId);
                if (item.WhitePlayer == null)
                {
                    return BadRequest( new { Info = $"White Player with Id:{item.WhitePlayerId} Not Found" });
                }
            }
            if (item.BlackPlayer == null)
            {
                item.BlackPlayer = _context.PlayerItems.FirstOrDefault(e => e.Id == item.BlackPlayerId);
                if (item.BlackPlayer == null)
                {
                    return BadRequest(new { Info = $"Black Player with Id:{item.BlackPlayerId} Not Found" });
                }
            }
            if (item.Tournament == null)
            {
                item.TournamentId = 1;
                item.Tournament = _context.TournamentItems.FirstOrDefault(e => e.Id == item.TournamentId);
                if (item.Tournament == null)
                {
                    return BadRequest(new { Info = $"Tournament with Id:{item.TournamentId} Not Found" });
                }
            }

            item.WhitePlayer.Score += item.WhiteScoreChange;
            item.BlackPlayer.Score += item.BlackScoreChange;

            _context.GameItems.Add(item);
            _context.SaveChanges();
            _context.FixRanks();
            return CreatedAtRoute("GetGame", new { id = item.Id }, item);
        }

        [HttpPut("{id}")]
        public IActionResult Update(long id, [FromBody] Game item)
        {
            if (item == null || item.Id != id)
            {
                return BadRequest();
            }

            var game = _context.GameItems.FirstOrDefault(t => t.Id == id);
            if (game == null)
            {
                return BadRequest();
            }

            ResetPlayerScores(game);
            
            var player = _context.PlayerItems.FirstOrDefault(e => e.Id == item.WhitePlayerId);
            if (player == null)
            {
                return BadRequest( new { Info = $"White Player with Id:{item.WhitePlayerId} Not Found" } );
            }
            game.WhitePlayerId = item.WhitePlayerId;
            game.WhitePlayer = player;
            player = _context.PlayerItems.FirstOrDefault(e => e.Id == item.BlackPlayerId);
            if (player == null)
            {
                return BadRequest(new { Info = $"Black Player with Id:{item.BlackPlayerId} Not Found" });
            }
            game.BlackPlayerId = item.BlackPlayerId;
            game.BlackPlayer = player;

            game.WhiteScoreChange = item.WhiteScoreChange;
            game.BlackScoreChange = item.BlackScoreChange;

            SetPlayerScores(item);
            
            game.Date = item.Date;

            //Dirty fix, I don't need to use tournaments at all :<
            item.TournamentId = 1;
            var tournament = _context.TournamentItems.FirstOrDefault(e => e.Id == item.TournamentId);
            if (tournament == null)
            {
                return BadRequest(new { Info = $"Tournament with Id:{item.TournamentId} Not Found" });
            }
            game.TournamentId = item.TournamentId;
            game.Tournament = tournament;

            _context.GameItems.Update(game);
            _context.SaveChanges();
            _context.FixRanks();
            return new NoContentResult();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            var game = _context.GameItems.FirstOrDefault(x => x.Id == id);
            if (game == null)
            {
                return BadRequest(new { Info = $"Game with game ID:{id} not found" });
            }
            
            ResetPlayerScores(game);

            _context.GameItems.Remove(game);
            _context.SaveChanges();
            _context.FixRanks();
            return new NoContentResult();
        }

        private void ResetPlayerScores(Game game)
        {
            var player = _context.PlayerItems.FirstOrDefault(e => e.Id == game.WhitePlayerId);
            if (player != null)
            {

                game.WhitePlayerId = game.WhitePlayerId;
                game.WhitePlayer = player;
                game.WhitePlayer.Score -= game.WhiteScoreChange;
            }
            player = _context.PlayerItems.FirstOrDefault(e => e.Id == game.BlackPlayerId);
            if (player != null)
            {
                game.BlackPlayerId = game.BlackPlayerId;
                game.BlackPlayer = player;
                game.BlackPlayer.Score -= game.BlackScoreChange;
            }
        }
        private void SetPlayerScores(Game game)
        {
            var player = _context.PlayerItems.FirstOrDefault(e => e.Id == game.WhitePlayerId);
            if (player != null)
            {

                game.WhitePlayerId = game.WhitePlayerId;
                game.WhitePlayer = player;
                game.WhitePlayer.Score += game.WhiteScoreChange;
            }
            player = _context.PlayerItems.FirstOrDefault(e => e.Id == game.BlackPlayerId);
            if (player != null)
            {
                game.BlackPlayerId = game.BlackPlayerId;
                game.BlackPlayer = player;
                game.BlackPlayer.Score += game.BlackScoreChange;
            }
        }
    }
}
