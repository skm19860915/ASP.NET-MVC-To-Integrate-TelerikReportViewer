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
    public class ItemsOptionsController : ePonti.web.Controllers._baseMVCController
    {
        private ePontiv2Entities db = new ePontiv2Entities();

        // GET: Options/ItemsOptions
        public ActionResult Index()
        {
            int siteCoID = base.siteusercompanyid;
            ViewBag.groups = new SelectList(db.GetItemsBySiteCoID(siteCoID).GroupBy(p=>p.Group).Distinct().Select(p=>p.Key).ToList());
            ViewBag.Manufacturers = new SelectList(db.GetItemsBySiteCoID(siteCoID).GroupBy(p => p.Manufacturer).Distinct().Select(p => p.Key).ToList());
            ViewBag.CoItems = db.GetItemsBySiteCoID(siteCoID).ToList();
            return View();
        }
        // Post: Options/ItemsOptions
        [HttpPost]
        public ActionResult Index(string Group, string Manufacturer,string Model)
        {
            try
            {
                int siteCoID = base.siteusercompanyid;
                ViewBag.groups = new SelectList(db.GetItemsBySiteCoID(siteCoID).GroupBy(p => p.Group).Distinct().Select(p => p.Key).ToList());
                ViewBag.Manufacturers = new SelectList(db.GetItemsBySiteCoID(siteCoID).GroupBy(p => p.Manufacturer).Distinct().Select(p => p.Key).ToList());
                var CoItems = db.GetItemsBySiteCoID(siteCoID).ToList();
                if (Group != null && Group != "")
                    CoItems = CoItems.Where(p => p.Group==Group).ToList();
                if (Manufacturer != null && Manufacturer != "")
                    CoItems = CoItems.Where(p => p.Manufacturer == Manufacturer).ToList();
                if (Model != null && Model != "")
                    CoItems = CoItems.Where(p => p.Model.ToLower() == Model.ToLower()).ToList();
                ViewBag.CoItems = CoItems;
                return View("Index", CoItems);
                //  return Json(new { status = "success", leads = leads });
            }
            catch (Exception ex)
            {
                return Json(new { status = "error", message = ex.Message });
            }
        }
    }
}