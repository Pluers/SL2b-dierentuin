using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Xunit;
using dierentuin.Controllers;
using dierentuin.Data;
using dierentuin.Models;
namespace dierentuinTests
{
    // This tests the CRUD operations of the AnimalsController
    public class AnimalTests
    {
        public static DbContextOptions<dierentuinContext> GetDbContextOptions(string databaseName)
        {
            // The reference to the dbcontext
            var options = new DbContextOptionsBuilder<dierentuinContext>()
                .UseInMemoryDatabase(databaseName)
                .Options;

            // Add an animal if it doesn't exist yet in the in-memory database
            using (var context = new dierentuinContext(options))
            {
                if (!context.Animal.Any())
                {
                    context.Animal.Add(new Animal { Name = "Lion", Species = "Panthera leo" });
                    context.SaveChanges();
                }
            }

            return options;
        }

        // GET
        [Fact]
        public async Task Index_ReturnsAViewResult_WithAListOfAnimals()
        {
            // Get all animals in the in-memory database
            var options = GetDbContextOptions("TestDB_IndexTest");

            using (var context = new dierentuinContext(options))
            {
                var controller = new AnimalsController(context);

                var result = await controller.Index();

                var viewResult = Assert.IsType<ViewResult>(result);
                var model = Assert.IsAssignableFrom<IEnumerable<Animal>>(viewResult.Model);
            }
        }

        // POST
        [Fact]
        public async Task AddAnimal_CreatesNewAnimal_ReturnsRedirectToActionResult()
        {
            // Create a new animal in the in-memory database
            var options = GetDbContextOptions("TestDB_AddAnimal");
            var newAnimal = new Animal { Name = "New Lion", Species = "Panthera leo new" };

            using (var context = new dierentuinContext(options))
            {
                var controller = new AnimalsController(context);

                var result = await controller.Create(newAnimal);

                var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);



                // Add the new animal to the in-memory database
                Assert.True(context.Animal.Any(a => a.Name == "New Lion" && a.Species == "Panthera leo new"));
            }
        }

        // POST
        [Fact]
        public async Task PostAnimal_UpdatesAnimal_ReturnsRedirectToActionResult()
        {
            // Update an animal in the in-memory database
            var options = GetDbContextOptions("TestDB_PutAnimal");
            var testAnimalId = 1;

            using (var context = new dierentuinContext(options))
            {
                var updatedAnimal = new Animal { Id = testAnimalId, Name = "Updated Lion", Species = "Panthera leo updated" };
                var controller = new AnimalsController(context);

                var result = await controller.Edit(testAnimalId, updatedAnimal);

                var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
                Assert.Equal("Index", redirectToActionResult.ActionName);



                // Get the updated animal from the in-memory database
                var animal = await context.Animal.FindAsync(testAnimalId);
                Assert.NotNull(animal);
                Assert.Equal("Updated Lion", animal.Name);
                Assert.Equal("Panthera leo updated", animal.Species);
            }
        }

        // POST
        [Fact]
        public async Task DeleteAnimal_RemovesAnimal_ReturnsRedirectToActionResult()
        {
            // Delete an animal in the in-memory database
            var options = GetDbContextOptions("TestDB_DeleteAnimal");
            var testAnimalId = 1;

            using (var context = new dierentuinContext(options))
            {
                var controller = new AnimalsController(context);

                var result = await controller.DeleteConfirmed(testAnimalId);

                var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
                Assert.Equal(nameof(AnimalsController.Index), redirectToActionResult.ActionName);
                // Verify the animal has been removed
                Assert.False(context.Animal.Any(a => a.Id == testAnimalId));
            }
        }
    }
}