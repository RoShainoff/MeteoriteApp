using MeteoriteApp.Server.BLL.Models.API;
using MeteoriteApp.Server.BLL.Models.DTO;
using MeteoriteApp.Server.DAL.Models;
using Microsoft.Extensions.Primitives;
using System.Globalization;
using System.Text;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace MeteoriteApp.Server.BLL.Helpers
{
    public static class MeteoritesHelper
    {
        public static Meteorite ToMeteorite(this MeteoriteApi meteoriteApi) =>
            new()
            {
                Id = Convert.ToInt32(meteoriteApi.Id),
                Name = meteoriteApi.Name,
                Fall = meteoriteApi.Fall,
                Mass = !string.IsNullOrEmpty(meteoriteApi.Mass) ?
                    double.Parse(meteoriteApi.Mass, CultureInfo.InvariantCulture) :
                    null,
                RecClass = meteoriteApi.RecClass,
                Year = meteoriteApi.Year,
                Longitude = meteoriteApi?.Geolocation?.Coordinates[0],
                Latitude = meteoriteApi?.Geolocation?.Coordinates[1]
            };

        public static MeteoriteDto ToMeteorite(this Meteorite meteorite) =>
            new()
            {
                Id = meteorite.Id.ToString(),
                Name = meteorite.Name,
                Fall = meteorite.Fall,
                Mass = meteorite.Mass?.ToString(),
                RecClass = meteorite.RecClass,
                RecLat = meteorite.Latitude?.ToString(),
                RecLong = meteorite.Longitude?.ToString(),
            };

        public static string GetCacheKey (this MeteoriteGroupFilter meteoriteFilter, int? pageNumber, int? pageSize)
        {
            var sb = new StringBuilder(nameof(MeteoriteGroupFilter));

            if (meteoriteFilter.StartYear.HasValue)
            {
                sb.Append($"_StartYear: {meteoriteFilter.StartYear}");
            }

            if (meteoriteFilter.EndYear.HasValue)
            {
                sb.Append($"_EndYear: {meteoriteFilter.EndYear}");
            }

            if (!string.IsNullOrEmpty(meteoriteFilter.RecClass))
            {
                sb.Append($"_RecClass: {meteoriteFilter.RecClass}");
            }

            if (!string.IsNullOrEmpty(meteoriteFilter.Name))
            {
                sb.Append($"_Name: {meteoriteFilter.Name}");
            }

            sb.Append($"_SortField: {meteoriteFilter.SortField}");

            sb.Append($"_SortOrder: {meteoriteFilter.SortOrder}");

            if (pageNumber.HasValue && pageNumber > 0) 
            {
                sb.Append($"_PageNumber: {pageNumber}");
            }

            if (pageSize.HasValue && pageSize > 0) 
            {
                sb.Append($"_PageSize: {pageSize}");
            }

            return sb.ToString();
        }
    }
}
