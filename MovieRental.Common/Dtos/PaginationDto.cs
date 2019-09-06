using System;
using System.Collections.Generic;
using System.Text;

namespace MovieRental.Common.Dtos
{
    public class PaginationDto
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string SortBy { get; set; }
        public string Order { get; set; }

    }
}
