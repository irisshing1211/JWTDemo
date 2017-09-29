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

        /// <summary>
        /// secret key
        /// </summary>
        public string Key { get => _key; set => _key = value; }
        /// <summary>
        /// identifies principal that issued the JWT
        /// </summary>
        public string issuer { get => _issuer; set => _issuer = value; }
        /// <summary>
        /// The "aud" (audience) claim identifies the recipients that the JWT is intended for.
        /// Each principal intended to process the JWT MUST identify itself with a value in the audience claim.
        /// If the principal processing the claim does not identify itself with a value 
        /// in the aud claim when this claim is present, then the JWT MUST be rejected
        /// </summary>
        public string audience { get => _audience; set => _audience = value; }
        /// <summary>
        /// expire mins
        /// </summary>
        public int Expire { get => _expire; set => _expire = value; }
    }
}
