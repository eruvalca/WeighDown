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
    public class WeightLogsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public WeightLogsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<WeightLog>>> GetWeightLogs()
        {
            return await _context.WeightLogs.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<WeightLog>> GetWeightLog(int id)
        {
            var weightLog = await _context.WeightLogs.FindAsync(id);

            if (weightLog == null)
            {
                return NotFound();
            }

            return weightLog;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutWeightLog(int id, WeightLog weightLog)
        {
            if (id != weightLog.WeightLogId)
            {
                return BadRequest();
            }

            _context.Entry(weightLog).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WeightLogExists(id))
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
        public async Task<ActionResult<WeightLog>> PostWeightLog(WeightLog weightLog)
        {
            _context.WeightLogs.Add(weightLog);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetWeightLog", new { id = weightLog.WeightLogId }, weightLog);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWeightLog(int id)
        {
            var weightLog = await _context.WeightLogs.FindAsync(id);
            if (weightLog == null)
            {
                return NotFound();
            }

            _context.WeightLogs.Remove(weightLog);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool WeightLogExists(int id)
        {
            return _context.WeightLogs.Any(e => e.WeightLogId == id);
        }
    }
}
