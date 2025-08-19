using EntityLayer.Concrete;
using FluentValidation;

namespace BusinessLayer.ValidationRules
{
    public class CategoryValidatior : AbstractValidator<Category>
    {
        public CategoryValidatior()
        {
            RuleFor(x => x.CategoryName)
                .NotEmpty().WithMessage("Kategori ismi boş olamaz!");
            RuleFor(x => x.CategoryDescription)
                .NotEmpty().WithMessage("Kategori açıklaması boş olamaz!");
            RuleFor(x => x.CategoryName)
                .MinimumLength(5).WithMessage("Kategori ismi en az 3 karakter olmalıdır!")
                .MaximumLength(50).WithMessage("Kategori ismi en fazla 50 karakter olmalıdır!");              

        }
    }
}
