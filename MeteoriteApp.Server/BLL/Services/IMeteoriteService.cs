using MeteoriteApp.Server.BLL.Models.API;
using MeteoriteApp.Server.BLL.Models.DTO;
using MeteoriteApp.Server.DAL.Models;

namespace MeteoriteApp.Server.BLL.Services
{
    public interface IMeteoriteService
    {
        Task<List<MeteoriteApi>> FetchDataAsync(CancellationToken cancellationToken);
        Task FetchAndSaveDataAsync(CancellationToken cancellationToken);
        Task SaveDataAsync(IEnumerable<Meteorite> meteorites);
        Task SaveDataAsync(IEnumerable<MeteoriteApi> meteorites);
        Task<PagedResponse<MeteoriteGroup>> GetFilteredGroupedDataAsync(MeteoriteGroupFilter filter, int pageNumber, int pageSize);
        Task<List<int>> GetDistinctYearsAsync();
        Task<List<string>> GetDistinctClassesAsync();
    }
}
