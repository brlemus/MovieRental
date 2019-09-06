using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MovieRental.Core.Account;
using MovieRental.Dtos;
using MovieRental.Utils;
using MovieRental.Common;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MovieRental.Controllers
{
    [Route("account")]
    public class AccountController : BaseController
    {
        private readonly IAccountRepository _accountRepository;
        private readonly ITokenFactory _tokenFactory;

        public AccountController(IAccountRepository accountRepository, ITokenFactory tokenFactory)
        {
            _accountRepository = accountRepository;
            _tokenFactory = tokenFactory;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody]RegisterAccountDto item)
        {
            var account = await _accountRepository.GetByUserName(item.UserName);
            if (account != null)
                return Error($"Account with username :{item.UserName} already registered.");

            await _accountRepository.CreateAccount(new AccountEntity
            {
                UserName = item.UserName,
                Password = item.Password.ToSha256(),
                Role = item.Role
            });
            return Ok();
        }

        private IActionResult Error(string v)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody]LoginDto item)
        {
            var account = await _accountRepository.GetByUserNameAndPassword(item.UserName, item.Password.ToSha256());
            if (account == null)
                return Error($"Account with username :{item.UserName} not found.");

            var token = _tokenFactory.GenerateToken(account.UserName, account.Role);

            return token == null ? Unauthorized() : Ok(token);
        }

    }
}
