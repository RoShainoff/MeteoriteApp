namespace MeteoriteApp.Server.BLL.Helpers
{
    public static class EnumerableHelper
    {
        public static IEnumerable<T> ToPaged<T>(this IEnumerable<T> values, int pageNumber, int pageSize)
        {
            if (pageNumber <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(pageNumber));
            }

            if (pageSize <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(pageSize));
            }

            return values.Skip((pageNumber - 1) * pageSize).Take(pageSize);
        }

        public static IQueryable<T> ToPaged<T>(this IQueryable<T> values, int pageNumber, int pageSize)
        {
            if (pageNumber <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(pageNumber));
            }

            if (pageSize <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(pageSize));
            }

            return values.Skip((pageNumber - 1) * pageSize).Take(pageSize);
        }
    }
}
