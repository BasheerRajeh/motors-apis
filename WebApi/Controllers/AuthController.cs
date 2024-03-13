using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using WebApi.Models;
using WebApi.Models.Entities;
using WebApi.ViewModels;
using FluentValidation;
using WebApi.Services.Common;
using WebApi.Common;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApi.Controllers
{
    public class AuthController : ApiControllerBase
    {
        //private readonly int _emailTokenLifetimeHours = 12;
        private readonly IConfiguration _configuration;
        private readonly LoggedInUser _user;
        private readonly AppDbContext _db;
        //private readonly EmailService _email;
        private readonly UploadService _upload;
        private readonly KeyGenerator _keyGen;
        //private readonly FirebaseNotifier _notifier;
        private readonly IValidator<RegisterUserVm> _validateUser;
        private readonly IValidator<ChangePasswordModelVm> _validateChangePassword;

        public AuthController(IConfiguration configuration,
            AppDbContext context,
            LoggedInUser user,
            //EmailService email,
            IValidator<RegisterUserVm> validateUser,
            IValidator<ChangePasswordModelVm> validateChangePassword,
            KeyGenerator keyGen,
            //FirebaseNotifier notifier,
            UploadService upload)
        {
            _configuration = configuration;
            _db = context;
            _user = user;
            //_email = email;
            _validateUser = validateUser;
            _validateChangePassword = validateChangePassword;
            _keyGen = keyGen;
            //_notifier = notifier;
            _upload = upload;
        }
        //[HttpGet("notify-test")]
        //public async Task<IActionResult> NotifyTest(string? title = "Hey!", string? message = "Welcome to Nomad")
        //{
        //    var resp = await _notifier.NotifyAsync(title, message);
        //    return Ok(resp);
        //}
        [HttpPost("register")]
        public async Task<IActionResult> Post(RegisterUserVm user)
        {
            _validateUser.ValidateAndThrow(user);

            var usersQuery = _db.Users;

            if (await usersQuery.AnyAsync(x => x.Contact == user.Contact))
            {
                return BadRequest("User with contact number already exists");
            }
            if (await usersQuery.AnyAsync(x => x.Email == user.Email))
            {
                return BadRequest("User with email already exists");
            }
            var curTime = DateTime.UtcNow;
            //var emailToken = _keyGen.GetUniqueKey(64);
            var result = await _db.Users.AddAsync(new User
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                CreatedDate = curTime,
                UpdatedDate = curTime,
                Status = UserStatus.Normal,
                Contact = user.Contact,
                //EmailOtp = emailToken,
                //EmailOtpExpiry = curTime.AddHours(_emailTokenLifetimeHours),
                Password = BCrypt.Net.BCrypt.HashPassword(user.Password)
            });
            await _db.SaveChangesAsync();

            //await _email.SendEmailConfirmationToken(user.Email!, result.Entity.Id, user.FirstName!, emailToken);

            var resp = await GenerateToken(result.Entity);
            return Ok(resp);

        }

        [HttpGet("confirm-email/{userId}/{token}")]
        public async Task<IActionResult> ConfirmEmail(int userId, string token)
        {
            var curTime = DateTime.UtcNow;
            var user = await _db.Users.FindAsync(userId);
            if (user is null
                || string.IsNullOrEmpty(user.EmailOtp)
                || user.EmailOtp != token)
            {
                return BadRequest("Sorry, but we're unable to process your request at the moment.");
            }
            if (!user.EmailOtpExpiry.HasValue || curTime > user.EmailOtpExpiry)
            {
                return BadRequest("Sorry, your email verification link has expired");
            }
            user.EmailOtp = null;
            user.EmailOtpExpiry = null;
            user.EmailVerified = true;
            await _db.SaveChangesAsync();
            return Ok("Congratulations! Your email has been successfully verified.");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginModelVm model)
        {
            var user = await GetUser(model.Username);
            if (user is null || user.Status != UserStatus.Normal)
            {
                return BadRequest("Неверный адрес электронной почты");
            }
            if (!BCrypt.Net.BCrypt.Verify(model.Password, user.Password))
            {
                return BadRequest("Неверный пароль");
            }
            var resp = await GenerateToken(user);
            return Ok(resp);
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken(TokenModelVm tokenModel)
        {
            if (tokenModel is null)
            {
                return BadRequest("Invalid client request.");
            }
            string? accessToken = tokenModel.AccessToken;
            string? refreshToken = tokenModel.RefreshToken;
            var principal = GetPrincipalFromExpiredToken(accessToken);
            if (principal == null)
            {
                return BadRequest("Invalid access token.");
            }
            var username = principal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value ?? "";
            var user = await GetUser(username);
            if (user == null)
            {
                return BadRequest("Invalid access token 2.");
            }
            var tokenObj = await _db.UserTokens.FirstOrDefaultAsync(x => x.UserId == user.Id && x.RefreshToken == refreshToken);
            if (tokenObj is null || DateTime.UtcNow > tokenObj.ExpiryTime)
            {
                return BadRequest("Invalid refresh token.");
            }
            //var authClaims = GetUserClaims(user);
            var resp = await GenerateToken(user, tokenObj);
            return Ok(resp);
        }

        [Authorize]
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword(ChangePasswordModelVm req)
        {
            _validateChangePassword.ValidateAndThrow(req);
            var user = await _db.Users.FindAsync(_user.UserId);
            if (user is null)
            {
                return BadRequest("User not found.");
            }
            //if(BCrypt.Net.BCrypt.Verify(req.Password, user.Password))
            //{
            //    return BadRequest("invalid old password");
            //}
            user.Password = BCrypt.Net.BCrypt.HashPassword(req.Password);
            user.UpdatedDate = DateTime.UtcNow;
            _db.Entry(user).State = EntityState.Modified;
            await _db.SaveChangesAsync();
            return Ok(new { user.UpdatedDate, user.Id });
        }

        [Authorize]
        [HttpGet("logout")]
        public async Task<IActionResult> Revoke()
        {
            var count = await _db.UserTokens.Where(x => x.UserId == _user.UserId).ExecuteDeleteAsync();
            return Ok(new { _user.UserId, tokensDeleted = count });
        }

        [Authorize]
        [HttpGet("deactivate")]
        public async Task<IActionResult> DeActivate()
        {
            var user = await _db.Users.FirstOrDefaultAsync(x => x.Id == _user.UserId);
            if (user == null) return BadRequest("User not found.");

            await _db.UserTokens.Where(x => x.UserId == _user.UserId).ExecuteDeleteAsync();

            user.Status = UserStatus.InActive;
            user.UpdatedDate = DateTime.UtcNow;

            _db.SaveChanges();

            return Ok(new { UserId = user.Id });
        }
        private async Task<object> GenerateToken(User user, UserToken? existingToken = null)
        {
            var tokenId = Guid.NewGuid().ToString();
            var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString(), ClaimValueTypes.Integer32),
                    new Claim(JwtRegisteredClaimNames.Jti, tokenId),
                };
            if (!string.IsNullOrEmpty(user.Email))
                authClaims.Add(new Claim(ClaimTypes.Email, user.Email));
            foreach (var userRole in user.Roles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole.Role!));
            }
            var newAccessToken = GenerateAccessToken(authClaims);
            var newRefreshToken = await GenerateRefereshToken(user, existingToken);
            var resp = MapUserAndToken(user, newAccessToken, newRefreshToken);
            return resp;
        }

        private async Task<string> GenerateRefereshToken(User user, UserToken? existingToken)
        {
            var newRefreshToken = GenerateRandomToken();
            int.TryParse(_configuration["JWT:RefreshTokenValidityInDays"], out int tokenValidityIndays);

            var token = existingToken ?? new UserToken();
            token.RefreshToken = newRefreshToken;
            token.ExpiryTime = DateTime.UtcNow.AddDays(tokenValidityIndays);

            if (existingToken is null)
            {
                token.UserId = user.Id;
                _db.UserTokens.Add(token);
            }

            await _db.SaveChangesAsync();

            return newRefreshToken;
        }
        private async Task<User?> GetUser(string username)
        {
            var isUserId = int.TryParse(username, out var userId);
            var canCon = await _db.Database.CanConnectAsync();
            var user = await _db.Users.Include(x => x.Roles).FirstOrDefaultAsync(x => x.Email == username || x.Contact == username || (isUserId && x.Id == userId));
            return user;
        }

        private object MapUserAndToken(User user, JwtSecurityToken token, string refreshToken)
        {
            return new
            {
                user.Id,
                user.Email,
                user.FirstName,
                user.LastName,
                user.Contact,
                user.CreatedDate,
                user.UpdatedDate,
                ProfilePhoto = _upload.GetFullPath(user.ProfilePhoto),
                Token = new
                {
                    AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
                    RefreshToken = refreshToken,
                    Expiration = token.ValidTo
                }
            };
        }
        private JwtSecurityToken GenerateAccessToken(List<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"] ?? ""));
            _ = int.TryParse(_configuration["JWT:TokenValidityInMinutes"], out int tokenValidityInMinutes);

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.UtcNow.AddMinutes(tokenValidityInMinutes),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            return token;
        }
        private static string GenerateRandomToken(int size = 32)
        {
            var randomNumber = new byte[size];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToHexString(randomNumber).ToLower();
        }
        private ClaimsPrincipal? GetPrincipalFromExpiredToken(string? token)
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

            return principal;
        }
    }
}
