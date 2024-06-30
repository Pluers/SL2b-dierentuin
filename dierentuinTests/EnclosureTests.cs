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
    // This tests the CRUD operations of the EnclosuresController
    public class EnclosureTests
    {
        public static DbContextOptions<dierentuinContext> GetDbContextOptions(string databaseName)
        {
            // The reference to the dbcontext
            var options = new DbContextOptionsBuilder<dierentuinContext>()
                .UseInMemoryDatabase(databaseName)
                .Options;

            // Add an enclosure if it doesn't exist yet in the in-memory database
            using (var context = new dierentuinContext(options))
            {
                if (!context.Animal.Any())
                {
                    context.Enclosure.Add(new Enclosure { Name = "Birds Enclosure", EnclosureSize = 20 });
                    context.SaveChanges();
                }
            }

            return options;
        }

        // GET
        [Fact]
        public async Task Index_ReturnsAViewResult_WithAListOfEnclosures()
        {
            // Get all enclosures in the in-memory database
            var options = AnimalTests.GetDbContextOptions("TestDB_IndexTest_Enclosure");

            using (var context = new dierentuinContext(options))
            {
                var controller = new EnclosuresController(context);

                var result = await controller.Index();

                var viewResult = Assert.IsType<ViewResult>(result);
                var model = Assert.IsAssignableFrom<IEnumerable<Enclosure>>(viewResult.Model);
            }
        }
        // POST
        [Fact]
        public async Task AddEnclosure_CreatesNewEnclosure_ReturnsRedirectToActionResult()
        {
            // Create a new enclosure in the in-memory database
            var options = AnimalTests.GetDbContextOptions("TestDB_AddEnclosure");
            var newEnclosure = new Enclosure { Name = "New Enclosure", EnclosureSize = 123 };

            using (var context = new dierentuinContext(options))
            {
                var controller = new EnclosuresController(context);

                var result = await controller.Create(newEnclosure);

                // Check if the enclosure was added in the in-memory database
                var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
                Assert.True(context.Enclosure.Any(e => e.Name == "New Enclosure" && e.EnclosureSize == 123));
            }
        }

        // POST
        [Fact]
        public async Task PostEnclosure_UpdatesEnclosure_ReturnsRedirectToActionResult()
        {
            // Update an existing enclosure in the in-memory database
            var options = GetDbContextOptions("TestDB_PutEnclosure");
            var testEnclosureId = 1;

            using (var context = new dierentuinContext(options))
            {
                // Ensure an enclosure exists before attempting to update
                var existingEnclosure = await context.Enclosure.FindAsync(testEnclosureId);
                if (existingEnclosure == null)
                {
                    // Add an enclosure if it doesn't exist yet in the in-memory database
                    context.Enclosure.Add(new Enclosure { Id = testEnclosureId, Name = "Initial Enclosure", EnclosureSize = 100 });
                    await context.SaveChangesAsync();
                }



                // Retrieve the existing enclosure and update its properties
                var enclosureToUpdate = await context.Enclosure.FindAsync(testEnclosureId);
                Assert.NotNull(enclosureToUpdate);
                enclosureToUpdate.Name = "Updated Enclosure";
                enclosureToUpdate.EnclosureSize = 123;

                var controller = new EnclosuresController(context);
                var result = await controller.Edit(testEnclosureId, enclosureToUpdate);

                var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
                Assert.Equal("Index", redirectToActionResult.ActionName);


                // Check if the enclosure was updated in the in-memory database
                var updatedEnclosure = await context.Enclosure.FindAsync(testEnclosureId);
                Assert.NotNull(updatedEnclosure);
                Assert.Equal("Updated Enclosure", updatedEnclosure.Name);
                Assert.Equal(123, updatedEnclosure.EnclosureSize);
            }
        }

        // POST
        [Fact]
        public async Task DeleteEnclosure_RemovesEnclosure_ReturnsRedirectToActionResult()
        {
            // Delete an existing enclosure in the in-memory database
            var options = AnimalTests.GetDbContextOptions("TestDB_DeleteEnclosure");
            var testEnclosureId = 1;

            using (var context = new dierentuinContext(options))
            {
                var controller = new EnclosuresController(context);

                var result = await controller.DeleteConfirmed(testEnclosureId);

                // Check if the enclosure was removed from the in-memory database
                var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
                Assert.Equal(nameof(EnclosuresController.Index), redirectToActionResult.ActionName);
                Assert.False(context.Enclosure.Any(e => e.Id == testEnclosureId));
            }
        }
    }
}