using System.Collections.ObjectModel;

namespace dierentuin.Models
{
    // Created a DTO model for the animal to control what information is send to and recieved from the API.
    public class CategoryDTO
    {
        public int Id { get; set; }
        // The name of the category cannot be null or empty.
        public string Name { get; set; }
        public ICollection<ForeignAnimalDTO>? Animals { get; set; }
    }
}
