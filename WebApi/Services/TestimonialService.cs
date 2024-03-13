using WebApi.Common.Exceptions;
using WebApi.Common.Models;
using WebApi.Models.Entities;
using WebApi.Persistence;
using WebApi.Services.Common;
using WebApi.ViewModels;
using WebApi.ViewModels.Filters;

namespace WebApi.Services
{
    public class TestimonialService
    {
        private readonly UnitOfWork _uow;
        private readonly LoggedInUser _user;
        private readonly TokenService _tokenService;
        //private readonly IConfiguration _configs;
        private readonly Type _testimonialType = typeof(Testimonial);
        private readonly UploadService _upload;

        public TestimonialService(UnitOfWork uow, TokenService tokenService,
            //IConfiguration configs,
            LoggedInUser user, UploadService upload)
        {
            _uow = uow;
            _tokenService = tokenService;
            //_configs = configs;
            _user = user;
            _upload = upload;
        }

        public async Task<Testimonial> Save(TestimonialVm testimonial)
        {
            var existing = testimonial.Id.HasValue && testimonial.Id > 0 ? await _uow.Testimonials.GetDetails(testimonial.Id.Value) : null;

            var testiNew = Map(testimonial, existing ?? new Testimonial());
            if (existing is null)
            {
                testiNew.CreatedDate = testiNew.UpdatedDate;
                testiNew.Active = true;
                testiNew = await _uow.Testimonials.Add(testiNew);
            }

            await _uow.CompleteAsync();

            return testiNew;
        }

        public async Task<DataPageModel<TestimonialVm>> Filter(FilterBase filter)
        {
            var data = await _uow.Testimonials.Filter(filter);

            return new DataPageModel<TestimonialVm>
            {
                Data = data.Data.ToList().Select(x => Map(x, new TestimonialVm())),
                Count = data.Count,
                PageCount = data.PageCount,
            };
        }

        internal async Task<TestimonialVm> GetTestimonial(int id, bool forEdit = false)
        {
            var testimonial = await _uow.Testimonials.GetDetails(id);

            return testimonial is null
                ? throw new Exception($"Testimonial with Id:{id} not found")
                : Map(testimonial, new TestimonialVm(), forEdit);
        }

        private Testimonial Map(TestimonialVm testimonial, Testimonial existing)
        {
            var curTime = DateTime.UtcNow;

            existing.Name = testimonial.Name;
            existing.Title = testimonial.Title;
            existing.Comments = testimonial.Comments;
            existing.UpdatedDate = curTime;

            var tempImg = testimonial.Images.FirstOrDefault();
            if (tempImg is not null && !string.IsNullOrEmpty(tempImg.Token))
            {
                existing.ProfilePhoto = _tokenService.ParseToken<FileToken>(tempImg.Token)?.RelativePath;
            }

            return existing;
        }

        private TestimonialVm Map(Testimonial source, TestimonialVm dest, bool forEdit = false)
        {
            dest.Id = source.Id;
            dest.Name = source.Name;
            dest.Title = source.Title;
            dest.Comments = source.Comments;
            dest.ProfilePhoto = source.ProfilePhoto;
            dest.Active = source.Active;
            dest.UpdatedDate = source.UpdatedDate;
            dest.CreatedDate = source.CreatedDate;

            if (!string.IsNullOrEmpty(source.ProfilePhoto))
            {
                dest.ProfilePhoto = _upload.GetFullPath(source.ProfilePhoto);
                if (forEdit)
                {
                    dest.Images.Add(new FileToken
                    {
                        FullPath = dest.ProfilePhoto,
                        RelativePath = source.ProfilePhoto,
                        Token = _tokenService.CreateToken(new FileModel
                        {
                            RelativePath = source.ProfilePhoto,
                            FullPath = dest.ProfilePhoto,
                        })
                    });
                }
            }

            return dest;
        }

        internal async Task<TestimonialVm> ChangeTestinonailStatus(int id, bool active)
        {
            var testimonial = await _uow.Testimonials.FindOne(x => x.Id == id)
                ?? throw new AppBadRequestException($"Testimonial with id:{id} for logged in user not found.");

            if (testimonial.Active != active)
            {
                testimonial.Active = active;
                await _uow.CompleteAsync();
            }
            return Map(testimonial, new TestimonialVm());
        }



    }
}
