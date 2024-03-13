using System.Reflection;

namespace WebApi.Models.Entities
{
    public class Booking
    {
        public int Id { get; set; }
        public string? FullName { get; set; }
        public string? Gender { get; set; }
        public string? Contact { get; set; }
        public string? Email { get; set; }
        public string? Country { get; set; }
        public string? Comments { get; set; }
        public string? UserIp { get; set; }
        public bool Closed { get; set; }
        public int CarId { get; set; }
        public Car? Car { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}