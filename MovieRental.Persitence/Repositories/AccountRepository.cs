using Microsoft.EntityFrameworkCore;
using MovieRental.Core.Account;
using MovieRental.Core.Events.Common;
using System.Threading.Tasks;

namespace MovieRental.Persitence.Repositories
{
    public class AccountRepository : Repository<AccountEntity>, IAccountRepository
    {
        public AccountRepository(MovieDbContext context, IEventDispatcher domainEventsDispatcher) : base(context, domainEventsDispatcher)
        {
        }
        public async Task CreateAccount(AccountEntity accountEntity)
        {
            Create(accountEntity);
            await SaveAsync();
        }

        public async Task<AccountEntity> GetByUserName(string userName)
        {
            return await FindByCondition(p => p.UserName == userName).FirstOrDefaultAsync();
        }

        public async Task<AccountEntity> GetByUserNameAndPassword(string userName, string password)
        {
            return await FindByCondition(p => p.UserName == userName && p.Password == password).FirstOrDefaultAsync();
        }
    }
}
