using MeteoriteApp.Server.BLL.Models;
using MeteoriteApp.Server.BLL.Services;
using Microsoft.Extensions.Options;

namespace MeteoriteApp.Server.HostedServices
{
    public class MeteoriteFetchService : BackgroundService
    {
        private readonly ILogger<MeteoriteFetchService> _logger;
        private readonly IMeteoriteService _meteoriteService;
        private readonly MeteoriteFetchOptions _options;

        public MeteoriteFetchService(
            ILogger<MeteoriteFetchService> logger,
            IMeteoriteService meteoriteService,
            IHttpClientFactory httpClientFactory,
            IOptions<MeteoriteFetchOptions> options)
        {
            _logger = logger;
            _meteoriteService = meteoriteService;
            _options = options.Value;
        }

        /// <summary>
        /// Handler
        /// </summary>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Task</returns>
        protected override Task ExecuteAsync(CancellationToken cancellationToken)
        {
            Task.Run(async () => await DoWork(cancellationToken), cancellationToken);
            return Task.CompletedTask;
        }

        private async Task DoWork(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Timed Hosted Service running.");

            try
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    await Do(cancellationToken);
                }
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("Timed Hosted Service is stopping.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while running hosted service");
            }
        }

        private async Task Do(CancellationToken cancellationToken)
        {
            try
            {
                try
                {
                    await _meteoriteService.FetchAndSaveDataAsync(cancellationToken);
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "Error while running meteorites fetching and save");
                }

                await Task.Delay(_options.FetchInterval, cancellationToken);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error processing meteorites");
            }
        }
    }

}
