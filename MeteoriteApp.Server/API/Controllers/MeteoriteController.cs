using MeteoriteApp.Server.BLL.Models.API;
using MeteoriteApp.Server.BLL.Services;
using Microsoft.AspNetCore.Mvc;

namespace MeteoriteApp.Server.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MeteoriteController(IMeteoriteService _meteoriteService) : ControllerBase
    {
        [HttpGet("pagedFilteredGroups")]
        public async Task<IActionResult> GetPagedFilteredGroupedMeteoritesData([FromQuery] MeteoriteGroupFilter filter, int pageNumber = 1, int pageSize = 25)
        {
            var result = await _meteoriteService.GetFilteredGroupedDataAsync(filter, pageNumber, pageSize);
            return Ok(result);
        }

        [HttpGet("years")]
        public async Task<IActionResult> GetYears()
        {
            var years = await _meteoriteService.GetDistinctYearsAsync();
            return Ok(years);
        }

        [HttpGet("classes")]
        public async Task<IActionResult> GetClasses()
        {
            var classes = await _meteoriteService.GetDistinctClassesAsync();
            return Ok(classes);
        }
    }
}
