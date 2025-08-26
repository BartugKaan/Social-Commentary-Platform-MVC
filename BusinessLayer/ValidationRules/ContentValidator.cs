using EntityLayer.Concrete;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.ValidationRules
{
    public class ContentValidator : AbstractValidator<Content>
    {
        public ContentValidator()
        {
            RuleFor(x => x.ContentValue)
                .NotEmpty().WithMessage("Ýçerik boþ olamaz!")
                .MinimumLength(10).WithMessage("Ýçerik en az 10 karakter olmalýdýr!")
                .MaximumLength(1000).WithMessage("Ýçerik en fazla 1000 karakter olmalýdýr!");
        }
    }
}