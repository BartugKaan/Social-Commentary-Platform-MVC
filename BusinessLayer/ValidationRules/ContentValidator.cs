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
                .NotEmpty().WithMessage("��erik bo� olamaz!")
                .MinimumLength(10).WithMessage("��erik en az 10 karakter olmal�d�r!")
                .MaximumLength(1000).WithMessage("��erik en fazla 1000 karakter olmal�d�r!");
        }
    }
}