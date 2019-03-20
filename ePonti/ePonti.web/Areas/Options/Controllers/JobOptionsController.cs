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

namespace ePonti.web.Areas.Options.Controllers
{
    [Authorize]
    public class JobOptionsController : ePonti.web.Controllers._baseMVCController
    {
        private ePontiv2Entities db = new ePontiv2Entities();
        BOL.Repository.CommonRepository repo = new BOL.Repository.CommonRepository();

        // GET: Options/JobOptions
        public ActionResult Index()
        {
            int siteCoID = base.siteusercompanyid;

            ViewBag.CoJobNumFormats = db.GetJobNumFormatsBySiteCoID(siteCoID).ToList();
            ViewBag.CoJobStatus = db.GetJobStatusBySiteCoID(siteCoID);
            ViewBag.CoCorStatus = db.GetCorStatusBySiteCoID(siteCoID);
            ViewBag.CoCorTypes = db.GetCorTypesBySiteCoID(siteCoID);
            ViewBag.CoJobTemplates = db.GetJobTemplatesBySiteCoID(siteCoID);

            return View();
        }

        public ActionResult DeleteOption(int ItemID, string Type)
        {
            var res = repo.DeleteOption(Type, ItemID, siteusercompanyid);
            return Json(new { status = (res == "" ? "success" : res) });
        }

        #region Options Update
        public ActionResult UpdateNumberFormat(string Data, string Sep, decimal Num)
        {
            //there can be only one entry for number format
            CoJobNumFormats item = db.CoJobNumFormats.Where(p => p.SiteCoID == siteusercompanyid).FirstOrDefault();
            if (item == null)
            {
                item = new CoJobNumFormats() { SiteCoID = siteusercompanyid };
                db.CoJobNumFormats.Add(item);
            }

            item.NumData = Data;
            item.NumSep = Sep;
            item.NumStart = Num;
            db.SaveChanges();
            return Json(new { status = "success", itemID = item.NumID });
        }

        public ActionResult UpdateJobStatus(int ItemID, string Name, int Order, bool Lock)
        {
            CoProjectStatus item;
            if (ItemID == 0)
            {
                item = new CoProjectStatus() { SiteCoID = siteusercompanyid };
                db.CoProjectStatus.Add(item);
            }
            else
            {
                item = db.CoProjectStatus.Where(p => p.ProjectStatusID == ItemID && p.SiteCoID == siteusercompanyid).FirstOrDefault();

            }

            if (item != null)
            {
                item.ProjectStatusName = Name;
                item.ProjectStatusOrder = Order;
                item.Lock = Lock;
                item.ProjectTypeID = 4;
            }
            db.SaveChanges();
            return Json(new { status = "success", itemID = item.ProjectStatusID });
        }

        //public ActionResult UpdateJobTemplate(int ItemID, string Name, int Description)
        //{
        //    CoLeadTypes item;
        //    if (ItemID == 0)
        //    {
        //        item = new CoLeadTypes() { SiteCoID = siteusercompanyid };
        //        db.CoLeadTypes.Add(item);
        //    }
        //    else
        //    {
        //        item = db.CoLeadTypes.Where(p => p.LeadTypeID == ItemID && p.SiteCoID == siteusercompanyid).FirstOrDefault();

        //    }

        //    if (item != null)
        //    {
        //        item.LeadType = Name;
        //        item.LeadTypeOrder = Order;
        //    }
        //    db.SaveChanges();
        //    return Json(new { status = "success", itemID = item.LeadTypeID });
        //}

        public ActionResult UpdateCORStatus(int ItemID, string Name, int Order)
        {
            CoCorStatus item;
            if (ItemID == 0)
            {
                item = new CoCorStatus() { SiteCoID = siteusercompanyid };
                db.CoCorStatus.Add(item);
            }
            else
            {
                item = db.CoCorStatus.Where(p => p.CorStatusID == ItemID && p.SiteCoID == siteusercompanyid).FirstOrDefault();

            }

            if (item != null)
            {
                item.CorStatus = Name;
                item.ChangeRequestOrder = Order;
            }
            db.SaveChanges();
            return Json(new { status = "success", itemID = item.CorStatusID });
        }

        public ActionResult UpdateCORType(int ItemID, string Name, int Order)
        {
            CoCorTypes item;
            if (ItemID == 0)
            {
                item = new CoCorTypes() { SiteCoID = siteusercompanyid };
                db.CoCorTypes.Add(item);
            }
            else
            {
                item = db.CoCorTypes.Where(p => p.CorTypeID == ItemID && p.SiteCoID == siteusercompanyid).FirstOrDefault();

            }

            if (item != null)
            {
                item.CorType = Name;
                item.CorOrder = Order;
            }
            db.SaveChanges();
            return Json(new { status = "success", itemID = item.CorTypeID });
        }

        #endregion
    }
}