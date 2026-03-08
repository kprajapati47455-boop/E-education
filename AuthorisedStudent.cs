using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace E_education.App_Code
{
    public class AuthorisedStudent:AuthorizeAttribute
    {
        protected override bool  AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext.Session["uid"] == null)

            {
                return false;
            }
            else
            {
                return true;
            }
        }
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectResult("/General/Login");
        }
    }

    
}