using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using dierentuin.Data;
using dierentuin.Models;

namespace dierentuin.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class EnclosuresController : ControllerBase
    {
        private readonly dierentuinContext _context;

        public EnclosuresController(dierentuinContext context)
        {
            _context = context;
        }

        // GET: api/Enclosures
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Enclosure>>> GetEnclosure()
        {
            var enclosures = await _context.Enclosure
                .Select(e => new
                {
                    e.Id,
                    e.Name,
                    e.Climate,
                    e.HabitatType,
                    e.SecurityLevel,
                    e.EnclosureSize,
                    Animals = e.Animals.Select(a => new ForeignAnimalDTO { Id = a.Id, Name = a.Name, Size = a.Size }).ToList()
                })
                .ToListAsync();
        
            return Ok(enclosures);
        }

        // GET: api/Enclosures/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Enclosure>> GetEnclosure(int id)
        {
            var enclosure = await _context.Enclosure
                .Where(m => m.Id == id)
                .Select(e => new
                {
                    e.Id,
                    e.Name,
                    e.Climate,
                    e.HabitatType,
                    e.SecurityLevel,
                    e.EnclosureSize,
                    Animals = e.Animals.Select(a => new ForeignAnimalDTO { Id = a.Id, Name = a.Name, Size = a.Size }).ToList()
                })
                .FirstOrDefaultAsync();

            if (enclosure == null)
            {
                return NotFound();
            }

            return Ok(enclosure);
        }
        // PUT: api/Enclosures/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEnclosure(int id, Enclosure enclosure)
        {
            if (id != enclosure.Id)
            {
                return BadRequest();
            }

            _context.Entry(enclosure).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EnclosureExists(id))
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

        // POST: api/Enclosures
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Enclosure>> PostEnclosure(Enclosure enclosure)
        {
            _context.Enclosure.Add(enclosure);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEnclosure", new { id = enclosure.Id }, enclosure);
        }

        // DELETE: api/Enclosures/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEnclosure(int id)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var enclosure = await _context.Enclosure.Include(e => e.Animals).FirstOrDefaultAsync(e => e.Id == id);
                    if (enclosure == null)
                    {
                        return NotFound();
                    }
        
                    // Unlink animals from the enclosure
                    foreach (var animal in enclosure.Animals)
                    {
                        animal.EnclosureId = null;
                    }
        
                    // Remove the enclosure
                    _context.Enclosure.Remove(enclosure);
        
                    await _context.SaveChangesAsync();
        
                    await transaction.CommitAsync();
        
                    return NoContent(); 
                }
                catch (Exception ex)
                {
                    // Rollback transaction if any error occurs
                    await transaction.RollbackAsync();
                    Console.WriteLine(ex.Message);
                    return StatusCode(StatusCodes.Status500InternalServerError, "Error deleting data");
                }
            }
        }

        private bool EnclosureExists(int id)
        {
            return _context.Enclosure.Any(e => e.Id == id);
        }
    }
}
