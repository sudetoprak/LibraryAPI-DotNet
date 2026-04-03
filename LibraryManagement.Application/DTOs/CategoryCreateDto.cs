using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace LibraryManagement.Application.DTOs
{
    public class CategoryCreateDto
    {
        [Required(ErrorMessage = "İsim alanı zorunludur")]
        public string Name { get; set; } = string.Empty;
    }
}
