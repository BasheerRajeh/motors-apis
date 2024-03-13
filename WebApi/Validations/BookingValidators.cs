using FluentValidation;
using WebApi.Models.Entities;
using WebApi.ViewModels;

namespace WebApi.Validations
{
    public class BookingValidators
    {
    }
    public class AddBookingValidator : AbstractValidator<BookingVm>
    {
        static readonly string Genders = "male,female";
        static readonly string[] GendersList = Genders.Replace(" ", "").Split(',');
        public AddBookingValidator()
        {
            RuleFor(x => x.CarId).GreaterThan(0);
            RuleFor(x => x.FullName).NotNull().Length(1, 100);
            RuleFor(x => x.Gender).MaximumLength(10);
            RuleFor(x => x.Contact).NotNull().Length(1, 30);
            RuleFor(x => x.Email).NotNull().Length(1, 50);
            //RuleFor(x => x.UserIp).NotNull().Length(1, 30);
            RuleFor(x => x.Comments).MaximumLength(300);

            //RuleFor(x => x.Gender).Must((x) => !string.IsNullOrWhiteSpace(x) && GendersList.Contains(x.ToLower())).WithMessage($"Please select a valid value for gender, options are: {Genders}");
        }
    }
}
