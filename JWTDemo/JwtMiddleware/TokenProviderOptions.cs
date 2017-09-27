﻿using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JWTDemo.JwtMiddleware
{
    public class TokenProviderOptions
    {
        public string Path { get; set; } = "/api/Login/Login";

        public string Issuer { get; set; }

        public string Audience { get; set; }

        public TimeSpan Expiration { get; set; } = TimeSpan.FromMinutes(3600);

        public SigningCredentials SigningCredentials { get; set; }
        public string Key { get; set; }
    }

}
