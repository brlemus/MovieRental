using System;
using System.Collections.Generic;
using System.Text;

namespace MovieRental.Core.Movie
{
    public class PriceUpdateLogEntity : LogEntity
    {
        public int MovieId { get; set; }
        public string MovieName { get; set; }
        public double PriceBefore { get; set; }
        public double PriceAfter { get; set; }
        public double PriceSaleBefore { get; set; }
        public double PriceSaleAfter { get; set; }
    }
}
