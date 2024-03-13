namespace WebApi.ViewModels.Filters
{
    public class CarFilter : FilterBase
    {
        public int? BrandId { get; set; }
        public bool? BestSeller { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
    }
}
