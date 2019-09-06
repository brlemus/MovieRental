using MovieRental.Core.Account;
using MovieRental.Core.Events;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace MovieRental.Core.Movie
{
    public class MovieEntity : Entity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; protected set; }
        public string Name { get; set; }
        public int Stock { get; set; }
        public double Price { get; set; }
        public int Likes { get; protected set; }
        public double SalePrice { get; set; }
        public bool Availability { get; set; }
        public string Description { get; set; }


        public ICollection<MovieLikesEntity> AccountLikes { get; set; }

        public MovieEntity Create(string name, int stock, double price, double salePrice, bool availability, string description)
        {
            if (string.IsNullOrEmpty(name.Trim()) || stock < 0 || price < 0 || salePrice < 0 )
                return null;
            return new MovieEntity()
            {
                Name = name,
                Stock = stock,
                Price = price,
                SalePrice = salePrice,
                Description = description,
                Availability = availability

            };
        }

        //public MovieEntity Update(MovieEntity item1, string name, double price, int stock, double salePrice, bool availability, string description, int id)
        //{
        //    MovieEntity movie = new MovieEntity()
        //    {
        //        Id = id,
        //        Name = item1.Name != name ? name : item1.Name,
        //        Price = item1.Price != price ? price :item1.Price,
        //        Stock = item1.Stock != stock ? stock : item1.Stock,
        //        SalePrice = item1.SalePrice != salePrice ? salePrice : item1.SalePrice,
        //        Availability = item1.Availability != availability ? availability: item1.Availability,
        //        Description = item1.Description != description ? description : item1.Description


        //    };
        //    return movie;

        //}

        public void ToggleLike(AccountEntity accountWhoLikes)
        {
            var account = AccountLikes.FirstOrDefault(a => a.AccountId == accountWhoLikes.Id);
            if (account != null)
            {
                Likes--;
                AccountLikes.Remove(account);
            }
            else
            {
                Likes++;
                AccountLikes.Add(new MovieLikesEntity
                {
                    AccountId = accountWhoLikes.Id,
                    MovieId = Id
                });
            }
        }

        public bool Buy(int quantity)
        {
            if (quantity < 1)
                return false;
            if (Stock < quantity)
                return false;
            Stock -= quantity;
            AddDomainEvent(new MovieBuyed() { Movie = this, Quantity = quantity });
            return true;
        }

        public bool Rental(int quantity)
        {
            if (quantity < 1)
                return false;
            if (Stock < quantity)
                return false;
            Stock -= quantity;
            AddDomainEvent(new RentalMovie() { Movie = this, Quantity = quantity });
            return true;
        }

        public bool ReturnMovie(int quantity)
        {
            if (quantity < 1)
                return false;
            //if (Stock < quantity)
            //    return false;
            Stock += quantity;
            //AddDomainEvent(new RentalMovie() { Movie = this, Quantity = quantity });
            return true;
        }

        public bool ChangePrice(double newPrice, double newPriceSale)
        {
            if (newPrice < 0 && newPriceSale < 0)
                return false;
            var lastPrice = Price;
            var lastPriceSale = SalePrice;
            Price = newPrice;
            SalePrice = newPriceSale;
            AddDomainEvent(new PriceUpdated() { Movie = this, LastPrice = lastPrice,  LastPriceSale = lastPriceSale });
            return true;
        }

        public bool ChangeAvailability(bool availability)
        {
            //if (stock < 0)
              //  return false;
            Availability = availability;
            return true;
        }

        public static List<MovieEntity> CreateDumpData()
        {
            return new List<MovieEntity>
            {
                new MovieEntity()
                {
                    Name = "Toy Story 4",
                    Stock = 5,
                    Price = 10,
                    Likes = 2,
                    SalePrice = 16,
                    Availability = true,
                    Description = "Movie Toy Story 4"
                },
                new MovieEntity()
                {
                    Name = "Avengers Endgame",
                    Stock = 7,
                    Price = 15,
                    Likes = 1,
                    SalePrice = 19,
                    Availability = true,
                    Description = "Avengers Endgame"
                },
                new MovieEntity()
                {
                    Name = "John Wick: Chapter 3",
                    Stock = 9,
                    Price = 12,
                    Likes = 1,
                    SalePrice = 29,
                    Availability = true,
                    Description = "John Wick: Chapter 3"
                },
                new MovieEntity()
                {
                    Name = "Once Upon a Time in…Hollywood",
                    Stock = 3,
                    Price = 16,
                    Likes = 3,
                    SalePrice = 20,
                    Availability = true,
                    Description = "Once Upon a Time in…Hollywood"
                },
                new MovieEntity()
                {
                    Name = "Aladdin",
                    Stock = 4,
                    Price = 11,
                    Likes = 2,
                    SalePrice = 25,
                    Availability = true,
                    Description = "Aladdin"
                },
                new MovieEntity()
                {
                    Name = "Godzilla",
                    Stock = 10,
                    Price = 7,
                    Likes = 1,
                    SalePrice = 21,
                    Availability = true,
                    Description = "Godzilla"
                },
                new MovieEntity()
                {
                    Name = "Coco",
                    Stock = 5,
                    Price = 6,
                    Likes = 2,
                    SalePrice = 21.5,
                    Availability = true,
                    Description = "Coco"
                },
                new MovieEntity()
                {
                    Name = "X-Men",
                    Stock = 5,
                    Price = 7.5,
                    Likes = 0,
                    SalePrice = 15.5,
                    Availability = true,
                    Description = "X-Men"
                },
                new MovieEntity()
                {
                    Name = "Lego",
                    Stock = 4,
                    Price = 8.5,
                    Likes = 4,
                    SalePrice = 18.5,
                    Availability = true,
                    Description = "Lego"
                },
                new MovieEntity()
                {
                    Name = "Lion King",
                    Stock = 6,
                    Price = 9.25,
                    Likes = 6,
                    SalePrice = 22.5,
                    Availability = true,
                    Description = "Lion King"
                }
            };
        }

    }
}
