using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JWTDemo.JwtMiddleware
{
    public class JwtSetting
    {
        string _key, _issuer, _audience;
        int _expire;

        public string Key { get => _key; set => _key = value; }
        public string issuer { get => _issuer; set => _issuer = value; }
        public string audience { get => _audience; set => _audience = value; }
        public int Expire { get => _expire; set => _expire = value; }
    }
}
