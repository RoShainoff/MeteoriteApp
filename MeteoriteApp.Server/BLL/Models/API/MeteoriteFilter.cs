using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MeteoriteApp.Server.BLL.Models.API
{
    public class MeteoriteGroupFilter
    {
        [Range(1, 2100)]
        [JsonPropertyName("startYear")]
        public int? StartYear { get; set; }

        [Range(1, 2100)]
        [JsonPropertyName("endYear")]
        public int? EndYear { get; set; }

        [JsonPropertyName("recClass")]
        public string? RecClass { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("orderBy")]
        public MeteoriteGroupSortField SortField { get; set; }

        [JsonPropertyName("orderByDirection")]
        public SortOrder SortOrder { get; set; }
    }

}
