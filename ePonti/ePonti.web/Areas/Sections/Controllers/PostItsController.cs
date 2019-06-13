using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ePonti.BOL;

namespace ePonti.web.Areas.Sections.Controllers
{
    [Authorize]
    public class PostItsController : ePonti.web.Controllers._baseMVCController
    {
        private ePontiv2Entities db = new ePontiv2Entities();

        // GET: Sections/PostIts
        public ActionResult Index()
        {
            int siteCoID = base.siteusercompanyid;

            ViewBag.CoPostIts = db.GetPostItsBySiteCoID(siteCoID);

            return View();
        }
    }
}
