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
    public class ManufacturersOptionsController : ePonti.web.Controllers._baseMVCController
    {
        private ePontiv2Entities db = new ePontiv2Entities();
        BOL.Repository.CommonRepository repo = new BOL.Repository.CommonRepository();

        public ActionResult Index()
        {
            int siteCoID = base.siteusercompanyid;

            ViewBag.CoManufacturers = db.GetManufacturersBySiteCoID(siteCoID);

            return View();
        }

        public ActionResult DeleteOption(int ItemID, string Type)
        {
            var res = repo.DeleteOption(Type, ItemID, siteusercompanyid);
            return Json(new { status = (res == "" ? "success" : res) });
        }

        #region Options Update
        public ActionResult UpdateManufacturer(int ItemID, string Name)
        {
            CoDataManufacturers item;
            if (ItemID == 0)
            {
                item = new CoDataManufacturers() { SiteCoID = siteusercompanyid };
                db.CoDataManufacturers.Add(item);
            }
            else
            {
                item = db.CoDataManufacturers.Where(p => p.ManufacturerID== ItemID && p.SiteCoID == siteusercompanyid).FirstOrDefault();

            }

            if (item != null)
            {
                item.ManufacturerName = Name;
            }
            db.SaveChanges();
            return Json(new { status = "success", itemID = item.ManufacturerID });
        }

        #endregion
    }
}