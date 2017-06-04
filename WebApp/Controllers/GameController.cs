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
                    return BadRequest( new { Info = "White Player Not Found" });
                }
            }
            if (item.BlackPlayer == null)
            {
                item.BlackPlayer = _context.PlayerItems.FirstOrDefault(e => e.Id == item.BlackPlayerId);
                if (item.BlackPlayer == null)
                {
                    return BadRequest("Black Player NotFound");
                }
            }
            if (item.Tournament == null)
            {
                item.Tournament = _context.TournamentItems.FirstOrDefault(e => e.Id == item.TournamentId);
                if (item.Tournament == null)
                {
                    return BadRequest("Tournament NotFound");
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

            var player = _context.PlayerItems.FirstOrDefault(e => e.Id == item.WhitePlayerId);
            if (player == null)
            {
                return BadRequest("White Player NotFound");
            }
            game.WhitePlayerId = item.WhitePlayerId;
            game.WhitePlayer = player;
            player = _context.PlayerItems.FirstOrDefault(e => e.Id == item.BlackPlayerId);
            if (player == null)
            {
                return BadRequest("Black Player NotFound");
            }
            game.BlackPlayerId = item.BlackPlayerId;
            game.BlackPlayer = player;

            game.WhiteScoreChange = item.WhiteScoreChange;
            game.WhitePlayer.Score += game.WhiteScoreChange;
            game.BlackScoreChange = item.BlackScoreChange;
            game.BlackPlayer.Score += game.BlackScoreChange;

            game.Date = item.Date;

            var tournament = _context.TournamentItems.FirstOrDefault(e => e.Id == item.TournamentId);
            if (tournament == null)
            {
                return BadRequest("Tournament doesn't exist");
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
                return BadRequest();
            }

            _context.GameItems.Remove(game);
            _context.SaveChanges();
            return new NoContentResult();
        }
    }
}
