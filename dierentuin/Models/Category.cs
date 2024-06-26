using System.Collections.ObjectModel;

namespace dierentuin.Models
{
    public class Category
    {
        public int id { get; set; }
        public string name { get; set; }
        public Collection<Animal>? Animals { get; set; }
    }
}
