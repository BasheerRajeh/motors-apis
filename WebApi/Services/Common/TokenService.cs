using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace WebApi.Services.Common
{
    public class TokenService
    {
        private readonly IConfiguration _configuration;
        private readonly JwtSecurityTokenHandler _tokenHandler = new JwtSecurityTokenHandler();

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        //public T? ParseToken<T>(string token) where T : new()
        //{
        //    var claims=GetClaims(token);
        //    var obj = new T();
        //    var props = obj.GetType().GetProperties();
        //    foreach (var prop in props) {



        //    prop.SetValue(obj,)

        //            }
        //    return default;
        //}

        public string CreateToken(object obj, int expiryMinutes = 0)
        {
            var claims = obj.GetType().GetProperties().Select(x =>
            {
                var val = x.GetValue(obj);
                if(val is bool)
                {
                    return new Claim(x.Name, val!.ToString()!, ClaimValueTypes.Boolean);
                }
                if(val is int)
                {
                    return new Claim(x.Name, val!.ToString()!, ClaimValueTypes.Integer32);
                }
                if(val is DateTime dt)
                {
                    return new Claim(x.Name, dt.ToString("o"));
                }
                return new Claim(x.Name, x.GetValue(obj)?.ToString() ?? "");
            });
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"] ?? ""));
            var token = new JwtSecurityToken(
                claims: claims,
                expires: expiryMinutes > 0 ? DateTime.UtcNow.AddMinutes(expiryMinutes) : null,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            return _tokenHandler.WriteToken(token);
        }

        public T? ParseToken<T>(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"] ?? "")),
                ValidateLifetime = false
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
            if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");

            var map = principal.Claims.ToDictionary(x => x.Type, x => x.Value);
            return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(map));
        }
    }
}
