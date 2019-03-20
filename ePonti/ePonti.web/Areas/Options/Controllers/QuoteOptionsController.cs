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
    public class QuoteOptionsController : ePonti.web.Controllers._baseMVCController
    {
        private ePontiv2Entities db = new ePontiv2Entities();
        BOL.Repository.CommonRepository repo = new BOL.Repository.CommonRepository();

        // GET: Options/QuoteOptions
        public ActionResult Index()
        {
            int siteCoID = base.siteusercompanyid;

            ViewBag.CoQuoteNumFormats = db.GetQuoteNumFormatsBySiteCoID(siteCoID).ToList();
            ViewBag.CoQuoteStatus = db.GetQuoteStatusBySiteCoID(siteCoID);
            ViewBag.CoAreas = db.GetCoAreasBySiteCoID(siteCoID);
            ViewBag.CoDivisions = db.GetCoDivisionsBySiteCoID(siteCoID);
            ViewBag.CoQuoteTemplates = db.GetQuoteTemplatesBySiteCoID(siteCoID);

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
            CoQuoteNumFormats item = db.CoQuoteNumFormats.Where(p => p.SiteCoID == siteusercompanyid).FirstOrDefault();
            if (item == null)
            {
                item = new CoQuoteNumFormats() { SiteCoID = siteusercompanyid };
                db.CoQuoteNumFormats.Add(item);
            }

            item.NumData = Data;
            item.NumSep = Sep;
            item.NumStart = Num;
            db.SaveChanges();
            return Json(new { status = "success", itemID = item.NumID });
        }

        public ActionResult UpdateQuoteStatus(int ItemID, string Name, int Order, bool Lock)
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
                item.ProjectTypeID = 5;
            }
            db.SaveChanges();
            return Json(new { status = "success", itemID = item.ProjectStatusID });
        }

        //public ActionResult UpdateQuoteTemplate(int ItemID, string Name, int Description)
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

        public ActionResult UpdateArea(int ItemID, string Name, int Order)
        {
             CoAreas item;
            if (ItemID == 0)
            {
                item = new  CoAreas() { SiteCoID = siteusercompanyid };
                db. CoAreas.Add(item);
            }
            else
            {
                item = db. CoAreas.Where(p => p.AreaID == ItemID && p.SiteCoID == siteusercompanyid).FirstOrDefault();

            }

            if (item != null)
            {
                item.AreaName = Name;
                item.AreaOrder= Order;
            }
            db.SaveChanges();
            return Json(new { status = "success", itemID = item.AreaID });
        }

        public ActionResult UpdateDivision(int ItemID, string Name, int Order)
        {
            CoDivisions item;
            if (ItemID == 0)
            {
                item = new CoDivisions() { SiteCoID = siteusercompanyid };
                db.CoDivisions.Add(item);
            }
            else
            {
                item = db.CoDivisions.Where(p => p.DivisionID == ItemID && p.SiteCoID == siteusercompanyid).FirstOrDefault();

            }

            if (item != null)
            {
                item.DivisionName = Name;
                item.DivisionOrder = Order;
            }
            db.SaveChanges();
            return Json(new { status = "success", itemID = item.DivisionID });
        }

        #endregion
    }
}
