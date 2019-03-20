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
    public class ServiceOptionsController : ePonti.web.Controllers._baseMVCController
    {
        private ePontiv2Entities db = new ePontiv2Entities();
        BOL.Repository.CommonRepository repo = new BOL.Repository.CommonRepository();

        // GET: Options/ServiceOptions
        public ActionResult Index()
        {
            int siteCoID = base.siteusercompanyid;

            ViewBag.CoServiceNumFormats = db.GetServiceNumFormatsBySiteCoID(siteCoID).ToList();
            ViewBag.CoServiceStatus = db.GetServiceStatusBySiteCoID(siteCoID);
            ViewBag.CoServiceTemplates = db.GetServiceTemplatesBySiteCoID(siteCoID);

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
            CoServiceNumFormats item = db.CoServiceNumFormats.Where(p => p.SiteCoID == siteusercompanyid).FirstOrDefault();
            if (item == null)
            {
                item = new CoServiceNumFormats() { SiteCoID = siteusercompanyid };
                db.CoServiceNumFormats.Add(item);
            }

            item.NumData = Data;
            item.NumSep = Sep;
            item.NumStart = Num;
            db.SaveChanges();
            return Json(new { status = "success", itemID = item.NumID });
        }

        public ActionResult UpdateServiceStatus(int ItemID, string Name, int Order, bool Lock)
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
                item.ProjectTypeID = 3;
            }
            db.SaveChanges();
            return Json(new { status = "success", itemID = item.ProjectStatusID });
        }

        //public ActionResult UpdateServiceTemplate(int ItemID, string Name, int Description)
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

        #endregion
    }
}