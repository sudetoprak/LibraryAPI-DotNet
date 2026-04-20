using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryManagement.Application.Interfaces;
namespace LibraryManagement.Application.DTOs.Responses
{
   public  class AuthorDto
    {//Bu sınıf, Author varlığının temel bilgilerini içeren bir DTO'dur. Id, FullName ve Bio gibi özellikler içerir.
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Bio { get; set; } = string.Empty;
    }
}
