using MovieRental.Common.Dtos;
using MovieRental.Core.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MovieRental.Core.Movie
{
    public interface IMovieRepository : IRepository<MovieEntity>
    {
        Task<IEnumerable<MovieEntity>> GetMoviesAsync();
        Task<IEnumerable<MovieEntity>> GetAllMoviesChunkAsync(PaginationDto pagination);
        Task<MovieEntity> GetByIdAsync(int id);
        Task<MovieEntity> GetByNameAsync(string name);

        Task<IEnumerable<MovieEntity>> GetByAvailabilityAsync(bool availability, PaginationDto pagination);
        Task UpdateMovieAsync(MovieEntity movie);

        Task UpdateAllMovieAsync(int id, MovieEntity movie);
        Task CreateMovieAsync(MovieEntity movie);
        Task DeleteMovie(MovieEntity movie);

    }
}
