using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryManagement.Application.DTOs.Responses;


namespace LibraryManagement.Application.DTOs.Responses
{
    public class PagedResult<T>
    {
        public List<T> Items { get; set; } = new();
        public int TotalCount { get; set; }
        public int TotalSize { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}
