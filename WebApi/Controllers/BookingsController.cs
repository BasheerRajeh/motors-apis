using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices;
using WebApi.Common;
using WebApi.Common.Exceptions;
using WebApi.Models.Entities;
using WebApi.Persistence;
using WebApi.Services;
using WebApi.Services.Common;
using WebApi.ViewModels;
using WebApi.ViewModels.Filters;

namespace WebApi.Controllers
{
    [Authorize]
    public class BookingsController : ApiControllerBase
    {

        private readonly UnitOfWork _uow;
        private readonly LoggedInUser _user;
        private readonly BookingService _service;
        private readonly IValidator<BookingVm> _validateBooking;


        public BookingsController(UnitOfWork uow,
            IValidator<BookingVm> validateBooking,
            LoggedInUser user,
            BookingService service)
        {

            _uow = uow;
            _validateBooking = validateBooking;
            _user = user;
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Add(BookingVm booking)
        {
            _validateBooking.ValidateAndThrow(booking);
            var book = await _service.Save(booking);
            return Ok(new { book.Id, book.CreatedDate, book.FullName });
        }


        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Filter([FromQuery] BookingFilter filter)
        {
            var bookings = await _service.Filter(filter);
            return Ok(bookings);

        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> ById(int id)
        {
            var bookings = await _uow.Bookings.Find(id);
            return Ok(bookings);
        }


        [Authorize]
        [HttpGet("{id}/close")]
        public async Task<IActionResult> Close(int id, bool close = true)
        {
            var booking = await _service.BookingClose(id, close);
            return Ok(booking);
        }
    }
}
