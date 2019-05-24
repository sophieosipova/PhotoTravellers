﻿using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharedAutorizationOptions
{
    public class AuthorizationOptions
    {
            public string ISSUER { get; set; }
            public string AUDIENCE { get; set; }

            const string KEY = "AutorizationServiceSecretKey";
            public double RefreshTokenLifetime { get; } = 30;
            public double AccessTokenLifetime { get; } = 2;

            public SymmetricSecurityKey GetSymmetricSecurityKey()
            {
                return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
            }

            public TokenValidationParameters GetParameters()
            {
                return new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidIssuer = ISSUER,
                    ValidateAudience = true,
                    ValidAudience = AUDIENCE,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    IssuerSigningKey = GetSymmetricSecurityKey()
                };
            }

      
    }
}
