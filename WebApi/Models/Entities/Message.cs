namespace WebApi.Models.Entities
{
    public class Message
    {
        public int Id { get; set; }
        public string Body { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public int ContactSubmissionId { get; set; }
        public ContactSubmission? ContactSubmission{ get; set; }
    }
}
