﻿using System;
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
    public class EnclosuresController : Controller
    {
        private readonly dierentuinContext _context;

        // Reference to the dbcontext
        public EnclosuresController(dierentuinContext context)
        {
            _context = context;
        }

        // GET: Enclosures
        public async Task<IActionResult> Index()
        {
            // Get all enclosures
            return View(await _context.Enclosure.ToListAsync());
        }

        // GET: Enclosures/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Get a single enclosure with animals to display other properties other than the saved id from the database
            var enclosure = await _context.Enclosure
                .Include(a => a.Animals)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (enclosure == null)
            {
                return NotFound();
            }

            return View(enclosure);
        }

        // GET: Enclosures/Create
        public IActionResult Create()
        {
            // Get all the values from the enums to display in the dropdowns and save them in a viewbag
            ViewBag.Climate = new SelectList(Enum.GetValues(typeof(EnclosureClimateType)));
            ViewBag.HabitatType = new SelectList(Enum.GetValues(typeof(EnclosureHabitatEnvironment)));
            ViewBag.SecurityLevel = new SelectList(Enum.GetValues(typeof(SecurityClassification)));

            return View();
        }

        // POST: Enclosures/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Climate,HabitatType,SecurityLevel,EnclosureSize")] Enclosure enclosure)
        {
            if (ModelState.IsValid)
            {
                // Create the enclosure
                _context.Add(enclosure);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(enclosure);
        }

        // GET: Enclosures/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Get a single enclosure to edit
            var enclosure = await _context.Enclosure.FindAsync(id);
            if (enclosure == null)
            {
                return NotFound();
            }
            // Get all the values from the enums to display in the dropdowns and save them in a viewbag
            ViewBag.Climate = new SelectList(Enum.GetValues(typeof(EnclosureClimateType)));
            ViewBag.HabitatType = new SelectList(Enum.GetValues(typeof(EnclosureHabitatEnvironment)));
            ViewBag.SecurityLevel = new SelectList(Enum.GetValues(typeof(SecurityClassification)));
            return View(enclosure);
        }

        // POST: Enclosures/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Climate,HabitatType,SecurityLevel,EnclosureSize")] Enclosure enclosure)
        {
            if (id != enclosure.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Update the enclosure
                    _context.Update(enclosure);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EnclosureExists(enclosure.Id))
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
            return View(enclosure);
        }

        // GET: Enclosures/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Get a single enclosure to delete
            var enclosure = await _context.Enclosure
                .Include(a => a.Animals)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (enclosure == null)
            {
                return NotFound();
            }

            return View(enclosure);
        }

        // POST: Enclosures/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // Fetch the enclosure to be deleted
            var enclosure = await _context.Enclosure.FindAsync(id);
            if (enclosure != null)
            {
                // Get all the animals in the enclosure to unlink them
                var animalsInEnclosure = await _context.Animal
                    .Where(a => a.EnclosureId == id)
                    .ToListAsync();

                // Set the EnclosureId of each animal to null
                foreach (var animal in animalsInEnclosure)
                {
                    animal.EnclosureId = null;
                }

                // Save the changes to the animals
                await _context.SaveChangesAsync();

                // Remove the enclosure
                _context.Enclosure.Remove(enclosure);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        // Check if the enclosure exists
        private bool EnclosureExists(int id)
        {
            return _context.Enclosure.Any(e => e.Id == id);
        }
    }
}
