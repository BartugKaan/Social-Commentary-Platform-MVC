using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MvcProjeKampi.Filters
{
    public class WriterAuthorizationAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if(HttpContext.Current.Session["WriterMail"] == null ||
                HttpContext.Current.Session["WriterId"] == null)
            {
                filterContext.Result = new RedirectToRouteResult(
                    new RouteValueDictionary
                    {
                        {"controller", "WriterLogin" },
                        {"action", "Index"   }
                    });
                return;
            }
            base.OnActionExecuting(filterContext);
        }
    }
}