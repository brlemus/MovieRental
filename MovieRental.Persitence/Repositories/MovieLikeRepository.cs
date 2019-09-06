using MovieRental.Core.Events.Common;
using MovieRental.Core.Movie;
using System;
using System.Collections.Generic;
using System.Text;

namespace MovieRental.Persitence.Repositories
{
    public class MovieLikeRepository : Repository<MovieLikesEntity>, IMovieLikeRepository
    {
        public MovieLikeRepository(MovieDbContext context, IEventDispatcher domainEventsDispatcher) : base(context, domainEventsDispatcher)
        {
        }
        public void AddRange(IEnumerable<MovieLikesEntity> rangeToAdd)
        {
            _dbSet.AddRange(rangeToAdd);
        }

        public void RemoveRange(IEnumerable<MovieLikesEntity> rangeToRemove)
        {
            _dbSet.RemoveRange(rangeToRemove);
        }
    }
}
