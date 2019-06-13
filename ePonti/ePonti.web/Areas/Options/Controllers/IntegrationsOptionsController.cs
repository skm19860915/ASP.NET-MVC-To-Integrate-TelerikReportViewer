using Dropbox.Api;
using ePonti.BOL;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;
using System.Web.Mvc;
using ePonti.web.Models;
using System.Net;
using DevDefined.OAuth.Consumer;
using DevDefined.OAuth.Framework;
using System.IO;
using System.Collections.Generic;
using System.Data.Entity;
using ePonti.BLL.Common;
using ePonti.BOL.Repository;
using Newtonsoft.Json;
using QbSync.QbXml;
using QbSync.WebConnector;
using QbSync.WebConnector.Synchronous;
using Moq;
using Moq.Protected;

namespace ePonti.web.Areas.Options.Controllers
{
    [Authorize]
    public class IntegrationsOptionsController : ePonti.web.Controllers._baseMVCController
    {
        ePontiv2Entities db = new ePontiv2Entities();
        QBAuthorizationdto qbAuthorizationdto = null;
        QBOAuthService qbOAuthService = null;
        Controller integrationsOptionsController = null;
        Dictionary<Int64, QBSyncdto> syncRepo = null;
        QBSyncService syncService = null;
        QBSyncdto syncObjects = null;
        QBModels qbModels = null;
        STServices sTService = null;
        public IntegrationsOptionsController()
        {
            syncRepo = new Dictionary<Int64, QBSyncdto>();
        }
        public ActionResult Index()
        {
            if (TempData["SyncSuccessMessage"] != null)
            {
                ViewBag.SyncSuccessMessage = TempData["SyncSuccessMessage"];
                TempData.Remove("SyncSuccessMessage");
            }
            sTService = new STServices();
            ViewBag.IsDBConnected = false;
            ViewBag.IsQBConnected = false;
            ViewBag.IsSTConnected = sTService.IsConnected(siteuserid);

            CoDropbox drpconnection = db.CoDropbox.Where(p => p.SiteCoID == siteusercompanyid).FirstOrDefault();
            if (drpconnection != null)
                ViewBag.IsDBConnected = true;
            CoQuickBooks currentIndex = db.CoQuickBooks.Where(p => p.SiteCoID == siteusercompanyid).FirstOrDefault();
            if (currentIndex != null)
                ViewBag.IsQBConnected = true;
            ViewBag.ContactType = new SelectList(db.CoContactTypes.Where(p => p.SiteCoID == siteusercompanyid).OrderBy(p => p.ContactTypeOrder), "ContactTypeID", "ContactTypeName");
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
            qbModels = new QBModels();
            qbModels.coContacts = coContacts.ToList();
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
        public ActionResult STRegisterUser()
        {
            sTService = new STServices();
            string json = "{\"userId\": \"bill@eponti.com\", \"password\": \"@925Stream\", \"licenseKey\": \"JSWYAV-NZRKVF-IAFFVX-PTRYCO\"}";
            sTService.RegisterUser(json);
            return View();
        }
        public ActionResult STAuthorizeUser()
        {
            sTService = new STServices();
            string json = "{\"userId \": \"gxxmmekkckyx\", \"password\": \"gnjspibz\"}";
            string authKey = sTService.GetAuthorizationKey(json);
            return View();
        }
        [HttpPost]
        public ActionResult Index(int? TypeID, long id)
        {
            ViewBag.ContactType = new SelectList(db.CoContactTypes.Where(p => p.SiteCoID == siteusercompanyid).OrderBy(p => p.ContactTypeOrder), "ContactTypeID", "ContactTypeName");
            string type = db.CoContactTypes.Where(p => p.ContactTypeID == TypeID).Select(p => p.ContactTypeName).FirstOrDefault();
            try
            {
                QBAuthorizationdto oAuthDetails = new QBOAuthService(new QBAuthorizationdto()).GetAccessToken(this);
                if (type.ToUpper() == "VENDOR")
                {
                    var dbvendorList = db.GetQbVendorsBySiteCoID(base.siteusercompanyid).ToList();
                    syncService = new QBSyncService(oAuthDetails);
                    syncObjects = id > 0 ? GetSyncObjects(this, id) : new QBSyncdto();
                    syncObjects.OauthToken = oAuthDetails;
                    syncObjects.CompanyId = oAuthDetails.Realmid;
                    syncObjects = syncService.GetVendorsFromQB(this, syncObjects);
                    if (syncObjects.VendorList.Count() > 0)
                    {
                        foreach (var q in syncObjects.VendorList.ToList())
                        {
                            var existingvendor = dbvendorList.Where(p => p.Id == q.Id && p.FamilyName == q.FamilyName && p.GivenName == q.GivenName).FirstOrDefault();
                            if (existingvendor == null)
                            {
                                var exsitingfamilyname = dbvendorList.Where(p => p.FamilyName == q.FamilyName).FirstOrDefault();
                                if (exsitingfamilyname != null)
                                {
                                    var exsitinggivingname = dbvendorList.Where(p => p.GivenName == q.GivenName).FirstOrDefault();
                                    if (exsitinggivingname == null)
                                    {
                                        var contacts = GetDataForVendor(q, (int)TypeID);
                                        int contactid = SaveContact(contacts, q.Id);
                                    }
                                }
                                else
                                {
                                    var contacts = GetDataForVendor(q, (int)TypeID);
                                    int contactid = SaveContact(contacts, q.Id);
                                }
                            }

                        }
                    }
                    if (!syncService.IsVendorSync(syncObjects, syncService, siteusercompanyid).IsVendorSync)
                    {
                        syncObjects = syncService.GetDatafromDBVendor(syncObjects, siteusercompanyid);
                        if (syncObjects.VendorList.Count > 0)
                        {
                            syncObjects = syncService.SyncVendor(this, syncObjects);
                        }
                        this.Save(this, syncObjects);
                    }
                    TempData["SyncSuccessMessage"] = "Synchronization Process Completed";
                }
                else if (type.ToUpper() == "CUSTOMER")
                {
                    var dbcustomerList = db.GetQbCustomersBySiteCoID(base.siteusercompanyid).ToList();
                    syncService = new QBSyncService(oAuthDetails);
                    syncObjects = id > 0 ? GetSyncObjects(this, id) : new QBSyncdto();
                    syncObjects.OauthToken = oAuthDetails;
                    syncObjects.CompanyId = oAuthDetails.Realmid;
                    syncObjects = syncService.GetCustomersFromQB(this, syncObjects);
                    if (syncObjects.CustomerList.Count() > 0)
                    {
                        foreach (var q in syncObjects.CustomerList.ToList())
                        {
                            var existingcustomer = dbcustomerList.Where(p => p.Id == q.Id).FirstOrDefault();
                            if (existingcustomer == null)
                            {
                                var exsitingfamilyname = dbcustomerList.Where(p => p.FamilyName == q.FamilyName).FirstOrDefault();
                                if (exsitingfamilyname != null)
                                {
                                    var exsitinggivingname = dbcustomerList.Where(p => p.GivenName == q.GivenName).FirstOrDefault();
                                    if (exsitinggivingname == null)
                                    {
                                        var contacts = GetDataForCustomers(q, (int)TypeID);
                                        int contactid = SaveContact(contacts, q.Id);
                                    }
                                }
                                else
                                {
                                    var contacts = GetDataForCustomers(q, (int)TypeID);
                                    int contactid = SaveContact(contacts, q.Id);
                                }
                            }

                        }
                    }
                    if (!syncService.IsCustSync(syncObjects, syncService, siteusercompanyid).IsCustomerSync)
                    {
                        syncObjects = syncService.GetDatafromDBCustomer(syncObjects, siteusercompanyid);
                        if (syncObjects.CustomerList.Count > 0)
                        {
                            syncObjects = syncService.SyncCustomer(this, syncObjects);
                        }
                        this.Save(this, syncObjects);
                    }
                    TempData["SyncSuccessMessage"] = "Synchronization Process Completed";
                }
                return RedirectToAction("Index", "IntegrationsOptions", new { id = syncObjects.Id, isConnected = oAuthDetails.IsConnected });
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "IntegrationsOptions");
                //throw ex;
            }
        }

        string appKey = WebConfigurationManager.AppSettings["DropboxAppKey"];
        string appSecret = WebConfigurationManager.AppSettings["DropboxAppSecret"];
        string qbappKey = WebConfigurationManager.AppSettings["QBAppKey"];
        string qbappSecret = WebConfigurationManager.AppSettings["QBAppSecret"];
        string skey = WebConfigurationManager.AppSettings["securityKey"];
        static string AuthorizeUrl = WebConfigurationManager.AppSettings["AuthorizeUrl"];
        static string GET_REQUEST_TOKEN = WebConfigurationManager.AppSettings["GET_REQUEST_TOKEN"];
        static string GET_ACCESS_TOKEN = WebConfigurationManager.AppSettings["GET_ACCESS_TOKEN"];
        private string RedirectUri
        {
            get
            {
                if (this.Request.Url.Host.ToLowerInvariant() == "localhost")
                {
                    return string.Format("https://{0}:{1}/Options/IntegrationsOptions/Auth", this.Request.Url.Host, this.Request.Url.Port);
                }

                var builder = new UriBuilder(
                    Uri.UriSchemeHttps,
                    this.Request.Url.Host);

                builder.Path = "/Options/IntegrationsOptions/Auth";

                return builder.ToString();
            }
        }
        public ActionResult Connect()
        {
            var redirect = DropboxOAuth2Helper.GetAuthorizeUri(
                OAuthResponseType.Code,
                appKey,
                RedirectUri,
                null);
            return Redirect(redirect.ToString());
        }
        public ActionResult DBDisconnect()
        {
            CoDropbox cdb = db.CoDropbox.Where(p => p.SiteCoID == siteusercompanyid).FirstOrDefault();
            if (cdb != null)
            {
                db.CoDropbox.Remove(cdb);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        // GET: /IntegratedOptions/Auth      
        public async Task<ActionResult> Auth(string code, string state)
        {
            try
            {
                var response = await DropboxOAuth2Helper.ProcessCodeFlowAsync(
                    code,
                    appKey,
                    appSecret,
                    RedirectUri);
                using (var dbx = new DropboxClient(response.AccessToken))
                {
                    var full = await dbx.Users.GetCurrentAccountAsync();
                    var list = await dbx.Files.ListFolderAsync(string.Empty);

                    var checkexisting = db.CoDropbox.Where(p => p.SiteCoID == siteusercompanyid).FirstOrDefault();
                    if (checkexisting == null)
                    {
                        db.InsertDropbox(base.siteusercompanyid, appKey, appSecret, Encoding.ASCII.GetBytes(response.AccessToken), full.Email, "", DateTime.UtcNow.Date, RedirectUri);
                        db.SaveChanges();
                    }

                }
                return RedirectToAction("Index");

            }
            catch (Exception e)
            {
                var message = string.Format(
                    "code: {0}\nAppKey: {1}\nAppSecret: {2}\nRedirectUri: {3}\nException : {4}",
                    code,
                   appKey,
                    appSecret,
                    this.RedirectUri,
                    e);
                return RedirectToAction("error");
            }
        }
        public ActionResult QBConnect()
        {
            qbAuthorizationdto = new QBAuthorizationdto();
            qbOAuthService = new QBOAuthService(qbAuthorizationdto);
            qbAuthorizationdto.CallBackUrl = Request.Url.GetLeftPart(UriPartial.Authority) + "/Options/IntegrationsOptions/QBAuth";
            return Redirect(qbOAuthService.GrantUrl(this));
        }
        public ActionResult QBDisconnect()
        {
            CoQuickBooks cqb = db.CoQuickBooks.Where(p => p.SiteCoID == siteusercompanyid).FirstOrDefault();
            if (cqb != null)
            {
                string testAccesToken = Utility.Decrypt(Encoding.UTF8.GetString(cqb.AccessToken), skey);
                string st = DisconnectRealm(qbappKey, qbappSecret, testAccesToken, cqb.apiSecret);
                db.CoQuickBooks.Remove(cqb);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        // GET: /IntegratedOptions/QBAuth      
        public ActionResult QBAuth()
        {
            try
            {
                qbAuthorizationdto = new QBAuthorizationdto();
                qbOAuthService = new QBOAuthService(qbAuthorizationdto);
                qbAuthorizationdto = qbOAuthService.GetRequestToken(this);
                if (Request.QueryString.HasKeys())
                {
                    qbAuthorizationdto.OauthVerifyer = Request.QueryString["oauth_verifier"].ToString();
                    qbAuthorizationdto.Realmid = Request.QueryString["realmId"].ToString();
                    qbAuthorizationdto.DataSource = Request.QueryString["dataSource"].ToString();
                    qbAuthorizationdto = qbOAuthService.GetAccessTokenFromServer(this, qbAuthorizationdto);
                    string access_secret = Utility.Encrypt(qbAuthorizationdto.AccessTokenSecret, qbAuthorizationdto.SecurityKey);
                    string access_token = Utility.Encrypt(qbAuthorizationdto.AccessToken, qbAuthorizationdto.SecurityKey);
                    db.InsertQuickBooks(base.siteusercompanyid, "", qbAuthorizationdto.AccessTokenSecret, Encoding.ASCII.GetBytes(access_token), "", "", DateTime.UtcNow, qbAuthorizationdto.CallBackUrl, qbAuthorizationdto.Realmid, qbAuthorizationdto.DataSource);
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                var message = string.Format(
                    "nAppKey: {0}\nAppSecret: {1}\nRedirectUri: {2}\nException : {3}",

                   appKey,
                    appSecret,
                    this.RedirectUri,
                    e);
                return RedirectToAction("error");
            }
        }
        public static string DisconnectRealm(string consumerKey, string consumerSecret, string accessToken, string accessTokenSecret)
        {
            HttpWebRequest httpWebRequest = WebRequest.Create("https://appcenter.intuit.com/api/v1/Connection/Disconnect") as HttpWebRequest;
            httpWebRequest.Method = "GET";
            httpWebRequest.Headers.Add("Authorization", GetDevDefinedOAuthHeader(httpWebRequest, consumerKey, consumerSecret, accessToken, accessTokenSecret));
            UTF8Encoding encoding = new UTF8Encoding();
            HttpWebResponse httpWebResponse = httpWebRequest.GetResponse() as HttpWebResponse;
            using (Stream data = httpWebResponse.GetResponseStream())
            {
                //return XML response
                return new StreamReader(data).ReadToEnd();
            }
        }
        static string GetDevDefinedOAuthHeader(HttpWebRequest webRequest, string consumerKey, string consumerSecret, string accessToken, string accessTokenSecret)
        {
            OAuthConsumerContext consumerContext = new OAuthConsumerContext
            {
                ConsumerKey = consumerKey,
                ConsumerSecret = consumerSecret,
                SignatureMethod = SignatureMethod.HmacSha1,
                UseHeaderForOAuthParameters = true
            };
            consumerContext.UseHeaderForOAuthParameters = true;
            OAuthSession oSession = new OAuthSession(consumerContext, GET_REQUEST_TOKEN, AuthorizeUrl, GET_ACCESS_TOKEN);
            oSession.AccessToken = new TokenBase
            {
                Token = accessToken,
                ConsumerKey = consumerKey,
                TokenSecret = accessTokenSecret
            };
            IConsumerRequest consumerRequest = oSession.Request();
            consumerRequest = ConsumerRequestExtensions.ForMethod(consumerRequest, webRequest.Method);
            consumerRequest = ConsumerRequestExtensions.ForUri(consumerRequest, webRequest.RequestUri);
            consumerRequest = consumerRequest.SignWithToken();
            return consumerRequest.Context.GenerateOAuthParametersForHeader();
        }
        internal QBSyncdto Save(object controller, QBSyncdto syncObjects)
        {
            integrationsOptionsController = controller as Controller;
            Random random = new Random();
            syncObjects.Id = random.Next(1, 100);
            syncRepo.Add(syncObjects.Id, syncObjects);
            integrationsOptionsController.TempData["Sync"] = syncRepo;
            integrationsOptionsController.TempData.Keep();
            return syncObjects;
        }
        internal QBSyncdto GetSyncObjects(object controller, Int64 id)
        {
            integrationsOptionsController = controller as System.Web.Mvc.Controller;
            Dictionary<Int64, QBSyncdto> syncRepo = integrationsOptionsController.TempData["Sync"] as Dictionary<Int64, QBSyncdto>;
            integrationsOptionsController.TempData.Keep();
            if (syncRepo.ContainsKey(id))
            {
                return syncRepo[id];
            }
            else
            {

                return syncRepo[syncRepo.First().Key];
            }
        }
        private int SaveContact(PeopleModels.NewContact Model, string AcctCustomerId)
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
                ContactManager = siteuserid,
                AcctCustomerID = AcctCustomerId
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
        private PeopleModels.NewContact GetDataForVendor(Intuit.Ipp.Data.Vendor Model, int typeid)
        {
            int siteCoID = siteusercompanyid;
            var coContact = new PeopleModels.NewContact();
            coContact.ContactID = 0;
            coContact.First = Model.GivenName;
            coContact.Last = Model.FamilyName;
            coContact.Company = Model.CompanyName;
            if (Model.PrimaryPhone != null)
            {
                coContact.Phone = Model.PrimaryPhone.FreeFormNumber;
                coContact.Mobile = Model.PrimaryPhone.FreeFormNumber;
            }
            if (Model.PrimaryEmailAddr != null)
                coContact.Email = Model.PrimaryEmailAddr.Address;
            if (Model.BillAddr != null)
            {
                coContact.Address1 = Model.BillAddr.Line1;
                coContact.Address2 = Model.BillAddr.Line2;
                coContact.City = Model.BillAddr.City;
                coContact.Zip = Model.BillAddr.PostalCode;
            }
            coContact.State = "";
            coContact.CountryID = 0;
            coContact.TypeID = typeid;
            coContact.StatusID = (int?)Model.status;
            coContact.OwnerID = 0;


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
            if (Model.PrimaryPhone != null)
            {
                coContact.Phone = Model.PrimaryPhone.FreeFormNumber;
                coContact.Mobile = Model.PrimaryPhone.FreeFormNumber;
            }
            if (Model.PrimaryEmailAddr != null)
                coContact.Email = Model.PrimaryEmailAddr.Address;
            if (Model.BillAddr != null)
            {
                coContact.Address1 = Model.BillAddr.Line1;
                coContact.Address2 = Model.BillAddr.Line2;
                coContact.City = Model.BillAddr.City;
                coContact.Zip = Model.BillAddr.PostalCode;
            }
            coContact.State = "";
            coContact.CountryID = 0;
            coContact.TypeID = typeid;
            coContact.StatusID = (int?)Model.status;
            coContact.OwnerID = 0;
            return coContact;
        }

        #region Salez Toolz
        public ActionResult STConnect(string auth)
        {
            auth = auth.Replace(" ", "+");
            CommonRepository repo = new CommonRepository();
            sTService = new STServices();
            string JSON = string.Empty;

            string masterKey = WebConfigurationManager.AppSettings["SalezToolzMasterKey"];
            string ClientSecret = WebConfigurationManager.AppSettings["SalezToolzClientSecret"];  // sTService.GetClientSecret(masterKey);

            string RealmID = string.Empty;
            string OAuthAccessToken = string.Empty;
            string OAuthAccessTokenSecret = string.Empty;
            string RedirectUrl = string.Empty;

            string AuthorizationKey = auth;
            int siteCoID = siteusercompanyid;
            string DisplayName = repo.GetUserDisplayName(siteuserid);
            string SalezToolzUserID = Convert.ToString(siteuserid);
            var RD = new
            {
                masterKey = masterKey,
                clientSecret = ClientSecret,
                authorizationKey = AuthorizationKey
            };
            JSON = JsonConvert.SerializeObject(RD);
            Token AccessToken = sTService.GetAccessToken(JSON);
            if (AccessToken != null)
            {
                OAuthAccessToken = AccessToken.AccessToken;
                OAuthAccessTokenSecret = AccessToken.RefreshToken;
                // Inserting credential to Database.
                db.InsertSalezToolzOauth(siteCoID, SalezToolzUserID, DisplayName, null, OAuthAccessToken, OAuthAccessTokenSecret, DateTime.Now, RedirectUrl);
            }

            return RedirectToAction("index");
        }
        public ActionResult STDisconnect()
        {
            CoSalezToolz cqb = db.CoSalezToolz.Where(p => p.SalezToolzUserID == siteuserid.ToString()).FirstOrDefault();
            if (cqb != null)
            {
                db.CoSalezToolz.Remove(cqb);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        #endregion
        #region QB desktop

        public ActionResult QBDesktopConnect()
        {
            //string access_secret = "QBDesktop";
            //string access_token = "QBDesktop";
            Mock<IAuthenticator> authenticatorMock = new Mock<IAuthenticator>();
            var syncManagerMock = new Mock<QbManager>(authenticatorMock.Object);
            syncManagerMock
                .Protected()
                .Setup("SaveChanges");
            syncManagerMock
                .Protected()
                .Setup<int>("GetWaitTime", ItExpr.IsAny<AuthenticatedTicket>())
                .Returns(0);

            syncManagerMock.CallBase = true;
            var result = syncManagerMock.Object.Authenticate("user", "password");

            //Assert.IsNotEmpty(result[0]);
            //Assert.IsEmpty(result[1]);
            //Assert.IsEmpty(result[2]);
            //Assert.IsEmpty(result[3]);
            syncManagerMock
                .Protected()
                .Verify("SaveChanges", Times.Once());

            //QbSync.WebConnector.Synchronous.QbConnector qbConnector = new QbSync.WebConnector.Synchronous.QbConnector();
            //QbSync.WebConnector.Synchronous.QbManager qbManager = new QbSync.WebConnector.Synchronous.QbManager();
            //string[] str1 = qbManager.Authenticate("test", "test");
            //string[] str = qbConnector.authenticate("test", "test");

            //db.InsertQuickBooks(base.siteusercompanyid, "", access_secret, Encoding.ASCII.GetBytes(access_token), "", "", DateTime.UtcNow, "", "", "");
            //db.SaveChanges();
            return View();//Redirect(qbOAuthService.GrantUrl(this));
        }
        public ActionResult QBDesktopDisconnect()
        {
            CoQuickBooks cqb = db.CoQuickBooks.Where(p => p.SiteCoID == siteusercompanyid).FirstOrDefault();
            if (cqb != null)
            {
                string testAccesToken = Utility.Decrypt(Encoding.UTF8.GetString(cqb.AccessToken), skey);
                string st = DisconnectRealm(qbappKey, qbappSecret, testAccesToken, cqb.apiSecret);
                db.CoQuickBooks.Remove(cqb);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        #endregion
    }
}