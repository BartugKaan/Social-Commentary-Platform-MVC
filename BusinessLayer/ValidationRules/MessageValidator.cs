using EntityLayer.Concrete;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.ValidationRules
{
    public class MessageValidator : AbstractValidator<Message>
    {
        public MessageValidator()
        {

            RuleFor(x => x.ReceiverMail)
                .NotEmpty().WithMessage("Alıcı Mail Adresini Boş Geçemezsiniz")
                .EmailAddress().WithMessage("Lütfen Geçerli Bir Mail Adresi Giriniz.");
            RuleFor(x => x.Subject)
                .NotEmpty().WithMessage("Konu Başlığını Boş Geçemezsiniz")
                .Length(3, 50).WithMessage("Lütfen en az 3 en fazla 50 karakter girişi yapınız.");
            RuleFor(x => x.MessageContent)
                .NotEmpty().WithMessage("Mesaj İçeriğini Boş Geçemezsiniz")
                .MinimumLength(5).WithMessage("Lütfen en az 5 karakter girişi yapınız.");
        }
    }
}
