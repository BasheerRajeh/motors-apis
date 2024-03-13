namespace WebApi.ViewModels.Filters
{
    public class FilterBase
    {
        public int Page { get; set; }
        public int PageSize { get; set; } = 10;
        public string? OrderBy { get; set; }
        public bool Desc { get; set; }
    }
}
