namespace WebApi.ViewModels
{
    public class MessageVms
    {
    }

    public class MessageVm
    {
        public int? Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Body { get; set; }
        public int ContactSubmissionId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set;}
    }
}