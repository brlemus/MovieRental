using Microsoft.Extensions.DependencyInjection;
using MovieRental.Core.Events;
using MovieRental.Core.Events.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieRental.Handlers
{
    public class NetCoreEventContainer : IEventDispatcher
    {
        private readonly IServiceProvider _serviceProvider;
        public NetCoreEventContainer(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task Dispatch<TEvent>(TEvent eventToDispatch) where TEvent : IDomainEvent
        {
            if (eventToDispatch.GetType() == typeof(PriceUpdated))
            {
                var domainHandler = _serviceProvider.GetServices<IDomainHandler<PriceUpdated>>();
                foreach (var handler in domainHandler)
                {
                    await handler.Handle(eventToDispatch as PriceUpdated);
                }
            }
            if (eventToDispatch.GetType() == typeof(MovieBuyed))
            {
                var domainHandler = _serviceProvider.GetServices<IDomainHandler<MovieBuyed>>();
                foreach (var handler in domainHandler)
                {
                    await handler.Handle(eventToDispatch as MovieBuyed);
                }
            }

            if (eventToDispatch.GetType() == typeof(RentalMovie))
            {
                var domainHandler = _serviceProvider.GetServices<IDomainHandler<RentalMovie>>();
                foreach (var handler in domainHandler)
                {
                    await handler.Handle(eventToDispatch as RentalMovie);
                }
            }

        }
    }
}
