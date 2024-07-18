using MeteoriteApp.Server.BLL.Models;
using MeteoriteApp.Server.DAL.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace MeteoriteApp.Server.BLL
{
    public class FetchMeteoriteClientService(IHttpClientFactory _httpClientFactory, IOptions<MeteoriteFetchOptions> options) : IFetchMeteoriteClientService
    {
        private readonly string _apiUrl = options.Value.ApiUrl;

        public async Task<List<MeteoriteApi>> FetchMeteorites(CancellationToken cancellationToken)
        {
            using var httpClient = _httpClientFactory.CreateClient();

            using var response = await httpClient.GetAsync(_apiUrl, cancellationToken);
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync(cancellationToken);
                return JsonConvert.DeserializeObject<List<MeteoriteApi>>(data)!;
            }
            else
            {
                throw new HttpRequestException($"Ошибка при получении данных: {response.StatusCode}");
            }
        }
    }
}
