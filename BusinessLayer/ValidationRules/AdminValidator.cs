using EntityLayer.Concrete;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.ValidationRules
{
    public class AdminValidator : AbstractValidator<Admin>
    {
        public AdminValidator()
        {
            RuleFor(x => x.AdminUserName)
                .NotEmpty().WithMessage("Kullanıcı adı boş geçilemez")
                .MinimumLength(5).WithMessage("Lütfen en az 3 karakter girişi yapınız");
            RuleFor(x => x.AdminPassword)
                .NotEmpty().WithMessage("Parola boş gelemez")
                .MinimumLength(6).WithMessage("Lütfen en az 6 karakter girişi yapınız");
        }
    }
}
