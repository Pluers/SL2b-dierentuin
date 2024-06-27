﻿using System;
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
    public class EnclosuresAPIController : ControllerBase
    {
        private readonly dierentuinContext _context;

        public EnclosuresAPIController(dierentuinContext context)
        {
            _context = context;
        }

        // GET: api/EnclosuresAPI
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Enclosure>>> GetEnclosure()
        {
            return await _context.Enclosure.ToListAsync();
        }

        // GET: api/EnclosuresAPI/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Enclosure>> GetEnclosure(int id)
        {
            var enclosure = await _context.Enclosure.FindAsync(id);

            if (enclosure == null)
            {
                return NotFound();
            }

            return enclosure;
        }

        // PUT: api/EnclosuresAPI/5
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

        // POST: api/EnclosuresAPI
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Enclosure>> PostEnclosure(Enclosure enclosure)
        {
            _context.Enclosure.Add(enclosure);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEnclosure", new { id = enclosure.Id }, enclosure);
        }

        // DELETE: api/EnclosuresAPI/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEnclosure(int id)
        {
            var enclosure = await _context.Enclosure.FindAsync(id);
            if (enclosure == null)
            {
                return NotFound();
            }

            _context.Enclosure.Remove(enclosure);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EnclosureExists(int id)
        {
            return _context.Enclosure.Any(e => e.Id == id);
        }
    }
}