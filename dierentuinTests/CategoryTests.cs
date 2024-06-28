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
            var options = new DbContextOptionsBuilder<dierentuinContext>()
                .UseInMemoryDatabase(databaseName)
                .ConfigureWarnings(warnings => 
                    warnings.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                .Options;
        
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
            var options = GetDbContextOptions("TestDB_AddCategory");
            var newCategory = new Category { Name = "Reptiles" };

            using (var context = new dierentuinContext(options))
            {
                var controller = new CategoriesController(context);

                var result = await controller.Create(newCategory);

                var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            }

            using (var context = new dierentuinContext(options))
            {
                var category = await context.Category.FirstOrDefaultAsync(c => c.Name == "Reptiles");
                Assert.NotNull(category);
            }
        }

        [Fact]
        public async Task PostCategory_UpdatesCategory_ReturnsRedirectToActionResult()
        {
            var options = GetDbContextOptions("TestDB_PutCategory");
            var testCategoryId = 1;
        
            using (var context = new dierentuinContext(options))
            {
                var existingCategory = await context.Category.FindAsync(testCategoryId);
                if (existingCategory == null)
                {
                    context.Category.Add(new Category { Id = testCategoryId, Name = "Initial Category" });
                    await context.SaveChangesAsync();
                }
            }
        
            using (var context = new dierentuinContext(options))
            {
                var categoryToUpdate = await context.Category.FindAsync(testCategoryId);
                Assert.NotNull(categoryToUpdate);
                categoryToUpdate.Name = "Updated Category";
        
                var controller = new CategoriesController(context);
                var result = await controller.Edit(testCategoryId, categoryToUpdate);
        
                var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
                Assert.Equal("Index", redirectToActionResult.ActionName);
            }
        
            using (var context = new dierentuinContext(options))
            {
                var updatedCategory = await context.Category.FindAsync(testCategoryId);
                Assert.NotNull(updatedCategory);
                Assert.Equal("Updated Category", updatedCategory.Name);
            }
        }

        [Fact]
        public async Task DeleteCategory_RemovesCategory_ReturnsRedirectToActionResult()
        {
            var options = GetDbContextOptions("TestDB_DeleteCategory");
            var testCategoryId = 1;
        
            using (var context = new dierentuinContext(options))
            {
                var controller = new CategoriesController(context);
        
                var result = await controller.DeleteConfirmed(testCategoryId);
        
                var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
                Assert.Equal(nameof(CategoriesController.Index), redirectToActionResult.ActionName);
            }
        
            using (var context = new dierentuinContext(options))
            {
                Assert.False(context.Category.Any(c => c.Id == testCategoryId));
            }
        }
    }
}