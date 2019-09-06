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
    public class PriceUpdatedHandler : IDomainHandler<PriceUpdated>
    {
        private readonly IRepository<PriceUpdateLogEntity> _priceUpdateLogRepository;
        private readonly ITokenFactory _tokenFactory;

        public PriceUpdatedHandler(IRepository<PriceUpdateLogEntity> priceUpdateLogRepository, ITokenFactory tokenFactory)
        {
            _priceUpdateLogRepository = priceUpdateLogRepository;
            _tokenFactory = tokenFactory;
        }

        public async Task Handle(PriceUpdated @event)
        {
            _priceUpdateLogRepository.Create(new PriceUpdateLogEntity
            {
                MovieId = @event.Movie.Id,
                MovieName = @event.Movie.Name,
                PriceBefore = @event.LastPrice,
                PriceAfter = @event.Movie.Price,
                PriceSaleBefore = @event.LastPriceSale,
                PriceSaleAfter = @event.Movie.SalePrice,
                UserName = _tokenFactory.GetUser()
            });
        }
    }
}
