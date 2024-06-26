using System.Collections.ObjectModel;
using dierentuin.Enums;
namespace dierentuin.Models
{
    public class Enclosure
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Collection<Animal>? Animals { get; set; }
        public ClimateType Climate;

        public HabitatEnvironment HabitatType;
        public SecurityClassification SecurityLevel;

        /// <summary>
        /// Size is in square meters.
        /// </summary>
        public double Size { get; set; }
    }

    [Flags]
    public enum HabitatEnvironment
    {
        Forest,
        Savannah,
        Desert,
        Tundra,
        Aquatic
    }
}
