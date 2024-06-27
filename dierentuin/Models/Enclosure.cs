﻿using System.Collections.ObjectModel;
using dierentuin.Enums;

namespace dierentuin.Models
{
    public class Enclosure
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public EnclosureClimateType Climate { get; set; }
        public EnclosureHabitatEnvironment HabitatType { get; set; }
        public SecurityClassification SecurityLevel { get; set; }

        /// <summary>
        /// Size is in square meters.
        /// </summary>
        public double EnclosureSize { get; set; }
    }

    [Flags]
    public enum EnclosureHabitatEnvironment
    {
        Forest,
        Savannah,
        Desert,
        Tundra,
        Aquatic
    }
}
