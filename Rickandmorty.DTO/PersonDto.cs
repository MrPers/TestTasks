namespace Rickandmorty.DTO
{
    public class PersonFullInformationDto 
    {
        public string Name { get; set; }
        public string Status { get; set; }
        public string Species { get; set; }
        public string Type { get; set; }
        public string Gender { get; set; }
        public List<string> Episode { get; set; }
        public LocationDto Location { get; set; }
    }
}