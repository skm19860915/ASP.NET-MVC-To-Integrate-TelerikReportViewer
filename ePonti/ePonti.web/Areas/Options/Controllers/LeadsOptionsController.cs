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
    public class LeadsOptionsController : ePonti.web.Controllers._baseMVCController
    {
        private ePontiv2Entities db = new ePontiv2Entities();
        BOL.Repository.CommonRepository repo = new BOL.Repository.CommonRepository();

        // GET: Options/LeadsOptions
        public ActionResult Index()
        {
            int siteCoID = base.siteusercompanyid;

            ViewBag.CoLeadNumFormats = db.GetLeadNumFormatsBySiteCoID(siteCoID).ToList();
            ViewBag.CoLeadStatus = db.GetLeadStatusBySiteCoID(siteCoID);
            ViewBag.CoLeadTypes = db.GetLeadTypesBySiteCoID(siteCoID);
            ViewBag.CoLeadStages = db.GetLeadStagesBySiteCoID(siteCoID);
            ViewBag.CoLeadRatings = db.GetLeadRatingsBySiteCoID(siteCoID);
            ViewBag.CoLeadSystems = db.GetLeadSystemsBySiteCoID(siteCoID);
            ViewBag.CoLeadProp = db.GetLeadProbabilityBySiteCoID(siteCoID);

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
            CoLeadNumFormats item = db.CoLeadNumFormats.Where(p => p.SiteCoID == siteusercompanyid).FirstOrDefault();
            if (item == null)
            {
                item = new CoLeadNumFormats() { SiteCoID = siteusercompanyid };
                db.CoLeadNumFormats.Add(item);
            }

            item.NumData = Data;
            item.NumSep = Sep;
            item.NumStart = Num;
            db.SaveChanges();
            return Json(new { status = "success", itemID = item.NumID });
        }

        public ActionResult UpdateLeadStatus(int ItemID, string Name, int Order, bool Lock)
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
                item.ProjectTypeID = 6;
            }
            db.SaveChanges();
            return Json(new { status = "success", itemID = item.ProjectStatusID });
        }

        public ActionResult UpdateLeadType(int ItemID, string Name, int Order)
        {
            CoLeadTypes item;
            if (ItemID == 0)
            {
                item = new CoLeadTypes() { SiteCoID = siteusercompanyid };
                db.CoLeadTypes.Add(item);
            }
            else
            {
                item = db.CoLeadTypes.Where(p => p.LeadTypeID == ItemID && p.SiteCoID == siteusercompanyid).FirstOrDefault();

            }

            if (item != null)
            {
                item.LeadType = Name;
                item.LeadTypeOrder = Order;
            }
            db.SaveChanges();
            return Json(new { status = "success", itemID = item.LeadTypeID });
        }

        public ActionResult UpdateRating(int ItemID, string Name, int Order)
        {
            CoLeadRatings item;
            if (ItemID == 0)
            {
                item = new CoLeadRatings() { SiteCoID = siteusercompanyid };
                db.CoLeadRatings.Add(item);
            }
            else
            {
                item = db.CoLeadRatings.Where(p => p.LeadRatingID == ItemID && p.SiteCoID == siteusercompanyid).FirstOrDefault();

            }

            if (item != null)
            {
                item.LeadRating = Name;
                item.LeadRatingOrder = Order;
            }
            db.SaveChanges();
            return Json(new { status = "success", itemID = item.LeadRatingID });
        }

        public ActionResult UpdatePhase(int ItemID, string Name, int Order)
        {
            CoLeadStages item;
            if (ItemID == 0)
            {
                item = new CoLeadStages() { SiteCoID = siteusercompanyid };
                db.CoLeadStages.Add(item);
            }
            else
            {
                item = db.CoLeadStages.Where(p => p.LeadStageID == ItemID && p.SiteCoID == siteusercompanyid).FirstOrDefault();

            }

            if (item != null)
            {
                item.SalesLeadStage = Name;
                item.PhaseOrder = Order;
            }
            db.SaveChanges();
            return Json(new { status = "success", itemID = item.LeadStageID });
        }

        public ActionResult UpdateProbability(int ItemID, string Name)
        {
            CoLeadProbabilities item;
            if (ItemID == 0)
            {
                item = new CoLeadProbabilities() { SiteCoID = siteusercompanyid };
                db.CoLeadProbabilities.Add(item);
            }
            else
            {
                item = db.CoLeadProbabilities.Where(p => p.LeadProbabilityID == ItemID && p.SiteCoID == siteusercompanyid).FirstOrDefault();

            }

            if (item != null)
            {
                item.LeadProbability = Name;
            }
            db.SaveChanges();
            return Json(new { status = "success", itemID = item.LeadProbabilityID });
        }

        public ActionResult UpdateSystemType(int ItemID, string Name, int Order)
        {
            CoLeadSystemTypes item;
            if (ItemID == 0)
            {
                item = new CoLeadSystemTypes() { SiteCoID = siteusercompanyid };
                db.CoLeadSystemTypes.Add(item);
            }
            else
            {
                item = db.CoLeadSystemTypes.Where(p => p.LeadSystemTypeID == ItemID && p.SiteCoID == siteusercompanyid).FirstOrDefault();

            }

            if (item != null)
            {
                item.LeadSystemType = Name;
                item.LeadSystemOrder = Order;
            }
            db.SaveChanges();
            return Json(new { status = "success", itemID = item.LeadSystemTypeID });
        }

        #endregion
    }
}