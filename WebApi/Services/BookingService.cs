using System.Runtime.InteropServices;
using WebApi.Common.Exceptions;
using WebApi.Common.Models;
using WebApi.Models.Entities;
using WebApi.Persistence;
using WebApi.Services.Common;
using WebApi.ViewModels;
using WebApi.ViewModels.Filters;

namespace WebApi.Services
{
    public class BookingService
    {
        private readonly UnitOfWork _uow;
        private readonly LoggedInUser _user;
        private readonly TokenService _tokenService;
        //private readonly IConfiguration _configs;
        private readonly Type _bookingType = typeof(Booking);
        private readonly UploadService _upload;

        public BookingService(UnitOfWork uow, TokenService tokenService,
            //IConfiguration configs,
            LoggedInUser user, UploadService upload)
        {
            _uow = uow;
            _tokenService = tokenService;
            //_configs = configs;
            _user = user;
            _upload = upload;
        }

        public async Task<Booking> Save(BookingVm bookingVm)
        {
            var car = await _uow.Cars.GetDetails(bookingVm.CarId);

            if (car is null)
            {
                throw new Exception($"Car with Id:{bookingVm.CarId} not found");
            }

            var curTime = DateTime.UtcNow;
            var booking = new Booking
            {
                CreatedDate = curTime,
                UpdatedDate = curTime,
                CarId = car.Id,
                Contact = bookingVm.Contact,
                Email = bookingVm.Email,
                FullName = bookingVm.FullName,
                Gender = bookingVm.Gender,
                Country = bookingVm.Country,
                Comments = bookingVm.Comments,
                UserIp = bookingVm.UserIp
            };
            booking = await _uow.Bookings.Add(booking);
            await _uow.CompleteAsync();

            return booking;
        }

        public async Task<DataPageModel<Booking>> Filter(BookingFilter filter)
        {
            var data = await _uow.Bookings.Filter(filter);
            return data;
        }

        internal async Task<Booking> BookingClose(int id, bool close)
        {
            var booking = await _uow.Bookings.Find(id)
                ?? throw new AppBadRequestException($"Booking with id:{id} for logged in user not found.");

            if (booking.Closed != close)
            {
                booking.Closed = close;
                await _uow.CompleteAsync();
            }

            return booking;
        }
    }
}
