using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ePonti.BOL;
using System.Diagnostics;
using ePonti.BOL.Models;

namespace ePonti.web.Areas.Mobile.Controllers
{
    [Authorize]
    public class mCalendarController : ePonti.web.Controllers._baseMVCController
    {

        ePontiv2Entities db = new ePontiv2Entities();

        // GET: Mobile/mCalendar
        public ActionResult Index()
        {
            int siteuser = base.siteuserid;
            int siteusercompanyid = base.siteusercompanyid;
            Debug.WriteLine(base.siteuserid);
            Debug.WriteLine(base.siteusercompanyid);

            ViewBag.SiteUserActivities = db.GetActivitiesBySiteUserID(siteuserid);

            return View();
        }
        public JsonResult getDashboard()
        {
            List<CalendarActivities> dashboardActivities = new List<CalendarActivities>();
            var ActivitiesList = db.GetCalendarBySiteUserID(siteuserid).ToList();
            string colorcode = db.SiteUserOptions.Where(p => p.SiteUserID == siteuserid).Select(p => p.ColorCode).FirstOrDefault();
            foreach (var q in ActivitiesList)
            {
                CalendarActivities da = new CalendarActivities();
                da.id = Convert.ToString(q.ViewID);
                da.title = q.Title;
                da.start = q.DateTime;
                DateTime ed = Convert.ToDateTime(q.DateTime);
                for (decimal i = 0.25M; i <= q.Duration; i = i + (decimal)0.25)
                {
                    ed = ed.AddMinutes(15);
                }
                da.end = ed.ToString();
                da.color = colorcode;
                da.description = q.ActivityType;
                dashboardActivities.Add(da);
            }
            var rows = dashboardActivities.ToArray();
            return Json(rows, JsonRequestBehavior.AllowGet);
        }
    }
}
