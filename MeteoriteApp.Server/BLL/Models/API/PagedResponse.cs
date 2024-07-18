namespace MeteoriteApp.Server.BLL.Models.API
{
    public class PagedResponse<T> where T : class
    {
        public IEnumerable<T> Items { get; set; }
        public int TotalCount { get; set; }
    }
}
