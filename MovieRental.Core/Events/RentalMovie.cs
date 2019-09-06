using MovieRental.Core.Events.Common;
using MovieRental.Core.Movie;
using System;
using System.Collections.Generic;
using System.Text;

namespace MovieRental.Core.Events
{
    public class RentalMovie : IDomainEvent
    {
        public MovieEntity Movie { get; set; }
        public int Quantity { get; set; }
        public string UserName { get; set; }
    }
}
