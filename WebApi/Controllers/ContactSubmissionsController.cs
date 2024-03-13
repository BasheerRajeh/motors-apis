using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using WebApi.Persistence;
using WebApi.Services.Common;
using WebApi.Services;
using WebApi.ViewModels.Filters;
using WebApi.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace WebApi.Controllers
{
    public class ContactSubmissionsController : ApiControllerBase
    {
        private readonly UnitOfWork _uow;
        private readonly LoggedInUser _user;
        private readonly ContactSubmissionService _service;
        private readonly UploadService _upload;
        private readonly TokenService _tokenGen;
        private readonly IValidator<ContactSubmissionVm> _validateContactSubmission;
        private readonly IValidator<FilterBase> _validateFilter;

        public ContactSubmissionsController(UnitOfWork uow,
            IValidator<ContactSubmissionVm> validateContactSubmission,
            IValidator<FilterBase> validateFilter,
            LoggedInUser user,
            ContactSubmissionService service,
            TokenService tokenGen,
            UploadService upload)
        {
            _uow = uow;
            _validateContactSubmission = validateContactSubmission;
            _validateFilter = validateFilter;
            _user = user;
            _service = service;
            _tokenGen = tokenGen;
            _upload = upload;
        }

        [HttpPost]
        public async Task<IActionResult> Save(ContactSubmissionVm contactSubmission)
        {
            _validateContactSubmission.ValidateAndThrow(contactSubmission);

            var data = await _service.Save(contactSubmission);
            return Ok(new {data.Id, data.Name, data.CreatedDate});

        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Filter([FromQuery] FilterBase filter)
        {
            _validateFilter.ValidateAndThrow(filter);
            var data = await _service.Filter(filter);
            return Ok(data);
        }

        [Authorize]
        [HttpGet("{id}/change-status")]
        public async Task<IActionResult> ChangeStatus(int id, bool active = false)
        {
            var cs = await _service.ChangeStatus(id, active);
            return Ok(cs);
        }
    }
}
