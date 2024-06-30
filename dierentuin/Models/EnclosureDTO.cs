using System.Collections.ObjectModel;
using dierentuin.Enums;

namespace dierentuin.Models
{
    // Created a DTO model for the animal to control what information is send to and recieved from the API.
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
        // EnclosureSize is in square meters.
        public double EnclosureSize { get; set; }
    }
}
