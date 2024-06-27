using System.Collections.ObjectModel;

namespace dierentuin.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Animal>? Animals { get; set; }
    }
}
