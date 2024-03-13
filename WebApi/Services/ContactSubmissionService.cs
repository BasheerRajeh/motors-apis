using Microsoft.AspNetCore.Mvc;
using WebApi.Common.Exceptions;
using WebApi.Common.Models;
using WebApi.Models.Entities;
using WebApi.Persistence;
using WebApi.Services.Common;
using WebApi.ViewModels;
using WebApi.ViewModels.Filters;

namespace WebApi.Services
{
    public class ContactSubmissionService
    {
        private readonly UnitOfWork _uow;
        private readonly LoggedInUser _user;
        private readonly TokenService _tokenService;
        private readonly Type _contactsubmissionType = typeof(ContactSubmission);
        private readonly UploadService _upload;

        public ContactSubmissionService(UnitOfWork uow, TokenService tokenService,
            LoggedInUser user, UploadService upload)
        {
            _uow = uow;
            _tokenService = tokenService;
            _user = user;
            _upload = upload;
        }

        public async Task<ContactSubmission> Save(ContactSubmissionVm contactSubmission)
        {
            var contactNew = Map(contactSubmission, new ContactSubmission());
            contactNew.CreatedDate = contactNew.UpdatedDate;
            contactNew.Active = true;
            contactNew = await _uow.ContactSubmissions.Add(contactNew);
            await _uow.CompleteAsync();

            return contactNew;
        }

        public async Task<DataPageModel<ContactSubmissionVm>> Filter(FilterBase filter)
        {
            var data = await _uow.ContactSubmissions.Filter(filter);

            return new DataPageModel<ContactSubmissionVm>
            {
                Data = data.Data.ToList().Select(x => Map(x, new ContactSubmissionVm())),
                Count = data.Count,
                PageCount = data.PageCount,
            };
        }

        private ContactSubmission Map(ContactSubmissionVm contactSubmission,
                                      ContactSubmission existing)
        {
            var curTime = DateTime.UtcNow;
            existing.Id = contactSubmission.Id;
            existing.Name = contactSubmission.Name;
            existing.Email = contactSubmission.Email;
            existing.Contact = contactSubmission.Contact;
            existing.Country = contactSubmission.Country;
            existing.UserIp = contactSubmission.UserIp;
            existing.Message = contactSubmission.Message;
            existing.UpdatedDate = curTime;

            return existing;
        }

        private ContactSubmissionVm Map(ContactSubmission source,
                                        ContactSubmissionVm dest)
        {
            dest.Id = source.Id;
            dest.Name = source.Name;
            dest.Email = source.Email;
            dest.Contact = source.Contact;
            dest.Country = source.Country;
            dest.UserIp = source.UserIp;
            dest.Message = source.Message;
            dest.CreatedDate = source.CreatedDate;
            dest.UpdatedDate = source.UpdatedDate;

            return dest;
        }

        internal async Task<ContactSubmissionVm> ChangeStatus(int id, bool active)
        {
            var cs = await _uow.ContactSubmissions.FindOne(x => x.Id == id)
               ?? throw new AppBadRequestException($"ContactSubmission with id:{id} for logged in user not found.");

            if (cs.Active != active)
            {
                cs.Active = active;
                await _uow.CompleteAsync();
            }
            return Map(cs, new ContactSubmissionVm());
        }
    }
}