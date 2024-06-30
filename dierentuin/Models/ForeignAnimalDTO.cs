using dierentuin.Enums;

namespace dierentuin.Models
{
    // DTO to get the most basic information of an animal. 
    // This is mostly used when other models request information about an animal.
    public class ForeignAnimalDTO
    {
        public int Id { get; set; }
        // Name of the animal cannot be null or empty.
        public string Name { get; set; }
        public AnimalSize Size { get; set; }
        // List the name of the size of the animal for the api
        public string SizeName => Size.ToString();
    }
}
