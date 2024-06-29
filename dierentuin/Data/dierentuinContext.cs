using Microsoft.EntityFrameworkCore;
using dierentuin.Models;

namespace dierentuin.Data
{
    public class dierentuinContext : DbContext
    {
        public dierentuinContext(DbContextOptions<dierentuinContext> options) : base(options) { }

        // List of models
        public virtual DbSet<Animal> Animal { get; set; } = default!;
        public virtual DbSet<Category> Category { get; set; } = default!;
        public virtual DbSet<Enclosure> Enclosure { get; set; } = default!;
    }
}
