using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryManagement.Domain;
using LibraryManagement.Domain.Entities;

namespace LibraryManagement.Domain.Entities
{ // Kitap ve yazar arasındaki çoktan çoğa ilişkiyi temsil eden bir varlık sınıfıdır. Bu sınıf, bir kitabın birden fazla yazarı olabileceği ve bir yazarın da birden fazla kitabı olabileceği durumları yönetmek için kullanılır.
    public class BookAuthor:BaseEntity
    {
        //kitap ve yazar arasındaki ilişkiyi temsil eden iki özellik içerir: BookId ve AuthorId. Bu özellikler, ilgili kitabın ve yazarın kimliklerini tutar. Ayrıca, Book ve Author türünde iki navigasyon özelliği de bulunur. Bu özellikler, Entity Framework gibi bir ORM (Object-Relational Mapping) aracı tarafından kullanılarak, ilişkili kitap ve yazar verilerine kolayca erişim sağlar.
        public int BookId { get; set; }
        public Book? Book { get; set; }
        public int AuthorId { get; set; }
        public Author? Author { get; set; }
    }
}
