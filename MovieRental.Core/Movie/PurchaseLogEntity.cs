using System;
using System.Collections.Generic;
using System.Text;

namespace MovieRental.Core.Movie
{
    public class PurchaseLogEntity : LogEntity
    {
        public int MovieId { get; set; }
        public string MovieName { get; set; }
        public int Quantity { get; set; }
    }
}
