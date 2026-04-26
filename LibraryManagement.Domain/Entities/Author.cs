using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryManagement.Domain;

namespace LibraryManagement.Domain.Entities
{
    public class Author:BaseEntity
    { // Author sınıfı, kitap yazarlarını temsil eder. Bu sınıf, yazarların adlarını, biyografilerini ve yazdıkları kitapları takip etmek için kullanılır. Author sınıfı, kitaplarla bire çok ilişki içindedir, yani bir yazar birden fazla kitaba sahip olabilir ve bir kitap da birden fazla yazara sahip olabilir (örneğin, ortak yazarlık durumlarında). Bu ilişkiyi yönetmek için genellikle bir ara tablo (örneğin, BookAuthor) kullanılır.
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Bio { get; set; } = string.Empty;

        //bir yazarın birden fazla kitabı olabilir, bu nedenle BookAuthor koleksiyonu, yazarın yazdığı kitapları temsil eder. 
        public ICollection<BookAuthor>? BookAuthors { get; set; }
    }
}
