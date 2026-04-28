using FluentValidation;
using LibraryManagement.Application.DTOs.Requests;

namespace LibraryManagement.Application.Validators
{
    public class UserCreateDtoValidator : AbstractValidator<UserCreateDto>
    {
        public UserCreateDtoValidator()
        {
            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("Ad soyad zorunludur.")
                .MaximumLength(150).WithMessage("Ad soyad en fazla 150 karakter olabilir.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("E-posta zorunludur.")
                .EmailAddress().WithMessage("Geçerli bir e-posta adresi giriniz.")
                .MaximumLength(200).WithMessage("E-posta en fazla 200 karakter olabilir.");
        }
    }
}
