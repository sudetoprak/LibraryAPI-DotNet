using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryManagement.Domain.Entities;
namespace LibraryManagement.Domain.Entities
{
    public class Book: BaseEntity
    {
        public string Title { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public string ISBN { get; set; } = string.Empty;

        public int StockCount { get; set; } 
        public bool IsAvailable => StockCount > 0; 
    }
}
