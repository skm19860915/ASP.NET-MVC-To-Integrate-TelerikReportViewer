using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ePonti.BOL;
using System.Data.Entity.Core.Objects;
using ePonti.web.Models;
using ePonti.BOL.Repository;
using ePonti.BLL.Common;

namespace ePonti.web.Areas.Pages.Controllers
{
    public class ContactInfoController : web.Controllers._baseMVCController
    {
        private ePontiv2Entities db = new ePontiv2Entities();
        Controller contactInfoController = null;
        Dictionary<Int64, QBSyncdto> syncRepo = null;
        QBSyncService syncService = null;
        QBSyncdto syncObjects = null;
        QBModels qbModels = null;
        static long SyncObjectsModelId = 0;
        public ContactInfoController()
        {
            syncRepo = new Dictionary<Int64, QBSyncdto>();
        }
        // GET: Pages/ContactInfo

        public ActionResult Details(int? id, int? ad)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var coContacts = db.GetContactInfoByContactID(id, base.siteusercompanyid).FirstOrDefault();
            if (coContacts == null)
            {
                return HttpNotFound();
            }
            
            ViewBag.Country = db.SiteCountries;
            ViewBag.Activities = db.GetActivitiesByContactID(id);
            ViewBag.ContactCustoms = db.GetContactCustomFieldsBySiteCoID(base.siteusercompanyid).FirstOrDefault();
            if (coContacts.IsVendor != true)
            {
                ViewBag.Projects = db.GetProjectsByContactID(id);
            }
            if (coContacts.IsVendor == true)
            {
                ViewBag.POR = db.GetPORByContactID(id);
            }
            ViewBag.Relationships = db.GetContactRelationshipsByContactID(id);
            //ViewBag.ContactTypes = db.GetContactTypesBySiteCoID(id);
            //ViewBag.ContactSubTypes = db.GetContactSubTypesBySiteCoID(id, ad);

            return View("details", coContacts);
        }
        #region sync single customer
        public ActionResult syncSingleCustomer(string type, int contactID,string nav)
        { 
            SyncSingleCustomer(type, contactID);
            return RedirectToAction("details", new { id = contactID, nav = nav });
        }
        private string SyncSingleCustomer(string type, int contactID)
        {

            if (type.ToUpper() == "CUSTOMER")
            {
                List<Intuit.Ipp.Data.Customer> custList = new List<Intuit.Ipp.Data.Customer>();
                var coContacts = db.GetQbCustomersBySiteCoID(siteusercompanyid).Where(x => x.ViewID == contactID).ToList();
                foreach (var cust in coContacts)
                {
                    Intuit.Ipp.Data.Customer qboCust = new Intuit.Ipp.Data.Customer();
                    qboCust.SyncToken = "1";
                    if (cust.Id != null && cust.Id != "")
                        qboCust.Id = cust.Id.ToString();
                    qboCust.GivenName = cust.GivenName;
                    if (cust.FamilyName != null && cust.FamilyName != "")
                        qboCust.FamilyName = cust.FamilyName;
                    if (cust.DisplayName != null && cust.DisplayName != "")
                        qboCust.DisplayName = cust.DisplayName;
                    if (cust.CompanyName != null && cust.CompanyName != "")
                    {
                        if (cust.CompanyName.Contains("'"))
                            cust.CompanyName = cust.CompanyName.Replace("'", "\'");
                        qboCust.CompanyName = cust.CompanyName;
                    }
                    if (cust.PrimaryPhone != null && cust.PrimaryPhone != "")
                        qboCust.PrimaryPhone = new Intuit.Ipp.Data.TelephoneNumber { FreeFormNumber = cust.PrimaryPhone.ToString() };
                    if (cust.Mobile != null && cust.Mobile != "")
                        qboCust.Mobile = new Intuit.Ipp.Data.TelephoneNumber { FreeFormNumber = cust.Mobile.ToString() };
                    if (cust.PrimaryEmailAddr != null && cust.PrimaryEmailAddr != "")
                        qboCust.PrimaryEmailAddr = new Intuit.Ipp.Data.EmailAddress { Address = cust.PrimaryEmailAddr.ToString() };
                    qboCust.BillAddr = new Intuit.Ipp.Data.PhysicalAddress();
                    if (cust.BillAddrId != 0)
                        qboCust.BillAddr.Id = Convert.ToString(cust.BillAddrId);
                    if (cust.BillAddrLine1 != null && cust.BillAddrLine1 != "")
                        qboCust.BillAddr.Line1 = cust.BillAddrLine1;
                    if (cust.BillAddrLine2 != null && cust.BillAddrLine2 != "")
                        qboCust.BillAddr.Line2 = cust.BillAddrLine2;
                    if (cust.BillAddrCity != null && cust.BillAddrCity != "")
                        qboCust.BillAddr.City = cust.BillAddrCity;
                    if (cust.BillAddrCountry != null && cust.BillAddrCountry != "")
                        qboCust.BillAddr.Country = cust.BillAddrCountry;
                    if (cust.BillAddrCountrySubDivisionCode != null && cust.BillAddrCountrySubDivisionCode != "")
                        qboCust.BillAddr.CountrySubDivisionCode = cust.BillAddrCountrySubDivisionCode;
                    if (cust.BillAddrPostalCode != null && cust.BillAddrPostalCode != "")
                        qboCust.BillAddr.PostalCode = cust.BillAddrPostalCode;
                    if (cust.BillAddrLine1 != null && cust.BillAddrLine1 != "")
                        qboCust.BillAddr.Line1 = cust.BillAddrLine1;
                    custList.Add(qboCust);
                }
                QBAuthorizationdto oAuthDetails = new QBOAuthService(new QBAuthorizationdto()).GetAccessToken(this);
                syncService = new QBSyncService(oAuthDetails);
                syncObjects = new QBSyncdto();
                syncObjects.OauthToken = oAuthDetails;
                syncObjects.CompanyId = oAuthDetails.Realmid;
                syncObjects.CustomerList = custList;
                if (syncObjects.CustomerList.Count > 0)
                {
                    syncObjects = syncService.SyncCustomer(this, syncObjects);
                }
            }
            else if(type.ToUpper() == "VENDOR")
            {
                List<Intuit.Ipp.Data.Vendor> vendorList = new List<Intuit.Ipp.Data.Vendor>();
                var vendor = db.GetQbVendorsBySiteCoID(siteusercompanyid).Where(p => p.ViewID == contactID).FirstOrDefault();
                if (vendor != null)
                {
                    Intuit.Ipp.Data.Vendor qboVendor = new Intuit.Ipp.Data.Vendor();
                    qboVendor.SyncToken = "1";
                    qboVendor.GivenName = vendor.GivenName;
                    if (vendor.FamilyName != null && vendor.FamilyName != "")
                        qboVendor.FamilyName = vendor.FamilyName;
                    if (vendor.DisplayName != null && vendor.DisplayName != "")
                        qboVendor.DisplayName = vendor.DisplayName;
                    if (vendor.CompanyName != null && vendor.CompanyName != "")
                        qboVendor.CompanyName = vendor.CompanyName;
                    if (vendor.PrimaryPhone != null && vendor.PrimaryPhone != "")
                        qboVendor.PrimaryPhone = new Intuit.Ipp.Data.TelephoneNumber { FreeFormNumber = vendor.PrimaryPhone.ToString() };
                    if (vendor.Mobile != null && vendor.Mobile != "")
                        qboVendor.Mobile = new Intuit.Ipp.Data.TelephoneNumber { FreeFormNumber = vendor.Mobile.ToString() };
                    if (vendor.PrimaryEmailAddr != null && vendor.PrimaryEmailAddr != "")
                        qboVendor.PrimaryEmailAddr = new Intuit.Ipp.Data.EmailAddress { Address = vendor.PrimaryEmailAddr.ToString() };
                    qboVendor.BillAddr = new Intuit.Ipp.Data.PhysicalAddress();
                    if (vendor.BillAddrId != 0)
                        qboVendor.BillAddr.Id = Convert.ToString(vendor.BillAddrId);
                    if (vendor.BillAddrLine1 != null && vendor.BillAddrLine1 != "")
                        qboVendor.BillAddr.Line1 = vendor.BillAddrLine1;
                    if (vendor.BillAddrLine2 != null && vendor.BillAddrLine2 != "")
                        qboVendor.BillAddr.Line2 = vendor.BillAddrLine2;
                    if (vendor.BillAddrCity != null && vendor.BillAddrCity != "")
                        qboVendor.BillAddr.City = vendor.BillAddrCity;
                    if (vendor.BillAddrCountry != null && vendor.BillAddrCountry != "")
                        qboVendor.BillAddr.Country = vendor.BillAddrCountry;
                    if (vendor.BillAddrCountrySubDivisionCode != null && vendor.BillAddrCountrySubDivisionCode != "")
                        qboVendor.BillAddr.CountrySubDivisionCode = vendor.BillAddrCountrySubDivisionCode;
                    if (vendor.BillAddrPostalCode != null && vendor.BillAddrPostalCode != "")
                        qboVendor.BillAddr.PostalCode = vendor.BillAddrPostalCode;
                    if (vendor.BillAddrLine1 != null && vendor.BillAddrLine1 != "")
                        qboVendor.BillAddr.Line1 = vendor.BillAddrLine1;
                    vendorList.Add(qboVendor); 
                }
                QBAuthorizationdto oAuthDetails = new QBOAuthService(new QBAuthorizationdto()).GetAccessToken(this);
                syncService = new QBSyncService(oAuthDetails);
                syncObjects = new QBSyncdto();
                syncObjects.OauthToken = oAuthDetails;
                syncObjects.CompanyId = oAuthDetails.Realmid;
                syncObjects.VendorList = vendorList;
                if (syncObjects.VendorList.Count > 0)
                {
                    syncObjects = syncService.SyncVendor(this, syncObjects);
                }
            }
            return "";
        }
        #endregion
        // GET: Pages/ContactInfo/Create
        public ActionResult Create(int? contactid, int? contactTypeID)
        {
            var siteCoID = siteusercompanyid;
            ViewBag.ContactCo = new SelectList(db.GetContactCompaniesBySiteCoID(siteCoID), "ViewID", "Company", siteCoID);
            ViewBag.Country = new SelectList(new CommonRepository().GetCountries(), "CountryID", "Country");
            ViewBag.ContactType = new SelectList(db.GetPeopleTypesBySiteCoID(siteCoID), "ViewID", "Name", siteCoID);
            ViewBag.ContactSubType = new SelectList(db.GetContactSubTypesBySiteCoID(siteCoID), "ViewID", "Type", siteCoID);
            ViewBag.ContactStatus = new SelectList(db.SiteContactStatus.OrderBy(p => p.StatusOrder), "StatusID", "Status");
            ViewBag.Owner = new SelectList(db.GetCoUsersBySiteCoID(siteCoID), "UserID", "User", siteCoID);
            ViewBag.ContactCustoms = db.GetContactCustomFieldsBySiteCoID(siteCoID).FirstOrDefault();
            ViewBag.siteuserid = siteuserid;
            qbModels = new QBModels();
            qbModels.SyncObjectsModel = new QBSyncdto();
            qbModels.OAuthorizationModel = new QBAuthorizationdto();
            qbModels.IsReadySync = false;
            var oAuthModel = new QBOAuthService(qbModels.OAuthorizationModel).IsTokenAvailable(this, siteusercompanyid);
            ViewBag.IsQBConnected = "false";
            if (oAuthModel.IsConnected)
            {
                qbModels.IsReadySync = true;
                qbModels.OAuthorizationModel = oAuthModel;
                qbModels.IsConnected = oAuthModel.IsConnected;
                var syncService = new QBSyncService(oAuthModel);
                qbModels.SyncObjectsModel.OauthToken = oAuthModel;
                Random random = new Random();
                SyncObjectsModelId = random.Next(1, 100);
                ViewBag.IsQBConnected = "true";
                //SyncObjectsModelId = qbModels.SyncObjectsModel.Id;
            }
            GetContactInfoByContactID_Result Model = new GetContactInfoByContactID_Result();
            Model.ViewID = 0;
            return View(Model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(GetContactInfoByContactID_Result Model)
        {
            var errorList = new List<string>();
            if (ModelState.IsValid)
            {
                int contactID = SaveContact(Model);
                Model.ViewID = contactID;
                if (contactID > 0)
                {
                    Int64 id = SyncObjectsModelId;
                    //QBAuthorizationdto oAuthDetails = new QBOAuthService(new QBAuthorizationdto()).GetAccessToken(this);
                    //if (oAuthDetails != null)
                    //{
                    //    syncService = new QBSyncService(oAuthDetails);
                    //    syncObjects = id > 0 ? GetSyncObjects(this, id) : new QBSyncdto();
                    //    syncObjects.OauthToken = oAuthDetails;
                    //    syncObjects.CompanyId = oAuthDetails.Realmid;
                    //    if (Model.TypeID == 1)
                    //    {
                    //        if (!syncService.IsCustSync(syncObjects, syncService, siteusercompanyid).IsCustomerSync)
                    //        {
                    //            syncObjects = syncService.GetDatafromModelCustomer(syncObjects, siteusercompanyid);
                    //            if (syncObjects.CustomerList.Count > 0)
                    //            {
                    //                syncObjects = syncService.SyncCustomer(this, syncObjects);
                    //            }
                    //            this.Save(this, syncObjects);
                    //        }
                    //    }
                    //    else if (Model.TypeID == 3)
                    //    {
                    //        if (!syncService.IsVendorSync(syncObjects, syncService, siteusercompanyid).IsVendorSync)
                    //        {
                    //            syncObjects = syncService.GetDatafromModelVendor(syncObjects, siteusercompanyid);
                    //            if (syncObjects.VendorList.Count > 0)
                    //            {
                    //                syncObjects = syncService.SyncVendor(this, syncObjects);
                    //            }
                    //            this.Save(this, syncObjects);
                    //        }
                    //    }
                    //}
                    return Json(new { status = "success", contactID });
                }
                else
                {
                    errorList.Add("Contact couldn't be saved. Please retry.");
                }
            }
            errorList.AddRange((from item in ModelState.Values
                                from error in item.Errors
                                select error.ErrorMessage).ToList()
                             );
            return Json(new { status = "error", errors = errorList });
        }

        private int SaveContact(GetContactInfoByContactID_Result Model)
        {
            int siteCoID = siteusercompanyid;

            CommonRepository repo = new CommonRepository();
            CoContactCompanies company = null;
            if (!string.IsNullOrWhiteSpace(Model.ContactCompany))
            {
                company = repo.AddCompany(siteCoID, Model.ContactCompany);
            }
            if (company == null)
            {
                company = new CoContactCompanies() { ContactCoID = 0 };
            }

            var coContact = new CoContacts()
            {
                ContactID = Model.ViewID,
                SiteCoID = siteCoID,
                ContactTypeID = Model.ContactTypeID ?? 0,
                ContactSubtypeID = Model.ContactSubtypeID ?? 0,
                //When creating it needs to be set to 1 Active otherwise if they do Not it errors - REQUIRED
                ContactStatusID = Model.ContactStatusID,
                ContactFirstName = Model.ContactFirst,
                ContactLastName = Model.ContactLast,
                ContactCoID = company.ContactCoID,
                ModifiedDate = DateTime.Now,
                Custom1 = Model.Custom1,
                Custom2 = Model.Custom2,
                Custom3 = Model.Custom3,
                Custom4 = Model.Custom4,
                ContactManager = Model.ContactOwnerID
            };
            coContact.CoContactPhones.Add(new CoContactPhones() { PhoneTypeID = (int)EnumWrapper.PhoneTypes.Mobile, Phone = Model.Mobile, IsDefault = true });
            coContact.CoContactPhones.Add(new CoContactPhones() { PhoneTypeID = (int)EnumWrapper.PhoneTypes.Phone, Phone = Model.Phone, IsDefault = true });
            coContact.CoContactEmails.Add(new CoContactEmails() { EmailAddress = Model.ContactEmail, IsDefault = true });
            coContact.CoContactAddresses.Add(new CoContactAddresses()
            {
                Address1 = Model.ContactAddress1,
                Address2 = Model.ContactAddress2,
                City = Model.ContactCity,
                State = Model.ContactState,
                Zip = Model.ContactZip,
                CountryID = Model.CountryID,
                AddressTypeID = (int)EnumWrapper.AddressTypes.Main
            });

            int contactID = repo.AddContact(coContact);
            if (contactID > 0)
            {
                return contactID;
            }
            else
            {
                return 0;
            }
        }

        public ActionResult CreateCustomer(PeopleModels.NewContact Model)
        {
            Int64 id = SyncObjectsModelId;
            QBAuthorizationdto oAuthDetails = new QBOAuthService(new QBAuthorizationdto()).GetAccessToken(this);
            syncService = new QBSyncService(oAuthDetails);
            syncObjects = id > 0 ? GetSyncObjects(this, id) : new QBSyncdto();
            syncObjects.OauthToken = oAuthDetails;
            syncObjects.CompanyId = oAuthDetails.Realmid;

            if (!syncService.IsCustSync(syncObjects, syncService, siteusercompanyid).IsCustomerSync)
            {
                syncObjects = syncService.GetDatafromModelCustomer(syncObjects, siteusercompanyid);
                if (syncObjects.CustomerList.Count > 0)
                {
                    syncObjects = syncService.SyncCustomer(this, syncObjects);
                }
                this.Save(this, syncObjects);
            }
            return RedirectToAction("Details", "contactinfo", new { id = Model.ContactID, area = "pages" });
        }
        internal QBSyncdto Save(object controller, QBSyncdto syncObjects)
        {
            contactInfoController = controller as Controller;
            Random random = new Random();
            syncObjects.Id = random.Next(1, 100);
            syncRepo.Add(syncObjects.Id, syncObjects);
            contactInfoController.TempData["Sync"] = syncRepo;
            contactInfoController.TempData.Keep();
            return syncObjects;
        }
        internal QBSyncdto GetSyncObjects(object controller, Int64 id)
        {
            contactInfoController = controller as System.Web.Mvc.Controller;
            Dictionary<Int64, QBSyncdto> syncRepo = contactInfoController.TempData["Sync"] as Dictionary<Int64, QBSyncdto>;
            contactInfoController.TempData.Keep();
            if (syncRepo.ContainsKey(id))
            {
                return syncRepo[id];
            }
            else
            {
                return syncRepo[syncRepo.First().Key];
            }
        }
        public ActionResult Edit(int id)
        {

            var siteCoID = siteusercompanyid;
            ViewBag.ContactCo = new SelectList(db.GetContactCompaniesBySiteCoID(siteCoID), "ViewID", "Company", siteCoID);
            ViewBag.Country = new SelectList(new CommonRepository().GetCountries(), "CountryID", "Country");
            ViewBag.ContactType = new SelectList(db.GetPeopleTypesBySiteCoID(siteCoID), "ViewID", "Name", siteCoID);
            ViewBag.ContactSubType = new SelectList(db.GetContactSubTypesBySiteCoID(siteCoID), "ViewID", "Type", siteCoID);
            ViewBag.ContactStatus = new SelectList(db.SiteContactStatus.OrderBy(p => p.StatusOrder), "StatusID", "Status");
            ViewBag.Owner = new SelectList(db.GetCoUsersBySiteCoID(siteCoID), "UserID", "User", siteCoID);
            ViewBag.ContactCustoms = db.GetContactCustomFieldsBySiteCoID(siteCoID).FirstOrDefault();
            ViewBag.siteuserid = siteuserid;

            GetContactInfoByContactID_Result Model = new GetContactInfoByContactID_Result();
            CoContacts contact = db.CoContacts
                .Include(p => p.CoContactAddresses)
                .Include(p => p.CoContactEmails)
                .Where(p => p.ContactID == id).FirstOrDefault();
            CoContactAddresses address = contact.CoContactAddresses.Where(p => p.ContactID == id).FirstOrDefault() ?? new CoContactAddresses();
            if (contact != null)
            {
                Model = new GetContactInfoByContactID_Result()
                {
                    ContactAddress1 = address.Address1,
                    ContactAddress2 = address.Address2,
                    ContactCity = address.City,
                    CountryID = address.CountryID,
                    ContactState = address.State,
                    ContactZip = address.Zip,
                    ContactCompany = contact.CoContactCompanies != null ? contact.CoContactCompanies.ContactCoName : "",
                    ViewID = contact.ContactID,
                    ContactEmail = contact.CoContactEmails.Any() ? contact.CoContactEmails.Select(p => p.EmailAddress).FirstOrDefault() : "",
                    ContactFirst = contact.ContactFirstName,
                    ContactLast = contact.ContactLastName,
                    Mobile = contact.CoContactPhones.Any() ? contact.CoContactPhones.Where(p => p.PhoneTypeID == 2).Select(p => p.Phone).FirstOrDefault() : "",
                    Phone = contact.CoContactPhones.Any() ? contact.CoContactPhones.Where(p => p.PhoneTypeID == 1).Select(p => p.Phone).FirstOrDefault() : "",
                    ContactStatusID = contact.ContactStatusID,
                    ContactTypeID = contact.ContactTypeID,
                    ContactOwnerID = contact.ContactManager,
                    ContactSubtypeID = contact.ContactSubtypeID,
                    FaceBook = contact.FaceBook,
                    LinkedIn = contact.LinkedIn,
                    Skype = contact.Skype,
                    Custom1 = contact.Custom1,
                    Custom2 = contact.Custom2,
                    Custom3 = contact.Custom3,
                    Custom4 = contact.Custom4

                };
            }

            return View("_Edit", Model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(GetContactInfoByContactID_Result Model)
        {
            var errorList = new List<string>();

            if (ModelState.IsValid)
            {
                if (EditContact(Model))
                {
                    return Json(new { status = "success", contactID = Model.ViewID });
                }
                else
                {
                    errorList.Add("Contact couldn't be updated. Please retry.");
                }
            }

            errorList.AddRange((from item in ModelState.Values
                                from error in item.Errors
                                select error.ErrorMessage).ToList()
                             );

            return Json(new { status = "error", errors = errorList });


        }

        private bool EditContact(GetContactInfoByContactID_Result Model)
        {
            int siteCoID = siteusercompanyid;

            CommonRepository repo = new CommonRepository();
            CoContactCompanies company = null;
            if (!string.IsNullOrWhiteSpace(Model.ContactCompany))
            {
                company = repo.AddCompany(siteCoID, Model.ContactCompany);
            }
            if (company == null)
            {
                company = new CoContactCompanies() { ContactCoID = 0 };
            }

            CoContacts contact = db.CoContacts
            .Include(p => p.CoContactAddresses)
            .Include(p => p.CoContactEmails)
            .Where(p => p.ContactID == Model.ViewID).FirstOrDefault();

            contact.ContactID = Model.ViewID;
            contact.SiteCoID = siteCoID;
            contact.ContactTypeID = Model.ContactTypeID ?? 0;
            contact.ContactSubtypeID = Model.ContactSubtypeID ?? 0;
            contact.ContactStatusID = Model.ContactStatusID;
            contact.ContactFirstName = Model.ContactFirst;
            contact.ContactLastName = Model.ContactLast;
            contact.ContactCoID = company.ContactCoID;
            contact.ModifiedDate = DateTime.Now;
            contact.ContactManager = Model.ContactOwnerID;
            contact.Custom1 = Model.Custom1;
            contact.Custom2 = Model.Custom2;
            contact.Custom3 = Model.Custom3;
            contact.Custom4 = Model.Custom4;
            db.SaveChanges();

            repo.UpdatePhone(Model.ViewID, (int)EnumWrapper.PhoneTypes.Mobile, Model.Mobile, true);
            repo.UpdatePhone(Model.ViewID, (int)EnumWrapper.PhoneTypes.Phone, Model.Phone, true);
            repo.UpdateEmail(Model.ViewID, Model.ContactEmail, true);
            repo.UpdateAddress(Model.ViewID, new CoContactAddresses()
            {
                Address1 = Model.ContactAddress1,
                Address2 = Model.ContactAddress2,
                City = Model.ContactCity,
                State = Model.ContactState,
                Zip = Model.ContactZip,
                CountryID = Model.CountryID,
                AddressTypeID = (int)EnumWrapper.AddressTypes.Main
            });
            return true;

        }

        // GET: Pages/ContactInfo/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CoContacts coContacts = db.CoContacts.Find(id);
            if (coContacts == null)
            {
                return HttpNotFound();
            }
            return View(coContacts);
        }

        // POST: Pages/ContactInfo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CoContacts coContacts = db.CoContacts.Find(id);
            db.CoContacts.Remove(coContacts);
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
        [HttpPost]
        public ActionResult GetTemplates()
        {
            int siteCoID = siteusercompanyid;
           
            var Templates = db.GetServiceTemplatesBySiteCoID(siteCoID);
            return Json(Templates, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult CheckItemsForProject(int ServiceTemplateId)
        {
            int siteCoID = siteusercompanyid;
            int ProjectId = 0;
            var Template = db.ProjectInfo.Where(s => s.ProjectTypeID == 9 && s.SiteCoID == siteCoID).FirstOrDefault();
            if(Template != null) {
             ProjectId = Template.ProjectID;
            }

            int TotalItems = db.ProjectItems.Where(s => s.ProjectID == ProjectId).Count();
            
            return Json(TotalItems, JsonRequestBehavior.AllowGet);
        }
    }
}
