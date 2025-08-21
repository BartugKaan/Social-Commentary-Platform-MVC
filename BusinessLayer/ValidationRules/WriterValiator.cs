using EntityLayer.Concrete;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.ValidationRules
{
    public class WriterValiator : AbstractValidator<Writer>
    {
        public WriterValiator()
        {
            RuleFor(x => x.WriterName)
                .NotEmpty().WithMessage("Yazar İsmi boş olamaz!");
            RuleFor(x => x.WriterSurname)
                .NotEmpty().WithMessage("Yazar Soyadı boş olamaz!");
            RuleFor(x => x.WriterMail)
                .NotEmpty().WithMessage("Yazar Maili boş olamaz!")
                .EmailAddress().WithMessage("Lütfen geçerli bir e-posta adresi giriniz!");
            RuleFor(x => x.WriterName)
                .MinimumLength(2).WithMessage("Yazar İsmi en az 2 karakter olmalıdır!")
                .MaximumLength(50).WithMessage("Yazar İsmi en fazla 50 karakter olmalıdır!");
            RuleFor(x => x.WriterSurname)
                .MinimumLength(4).WithMessage("Yazar Soyadı en az 4 karakter olmalıdır!")
                .MaximumLength(50).WithMessage("Yazar Soyadı en fazla 50 karakter olmalıdır!");
            RuleFor(x => x.WriterPassword)
                .NotEmpty().WithMessage("Yazar Parolası boş olamaz!")
                .MinimumLength(6).WithMessage("Yazar Parolası en az 6 karakter olmalıdır!")
                .MaximumLength(50).WithMessage("Yazar Parolası en fazla 50 karakter olmalıdır!")
                .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[a-zA-Z\d]{6,}$")
                .WithMessage("Yazar Parolası en az bir büyük harf, bir küçük harf ve bir rakam içermelidir!");
            RuleFor(x => x.WriterTitle)
                .NotEmpty().WithMessage("Yazar Ünvanı boş olamaz!")
                .MaximumLength(50).WithMessage("Yazar Ünvanı en fazla 50 karakter olmalıdır!");

        }
    }
}
