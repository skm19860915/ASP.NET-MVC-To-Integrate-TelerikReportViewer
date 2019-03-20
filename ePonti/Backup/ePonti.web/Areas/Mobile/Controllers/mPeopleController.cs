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

namespace ePonti.web.Areas.Mobile.Controllers
{
    [Authorize]
    public class mPeopleController : ePonti.web.Controllers._baseMVCController
    {
        private ePontiv2Entities db = new ePontiv2Entities();

        // GET: Mobile/mPeople
        public ActionResult Index()
        {
            int siteCoID = base.siteusercompanyid;
            var coContacts = db.CoContacts
                .Where(p => p.SiteCoID == siteCoID)
                .Include(c => c.CoContactCompanies)
                .Include(p => p.CoContactPhones)
                .Include(p => p.CoContactEmails)
                //.Include(c => c.CoContactSubtypes)
                .Include(c => c.CoContactTypes)
                .Include(c => c.SiteContactStatus)
                .OrderBy(p => p.ContactLastName).ThenBy(p => p.ContactFirstName).ThenBy(p => p.CoContactCompanies.ContactCoName);

            return View(coContacts.ToList());
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