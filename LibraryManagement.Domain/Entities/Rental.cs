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

        public enum RentalStatus
        {
            Active,
            Returned,
            Overdue
        }
        // Rental sınıfı, kitap kiralama işlemlerini temsil eder. Bu sınıf, kullanıcıların hangi kitapları kiraladığını, kiralama tarihlerini, iade durumlarını ve geç iade durumlarında uygulanacak ücretleri takip etmek için kullanılır. Rental sınıfı, kullanıcılar ve kitaplar arasında bire çok ilişki içindedir, yani bir kullanıcı birden fazla kitabı kiralayabilir ve bir kitap da birden fazla kullanıcı tarafından kiralanabilir.
        public int BookId { get; set; }
        public int UserId { get; set; }

        public User? User { get; set; }
        public Book? Book { get; set; }

        public DateTime RentalDate { get; set; }
        public bool IsReturned { get; set; } = false;

        public DateTime ReturnDate { get; set; }

        public RentalStatus Status { get; set; } = RentalStatus.Active;
        public DateTime? DueDate { get; set; }
        public Decimal LateFee { get; set; }

    }
}