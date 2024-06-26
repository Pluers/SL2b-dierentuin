using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using dierentuin.Models;

namespace dierentuin.Data
{
    public class dierentuinContext : DbContext
    {
        public dierentuinContext(DbContextOptions<dierentuinContext> options) : base(options) { }

        // List of models
        public DbSet<Animal> Animal { get; set; } = default!;
        public DbSet<Category> Category { get; set; } = default!;
        public DbSet<Enclosure> Enclosure { get; set; } = default!;
    }
}
