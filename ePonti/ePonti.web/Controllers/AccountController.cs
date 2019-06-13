using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using ePonti.web.Models;
using ePonti.BOL;
using ePonti.BOL.Repository;
using System.Transactions;
using ePonti.BLL.Common;

namespace ePonti.web.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: false);
            switch (result)
            {
                case SignInStatus.Success:
                    {
                        string id="";
                        int SiteUserId = 0,license=0;
                        using (ePontiv2Entities db = new ePontiv2Entities())
                        {
                            id = db.AspNetUsers.Where(p => p.UserName == model.Email).Select(p => p.Id).FirstOrDefault();
                            SiteUserId = db.SiteUsers.Where(p => p.ASPNetUserID == id).Select(p => p.SiteUserID).FirstOrDefault();
                            license = (int)db.SiteLicenseOrdered.Where(p => p.SiteUserID == SiteUserId).Select(p => p.LicenseID).FirstOrDefault();
                        }
                        if (license == 1)
                            Session["IsAdmin"] = true;
                        else
                            Session["IsAdmin"] = false;
                        if (returnUrl != null && returnUrl.Trim() != "")
                        {
                            return RedirectToLocal(returnUrl);
                        }
                        else
                        {
                            return RedirectToAction("Index", "Dashboard", new { area = "Sections" });
                        }
                    }
                //return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(model);
            }
        }
        //
        // GET: /Account/VerifyCode
        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
        {
            // Require that the user has already logged in via username/password or external login
            if (!await SignInManager.HasBeenVerifiedAsync())
            {
                return View("Error");
            }
            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/VerifyCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // The following code protects for brute force attacks against the two factor codes. 
            // If a user enters incorrect codes for a specified amount of time then the user account 
            // will be locked out for a specified amount of time. 
            // You can configure the account lockout settings in IdentityConfig
            var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent: model.RememberMe, rememberBrowser: model.RememberBrowser);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(model.ReturnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid code.");
                    return View(model);
            }
        }

        [AllowAnonymous]
        public ActionResult Register(string invitation = "")
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Dashboard", new { area = "Sections" });
            }

            RegisterViewModel model = new RegisterViewModel();
            //string regnote = "Create a new account.";

            if (!string.IsNullOrWhiteSpace(invitation))
            {
                model.InvitationCode = invitation;

                using (ePontiv2Entities db = new ePontiv2Entities())
                {
                    var aspNetUser = db.AspNetUsers.Where(p => p.InvitationCode == invitation && p.InvitationAccepted != true).FirstOrDefault();
                    if (aspNetUser == null)
                    {
                        model.InvitationCodeValid = false;
                        return View(model);
                    }

                    var siteUser = db.SiteUsers.Where(p => p.ASPNetUserID == aspNetUser.Id).FirstOrDefault();
                    if (siteUser == null)
                    {
                        model.InvitationCodeValid = false;
                        return View(model);
                    }

                    var company = db.SiteCompanies.Where(p => p.SiteCoID == siteUser.SiteCoID).FirstOrDefault();
                    if (company == null)
                    {
                        model.InvitationCodeValid = false;
                        return View(model);
                    }

                    model.InvitationCodeValid = true;
                    model.Email = aspNetUser.Email;
                    model.Phone = aspNetUser.PhoneNumber;
                    model.FirstName = siteUser.UserFirstName;
                    model.LastName = siteUser.UserLastName;
                    model.CompanyDetails.CompanyID = company.SiteCoID;
                    model.CompanyDetails.CompanyName = company.CoName;
                }
            }

            model.CompanyDetails.Country = "USA";

            return View(model);
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel Model)
        {
            var isAdmin = string.IsNullOrWhiteSpace(Model.InvitationCode);

            //if it is invited user, then company name will be null. Fill it to avoid false model error
            if (!isAdmin)
            {
                Model.CompanyDetails.CompanyName = "Company Name";
            }

            if (ModelState.IsValid)
            {
                AspNetUsers aspNetUser = new AspNetUsers();
                SiteUsers siteUser = new SiteUsers();
                SiteCompanies company = new SiteCompanies();
                bool? invitationAccepted = null;

                using (ePontiv2Entities db = new ePontiv2Entities())
                using (TransactionScope tran = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    CommonRepository repo = new CommonRepository(db);

                    //Site Company
                    if (!isAdmin)
                    {
                        aspNetUser = db.AspNetUsers.AsNoTracking().Where(p => p.InvitationCode == Model.InvitationCode && p.InvitationAccepted != true).FirstOrDefault();
                        if (aspNetUser == null)
                        {
                            ModelState.AddModelError("", "Invitation code invalid or expired.");
                            return View(Model);
                        }

                        siteUser = db.SiteUsers.Where(p => p.ASPNetUserID == aspNetUser.Id).FirstOrDefault();
                        if (siteUser == null)
                        {
                            ModelState.AddModelError("", "Invitation code invalid or expired.");
                            return View(Model);
                        }

                        company = db.SiteCompanies.Where(p => p.SiteCoID == siteUser.SiteCoID).FirstOrDefault();
                        if (company == null)
                        {
                            ModelState.AddModelError("", "Invitation code invalid or expired.");
                            return View(Model);
                        }

                        invitationAccepted = true;
                        Model.CompanyDetails.CompanyID = company.SiteCoID;
                    }
                    else
                    {
                        #region Add New Company

                        var co = Model.CompanyDetails;

                        int status = repo.AddNewSiteCompany(new SiteCompanies()
                        {
                            CoName = co.CompanyName,
                            CoAddress1 = co.Address1,
                            CoAddress2 = co.Address2,
                            CoCity = co.City,
                            CoState = co.State,
                            CoZip = co.Zip,
                            CoCountry = co.Country,
                            CoPhone = co.Phone
                        });

                        if (status == -1)
                        {
                            ModelState.AddModelError("", "Company already registered. Please contact your company for invitation.");
                            return View(Model);
                        }

                        Model.CompanyDetails.CompanyID = status;
                        db.InsertNewCoData(status);
                        db.SaveChanges();

                        //db.InsertFirstLicense(status,DateTime.Now,d)
                        #endregion
                    }

                    //ASP Net User
                    var emailAlreadyExists = db.AspNetUsers.Where(p => p.Email == Model.Email //check for same email
                                        && p.Id != aspNetUser.Id) // ignore user's own email - in case for invited user
                                        .Any();
                    if (emailAlreadyExists)
                    {
                        ModelState.AddModelError("", "Email already registered.");
                        return View(Model);
                    }

                    string aspNetUserID;
                    if (isAdmin)
                    {
                        var user = new ApplicationUser
                        {
                            UserName = Model.Email,
                            Email = Model.Email,
                            PhoneNumber = Model.Phone,
                            //siteuserid = Model.CompanyDetails.CompanyID
                        };

                        var userCreateStatus = await UserManager.CreateAsync(user, Model.Password);
                        if (userCreateStatus.Succeeded == false)
                        {
                            AddErrors(userCreateStatus);
                            return View(Model);
                        }

                        aspNetUserID = user.Id;

                        //var roleStore = new Microsoft.AspNet.Identity.EntityFramework.RoleStore<Microsoft.AspNet.Identity.EntityFramework.IdentityRole>(context);
                        //var roleManager = new RoleManager<IdentityRole>(roleStore);

                       // UserManager.AddToRole(aspNetUserID, EnumWrapper.SiteUserRoles.admin.ToString());
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        db.InsertFirstUserBySiteID(Model.CompanyDetails.CompanyID,user.Id, string.Format("{0} {1}", Model.FirstName, Model.LastName), Model.FirstName, Model.LastName, "", user.Email, user.PasswordHash, user.SecurityStamp, user.PhoneNumber);
                        db.SaveChanges();

                    }
                    else
                    {
                        aspNetUserID = aspNetUser.Id;
                        aspNetUser.UserName = Model.Email;
                        aspNetUser.Email = Model.Email;
                        aspNetUser.PhoneNumber = Model.Phone;

                        UserManager.AddToRole(aspNetUserID, EnumWrapper.SiteUserRoles.user.ToString());

                        //set password
                        string passwordResetCode = await UserManager.GeneratePasswordResetTokenAsync(aspNetUserID);
                        var result = await UserManager.ResetPasswordAsync(aspNetUserID, passwordResetCode, Model.Password);
                        if (!result.Succeeded)
                        {
                            ModelState.AddModelError("", "Some error occurred. Please retry.");
                            return View(Model);
                        }


                        await SignInManager.SignInAsync(UserManager.FindById(aspNetUserID), isPersistent: false, rememberBrowser: false);
                    }


                    //site user
                    //if (isAdmin)
                    //{
                    //    siteUser = new SiteUsers();
                    //    db.SiteUsers.Add(siteUser);
                    //}
                    if (!isAdmin)
                    { 
                        siteUser.ASPNetUserID = aspNetUserID;
                    siteUser.SiteCoID = Model.CompanyDetails.CompanyID;
                    siteUser.UserFirstName = Model.FirstName;
                    siteUser.UserLastName = Model.LastName;
                    siteUser.UserDisplayName = string.Format("{0} {1}", Model.FirstName, Model.LastName);
                    siteUser.UserStatus = "Active";
                    siteUser.TimeZoneID = 1;
                  
                        db.SaveChanges();
                                
                    aspNetUser = db.AspNetUsers.Where(p => p.Id == aspNetUserID).FirstOrDefault();
                        if (aspNetUser != null)
                        {
                            aspNetUser.siteuserid = siteUser.SiteUserID;
                            aspNetUser.sitecoid = siteUser.SiteCoID ?? 0;
                            if (invitationAccepted.HasValue)
                            {
                                aspNetUser.InvitationAccepted = invitationAccepted;
                            }
                            db.SaveChanges();
                        }
                    }

                    tran.Complete();

                    // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

                    //return RedirectToAction("Index", "Home");

                }
                if (isAdmin)
                {
                    return RedirectToAction("Welcome");
                }
                else
                {
                    return RedirectToAction("Index", "Dashboard", new { area = "Sections" });
                }
            }

            // If we got this far, something failed, redisplay form
            return View(Model);
        }
        //
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var result = await UserManager.ConfirmEmailAsync(userId, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(model.Email);
                if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return View("ForgotPasswordConfirmation");
                }

                // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                // Send an email with this link
                 string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                 var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);		
                 await UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
                 return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            AddErrors(result);
            return View();
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/SendCode
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
        {
            var userId = await SignInManager.GetVerifiedUserIdAsync();
            if (userId == null)
            {
                return View("Error");
            }
            var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/SendCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            // Generate the token and send it
            if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
            {
                return View("Error");
            }
            return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
                case SignInStatus.Failure:
                default:
                    // If the user does not have an account, then prompt the user to create an account
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Manage");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion

        [Authorize(Roles = "Admin")]
        public ActionResult Welcome()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Redirect("Login");
            }

            return View();
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Invite()
        {
            var manager = new UserManager<ApplicationUser>(new Microsoft.AspNet.Identity.EntityFramework.UserStore<ApplicationUser>(new ApplicationDbContext()));
            var currentUser = manager.FindById(User.Identity.GetUserId());
            var siteUserID = currentUser.siteuserid;
            var siteCoID = currentUser.sitecoid;
            using (ePontiv2Entities db = new ePontiv2Entities())
            {
                ViewBag.Licenses = new SelectList(db.GetLicensesBySiteCoID(siteCoID).ToList(), nameof(GetLicenseListBySiteCoID_Result.LicenseID), nameof(GetLicenseListBySiteCoID_Result.Version));
                ViewBag.Profiles = new SelectList(db.GetProfilesBySiteCoID(siteCoID).ToList(), nameof(GetProfilesBySiteCoID_Result.ViewID), nameof(GetProfilesBySiteCoID_Result.Name));
            }
            var model = new Models.InviteUserModel();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Invite(InviteUserModel Model,int? CoLicense,int? CoProfile)
        {
            if (ModelState.IsValid)
            {
                var manager = new UserManager<ApplicationUser>(new Microsoft.AspNet.Identity.EntityFramework.UserStore<ApplicationUser>(new ApplicationDbContext()));
                var currentUser = manager.FindById(User.Identity.GetUserId());
                var siteUserID = currentUser.siteuserid;
                var siteCoID = currentUser.sitecoid;

                using (ePontiv2Entities db = new ePontiv2Entities())
                using (TransactionScope tran = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    CommonRepository repo = new CommonRepository();
                    ViewBag.Licenses = new SelectList(db.GetLicensesBySiteCoID(siteCoID).ToList(), nameof(GetLicenseListBySiteCoID_Result.LicenseID), nameof(GetLicenseListBySiteCoID_Result.Version));
                    ViewBag.Profiles = new SelectList(db.GetProfilesBySiteCoID(siteCoID).ToList(), nameof(GetProfilesBySiteCoID_Result.ViewID), nameof(GetProfilesBySiteCoID_Result.Name));
                    string invitationCode = Guid.NewGuid().ToString("n") + Guid.NewGuid().ToString("n");
                    //db.InsertInviteeBySiteCoID(siteCoID, Model.FirstName, Model.LastName, CoProfile, Model.Email, Model.Phone, invitationCode, CoLicense);
                    //db.SaveChanges();
                    //ASP Net User
                    var user = new ApplicationUser
                    {
                        UserName = Model.Email,
                        Email = Model.Email,
                        PhoneNumber = Model.Phone,
                        sitecoid = siteCoID
                    };

                    var userCreateStatus = await UserManager.CreateAsync(user);
                    if (userCreateStatus.Succeeded == false)
                    {
                        AddErrors(userCreateStatus);
                        return View(Model);
                    }

                    //site user
                    var siteUser = new SiteUsers();
                    db.SiteUsers.Add(siteUser);

                    siteUser.ASPNetUserID = user.Id;
                    siteUser.SiteCoID = siteCoID;
                    siteUser.UserFirstName = Model.FirstName;
                    siteUser.UserLastName = Model.LastName;
                    siteUser.UserDisplayName = string.Format("{0} {1}", Model.FirstName, Model.LastName);
                    siteUser.UserStatus = "Active";
                    siteUser.TimeZoneID = 1;
                    db.SaveChanges();

                    var aspNetUser = db.AspNetUsers.Where(p => p.Id == user.Id).FirstOrDefault();
                    if (aspNetUser != null)
                    {
                        aspNetUser.siteuserid = siteUser.SiteUserID;
                        aspNetUser.sitecoid = siteUser.SiteCoID ?? 0;
                        aspNetUser.InvitationCode = invitationCode;

                        db.SaveChanges();
                    }
                    tran.Complete();

                    // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    try
                    {
                        var callbackUrl = Url.Action("Register", "Account", new { invitation = invitationCode }, protocol: Request.Url.Scheme);
                        //await UserManager.SendEmailAsync(user.Id, "You are invited", "Hi,<br>You are invited. Please accept the invitation by clicking <a href=\"" + callbackUrl + "\">here</a>");
                        await Mailer.Execute("You are invited", Model.Email, Model.FirstName+" "+Model.LastName, "Hi,<br>You are invited. Please accept the invitation by clicking <a href=\"" + callbackUrl + "\">here</a><br><br>Thanks.");
                        return RedirectToAction("Login");
                    }
                    catch (Exception ex)
                    {
                        LogRepository.LogException(ex);
                    }
                  
                }
                //invitation sent. reset model, so that new use can be invited
                Model = new InviteUserModel() { IsInvitationSent = true };
                return View(Model);
            }

            // If we got this far, something failed, redisplay form
            return View(Model);
        }
        

    }
}