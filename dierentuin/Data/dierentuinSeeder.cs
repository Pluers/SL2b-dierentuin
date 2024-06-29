using dierentuin.Models;
using dierentuin.Enums;

using Bogus;

namespace dierentuin.Data
{
    public class dierentuinSeeder
    {
        private readonly dierentuinContext _context;

        // Reference to the dbcontext
        public dierentuinSeeder(dierentuinContext context)
        {
            _context = context;
        }

        public void DataSeeder()
        {
            // Create the database if it doesn't exist
            _context.Database.EnsureCreated();

            if (!_context.Animal.Any())
            {
                // Create a list of categories
                var categoryNames = new[] { "Mammals", "Birds", "Reptiles", "Fish", "Amphibians", "Insects", "Arachnids" };
                var categories = categoryNames.Select(name => new Category { Name = name }).ToList();
                List<Category> usedCategories = new List<Category>();

                // Add the categories to the database
                _context.Category.AddRange(categories);
                _context.SaveChanges();

                // Create a list of enclosures
                var enclosureFaker = new Faker<Enclosure>()
                    .RuleFor(e => e.Name, f =>
                    {
                        // Pick a random category that hasn't been used yet
                        Category category;
                        do
                        {
                            category = f.PickRandom(categories);
                        } while (usedCategories.Contains(category));
                        usedCategories.Add(category);
                        return $"{category.Name} Enclosure";
                    })
                    .RuleFor(e => e.Climate, f => f.PickRandom<EnclosureClimateType>())
                    .RuleFor(e => e.HabitatType, f => f.PickRandom<EnclosureHabitatEnvironment>())
                    .RuleFor(e => e.SecurityLevel, f => f.PickRandom<SecurityClassification>())
                    // Enclosure size is random
                    .RuleFor(e => e.EnclosureSize, f => f.Random.Number(20, 200));

                // Generate the enclosures
                var enclosures = enclosureFaker.Generate(5);
                _context.Enclosure.AddRange(enclosures);
                _context.SaveChanges();

                // Create a list of animals
                var animalFaker = new Faker<Animal>()
                    .RuleFor(a => a.Name, f => f.Name.FirstName())
                    .RuleFor(a => a.Species, f => f.Lorem.Word())
                    .RuleFor(a => a.Size, f => f.PickRandom<AnimalSize>())
                    .RuleFor(a => a.DietaryClass, f => f.PickRandom<AnimalDietaryClass>())
                    .RuleFor(a => a.ActivityPattern, f => f.PickRandom<AnimalActivityPattern>())
                    .RuleFor(a => a.Category, f => f.PickRandom(categories))
                    .RuleFor(a => a.Enclosure, f => f.PickRandom(enclosures))
                    .RuleFor(a => a.SpaceRequirement, f => f.Random.Number(1, 20))
                    .RuleFor(a => a.SecurityRequirement, f => f.PickRandom<SecurityClassification>())
                    .RuleFor(a => a.Enclosure, f => f.PickRandom(enclosures));
                var animals = animalFaker.Generate(20);
                var random = new Random();

                // Add the animals to the database
                _context.Animal.AddRange(animals);
                _context.SaveChanges();

                // Assign a random prey to each animal
                // The prey is an animal reference inside an animal model
                foreach (var animal in animals)
                {
                    var potentialPrey = animals.Where(a => a.Id != animal.Id).ToList();
                    if (potentialPrey.Any())
                    {
                        int randomIndex = random.Next(potentialPrey.Count);
                        animal.PreyId = potentialPrey[randomIndex].Id;
                    }
                }

                // Update the Animals
                _context.Animal.UpdateRange(animals);
                _context.SaveChanges();
            }
        }
    }
}