namespace dierentuin.Enums
{
    public enum SecurityClassification
    {
        Low,
        Medium,
        High
    }

    // To access the AnimalSize enum in both the api and the model
    public enum AnimalSize
    {
        Microscopic,
        VerySmall,
        Small,
        Medium = 5,
        Large = 10,
        VeryLarge = 20
    }
}