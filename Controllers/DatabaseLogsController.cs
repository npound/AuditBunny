using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LoanLogics;

namespace LoanLogics.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class DatabaseLogsController : Controller
    {
        private readonly AdventureWorks2014Context _context;

        public DatabaseLogsController(AdventureWorks2014Context context)
        {
            _context = context;
        }

        // GET: api/DatabaseLogs
        [HttpGet]
        public IEnumerable<DatabaseLog> GetDatabaseLog()
        {
            return _context.DatabaseLog;
        }

        // GET: api/DatabaseLogs/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDatabaseLog([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var databaseLog = await _context.DatabaseLog.FindAsync(id);

            if (databaseLog == null)
            {
                return NotFound();
            }

            return Ok(databaseLog);
        }

        // PUT: api/DatabaseLogs/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDatabaseLog([FromRoute] int id, [FromBody] DatabaseLog databaseLog)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != databaseLog.DatabaseLogId)
            {
                return BadRequest();
            }

            _context.Entry(databaseLog).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DatabaseLogExists(id))
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

        // POST: api/DatabaseLogs
        [HttpPost]
        public async Task<IActionResult> PostDatabaseLog([FromBody] DatabaseLog databaseLog)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.DatabaseLog.Add(databaseLog);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDatabaseLog", new { id = databaseLog.DatabaseLogId }, databaseLog);
        }

        // DELETE: api/DatabaseLogs/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDatabaseLog([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var databaseLog = await _context.DatabaseLog.FindAsync(id);
            if (databaseLog == null)
            {
                return NotFound();
            }

            _context.DatabaseLog.Remove(databaseLog);
            await _context.SaveChangesAsync();

            return Ok(databaseLog);
        }

        private bool DatabaseLogExists(int id)
        {
            return _context.DatabaseLog.Any(e => e.DatabaseLogId == id);
        }
    }
}