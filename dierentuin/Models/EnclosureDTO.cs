using System.Collections.ObjectModel;
using dierentuin.Enums;

namespace dierentuin.Models
{
    public class EnclosureDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<ForeignAnimalDTO>? Animals { get; set; }
        public EnclosureClimateType Climate { get; set; }
        public EnclosureHabitatEnvironment HabitatType { get; set; }
        public SecurityClassification SecurityLevel { get; set; }
        public double EnclosureSize { get; set; }
    }
}
