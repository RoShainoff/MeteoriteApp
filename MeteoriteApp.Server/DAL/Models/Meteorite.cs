namespace MeteoriteApp.Server.DAL.Models
{
    public class Meteorite
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string RecClass { get; set; }
        public double? Mass { get; set; }
        public string Fall { get; set; }
        public DateTime Year { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
    }
}
