using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json.Linq;
using WebApi.Common;
using WebApi.Common.Models;
using WebApi.Models;
using WebApi.Models.Entities;
using WebApi.Persistence;
using WebApi.Services;
using WebApi.Services.Common;
using WebApi.ViewModels;
using WebApi.ViewModels.Filters;

namespace WebApi.Controllers
{
    public class CarsController : ApiControllerBase
    {
        private readonly UnitOfWork _uow;
        private readonly LoggedInUser _user;
        private readonly CarService _service;
        private readonly UploadService _upload;
        private readonly TokenService _tokenGen;
        private readonly IValidator<CarVm> _validateCar;
        private readonly IValidator<FilterBase> _validateFilter;

        public CarsController(UnitOfWork uow,
            IValidator<CarVm> validateCar,
            IValidator<FilterBase> validateFilter,
            LoggedInUser user,
            CarService service,
            TokenService tokenGen,
            UploadService upload)
        {
            _uow = uow;
            _validateCar = validateCar;
            _validateFilter = validateFilter;
            _user = user;
            _service = service;
            _tokenGen = tokenGen;
            _upload = upload;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Save(CarVm car)
        {
            _validateCar.ValidateAndThrow(car);

            var carNew = await _service.Save(car);
            return Ok(new { carNew.Id, carNew.UpdatedDate, carNew.Name });
        }

        [HttpGet]
        public async Task<IActionResult> Filter([FromQuery] CarFilter filter, string? currency = "aed")
        {
            _validateFilter.ValidateAndThrow(filter);
            var data = await _service.Filter(filter);
            data.Data.Select(car =>
            {
                car.Price *= ExchangeRate.Currencies.ContainsKey($"_{currency}") ? ExchangeRate.Currencies[$"_{currency}"].Rate : (decimal)1;
                return car;
            });
            return Ok(data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Index(int id, bool forEdit = false)
        {
            var car = await _service.GetCar(id, forEdit);

            return Ok(car);
        }

        [Authorize]
        [HttpGet("{id}/change-status")]
        public async Task<IActionResult> ChangeStatus(int id, bool active = false)
        {
            var car = await _service.ChangeCarStatus(id, active);
            return Ok(car);
        }

        [HttpGet("brands")]
        public async Task<IActionResult> AllBrands()
        {
            var brands = await _uow.Brands.FindPaged(x => true, 0);
            return Ok(brands.Select(x =>
            {
                x.IconPath = _upload.GetResourceFullPath(x.IconPath);
                return x;
            }));
        }

        [HttpPost("review")]
        public async Task<IActionResult> AddReview(CarReview review)
        {
            var result = await _service.SaveReview(review);
            return Ok(new { result.Id });
        }

        [HttpGet("{id}/reviews")]
        public async Task<IActionResult> Reviews(int id)
        {
            var result = await _service.GetReviews(id);
            return Ok(result);
        }
    }
}
