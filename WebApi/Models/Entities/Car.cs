using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace WebApi.Models.Entities
{
    public class Car
    {
        public Car()
        {
            Images = new HashSet<CarImage>();
            Reviews = new HashSet<CarReview>();
        }
        public int Id { get; set; }
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
        public string? ImagePath { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public CarBrand? Brand { get; set; }
        public ICollection<CarReview> Reviews { get; set; }
        public ICollection<CarImage> Images { get; set; }
    }

    public class CarBrand 
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? IconPath { get; set; }
    }
    public class CarReview
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
        public Car? Car { get; set; }

    }
    public class CarImage
    {
        public int Id { get; set; }
        public int CarId { get; set; }
        public Car? Car { get; set; }
        public string? Path { get; set; }
        public string? Name { get; set; }
        public string? MimeType { get; set; }
    }

    public class Testimonial
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Title { get; set; }
        public string? Comments { get; set; }
        public string? ProfilePhoto { get; set; }
        public bool Active { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }

}
