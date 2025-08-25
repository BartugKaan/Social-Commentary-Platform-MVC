using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MvcProjeKampi.Filters
{
    public class AdminAuthorizationAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if(HttpContext.Current.Session["AdminUserName"] == null ||
                HttpContext.Current.Session["AdminId"] == null)
            {
                filterContext.Result = new RedirectToRouteResult(
                    new RouteValueDictionary
                    {
                        {"controller", "Login" },
                        {"action", "Index"   }
                    });
                return;
            }
            base.OnActionExecuting(filterContext);
        }
    }
}