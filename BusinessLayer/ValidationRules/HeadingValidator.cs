using EntityLayer.Concrete;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.ValidationRules
{
    public class HeadingValidator : AbstractValidator<Heading>
    {
        public HeadingValidator()
        {
            RuleFor(x => x.HeadingName)
                .NotEmpty().WithMessage("Başlık adı boş olamaz!")
                .MinimumLength(2).WithMessage("Başlık adı en az 2 karakter olmalıdır!")
                .MaximumLength(100).WithMessage("Başlık adı en fazla 100 karakter olmalıdır!");
            //RuleFor(x => x.Category)
            //    .NotNull().WithMessage("Kategori boş olamaz!")
            //    .WithMessage("Lütfen bir kategori seçiniz!");
            //RuleFor(x => x.Writer)
            //    .NotNull().WithMessage("Yazar boş olamaz!")
            //    .WithMessage("Lütfen bir yazar seçiniz!");
        }
    }
}
