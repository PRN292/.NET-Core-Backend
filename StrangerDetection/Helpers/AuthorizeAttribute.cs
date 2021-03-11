using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using StrangerDetection.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StrangerDetection.Helpers
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]

    public class AuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        private int role;

        public AuthorizeAttribute(int role)
        {
            this.role = role;
        }

        public AuthorizeAttribute()
        {
            this.role = -1;
        }
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = (TblAccount)context.HttpContext.Items["User"];
            if (user == null || user.IsLogin == false)
            {
                context.Result = new JsonResult(new { message = "Unauthorized" })
                { StatusCode = StatusCodes.Status401Unauthorized };
            }
            else if (this.role != -1)
            {
                if (this.role != user.RoleId)
                {
                    context.Result = new JsonResult(new { message = "Unauthorized" })
                    { StatusCode = StatusCodes.Status403Forbidden };
                }
            }
            
        }
    }
}
