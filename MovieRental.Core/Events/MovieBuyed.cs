using MovieRental.Core.Events.Common;
using MovieRental.Core.Movie;

namespace MovieRental.Core.Events
{
    public class MovieBuyed : IDomainEvent
    {
        public MovieEntity Movie { get; set; }
        public int Quantity { get; set; }
        public string UserName { get; set; }


    }
}
