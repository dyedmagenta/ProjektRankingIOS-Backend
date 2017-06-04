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
    public class PlayerController : Controller
    {
        private readonly RankingContext _context;

        public PlayerController(RankingContext context)
        {
            _context = context;

            if (_context.PlayerItems.Count() == 0)
            {
                _context.FillPlayers();
            }
        }

        [HttpGet]
        public IEnumerable<Player> GetAll()
        {
            return _context.PlayerItems.OrderBy(item => item.Rank).ToList();
        }

        [HttpGet("{id}", Name = "GetPlayer")]
        public IActionResult GetById(long id)
        {
            var item = _context.PlayerItems.FirstOrDefault(t => t.Id == id);
            if (item == null)
            {
                return NotFound();
            }
            return new ObjectResult(item);
        }

        [HttpPost]
        public IActionResult Create([FromBody] Player item)
        {
            if (item == null)
            {
                return BadRequest();
            }

            _context.PlayerItems.Add(item);
            _context.SaveChanges();
            _context.FixRanks();
            return CreatedAtRoute("GetPlayer", new { id = item.Id }, item);
        }

        [HttpPut("{id}")]
        public IActionResult Update(long id, [FromBody] Player item)
        {
            if (item == null || item.Id != id)
            {
                return BadRequest();
            }

            var player = _context.PlayerItems.FirstOrDefault(t => t.Id == id);
            if (player == null)
            {
                return BadRequest();
            }

            player.Name = item.Name;
            player.Rank = null;
            player.FirstGame = item.FirstGame;
            player.PlayingSince = item.PlayingSince;
            player.Score = item.Score;

            _context.PlayerItems.Update(player);
            _context.SaveChanges();
            _context.FixRanks();
            return new NoContentResult();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            var player = _context.PlayerItems.First(x => x.Id == id);
            if (player == null)
            {
                return BadRequest();
            }

            _context.PlayerItems.Remove(player);
            _context.SaveChanges();
            _context.FixRanks();
            return new NoContentResult();
        }
    }
}
