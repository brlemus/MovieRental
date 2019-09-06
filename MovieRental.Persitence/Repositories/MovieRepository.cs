using Microsoft.EntityFrameworkCore;
using MovieRental.Common.Dtos;
using MovieRental.Core.Events.Common;
using MovieRental.Core.Movie;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieRental.Persitence.Repositories
{
    public class MovieRepository : Repository<MovieEntity>, IMovieRepository
    {

        public MovieRepository(MovieDbContext context, IEventDispatcher domainEventsDispatcher) : base(context, domainEventsDispatcher)
        {
        }
        public async Task CreateMovieAsync(MovieEntity movie)
        {
            Create(movie);
            await SaveAsync();
        }

        public async Task DeleteMovie(MovieEntity movie)
        {
            Delete(movie);
            await SaveAsync();
        }

        public async Task<IEnumerable<MovieEntity>> GetAllMoviesChunkAsync(PaginationDto pagination)
        {
            var property = TypeDescriptor.GetProperties(typeof(MovieEntity)).Find(pagination.SortBy, true);
            var query = pagination.Order == "Desc"
                ? FindAll().OrderByDescending(a => property.GetValue(a))
                : FindAll().OrderBy(a => property.GetValue(a));
            return await query
                .Skip((pagination.PageNumber - 1) * pagination.PageSize)
                .Take(pagination.PageSize)
                .ToListAsync();
        }

        public async Task<MovieEntity> GetByIdAsync(int id)
        {
            return await FindByCondition(p => p.Id == id).Include(a => a.AccountLikes).FirstOrDefaultAsync();
        }

        public async Task<MovieEntity> GetByNameAsync(string name)
        {
            return await FindByCondition(p => p.Name == name).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<MovieEntity>> GetByAvailabilityAsync(bool availability, PaginationDto pagination)
        {
            var property = await FindByCondition(p => p.Availability == availability).ToListAsync();
            //var property = TypeDescriptor.GetProperties(typeof(MovieEntity)).Find(pagination.SortBy, true);
            return pagination.Order == "Desc"
                ? property.OrderByDescending(a => a.Availability).Skip((pagination.PageNumber - 1) * pagination.PageSize)
                .Take(pagination.PageSize)
                : property.OrderBy(a => a.Availability).Skip((pagination.PageNumber - 1) * pagination.PageSize)
                .Take(pagination.PageSize);
        }

        public async Task<IEnumerable<MovieEntity>> GetMoviesAsync()
        {
            return await FindAll().OrderBy(a => a.Name).ToListAsync();
        }

        public async Task UpdateMovieAsync(MovieEntity movie)
        {

            Update(movie);
            await SaveAsync();
        }

        public async Task UpdateAllMovieAsync(int id, MovieEntity item2)
        {
            var item1 = await GetByIdAsync(id);
            item1.Name = item1.Name != item2.Name && item2.Name != null? item2.Name : item1.Name;
            item1.Stock = item1.Stock != item2.Stock && item2.Stock !=0? item2.Stock : item1.Stock;
            item1.Price = item1.Price != item2.Price && item2.Price  != 0? item2.Price : item1.Price;
            item1.SalePrice = item1.SalePrice != item2.SalePrice && item2.SalePrice != 0? item2.SalePrice : item1.SalePrice;
            item1.Description = item1.Description != item2.Description && item2.Description  != null? item2.Description : item1.Description;

            Update(item1);
            await SaveAsync();
        }

        
    }
}
