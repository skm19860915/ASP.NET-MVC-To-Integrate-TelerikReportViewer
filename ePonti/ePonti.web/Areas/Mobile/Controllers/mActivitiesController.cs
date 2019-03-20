using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ePonti.BOL;
using System.Diagnostics;

namespace ePonti.web.Areas.Mobile.Controllers
{
    [Authorize]
    public class mActivitiesController : ePonti.web.Controllers._baseMVCController
    {

        ePontiv2Entities db = new ePontiv2Entities();

        // GET: Mobile/mActivities
        public ActionResult Index()
        {
            int siteuser = base.siteuserid;
            int siteusercompanyid = base.siteusercompanyid;
            Debug.WriteLine(base.siteuserid);
            Debug.WriteLine(base.siteusercompanyid);

            ViewBag.SiteUserActivities = db.GetActivitiesBySiteUserID(siteuserid);

            return View();
        }
    }
}
