using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieRental.Dtos
{
    public class GetMovieAdminDto
    {


        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;
        public SorterAdmin SortBy { get; set; } = SorterAdmin.availability;
        public Order Order { get; set; } = Order.Asc;
    }

    public enum SorterAdmin
    {
        availability = 1,
        unavailability = 0
    }

}
