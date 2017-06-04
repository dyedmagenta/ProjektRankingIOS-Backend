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
    public class NewsController : Controller
    {
        private readonly RankingContext _context;

        public NewsController(RankingContext context)
        {
            _context = context;

            if (_context.NewsItems.Count() == 0)
            {
                _context.FillNews();
            }
        }

        [HttpGet]
        public IEnumerable<News> GetAll()
        {
            return _context.NewsItems.OrderBy(item => item.Date).ToList();
        }

        [HttpGet("{id}", Name = "GetNews")]
        public IActionResult GetById(long id)
        {
            var item = _context.NewsItems.FirstOrDefault(t => t.Id == id);
            if (item == null)
            {
                return NotFound();
            }
            return new ObjectResult(item);
        }

        [HttpPost]
        public IActionResult Create([FromBody] News item)
        {
            if (item == null)
            {
                return BadRequest();
            }

            _context.NewsItems.Add(item);
            _context.SaveChanges();

            return CreatedAtRoute("GetNews", new { id = item.Id }, item);
        }

        [HttpPut("{id}")]
        public IActionResult Update(long id, [FromBody] News item)
        {
            if (item == null || item.Id != id)
            {
                return BadRequest();
            }

            var newsItem = _context.NewsItems.FirstOrDefault(t => t.Id == id);
            if (newsItem == null)
            {
                return BadRequest();
            }

            newsItem.Content = item.Content;
            newsItem.Date = item.Date;

            _context.NewsItems.Update(newsItem);
            _context.SaveChanges();
            return new NoContentResult();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            var newsItem = _context.NewsItems.First(x => x.Id == id);
            if (newsItem == null)
            {
                return BadRequest();
            }

            _context.NewsItems.Remove(newsItem);
            _context.SaveChanges();
            return new NoContentResult();
        }
    }
}
