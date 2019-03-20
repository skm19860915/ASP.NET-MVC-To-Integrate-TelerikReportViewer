using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ePonti.web.Areas.Options.Controllers
{
    public class EmailOptionsController : Controller
    {
        // GET: Options/EmailOptions
        public ActionResult Index()
        {
            return View();
        }
    }
}