namespace dierentuin.Enums
{
    // Separated the enums from the models to keep the code clean and organized.
    public enum AnimalDietaryClass
    {
        Carnivore,
        Herbivore,
        Omnivore,
        Insectivore,
        Piscivore
    }
    public enum AnimalActivityPattern
    {
        Diurnal,
        Nocturnal,
        Cathemeral
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