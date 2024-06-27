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

        public AnimalsController(dierentuinContext context)
        {
            _context = context;
        }

        // GET: Animals
        public async Task<IActionResult> Index()
        {
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
            ViewBag.SizeTypes = new SelectList(Enum.GetValues(typeof(AnimalSize)));
            ViewBag.DietaryClass = new SelectList(Enum.GetValues(typeof(AnimalDietaryClass)));
            ViewBag.ActivityPattern = new SelectList(Enum.GetValues(typeof(AnimalActivityPattern)));
            ViewBag.SecurityLevel = new SelectList(Enum.GetValues(typeof(SecurityClassification)));
            ViewBag.Prey = new SelectList(_context.Animal.Select(a => a.Prey).Distinct().ToList());
            ViewBag.Categories = new SelectList(_context.Category.ToList(), "Id", "Name");

            return View();
        }

        // POST: Animals/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Species,Size,DietaryClass,ActivityPattern,CategoryId,EnclosureId,Prey,SpaceRequirement,SecurityRequirement")] Animal animal)
        {
            if (ModelState.IsValid)
            {
                var enclosureIds = _context.Enclosure.Select(e => e.Id).ToList();

                if (enclosureIds.Any())
                {
                    var random = new Random();
                    // Select a random EnclosureId from the list of existing IDs
                    var randomEnclosureId = enclosureIds[random.Next(enclosureIds.Count)];

                    // Assign the random EnclosureId to the animal
                    animal.EnclosureId = randomEnclosureId;
                }

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

            var animal = await _context.Animal.FindAsync(id);
            if (animal == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Set<Category>(), "id", "id", animal.CategoryId);
            ViewData["EnclosureId"] = new SelectList(_context.Set<Enclosure>(), "Id", "Id", animal.EnclosureId);
            return View(animal);
        }

        // POST: Animals/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Species,Size,DietaryClass,ActivityPattern,CategoryId,EnclosureId,Prey,SpaceRequirement,SecurityRequirement")] Animal animal)
        {
            if (id != animal.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
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
            ViewData["CategoryId"] = new SelectList(_context.Set<Category>(), "id", "id", animal.CategoryId);
            ViewData["EnclosureId"] = new SelectList(_context.Set<Enclosure>(), "Id", "Id", animal.EnclosureId);
            return View(animal);
        }

        // GET: Animals/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

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
            var animal = await _context.Animal.FindAsync(id);
            if (animal != null)
            {
                _context.Animal.Remove(animal);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AnimalExists(int id)
        {
            return _context.Animal.Any(e => e.Id == id);
        }
    }
}
