namespace WebApi.Models.Entities
{
    public class User
    {
        public User()
        {
            Roles = new HashSet<UserRole>();
        }
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? Contact { get; set; }
        public string? Password { get; set; }
        //public string? ContactOtp { get; set; }
        public string? EmailOtp { get; set; }
        public DateTime? EmailOtpExpiry { get; set; }
        public bool EmailVerified { get; set; }
        public string? ProfilePhoto { get; set; }
        public UserStatus Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public ICollection<UserRole> Roles { get; set; }
    }

    public class UserToken
    {
        public int? Id { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime ExpiryTime { get; set; }
        public int UserId { get; set; }
        public User? User { get; set; }
    }
    public class UserRole
    {
        public int UserId { get; set; }
        public string? Role { get; set; }
        public User? User { get; set; }
    }
    public enum UserStatus
    {
        Normal = 10,
        Locked = 11,
        Blocked = 12,
        InActive = 13,
    }
}
