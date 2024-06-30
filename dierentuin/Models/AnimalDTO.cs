using dierentuin.Enums;

namespace dierentuin.Models
{
    public class AnimalDTO
    {
        public int Id { get; set; }
        // Name of the animal cannot be null or empty.
        public string Name { get; set; }
        // Species of the animal cannot be null or empty.
        public string Species { get; set; }
        public AnimalSize Size { get; set; }
        public AnimalDietaryClass DietaryClass { get; set; }
        public AnimalActivityPattern ActivityPattern { get; set; }
        public int? CategoryId { get; set; }
        public int? EnclosureId { get; set; }
        public int? PreyId { get; set; }
        // SpaceRequirement is in square meters.
        public double SpaceRequirement { get; set; }
        public SecurityClassification? SecurityRequirement { get; set; }
    }
}
