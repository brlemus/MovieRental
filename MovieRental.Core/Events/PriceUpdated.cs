using MovieRental.Core.Events.Common;
using MovieRental.Core.Movie;

namespace MovieRental.Core.Events
{
    public class PriceUpdated : IDomainEvent
    {
        public MovieEntity Movie { get; set; }
        public double LastPrice { get; set; }
        public double LastPriceSale { get; set; }
        public string UserName { get; set; }
    }
}
