using MovieRental.Core.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieRental.Utils
{
    public interface ITokenFactory
    {
        string UserIdClaim { get; }
        string RoleClaim { get; }
        string GenerateToken(string userId, Role role);
        string GetUser();
        string GetRole();
    }
}
