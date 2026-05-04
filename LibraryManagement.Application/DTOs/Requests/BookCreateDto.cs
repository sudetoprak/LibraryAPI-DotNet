
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.Application.DTOs.Requests
{
    public class BookCreateDto
    {
        [Required(ErrorMessage = "Kitap başlığı boş bırakılamaz.")]
        [StringLength(200, ErrorMessage = "Kitap başlığı en fazla 200 karakter olabilir.")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Yazar adı zorunludur.")]
        public string Author { get; set; } = string.Empty;

        [Required(ErrorMessage = "ISBN numarası gereklidir.")]
        public string ISBN { get; set; } = string.Empty;

        [Range(0, int.MaxValue, ErrorMessage = "Stok adedi 0'dan küçük olamaz.")]
        public int StockCount { get; set; }

        public string PhotoUrl { get; set; } = string.Empty;

        public IFormFile? Photo { get; set; }

        public int? CategoryId { get; set; }

        public int? PublishedYear { get; set; }
    }
}




