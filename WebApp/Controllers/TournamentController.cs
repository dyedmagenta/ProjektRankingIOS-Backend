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
    public class TournamentController : Controller
    {
        private readonly RankingContext _context;

        public TournamentController(RankingContext context)
        {
            _context = context;

            if (_context.TournamentItems.Count() == 0)
            {
                _context.FillTournaments();
            }
        }

        [HttpGet]
        public IEnumerable<Tournament> GetAll()
        {
            return _context.TournamentItems.ToList();
        }

        [HttpGet("{id}", Name = "GetTournament")]
        public IActionResult GetById(long id)
        {
            var item = _context.TournamentItems.FirstOrDefault(t => t.Id == id);
            if (item == null)
            {
                return NotFound();
            }
            return new ObjectResult(item);
        }

        [HttpPost]
        public IActionResult Create([FromBody] Tournament item)
        {
            if (item == null)
            {
                return BadRequest();
            }

            _context.TournamentItems.Add(item);
            _context.SaveChanges();

            return CreatedAtRoute("GetTournament", new { id = item.Id }, item);
        }

        [HttpPut("{id}")]
        public IActionResult Update(long id, [FromBody] Tournament item)
        {
            if (item == null || item.Id != id)
            {
                return BadRequest();
            }

            var tournament = _context.TournamentItems.FirstOrDefault(t => t.Id == id);
            if (tournament == null)
            {
                return BadRequest();
            }

            tournament.Name = item.Name;
            _context.TournamentItems.Update(tournament);
            _context.SaveChanges();
            return new NoContentResult();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            var tournament = _context.TournamentItems.First(x => x.Id == id);
            if (tournament == null)
            {
                return BadRequest();
            }

            _context.TournamentItems.Remove(tournament);
            _context.SaveChanges();
            return new NoContentResult();
        }
    }
}
