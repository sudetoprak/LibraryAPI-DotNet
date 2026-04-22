using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryManagement.Domain;

namespace LibraryManagement.Domain.Entities
{
    public class Category : BaseEntity
    {  //Kategori sınıfı, kitapların kategorilerini temsil eder. Bu sınıf, kitapların hangi kategoriye ait olduğunu belirlemek ve kategorilere göre kitapları organize etmek için kullanılır. Kategori sınıfı, kitaplarla bire çok ilişki içindedir, yani bir kategori birden fazla kitaba sahip olabilir.

        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public ICollection<Book>? Books { get; set; } = new List<Book>();

    }
}
