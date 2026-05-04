using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryManagement.Domain;

namespace LibraryManagement.Domain.Entities
{
    public class Author:BaseEntity
    { //yazar bilgileri(varlıklar )
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Bio { get; set; } = string.Empty;

        //bir yazarın birden fazla kitabı olabilir, bu nedenle BookAuthor koleksiyonu, yazarın yazdığı kitapları temsil eder. 
        public ICollection<BookAuthor>? BookAuthors { get; set; }
    }
}
