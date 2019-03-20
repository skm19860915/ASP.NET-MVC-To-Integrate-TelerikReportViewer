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
using System.Text;
using System.Web.Configuration;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;

namespace ePonti.web.Areas.Sections.Controllers
{
    [Authorize]
    public class QuotesController : ePonti.web.Controllers._baseMVCController
    {
        private ePontiv2Entities db = new ePontiv2Entities();
        STServices sTService = null;

        // GET: Sections/Quotes
        public ActionResult Index()
        {
            ViewBag.ProjectHideStatus = new SelectList(db.CoProjectStatus.Where(p => p.SiteCoID == siteusercompanyid).GroupBy(p => p.ProjectStatusName).Select(c => c.Key).ToList());
            ViewBag.ContactCompanies = db.GetQuoteCustomersBySiteCoID(siteusercompanyid).Where(s => s.Customer != null && s.Customer.Trim() != "").ToList();
            ViewBag.ProjectStatus = db.GetQuoteUsedStatusBySiteCoID(siteusercompanyid).ToList();
            //var leads = db.GetQuotesBySiteCoID(siteusercompanyid).ToList();
            return View();
        }

        [HttpPost]
        public ActionResult GetQuotes([DataSourceRequest]DataSourceRequest request,string SearchText, string SelectedCompanies, string SelectedProjectStatus, IEnumerable<string> HideStatus = null)
        {
            try
            {
                ViewBag.ProjectHideStatus = new SelectList(db.CoProjectStatus.Where(p => p.SiteCoID == siteusercompanyid).GroupBy(p => p.ProjectStatusName).Select(c => c.Key).ToList());
                ViewBag.ContactCompanies = db.GetQuoteCustomersBySiteCoID(siteusercompanyid).Where(s => s.Customer != null && s.Customer.Trim() != "").ToList();
                ViewBag.ProjectStatus = db.GetQuoteUsedStatusBySiteCoID(siteusercompanyid).ToList();
                var Quotes = db.GetQuotesBySiteCoID(siteusercompanyid).ToList();
                if (SearchText != null && SearchText != "")
                {
                    SearchText = SearchText.Trim().ToLower();
                    Quotes = Quotes.Where(p => (p.Project == null ? "" : p.Project).ToLower().Contains(SearchText)).ToList();
                }
                char[] sep = new char[] { ',' };
                if (SelectedCompanies != null && SelectedCompanies != "")
                    Quotes = Quotes.Where(p => SelectedCompanies.Split(sep).Contains(Convert.ToString(p.ContactID))).ToList();
                if (SelectedProjectStatus != null && SelectedProjectStatus != "")
                    Quotes = Quotes.Where(p => SelectedProjectStatus.Split(sep).Contains(Convert.ToString(p.StatusID))).ToList();
                if (HideStatus != null)
                {
                    if (HideStatus.Count() > 0)
                    {
                        foreach (string sts in HideStatus)
                            Quotes = Quotes.Where(p => p.Status != sts).ToList();
                    }
                }

                DataSourceResult result = Quotes.ToDataSourceResult(request);
                return Json(result, JsonRequestBehavior.AllowGet);
                // return View("Index", Quotes);
                //  return Json(new { status = "success", leads = leads });
            }
            catch (Exception ex)
            {
                return Json(new { status = "error", message = ex.Message });
            }
        }

        public ActionResult SessionFiles()
        {
            if (TempData["SaleztoolSyncMessage"] != null && TempData["SaleztoolSyncSuccess"] != null)
            {
                ViewBag.SyncSuccessMessage = TempData["SaleztoolSyncMessage"];
                ViewBag.IsSuccess = TempData["SaleztoolSyncSuccess"];
                TempData.Remove("SaleztoolSyncSuccess");
                TempData.Remove("SaleztoolSyncMessage");
            }
            sTService = new STServices(siteuserid);
            ViewBag.IsSTConnected = sTService.IsConnected(siteuserid);

            if (ViewBag.IsSTConnected == true)
            {
                List<STSessionModel> sessionList = sTService.GetSessions();
                ViewBag.SessionList = sessionList;
            }
            return View();
        }
        public ActionResult STSync(string fileid)
        {
            sTService = new STServices(siteuserid);
            SessionInfoModel sessionList = sTService.GetSessionInfo(fileid);
            //ViewBag.SessionList = sessionList;
            if (sessionList != null && sessionList.clientInfo != null)
            {
                bool issuccess = AddUpdateSaleztoolSync(fileid, sessionList);
                TempData["SaleztoolSyncSuccess"] = issuccess;
                TempData["SaleztoolSyncMessage"] = issuccess ? "Contact and project synced successfully" : "An error occoured! Please try again later";
            }
            else
            {
                TempData["SaleztoolSyncSuccess"] = false;
                TempData["SaleztoolSyncMessage"] = "No contact info available";
            }
            return RedirectToAction("sessionfiles");
        }
        public bool AddUpdateSaleztoolSync(string fileid, SessionInfoModel Sessioninfo)
        {
            bool ret = false;
            try
            {
                STClientModel info = Sessioninfo.clientInfo;
                if (info != null)
                {
                    CoContacts contact = db.CoContacts.Where(s => (s.ContactFirstName == null ? "" : s.ContactFirstName).Trim().ToLower() == info.firstName.Trim().ToLower() && (s.ContactLastName == null ? "" : s.ContactLastName).Trim().ToLower() == info.lastName.Trim().ToLower()).FirstOrDefault();
                    if (contact != null)
                    {
                        contact.ApiLinkID = fileid;// info.otherField == null ? "" : info.otherField;
                        db.SaveChanges();
                    }
                    else
                    {
                        string firstName = info.firstName == null ? "" : info.firstName.Trim();
                        string lastName = info.lastName == null ? "" : info.lastName.Trim();
                        string otherField = fileid;//info.otherField == null ? "" : info.otherField.Trim();
                        string address1 = info.homeAddress == null ? "" : info.homeAddress.Street == null ? "" : info.homeAddress.Street.Trim();
                        string City = info.homeAddress == null ? "" : info.homeAddress.City == null ? "" : info.homeAddress.City.Trim();
                        string Zip = info.homeAddress == null ? "" : info.homeAddress.Zip == null ? "" : info.homeAddress.Zip.Trim();
                        string State = info.homeAddress == null ? "" : info.homeAddress.State == null ? "" : info.homeAddress.State.Trim();
                        string emailAddress = info.emailAddress == null ? "" : info.emailAddress.Trim();
                        string Phone = info.homeAddress == null ? "" : info.homeAddress.Phone == null ? "" : info.homeAddress.Phone.Trim();
                        db.InsertSalezToolzCustomer(siteusercompanyid, firstName, lastName, siteuserid, siteuserid, otherField, address1, City, State, Zip, emailAddress, Phone);
                        //Get Contact
                        contact = db.CoContacts.Where(s => (s.ContactFirstName == null ? "" : s.ContactFirstName).Trim().ToLower() == firstName.ToLower() && (s.ContactLastName == null ? "" : s.ContactLastName).Trim().ToLower() == lastName.ToLower()).FirstOrDefault();
                    }
                    //Project for contact
                    if (contact != null)
                    {
                        string projectname = info.fileName == null ? "" : info.fileName.Trim();
                        if (projectname != "")
                        {
                            ProjectInfo project = db.ProjectInfo.Where(s => (s.ProjectName == null ? "" : s.ProjectName).Trim().ToLower() == projectname.ToLower()).FirstOrDefault();
                            if (project != null)
                            {
                                project.ApiLinkID = fileid;// info.otherField == null ? "" : info.otherField;
                                db.SaveChanges();
                            }
                            else
                            {
                                string otherField = fileid;//info.otherField == null ? "" : info.otherField.Trim();
                                string address1 = info.workAddress == null ? "" : info.workAddress.Street == null ? "" : info.workAddress.Street.Trim();
                                string City = info.workAddress == null ? "" : info.workAddress.City == null ? "" : info.workAddress.City.Trim();
                                string Zip = info.workAddress == null ? "" : info.workAddress.Zip == null ? "" : info.workAddress.Zip.Trim();
                                string State = info.workAddress == null ? "" : info.workAddress.State == null ? "" : info.workAddress.State.Trim();
                                string emailAddress = info.emailAddress == null ? "" : info.emailAddress.Trim();
                                string Phone = info.workAddress == null ? "" : info.workAddress.Phone == null ? "" : info.workAddress.Phone.Trim();
                                DateTime startdate = info.createDate == null ? DateTime.Now : info.createDate;

                                //tax info
                                bool istax = Sessioninfo.taxProfile != null;
                                string taxCode = !istax ? "" : Sessioninfo.taxProfile.taxName == null ? "" : Sessioninfo.taxProfile.taxName.Trim();
                                string taxDescription = (istax && Sessioninfo.taxProfile.taxes != null && Sessioninfo.taxProfile.taxes.Count > 0) ? Sessioninfo.taxProfile.taxes[0].applyType == null ? "" : Sessioninfo.taxProfile.taxes[0].applyType : "";
                                decimal salesrate_dec = (istax && Sessioninfo.taxProfile.taxes != null && Sessioninfo.taxProfile.taxes.Count > 0) ? Sessioninfo.taxProfile.taxes[0].value == null ? 0 : ExtractDecimalFromString(Sessioninfo.taxProfile.taxes[0].value) : 0;
                                //decimal salesrate_dec = 0;
                                //decimal.TryParse(salesrate, out salesrate_dec);

                                //Adjustment
                                bool isadjust = Sessioninfo.adjustment != null;
                                decimal equipAdjust = 0;
                                string equipAdjustLabel = "";
                                decimal laborAdjust = 0;
                                string laborAdjustLabel = "";
                                decimal otherAdjust = 0;
                                string otherAdjustLabel = "";
                                string otherAdjustType = "";
                                decimal profileAdjust = 0;
                                string profileAdjustLabel = "";
                                string profileAdjustType = "";
                                StringBuilder projectItems = new StringBuilder();
                                if (isadjust)
                                {
                                    equipAdjust = Sessioninfo.adjustment.equipment == null ? 0 : ExtractDecimalFromString(Sessioninfo.adjustment.equipment.Trim());
                                    equipAdjustLabel = Sessioninfo.adjustment.equipmentLabel == null ? "" : Sessioninfo.adjustment.equipmentLabel.Trim();
                                    laborAdjust = Sessioninfo.adjustment.labor == null ? 0 : ExtractDecimalFromString(Sessioninfo.adjustment.labor.Trim());
                                    laborAdjustLabel = Sessioninfo.adjustment.laborLabel == null ? "" : Sessioninfo.adjustment.laborLabel.Trim();
                                    otherAdjust = Sessioninfo.adjustment.otherDiscountValue == null ? 0 : ExtractDecimalFromString(Sessioninfo.adjustment.otherDiscountValue.Trim());
                                    otherAdjustLabel = Sessioninfo.adjustment.otherDiscountLabel == null ? "" : Sessioninfo.adjustment.otherDiscountLabel.Trim();
                                    otherAdjustType = Sessioninfo.adjustment.discountType == null ? "" : Sessioninfo.adjustment.discountType.Trim();
                                    //profileAdjust = Sessioninfo.adjustment.equipment == null ? 0 : ExtractDecimalFromString(Sessioninfo.adjustment.equipment.Trim());
                                }

                                if(Sessioninfo.selectedPackages!=null && Sessioninfo.selectedPackages.Count>0)
                                {
                                    projectItems.Append("<ArrayProjectItems>");
                                    foreach (STPackageModel package in Sessioninfo.selectedPackages)
                                    {
                                        projectItems.Append("<ProjectItem>");
                                        projectItems.Append("<SiteUserID>" + siteuserid + "</SiteUserID>");
                                        projectItems.Append("<GroupName>" + SetValueForXML(package.packageGroupName) + "</GroupName>");
                                        projectItems.Append("<ManufacturerName>" + SetValueForXML(package.manufacturer) + "</ManufacturerName>");
                                        projectItems.Append("<ContactCoName>" + SetValueForXML(package.vendor) + "</ContactCoName>");
                                        projectItems.Append("<Model>" + SetValueForXML(package.packageName) + "</Model>");
                                        projectItems.Append("<SKU>" + SetValueForXML(package.sku) + "</SKU>");
                                        projectItems.Append("<ProductDescription>" + SetValueForXML(package.description) + "</ProductDescription>");
                                        projectItems.Append("<SalesDescription>" + SetValueForXML(package.description) + "</SalesDescription>");
                                        //projectItems.Append("<ProductDescription></ProductDescription>");
                                        //projectItems.Append("<SalesDescription></SalesDescription>");
                                        projectItems.Append("<Hyperlink></Hyperlink>");
                                        projectItems.Append("<StageName>" + SetValueForXML(package.phase) + "</StageName>");
                                        projectItems.Append("<SalesTaxed>" + (SetValueForXML(package.taxable).Trim().ToLower()=="yes" ? "True" : "False") + "</SalesTaxed>");
                                        projectItems.Append("<Cost>" + SetDecimalValueForXML(package.cost) + "</Cost>");
                                        projectItems.Append("<Price>" + SetDecimalValueForXML(package.equipment) + "</Price>");
                                        projectItems.Append("<DivisionName>" + SetValueForXML(package.packageGroupName) + "</DivisionName>");
                                        if (package.roomsQuantity != null && package.roomsQuantity.Count > 0)
                                        {
                                            projectItems.Append("<Qty>" + SetDecimalValueForXML(package.roomsQuantity[0].quantity) + "</Qty>");
                                        }
                                        else
                                        {
                                            projectItems.Append("<Qty>1</Qty>");
                                        }
                                        projectItems.Append("<EstHrs>" + (SetDecimalValueForXML(package.labor1Hours)<=0 ? 1 : SetDecimalValueForXML(package.labor1Hours)) + "</EstHrs>");
                                        projectItems.Append("<StageLabor1Cost>" + SetDecimalValueForXML(package.labor1HourlyRateCost) + "</StageLabor1Cost>");
                                        projectItems.Append("<StageLabor1Price>" + SetDecimalValueForXML(package.labor1HourlyRate) + "</StageLabor1Price>");
                                        projectItems.Append("<StageLabor2Cost>" + SetDecimalValueForXML(package.labor2HourlyRateCost) + "</StageLabor2Cost>");
                                        projectItems.Append("<StageLabor2Price>" + SetDecimalValueForXML(package.labor2HourlyRate) + "</StageLabor2Price>");
                                        projectItems.Append("<Labor1Cost>" + SetDecimalValueForXML(package.labor1Cost) + "</Labor1Cost>");
                                        projectItems.Append("<Labor1Price>" + SetDecimalValueForXML(package.labor1Price) + "</Labor1Price>");
                                        projectItems.Append("<Labor2Cost>" + SetDecimalValueForXML(package.labor2Cost) + "</Labor2Cost>");
                                        projectItems.Append("<Labor2Price>" + SetDecimalValueForXML(package.labor2Price) + "</Labor2Price>");
                                        projectItems.Append("<ModelLabor1>" + SetValueForXML(package.packageName) + " Labor 1</ModelLabor1>");
                                        projectItems.Append("<ModelLabor2>" + SetValueForXML(package.packageName) + " Labor 2</ModelLabor2>");
                                        projectItems.Append("</ProjectItem>");
                                    }
                                    projectItems.Append("</ArrayProjectItems>");
                                }

                                db.InsertSalezToolzProjectInfo(siteusercompanyid, 5/*projecttypeid*/, projectname, contact.ContactID, address1, City, State, Zip, startdate, Phone, emailAddress, siteuserid/*salesid*/, otherField, taxCode, taxDescription, salesrate_dec, equipAdjust, equipAdjustLabel, laborAdjust, laborAdjustLabel, otherAdjust, otherAdjustLabel, otherAdjustType, profileAdjust, profileAdjustLabel, profileAdjustType, projectItems.ToString());
                            }
                        }
                    }
                }
                ret = true;
            }
            catch (Exception ex)
            {
                ret = false;
                //throw ex;
            }
            return ret;
        }
        public string SetValueForXML(string val)
        {
            if(val==null)
            {
                return "";
            }
            else
            {
                return val.Replace("\n", " ").Replace("\r", "").Replace("&","&amp;");//.Replace("•", "");
            }
        }
        public Decimal SetDecimalValueForXML(decimal? val)
        {
            if (val == null)
            {
                return 0;
            }
            else
            {
                return val.Value;
            }
        }
        public static Decimal ExtractDecimalFromString(string str)
        {
            try
            {
                str = str.Replace("Rs.", "").Replace("%", "").Replace("-", "").Replace("+", "").Replace("$", "");
                decimal dc = Convert.ToDecimal(str);
                return dc;
            }
            catch (Exception)
            {
                return 0;
            }
        }
    }
}
