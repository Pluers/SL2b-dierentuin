namespace dierentuin.Enums
{
    // Separated the enums from the models to keep the code clean and organized.
    // Defines the climate types for enclosures
    public enum EnclosureClimateType
    {
        Tropical,
        Subtropical,
        Temperate,
        Subarctic,
        Arctic
    }

    // The [Flags] attribute allows bitwise combination of enum values.
    [Flags]
    // The different types of habitats of an enclosure
    public enum EnclosureHabitatEnvironment
    {
        Forest,
        Savannah,
        Desert,
        Tundra,
        Aquatic
    }
}