using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using ePonti.web.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using ePonti.BOL;

namespace ePonti.web.Controllers {
    public class _baseMVCController : Controller {

        public string aspnetuserid = "";
        public int siteuserid = 0;
        public int siteusercompanyid = 0;
        public string companyname = "Company Name";
        public string displayusername = "";

        protected override void OnActionExecuting(ActionExecutingContext filterContext) {
            base.OnActionExecuting(filterContext);
            HttpSessionStateBase session = filterContext.HttpContext.Session;
            ePontiv2Entities db = new ePontiv2Entities();

            // set up user profile here
            if (User.Identity.IsAuthenticated) {
               // Session.Remove("displayusername");
                aspnetuserid = User.Identity.GetUserId();
                var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
                //var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(DBContext));
                var currentUser = manager.FindById(User.Identity.GetUserId());
                siteuserid = currentUser.siteuserid;               
                siteusercompanyid = currentUser.sitecoid;              
                    displayusername = db.SiteUsers.Where(p => p.SiteUserID == siteuserid).Select(p => p.UserDisplayName).FirstOrDefault();
                    Session["displayusername"] = displayusername;                
                    // check session for company id and name
                //if (Session["companyname"] != null) {
                //    companyname = Session["companyname"].ToString();
                //} else {
                    if (siteusercompanyid > 0) {
                        SiteCompanies sitecompany = db.SiteCompanies.Where(x => x.SiteCoID == siteusercompanyid).ToList().Single();
                        companyname = sitecompany.CoName;
                        Session["companyname"] = companyname;                       
                    }
             //   }
                ViewBag.companyname = companyname;
                var siteco = db.SiteCompanies.Where(p => p.SiteCoID == (siteusercompanyid)).FirstOrDefault();
                ViewBag.SiteCoInfo = siteco;
                if (siteco.Logo != null)
                {
                    Session.Remove("ImageData");
                    string imageBase64Data = Convert.ToBase64String(siteco.Logo);
                    string imageDataURL = string.Format("data:image/png;base64,{0}", imageBase64Data);
                    Session["ImageData"] = imageDataURL;
                }
            }


        }


    }
}