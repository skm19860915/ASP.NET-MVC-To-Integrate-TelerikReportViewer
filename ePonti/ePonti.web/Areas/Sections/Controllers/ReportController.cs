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
using Rotativa;

namespace ePonti.web.Areas.Sections.Controllers
{
    [Authorize]
    public class ReportController : ePonti.web.Controllers._baseMVCController
    {
        private ePontiv2Entities db = new ePontiv2Entities();

        // GET: Sections/Reports
        public ActionResult Index()
        {
            int siteCoID = base.siteusercompanyid;

            ViewBag.CoReports = db.GetReportsBySiteCoID(siteCoID);
            string projectId = HttpContext.Request.QueryString["projectid"];
            ViewBag.projectId = projectId == null ? "0" : projectId;
            ViewBag.Builder = db.GetProjectsByBuilder(siteCoID);
            return View();
        }
        public ActionResult ShortList()
        {
            int siteCoID = base.siteusercompanyid;

            ViewBag.CoReports = db.GetReportsBySiteCoID(siteCoID);
            string projectId = HttpContext.Request.QueryString["projectid"];
            ViewBag.projectId = projectId == null ? "0" : projectId;
            return View();
        }
        // GET: Sections/ReportViewer
        //public ActionResult ReportViewer(int? projectId)
        //{
        //    if (projectId == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    int siteCoID = base.siteusercompanyid; 

        //    // Getting error while updating EF model so use for temporary only.
        //    ViewBag.ProjectList = (from x in db.ProjectInfo
        //                           where x.SiteCoID == siteCoID
        //                           select new SelectListItem
        //                           {
        //                               Text = x.ProjectName,
        //                               Value = x.ProjectID.ToString()
        //                           }).ToList();

        //    var reports = db.GetProjectReportByProjectID(projectId).ToList();
        //    ProjectReportModel projectReportModel = new ProjectReportModel();
        //    projectReportModel.TotalRecord = reports.Count();
        //    if (reports != null)
        //    {
        //        GetProjectReportByProjectID_Result report = reports.FirstOrDefault();
        //        if (report != null)
        //        {
        //            byte[] imagem = (byte[])(report.LOGO);
        //            string base64String = Convert.ToBase64String(imagem); 
        //            projectReportModel.ViewID = report.ViewID;
        //            projectReportModel.Logo = String.Format("data:image/jpg;base64,{0}", base64String);
        //            projectReportModel.Company = report.Company;
        //            projectReportModel.CoAddress = report.CoAddress;
        //            projectReportModel.CoAddress2 = report.CoAddress2;
        //            projectReportModel.CoPhone = report.CoPhone;
        //            projectReportModel.Date = report.Date;
        //            projectReportModel.Customer = report.Customer;
        //            projectReportModel.CustomerAddress1 = report.CustomerAddress1;
        //            projectReportModel.CustomerAddress2 = report.CustomerAddress2;
        //            projectReportModel.ProjectName = report.ProjectName;
        //            projectReportModel.ProjectAddress = report.ProjectAddress;
        //            projectReportModel.ProjectAddress2 = report.ProjectAddress2;
        //            projectReportModel.Area = report.Area;
        //            projectReportModel.AreaDescription = string.Empty;
        //            projectReportModel.ProjectReportItemList = new List<ProjectReportItem>();
        //            foreach (var r in reports)
        //            {
        //                ProjectReportItem item = new ProjectReportItem();
        //                item.Division = r.Division;
        //                item.Hrs = r.Hrs;
        //                item.Item = r.Item;
        //                item.ItemDesription = string.Empty;
        //                item.Price = r.Price;
        //                item.Qty = r.Qty;
        //                projectReportModel.ProjectReportItemList.Add(item);
        //            }
        //        }
        //    }

        //    return View(projectReportModel);
        //}
        // GET: Sections/ShortViewer
        public ActionResult ShortViewer(int? projectId)
        {
            if (projectId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            int siteCoID = base.siteusercompanyid;

            // Getting error while updating EF model so use for temporary only.
            ViewBag.ProjectList = (from x in db.ProjectInfo
                                   where x.SiteCoID == siteCoID
                                   select new SelectListItem
                                   {
                                       Text = x.ProjectName,
                                       Value = x.ProjectID.ToString()
                                   }).ToList();

            var reports = db.GetProjectReportByProjectID(projectId).ToList();
            ProjectReportModel projectReportModel = new ProjectReportModel();
            projectReportModel.TotalRecord = reports.Count();
            if (reports != null)
            {
                GetProjectReportByProjectID_Result report = reports.FirstOrDefault();
                if (report != null)
                {
                    byte[] imagem = (byte[])(report.LOGO);
                    string base64String = Convert.ToBase64String(imagem);
                    projectReportModel.ViewID = report.ViewID;
                    projectReportModel.Logo = String.Format("data:image/jpg;base64,{0}", base64String);
                    projectReportModel.Company = report.Company;
                    projectReportModel.CoAddress = report.CoAddress;
                    projectReportModel.CoAddress2 = report.CoAddress2;
                    projectReportModel.CoPhone = report.CoPhone;
                    projectReportModel.Date = report.Date;
                    projectReportModel.Customer = report.Customer;
                    projectReportModel.CustomerAddress1 = report.CustomerAddress1;
                    projectReportModel.CustomerAddress2 = report.CustomerAddress2;
                    projectReportModel.ProjectName = report.ProjectName;
                    projectReportModel.ProjectAddress = report.ProjectAddress;
                    projectReportModel.ProjectAddress2 = report.ProjectAddress2;
                    projectReportModel.Area = report.Area;
                    projectReportModel.AreaDescription = string.Empty;
                    projectReportModel.ProjectReportItemList = new List<ProjectReportItem>();
                    foreach (var r in reports)
                    {
                        ProjectReportItem item = new ProjectReportItem();
                        item.Division = r.Division;
                        item.Hrs = r.Hrs;
                        item.Item = r.Item;
                        item.ItemDesription = string.Empty;
                        item.Price = r.Price;
                        item.Qty = r.Qty;
                        projectReportModel.ProjectReportItemList.Add(item);
                    }
                }
            }

            return View(projectReportModel);
        }
        public ActionResult SaveAdPdf(int? projectId)
        {
            if (projectId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Dictionary<string, string> cookieCollection = new Dictionary<string, string>();

            //foreach(var key in Request.Cookies.AllKeys)
            //{
            //    cookieCollection.Add(key, Request.Cookies.Get(key).Value);
            //}
            var cookies = Request.Cookies.AllKeys.ToDictionary(k => k, k => Request.Cookies[k].Value);

            return new ActionAsPdf("ReportViewer", new { projectId = projectId })
            {
                FileName = "Name.pdf",
                Cookies = cookies
            };
        }
        // GET: Sections/Reports
        public ActionResult BuildersView()
        {
            int siteCoID = base.siteusercompanyid;
            
            ViewBag.Builder = db.GetProjectsByBuilder(siteCoID).ToList();
            return View();
        }


        /// <summary>
        /// Created By Alexy
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="groupid"></param>
        /// <returns></returns>
        public ActionResult ReportViewer(int? projectId, int groupid)
        {
            if (projectId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            int siteCoID = base.siteusercompanyid;

            ViewBag.ProjectList = (from x in db.ProjectInfo
                                   where x.SiteCoID == siteCoID && x.ProjectTypeID == groupid
                                   select new SelectListItem
                                   {
                                       Text = x.ProjectName,
                                       Value = x.ProjectID.ToString()
                                   }).ToList();

            return View();
        }
        /// <summary>
        /// Created By Alexy
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public ActionResult JobViewer(int projectId)
        {
            int siteCoID = base.siteusercompanyid;
            ViewBag.ProjectList = (from x in db.ProjectInfo
                                   where x.ProjectID == projectId
                                   select new SelectListItem
                                   {
                                       Text = x.ProjectName,
                                       Value = x.ProjectID.ToString()
                                   }).ToList();
            return View();
        }
        /// <summary>
        /// Created By Alexy
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public ActionResult LeadViewer(int projectId)
        {
            int siteCoID = base.siteusercompanyid;
            ViewBag.ProjectList = (from x in db.ProjectInfo
                                   where x.ProjectID == projectId
                                   select new SelectListItem
                                   {
                                       Text = x.ProjectName,
                                       Value = x.ProjectID.ToString()
                                   }).ToList();
            return View();
        }
        /// <summary>
        /// Created By Alexy
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public ActionResult QuoteViewer(int projectId)
        {
            int siteCoID = base.siteusercompanyid;
            ViewBag.ProjectList = (from x in db.ProjectInfo
                                   where x.ProjectID == projectId
                                   select new SelectListItem
                                   {
                                       Text = x.ProjectName,
                                       Value = x.ProjectID.ToString()
                                   }).ToList();
            return View();
        }
        /// <summary>
        /// Created By Alexy
        /// </summary>
        /// <param name="corid"></param>
        /// <returns></returns>
        public ActionResult ChangeViewer(int corid)
        {
            ViewBag.CorId = corid;
            return View();
        }
        /// <summary>
        /// Created By Alexy
        /// </summary>
        /// <param name="woid"></param>
        /// <returns></returns>
        public ActionResult WorkOrderViewer(int woid)
        {
            ViewBag.WoId = woid;
            return View();
        }
        /// <summary>
        /// Created By Alexy
        /// </summary>
        /// <param name="porid"></param>
        /// <returns></returns>
        public ActionResult PorViewer(int porid)
        {
            ViewBag.PorId = porid;
            return View();
        }
        /// <summary>
        /// Created By Alexy
        /// </summary>
        /// <param name="deliveryid"></param>
        /// <returns></returns>
        public ActionResult DeliveryViewer(int deliveryid)
        {
            ViewBag.DeliveryId = deliveryid;
            return View();
        }
        /// <summary>
        /// Created By Alexy
        /// </summary>
        /// <param name="soiid"></param>
        /// <returns></returns>
        public ActionResult SalesOrderViewer(int soiid)
        {
            ViewBag.SalesOrderId = soiid;
            return View();
        }
    }
}
