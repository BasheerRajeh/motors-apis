using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Common;
using WebApi.Models.Entities;
using WebApi.Persistence;
using WebApi.Services;
using WebApi.ViewModels;
using WebApi.ViewModels.Filters;

namespace WebApi.Controllers
{
    public class ServicesController : ApiControllerBase
    {
        private readonly ServiceService _service;
        private readonly IValidator<ServiceVm> _validateService;

        public ServicesController(
            IValidator<ServiceVm> validateService,
            ServiceService service)
        {
            _validateService = validateService;
            _service = service;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Index(int id, bool forEdit = false)
        {
            var services = await _service.GetService(id, forEdit);
            return Ok(services);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Save(ServiceVm service)
        {
            _validateService.ValidateAndThrow(service);
            var ser = await _service.Save(service);
            return Ok(new {ser.Id, ser.UpdatedDate, ser.Title });
        }

        [Authorize]
        [HttpGet("{id}/change-status")]
        public async Task<IActionResult> ChangeStatus(int id, bool active = false)
        {
            var service = await _service.ChangeServiceStatus(id, active);
            return Ok(service);
        }

        [HttpGet]
        public async Task<IActionResult> Filter([FromQuery] ServiceFilter filter)
        {
            var data = await _service.Filter(filter);
            return Ok(data);
        }
    }
}