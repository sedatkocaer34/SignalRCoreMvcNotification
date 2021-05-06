using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SignalRCoreMvcNotification.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRCoreMvcNotification.Security.Jwt
{
    public class AuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = (UserContextModel)context.HttpContext.Items["User"];
            if (user == null)
            {
                context.Result = new RedirectToActionResult("Login", "Home", null);
            }
        }
    }
}
