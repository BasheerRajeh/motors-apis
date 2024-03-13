namespace WebApi.Models.Entities
{
    public class ContactSubmission
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Contact { get; set; }
        public string? Country { get; set; }
        public string? UserIp { get; set; }
        public string? Message { get; set; }
        public bool Active { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public ICollection<Message> Messages { get; set; }

    }
}
