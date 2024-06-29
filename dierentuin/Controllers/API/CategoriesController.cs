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
    public class CategoriesController : ControllerBase
    {
        private readonly dierentuinContext _context;

        // Reference to the dbcontext
        public CategoriesController(dierentuinContext context)
        {
            _context = context;
        }

        // GET: api/Categories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetCategory()
        {
            // Get all categories by animal dto
            var categories = await _context.Category
                .Select(c => new 
                {
                    c.Id,
                    c.Name,
                    Animals = c.Animals.Select(a => new ForeignAnimalDTO { Id = a.Id, Name = a.Name, Size = a.Size }).ToList()
                })
                .ToListAsync();
        
            return Ok(categories);
        }

        // GET: api/Categories/5
        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetCategory(int id)
        {
            // Get a single category by animal dto
            var category = await _context.Category
                .Where(c => c.Id == id)
                .Select(c => new 
                {
                    c.Id,
                    c.Name,
                    Animals = c.Animals.Select(a => new ForeignAnimalDTO { Id = a.Id, Name = a.Name, Size = a.Size }).ToList()
                })
                .FirstOrDefaultAsync();
        
            if (category == null)
            {
                return NotFound();
            }
        
            return Ok(category);
        }

        // PUT: api/Categories/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCategory(int id, Category category)
        {
            if (id != category.Id)
            {
                return BadRequest();
            }

            // Update the category
            _context.Entry(category).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoryExists(id))
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

        // POST: api/Categories
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Category>> PostCategory(Category category)
        {
            // Create category
            _context.Category.Add(category);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCategory", new { id = category.Id }, category);
        }

        // DELETE: api/Categories/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
{
    using (var transaction = await _context.Database.BeginTransactionAsync())
    {
        try
        {
            var category = await _context.Category.Include(a => a.Animals).FirstOrDefaultAsync(c => c.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            // Unlink animals from the category
            foreach (var animal in category.Animals)
            {
                animal.CategoryId = null;
            }

            // Remove the category
            _context.Category.Remove(category);

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

        private bool CategoryExists(int id)
        {
            return _context.Category.Any(e => e.Id == id);
        }
    }
}
