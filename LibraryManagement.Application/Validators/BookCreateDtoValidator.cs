using LibraryManagement.Application.DTOs.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
namespace LibraryManagement.Application.Validators
{
    public class BookCreateDtoValidator : AbstractValidator<BookCreateDto>
    {
        public BookCreateDtoValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Kitap başlığı boş bırakılamaz.")
                .MaximumLength(200).WithMessage("Kitap başlığı en fazla 200 karakter olabilir.");

            RuleFor(x => x.Author)
                .NotEmpty().WithMessage("Yazar adı zorunludur.");

            RuleFor(x => x.ISBN)
                .NotEmpty().WithMessage("ISBN numarası zorunludur.");

            RuleFor(x => x.StockCount)
                .GreaterThanOrEqualTo(0).WithMessage("Stok adedi 0'dan küçük olamaz.");
        }
    }
}
