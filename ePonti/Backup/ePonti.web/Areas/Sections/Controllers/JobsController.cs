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
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;

namespace ePonti.web.Areas.Sections.Controllers
{
    [Authorize]
    public class JobsController : ePonti.web.Controllers._baseMVCController
    {
        private ePontiv2Entities db = new ePontiv2Entities();

        // GET: Sections/Jobs
        public ActionResult Index()
        {
            ViewBag.ProjectHideStatus = new SelectList(db.CoProjectStatus.Where(p => p.SiteCoID == siteusercompanyid).GroupBy(p => p.ProjectStatusName).Select(c => c.Key).ToList());
            ViewBag.ContactCompanies = db.GetJobCustomersBySiteCoID(siteusercompanyid).Where(s => s.Customer != null && s.Customer.Trim() != "").ToList();
            ViewBag.ProjectStatus = db.GetJobUsedStatusBySiteCoID(siteusercompanyid).ToList();
            //var jobs = db.GetJobsBySiteCoID(siteusercompanyid).ToList();
            return View();
        }
        // Post: Sections/Jobs


        //[HttpPost]
        //public ActionResult Index(string SearchText, string SelectedCompanies, string SelectedProjectStatus, IEnumerable<string> HideStatus = null)
        //{
        //    try
        //    {
        //        ViewBag.ProjectHideStatus = new SelectList(db.CoProjectStatus.Where(p => p.SiteCoID == siteusercompanyid).GroupBy(p => p.ProjectStatusName).Select(c => c.Key).ToList());
        //        ViewBag.ContactCompanies = db.GetJobCustomersBySiteCoID(siteusercompanyid).Where(s => s.Customer != null && s.Customer.Trim() != "").ToList();
        //        ViewBag.ProjectStatus = db.GetJobUsedStatusBySiteCoID(siteusercompanyid).ToList();
        //        var jobs = db.GetJobsBySiteCoID(siteusercompanyid).ToList();
        //        if (SearchText != null && SearchText != "")
        //        {
        //            SearchText = SearchText.Trim().ToLower();
        //            jobs = jobs.Where(p => (p.Project == null ? "" : p.Project).ToLower().Contains(SearchText)).ToList();
        //        }
        //        char[] sep = new char[] { ',' };
        //        if (SelectedCompanies != null && SelectedCompanies != "")
        //            jobs = jobs.Where(p => SelectedCompanies.Split(sep).Contains(Convert.ToString(p.ContactID))).ToList();
        //        if (SelectedProjectStatus != null && SelectedProjectStatus != "")
        //            jobs = jobs.Where(p => SelectedProjectStatus.Split(sep).Contains(Convert.ToString(p.StatusID))).ToList();
        //        if (HideStatus != null)
        //        {
        //            if (HideStatus.Count() > 0)
        //            {
        //                foreach (string sts in HideStatus)
        //                    jobs = jobs.Where(p => p.Status != sts).ToList();
        //            }
        //        }
        //        return View("Index", jobs);
        //        //  return Json(new { status = "success", leads = leads });
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { status = "error", message = ex.Message });
        //    }
        //}

        [HttpPost]
        public ActionResult GetJobs([DataSourceRequest]DataSourceRequest request,string SearchText, string SelectedCompanies, string SelectedProjectStatus, IEnumerable<string> HideStatus = null)
        {
            try
            {
                ViewBag.ProjectHideStatus = new SelectList(db.CoProjectStatus.Where(p => p.SiteCoID == siteusercompanyid).GroupBy(p => p.ProjectStatusName).Select(c => c.Key).ToList());
                ViewBag.ContactCompanies = db.GetJobCustomersBySiteCoID(siteusercompanyid).Where(s => s.Customer != null && s.Customer.Trim() != "").ToList();
                ViewBag.ProjectStatus = db.GetJobUsedStatusBySiteCoID(siteusercompanyid).ToList();
                var jobs = db.GetJobsBySiteCoID(siteusercompanyid).ToList();
                if (SearchText != null && SearchText != "")
                {
                    SearchText = SearchText.Trim().ToLower();
                    jobs = jobs.Where(p => (p.Project == null ? "" : p.Project).ToLower().Contains(SearchText)).ToList();
                }
                char[] sep = new char[] { ',' };
                if (SelectedCompanies != null && SelectedCompanies != "")
                    jobs = jobs.Where(p => SelectedCompanies.Split(sep).Contains(Convert.ToString(p.ContactID))).ToList();
                if (SelectedProjectStatus != null && SelectedProjectStatus != "")
                    jobs = jobs.Where(p => SelectedProjectStatus.Split(sep).Contains(Convert.ToString(p.StatusID))).ToList();
                if (HideStatus != null)
                {
                    if (HideStatus.Count() > 0)
                    {
                        foreach (string sts in HideStatus)
                            jobs = jobs.Where(p => p.Status != sts).ToList();
                    }
                }
                DataSourceResult result = jobs.ToDataSourceResult(request);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { status = "error", message = ex.Message });
            }
        }


    }
}
