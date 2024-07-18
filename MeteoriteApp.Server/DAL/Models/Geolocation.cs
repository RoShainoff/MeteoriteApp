namespace MeteoriteApp.Server.DAL.Models
{
    public class Geolocation
    {
        public string Type { get; set; }
        public List<double> Coordinates { get; set; }
    }
}
