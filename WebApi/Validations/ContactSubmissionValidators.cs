using FluentValidation;
using WebApi.Models.Entities;
using WebApi.ViewModels;

namespace WebApi.Validations
{
    public class ContactSubmissionValidators
    {

    }
    public class AddContactSubmissionValidator : AbstractValidator<ContactSubmissionVm>
    {
        public AddContactSubmissionValidator()
        {
            RuleFor(x => x.Name).NotNull().Length(1, 100);
            RuleFor(x => x.Email).NotNull().Length(1, 100);
            RuleFor(x => x.Contact).NotNull().Length(1, 100);
            RuleFor(x => x.Country).NotNull().Length(1, 100);
            //RuleFor(x => x.UserIp).NotNull().Length(1, 100);
            RuleFor(x => x.Message).NotNull().Length(1, 800);
        }
    }
}
