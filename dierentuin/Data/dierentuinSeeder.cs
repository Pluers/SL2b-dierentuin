using dierentuin.Models;
using dierentuin.Enums;

using Bogus;

namespace dierentuin.Data
{
    public class dierentuinSeeder
    {
        private readonly dierentuinContext _context;

        public dierentuinSeeder(dierentuinContext context)
        {
            _context = context;
        }

        public void DataSeeder()
        {
            _context.Database.EnsureCreated();

            if (!_context.Animal.Any())
            {
                var categoryFaker = new Faker<Category>()
                    .RuleFor(c => c.Name, f => f.PickRandom(new[] { "Mammals", "Birds", "Reptiles", "Fish", "Amphibians" }));

                var categories = categoryFaker.Generate(5);
                _context.Category.AddRange(categories);

                var enclosureFaker = new Faker<Enclosure>()
                    .RuleFor(e => e.Name, f => $"{f.PickRandom(categories).Name} Enclosure")
                    .RuleFor(e => e.Climate, f => f.PickRandom<EnclosureClimateType>())
                    .RuleFor(e => e.HabitatType, f => f.PickRandom<EnclosureHabitatEnvironment>())
                    .RuleFor(e => e.SecurityLevel, f => f.PickRandom<SecurityClassification>())
                    .RuleFor(e => e.EnclosureSize, f => f.Random.Number(20, 200));

                var enclosures = enclosureFaker.Generate(3);
                _context.Enclosure.AddRange(enclosures);

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
                var animals = animalFaker.Generate(10);
                var random = new Random(); 

                _context.Animal.AddRange(animals);
                _context.SaveChanges();
                foreach (var animal in animals)
                {
                    var potentialPrey = animals.Where(a => a.Id != animal.Id).ToList();
                    if (potentialPrey.Any())
                    {
                        int randomIndex = random.Next(potentialPrey.Count);
                        animal.PreyId = potentialPrey[randomIndex].Id;
                    }
                }

                _context.Animal.UpdateRange(animals);  
                _context.SaveChanges();
            }
        }
    }
}