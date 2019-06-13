using ePonti.web.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ePonti.web
{
    public class UpdateNavigationCookieAttribute: ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var currentUrl = HttpContext.Current.Request.RawUrl;

            NavigationHelper.PushUrl(currentUrl);

            base.OnActionExecuted(filterContext);
        }
    }
}