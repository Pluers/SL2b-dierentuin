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
    public class EnclosureTests
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
            var options = AnimalTests.GetDbContextOptions("TestDB_AddEnclosure");
            var newEnclosure = new Enclosure { Name = "New Enclosure", EnclosureSize = 123 };
        
            using (var context = new dierentuinContext(options))
            {
                var controller = new EnclosuresController(context);
        
                var result = await controller.Create(newEnclosure);
        
                var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            }
        
            using (var context = new dierentuinContext(options))
            {
                Assert.True(context.Enclosure.Any(e => e.Name == "New Enclosure" && e.EnclosureSize == 123));
            }
        }

        // POST
        [Fact]
        public async Task PostEnclosure_UpdatesEnclosure_ReturnsRedirectToActionResult()
        {
            var options = GetDbContextOptions("TestDB_PutEnclosure");
            var testEnclosureId = 1;
        
            using (var context = new dierentuinContext(options))
            {
                // Ensure an enclosure exists before attempting to update
                var existingEnclosure = await context.Enclosure.FindAsync(testEnclosureId);
                if (existingEnclosure == null)
                {
                    context.Enclosure.Add(new Enclosure { Id = testEnclosureId, Name = "Initial Enclosure", EnclosureSize = 100 });
                    await context.SaveChangesAsync();
                }
            }
        
            using (var context = new dierentuinContext(options))
            {
                // Retrieve the existing enclosure and update its properties
                var enclosureToUpdate = await context.Enclosure.FindAsync(testEnclosureId);
                Assert.NotNull(enclosureToUpdate);
                enclosureToUpdate.Name = "Updated Enclosure";
                enclosureToUpdate.EnclosureSize = 123;
        
                var controller = new EnclosuresController(context);
                var result = await controller.Edit(testEnclosureId, enclosureToUpdate);
        
                var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
                Assert.Equal("Index", redirectToActionResult.ActionName);
            }
        
            using (var context = new dierentuinContext(options))
            {
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
            var options = AnimalTests.GetDbContextOptions("TestDB_DeleteEnclosure");
            var testEnclosureId = 1;
        
            using (var context = new dierentuinContext(options))
            {
                var controller = new EnclosuresController(context);
        
                var result = await controller.DeleteConfirmed(testEnclosureId);
        
                var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
                Assert.Equal(nameof(EnclosuresController.Index), redirectToActionResult.ActionName);
            }
        
            using (var context = new dierentuinContext(options))
            {
                Assert.False(context.Enclosure.Any(e => e.Id == testEnclosureId));
            }
        }
    }
}