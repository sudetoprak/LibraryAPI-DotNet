using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Application.DTOs
{
    //kullanıc tarafından kitap eklemek istediğinde ,kullanıcıdan hangi bilgileri alacağımızı belirtiyoruz
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
    }
}

