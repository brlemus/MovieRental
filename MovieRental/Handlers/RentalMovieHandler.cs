using MovieRental.Core.Events;
using MovieRental.Core.Events.Common;
using MovieRental.Core.Interfaces;
using MovieRental.Core.Movie;
using MovieRental.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieRental.Handlers
{
    public class RentalMovieHandler : IDomainHandler<RentalMovie>
    {
        private readonly IRepository<RentalLogEntity> _rentalLogRepository;
        private readonly ITokenFactory _tokenFactory;

        public RentalMovieHandler(IRepository<RentalLogEntity> rentalLogRepository, ITokenFactory tokenFactory)
        {
            _rentalLogRepository = rentalLogRepository;
            _tokenFactory = tokenFactory;
        }

        public async Task Handle(RentalMovie @event)
        {
            _rentalLogRepository.Create(new RentalLogEntity
            {
                MovieId = @event.Movie.Id,
                MovieName = @event.Movie.Name,
                Quantity = @event.Quantity,
                UserName = _tokenFactory.GetUser()
            });
        }
    }
}
