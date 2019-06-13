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
    public class PostItInfoController : Controller
    {
        private ePontiv2Entities db = new ePontiv2Entities();

        // GET: Common/PostItInfo
        public ActionResult Index()
        {
            return View(db.PostIt.ToList());
        }

        // GET: Common/PostItInfo/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PostIt postIt = db.PostIt.Find(id);
            if (postIt == null)
            {
                return HttpNotFound();
            }
            return View(postIt);
        }

        // GET: Common/PostItInfo/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Common/PostItInfo/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PostID,ProjectID,PostTopic,PostBody,PostDate,PostNotify,SiteCoID,SiteUserID,PostTime")] PostIt postIt)
        {
            if (ModelState.IsValid)
            {
                db.PostIt.Add(postIt);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(postIt);
        }

        // GET: Common/PostItInfo/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PostIt postIt = db.PostIt.Find(id);
            if (postIt == null)
            {
                return HttpNotFound();
            }
            return View(postIt);
        }

        // POST: Common/PostItInfo/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PostID,ProjectID,PostTopic,PostBody,PostDate,PostNotify,SiteCoID,SiteUserID,PostTime")] PostIt postIt)
        {
            if (ModelState.IsValid)
            {
                db.Entry(postIt).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(postIt);
        }

        // GET: Common/PostItInfo/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PostIt postIt = db.PostIt.Find(id);
            if (postIt == null)
            {
                return HttpNotFound();
            }
            return View(postIt);
        }

        // POST: Common/PostItInfo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PostIt postIt = db.PostIt.Find(id);
            db.PostIt.Remove(postIt);
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
