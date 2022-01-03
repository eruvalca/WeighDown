using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WeighDown.Server.Data;
using WeighDown.Shared.Models;

namespace WeighDown.Server.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class ContestantsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ContestantsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Contestant>>> GetContestants()
        {
            return await _context.Contestants.ToListAsync();
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult<Contestant>> GetContestant(string userId)
        {
            var contestant = await _context.Contestants
                .Where(c => c.WeighDownUserId == userId)
                .Include(c => c.WeightLogs)
                .FirstOrDefaultAsync();

            if (contestant == null)
            {
                return NotFound();
            }

            return contestant;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutContestant(int id, Contestant contestant)
        {
            if (id != contestant.ContestantId)
            {
                return BadRequest();
            }

            _context.Entry(contestant).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ContestantExists(id))
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

        [HttpPost]
        public async Task<ActionResult<Contestant>> PostContestant(Contestant contestant)
        {
            _context.Contestants.Add(contestant);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetContestant", new { userId = contestant.WeighDownUserId }, contestant);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteContestant(int id)
        {
            var contestant = await _context.Contestants.FindAsync(id);
            if (contestant == null)
            {
                return NotFound();
            }

            _context.Contestants.Remove(contestant);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ContestantExists(int id)
        {
            return _context.Contestants.Any(e => e.ContestantId == id);
        }
    }
}
