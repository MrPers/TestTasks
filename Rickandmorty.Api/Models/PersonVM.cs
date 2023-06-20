namespace Rickandmorty.Api.Models
{
    public class PersonVM 
    {
        public string Name { get; set; }
        public string Status { get; set; }
        public string Species { get; set; }
        public string Type { get; set; }
        public string Gender { get; set; }
        public LocationVM Origin { get; set; }
    }
}