using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryManagement.Domain.Entities;
namespace LibraryManagement.Domain.Entities
{
    public class Book: BaseEntity
    {
        public string Title { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public string ISBN { get; set; } = string.Empty;

        public int? CategoryId { get; set; }
        public  Category? Category { get; set; }

        public int? PublishedYear { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Stok adedi 0'dan küçük olamaz!")]
        public int StockCount { get; set; }
        public bool IsAvailable => StockCount > 0;
    }
}
