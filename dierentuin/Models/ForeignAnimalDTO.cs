using dierentuin.Enums;

namespace dierentuin.Models
{
    public class ForeignAnimalDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public AnimalSize Size { get; set; }
        public string SizeName => Size.ToString();
    }
}
