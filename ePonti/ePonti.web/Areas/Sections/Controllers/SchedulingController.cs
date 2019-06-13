using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ePonti.BOL;
using ePonti.web.Models;
using ePonti.BOL.Models;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;


namespace ePonti.web.Areas.Sections.Controllers
{
    [Authorize]
    public class SchedulingController : ePonti.web.Controllers._baseMVCController
    {
        private ePontiv2Entities db = new ePontiv2Entities();

        // GET: Sections/Scheduling
        public ActionResult Index()
        {
            int siteCoID = base.siteusercompanyid;
            ViewBag.CoSiteUsers = db.GetSiteUsersBySiteCoID(siteCoID);
           // ViewBag.Events = GetEvents();
            return View();
        }

     
        public ActionResult GetEvents([DataSourceRequest] DataSourceRequest request, List<int> SiteUserIdArray)
        {
            List<SchedulerEvents> dashboardActivities = new List<SchedulerEvents>();

            if (SiteUserIdArray != null && SiteUserIdArray.Count > 0)
            {
                foreach (int id in SiteUserIdArray)
                {
                    var ActivitiesList = db.GetCalendarBySiteUserID(id).ToList();
                    string colorcode = db.SiteUserOptions.Where(p => p.SiteUserID == id).Select(p => p.ColorCode).FirstOrDefault();
                    foreach (var q in ActivitiesList)
                    {
                        SchedulerEvents da = new SchedulerEvents();
                        da.id = Convert.ToString(q.ViewID);
                        da.Title = q.Title;
                        da.Start = Convert.ToDateTime(q.DateTime);
                        DateTime ed = Convert.ToDateTime(q.DateTime);
                        for (decimal i = 0.25M; i <= q.Duration; i = i + (decimal)0.25)
                        {
                            ed = ed.AddMinutes(15);
                        }
                        da.End = ed;
                        da.color = colorcode;
                        da.Description = q.ActivityType;
                        dashboardActivities.Add(da);
                    }
                }


            }
            else
            {
                var ActivitiesList = db.GetCalendarBySiteUserID(siteuserid).ToList();
                string colorcode = db.SiteUserOptions.Where(p => p.SiteUserID == siteuserid).Select(p => p.ColorCode).FirstOrDefault();
                foreach (var q in ActivitiesList)
                {
                    SchedulerEvents da = new SchedulerEvents();
                    da.id = Convert.ToString(q.ViewID);
                    da.Title = q.Title;
                    da.Start = Convert.ToDateTime(q.DateTime);
                    DateTime ed = Convert.ToDateTime(q.DateTime);
                    for (decimal i = 0.25M; i <= q.Duration; i = i + (decimal)0.25)
                    {
                        ed = ed.AddMinutes(15);
                    }
                    da.End = ed;
                    da.color = colorcode;
                    da.Description = q.ActivityType;
                    dashboardActivities.Add(da);
                }
            }
            DataSourceResult result = dashboardActivities.ToDataSourceResult(request);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        //public JsonResult getFilteredSchedulingEvents(List<int> SiteUserIdArray)
        //{
        //    List<CalendarActivities> dashboardActivities = new List<CalendarActivities>();
        //    foreach (int id in SiteUserIdArray)
        //    {
        //        var ActivitiesList = db.GetCalendarBySiteUserID(id).ToList();
        //        string colorcode = db.SiteUserOptions.Where(p => p.SiteUserID == id).Select(p => p.ColorCode).FirstOrDefault();
        //        foreach (var q in ActivitiesList)
        //        {
        //            CalendarActivities da = new CalendarActivities();
        //            da.id = Convert.ToString(q.ViewID);
        //            da.title = q.Title;
        //            da.start = q.DateTime;
        //            DateTime ed = Convert.ToDateTime(q.DateTime);
        //            for (decimal i = 0.25M; i <= q.Duration; i = i + (decimal)0.25)
        //            {
        //                ed = ed.AddMinutes(15);
        //            }
        //            da.end = ed.ToString();
        //            da.color = colorcode;
        //            da.description = q.ActivityType;
        //            dashboardActivities.Add(da);
        //        }
        //    }
           
        //    ViewBag.Events = dashboardActivities;
        //     var rows = dashboardActivities.ToArray();
        //     return Json(rows, JsonRequestBehavior.AllowGet);
        //}

        //public List<SchedulerEvents> GetEvents()
        //{
        //    List<SchedulerEvents> dashboardActivities = new List<SchedulerEvents>();


        //    var ActivitiesList = db.GetCalendarBySiteUserID(siteuserid).ToList();
        //    string colorcode = db.SiteUserOptions.Where(p => p.SiteUserID == siteuserid).Select(p => p.ColorCode).FirstOrDefault();
        //    foreach (var q in ActivitiesList)
        //    {
        //        SchedulerEvents da = new SchedulerEvents();
        //        da.id = Convert.ToString(q.ViewID);
        //        da.Title = q.Title;
        //        da.Start = Convert.ToDateTime(q.DateTime);
        //        DateTime ed = Convert.ToDateTime(q.DateTime);
        //        for (decimal i = 0.25M; i <= q.Duration; i = i + (decimal)0.25)
        //        {
        //            ed = ed.AddMinutes(15);
        //        }
        //        da.End = ed;
        //        da.color = colorcode;
        //        da.Description = q.ActivityType;
        //        dashboardActivities.Add(da);
        //    }
        //    return dashboardActivities;
        //}

       

    }
}