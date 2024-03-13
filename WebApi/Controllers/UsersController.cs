using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi.Common;
using WebApi.Common.Models;
using WebApi.Models.Entities;
using WebApi.Persistence;
using WebApi.Services.Common;
using WebApi.ViewModels.Filters;

namespace WebApi.Controllers
{
    public class UsersController : ApiControllerBase
    {
        private readonly UnitOfWork _uow;
        private readonly UploadService _upload;
        private readonly LoggedInUser _user;
        public UsersController(UnitOfWork uow,
            UploadService upload,
            LoggedInUser user)
        {
            _uow = uow;
            _upload = upload;
            _user = user;
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpGet]
        public async Task<IActionResult> All([FromQuery] FilterBase filter)
        {
            var users = await _uow.Users.Filter(filter);
            return Ok(new DataPageModel<object>
            {
                Count = users.Count,
                PageCount = users.PageCount,
                Data = users.Data.Select(MapUser)
            });
        }

        [Authorize]
        [HttpPost("profile")]
        public async Task<IActionResult> UpdateProfile(User req)
        {
            var user = await _uow.Users.Find(_user.UserId)
                ?? throw new Exception($"User with Id:'{_user.UserId}' not found");

            var curTime = DateTime.UtcNow;

            user.FirstName = req.FirstName;
            user.LastName = req.LastName;
            user.UpdatedDate = curTime;

            await _uow.CompleteAsync();

            return Ok(new { user.UpdatedDate, user.Id });
        }

        [Authorize]
        [HttpGet("my-profile")]
        public async Task<IActionResult> GetProfile()
        {
            var user = await _uow.Users.Find(_user.UserId)
                ?? throw new Exception($"User with Id:'{_user.UserId}' not found");

            return Ok(MapUser(user));
        }

        [Authorize]
        [HttpPost("profile-picture")]
        public async Task<IActionResult> SaveFile(IFormFile file)
        {
            var user = await _uow.Users.Find(_user.UserId)
                ?? throw new Exception($"User with Id:'{_user.UserId}' not found");

            var curTime = DateTime.UtcNow;

            var token = (await _upload.SaveFiles(new List<IFormFile> { file }, true)).FirstOrDefault();
            user.ProfilePhoto = token?.RelativePath;
            user.UpdatedDate = curTime;

            await _uow.CompleteAsync();

            return Ok(token);
        }


        private object MapUser(User user)
        {
            return new
            {
                user.Id,
                user.FirstName,
                user.LastName,
                user.Email,
                user.Contact,
                user.CreatedDate,
                user.UpdatedDate,
                user.Status,
                user.EmailVerified,
                ProfilePhoto = _upload.GetFullPath(user.ProfilePhoto),
            };
        }
    }
}
