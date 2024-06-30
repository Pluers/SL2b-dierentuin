using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Xunit;
using dierentuin.Controllers;
using dierentuin.Data;
using dierentuin.Models;

namespace dierentuinTests
{
    public class CategoryTests
    {
        public static DbContextOptions<dierentuinContext> GetDbContextOptions(string databaseName)
        {
            // The reference to the dbcontext
            var options = new DbContextOptionsBuilder<dierentuinContext>()
                .UseInMemoryDatabase(databaseName)
                .ConfigureWarnings(warnings =>
                    warnings.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                .Options;

            // Add a category if it doesn't exist yet in the in-memory database
            using (var context = new dierentuinContext(options))
            {
                if (!context.Category.Any())
                {
                    context.Category.Add(new Category { Name = "Mammals" });
                    context.SaveChanges();
                }
            }

            return options;
        }

        [Fact]
        public async Task Index_ReturnsAViewResult_WithAListOfCategories()
        {
            // Get all categories in the in-memory database
            var options = GetDbContextOptions("TestDB_IndexTest_Category");

            using (var context = new dierentuinContext(options))
            {
                var controller = new CategoriesController(context);

                var result = await controller.Index();

                var viewResult = Assert.IsType<ViewResult>(result);
                var model = Assert.IsAssignableFrom<IEnumerable<Category>>(viewResult.Model);
            }
        }

        [Fact]
        public async Task AddCategory_CreatesNewCategory_ReturnsRedirectToActionResult()
        {
            // Create a new category in the in-memory database
            var options = GetDbContextOptions("TestDB_AddCategory");
            var newCategory = new Category { Name = "Reptiles" };

            using (var context = new dierentuinContext(options))
            {
                var controller = new CategoriesController(context);

                var result = await controller.Create(newCategory);

                var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);

                // Check if the category was added to the in-memory database
                var category = await context.Category.FirstOrDefaultAsync(c => c.Name == "Reptiles");
                Assert.NotNull(category);
            }
        }

        [Fact]
        public async Task PostCategory_UpdatesCategory_ReturnsRedirectToActionResult()
        {
            // Update a category in the in-memory database
            var options = GetDbContextOptions("TestDB_PutCategory");
            var testCategoryId = 1;

            using (var context = new dierentuinContext(options))
            {
                var existingCategory = await context.Category.FindAsync(testCategoryId);
                if (existingCategory == null)
                {
                    // Add a category if it doesn't exist yet in the in-memory database
                    context.Category.Add(new Category { Id = testCategoryId, Name = "Initial Category" });
                    await context.SaveChangesAsync();
                }



                // Get the existing category and update it
                var categoryToUpdate = await context.Category.FindAsync(testCategoryId);
                Assert.NotNull(categoryToUpdate);
                categoryToUpdate.Name = "Updated Category";

                var controller = new CategoriesController(context);
                var result = await controller.Edit(testCategoryId, categoryToUpdate);

                var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
                Assert.Equal("Index", redirectToActionResult.ActionName);



                // Check if the category was updated in the in-memory database
                var updatedCategory = await context.Category.FindAsync(testCategoryId);
                Assert.NotNull(updatedCategory);
                Assert.Equal("Updated Category", updatedCategory.Name);
            }
        }

        [Fact]
        public async Task DeleteCategory_RemovesCategory_ReturnsRedirectToActionResult()
        {
            // Remove a category from the in-memory database
            var options = GetDbContextOptions("TestDB_DeleteCategory");
            var testCategoryId = 1;

            using (var context = new dierentuinContext(options))
            {
                var controller = new CategoriesController(context);

                var result = await controller.DeleteConfirmed(testCategoryId);

                // Check if the category was removed from the in-memory database
                var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
                Assert.Equal(nameof(CategoriesController.Index), redirectToActionResult.ActionName);
                Assert.False(context.Category.Any(c => c.Id == testCategoryId));
            }
        }
    }
}