using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JWTDemo.Models
{
    public class JwtModel
    {
        int _nAccId, _nExpireMin;
        string _sUserName, _iss, _aud;
        DateTime _exp, _nbf = DateTime.Now;
        //_dExpireTime
        /// <summary>
        /// account.id
        /// </summary>
        public int AccId { get => _nAccId; set => _nAccId = value; }
        public int ExpireMin { get => _nExpireMin; set => _nExpireMin = value; }
        public string UserName { get => _sUserName; set => _sUserName = value; }
        public string iss { get => _iss; set => _iss = value; }
        public string aud { get => _aud; set => _aud = value; }

        // public DateTime ExpireTime { get => _dExpireTime; set => _dExpireTime = value; }
        /// <summary>
        /// expire datetime
        /// </summary>
        public DateTime exp { get => _nbf.AddMinutes(_nExpireMin); }
        /// <summary>
        /// create datetime
        /// </summary>
        public DateTime nbf { get => _nbf; }
    }
    public enum JWTStatus
    {
        Valid, Timeout, Invalid
    }
}
