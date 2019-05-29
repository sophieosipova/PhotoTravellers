using System;
using System.Collections.Generic;
using System.Text;

namespace SharedAutorizationOptions
{
    public interface ITokenGenerator
    {
        string GenerateRefreshToken(string id, string userName);

        string GenerateAccessToken(string id, string userName);
        bool ValidateAccessToken(string token);

        bool ValidateRefreshToken(string token);
        //  string GetUsernameFromToken(string token);
    }
}
