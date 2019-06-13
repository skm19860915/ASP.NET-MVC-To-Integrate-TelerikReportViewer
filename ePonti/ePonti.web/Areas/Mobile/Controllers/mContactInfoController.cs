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

namespace ePonti.web.Areas.Mobile.Controllers
{
    public class mContactInfoController : ePonti.web.Controllers._baseMVCController
    {
        private ePontiv2Entities db = new ePontiv2Entities();

        // GET: Mobile/mContactInfo
        public ActionResult Details(int? id)
        {
            if (id == null) { return new HttpStatusCodeResult(HttpStatusCode.BadRequest); }

            CoContacts coContacts = db.CoContacts.Find(id);
            if (coContacts == null || coContacts.SiteCoID != base.siteusercompanyid)
            { return HttpNotFound(); }

            ViewBag.Country = "";
            var address = coContacts.CoContactAddresses.FirstOrDefault();
            if (address != null)
            {
                ViewBag.Country = new CommonRepository().GetCountryNameByID(address.CountryID ?? 0);
            }

            if (coContacts.CoContactTypes == null)
            {
                coContacts.CoContactTypes = new CoContactTypes();//to prevent null checks, init to new()
            }

            ViewBag.Activities = db.GetActivitiesByContactID(id);
            if ((coContacts.CoContactTypes).IsVendor != true)
            {
                ViewBag.Projects = db.GetProjectsByContactID(id);
            }
            if (coContacts.CoContactTypes.IsVendor == true)
            {
                ViewBag.POR = db.GetPORByContactID(id);
            }

            return View("details", coContacts);
        }


        // GET: Mobile/mContactInfo/Create
        public ActionResult Create()
        {
            ViewBag.ContactCo = new SelectList(db.CoContactCompanies.Where(p => p.SiteCoID == siteusercompanyid).OrderBy(p => p.ContactCoName), "ContactCoID", "ContactCoName");
            ViewBag.Country = new SelectList(new CommonRepository().GetCountries(), "CountryID", "Country");
            ViewBag.ContactType = new SelectList(db.CoContactTypes.Where(p => p.SiteCoID == siteusercompanyid).OrderBy(p => p.ContactTypeOrder), "ContactTypeID", "ContactTypeName");
            ViewBag.ContactStatus = new SelectList(db.SiteContactStatus.OrderBy(p => p.StatusOrder), "StatusID", "Status");
            ViewBag.Owner = new SelectList(db.SiteUsers.Where(p => p.SiteCoID == siteusercompanyid).OrderBy(p => p.UserDisplayName), "SiteUserID", "UserDisplayName", siteuserid);
            ViewBag.siteuserid = siteuserid;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PeopleModels.NewContact Model)
        {
            var errorList = new List<string>();

            if (ModelState.IsValid)
            {
                int contactID = SaveContact(Model);
                if (contactID > 0)
                {
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

        private int SaveContact(PeopleModels.NewContact Model)
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
                ContactManager = Model.OwnerID
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

        public ActionResult Edit(int id)
        {
            ViewBag.ContactCo = new SelectList(db.CoContactCompanies.Where(p => p.SiteCoID == siteusercompanyid).OrderBy(p => p.ContactCoName), "ContactCoID", "ContactCoName");
            ViewBag.Country = new SelectList(db.SiteCountries.OrderBy(p => p.Country), "CountryID", "Country");
            ViewBag.ContactType = new SelectList(db.CoContactTypes.Where(p => p.SiteCoID == siteusercompanyid).OrderBy(p => p.ContactTypeOrder), "ContactTypeID", "ContactTypeName");
            ViewBag.ContactStatus = new SelectList(db.SiteContactStatus.OrderBy(p => p.StatusOrder), "StatusID", "Status");
            ViewBag.Owner = new SelectList(db.SiteUsers.Where(p => p.SiteCoID == siteusercompanyid).OrderBy(p => p.UserDisplayName), "SiteUserID", "UserDisplayName", siteuserid);
            ViewBag.siteuserid = siteuserid;

            PeopleModels.NewContact Model = new PeopleModels.NewContact();
            CoContacts contact = db.CoContacts
                .Include(p => p.CoContactAddresses)
                .Include(p => p.CoContactEmails)
                .Where(p => p.ContactID == id).FirstOrDefault();
            CoContactAddresses address = contact.CoContactAddresses.Where(p => p.ContactID == id).FirstOrDefault() ?? new CoContactAddresses();
            if (contact != null)
            {
                Model = new PeopleModels.NewContact()
                {
                    Address1 = address.Address1,
                    Address2 = address.Address2,
                    City = address.City,
                    CountryID = address.CountryID,
                    State = address.State,
                    Zip = address.Zip,
                    Company = contact.CoContactCompanies != null ? contact.CoContactCompanies.ContactCoName : "",
                    ContactID = contact.ContactID,
                    Email = contact.CoContactEmails.Any() ? contact.CoContactEmails.Select(p => p.EmailAddress).FirstOrDefault() : "",
                    First = contact.ContactFirstName,
                    Last = contact.ContactLastName,
                    Mobile = contact.CoContactPhones.Any() ? contact.CoContactPhones.Where(p => p.PhoneTypeID == 1).Select(p => p.Phone).FirstOrDefault() : "",
                    Phone = contact.CoContactPhones.Any() ? contact.CoContactPhones.Where(p => p.PhoneTypeID == 2).Select(p => p.Phone).FirstOrDefault() : "",
                    StatusID = contact.ContactStatusID,
                    TypeID = contact.ContactTypeID,
                    OwnerID = contact.ContactManager
                };
            }


            return View("_Edit", Model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PeopleModels.NewContact Model)
        {
            var errorList = new List<string>();

            if (ModelState.IsValid)
            {
                if (EditContact(Model))
                {
                    return Json(new { status = "success", contactID = Model.ContactID });
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

        private bool EditContact(PeopleModels.NewContact Model)
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

            CoContacts contact = db.CoContacts
            .Include(p => p.CoContactAddresses)
            .Include(p => p.CoContactEmails)
            .Where(p => p.ContactID == Model.ContactID).FirstOrDefault();

            contact.ContactID = Model.ContactID ?? 0;
            contact.SiteCoID = siteCoID;
            contact.ContactTypeID = Model.TypeID ?? 0;
            contact.ContactStatusID = Model.StatusID;
            contact.ContactFirstName = Model.First;
            contact.ContactLastName = Model.Last;
            contact.ContactCoID = company.ContactCoID;
            contact.ModifiedDate = DateTime.Now;
            contact.ContactManager = Model.OwnerID;
            db.SaveChanges();

            repo.UpdatePhone(Model.ContactID ?? 0, (int)EnumWrapper.PhoneTypes.Mobile, Model.Mobile, true);
            repo.UpdatePhone(Model.ContactID ?? 0, (int)EnumWrapper.PhoneTypes.Phone, Model.Phone, true);
            repo.UpdateEmail(Model.ContactID ?? 0, Model.Email, true);
            repo.UpdateAddress(Model.ContactID ?? 0, new CoContactAddresses()
            {
                Address1 = Model.Address1,
                Address2 = Model.Address2,
                City = Model.City,
                State = Model.State,
                Zip = Model.Zip,
                CountryID = Model.CountryID
            });
            return true;

        }

        // GET: Mobile/mContactInfo/Delete/5
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

        // POST: Mobile/mContactInfo/Delete/5
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
    }
}
