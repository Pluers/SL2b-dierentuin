using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using dierentuin.Controllers.API;
using dierentuin.Data;
using dierentuin.Models;

namespace dierentuinTests
{  
    // public class AnimalApiTests
    // {
    //     private readonly Mock<dierentuinContext> _mockContext;
    //     private readonly Mock<DbSet<Animal>> _mockDbSet;
    //     private readonly List<Animal> _animals;

    //     public AnimalApiTests()
    //     {
    //         _mockContext = new Mock<dierentuinContext>();
    //         _mockDbSet = new Mock<DbSet<Animal>>();
    //         _animals = new List<Animal>
    //         {
    //             new Animal { Id = 1, Name = "Lion" },
    //             new Animal { Id = 2, Name = "Tiger" }
    //         };

    //         var queryable = _animals.AsQueryable();
    //         _mockDbSet.As<IQueryable<Animal>>().Setup(m => m.Provider).Returns(queryable.Provider);
    //         _mockDbSet.As<IQueryable<Animal>>().Setup(m => m.Expression).Returns(queryable.Expression);
    //         _mockDbSet.As<IQueryable<Animal>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
    //         _mockDbSet.As<IQueryable<Animal>>().Setup(m => m.GetEnumerator()).Returns(() => queryable.GetEnumerator());
    //         _mockContext.Setup(c => c.Animal).Returns(_mockDbSet.Object);
    //     }

    //     [Fact]
    //     public async Task GetAnimal_ReturnsAllAnimals()
    //     {
    //         var controller = new AnimalsController(_mockContext.Object);
    //         var result = await controller.GetAnimal();

    //         var okResult = Assert.IsType<OkObjectResult>(result.Result);
    //         var returnValue = Assert.IsType<List<AnimalDTO>>(okResult.Value);
    //         Assert.Equal(2, returnValue.Count);
    //     }

    //     [Fact]
    //     public async Task GetAnimal_ById_ReturnsCorrectAnimal()
    //     {
    //         _mockDbSet.Setup(m => m.FindAsync(1)).ReturnsAsync(_animals[0]);
    //         var controller = new AnimalsController(_mockContext.Object);
    //         var result = await controller.GetAnimal(1);

    //         var okResult = Assert.IsType<ActionResult<Animal>>(result);
    //         var animal = Assert.IsType<Animal>(okResult.Value);
    //         Assert.Equal("Lion", animal.Name);
    //     }
    // }
}