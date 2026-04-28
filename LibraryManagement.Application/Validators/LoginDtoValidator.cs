using FluentValidation;
using LibraryManagement.Application.DTOs.Responses;

namespace LibraryManagement.Application.Validators
{
    public class LoginDtoValidator : AbstractValidator<LoginDto>
    {
        public LoginDtoValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("E-posta zorunludur.")
                .EmailAddress().WithMessage("Geçerli bir e-posta adresi giriniz.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Parola zorunludur.")
                .MinimumLength(6).WithMessage("Parola en az 6 karakter olmalıdır.");
        }
    }
}
