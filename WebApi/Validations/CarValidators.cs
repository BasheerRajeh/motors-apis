using FluentValidation;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using WebApi.ViewModels;
using WebApi.ViewModels.Filters;

namespace WebApi.Validations
{
    public class CarValidators
    {
    }

    public class AddCarValidator : AbstractValidator<CarVm>
    {
        public AddCarValidator()
        {
            RuleFor(x => x.Name).NotNull().Length(1, 100);
            RuleFor(x => x.Battery).NotNull().Length(1, 4000);
            RuleFor(x => x.BatteryKz).NotNull().Length(1, 4000);
            RuleFor(x => x.BatteryRu).NotNull().Length(1, 4000);
            RuleFor(x => x.Performance).NotNull().Length(1, 4000);
            RuleFor(x => x.PerformanceKz).NotNull().Length(1, 4000);
            RuleFor(x => x.PerformanceRu).NotNull().Length(1, 4000);
            RuleFor(x => x.Range).NotNull().Length(1, 4000);
            RuleFor(x => x.RangeKz).NotNull().Length(1, 4000);
            RuleFor(x => x.RangeRu).NotNull().Length(1, 4000);
            RuleFor(x => x.Efficiency).NotNull().Length(1, 4000);
            RuleFor(x => x.EfficiencyKz).NotNull().Length(1, 4000);
            RuleFor(x => x.EfficiencyRu).NotNull().Length(1, 4000);
            RuleFor(x => x.PriceCurrency).NotNull().Length(1, 100);
            RuleFor(x => x.BrandId).GreaterThan(0);
        }
    }
    public class FilterValidator : AbstractValidator<FilterBase>
    {
        public FilterValidator()
        {
            RuleFor(x => x.Page).GreaterThanOrEqualTo(0);
            RuleFor(x => x.PageSize).GreaterThanOrEqualTo(1).LessThanOrEqualTo(100);
        }
    }
}
