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
        
        public int BookId { get; set; }
        public Book? Book { get; set; }
        public int AuthorId { get; set; }
        public Author? Author { get; set; }
    }
}
