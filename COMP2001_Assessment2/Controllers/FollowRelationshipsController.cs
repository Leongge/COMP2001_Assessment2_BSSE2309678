using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using COMP2001_Assessment2.Models;

namespace COMP2001_Assessment2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FollowRelationshipsController : ControllerBase
    {
        private readonly COMP2001MAL_KChunleongContext _context;

        public FollowRelationshipsController(COMP2001MAL_KChunleongContext context)
        {
            _context = context;
        }

        // GET: api/FollowRelationships
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FollowRelationship>>> GetFollowRelationships()
        {
          if (_context.FollowRelationships == null)
          {
              return NotFound();
          }
            return await _context.FollowRelationships.ToListAsync();
        }

        // GET: api/FollowRelationships/5
        [HttpGet("{id}")]
        public async Task<ActionResult<FollowRelationship>> GetFollowRelationship(int id)
        {
          if (_context.FollowRelationships == null)
          {
              return NotFound();
          }
            var followRelationship = await _context.FollowRelationships.FindAsync(id);

            if (followRelationship == null)
            {
                return NotFound();
            }

            return followRelationship;
        }

        // PUT: api/FollowRelationships/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFollowRelationship(int id, FollowRelationship followRelationship)
        {
            if (id != followRelationship.FollowId)
            {
                return BadRequest();
            }

            _context.Entry(followRelationship).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FollowRelationshipExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/FollowRelationships
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<FollowRelationship>> PostFollowRelationship(FollowRelationship followRelationship)
        {
          if (_context.FollowRelationships == null)
          {
              return Problem("Entity set 'COMP2001MAL_KChunleongContext.FollowRelationships'  is null.");
          }
            _context.FollowRelationships.Add(followRelationship);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFollowRelationship", new { id = followRelationship.FollowId }, followRelationship);
        }

        // DELETE: api/FollowRelationships/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFollowRelationship(int id)
        {
            if (_context.FollowRelationships == null)
            {
                return NotFound();
            }
            var followRelationship = await _context.FollowRelationships.FindAsync(id);
            if (followRelationship == null)
            {
                return NotFound();
            }

            _context.FollowRelationships.Remove(followRelationship);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool FollowRelationshipExists(int id)
        {
            return (_context.FollowRelationships?.Any(e => e.FollowId == id)).GetValueOrDefault();
        }
    }
}
