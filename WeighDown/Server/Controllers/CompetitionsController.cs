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
    public class CompetitionsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CompetitionsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Competition>>> GetCompetitions()
        {
            return await _context.Competitions
                .Include(c => c.Contestants)
                .ThenInclude(c => c.WeightLogs)
                .Include(c => c.WeighInDeadlines)
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Competition>> GetCompetition(int id)
        {
            var competition = await _context.Competitions.FindAsync(id);

            if (competition == null)
            {
                return NotFound();
            }

            return await _context.Competitions
                .Where(c => c.CompetitionId == id)
                .Include(c => c.Contestants)
                .ThenInclude(c => c.WeightLogs)
                .Include(c => c.WeighInDeadlines)
                .FirstOrDefaultAsync();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutCompetition(int id, Competition competition)
        {
            if (id != competition.CompetitionId)
            {
                return BadRequest();
            }

            _context.Entry(competition).State = EntityState.Modified;

            //competition.WeighInDeadlines.ForEach(w =>
            //{
            //    w.DeadlineDate = w.DeadlineDate.ToUniversalTime();
            //    w.WeighInDeadlineId = 0;
            //});

            //await _context.WeighInDeadlines.AddRangeAsync(competition.WeighInDeadlines);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CompetitionExists(id))
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
        public async Task<ActionResult<Competition>> PostCompetition(Competition competition)
        {
            _context.Competitions.Add(competition);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCompetition", new { id = competition.CompetitionId }, competition);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCompetition(int id)
        {
            var competition = await _context.Competitions.FindAsync(id);
            if (competition == null)
            {
                return NotFound();
            }

            _context.Competitions.Remove(competition);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CompetitionExists(int id)
        {
            return _context.Competitions.Any(e => e.CompetitionId == id);
        }
    }
}
