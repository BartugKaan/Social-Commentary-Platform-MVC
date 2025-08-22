using EntityLayer.Concrete;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.ValidationRules
{
    public class AboutValidatior : AbstractValidator<About>
    {
        public AboutValidatior()
        {
            RuleFor(x => x.AboutDetails1)
                .NotEmpty().WithMessage("Lütfen Hakkında kısmını boş geçmeyiniz");
            RuleFor(x => x.AboutDetails2)
                .NotEmpty().WithMessage("Lütfen Hakkında kısmını boş geçmeyiniz");
            //RuleFor(x => x.AboutImage1).NotEmpty().WithMessage("Lütfen Hakkında kısmını boş geçmeyiniz");
            //RuleFor(x => x.AboutImage2).NotEmpty().WithMessage("Lütfen Hakkında kısmını boş geçmeyiniz");
        }
    }
}
