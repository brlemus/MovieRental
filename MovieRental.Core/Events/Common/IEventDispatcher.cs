using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MovieRental.Core.Events.Common
{
    public interface IEventDispatcher
    {
        Task Dispatch<TEvent>(TEvent eventToDispatch) where TEvent : IDomainEvent;
    }
}
