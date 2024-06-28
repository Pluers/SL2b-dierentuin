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
    public class AnimalTests
    {
        public static DbContextOptions<dierentuinContext> GetDbContextOptions(string databaseName)
        {
            var options = new DbContextOptionsBuilder<dierentuinContext>()
                .UseInMemoryDatabase(databaseName)
                .Options;

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
            var options = AnimalTests.GetDbContextOptions("TestDB_IndexTest");
        
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
            var options = AnimalTests.GetDbContextOptions("TestDB_AddAnimal");
            var newAnimal = new Animal { Name = "New Lion", Species = "Panthera leo new" };

            using (var context = new dierentuinContext(options))
            {
                var controller = new AnimalsController(context);

                var result = await controller.Create(newAnimal);

                var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            }

            using (var context = new dierentuinContext(options))
            {
                Assert.True(context.Animal.Any(a => a.Name == "New Lion" && a.Species == "Panthera leo new"));
            }
        }

        // POST
        [Fact]
        public async Task PostAnimal_UpdatesAnimal_ReturnsRedirectToActionResult()
        {
            var options = AnimalTests.GetDbContextOptions("TestDB_PutAnimal");
            var testAnimalId = 1;
        
            using (var context = new dierentuinContext(options))
            {
                var updatedAnimal = new Animal { Id = testAnimalId, Name = "Updated Lion", Species = "Panthera leo updated" };
                var controller = new AnimalsController(context);
        
                var result = await controller.Edit(testAnimalId, updatedAnimal);
        
                var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
                Assert.Equal("Index", redirectToActionResult.ActionName);
            }
        
            using (var context = new dierentuinContext(options))
            {
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
            var options = AnimalTests.GetDbContextOptions("TestDB_DeleteAnimal");
            var testAnimalId = 1;
        
            using (var context = new dierentuinContext(options))
            {
                var controller = new AnimalsController(context);
        
                // Simulate the deletion confirmation
                var result = await controller.DeleteConfirmed(testAnimalId);
        
                var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
                Assert.Equal(nameof(AnimalsController.Index), redirectToActionResult.ActionName);
            }
        
            using (var context = new dierentuinContext(options))
            {
                // Verify the animal has been removed
                Assert.False(context.Animal.Any(a => a.Id == testAnimalId));
            }
        }
    }
}