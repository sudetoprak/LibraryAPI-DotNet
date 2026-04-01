using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryManagement.Domain.Entities;

namespace LibraryManagement.Domain.Entities
{
    public class Rental : BaseEntity
    {

        public int BookId { get; set; }
        public int UserId { get; set; }

        public User? User { get; set; }
        public Book? Book { get; set; }

        public DateTime RentalDate { get; set; }
        public bool IsReturned { get; set; } = false;

        public DateTime ReturnDate { get; set; } 

        public DateTime? DueDate { get; set; }
        public Decimal LateFee { get; set; }

    }
}