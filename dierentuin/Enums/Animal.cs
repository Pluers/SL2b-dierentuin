namespace dierentuin.Enums
{
    // Separated the enums from the models to keep the code clean and organized.
    // The dietary classifications of animals
    public enum AnimalDietaryClass
    {
        Carnivore,
        Herbivore,
        Omnivore,
        Insectivore,
        Piscivore
    }
    // The activity times for animals
    public enum AnimalActivityPattern
    {
        Diurnal,
        Nocturnal,
        Cathemeral
    }

    // Categorizes animals by size
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