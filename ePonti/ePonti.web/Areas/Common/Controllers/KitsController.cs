using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ePonti.BOL;

namespace ePonti.web.Areas.Common.Controllers
{
    public class KitsController : web.Controllers._baseMVCController
    {
        private ePontiv2Entities db = new ePontiv2Entities();

        // GET: Common/Kits
        public ActionResult Index()
        {
            return View(db.CoKits.ToList());
        }

        // GET: Common/Kits/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }       
            CoKits coKits = db.CoKits.Find(id);
            ViewBag.KitItems = db.GetKitItemsByKitID(id).ToList();
            ViewBag.AcctKitID = coKits.AcctKitID;
            coKits.KitDiscount = coKits.KitDiscount * 100;
            if (coKits == null)
            {
                return HttpNotFound();
            }
            ViewBag.KitItems = db.GetKitItemsByKitID(id).ToList();
            return View(coKits);
        }

        // POST: Common/Kits/updatediscount
      
        [HttpPost]      
        public ActionResult updatediscount(decimal? KitDiscount ,int? MasterKitID)
        {
            if (ModelState.IsValid)
            {
                var q = db.CoKits.Where(p => p.MasterKitID == MasterKitID).FirstOrDefault();
                q.KitDiscount = KitDiscount/100;              
                db.SaveChanges();
                return Json(new { status = "success" });
            }
            return Json(new { status = "error" });
        }

        // GET: Common/Kits/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Common/Kits/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MasterKitID,GroupID,KitName,KitSku,KitDescription,KitDiscount,KitSummarize,AcctKitID,KitImage,Discontinued,SiteCoID")] CoKits coKits)
        {
            if (ModelState.IsValid)
            {
                coKits.SiteCoID = base.siteusercompanyid;
                db.InsertMasterKit(coKits.SiteCoID, coKits.KitName, coKits.KitSku, coKits.KitDescription);
               // db.CoKits.Add(coKits);               
                db.SaveChanges();
                return RedirectToAction("Index", "KitsOptions",new { area = "Options" });
                }

            return View(coKits);
        }

        // GET: Common/Kits/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CoKits coKits = db.CoKits.Find(id);
            if (coKits == null)
            {
                return HttpNotFound();
            }
            return View("_Edit",coKits);
        }

        // POST: Common/Kits/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CoKits coKits)
        {
            if (ModelState.IsValid)
            {
                var q = db.CoKits.Where(p => p.MasterKitID == coKits.MasterKitID).FirstOrDefault();
                q.SiteCoID = base.siteusercompanyid;
                q.KitName = coKits.KitName;
                q.KitSku = coKits.KitSku;
                q.KitDescription = coKits.KitDescription;
                db.SaveChanges();
                return Json(new { status = "success", MasterKitID= coKits.MasterKitID });
            }
            return View(coKits);
        }

        // GET: Common/Kits/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CoKits coKits = db.CoKits.Find(id);
            if (coKits == null)
            {
                return HttpNotFound();
            }
            return View(coKits);
        }

        // POST: Common/Kits/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CoKits coKits = db.CoKits.Find(id);
            db.CoKits.Remove(coKits);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
