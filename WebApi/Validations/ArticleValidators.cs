using FluentValidation;
using WebApi.Models.Entities;
using WebApi.ViewModels;

namespace WebApi.Validations
{
    public class ArticleValidators
    {
        public class AddArticleValidators : AbstractValidator<ArticleVm>
        {
            public AddArticleValidators()
            {
                RuleFor(x => x.Title).NotNull().Length(1, 100);
                RuleFor(x => x.SubTitle).NotNull().Length(1, 300);
                RuleFor(x => x.ParagraphOne).NotNull().Length(1, 4000);
                RuleFor(x => x.ParagraphTwo).MaximumLength(4000);
            }
        }
    }
}