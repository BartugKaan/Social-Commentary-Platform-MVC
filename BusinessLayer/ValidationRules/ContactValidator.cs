using EntityLayer.Concrete;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.ValidationRules
{
    public class ContactValidator : AbstractValidator<Contact>
    {

        public ContactValidator()
        {
            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage("Ad Soyad kısmı boş geçilemez");
            RuleFor(x => x.UserEmail)
                .NotEmpty().WithMessage("Mail kısmı boş geçilemez")
                .EmailAddress().WithMessage("Lütfen geçerli bir mail adresi giriniz");
            RuleFor(x => x.Subject)
                .NotEmpty().WithMessage("Konu kısmı boş geçilemez")
                .MinimumLength(3).WithMessage("Lütfen en az 3 karakter girişi yapınız");
            RuleFor(x => x.Messsage)
                .NotEmpty().WithMessage("Mesaj kısmı boş geçilemez")
                .MinimumLength(10).WithMessage("Lütfen en az 10 karakter girişi yapınız");
        }
    }
}
