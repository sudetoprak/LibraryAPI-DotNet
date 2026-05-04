using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryManagement.Domain.Entities;

namespace LibraryManagement.Domain.Entities
{ //kiralama sınıfı (varlıklar)
    public class Rental : BaseEntity
    {

        public enum RentalStatus
        {
            Active,
            Returned,
            Overdue
        }
        //kiralama nın hangi kitaba ait 
        public int BookId { get; set; }

        //kitabın hangi kullanıcıya ait
        public int UserId { get; set; }


        //User ve book ilişkisi
        public User? User { get; set; }
        public Book? Book { get; set; }


        //Kitabın ne zmn kiralandıgı
        public DateTime RentalDate { get; set; }

        //Kitabın iade durumu 
        public bool IsReturned { get; set; } = false;

        //iade tarihi
        public DateTime ReturnDate { get; set; }


        // Kiralamanın aktif, iade edildi veya gecikti durumunu tutar.
        public RentalStatus Status { get; set; } = RentalStatus.Active;

        // kiralayn kişinin bilgileri 
        public string BorrowerName { get; set; } = string.Empty;
        public string BorrowerEmail { get; set; } = string.Empty;


        //Kitabın teslim edilmesi gereken tarih
        public DateTime? DueDate { get; set; }


        // gecikme olursa hesaplanan ucret tutarı 
        public Decimal LateFee { get; set; }

    }
}