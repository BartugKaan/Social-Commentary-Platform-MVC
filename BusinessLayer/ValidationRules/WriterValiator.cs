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
                .NotEmpty().WithMessage("Yazar İsmi boş olamaz!")
                .MinimumLength(2).WithMessage("Yazar İsmi en az 2 karakter olmalıdır!")
                .MaximumLength(50).WithMessage("Yazar İsmi en fazla 50 karakter olmalıdır!");
            
            RuleFor(x => x.WriterSurname)
                .NotEmpty().WithMessage("Yazar Soyadı boş olamaz!")
                .MinimumLength(2).WithMessage("Yazar Soyadı en az 2 karakter olmalıdır!")
                .MaximumLength(50).WithMessage("Yazar Soyadı en fazla 50 karakter olmalıdır!");
            
            RuleFor(x => x.WriterMail)
                .NotEmpty().WithMessage("Yazar Maili boş olamaz!")
                .EmailAddress().WithMessage("Lütfen geçerli bir e-posta adresi giriniz!");
            
            RuleFor(x => x.WriterPassword)
                .NotEmpty().WithMessage("Yazar Parolası boş olamaz!")
                .MinimumLength(6).WithMessage("Yazar Parolası en az 6 karakter olmalıdır!")
                .MaximumLength(200).WithMessage("Yazar Parolası en fazla 200 karakter olmalıdır!");
            
            // WriterTitle opsiyonel, sadece dolu ise kontrol et
            RuleFor(x => x.WriterTitle)
                .MaximumLength(50).WithMessage("Yazar Ünvanı en fazla 50 karakter olmalıdır!")
                .When(x => !string.IsNullOrEmpty(x.WriterTitle));
                
            // WriterAbout opsiyonel
            RuleFor(x => x.WriterAbout)
                .MaximumLength(150).WithMessage("Hakkında açıklaması en fazla 150 karakter olmalıdır!")
                .When(x => !string.IsNullOrEmpty(x.WriterAbout));
        }
    }
}
