using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryManagement.Domain;

namespace LibraryManagement.Domain.Entities
{
    public class Author:BaseEntity
    {
       

        public String FullName { get; set; } = string.Empty;
        public String Bio { get; set; } = string.Empty;
        public ICollection<BookAuthor> BookAuthors=new List<BookAuthor>();
    }
}
