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
        public dierentuinContext (DbContextOptions<dierentuinContext> options)
            : base(options)
        {
        }

        public DbSet<dierentuin.Models.Animal> Animal { get; set; } = default!;
        public DbSet<dierentuin.Models.Category> Category { get; set; } = default!;
        public DbSet<dierentuin.Models.Enclosure> Enclosure { get; set; } = default!;
    }
}
