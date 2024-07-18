using MeteoriteApp.Server.DAL.Models;

namespace MeteoriteApp.Server.BLL
{
    public interface IFetchMeteoriteClientService
    {
        Task<List<MeteoriteApi>> FetchMeteorites(CancellationToken cancellationToken);
    }
}
