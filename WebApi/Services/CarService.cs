using FluentValidation;
using WebApi.Models.Entities;
using WebApi.Persistence;
using WebApi.Services.Common;
using WebApi.ViewModels.Filters;
using WebApi.ViewModels;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;
using WebApi.Common.Models;
using System.Reflection;
using WebApi.Common;
using System.Linq;
using WebApi.Common.Exceptions;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace WebApi.Services
{
    public class CarService
    {
        private readonly UnitOfWork _uow;
        private readonly LoggedInUser _user;
        private readonly TokenService _tokenService;
        //private readonly IConfiguration _configs;
        private readonly Type _carType = typeof(Car);
        private readonly UploadService _upload;

        public CarService(UnitOfWork uow, TokenService tokenService,
            //IConfiguration configs,
            LoggedInUser user, UploadService upload)
        {
            _uow = uow;
            _tokenService = tokenService;
            //_configs = configs;
            _user = user;
            _upload = upload;
        }

        public async Task<DataPageModel<CarVm>> Filter(CarFilter filter)
        {
            var data = await _uow.Cars.Filter(filter);

            return new DataPageModel<CarVm>
            {
                Data = data.Data.ToList().Select(x => Map(x, new CarVm())),
                Count = data.Count,
                PageCount = data.PageCount,
            };
        }

        public async Task<Car> Save(CarVm car)
        {
            var existing = car.Id.HasValue && car.Id > 0 ? await _uow.Cars.GetDetails(car.Id.Value) : null;
            var brand = await _uow.Brands.Find(car.BrandId);

            if (brand is null) throw new AppBadRequestException($"Brand with id:'{car.BrandId}' not found");
            
            var carNew = Map(car, existing ?? new Car(), brand);
            if (existing is null)
            {
                carNew.CreatedDate = carNew.UpdatedDate;
                carNew.Active = true;
                carNew = await _uow.Cars.Add(carNew);
            }

            await _uow.CompleteAsync();

            return carNew;
        }

        private Car Map(CarVm car, Car existing, CarBrand brand)
        {
            var curTime = DateTime.UtcNow;

            existing.Name = car.Name;
            existing.Battery = car.Battery;
            existing.BatteryKz = car.BatteryKz;
            existing.BatteryRu = car.BatteryRu;
            existing.Performance = car.Performance;
            existing.PerformanceKz = car.PerformanceKz;
            existing.PerformanceRu = car.PerformanceRu;
            existing.Range = car.Range;
            existing.RangeKz = car.RangeKz;
            existing.RangeRu = car.RangeRu;
            existing.Efficiency = car.Efficiency;
            existing.EfficiencyKz = car.EfficiencyKz;
            existing.EfficiencyRu = car.EfficiencyRu;
            existing.BatteryPower = car.BatteryPower;
            existing.TopSpeed = car.TopSpeed;
            existing.EfficiencyVal = car.EfficiencyVal;
            existing.Price = car.Price;
            existing.PriceCurrency = car.PriceCurrency;
            existing.BestSeller = car.BestSeller;
            existing.Featured = car.Featured;
            existing.BrandId = car.BrandId;
            existing.ImagePath = car.ImagePath;

            existing.BrandName = brand.Name;

            existing.UpdatedDate = curTime;
            existing.Images.Clear();

            foreach (var img in car.Images)
            {
                var file = _tokenService.ParseToken<FileModel>(img.Token ?? "");
                if (file is not null)
                {
                    existing.Images.Add(new CarImage
                    {
                        MimeType = file.MimeType,
                        Name = file.Name,
                        Path = file.RelativePath,
                    });
                }
            }

            if (existing.Images.Count > 0)
            {
                existing.ImagePath = existing.Images.FirstOrDefault()?.Path;
            }

            return existing;
        }

        private CarVm Map(Car source, CarVm dest, bool forEdit = false)
        {
            dest.Id = source.Id;
            dest.BatteryKz = GetTranslatedValue(nameof(source.BatteryKz), source, source.BatteryKz);
            dest.PerformanceKz = GetTranslatedValue(nameof(source.PerformanceKz), source, source.PerformanceKz);
            dest.RangeKz = GetTranslatedValue(nameof(source.RangeKz), source, source.RangeKz);
            dest.EfficiencyKz = GetTranslatedValue(nameof(source.EfficiencyKz), source, source.EfficiencyKz);           
            dest.BatteryRu = GetTranslatedValue(nameof(source.BatteryRu), source, source.BatteryRu);
            dest.PerformanceRu = GetTranslatedValue(nameof(source.PerformanceRu), source, source.PerformanceRu);
            dest.RangeRu = GetTranslatedValue(nameof(source.RangeRu), source, source.RangeRu);
            dest.EfficiencyRu = GetTranslatedValue(nameof(source.EfficiencyRu), source, source.EfficiencyRu);

            dest.Name = source.Name;
            dest.Battery = source.Battery;
            //dest.BatteryKz = source.BatteryKz;
            //dest.BatteryRu = source.BatteryRu;
            dest.Performance = source.Performance;
            //dest.PerfomanceKz = source.PerfomanceKz;
            //dest.PerfomanceRu = source.PerfomanceRu;
            dest.Range = source.Range;
            //dest.RangeKz = source.RangeKz;
            //dest.RangeRu = source.RangeRu;
            dest.Efficiency = source.Efficiency;
            //dest.EfficiencyKz = source.EfficiencyKz;
            //dest.EfficiencyRu = source.EfficiencyRu;
            dest.BatteryPower = source.BatteryPower;
            dest.TopSpeed = source.TopSpeed;
            dest.EfficiencyVal = source.EfficiencyVal;
            dest.Price = source.Price;
            dest.PriceCurrency = source.PriceCurrency;
            dest.BestSeller = source.BestSeller;
            dest.Featured = source.Featured;
            dest.ImagePath = source.ImagePath;
            dest.ImagePath = _upload.GetFullPath(source.ImagePath);
            dest.BrandId = source.BrandId;
            dest.BrandName = source.BrandName;          
            dest.Stars1 = source.Stars1;
            dest.Stars2 = source.Stars2;
            dest.Stars3 = source.Stars3;
            dest.Stars4 = source.Stars4;
            dest.Stars5 = source.Stars5;
            dest.Active = source.Active;
            dest.UpdatedDate = source.UpdatedDate;
            dest.CreatedDate = source.CreatedDate;

            var images = new List<FileToken>();
            foreach (var img in source.Images)
            {
                var token = !forEdit?"":_tokenService.CreateToken(new FileModel
                {
                    MimeType = img.MimeType,
                    Name = img.Name,
                    RelativePath = img.Path,
                });
                images.Add(new FileToken
                {
                    Token = token,
                    Name = img.Name,
                    RelativePath= img.Path,
                    FullPath = _upload.GetFullPath(img.Path),
                });
            }
            dest.Images = images;
            return dest;
        }

        //private CarBrandVm? Map(CarBrand source, CarBrandVm dest)
        //{
        //    dest.Id = source.Id;
        //    dest.Name = source.Name;
        //    dest.IconPath = _upload.GetResourceFullPath(source.IconPath);
        //    return dest;
        //}

        //private UserVm? Map(User? source, UserVm dest)
        //{
        //    if (source is null) return null;

        //    dest.FirstName = source.FirstName;
        //    dest.LastName = source.LastName;
        //    dest.ProfilePhoto = _upload.GetFullPath(source.ProfilePhoto);
        //    dest.Contact = source.Contact;
        //    dest.Id = source.Id;

        //    return dest;
        //}

        private string? GetTranslatedValue(string carName, Car source, string? defaultValue)
        {
            if (string.IsNullOrEmpty(_user.LangPropertySuffix))
            {
                return defaultValue;
            }
            var car = _carType.GetProperty(carName + _user.LangPropertySuffix);
            if (car is not null)
            {
                return car.GetValue(source)?.ToString();
            }
            return defaultValue;
        }

        internal async Task<CarVm> GetCar(int id, bool forEdit = false)
        {
            var car = await _uow.Cars.GetDetails(id);

            return car is null
                ? throw new Exception($"Car with Id:{id} not found")
                : Map(car, new CarVm(), forEdit);
        }

        public async Task<CarReview> SaveReview(CarReview review)
        {
            var car = await _uow.Cars.Find(review.CarId);
            if (car is null)
            {
                throw new Exception($"Car with id: '{review.CarId}'not found");
            }

            if (review.Stars == 5) car.Stars5++;
            else if (review.Stars == 4) car.Stars4++;
            else if (review.Stars == 3) car.Stars3++;
            else if (review.Stars == 2) car.Stars2++;
            else if (review.Stars == 1) car.Stars1++;

            var curTime = DateTime.UtcNow;
            review.CreatedDate = curTime;
            review.UpdatedDate = curTime;

            review = await _uow.Reviews.Add(review);
            await _uow.CompleteAsync();
            return review;
        }

        internal async Task<IEnumerable<CarReviewVm>> GetReviews(int carId)
        {
            var reviews = await _uow.Reviews
             .ByCarId(carId);
            return reviews.Select(Map);
        }

        private CarReviewVm Map(CarReview source)
        {
            return new CarReviewVm
            {
                CreatedDate = source.CreatedDate,
                UpdatedDate = source.UpdatedDate,
                CarId = source.CarId,
                Stars = source.Stars,
                Name = source.Name,
                Contact = source.Contact,
                Email = source.Email,
                Message = source.Message,
                UserIp = source.UserIp,
                Id = source.Id,
            };
        }


        internal async Task<CarVm> ChangeCarStatus(int carId, bool active)
        {
            var car = await _uow.Cars.FindOne(x => x.Id == carId)
                ?? throw new AppBadRequestException($"Car with id:{carId} for logged in user not found.");

            if (car.Active != active)
            {
                car.Active = active;  
                await _uow.CompleteAsync();
            }
            return Map(car, new CarVm());
        }
    }
}
