using FluentValidation;
using System;
using WebApi.ViewModels;

namespace WebApi.Validations
{
    public class AuthValidators
    {
    }
    public class RegisterValidator : AbstractValidator<RegisterUserVm>
    {
        public RegisterValidator()
        {
            RuleFor(x => x.FirstName).NotNull().Length(1, 100);
            RuleFor(x => x.LastName).NotNull().Length(1, 100);
            RuleFor(x => x.Email).NotNull().EmailAddress();
            RuleFor(x => x.Contact).NotNull().Length(1, 100);
            RuleFor(x => x.Password).NotNull().MinimumLength(6);
            RuleFor(x => x.ConfirmPassword).NotNull().Equal(x => x.Password).WithMessage("Password and Confirm Password Must Match");
        }
    }
    public class ChangePasswordValidator : AbstractValidator<ChangePasswordModelVm>
    {
        public ChangePasswordValidator()
        {
            RuleFor(x => x.Password).NotNull().MinimumLength(6);
            RuleFor(x => x.ConfirmPassword).NotNull().Equal(x => x.Password).WithMessage("Password and Confirm Password Must Match");
        }
    }
}
