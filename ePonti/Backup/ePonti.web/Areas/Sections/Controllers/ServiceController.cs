using ePonti.BOL;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;

namespace ePonti.web.Areas.Sections.Controllers
{
    [Authorize]
    public class ServiceController : ePonti.web.Controllers._baseMVCController
    {
        private ePontiv2Entities db = new ePontiv2Entities();

        public ActionResult Index()
        {
            ViewBag.ContactCompanies = db.GetServiceCustomersBySiteCoID(siteusercompanyid).Where(s => s.Customer != null && s.Customer.Trim() != "").ToList();
            ViewBag.ProjectStatus = db.GetServiceUsedStatusBySiteCoID(siteusercompanyid).ToList();
            ViewBag.ProjectHideStatus = new SelectList(db.CoProjectStatus.Where(p => p.SiteCoID == siteusercompanyid).GroupBy(p => p.ProjectStatusName).Select(c => c.Key).ToList());
            //var services = db.GetServiceBySiteCoID(siteusercompanyid).ToList();
            return View();
        }

        [HttpPost]
        public ActionResult GetServices([DataSourceRequest]DataSourceRequest request, string SearchText, string SelectedCompanies, string SelectedProjectStatus, IEnumerable<string> HideStatus = null)
        {
            try
            {
                ViewBag.ProjectHideStatus = new SelectList(db.CoProjectStatus.Where(p => p.SiteCoID == siteusercompanyid).GroupBy(p => p.ProjectStatusName).Select(c => c.Key).ToList());
                ViewBag.ContactCompanies = db.GetServiceCustomersBySiteCoID(siteusercompanyid).Where(s => s.Customer != null && s.Customer.Trim() != "").ToList();
                ViewBag.ProjectStatus = db.GetServiceUsedStatusBySiteCoID(siteusercompanyid).ToList();
                var services = db.GetServiceBySiteCoID(siteusercompanyid).ToList();
                if (SearchText != null && SearchText != "")
                {
                    SearchText = SearchText.Trim().ToLower();
                    services = services.Where(p => (p.Project == null ? "" : p.Project).ToLower().Contains(SearchText)).ToList();
                }
                char[] sep = new char[] { ',' };
                if (SelectedCompanies != null && SelectedCompanies != "")
                    services = services.Where(p => SelectedCompanies.Split(sep).Contains(Convert.ToString(p.ContactID))).ToList();
                if (SelectedProjectStatus != null && SelectedProjectStatus != "")
                    services = services.Where(p => SelectedProjectStatus.Split(sep).Contains(Convert.ToString(p.StatusID))).ToList();
                if (HideStatus != null)
                {
                    if (HideStatus.Count() > 0)
                    {
                        foreach (string sts in HideStatus)
                            services = services.Where(p => p.Status != sts).ToList();
                    }
                }

                DataSourceResult result = services.ToDataSourceResult(request);
                return Json(result, JsonRequestBehavior.AllowGet);
                // return View("Index", services);
                //  return Json(new { status = "success", leads = leads });
            }
            catch (Exception ex)
            {
                return Json(new { status = "error", message = ex.Message });
            }
        }
        // Post: Sections/Services
        //[HttpPost]
        //public ActionResult Index(string SearchText, string SelectedCompanies, string SelectedProjectStatus, IEnumerable<string> HideStatus = null)
        //{
        //    try
        //    {
        //        ViewBag.ProjectHideStatus = new SelectList(db.CoProjectStatus.Where(p => p.SiteCoID == siteusercompanyid).GroupBy(p => p.ProjectStatusName).Select(c => c.Key).ToList());
        //        ViewBag.ContactCompanies = db.GetServiceCustomersBySiteCoID(siteusercompanyid).Where(s => s.Customer != null && s.Customer.Trim() != "").ToList();
        //        ViewBag.ProjectStatus = db.GetServiceUsedStatusBySiteCoID(siteusercompanyid).ToList();
        //        var services = db.GetServiceBySiteCoID(siteusercompanyid).ToList();
        //        if (SearchText != null && SearchText != "")
        //        {
        //            SearchText = SearchText.Trim().ToLower();
        //            services = services.Where(p => (p.Project == null ? "" : p.Project).ToLower().Contains(SearchText)).ToList();
        //        }
        //        char[] sep = new char[] { ',' };
        //        if (SelectedCompanies != null && SelectedCompanies != "")
        //            services = services.Where(p => SelectedCompanies.Split(sep).Contains(Convert.ToString(p.ContactID))).ToList();
        //        if (SelectedProjectStatus != null && SelectedProjectStatus != "")
        //            services = services.Where(p => SelectedProjectStatus.Split(sep).Contains(Convert.ToString(p.StatusID))).ToList();
        //        if (HideStatus != null)
        //        {
        //            if (HideStatus.Count() > 0)
        //            {
        //                foreach (string sts in HideStatus)
        //                services = services.Where(p => p.Status != sts).ToList();
        //            }
        //        }
        //        return View("Index", services);
        //        //  return Json(new { status = "success", leads = leads });
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { status = "error", message = ex.Message });
        //    }
        //}
    }
}
