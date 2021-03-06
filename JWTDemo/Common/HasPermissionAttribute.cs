﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using JWTDemo.JWTHepler;
using System.Threading.Tasks;
using JWTDemo.Controllers;

namespace JWTDemo
{
    public class HasPermissionAttribute : ActionFilterAttribute
    {
        private string _permission;

        public HasPermissionAttribute(string permission)
        {
            this._permission = permission;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var controller = (BaseController)filterContext.Controller;
            var db = controller._db;
            var options = controller._jwtSetting;
            var jwthelper = new JwtHelper(filterContext.HttpContext, new DAL.AccountDAL(db), options);
            if (!jwthelper.CheckApiPermission(_permission))
            {
                // If this user does not have the required permission then redirect to login page
                //var url = new UrlHelper(filterContext.HttpContext.Request.HttpContex);
                //var loginUrl = url.Content("/Account/Login");
                //filterContext.HttpContext.Response.Redirect(loginUrl, true);
                throw new UnauthorizedAccessException();
            }
        }
    }
}
