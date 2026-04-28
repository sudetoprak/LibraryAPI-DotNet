using FluentValidation;
using LibraryManagement.Application.DTOs.Responses;

namespace LibraryManagement.Application.Validators
{
    public class RegisterDtoValidator : AbstractValidator<RegisterDto>
    {
        public RegisterDtoValidator()
        {
            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("Ad soyad zorunludur.")
                .MaximumLength(150).WithMessage("Ad soyad en fazla 150 karakter olabilir.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("E-posta adresi zorunludur.")
                .EmailAddress().WithMessage("Geçerli bir e-posta adresi giriniz.")
                .MaximumLength(200).WithMessage("E-posta en fazla 200 karakter olabilir.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Parola zorunludur.")
                .MinimumLength(6).WithMessage("Parola en az 6 karakter olmalıdır.")
                .MaximumLength(100).WithMessage("Parola en fazla 100 karakter olabilir.");
        }
    }
}
