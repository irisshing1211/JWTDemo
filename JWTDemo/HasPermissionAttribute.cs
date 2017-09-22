﻿using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
            if (!CHECK_IF_USER_OR_ROLE_HAS_PERMISSION(_permission))
            {
                // If this user does not have the required permission then redirect to login page
                //var url = new UrlHelper(filterContext.HttpContext.Request.HttpContex);
                //var loginUrl = url.Content("/Account/Login");
                //filterContext.HttpContext.Response.Redirect(loginUrl, true);
                throw new UnauthorizedAccessException();
            }
        }
        private bool CHECK_IF_USER_OR_ROLE_HAS_PERMISSION(string permissionName)
        {
            return true;
            return false;
        }
    }
}
