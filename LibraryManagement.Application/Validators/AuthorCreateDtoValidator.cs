using FluentValidation;
using LibraryManagement.Application.DTOs.Requests;

namespace LibraryManagement.Application.Validators
{
    public class AuthorCreateDtoValidator : AbstractValidator<AuthorCreateDto>
    {
        public AuthorCreateDtoValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("Yazar adı zorunludur.")
                .MaximumLength(100).WithMessage("Yazar adı en fazla 100 karakter olabilir.");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Yazar soyadı zorunludur.")
                .MaximumLength(100).WithMessage("Yazar soyadı en fazla 100 karakter olabilir.");

            RuleFor(x => x.Bio)
                .MaximumLength(1000).WithMessage("Biyografi en fazla 1000 karakter olabilir.");
        }
    }
}
