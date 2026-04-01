using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Domain.Entities
{
    public class Author
    {
        public String FullName { get; set; } = string.Empty;
        public String Bio { get; set; } = string.Empty;
    }
}
