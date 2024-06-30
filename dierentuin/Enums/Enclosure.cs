namespace dierentuin.Enums
{
    // Separated the enums from the models to keep the code clean and organized.
    public enum EnclosureClimateType
    {
        Tropical,
        Subtropical,
        Temperate,
        Subarctic,
        Arctic
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