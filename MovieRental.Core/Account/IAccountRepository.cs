using MovieRental.Core.Interfaces;
using System.Threading.Tasks;

namespace MovieRental.Core.Account
{
    public interface IAccountRepository : IRepository<AccountEntity>
    {
        Task<AccountEntity> GetByUserName(string userName);
        Task<AccountEntity> GetByUserNameAndPassword(string userName, string password);
        Task CreateAccount(AccountEntity accountEntity);
    }
}
