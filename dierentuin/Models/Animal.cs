using dierentuin.Enums;

namespace dierentuin.Models
{
    public class Animal
    {
        public int Id { get; set; }
        // The name cannot be null or empty.
        public string Name { get; set; }
        // The species cannot be null or empty.
        public string Species { get; set; }
        public Category? Category { get; set; }
        public AnimalSize Size { get; set; }
        public AnimalDietaryClass DietaryClass { get; set; }
        public AnimalActivityPattern ActivityPattern { get; set; }
        public int? CategoryId { get; set; }
        public int? EnclosureId { get; set; }
        public int? PreyId { get; set; }
        public virtual Animal? Prey { get; set; }
        public Enclosure? Enclosure { get; set; }
        // SpaceRequirement is in square meters.
        public double SpaceRequirement { get; set; }
        public SecurityClassification? SecurityRequirement { get; set; }
    }
}
