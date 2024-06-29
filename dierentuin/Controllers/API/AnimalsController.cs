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
    // This is the API controller for the Animals model
    // It is used to control the data that is being sent and retrieved to the client
    [Route("api/[controller]")]
    [ApiController]
    public class AnimalsController : ControllerBase
    {
        private readonly dierentuinContext _context;

        // Reference to the dbcontext
        public AnimalsController(dierentuinContext context)
        {
            _context = context;
        }

        // GET: api/Animals
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Animal>>> GetAnimal()
        {
            // This is to exclude prey and not create any loops because a prey can be an animal so it will create a loop
            var animals = await _context.Animal.ToListAsync();

            // DTO to exclude the prey and to control the data that is being sent
            var animalDTOs = animals.Select(a => new AnimalDTO
            {
                Id = a.Id,
                Name = a.Name,
                Species = a.Species,
                Size = a.Size,
                DietaryClass = a.DietaryClass,
                ActivityPattern = a.ActivityPattern,
                CategoryId = a.CategoryId,
                EnclosureId = a.EnclosureId,
                PreyId = a.PreyId,
                SpaceRequirement = a.SpaceRequirement,
                SecurityRequirement = a.SecurityRequirement
            }).ToList();

            return Ok(animalDTOs);
        }

        // GET: api/Animals/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Animal>> GetAnimal(int id)
        {
            // Get a single animal 
            var animal = await _context.Animal.FindAsync(id);

            if (animal == null)
            {
                return NotFound();
            }

            return animal;
        }

        // PUT: api/Animals/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAnimal(int id, Animal animal)
        {
            if (id != animal.Id)
            {
                return BadRequest();
            }

            // Update the animal 
            _context.Entry(animal).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AnimalExists(id))
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

        // POST: api/Animals
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Animal>> PostAnimal(Animal animal)
        {
            // Create Animal 
            _context.Animal.Add(animal);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAnimal", new { id = animal.Id }, animal);
        }

        // DELETE: api/Animals/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAnimal(int id)
        {
            var animal = await _context.Animal.FindAsync(id);
            if (animal == null)
            {
                return NotFound();
            }

            // Delete animal
            _context.Animal.Remove(animal);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AnimalExists(int id)
        {
            // Check if animal exists
            return _context.Animal.Any(e => e.Id == id);
        }
    }
}
