using MeteoriteApp.Server.BLL.Models.API;

namespace MeteoriteApp.Server.BLL.Helpers
{
    public static class ResponseHelper
    {
        public static PagedResponse<T> ToPagedResponse<T>(this IEnumerable<T> collection, int totalCount = 0) where T : class =>
            new()
            {
                Items = collection,
                TotalCount = totalCount
            };
    }
}
