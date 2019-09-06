using Microsoft.EntityFrameworkCore;
using MovieRental.Core;
using MovieRental.Core.Events.Common;
using MovieRental.Core.Interfaces;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MovieRental.Persitence
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly MovieDbContext _context;
        protected readonly DbSet<TEntity> _dbSet;
        private readonly IEventDispatcher _domainEventsDispatcher;

        public Repository(MovieDbContext context, IEventDispatcher domainEventsDispatcher)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
            _domainEventsDispatcher = domainEventsDispatcher;
        }

        public IQueryable<TEntity> FindAll()
        {
            return _dbSet;
        }

        public IQueryable<TEntity> FindByCondition(Expression<Func<TEntity, bool>> expression)
        {
            return _dbSet
                .Where(expression);
        }

        public void Create(TEntity entity)
        {
            _dbSet.Add(entity);
        }

        public void Update(TEntity entity)
        {
            _dbSet.Update(entity);
        }

        public void Delete(TEntity entity)
        {
            _dbSet.Remove(entity);
        }

        public async Task SaveAsync()
        {
            await ExecuteDomainEvents();
            await _context.SaveChangesAsync();
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        private async Task ExecuteDomainEvents()
        {
            var domainEntities = _context.ChangeTracker
                .Entries<Entity>()
                .Where(x => x.Entity.DomainEvents != null && x.Entity.DomainEvents.Any());

            var domainEvents = domainEntities
                .SelectMany(x => x.Entity.DomainEvents)
                .ToList();

            domainEntities.ToList()
                .ForEach(entity => entity.Entity.ClearDomainEvents());

            var tasks = domainEvents
                .Select(async (domainEvent) => {
                    await _domainEventsDispatcher.Dispatch(domainEvent);
                });

            await Task.WhenAll(tasks);
        }
    }
}
