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
    public class ActivityTrailsController : ControllerBase
    {
        private readonly COMP2001MAL_KChunleongContext _context;

        public ActivityTrailsController(COMP2001MAL_KChunleongContext context)
        {
            _context = context;
        }

        // GET: api/ActivityTrails
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ActivityTrail>>> GetActivityTrails()
        {
          if (_context.ActivityTrails == null)
          {
              return NotFound();
          }
            return await _context.ActivityTrails.ToListAsync();
        }

        // GET: api/ActivityTrails/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ActivityTrail>> GetActivityTrail(int id)
        {
          if (_context.ActivityTrails == null)
          {
              return NotFound();
          }
            var activityTrail = await _context.ActivityTrails.FindAsync(id);

            if (activityTrail == null)
            {
                return NotFound();
            }

            return activityTrail;
        }

        // PUT: api/ActivityTrails/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutActivityTrail(int id, ActivityTrail activityTrail)
        {
            if (id != activityTrail.ActivityTrailId)
            {
                return BadRequest();
            }

            _context.Entry(activityTrail).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ActivityTrailExists(id))
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

        // POST: api/ActivityTrails
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ActivityTrail>> PostActivityTrail(ActivityTrail activityTrail)
        {
          if (_context.ActivityTrails == null)
          {
              return Problem("Entity set 'COMP2001MAL_KChunleongContext.ActivityTrails'  is null.");
          }
            _context.ActivityTrails.Add(activityTrail);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ActivityTrailExists(activityTrail.ActivityTrailId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetActivityTrail", new { id = activityTrail.ActivityTrailId }, activityTrail);
        }

        // DELETE: api/ActivityTrails/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteActivityTrail(int id)
        {
            if (_context.ActivityTrails == null)
            {
                return NotFound();
            }
            var activityTrail = await _context.ActivityTrails.FindAsync(id);
            if (activityTrail == null)
            {
                return NotFound();
            }

            _context.ActivityTrails.Remove(activityTrail);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ActivityTrailExists(int id)
        {
            return (_context.ActivityTrails?.Any(e => e.ActivityTrailId == id)).GetValueOrDefault();
        }
    }
}
