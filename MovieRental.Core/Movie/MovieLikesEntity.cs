using MovieRental.Core.Account;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MovieRental.Core.Movie
{
    public class MovieLikesEntity : Entity
    {

        [Key]
        public int MovieId { get; set; }
        public MovieEntity Movie { get; set; }
        [Key]
        public int AccountId { get; set; }
        public AccountEntity Account { get; set; }

        public static List<MovieLikesEntity> CreateDumpData()
        {
            return new List<MovieLikesEntity>
            {
                new MovieLikesEntity()
                {
                    MovieId = 1,
                    AccountId = 1
                },
                new MovieLikesEntity()
                {
                    MovieId = 2,
                    AccountId = 1
                },
                new MovieLikesEntity()
                {
                    MovieId = 3,
                    AccountId = 1
                },
                new MovieLikesEntity()
                {
                    MovieId = 1,
                    AccountId = 2
                },
            };
        }

    }
}
