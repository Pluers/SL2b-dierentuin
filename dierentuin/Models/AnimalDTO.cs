using dierentuin.Enums;

namespace dierentuin.Models
{
    public class AnimalDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Species { get; set; }
        public AnimalSize Size { get; set; }
        public AnimalDietaryClass DietaryClass { get; set; }
        public AnimalActivityPattern ActivityPattern { get; set; }
        public int? CategoryId { get; set; }
        public int? EnclosureId { get; set; }
        public int? PreyId { get; set; }
        public double SpaceRequirement { get; set; }
        public SecurityClassification? SecurityRequirement { get; set; }
    }
}
