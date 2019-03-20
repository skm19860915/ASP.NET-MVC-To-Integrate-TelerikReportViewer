using System;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using ePonti.BOL;
using ePonti.web.Models;
using System.Collections.Generic;
using System.Web;
using ePonti.BOL.Models;

namespace ePonti.web.Areas.Options.Controllers
{
    public class CoAccountController : ePonti.web.Controllers._baseMVCController
    {
        private ePontiv2Entities db = new ePontiv2Entities();
        BOL.Repository.CommonRepository repo = new BOL.Repository.CommonRepository();

        // GET: Options/CoAccount
        public ActionResult Index()
        {
            int siteCoID = base.siteusercompanyid;
            ViewBag.CoSiteUsers = db.GetSiteUsersBySiteCoID(siteCoID);
            ViewBag.CoPayTypes = db.GetPayTypesBySiteCoID(siteCoID);
            ViewBag.CoProfiles = db.GetProfilesBySiteCoID(siteCoID);
            //ViewBag.CoProfileInfo = db.GetProfileInfoByCoProfileID(siteCoID);
            ViewBag.CoLicenses = db.GetLicensesBySiteCoID(siteCoID);
            ViewBag.CoCalendar = db.GetCalendarsBySiteCoID(siteCoID).ToList();
            var q = db.GetPayrollBySiteCoID(siteCoID).FirstOrDefault();
            ViewBag.CoPayroll = q;
            ViewBag.Frequency = new SelectList(db.PayrollFrequencies.ToList(), "PayrollFrequencyID", "Frequency");
            if (q != null)
            {
                if (q.FrequencyID != null)
                    ViewBag.Frequency = new SelectList(db.PayrollFrequencies.ToList(), "PayrollFrequencyID", "Frequency", q.FrequencyID);
            }

            var siteco = db.SiteCompanies.Where(p => p.SiteCoID == (siteCoID)).FirstOrDefault();            
            ViewBag.SiteCoInfo = siteco;
            if (siteco.Logo != null)
            {
                string imageBase64Data = Convert.ToBase64String(siteco.Logo);
                string imageDataURL = string.Format("data:image/png;base64,{0}", imageBase64Data);
                ViewBag.ImageData = imageDataURL;
            }
            return View();
        }

        [HttpPost]
        public ActionResult DeleteOption(int itemID, string Type)
        {
            var res = repo.DeleteOption(Type, itemID, siteusercompanyid);
            return Json(new { status = (res == "1" ? "success" : res) });
        }

        #region Options Update
        [HttpPost]
        public ActionResult UpdatePayTypes(int ViewID, string Name, int Order, decimal Factor)
        {
            CoPayTypes item;
            if (ViewID == 0)
            {
                item = new CoPayTypes() { SiteCoID = siteusercompanyid };
                db.CoPayTypes.Add(item);
                db.SaveChanges();
            }
            else
            {
                item = db.CoPayTypes.Where(p => p.PayTypeID == ViewID && p.SiteCoID == siteusercompanyid).FirstOrDefault();

            }

            if (item != null)
            {
                item.PayType = Name;
                item.PayTypeNumber = Order;
                item.PayTypeFactor = Factor;
            }
            db.SaveChanges();
            return Json(new { status = "success", viewID = item.PayTypeID });
        }
        //public ActionResult UpdateCalendar(int ViewID, string Name, decimal Monday, decimal Tuesday, decimal Wednesday, decimal Thursday, decimal Friday, decimal Saturday, decimal Sunday, decimal DailyStartTime)
        [HttpPost]
        public ActionResult UpdateCalendar(GetCalendarsBySiteCoID_Result dataToPost)
        {
            CoCalendar item;
            if (dataToPost.ViewID == 0)
            {
                item = new CoCalendar() { SiteCoID = siteusercompanyid };
                db.CoCalendar.Add(item);
                db.SaveChanges();
            }
            else
            {
                item = db.CoCalendar.Where(p => p.CalendarID == dataToPost.ViewID && p.SiteCoID == siteusercompanyid).FirstOrDefault();
            }

            if (item != null)
            {
                var q = db.UpdateCoAccountCalendarByViewID(item.CalendarID, dataToPost.Name, dataToPost.Monday, dataToPost.Tuesday, dataToPost.Wednesday, dataToPost.Thursday, dataToPost.Friday, dataToPost.Saturday, dataToPost.Sunday, dataToPost.DailyStartTime);
                db.SaveChanges();

            }
            return Json(new { status = "success", ViewID = item.CalendarID });
        }
        #endregion

        #region Edit Company Info
        public ActionResult Edit(int? id)
        {
            if (id == null)
                id = base.siteusercompanyid;
            SiteCompanies coinfo = db.SiteCompanies.Where(p => p.SiteCoID == (id ?? 0)).FirstOrDefault();
            if (coinfo == null)
            {
                return HttpNotFound();
            }
            var model = LoadCoInfo(id ?? 0);

            return View("EditCo", coinfo);

        }
        [HttpPost]
        public ActionResult Edit(int? id, SiteCompanies sitecompany)
        {
            if (id == null)
                id = base.siteusercompanyid;
            SiteCompanies coinfo = db.SiteCompanies.Where(p => p.SiteCoID == (id ?? 0)).FirstOrDefault();
            if (coinfo == null)
            {
                return Json(new { status = "error" });
                // return HttpNotFound();
            }
            // var model = LoadCoInfo(id ?? 0);
            coinfo.CoName = sitecompany.CoName ?? coinfo.CoName;
            coinfo.CoAddress1 = sitecompany.CoAddress1 ?? coinfo.CoAddress1;
            coinfo.CoAddress2 = sitecompany.CoAddress2 ?? coinfo.CoAddress2;
            coinfo.CoCity = sitecompany.CoCity ?? coinfo.CoCity;
            coinfo.CoState = sitecompany.CoState ?? coinfo.CoState;
            coinfo.CoCountry = sitecompany.CoCountry ?? coinfo.CoCountry;
            coinfo.CoZip = sitecompany.CoZip ?? coinfo.CoZip;
            coinfo.CoPhone = sitecompany.CoPhone ?? coinfo.CoPhone;
            db.SaveChanges();
            return Json(new { status = "success" });
        }

        private CompanyModels.NewCo LoadCoInfo(int SiteCoID)
        {
            var model = new CompanyModels.NewCo();
            model.SiteCoID = SiteCoID;

            SiteCompanies coinfo = db.SiteCompanies.Where(p => p.SiteCoID == SiteCoID).FirstOrDefault();
            if (coinfo == null)
            {
                return model;
            }

            model = new CompanyModels.NewCo()
            {
                SiteCoID = coinfo.SiteCoID,
                CoName = coinfo.CoName,
                CoAddress1 = coinfo.CoAddress1,
                CoAddress2 = coinfo.CoAddress2,
                CoCity = coinfo.CoCity,
                CoState = coinfo.CoState,
                CoZip = coinfo.CoZip,
                CoCountry = coinfo.CoCountry,
                CoPhone = coinfo.CoPhone,
                CoDateCreated = coinfo.CoDateCreated,
                CoAcctNumber = coinfo.CoAcctNumber
            };

            var siteCoID = siteusercompanyid;

            ViewBag.SiteCountries = new SelectList("CountryID", "Country");

            return model;
        }

        #endregion

        #region Edit Profile Info
        public ActionResult ProfileInfo(int? id, string ab)
        {
            int? coProfileID = id;
            ViewBag.ProfileName = db.GetProfileInfoByCoProfileID(coProfileID, base.siteusercompanyid).Select(p => p.Profile).FirstOrDefault();
            ViewBag.CoSecurityProfiles = db.GetProfileInfoByCoProfileID(coProfileID, base.siteusercompanyid).ToList();
            return View("ProfileInfo");

        }
        [HttpPost]
        public ActionResult ProfileInfo(List<CoSecurityProfiles> Data)
        {
            int coProfileID = base.siteusercompanyid;
            foreach (var csp in Data)
            {
                db.UpdateProfileInfoByCoViewID(csp.CoSecurityProfileID, csp.SecurityView, csp.SecurityCreate, csp.SecurityEdit, csp.SecurityAdmin);
                db.SaveChanges();
            }

            return Json(new { status = "success" });
        }
        #endregion

        #region Update Payroll
        [HttpPost]
        public ActionResult UpdatePayroll(int? PayrollFrequencyID, DateTime? PayrollPeriodStart)
        {
            db.UpdatePayrollBySiteCoID(base.siteusercompanyid, PayrollFrequencyID, PayrollPeriodStart);
            db.SaveChanges();
            return Json(new { status = "success" });
        }
        #endregion

        #region Adding Licenses
        public ActionResult AddingLicenses()
        {
            ViewBag.duration = new SelectList(db.SiteLicenseDurations.ToList(), "LicenseDurationID", "Duration");
            int durationID = 1;
            ViewBag.AddingLicenses = db.GetLicensesInfo(durationID);

            return View("AddingLicenses");

        }
        [HttpPost]
        public ActionResult AddingLicenses(int? duration)
        {
            ViewBag.duration = new SelectList(db.SiteLicenseDurations.ToList(), "LicenseDurationID", "Duration");
            int durationID = (int)duration;
            ViewBag.AddingLicenses = db.GetLicensesInfo(durationID);
            return View("AddingLicenses");
        }
        [HttpPost]
        public ActionResult saveLicense(List<GetlicenseDetails> Data)
        {
            if (Data.Count > 0)
            {
                foreach (var licenseinfo in Data)
                {
                    for(int qty=1;qty<= licenseinfo.Quantity; qty++)
                    {
                        db.InsertLicense(base.siteusercompanyid, licenseinfo.LicenseID, licenseinfo.LicensePriceID, licenseinfo.Quantity);
                        db.SaveChanges();
                    }

                }
                return Json(new { status = "success" });
            }
            else
            return Json(new { status = "nodata" });
        }
        #endregion

        #region Insert Logo
        public ActionResult Upload()
        {
            return View();
        }
        // [HttpPost]
        public ActionResult UploadImage()
        {

            HttpPostedFileBase companylogo = null;
            foreach (string fileName in Request.Files)
            {
                companylogo = Request.Files[fileName];
            }
            if (companylogo != null)
            {
                string path = Server.MapPath("~/images/dummies/");
                companylogo.SaveAs(path + companylogo.FileName);
                byte[] imageByteData = System.IO.File.ReadAllBytes(path + companylogo.FileName);
                //string imageBase64Data = Convert.ToBase64String(imageByteData);
                //string imageDataURL = string.Format("data:image/png;base64,{0}", imageBase64Data);
                //ViewBag.ImageData = imageDataURL;
                db.UpdateCoLogoBySiteCoID(base.siteusercompanyid, imageByteData);
                db.SaveChanges();
                string upath = path + companylogo.FileName;
                if (System.IO.File.Exists(upath))
                {
                    System.IO.File.Delete(upath);
                }
            }
            return RedirectToAction("Index");
        }
        #endregion

        #region UserInfo
        public ActionResult UserInfo(int? id)
        {
            ViewBag.Licenses = new SelectList(db.GetLicensesBySiteCoID(siteusercompanyid).ToList(), nameof(GetLicenseListBySiteCoID_Result.LicenseID), nameof(GetLicenseListBySiteCoID_Result.Version));
            ViewBag.Profiles = new SelectList(db.GetProfilesBySiteCoID(siteusercompanyid).ToList(), nameof(GetProfilesBySiteCoID_Result.ViewID), nameof(GetProfilesBySiteCoID_Result.Name));
            ViewBag.Timekeeper = new SelectList(db.PayrollSecurity.ToList(), "PayrollSecurityID", "PayrollSecurityName");
            var model = db.GetUserProfileInfoBySiteUserID(id).FirstOrDefault();
            return View(model);
        }
        [HttpPost]
        public ActionResult UserInfo(UserProfile obj)
        {          
            if(obj!=null)
            {
                db.UpdateUserInfoBySiteUserID(base.siteusercompanyid, obj.ViewID, obj.UserDisplayName, obj.Job_Title, "active", obj.E_Mail, obj.Phone, obj.Profile, obj.PayrollSecurity, obj.License, obj.Calendar_Color);
                db.SaveChanges();
            return Json(new { status = "success" });
            }
            return Json(new { status = "error" });
        }
        #endregion
    }
}
