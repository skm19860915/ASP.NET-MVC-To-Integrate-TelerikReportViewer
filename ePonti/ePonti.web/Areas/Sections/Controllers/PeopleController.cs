using ePonti.BLL.Common;
using ePonti.BOL;
using ePonti.BOL.Repository;
using ePonti.web.Models;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace ePonti.web.Areas.Sections.Controllers
{
    [Authorize]
    public class PeopleController : ePonti.web.Controllers._baseMVCController
    {
        private ePontiv2Entities db = new ePontiv2Entities();
        Controller peopleOptionsController = null;
        Dictionary<Int64, QBSyncdto> syncRepo = null;
        QBSyncService syncService = null;
        QBSyncdto syncObjects = null;
        QBModels qbModels = null;
        public PeopleController()
        {
            syncRepo = new Dictionary<Int64, QBSyncdto>();
        }
        // GET: Sections/People
        public ActionResult Index()
        {
            //ViewBag.Sorting = new string[] { "Last", "First", "Company" };
            //ViewBag.Company=new SelectList(db.GetContactCompaniesBySiteCoID(siteusercompanyid).ToList(),nameof(GetContactCompaniesBySiteCoID_Result.ViewID), nameof(GetContactCompaniesBySiteCoID_Result.Company));
            ViewBag.CompanyList = db.GetContactCompaniesBySiteCoID(siteusercompanyid).Where(s => s.Company != null && s.Company.Trim() != "").ToList();
            //ViewBag.ContactType = new SelectList(db.CoContactTypes.Where(p => p.SiteCoID == siteusercompanyid).OrderBy(p => p.ContactTypeOrder), "ContactTypeID", "ContactTypeName");
            ViewBag.ContactTypeList = db.CoContactTypes.Where(p => p.SiteCoID == siteusercompanyid).OrderBy(p => p.ContactTypeOrder).ToList();
            int siteCoID = base.siteusercompanyid;
            //var coContacts = db.CoContacts
            //    .Where(p => p.SiteCoID == siteCoID)
            //    .Include(c => c.CoContactCompanies)
            //    .Include(p=>p.CoContactPhones)
            //    .Include(p=>p.CoContactEmails)                
            //    //.Include(c => c.CoContactSubtypes)
            //    .Include(c => c.CoContactTypes)
            //    .Include(c => c.SiteContactStatus)
            //    .OrderBy(p => p.ContactLastName).ThenBy(p => p.ContactFirstName).ThenBy(p=>p.CoContactCompanies.ContactCoName);
            CoQuickBooks cqb = db.CoQuickBooks.Where(p => p.SiteCoID == siteusercompanyid).FirstOrDefault();
            if (cqb != null)
            {
                ViewBag.IsConnected = true;
            }
            qbModels = new QBModels();
            //var q = coContacts.Where(p => p.ContactManager == siteuserid);
           // ViewBag.TotalRecords = q.Count();
            //ViewBag.CurrentPage = 1;
           // ViewBag.CurrentSort = "Last";
           // ViewBag.PageCount = Math.Ceiling(Convert.ToDouble(ViewBag.TotalRecords) / Convert.ToDouble(CommonCls.PageSize));
          //  List<CoContacts> listcontact = q.OrderBy(s=>s.ContactLastName).ToList();

           
            //foreach (var item in listcontact)
            //{
            //    if (item.CoContactCompanies == null)
            //    {
            //        item.CoContactCompanies = new CoContactCompanies();
            //    }
            //    if (item.CoContactTypes == null)
            //    {
            //        item.CoContactTypes = new CoContactTypes();
            //    }
            //    if (item.CoContactSubtypes == null)
            //    {
            //        item.CoContactSubtypes = new CoContactSubtypes();
            //    }

                

            //}
            //List<ContactList> Contact = new List<ContactList>();
            //Int64 Id = 0;
            //foreach (var C in listcontact)
            //{
            //    Id++;
            //    ContactList Contact1 = new ContactList();
            //    Contact1.Id = Id;
            //    Contact1.ContactFirstName = C.ContactFirstName;
            //    Contact1.ContactLastName = C.ContactLastName;
            //    Contact1.ContactCoName = C.CoContactCompanies.ContactCoName;
            //    Contact1.ContactTypeName = C.CoContactTypes.ContactTypeName;
            //    Contact1.SubtypeName = C.CoContactSubtypes.SubtypeName;
            //    Contact.Add(Contact1);
            //}

            //ViewBag.ContactsList = Contact;

            //qbModels.coContacts = listcontact;//coContacts.Where(p=>p.ContactManager==siteuserid).ToList().ForEach(s=>s.CoContactCompanies==null ? s.CoContactCompanies = new CoContactCompanies() : s.CoContactCompanies = s.CoContactCompanies);
            qbModels.SyncObjectsModel = new QBSyncdto();
            qbModels.OAuthorizationModel = new QBAuthorizationdto();
            qbModels.IsReadySync = false;
            var oAuthModel = new QBOAuthService(qbModels.OAuthorizationModel).IsTokenAvailable(this, siteCoID);
            if (oAuthModel.IsConnected)
            {
                qbModels.IsReadySync = true;
                qbModels.OAuthorizationModel = oAuthModel;
                qbModels.IsConnected = oAuthModel.IsConnected;
                var syncService = new QBSyncService(oAuthModel);
                qbModels.SyncObjectsModel.OauthToken = oAuthModel;
            //    qbModels.SyncObjectsModel = syncService.IsCustSync(qbModels.SyncObjectsModel, syncService, siteusercompanyid);
                qbModels.SyncObjectsModel.CompanyId = oAuthModel.Realmid;
                qbModels.SyncObjectsModel = Save(this, qbModels.SyncObjectsModel);
                qbModels.IsReadyTimeentry = qbModels.SyncObjectsModel.IsCustomerSync;
                qbModels.IsReadytoInvoice = false;
                
                 return View(qbModels);
                
            }
            else
            {
                 return View(qbModels);
            }
           
        }

        //[HttpPost]
        //public ActionResult Index(string LastName, string SelectedCompanies, string SelectedTypes, string SelectedSubTypes, bool? getall, int? page, string sort = "")
        //{
        //    string val = Request["SelectedCompanies"];
        //    ViewBag.Sorting = new string[] { "Last", "First", "Company" };
        //    //ViewBag.Company=new SelectList(db.GetContactCompaniesBySiteCoID(siteusercompanyid).ToList(),nameof(GetContactCompaniesBySiteCoID_Result.ViewID), nameof(GetContactCompaniesBySiteCoID_Result.Company));
        //    ViewBag.CompanyList = db.GetContactCompaniesBySiteCoID(siteusercompanyid).Where(s => s.Company != null && s.Company.Trim() != "").ToList();
        //    //ViewBag.ContactType = new SelectList(db.CoContactTypes.Where(p => p.SiteCoID == siteusercompanyid).OrderBy(p => p.ContactTypeOrder), "ContactTypeID", "ContactTypeName");
        //    ViewBag.ContactTypeList = db.CoContactTypes.Where(p => p.SiteCoID == siteusercompanyid).OrderBy(p => p.ContactTypeOrder).ToList();
        //    int siteCoID = base.siteusercompanyid;
        //    var coContacts = db.CoContacts
        //        .Where(p => p.SiteCoID == siteCoID)
        //        .Include(c => c.CoContactCompanies)
        //        .Include(p => p.CoContactPhones)
        //        .Include(p => p.CoContactEmails)
        //        //.Include(c => c.CoContactSubtypes)
        //        .Include(c => c.CoContactTypes)
        //        .Include(c => c.SiteContactStatus)
        //        .OrderBy(p => p.ContactLastName).ThenBy(p => p.ContactFirstName).ThenBy(p => p.CoContactCompanies.ContactCoName);
        //    CoQuickBooks cqb = db.CoQuickBooks.Where(p => p.SiteCoID == siteusercompanyid).FirstOrDefault();
        //    if (cqb != null)
        //    {
        //        ViewBag.IsConnected = true;
        //    }

        //    //var q = new List<CoContacts>();
        //    var q = coContacts.Where(p => p.ContactManager == siteuserid);
        //    if (getall == true)
        //        q = coContacts;
        //    if (LastName != null && LastName != "")
        //    {
        //        LastName = LastName.Trim().ToLower();
        //        q = q.Where(p => (p.ContactLastName == null ? "" : p.ContactLastName).ToLower().Contains(LastName) || (p.ContactFirstName == null ? "" : p.ContactFirstName).ToLower().Contains(LastName));
        //    }
        //    char[] sep = new char[] { ',' };
        //    if (SelectedCompanies != null && SelectedCompanies.Trim() != "")
        //    {
        //        int[] str_arr;
        //        str_arr = SelectedCompanies.Split(',').Select<string, int>(int.Parse).ToArray();
        //        q = q.Where(p => str_arr.Contains(p.ContactCoID.HasValue ? p.ContactCoID.Value : 0));
        //    }
        //    if (SelectedTypes != null && SelectedTypes.Trim() != "")
        //    {
        //        int[] str_arr;
        //        str_arr = SelectedTypes.Split(',').Select<string, int>(int.Parse).ToArray();
        //        q = q.Where(p => str_arr.Contains(p.ContactTypeID));
        //    }
        //    if (SelectedSubTypes != null && SelectedSubTypes.Trim() != "")
        //    {
        //        int[] str_arr;
        //        str_arr = SelectedSubTypes.Split(',').Select<string, int>(int.Parse).ToArray();
        //        q = q.Where(p => str_arr.Contains(p.ContactSubtypeID.HasValue ? p.ContactSubtypeID.Value : 0));
        //    }
        //    //if (Company != null && Company != 0)
        //    //    q = q.Where(p => p.ContactCoID == Company).ToList();
        //    //if (Type != null && Type != 0)
        //    //    q = q.Where(p => p.ContactTypeID == Type).ToList();
        //    int currentpage = page ?? 1;
        //    ViewBag.CurrentSort = sort == "" ? "Last" : sort;
        //    ViewBag.TotalRecords = q.Count();
        //    ViewBag.CurrentPage = currentpage;
        //    ViewBag.PageCount = Math.Ceiling(Convert.ToDouble(ViewBag.TotalRecords) / Convert.ToDouble(CommonCls.PageSize));
        //    switch (sort)
        //    {
        //        case "First":
        //            q = q.OrderBy(s => s.ContactFirstName);
        //            break;
        //        case "Company":
        //            q = q.OrderBy(s => s.CoContactCompanies != null ? s.CoContactCompanies.ContactCoName : "0");
        //            break;
        //        default:
        //            q = q.OrderBy(s => s.ContactLastName);
        //            break;
        //    }
        //    // List<CoContacts> listcontact = q.Skip((currentpage - 1) * CommonCls.PageSize).Take(CommonCls.PageSize).ToList();
        //    List<CoContacts> listcontact = q.ToList();
        //    foreach (var item in listcontact)
        //    {
        //        if (item.CoContactCompanies == null)
        //        {
        //            item.CoContactCompanies = new CoContactCompanies();
        //        }
        //        if (item.CoContactTypes == null)
        //        {
        //            item.CoContactTypes = new CoContactTypes();
        //        }
        //        if (item.CoContactSubtypes == null)
        //        {
        //            item.CoContactSubtypes = new CoContactSubtypes();
        //        }
        //    }

        //    List<ContactList> Contact = new List<ContactList>();
        //    Int64 Id = 0;
        //    foreach (var C in listcontact)
        //    {
        //        Id++;
        //        ContactList Contact1 = new ContactList();
        //        Contact1.Id = Id;
        //        Contact1.ContactFirstName = C.ContactFirstName;
        //        Contact1.ContactLastName = C.ContactLastName;
        //        Contact1.ContactCoName = C.CoContactCompanies.ContactCoName;
        //        Contact1.ContactTypeName = C.CoContactTypes.ContactTypeName;
        //        Contact1.SubtypeName = C.CoContactSubtypes.SubtypeName;
        //        Contact.Add(Contact1);
        //    }

        //    ViewBag.ContactsList = Contact;

        //    qbModels = new QBModels();
        //    qbModels.coContacts = listcontact;
        //    qbModels.SyncObjectsModel = new QBSyncdto();
        //    qbModels.OAuthorizationModel = new QBAuthorizationdto();
        //    qbModels.IsReadySync = false;
        //    var oAuthModel = new QBOAuthService(qbModels.OAuthorizationModel).IsTokenAvailable(this, siteCoID);
        //    if (oAuthModel.IsConnected)
        //    {
        //        qbModels.IsReadySync = true;
        //        qbModels.OAuthorizationModel = oAuthModel;
        //        qbModels.IsConnected = oAuthModel.IsConnected;
        //        var syncService = new QBSyncService(oAuthModel);
        //        qbModels.SyncObjectsModel.OauthToken = oAuthModel;
        //        //    qbModels.SyncObjectsModel = syncService.IsCustSync(qbModels.SyncObjectsModel, syncService, siteusercompanyid);
        //        qbModels.SyncObjectsModel.CompanyId = oAuthModel.Realmid;
        //        qbModels.SyncObjectsModel = Save(this, qbModels.SyncObjectsModel);
        //        qbModels.IsReadyTimeentry = qbModels.SyncObjectsModel.IsCustomerSync;
        //        qbModels.IsReadytoInvoice = false;
        //        return View(qbModels);
        //    }
        //    else
        //    {
        //        return View(qbModels);
        //    }
        //}
        public JsonResult getSubTypes(List<int> TypeIdArray)
        {
            if (db.CoContactSubtypes.Where(p => p.SiteCoID == siteusercompanyid && TypeIdArray.Contains(p.ContactTypeID == null ? 0 : p.ContactTypeID.Value)).FirstOrDefault() != null)
            {
                List<ContactTypes> SubTypes = db.CoContactSubtypes.Where(p => p.SiteCoID == siteusercompanyid && TypeIdArray.Contains(p.ContactTypeID == null ? 0 : p.ContactTypeID.Value)).OrderBy(p => p.ContactSubtypeOrder).Select(s=> new ContactTypes { ID=s.ContactSubtypeID, Name = s.SubtypeName }).ToList();
                return Json(SubTypes, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("");
            }
        }

        //[HttpPost]
        //public ActionResult Index(int? TypeID,long id)
        //{
        //    ViewBag.ContactType = new SelectList(db.CoContactTypes.Where(p => p.SiteCoID == siteusercompanyid).OrderBy(p => p.ContactTypeOrder), "ContactTypeID", "ContactTypeName");
        //    string type = db.CoContactTypes.Where(p => p.ContactTypeID == TypeID).Select(p => p.ContactTypeName).FirstOrDefault();
        //    try
        //    {
        //        QBAuthorizationdto oAuthDetails = new QBOAuthService(new QBAuthorizationdto()).GetAccessToken(this);
        //        if (type.ToUpper() == "VENDOR")
        //        {
        //            var dbvendorList = db.GetQbVendorsBySiteCoID(base.siteusercompanyid).ToList();                    
        //            syncService = new QBSyncService(oAuthDetails);
        //            syncObjects = id > 0 ? GetSyncObjects(this, id) : new QBSyncdto();
        //            syncObjects.OauthToken = oAuthDetails;
        //            syncObjects.CompanyId = oAuthDetails.Realmid;
        //            syncObjects = syncService.GetVendorsFromQB(this, syncObjects);
        //            if (syncObjects.VendorList.Count() > 0)
        //            {
        //                foreach (var q in syncObjects.VendorList.ToList())
        //                {
        //                    var existingvendor = dbvendorList.Where(p => p.Id == q.Id && p.FamilyName==q.FamilyName && p.GivenName==q.GivenName).FirstOrDefault();
        //                    if (existingvendor == null)
        //                    {
        //                        var exsitingfamilyname = dbvendorList.Where(p => p.FamilyName == q.FamilyName).FirstOrDefault();
        //                        if (exsitingfamilyname != null)
        //                        {
        //                            var exsitinggivingname = dbvendorList.Where(p => p.GivenName == q.GivenName).FirstOrDefault();
        //                            if (exsitinggivingname == null)
        //                            {
        //                                var contacts = GetDataForVendor(q, (int)TypeID);
        //                                int contactid = SaveContact(contacts, q.Id);
        //                            }
        //                        }
        //                        else
        //                        {
        //                            var contacts = GetDataForVendor(q, (int)TypeID);
        //                            int contactid = SaveContact(contacts, q.Id);
        //                        }
        //                    }

        //                }                      
        //            }
        //            if (!syncService.IsVendorSync(syncObjects, syncService, siteusercompanyid).IsVendorSync)
        //            {
        //                syncObjects = syncService.GetDatafromDBVendor(syncObjects, siteusercompanyid);
        //                if (syncObjects.VendorList.Count > 0)
        //                {
        //                    syncObjects = syncService.SyncVendor(this, syncObjects);
        //                }
        //                this.Save(this, syncObjects);                        
        //            }
        //            TempData["SyncSuccessMessage"] = "Synchronization Process Completed";
        //        }
        //        else if (type.ToUpper() == "CUSTOMER")
        //        {
        //            var dbcustomerList = db.GetQbCustomersBySiteCoID(base.siteusercompanyid).ToList();
        //            syncService = new QBSyncService(oAuthDetails);
        //            syncObjects = id > 0 ? GetSyncObjects(this, id) : new QBSyncdto();
        //            syncObjects.OauthToken = oAuthDetails;
        //            syncObjects.CompanyId = oAuthDetails.Realmid;
        //            syncObjects = syncService.GetCustomersFromQB(this, syncObjects);
        //            if (syncObjects.CustomerList.Count() > 0)
        //            {
        //                foreach (var q in syncObjects.CustomerList.ToList())
        //                {
        //                    var existingcustomer = dbcustomerList.Where(p => p.Id == q.Id).FirstOrDefault();
        //                    if (existingcustomer == null)
        //                    {
        //                        var exsitingfamilyname = dbcustomerList.Where(p => p.FamilyName == q.FamilyName).FirstOrDefault();
        //                        if (exsitingfamilyname != null)
        //                        {
        //                            var exsitinggivingname = dbcustomerList.Where(p => p.GivenName == q.GivenName).FirstOrDefault();
        //                            if (exsitinggivingname == null)
        //                            {
        //                                var contacts = GetDataForCustomers(q, (int)TypeID);
        //                                int contactid = SaveContact(contacts, q.Id);
        //                            }
        //                        }
        //                        else
        //                        {
        //                            var contacts = GetDataForCustomers(q, (int)TypeID);
        //                            int contactid = SaveContact(contacts, q.Id);
        //                        }
        //                    }

        //                }                      
        //            }
        //            if (!syncService.IsCustSync(syncObjects, syncService, siteusercompanyid).IsCustomerSync)
        //            {
        //                syncObjects = syncService.GetDatafromDBCustomer(syncObjects, siteusercompanyid);
        //                if (syncObjects.CustomerList.Count > 0)
        //                {
        //                    syncObjects = syncService.SyncCustomer(this, syncObjects);
        //                }
        //                this.Save(this, syncObjects);                       
        //            }
        //            TempData["SyncSuccessMessage"] = "Synchronization Process Completed";
        //        }
        //        return RedirectToAction("Index", "People", new { id = syncObjects.Id, isConnected = oAuthDetails.IsConnected });
        //    }
        //    catch (Exception ex)
        //    {
        //        return RedirectToAction("Index", "People");
        //        //throw ex;
        //    }            
        //}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
     
        internal QBSyncdto Save(object controller, QBSyncdto syncObjects)
        {
            peopleOptionsController = controller as Controller;
            Random random = new Random();
            syncObjects.Id = random.Next(1, 100);
            syncRepo.Add(syncObjects.Id, syncObjects);
            peopleOptionsController.TempData["Sync"] = syncRepo;
            peopleOptionsController.TempData.Keep();
            return syncObjects;
        }
        internal QBSyncdto GetSyncObjects(object controller, Int64 id)
        {
            peopleOptionsController = controller as System.Web.Mvc.Controller;
            Dictionary<Int64, QBSyncdto> syncRepo = peopleOptionsController.TempData["Sync"] as Dictionary<Int64, QBSyncdto>;
            peopleOptionsController.TempData.Keep();
            if (syncRepo.ContainsKey(id))
            {
                return syncRepo[id];
            }
            else
            {

                return syncRepo[syncRepo.First().Key];
            }
        }
        private int SaveContact(PeopleModels.NewContact Model,string AcctCustomerId)
        {
            int siteCoID = siteusercompanyid;

            CommonRepository repo = new CommonRepository();
            CoContactCompanies company = null;
            if (!string.IsNullOrWhiteSpace(Model.Company))
            {
                company = repo.AddCompany(siteCoID, Model.Company);
            }
            if (company == null)
            {
                company = new CoContactCompanies() { ContactCoID = 0 };
            }

            var coContact = new CoContacts()
            {
                ContactID = Model.ContactID ?? 0,
                SiteCoID = siteCoID,
                ContactTypeID = Model.TypeID ?? 0,
                ContactStatusID = Model.StatusID,
                ContactFirstName = Model.First,
                ContactLastName = Model.Last,
                ContactCoID = company.ContactCoID,
                ModifiedDate = DateTime.Now,
                ContactManager = Model.OwnerID,
                AcctCustomerID=AcctCustomerId
            };

            coContact.CoContactPhones.Add(new CoContactPhones() { PhoneTypeID = (int)EnumWrapper.PhoneTypes.Mobile, Phone = Model.Mobile, IsDefault = true });
            coContact.CoContactPhones.Add(new CoContactPhones() { PhoneTypeID = (int)EnumWrapper.PhoneTypes.Phone, Phone = Model.Phone, IsDefault = true });
            coContact.CoContactEmails.Add(new CoContactEmails() { EmailAddress = Model.Email, IsDefault = true });
            coContact.CoContactAddresses.Add(new CoContactAddresses()
            {
                Address1 = Model.Address1,
                Address2 = Model.Address2,
                City = Model.City,
                State = Model.State,
                Zip = Model.Zip,
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
        private PeopleModels.NewContact GetDataForVendor(Intuit.Ipp.Data.Vendor Model,int typeid)
        {
            int siteCoID = siteusercompanyid;
            var coContact = new PeopleModels.NewContact()
            {
                ContactID = 0,
                First = Model.GivenName,
                Last = Model.FamilyName,
                Company = Model.CompanyName,
                 Phone = Model.PrimaryPhone.FreeFormNumber,
                 Mobile = Model.PrimaryPhone.FreeFormNumber,
                 Email = Model.PrimaryEmailAddr.Address,
                Address1 = Model.BillAddr.Line1,
                Address2 = Model.BillAddr.Line2,
                City = Model.BillAddr.City,
                State = "",
                Zip = Model.BillAddr.PostalCode,
                CountryID = 0,
                TypeID = typeid,
                StatusID =(int?) Model.status,
                OwnerID=0
    };

            return coContact;
        }
        private PeopleModels.NewContact GetDataForCustomers(Intuit.Ipp.Data.Customer Model, int typeid)
        {
            int siteCoID = siteusercompanyid;
            var coContact = new PeopleModels.NewContact();
            coContact.ContactID = 0;
            coContact.First = Model.GivenName;
            coContact.Last = Model.FamilyName;
            coContact.Company = Model.CompanyName;
            coContact.Phone = Model.PrimaryPhone.FreeFormNumber;
            coContact.Mobile = Model.PrimaryPhone.FreeFormNumber;
            if (Model.PrimaryEmailAddr!=null)
            coContact.Email = Model.PrimaryEmailAddr.Address;
            coContact.Address1 = Model.BillAddr.Line1;
            coContact.Address2 = Model.BillAddr.Line2;
            coContact.City = Model.BillAddr.City;
            coContact.State = "";
            coContact.Zip = Model.BillAddr.PostalCode;
            coContact.CountryID = 0;
            coContact.TypeID = typeid;
            coContact.StatusID = (int?)Model.status;
            coContact.OwnerID = 0;           
            return coContact;
        }
        
        [HttpPost]
        public ActionResult GetContacts([DataSourceRequest]DataSourceRequest request,string LastName, string SelectedCompanies, string SelectedTypes, string  SelectedSubTypes, bool? getall)
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

            //var q = new List<CoContacts>();
            var q = coContacts.Where(p => p.ContactManager == siteuserid);
            if (getall == true)
                q = coContacts;
            if (LastName != null && LastName != "")
            {
                LastName = LastName.Trim().ToLower();
                q = q.Where(p => (p.ContactLastName == null ? "" : p.ContactLastName).ToLower().Contains(LastName) || (p.ContactFirstName == null ? "" : p.ContactFirstName).ToLower().Contains(LastName));
            }
            char[] sep = new char[] { ',' };
            if (SelectedCompanies != null && SelectedCompanies.Trim() != "")
            {
                int[] str_arr;
                str_arr = SelectedCompanies.Split(',').Select<string, int>(int.Parse).ToArray();
                q = q.Where(p => str_arr.Contains(p.ContactCoID.HasValue ? p.ContactCoID.Value : 0));
            }
            if (SelectedTypes != null && SelectedTypes.Trim() != "")
            {
                int[] str_arr;
                str_arr = SelectedTypes.Split(',').Select<string, int>(int.Parse).ToArray();
                q = q.Where(p => str_arr.Contains(p.ContactTypeID));
            }
            if (SelectedSubTypes != null && SelectedSubTypes.Trim() != "")
            {
                int[] str_arr;
                str_arr = SelectedSubTypes.Split(',').Select<string, int>(int.Parse).ToArray();
                q = q.Where(p => str_arr.Contains(p.ContactSubtypeID.HasValue ? p.ContactSubtypeID.Value : 0));
            }
            //if (Company != null && Company != 0)
            //    q = q.Where(p => p.ContactCoID == Company).ToList();
            //if (Type != null && Type != 0)
            //    q = q.Where(p => p.ContactTypeID == Type).ToList();
        
             //List<CoContacts> listcontact = q.Skip((currentpage - 1) * CommonCls.PageSize).Take(CommonCls.PageSize).ToList();
            List<CoContacts> listcontact = q.ToList();
            foreach (var item in listcontact)
            {
                if (item.CoContactCompanies == null)
                {
                    item.CoContactCompanies = new CoContactCompanies();
                }
                if (item.CoContactTypes == null)
                {
                    item.CoContactTypes = new CoContactTypes();
                }
                if (item.CoContactSubtypes == null)
                {
                    item.CoContactSubtypes = new CoContactSubtypes();
                }
            }


            qbModels = new QBModels();
            qbModels.coContacts = listcontact;
            qbModels.SyncObjectsModel = new QBSyncdto();
            qbModels.OAuthorizationModel = new QBAuthorizationdto();
            qbModels.IsReadySync = false;
            var oAuthModel = new QBOAuthService(qbModels.OAuthorizationModel).IsTokenAvailable(this, siteCoID);
            if (oAuthModel.IsConnected)
            {
                qbModels.IsReadySync = true;
                qbModels.OAuthorizationModel = oAuthModel;
                qbModels.IsConnected = oAuthModel.IsConnected;
                var syncService = new QBSyncService(oAuthModel);
                qbModels.SyncObjectsModel.OauthToken = oAuthModel;
                //    qbModels.SyncObjectsModel = syncService.IsCustSync(qbModels.SyncObjectsModel, syncService, siteusercompanyid);
                qbModels.SyncObjectsModel.CompanyId = oAuthModel.Realmid;
                qbModels.SyncObjectsModel = Save(this, qbModels.SyncObjectsModel);
                qbModels.IsReadyTimeentry = qbModels.SyncObjectsModel.IsCustomerSync;
                qbModels.IsReadytoInvoice = false;
               // return View(qbModels);


                List<ContactList> Contact = new List<ContactList>();
              
                foreach (var C in qbModels.coContacts)
                {
                  
                    ContactList Contact1 = new ContactList();
                   
                    Contact1.ContactID = C.ContactID;
                    Contact1.ContactFirstName = C.ContactFirstName;
                    Contact1.ContactLastName = C.ContactLastName;
                    Contact1.ContactCoName = C.CoContactCompanies.ContactCoName;
                    Contact1.ContactTypeName = C.CoContactTypes.ContactTypeName;
                    Contact1.SubtypeName = C.CoContactSubtypes.SubtypeName;
                    Contact.Add(Contact1);
                }


                DataSourceResult result = Contact.ToDataSourceResult(request);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {

                List<ContactList> Contact = new List<ContactList>();
               
                foreach (var C in qbModels.coContacts)
                {
                    ContactList Contact1 = new ContactList();
                    Contact1.ContactID = C.ContactID;
                    Contact1.ContactFirstName = C.ContactFirstName;
                    Contact1.ContactLastName = C.ContactLastName;
                    Contact1.ContactCoName = C.CoContactCompanies.ContactCoName;
                    Contact1.ContactTypeName = C.CoContactTypes.ContactTypeName;
                    Contact1.SubtypeName = C.CoContactSubtypes.SubtypeName;
                    Contact.Add(Contact1);
                }
                
                DataSourceResult result = Contact.ToDataSourceResult(request);
                return Json(result, JsonRequestBehavior.AllowGet);
                //return View(qbModels);
            }
        }
      
        //public ActionResult Index()
        //{
        //    ViewBag.Sorting = new string[] { "Last", "First", "Company" };
        //    //ViewBag.Company=new SelectList(db.GetContactCompaniesBySiteCoID(siteusercompanyid).ToList(),nameof(GetContactCompaniesBySiteCoID_Result.ViewID), nameof(GetContactCompaniesBySiteCoID_Result.Company));
        //    ViewBag.CompanyList = db.GetContactCompaniesBySiteCoID(siteusercompanyid).Where(s => s.Company != null && s.Company.Trim() != "").ToList();
        //    //ViewBag.ContactType = new SelectList(db.CoContactTypes.Where(p => p.SiteCoID == siteusercompanyid).OrderBy(p => p.ContactTypeOrder), "ContactTypeID", "ContactTypeName");
        //    ViewBag.ContactTypeList = db.CoContactTypes.Where(p => p.SiteCoID == siteusercompanyid).OrderBy(p => p.ContactTypeOrder).ToList();
        //    int siteCoID = base.siteusercompanyid;
        //    var coContacts = db.CoContacts
        //        .Where(p => p.SiteCoID == siteCoID)
        //        .Include(c => c.CoContactCompanies)
        //        .Include(p => p.CoContactPhones)
        //        .Include(p => p.CoContactEmails)
        //        //.Include(c => c.CoContactSubtypes)
        //        .Include(c => c.CoContactTypes)
        //        .Include(c => c.SiteContactStatus)
        //        .OrderBy(p => p.ContactLastName).ThenBy(p => p.ContactFirstName).ThenBy(p => p.CoContactCompanies.ContactCoName);
        //    CoQuickBooks cqb = db.CoQuickBooks.Where(p => p.SiteCoID == siteusercompanyid).FirstOrDefault();
        //    if (cqb != null)
        //    {
        //        ViewBag.IsConnected = true;
        //    }
        //    qbModels = new QBModels();
        //    var q = coContacts.Where(p => p.ContactManager == siteuserid);
        //    ViewBag.TotalRecords = q.Count();
        //    ViewBag.CurrentPage = 1;
        //    ViewBag.CurrentSort = "Last";
        //    ViewBag.PageCount = Math.Ceiling(Convert.ToDouble(ViewBag.TotalRecords) / Convert.ToDouble(CommonCls.PageSize));
        //    List<CoContacts> listcontact = q.OrderBy(s => s.ContactLastName).Take(CommonCls.PageSize).ToList();


        //    foreach (var item in listcontact)
        //    {
        //        if (item.CoContactCompanies == null)
        //        {
        //            item.CoContactCompanies = new CoContactCompanies();
        //        }
        //        if (item.CoContactTypes == null)
        //        {
        //            item.CoContactTypes = new CoContactTypes();
        //        }
        //        if (item.CoContactSubtypes == null)
        //        {
        //            item.CoContactSubtypes = new CoContactSubtypes();
        //        }



        //    }
        //    List<ContactList> Contact = new List<ContactList>();
        //    Int64 Id = 0;
        //    foreach (var C in listcontact)
        //    {
        //        Id++;
        //        ContactList Contact1 = new ContactList();
        //        Contact1.Id = Id;
        //        Contact1.ContactFirstName = C.ContactFirstName;
        //        Contact1.ContactLastName = C.ContactLastName;
        //        Contact1.ContactCoName = C.CoContactCompanies.ContactCoName;
        //        Contact1.ContactTypeName = C.CoContactTypes.ContactTypeName;
        //        Contact1.SubtypeName = C.CoContactSubtypes.SubtypeName;
        //        Contact.Add(Contact1);
        //    }
        //    ViewBag.ContactsList = Contact;

        //    qbModels.coContacts = listcontact;//coContacts.Where(p=>p.ContactManager==siteuserid).ToList().ForEach(s=>s.CoContactCompanies==null ? s.CoContactCompanies = new CoContactCompanies() : s.CoContactCompanies = s.CoContactCompanies);
        //    qbModels.SyncObjectsModel = new QBSyncdto();
        //    qbModels.OAuthorizationModel = new QBAuthorizationdto();
        //    qbModels.IsReadySync = false;
        //    var oAuthModel = new QBOAuthService(qbModels.OAuthorizationModel).IsTokenAvailable(this, siteCoID);
        //    if (oAuthModel.IsConnected)
        //    {
        //        qbModels.IsReadySync = true;
        //        qbModels.OAuthorizationModel = oAuthModel;
        //        qbModels.IsConnected = oAuthModel.IsConnected;
        //        var syncService = new QBSyncService(oAuthModel);
        //        qbModels.SyncObjectsModel.OauthToken = oAuthModel;
        //        //    qbModels.SyncObjectsModel = syncService.IsCustSync(qbModels.SyncObjectsModel, syncService, siteusercompanyid);
        //        qbModels.SyncObjectsModel.CompanyId = oAuthModel.Realmid;
        //        qbModels.SyncObjectsModel = Save(this, qbModels.SyncObjectsModel);
        //        qbModels.IsReadyTimeentry = qbModels.SyncObjectsModel.IsCustomerSync;
        //        qbModels.IsReadytoInvoice = false;

        //        return View(qbModels);

        //    }
        //    else
        //    {

        //        return View(qbModels);

        //    }

        //}
    }



}
