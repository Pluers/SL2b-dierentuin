using dierentuin.Enums;

namespace dierentuin.Models
{
    public class Animal
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Species { get; set; }
        public Category? Category { get; set; }
        public AnimalSize Size { get; set; }
        public AnimalDietaryClass DietaryClass { get; set; }
        public AnimalActivityPattern ActivityPattern { get; set; }
        public int CategoryId { get; set; }
        public int EnclosureId { get; set; }
        public string? Prey { get; set; }
        public Enclosure? Enclosure { get; set; }
        /// <summary>
        /// SpaceRequirement is in square meters.
        /// </summary>
        public double SpaceRequirement { get; set; }
        public SecurityClassification SecurityRequirement { get; set; }
    }

    public enum AnimalSize
    {
        Microscopic,
        VerySmall,
        Small,
        Medium,
        Large,
        VeryLarge
    }
    public enum AnimalDietaryClass
    {
        Carnivore,
        Herbivore,
        Omnivore,
        Insectivore,
        Piscivore
    }
    public enum AnimalActivityPattern
    {
        Diurnal,
        Nocturnal,
        Cathemeral
    }
}
