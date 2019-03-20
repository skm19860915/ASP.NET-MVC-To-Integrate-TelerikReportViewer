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
    public class LeadsController : ePonti.web.Controllers._baseMVCController
    {
        private ePontiv2Entities db = new ePontiv2Entities();
        BOL.Repository.CommonRepository repo = new BOL.Repository.CommonRepository();

        // GET: Sections/Leads
        public ActionResult Index()
        {
            ViewBag.ProjectHideStatus = new SelectList(db.CoProjectStatus.Where(p => p.SiteCoID == siteusercompanyid).GroupBy(p => p.ProjectStatusName).Select(c => c.Key).ToList());
            ViewBag.ContactCompanies = db.GetLeadProspectsBySiteCoID(siteusercompanyid).ToList();
            ViewBag.ProjectStatus = db.GetLeadUsedStatusBySiteCoID(siteusercompanyid).ToList();
            ViewBag.Phases = db.GetPhaseByLeads(siteusercompanyid);
            ViewBag.Types = db.GetSourceByLeads(siteusercompanyid);
            var leads = db.GetLeadsBySiteCoID(siteusercompanyid).ToList();
            return View(leads);
        }
        // Post: Sections/Leads
        //[HttpPost]
        //public ActionResult Index(string SearchText, string SelectedCompanies, string SelectedProjectStatus, string SelectedPhases, string SelectedTypes, IEnumerable<string> HideStatus=null)
        //{
        //    try
        //    {
        //        ViewBag.ProjectHideStatus = new SelectList(db.CoProjectStatus.Where(p => p.SiteCoID == siteusercompanyid).GroupBy(p => p.ProjectStatusName).Select(c => c.Key).ToList());
        //        ViewBag.ContactCompanies = db.GetLeadProspectsBySiteCoID(siteusercompanyid).ToList();
        //        ViewBag.ProjectStatus = db.GetLeadUsedStatusBySiteCoID(siteusercompanyid).ToList();
        //        ViewBag.Phases = db.GetPhaseByLeads(siteusercompanyid);
        //        ViewBag.Types = db.GetSourceByLeads(siteusercompanyid);
        //        var leads = db.GetLeadsBySiteCoID(siteusercompanyid).ToList();
        //        if (SearchText != null && SearchText != "")
        //        {
        //            SearchText = SearchText.Trim().ToLower();
        //            leads = leads.Where(p => (p.Project == null ? "" : p.Project).ToLower().Contains(SearchText)).ToList();
        //        }
        //        char[] sep = new char[] { ',' };
        //        if (SelectedCompanies != null && SelectedCompanies != "")
        //            leads = leads.Where(p => SelectedCompanies.Split(sep).Contains(Convert.ToString(p.ContactID))).ToList();
        //        if (SelectedProjectStatus != null && SelectedProjectStatus != "")
        //            leads = leads.Where(p => SelectedProjectStatus.Split(sep).Contains(Convert.ToString(p.StatusID))).ToList();
        //        if (SelectedPhases != null && SelectedPhases != "")
        //            leads = leads.Where(p => SelectedPhases.Split(sep).Contains(Convert.ToString(p.PhasesID))).ToList();
        //        if (SelectedTypes != null && SelectedTypes != "")
        //            leads = leads.Where(p => SelectedTypes.Split(sep).Contains(Convert.ToString(p.SourceID))).ToList();
        //        if (HideStatus != null)
        //        {
        //            if (HideStatus.Count() > 0)
        //            {
        //                foreach (string sts in HideStatus)
        //                    leads = leads.Where(p => p.Status != sts).ToList();
        //            }
        //        }
        //         return View("Index",leads);
        //      //  return Json(new { status = "success", leads = leads });
        //    }
        //    catch(Exception ex)
        //    {
        //        return Json(new { status = "error", message = ex.Message });
        //    }
        //}

        [HttpPost]
        public ActionResult GetLeads([DataSourceRequest]DataSourceRequest request,string SearchText, string SelectedCompanies, string SelectedProjectStatus, string SelectedPhases, string SelectedTypes, IEnumerable<string> HideStatus = null)
        {
            try
            {
                ViewBag.ProjectHideStatus = new SelectList(db.CoProjectStatus.Where(p => p.SiteCoID == siteusercompanyid).GroupBy(p => p.ProjectStatusName).Select(c => c.Key).ToList());
                ViewBag.ContactCompanies = db.GetLeadProspectsBySiteCoID(siteusercompanyid).ToList();
                ViewBag.ProjectStatus = db.GetLeadUsedStatusBySiteCoID(siteusercompanyid).ToList();
                ViewBag.Phases = db.GetPhaseByLeads(siteusercompanyid);
                ViewBag.Types = db.GetSourceByLeads(siteusercompanyid);
                var leads = db.GetLeadsBySiteCoID(siteusercompanyid).ToList();
                if (SearchText != null && SearchText != "")
                {
                    SearchText = SearchText.Trim().ToLower();
                    leads = leads.Where(p => (p.Project == null ? "" : p.Project).ToLower().Contains(SearchText)).ToList();
                }
                char[] sep = new char[] { ',' };
                if (SelectedCompanies != null && SelectedCompanies != "")
                    leads = leads.Where(p => SelectedCompanies.Split(sep).Contains(Convert.ToString(p.ContactID))).ToList();
                if (SelectedProjectStatus != null && SelectedProjectStatus != "")
                    leads = leads.Where(p => SelectedProjectStatus.Split(sep).Contains(Convert.ToString(p.StatusID))).ToList();
                if (SelectedPhases != null && SelectedPhases != "")
                    leads = leads.Where(p => SelectedPhases.Split(sep).Contains(Convert.ToString(p.PhasesID))).ToList();
                if (SelectedTypes != null && SelectedTypes != "")
                    leads = leads.Where(p => SelectedTypes.Split(sep).Contains(Convert.ToString(p.SourceID))).ToList();
                if (HideStatus != null)
                {
                    if (HideStatus.Count() > 0)
                    {
                        foreach (string sts in HideStatus)
                            leads = leads.Where(p => p.Status != sts).ToList();
                    }
                }
                // return View("Index", leads);
                DataSourceResult result = leads.ToDataSourceResult(request);
                return Json(result, JsonRequestBehavior.AllowGet);
                //  return Json(new { status = "success", leads = leads });



            }
            catch (Exception ex)
            {
                return Json(new { status = "error", message = ex.Message });
            }
        }
    }
}
