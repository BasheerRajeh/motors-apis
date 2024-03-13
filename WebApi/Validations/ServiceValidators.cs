using FluentValidation;
using WebApi.ViewModels;

namespace WebApi.Validations
{
    public class ServiceValidators
    {
        public class AddServiceValidators : AbstractValidator<ServiceVm>
        {
            public AddServiceValidators()
            {
                RuleFor(x => x.Title).NotNull().Length(1, 100);
                RuleFor(x => x.TitleKz).NotNull().Length(1, 100);
                RuleFor(x => x.TitleRu).NotNull().Length(1, 100);
                RuleFor(x => x.Description).NotNull().Length(1, 4000);
                RuleFor(x => x.DescriptionKz).NotNull().Length(1, 4000);
                RuleFor(x => x.DescriptionRu).NotNull().Length(1, 4000);
            }
        }
    }
}
