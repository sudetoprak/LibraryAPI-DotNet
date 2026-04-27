using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using LibraryManagement.Application.DTOs.Requests;
namespace LibraryManagement.Application.Validators
{
    public class RentalCreateDtoValidator : AbstractValidator<RentalCreateDto>
    {

        public RentalCreateDtoValidator() {

            RuleFor(x => x.FullName)
               .NotEmpty().WithMessage("Ad soyad zorunludur.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("E-posta zorunludur.")
                .EmailAddress().WithMessage("Geçerli bir e-posta giriniz.");

            RuleFor(x => x.BookId)
                .GreaterThan(0).WithMessage("Geçerli bir kitap seçiniz.");
        }
    }
}
