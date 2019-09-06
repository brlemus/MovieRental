using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieRental.Dtos
{
    public class AddMovieDto
    {
        public string Name { get; set; }
        public int Stock { get; set; }
        public double Price { get; set; }
        public double SalePrice { get; set; }

        public bool Availability { get; set; } = true;
        public string Description { get; set; }

    }
}
