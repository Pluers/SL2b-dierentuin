using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using dierentuin.Models;
using dierentuin.Data;

namespace dierentuin.Controllers
{
    public class HomeController : Controller
    {
        private readonly dierentuinContext _context;

        // Reference to the dbcontext
        public HomeController(dierentuinContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // Get all animals, enclosures and categories to display them in the home page
            ViewBag.Animals = await _context.Animal.ToListAsync();
            ViewBag.Enclosures = await _context.Enclosure.ToListAsync();
            ViewBag.Categories = await _context.Category.ToListAsync();
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
