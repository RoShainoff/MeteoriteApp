using MeteoriteApp.Server.DAL.Models;

namespace MeteoriteApp.Server.BLL.Models.DTO
{
    public class MeteoriteDto
    {
        public string Name { get; set; }
        public string Id { get; set; }
        public string NameType { get; set; }
        public string RecClass { get; set; }
        public string? Mass { get; set; }
        public string Fall { get; set; }
        public DateTime Year { get; set; }
        public string RecLat { get; set; }
        public string RecLong { get; set; }
        public Geolocation? Geolocation { get; set; }
    }
}
