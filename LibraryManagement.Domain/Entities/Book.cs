using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
        public ICollection<BookAuthor>? BookAuthors { get; set; } = new List<BookAuthor>();

        [Range(0, int.MaxValue, ErrorMessage = "Stok adedi 0'dan küçük olamaz!")]
        public int StockCount { get; set; }

        [NotMapped]
        public bool IsAvailable => StockCount > 0;

    }
}
