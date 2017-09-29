using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JWTDemo.Models
{
    public class JwtModel
    {
        int _nAccId, _nExpireMin;
        string _sUserName;
        DateTime _dExpireTime;
        List<string> _lApis;

        public int AccId { get => _nAccId; set => _nAccId = value; }
        public int ExpireMin { get => _nExpireMin; set => _nExpireMin = value; }
        public string UserName { get => _sUserName; set => _sUserName = value; }
        public DateTime ExpireTime { get => _dExpireTime; set => _dExpireTime = value; }
        //   public List<string> Apis { get => _lApis; set => _lApis = value; }
    }
    public enum JWTStatus
    {
        Valid, Timeout, Invalid
    }
}
