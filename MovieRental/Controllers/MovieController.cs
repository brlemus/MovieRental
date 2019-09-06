using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieRental.Common.Dtos;
using MovieRental.Core.Account;
using MovieRental.Core.Interfaces;
using MovieRental.Core.Movie;
using MovieRental.Dtos;
using MovieRental.Utils;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MovieRental.Controllers
{

   [Route("Movies")]
    public class MovieController : BaseController
    {
        private readonly IMovieRepository _movieRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly ITokenFactory _tokenFactory;
        private readonly IRepository<RentalLogEntity> _rentalLogRepository;

        public MovieController(IMovieRepository movieRepository, IAccountRepository accountRepository, ITokenFactory tokenFactory, IRepository<RentalLogEntity> rentalLogRepository)
        {
            _movieRepository = movieRepository;
            _accountRepository = accountRepository;
            _tokenFactory = tokenFactory;
            _rentalLogRepository = rentalLogRepository;
        }


        [HttpGet]
        [Route("SeeAllMovieAdminUser")]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> GetAllMovieAdmin([FromQuery]GetMovieAdminDto item = null)
        {
            var x = item.SortBy.GetHashCode();
            var userNameClaim = _tokenFactory.GetUser();
            if (userNameClaim == "user" && x==0)
                return Unauthorized();

            
            bool b = Convert.ToBoolean(x);
            item = item ?? new GetMovieAdminDto();
            var result = await _movieRepository.GetByAvailabilityAsync(b, new PaginationDto
            {
                PageNumber = item.PageNumber,
                PageSize = item.PageSize,
                Order = item.Order.ToString()
            });

            return Ok(result.Select(a => new GetMovieResponseDto
            {
                Id = a.Id,
                Name = a.Name,
                Stock = a.Stock,
                Price = a.Price,
                SalePrice = a.SalePrice,
                Availability = a.Availability,
                Likes = a.Likes,
                Description = a.Description
            }));
        }

        [HttpGet]
        [Route("AllMovie")]
        [AllowAnonymous]
        public async Task<IActionResult> Get([FromQuery]GetMovieDto item = null)
        {
            item = item ?? new GetMovieDto();
            var result = await _movieRepository.GetAllMoviesChunkAsync(new PaginationDto
            {
                PageNumber = item.PageNumber,
                PageSize = item.PageSize,
                SortBy = item.SortBy.ToString(),
                Order = item.Order.ToString()
            });

            return Ok(result.Select(a => new GetMovieResponseDto
            {
                Id = a.Id,
                Name = a.Name,
                Stock = a.Stock,
                Price = a.Price,
                SalePrice = a.SalePrice,
                Availability = a.Availability,
                Likes = a.Likes,
                Description = a.Description
            }));
        }

        [HttpGet]
        [Route("movie/searchByName")]
        [AllowAnonymous]
        public async Task<IActionResult> GetByName([FromQuery]string name)
        {
            var result = await _movieRepository.GetByNameAsync(name);
            if (result == null)
                return NotFound($"Movie with Name: {name} not found");
            return Ok(new GetMovieResponseDto
            {
                Id = result.Id,
                Name = result.Name,
                Stock = result.Stock,
                Price = result.Price,
                SalePrice = result.SalePrice,
                Availability = result.Availability,
                Likes = result.Likes,
                Description = result.Description

            });
        }

        [HttpGet]
        [Route("returnMovie")]
        [Authorize(Roles = "Admin")]
        //[AllowAnonymous]
        public async Task<IActionResult> GetReturnMovie(int id, [FromQuery]int quantity)
        {
            RentalLogEntity obj = new RentalLogEntity();
            var movie = await _movieRepository.GetByIdAsync(id);
            var dateRental = _rentalLogRepository.FindByCondition(x => (x.Id == id)).FirstOrDefault();

            if (movie == null && dateRental == null)
                return MovieNotFound(id);

            if (!movie.ReturnMovie(quantity))
                return Error("Invalid data.");

            await _movieRepository.UpdateMovieAsync(movie);

            var resp = obj.CalculatePenalty(dateRental.Date);

            return Ok(resp);
        }

        [HttpPost]
        [Route("createNewMovie")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Post([FromBody]AddMovieDto item)
        {
            var movie = await _movieRepository.GetByNameAsync(item.Name);

            if (movie != null)
                return Error($"Name is already in use: {item.Name}");

            movie = new MovieEntity().Create(item.Name, item.Stock, item.Price, item.SalePrice, item.Availability, item.Description);
            if (movie == null)
                return Error($"Invalid data.");

            await _movieRepository.CreateMovieAsync(movie);
            return Ok();
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        [Route("updateMovie")]
        public async Task<IActionResult> PostUpdate([FromQuery]int id, [FromBody]AddMovieDto item)
        {
            //var movie = await _movieRepository.GetByIdAsync(id);

            //if (movie == null)
            //    return Error($"movie does not exist");

            MovieEntity movieU = new MovieEntity()
            {
                Name = item.Name,
                Stock = item.Stock,
                Price = item.Price,
                SalePrice = item.SalePrice,
                Description = item.Description
            };
            //movie = new MovieEntity().Update(movie, item.Name, item.Price, item.Stock, item.SalePrice, item.Availability, item.Description, id);
            //if (movie == null)
            //return Error($"Invalid data.");

            await _movieRepository.UpdateAllMovieAsync(id, movieU);
            return Ok();
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        [Route("updateMovieAvailability")]
        public async Task<IActionResult> Stock([FromQuery]int id, bool availability)
        {
            var movie = await _movieRepository.GetByIdAsync(id);

            if (movie == null)
                return MovieNotFound(id);

            if (!movie.ChangeAvailability(availability))
                return Error("Invalid data.");

            await _movieRepository.UpdateMovieAsync(movie);
            return Ok();
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        [Route("changePriceMovie")]
        public async Task<IActionResult> Price([FromQuery]int id, double price, double salePrice)
        {
            var movie = await _movieRepository.GetByIdAsync(id);

            if (movie == null)
                return MovieNotFound(id);

            var result = movie.ChangePrice(price, salePrice);
            if (!result)
                return Error("Invalid price.");

            await _movieRepository.UpdateMovieAsync(movie);
            return Ok();
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        [Route("deleteMovie")]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _movieRepository.GetByIdAsync(id);
            if (product == null)
                return MovieNotFound(id);

            await _movieRepository.DeleteMovie(product);
            return Ok();
        }

        [HttpPut]
        [Authorize(Roles = "Admin,User")]
        [Route("{id}/like")]
        public async Task<IActionResult> ToggleLike(int id)
        {
            var movie = await _movieRepository.GetByIdAsync(id);
            if (movie == null)
                return MovieNotFound(id);

            var userNameClaim = _tokenFactory.GetUser();
            if (userNameClaim == null)
                return Unauthorized();

            var account = await _accountRepository.GetByUserName(userNameClaim);
            if (account == null)
                return Error("Account not found.");

            movie.ToggleLike(account);
            await _movieRepository.UpdateMovieAsync(movie);

            return Ok();
        }

        [HttpPut]
        [Authorize(Roles = "Admin,User")]
        [Route("{id}/buy")]
        public async Task<IActionResult> Buy(int id, [FromQuery]int quantity)
        {
            var movie = await _movieRepository.GetByIdAsync(id);

            if (movie == null)
                return MovieNotFound(id);

            var result = movie.Buy(quantity);
            if (!result)
                return Error("The quantity exceeds movies stock.");

            await _movieRepository.UpdateMovieAsync(movie);
            return Ok();
        }

        [HttpPut]
        [Authorize(Roles = "Admin,User")]
        [Route("{id}/rental")]
        public async Task<IActionResult> Rental(int id, [FromQuery]int quantity)
        {
            var movie = await _movieRepository.GetByIdAsync(id);

            if (movie == null)
                return MovieNotFound(id);

            var result = movie.Rental(quantity);
            if (!result)
                return Error("The quantity exceeds movies stock.");

            await _movieRepository.UpdateMovieAsync(movie);
            return Ok();
        }

        private IActionResult MovieNotFound(int id)
        {
            return NotFound($"Movie with Id: {id} not found.");
        }
    }
}
