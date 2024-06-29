using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using dierentuin.Data;
using dierentuin.Models;
using dierentuin.Enums;

namespace dierentuin.Controllers
{
    public class AnimalsController : Controller
    {
        private readonly dierentuinContext _context;

        // Reference to the dbcontext
        public AnimalsController(dierentuinContext context)
        {
            _context = context;
        }

        // GET: Animals
        public async Task<IActionResult> Index()
        {
            // Get all animals with category and enclosure to display other properties other than the saved id from the database
            var dierentuinContext = _context.Animal.Include(a => a.Category).Include(a => a.Enclosure);
            return View(await dierentuinContext.ToListAsync());
        }

        // GET: Animals/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Get a single animal with category and enclosure to display other properties other than the saved id from the database
            var animal = await _context.Animal
                .Include(a => a.Category)
                .Include(a => a.Enclosure)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (animal == null)
            {
                return NotFound();
            }

            return View(animal);
        }

        // GET: Animals/Create
        public IActionResult Create()
        {
            // Get all the values from the enums to display in the dropdowns and save them in a viewbag
            ViewBag.SizeTypes = new SelectList(Enum.GetValues(typeof(AnimalSize)));
            ViewBag.DietaryClass = new SelectList(Enum.GetValues(typeof(AnimalDietaryClass)));
            ViewBag.ActivityPattern = new SelectList(Enum.GetValues(typeof(AnimalActivityPattern)));
            ViewBag.SecurityLevel = new SelectList(Enum.GetValues(typeof(SecurityClassification)));
            ViewBag.PreyId = new SelectList(_context.Animal.Select(a => a.Prey).Distinct().ToList(), "Id", "Name");
            ViewBag.CategoryId = new SelectList(_context.Category.ToList(), "Id", "Name");

            return View();
        }

        // POST: Animals/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Species,Size,DietaryClass,ActivityPattern,CategoryId,EnclosureId,PreyId,SpaceRequirement,SecurityRequirement")] Animal animal)
        {
            if (ModelState.IsValid)
            {
                // When creating an animal, assign it a random enclosure
                var enclosureIds = _context.Enclosure.Select(e => e.Id).ToList();

                if (enclosureIds.Any())
                {
                    var random = new Random();
                    // Select a random EnclosureId from the list of existing ids
                    var randomEnclosureId = enclosureIds[random.Next(enclosureIds.Count)];

                    // Assign random EnclosureId to the animal
                    animal.EnclosureId = randomEnclosureId;
                }

                // finally create the animal
                _context.Add(animal);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(animal);
        }

        // GET: Animals/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Get a single animal
            var animal = await _context.Animal.FindAsync(id);
            if (animal == null)
            {
                return NotFound();
            }
            // Get all the values from the enums to display in the dropdowns and save them in a viewbag
            ViewBag.SizeTypes = new SelectList(Enum.GetValues(typeof(AnimalSize)));
            ViewBag.DietaryClass = new SelectList(Enum.GetValues(typeof(AnimalDietaryClass)));
            ViewBag.ActivityPattern = new SelectList(Enum.GetValues(typeof(AnimalActivityPattern)));
            ViewBag.SecurityLevel = new SelectList(Enum.GetValues(typeof(SecurityClassification)));
            // Display the name of the animal in the dropdown instead of the id
            ViewBag.PreyId = new SelectList(_context.Animal.Select(a => a.Prey).Distinct().ToList(), "Id", "Name");
            ViewBag.CategoryId = new SelectList(_context.Category.ToList(), "Id", "Name");
            ViewBag.EnclosureId = new SelectList(_context.Enclosure.ToList(), "Id", "Name");
            return View(animal);
        }

        // POST: Animals/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Species,Size,DietaryClass,ActivityPattern,CategoryId,EnclosureId,PreyId,SpaceRequirement,SecurityRequirement")] Animal animal)
        {
            if (id != animal.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Update the animal
                    _context.Update(animal);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AnimalExists(animal.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(animal);
        }

        // GET: Animals/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Get a single animal with category and enclosure to display other properties other than the saved id from the database
            var animal = await _context.Animal
                .Include(a => a.Category)
                .Include(a => a.Enclosure)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (animal == null)
            {
                return NotFound();
            }

            return View(animal);
        }

        // POST: Animals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // Get a single animal
            var animal = await _context.Animal.FindAsync(id);
            if (animal != null)
            {
                // Remove the animal
                _context.Animal.Remove(animal);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // Check if the animal exists
        private bool AnimalExists(int id)
        {
            return _context.Animal.Any(e => e.Id == id);
        }
    }
}
