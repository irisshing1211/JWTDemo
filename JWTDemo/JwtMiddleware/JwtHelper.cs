﻿using JWT;
using JWT.Algorithms;
using JWT.Serializers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JWTDemo.Models;
using Newtonsoft.Json;
using JWTDemo.DAL;
using Microsoft.AspNetCore.Http;
using JWTDemo.JwtMiddleware;
using JWTDemo.Data;

namespace JWTDemo.JWTHepler
{
    public class JwtHelper
    {
        private string _token;
        private byte[] _key;
        private AccountDAL _accountDAL;
        // private TokenProviderOptions _options;
        private JwtSetting _setting;

        /// <summary>
        /// for vaildate -> re-generate token, account dal for check the account still exist
        /// </summary>
        /// <param name="context">current request context</param>
        /// <param name="accountDAL">account dal</param>
        ///<param name="setting"></param>
        public JwtHelper(HttpContext context, AccountDAL accountDAL, JwtSetting setting)
        {
            context.Request.Headers.TryGetValue("Authorization", out var token);
            _token = token;
            _token = _token.Replace("Bearer ", "");
            _accountDAL = accountDAL;
            _setting = setting;
            _key = Convert.FromBase64String(setting.Key);
        }
        /// <summary>
        /// for Generate token
        /// </summary>
        /// <param name="accountDAL"></param>
        /// <param name="setting"></param>
        public JwtHelper(AccountDAL accountDAL, JwtSetting setting)
        {
            _accountDAL = accountDAL;
            _setting = setting;
            _key = Convert.FromBase64String(setting.Key);
        }
        /// <summary>
        /// generate token by user name and accessible api list
        /// </summary>
        /// <param name="userName">account.username</param>
        /// <param name="apis">accessible api list</param>
        /// <returns>token</returns>
        public string GenerateToken(Account acc)//, List<string> apis)
        {

            var payload = new JwtModel
            {
                AccId = acc.ID,
                ExpireMin = _setting.Expire,
                UserName = acc.UserName,
                aud = _setting.audience,
                iss = _setting.issuer
                //Apis = apis
            };

            IDateTimeProvider provider = new UtcDateTimeProvider();
            var now = provider.GetNow();
            var exp = now.AddMinutes(_setting.Expire);

            var unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc); // or use JwtValidator.UnixEpoch
            var secondsSinceEpoch = Math.Round((now - unixEpoch).TotalSeconds);

            payload.nbf= Math.Round((now - unixEpoch).TotalSeconds);
            payload.exp = Math.Round((exp - unixEpoch).TotalSeconds);
            //new Dictionary<string, object>
            //{
            //    { "UserName", userName },
            //    { "apis", apis },
            //    { "exp", secondsSinceEpoch },
            //};


            IJwtAlgorithm algorithm = new HMACSHA256Algorithm();
            IJsonSerializer serializer = new JsonNetSerializer();
            IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
            IJwtEncoder encoder = new JwtEncoder(algorithm, serializer, urlEncoder);

            var token = encoder.Encode(payload, _key);
            return token;
        }
        /// <summary>
        /// vaildate current token
        /// </summary>
        /// <returns>vaild, timeout, invaild</returns>
        public JWTStatus VaildateToken()
        {
            if (string.IsNullOrEmpty(_token))
                return JWTStatus.Invalid;

            var jwtobj = DecodeToken();

            if (jwtobj == null)
                return JWTStatus.Invalid;

            //check expire
            //if (jwtobj.exp <= DateTime.Now)
            //    return JWTStatus.Timeout;

            return JWTStatus.Valid;
        }

        /// <summary>
        /// refresh token with check if user exist and refresh api list
        /// </summary>
        /// <returns></returns>
        public string RefreshToken()
        {
            try
            {
                if (string.IsNullOrEmpty(_token))
                    throw new Exception();

                var jwtobj = DecodeToken();

                //if fail to decode
                if (jwtobj == null)
                    throw new Exception();

                var acc = _accountDAL.Get(jwtobj.UserName);

                if (acc == null)
                    throw new Exception();

                //  var apis = _accountDAL.GetApiList(acc.ID);

                return GenerateToken(acc);//, apis);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public bool CheckApiPermission(string apiName)
        {
            var jwtmodel = DecodeToken();
            return _accountDAL.CheckPermission(jwtmodel.AccId, apiName);
            //return jwtmodel.Apis.Contains(apiName);
        }

        /// <summary>
        /// decode token to readable object
        /// </summary>
        /// <returns></returns>
        private JwtModel DecodeToken()
        {
            try
            {
                IJsonSerializer serializer = new JsonNetSerializer();
                IDateTimeProvider provider = new UtcDateTimeProvider();
                IJwtValidator validator = new JwtValidator(serializer, provider);
                IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
                IJwtDecoder decoder = new JwtDecoder(serializer, validator, urlEncoder);

                var json = decoder.DecodeToObject<JwtModel>(_token, _key, verify: true);
                return json;//JsonConvert.DeserializeObject<JwtModel>(json);
                // Console.WriteLine(json);
            }
            catch (TokenExpiredException tex)
            {
                return new JwtModel();
            }
            catch (SignatureVerificationException signEx)
            {
                return new JwtModel();
            }
            catch (Exception ex)
            {
                return new JwtModel();
            }
        }
    }
}
