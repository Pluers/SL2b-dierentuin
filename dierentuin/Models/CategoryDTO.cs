using System.Collections.ObjectModel;

namespace dierentuin.Models
{
    public class CategoryDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<ForeignAnimalDTO>? Animals { get; set; }
    }
}
