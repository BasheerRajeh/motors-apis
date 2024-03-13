namespace WebApi.ViewModels
{
    public class AuthVms
    {
    }
    public class RegisterUserVm
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? Contact { get; set; }
        public string? Password { get; set; }
        public string? ConfirmPassword { get; set; }
    }
    public class ConfigEmailModel
    {
        public int UserId { get; set; }
        public string Email { get; set; } = "";
        public string EmailToken { get; set; } = "";
    }
    public class TokenModelVm
    {
        public string AccessToken { get; set; } = "";
        public string RefreshToken { get; set; } = "";
    }

    public class ChangePasswordModelVm
    {
        public string Password { get; set; } = "";
        public string ConfirmPassword { get; set; } = "";
    }

    public class LoginModelVm
    {
        public string Username { get; set; } = "";
        public string Password { get; set; } = "";
    }
}
