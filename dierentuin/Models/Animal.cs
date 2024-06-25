namespace dierentuin.Models
{
    public class Animal
    {
        public int id { get; set; }
        public string name { get; set; }
        public string species { get; set; }
        public string category { get; set; }
        public enum size;
        public enum dietaryClass;
        public string prey { get; set; }
        public string enclosure { get; set; }
        public double spaceRequirement { get; set; }
        public enum securityRequirement;
    }
}
