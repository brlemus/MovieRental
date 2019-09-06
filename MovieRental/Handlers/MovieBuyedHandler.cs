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
    public class MovieBuyedHandler : IDomainHandler<MovieBuyed>
    {
        private readonly IRepository<PurchaseLogEntity> _purchaseLogRepository;
        private readonly ITokenFactory _tokenFactory;

        public MovieBuyedHandler(IRepository<PurchaseLogEntity> purchaseLogRepository, ITokenFactory tokenFactory)
        {
            _purchaseLogRepository = purchaseLogRepository;
            _tokenFactory = tokenFactory;
        }

        public async Task Handle(MovieBuyed @event)
        {
            _purchaseLogRepository.Create(new PurchaseLogEntity
            {
                MovieId = @event.Movie.Id,
                MovieName = @event.Movie.Name,
                Quantity = @event.Quantity,
                UserName = _tokenFactory.GetUser()
            });
        }
    }
}
