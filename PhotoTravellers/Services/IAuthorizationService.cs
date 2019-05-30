using SharedModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhotoTravellers.Services
{
    public interface IAutorizationService
    {
        Task<UsersToken> Login(User user);
        Task<UsersToken> RefreshTokens(UsersToken token);
        Task<bool> ValidateToken(string accessToken);

        Task<UsersToken> Logout(string UserId);
    }
}
