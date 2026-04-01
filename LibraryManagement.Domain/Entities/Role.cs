using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryManagement.Domain;


namespace LibraryManagement.Domain.Entities
{
    public class Role: BaseEntity
    {
        public string Name { get; set; }=string.Empty;
        public string Description { get; set; } = string.Empty;

        public ICollection<User> Users { get; set; } = new List<User>();

    }
}
