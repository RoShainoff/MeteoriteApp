namespace MeteoriteApp.Server.BLL.Services
{
    using MeteoriteApp.Server.BLL;
    using MeteoriteApp.Server.BLL.Helpers;
    using MeteoriteApp.Server.BLL.Models.API;
    using MeteoriteApp.Server.DAL.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Caching.Memory;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class MeteoriteService(IDbContextFactory<MeteoriteContext> _context,
        IMemoryCache _cache,
        IFetchMeteoriteClientService _fetchMeteoriteClient,
        IConfiguration configuration) : IMeteoriteService
    {
        private readonly TimeSpan _cacheDuration = TimeSpan.FromMilliseconds(configuration.GetValue<double>("CacheDuration"));
        
        public async Task<List<MeteoriteApi>> FetchDataAsync(CancellationToken cancellationToken) =>
            await _fetchMeteoriteClient.FetchMeteorites(cancellationToken);

        public async Task FetchAndSaveDataAsync(CancellationToken cancellationToken)
        {
            var meteorites = await FetchDataAsync(cancellationToken);
            await SaveDataAsync(meteorites);
        }

        public async Task SaveDataAsync(IEnumerable<Meteorite> meteorites)
        {
            using var context = _context.CreateDbContext();
            await context.Meteorites.UpsertRange(meteorites)
                .On(p => p.Id)
                .AllowIdentityMatch()
                .RunAsync();
            await context.SaveChangesAsync();
        }

        public async Task SaveDataAsync(IEnumerable<MeteoriteApi> meteorites) =>
            await SaveDataAsync(meteorites.Select(m => m.ToMeteorite()));

        public async Task<PagedResponse<MeteoriteGroup>> GetFilteredGroupedDataAsync(MeteoriteGroupFilter filter, int pageNumber, int pageSize)
        {
            string cacheKey = filter.GetCacheKey(pageNumber, pageSize);

            if (_cache.TryGetValue(cacheKey, out List<MeteoriteGroup> cachedData))
            {
                return cachedData!.ToPagedResponse();
            }

            using var context = _context.CreateDbContext();

            var query = context.Meteorites.AsQueryable();

            if (filter.StartYear.HasValue)
            {
                query = query.Where(m => m.Year.Year >= filter.StartYear.Value);
            }

            if (filter.EndYear.HasValue)
            {
                query = query.Where(m => m.Year.Year <= filter.EndYear.Value);
            }

            if (!string.IsNullOrEmpty(filter.RecClass))
            {
                query = query.Where(m => m.RecClass == filter.RecClass);
            }

            if (!string.IsNullOrEmpty(filter.Name))
            {
                query = query.Where(m => m.Name.Contains(filter.Name));
            }

            var groupedQuery = query
                .GroupBy(m => m.Year.Year)
                .Select(g => new MeteoriteGroup
                {
                    Year = g.Key,
                    Count = g.Count(),
                    TotalMass = g.Sum(m => m.Mass ?? 0d)
                });

            switch (filter.SortField)
            {
                case MeteoriteGroupSortField.Count:
                    if(filter.SortOrder == SortOrder.Asc)
                    {
                        groupedQuery = groupedQuery.OrderBy(g => g.Count);
                    }
                    else
                    {
                        groupedQuery = groupedQuery.OrderByDescending(g => g.Count);
                    }
                    break;
                case MeteoriteGroupSortField.TotalMass:
                    if (filter.SortOrder == SortOrder.Asc)
                    {
                        groupedQuery = groupedQuery.OrderBy(g => g.TotalMass);
                    }
                    else
                    {
                        groupedQuery = groupedQuery.OrderByDescending(g => g.TotalMass);
                    }
                    break;
                case MeteoriteGroupSortField.Year:
                default:
                    if (filter.SortOrder == SortOrder.Asc)
                    {
                        groupedQuery = groupedQuery.OrderBy(g => g.Year);
                    }
                    else
                    {
                        groupedQuery = groupedQuery.OrderByDescending(g => g.Year);
                    }
                    break;
            }

            var totalCount = await groupedQuery.CountAsync();

            if (pageNumber > 0 && pageSize > 0)
            {
                groupedQuery = groupedQuery.ToPaged(pageNumber, pageSize);
            }
                        
            cachedData = await groupedQuery.ToListAsync();

            _cache.Set(cacheKey, cachedData, _cacheDuration);
            
            return cachedData!.ToPagedResponse(totalCount);
        }

        public async Task<List<int>> GetDistinctYearsAsync()
        {
            using var context = _context.CreateDbContext();
            var years = await context.Meteorites.Select(m => m.Year.Year).OrderBy(y => y).Distinct().ToListAsync();

            return years;
        }

        public async Task<List<string>> GetDistinctClassesAsync()
        {
            using var context = _context.CreateDbContext();
            var classes = await context.Meteorites.Select(m => m.RecClass).OrderBy(c => c).Distinct().ToListAsync();

            return classes;
        }
    }

}
