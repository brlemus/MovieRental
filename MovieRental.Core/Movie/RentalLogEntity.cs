using System;
using System.Collections.Generic;
using System.Text;

namespace MovieRental.Core.Movie
{
    public class RentalLogEntity : LogEntity
    {
        public int MovieId { get; set; }
        public string MovieName { get; set; }
        public int Quantity { get; set; }

        public string CalculatePenalty(DateTime dateRental)
        {
            var res = "";
            int aux = 0;
            var startDate = dateRental;
            var endDate = DateTime.Now;

            var interval = endDate.Subtract(startDate).Days;

            //int dayResp = endDay - startDay;
            if (interval > 5)
            {
                aux = (interval - 5) * 2;
                return res = "monetary penalty is: $" + aux + " *monetary penalty is $2 for each day after 5 days of the date return.";
            }
            else
            {
                return res = "monetary penalty is: $" + 0 + " *monetary penalty is $2 for each day after 5 days of the date return.";
            }
           
        }
    }
}
