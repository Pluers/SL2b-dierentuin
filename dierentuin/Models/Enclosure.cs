namespace dierentuin.Models
{
    public class Enclosure
    {
        public int id { get; set; }
        public string name { get; set; }
        public string Animals { get; set; }
        public enum Climate;

        [Flags]
        public enum habitatType;
        public enum securityLevel;
        public double Size { get; set; }


    }
}
