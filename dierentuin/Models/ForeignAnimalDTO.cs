using dierentuin.Enums;

namespace dierentuin.Models
{
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
