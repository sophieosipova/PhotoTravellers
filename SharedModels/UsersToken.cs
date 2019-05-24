using System;
using System.Collections.Generic;
using System.Text;

namespace SharedModels
{
    public class UsersToken
    {
        public string RefreshToken { get; set; }
        public string AccessToken { get; set; }
        public string UserName { get; set; }
        public string UserId { get; set; }
    }
}
