using System.Security.Claims;

namespace WebApi.Services.Common
{
    public class LoggedInUser
    {
        public int UserId { get; private set; }
        public string? Email { get; private set; }
        public string Lang { get; private set; }
        public string? LangPropertySuffix { get; private set; }

        public LoggedInUser(IHttpContextAccessor httpContext)
        {
            var claims = httpContext?.HttpContext?.User.Claims.ToList();
            Lang = httpContext?.HttpContext?.Request.Headers.FirstOrDefault(x => x.Key == "translate-lang").Value.ToString()??"";
            if (string.IsNullOrEmpty(Lang))
            {
                Lang = "en";
            }

            if(Lang != "en")
            {
                try
                {
                    var chars = Lang!.ToCharArray();
                    LangPropertySuffix = string.Join("", char.ToUpper(chars[0]), char.ToLower(chars[1]));
                }
                catch { }
            }

            claims?.ForEach(x =>
            {
                if (x.Type == ClaimTypes.NameIdentifier && int.TryParse(x.Value, out var userId))
                {
                    UserId = userId;
                }
                else if (x.Type == ClaimTypes.Email)
                {

                    Email = x.Value;
                }
            });
        }
    }
}
