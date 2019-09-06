using MovieRental.Core.Interfaces;
using System.Collections.Generic;

namespace MovieRental.Core.Movie
{
    public interface IMovieLikeRepository : IRepository<MovieLikesEntity>
    {
        void RemoveRange(IEnumerable<MovieLikesEntity> rangeToRemove);
        void AddRange(IEnumerable<MovieLikesEntity> rangeToAdd);
    }
}
