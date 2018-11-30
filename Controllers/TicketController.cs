using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860


/*
    Swagger urls
    https://localhost:44316/swagger/v1/swagger.json
    https://localhost:44316/swagger/index.html
 */

namespace CoreWebAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class TicketController : Controller
    {
        private TicketContext _context;

        public TicketController(TicketContext context)
        {
            _context = context;

            if (!_context.TicketItems.Any())
            {
                _context.TicketItems.Add(new TicketItem {Concert = "AC/DC"});
                _context.TicketItems.Add(new TicketItem { Concert = "Metallica" });
                _context.TicketItems.Add(new TicketItem { Concert = "Led Zepplin" });
                _context.TicketItems.Add(new TicketItem { Concert = "Aerosmith" });
                _context.TicketItems.Add(new TicketItem { Concert = "Scorpions" });
                _context.TicketItems.Add(new TicketItem { Concert = "Iron Maden" });
                _context.TicketItems.Add(new TicketItem { Concert = "Guns 'N Roses" });
                _context.TicketItems.Add(new TicketItem { Concert = "Bon Jovi" });
                _context.TicketItems.Add(new TicketItem { Concert = "Kiss" });
                _context.TicketItems.Add(new TicketItem { Concert = "Pantera" });
                _context.SaveChanges();
            }
        }
        
        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<TicketItem> GetAll()
        {
            return _context.TicketItems.AsNoTracking().ToList();
        }

        
        [HttpGet("{id}", Name = "GetTicket")]
        public IActionResult GetById(long id)
        {
            var ticket = _context.TicketItems.FirstOrDefault(x => x.Id == id);
            if (ticket == null)
            {
                return NotFound(); //404
            }

            return new ObjectResult(ticket);//200
        }

        
        [HttpPost]
        public IActionResult Create([FromBody]TicketItem ticket)
        {
            if (ticket == null)
            {
                return BadRequest(); //400
            }

            _context.TicketItems.Add(ticket);
            _context.SaveChanges();

            return CreatedAtRoute("GetTicket", new {it = ticket.Id}, ticket);


        }

        
        [HttpPut("{id}")]
        public IActionResult Update(long id, [FromBody]TicketItem ticket)
        {
            if (ticket == null || ticket.Id != id)
            {
                return BadRequest();
            }

            var tic = _context.TicketItems.FirstOrDefault(x => x.Id == id);
            if (tic == null)
            {
                return NotFound();
            }

            tic.Concert = ticket.Concert;
            tic.Available = ticket.Available;
            tic.Artist = ticket.Artist;

            _context.TicketItems.Update(tic);
            _context.SaveChanges();

            return new NoContentResult();
        }

        [HttpDelete("{id}")]
        private IActionResult Delete(long id)
        {
            var tic = _context.TicketItems.FirstOrDefault(x => x.Id == id);
            if (tic == null)
            {
                return NotFound();
            }

            _context.TicketItems.Remove(tic);
            _context.SaveChanges();
            return new NoContentResult();
        }
    }
}
