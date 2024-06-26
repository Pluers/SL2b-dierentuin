using System.Diagnostics;
using dierentuin.Enums;

namespace dierentuin.Models
{
    public class Animal
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Species { get; set; }
        public Category? Category { get; set; }
        public Size Size { get; set; }
        public DietaryClass DietaryClass { get; set; }
        public ActivityPattern ActivityPattern { get; set; }
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

    public enum Size
    {
        Microscopic,
        VerySmall,
        Small,
        Medium,
        Large,
        VeryLarge
    }
    public enum DietaryClass
    {
        Carnivore,
        Herbivore,
        Omnivore,
        Insectivore,
        Piscivore
    }
    public enum ActivityPattern
    {
        Diurnal,
        Nocturnal,
        Cathemeral
    }
}
