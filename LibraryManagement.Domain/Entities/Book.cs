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
    { // Book sınıfı, kitapları temsil eder. Bu sınıf, kitapların başlıklarını, yazarlarını, ISBN numaralarını, kategorilerini, yayın yıllarını ve stok durumlarını takip etmek için kullanılır. Book sınıfı, yazarlarla bire çok ilişki içindedir, yani bir kitap birden fazla yazara sahip olabilir ve bir yazar da birden fazla kitaba sahip olabilir (örneğin, ortak yazarlık durumlarında). Ayrıca, kitaplar kategorilere de ait olabilirler.
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
