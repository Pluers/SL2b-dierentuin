using System.Collections.ObjectModel;

namespace dierentuin.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Collection<Animal>? Animals { get; set; }
    }
}
