using WebApi.Common.Models;
using WebApi.Models.Entities;
using WebApi.Persistence;
using WebApi.Services.Common;
using WebApi.ViewModels.Filters;
using WebApi.ViewModels;
using WebApi.Common.Exceptions;

namespace WebApi.Services
{
    public class ServiceService
    {
        private readonly UnitOfWork _uow;
        private readonly LoggedInUser _user;
        private readonly TokenService _tokenService;
        //private readonly IConfiguration _configs;
        private readonly Type _serviceType = typeof(Service);
        private readonly UploadService _upload;

        public ServiceService(UnitOfWork uow, TokenService tokenService,
            //IConfiguration configs,
            LoggedInUser user, UploadService upload)
        {
            _uow = uow;
            _tokenService = tokenService;
            //_configs = configs;
            _user = user;
            _upload = upload;
        }

        public async Task<DataPageModel<ServiceVm>> Filter(ServiceFilter filter)
        {
            var data = await _uow.Services.Filter(filter);

            return new DataPageModel<ServiceVm>
            {
                Data = data.Data.ToList().Select(x => Map(x, new ServiceVm())),
                Count = data.Count,
                PageCount = data.PageCount,
            };
        }


        public async Task<Service> Save(ServiceVm ser)
        {
            var existing = ser.Id.HasValue && ser.Id > 0 ? await _uow.Services.GetDetails(ser.Id.Value) : null;

            var serNew = Map(ser, existing ?? new Service());
            if (existing is null)
            {
                serNew.CreatedDate = serNew.UpdatedDate;
                serNew.Active = true;
                serNew = await _uow.Services.Add(serNew);
            }

            await _uow.CompleteAsync();

            return serNew;
        }

        internal async Task<ServiceVm> GetService(int serId, bool forEdit = false)
        {
            var service = await _uow.Services.GetDetails(serId);

            return service is null
                ? throw new Exception($"Service with Id:{serId} not found")
                : Map(service, new ServiceVm(), forEdit);
        }

        private Service Map(ServiceVm ser, Service existing)
        {
            var curTime = DateTime.UtcNow;

            existing.Title = ser.Title;
            existing.TitleKz = ser.TitleKz;
            existing.TitleRu = ser.TitleRu;
            existing.Description = ser.Description;
            existing.DescriptionKz = ser.DescriptionKz;
            existing.DescriptionRu = ser.DescriptionRu;

            var tempImg = ser.Images.FirstOrDefault();
            if (tempImg is not null && !string.IsNullOrEmpty(tempImg.Token))
            {
                existing.ImagePath = _tokenService.ParseToken<FileToken>(tempImg.Token)?.RelativePath;
            }

            existing.UpdatedDate = curTime;

            return existing;
        }

        private ServiceVm Map(Service source, ServiceVm dest, bool forEdit = false)
        {
            dest.Id = source.Id;
            dest.TitleKz = GetTranslatedValue(nameof(source.TitleKz), source, source.TitleKz);
            dest.TitleRu = GetTranslatedValue(nameof(source.TitleRu), source, source.TitleRu);
            dest.DescriptionKz = GetTranslatedValue(nameof(source.DescriptionKz), source, source.DescriptionKz);
            dest.DescriptionRu = GetTranslatedValue(nameof(source.DescriptionRu), source, source.DescriptionRu);

            dest.Title = source.Title;
            dest.Description = source.Description;
            dest.Active = source.Active;

            if (!string.IsNullOrEmpty(source.ImagePath))
            {
                dest.ImagePath = _upload.GetFullPath(source.ImagePath);
                if (forEdit)
                {
                    dest.Images.Add(new FileToken
                    {
                        FullPath = dest.ImagePath,
                        RelativePath = source.ImagePath,
                        Token = _tokenService.CreateToken(new FileModel
                        {
                            RelativePath = source.ImagePath,
                            FullPath = dest.ImagePath,
                        })
                    });
                }
            }

            dest.UpdatedDate = source.UpdatedDate;
            dest.CreatedDate = source.CreatedDate;

            return dest;
        }


        private string? GetTranslatedValue(string serName, Service source, string? defaultValue)
        {
            if (string.IsNullOrEmpty(_user.LangPropertySuffix))
            {
                return defaultValue;
            }
            var ser = _serviceType.GetProperty(serName + _user.LangPropertySuffix);
            if (ser is not null)
            {
                return ser.GetValue(source)?.ToString();
            }
            return defaultValue;
        }

        internal async Task<ServiceVm> ChangeServiceStatus(int serId, bool active)
        {
            var service = await _uow.Services.FindOne(x => x.Id == serId)
                ?? throw new AppBadRequestException($"Service with id:{serId} for logged in user not found.");

            if (service.Active != active)
            {
                service.Active = active;
                await _uow.CompleteAsync();
            }
            return Map(service, new ServiceVm());
        }
    }
}
