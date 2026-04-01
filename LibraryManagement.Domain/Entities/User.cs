using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryManagement.Domain.Entities;
namespace LibraryManagement.Domain.Entities
{
    public class User:BaseEntity
    {
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        public int RoleId { get; set; }
        public Role? Role { get; set; }

        public ICollection<Rental>? Rentals { get; set; } = new List<Rental>();
    }
}

