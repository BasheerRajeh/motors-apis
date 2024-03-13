using FluentValidation;
using WebApi.ViewModels;

namespace WebApi.Validations
{
    public class TestimonailValidator
    {
        public class AddTestimonailValidator : AbstractValidator<TestimonialVm>
        {
            public AddTestimonailValidator()
            {
                RuleFor(x => x.Name).NotNull().Length(1, 100);
                RuleFor(x => x.Title).NotNull().Length(1, 100);
                RuleFor(x => x.Comments).NotNull().Length(1, 600);
            }
        }
    }
}
