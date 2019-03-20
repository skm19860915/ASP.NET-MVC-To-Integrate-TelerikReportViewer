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
    public class KitsOptionsController : ePonti.web.Controllers._baseMVCController
    {
        private ePontiv2Entities db = new ePontiv2Entities();

        // GET: Options/KitsOptions
        public ActionResult Index()
        {
            int siteCoID = base.siteusercompanyid;

            ViewBag.CoKits = db.GetKitsBySiteCoID(siteCoID);

            return View();
        }
    }
}