using WebApi.Services.Common;

namespace WebApi.ViewModels
{
    public class CarVms
    {
    }
    public class CarVm
    {
        public CarVm()
        {
            Images = new HashSet<FileToken>();
        }
        public int? Id { get; set; }
        public string? Name { get; set; }
        public string? Battery { get; set; }
        public string? BatteryKz { get; set; }
        public string? BatteryRu { get; set; }
        public string? Performance { get; set; }
        public string? PerformanceKz { get; set; }
        public string? PerformanceRu { get; set; }
        public string? Range { get; set; }
        public string? RangeKz { get; set; }
        public string? RangeRu { get; set; }
        public string? Efficiency { get; set; }
        public string? EfficiencyKz { get; set; }
        public string? EfficiencyRu { get; set; }
        public decimal BatteryPower { get; set; }
        public decimal RealRange { get; set; }
        public decimal TopSpeed { get; set; }
        public decimal EfficiencyVal { get; set; }
        public decimal Price { get; set; }
        public string? PriceCurrency { get; set; }
        public bool BestSeller { get; set; }
        public bool Featured { get; set; }
        public int BrandId { get; set; }
        public string? BrandName { get; set; }
        public bool Active { get; set; }
        public int Stars1 { get; set; }
        public int Stars2 { get; set; }
        public int Stars3 { get; set; }
        public int Stars4 { get; set; }
        public int Stars5 { get; set; }
        public decimal Rating
        {
            get
            {
                var sum = (decimal)Stars1 + Stars2 + Stars3 + Stars4 + Stars5;
                if (sum < 1) return 0;

                return (Stars1 + (Stars2 * 2) + (Stars3 * 3) + (Stars4 * 4) + (Stars5 * 5))
                    / sum;
            }
        }
        public int ReviewsCount
        {
            get
            {
                return Stars1 + Stars2 + Stars3 + Stars4 + Stars5;
            }
        }
        public string? ImagePath { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        //public CarBrandVm? Brand { get; set; }
        public ICollection<FileToken> Images { get; set; }
    }

    //public class CarBrandVm
    //{
    //    public int Id { get; set; }
    //    public string? Name { get; set; }
    //    public string? IconPath { get; set; }
    //}
    public class CarReviewVm
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Contact { get; set; }
        public string? Email { get; set; }
        public string? Message { get; set; }
        public string? UserIp { get; set; }
        public int CarId { get; set; }
        public int Stars { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }

    }

    public class TestimonialVm
    {
        public TestimonialVm()
        {
            Images = new HashSet<FileToken>();
        }
        public int? Id { get; set; }
        public string? Name { get; set; }
        public string? Title { get; set; }
        public string? Comments { get; set; }
        public string? ProfilePhoto { get; set; }
        public ICollection<FileToken> Images { get; set; }
        public bool Active { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
