using System.Collections.ObjectModel;
using dierentuin.Enums;

namespace dierentuin.Models
{
    public class EnclosureDTO
    {
        public int Id { get; set; }
        // Name of the enclosure cannot be null or empty.
        public string Name { get; set; }
        // List of animals in the enclosure.
        public ICollection<ForeignAnimalDTO>? Animals { get; set; }
        public EnclosureClimateType Climate { get; set; }
        public EnclosureHabitatEnvironment HabitatType { get; set; }
        public SecurityClassification SecurityLevel { get; set; }
        /// <summary>
        /// Size is in square meters.
        /// </summary>
        public double EnclosureSize { get; set; }
    }
}
