using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Services;
using WebApi.ViewModels.Filters;
using WebApi.ViewModels;

namespace WebApi.Controllers
{
    public class TestimonialsController : ApiControllerBase
    {
        private readonly TestimonialService _service;
        private readonly IValidator<TestimonialVm> _validateTestimonial;

        public TestimonialsController(
            IValidator<TestimonialVm> validateTestimonial,
            TestimonialService service)
        {
            _validateTestimonial = validateTestimonial;
            _service = service;
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> Index(int id, bool forEdit = false)
        {
            var testimonial = await _service.GetTestimonial(id, forEdit);
            return Ok(testimonial);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Save(TestimonialVm testimonial)
        {
            _validateTestimonial.ValidateAndThrow(testimonial);
            var testimo = await _service.Save(testimonial);
            return Ok(new { testimo.Id, testimo.UpdatedDate, testimo.Name});
        }


        [Authorize]
        [HttpGet("{id}/change-status")]
        public async Task<IActionResult> ChangeStatus(int id, bool active = false)
        {
            var testimonial = await _service.ChangeTestinonailStatus(id, active);
            return Ok(testimonial);
        }

        [HttpGet]
        public async Task<IActionResult> Filter([FromQuery] FilterBase filter)
        {
            var data = await _service.Filter(filter);
            return Ok(data);
        }
    }
}