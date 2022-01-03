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
    public class WeighInDeadlinesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public WeighInDeadlinesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("competition/{id}")]
        public async Task<ActionResult<IEnumerable<WeighInDeadline>>> GetWeighInDeadlinesByCompetition(int id)
        {
            return await _context.WeighInDeadlines
                .Where(w => w.CompetitionId == id && w.IsActive)
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<WeighInDeadline>> GetWeighInDeadline(int id)
        {
            var weighInDeadline = await _context.WeighInDeadlines.FindAsync(id);

            if (weighInDeadline == null)
            {
                return NotFound();
            }

            return weighInDeadline;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutWeighInDeadline(int id, WeighInDeadline weighInDeadline)
        {
            if (id != weighInDeadline.WeighInDeadlineId)
            {
                return BadRequest();
            }

            _context.Entry(weighInDeadline).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WeighInDeadlineExists(id))
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
        public async Task<ActionResult<WeighInDeadline>> PostWeighInDeadline(WeighInDeadline weighInDeadline)
        {
            _context.WeighInDeadlines.Add(weighInDeadline);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetWeighInDeadline", new { id = weighInDeadline.WeighInDeadlineId }, weighInDeadline);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWeighInDeadline(int id)
        {
            var weighInDeadline = await _context.WeighInDeadlines.FindAsync(id);
            if (weighInDeadline == null)
            {
                return NotFound();
            }

            _context.WeighInDeadlines.Remove(weighInDeadline);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("competition/{id}")]
        public async Task<IActionResult> DeleteWeighInDeadlineRangeByCompetition(int id)
        {
            var deadlines = _context.WeighInDeadlines.Where(w => w.CompetitionId == id);

            _context.WeighInDeadlines.RemoveRange(deadlines);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool WeighInDeadlineExists(int id)
        {
            return _context.WeighInDeadlines.Any(e => e.WeighInDeadlineId == id);
        }
    }
}
