using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ePonti.BOL;
using ePonti.BLL.Common;
using ePonti.web.Models;
using System.Data.Entity.Core.Objects;

namespace ePonti.web.Areas.Common.Controllers
{
    [Authorize]
    public class CallsController : ePonti.web.Controllers._baseMVCController
    {
        private ePontiv2Entities db = new ePontiv2Entities();

        // GET: Common/Calls/Details/5
        public ActionResult Details(int? id)
        {           
            var data = db.GetActivitiesCallByID(id, null, null).FirstOrDefault();
            if (data == null)
            {
                return HttpNotFound();
            }

            ViewBag.ProjectID = db.ActivitiesCalls.Where(p=>p.CallID == id).Select(p=>p.ProjectID).FirstOrDefault();

            return View(data);
        }

        // GET: Common/Calls/Create
        public ActionResult Create(int? contactid, int? projectid)
        {
            BOL.Repository.CommonRepository repo = new BOL.Repository.CommonRepository();
            if (projectid.HasValue)
            {
                contactid = repo.GetContactIdByProjectID(projectid.Value);
            }

            ViewBag.Contacts = new SelectList(db.GetContactsBySiteCoID(siteusercompanyid), "ViewID", "Customer", contactid);
            ViewBag.Purpose = new SelectList(db.CoCallPurposes.Where(p => p.SiteCoID == siteusercompanyid).OrderBy(p => p.CallPurposeOrder), "CallPurposeID", "PurposeName");
            ViewBag.Resources = new SelectList(db.SiteUsers.Where(p => p.SiteCoID == siteusercompanyid).OrderBy(p => p.UserDisplayName), "SiteUserID", "UserDisplayName", siteuserid);
            ViewBag.Status = new SelectList(repo.GetActivityStatus(siteusercompanyid), "StatusID", "StatusName");

            var model = new Models.CallModels.NewCall();

            var contactData = FetchContactData(contactid ?? 0);
            model.Phone = contactData.Item1;
            model.Mobile = contactData.Item2;
            ViewBag.Jobs = new SelectList(contactData.Item3.Select(p => new { p.ViewID, p.Project }), "ViewID", "Project", projectid);
            ViewBag.SelectedContactID = contactid;

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CallModels.NewCall Model)
        {
            var errorList = new List<string>();

            if (ModelState.IsValid)
            {
                int callID = SaveCall(Model);
                if (callID > 0)
                {
                    return Json(new { status = "success", callID });
                }
                else
                {
                    errorList.Add("Call couldn't be saved. Please retry.");
                }
            }

            errorList.AddRange((from item in ModelState.Values
                                from error in item.Errors
                                select error.ErrorMessage).ToList()
                             );

            return Json(new { status = "error", errors = errorList });
        }

        private int SaveCall(CallModels.NewCall Model)
        {
            int siteCoID = siteusercompanyid;

            ActivitiesCalls newCall = new ActivitiesCalls()
            {
                CallDate = Model.Date,
                CallTime = Model.Date.HasValue ? Model.Date.Value.TimeOfDay : new TimeSpan(),
                CallDuration = Model.Duration,
                CallPurposeID = Model.PurposeID,
                SiteCoID = siteusercompanyid,
                SiteUserID = Model.ResourceID ?? siteuserid,
                ProjectID = Model.JobID ?? 0,
                Subject = Model.Subject,
                ContactID = Model.ContactID,
                Notes = Model.Notes,
                CallID = Model.CallID ?? 0,
                StatusID = Model.StatusID
            };

            BOL.Repository.CommonRepository repo = new BOL.Repository.CommonRepository();

            int callID = repo.SaveCall(newCall);
            repo.UpdatePhone(Model.ContactID ?? 0, (int)EnumWrapper.PhoneTypes.Mobile, Model.Mobile, true);
            repo.UpdatePhone(Model.ContactID ?? 0, (int)EnumWrapper.PhoneTypes.Phone, Model.Phone, true);

            if (callID > 0)
            {
                return callID;
            }
            else
            {
                return 0;
            }
        }

        public ActionResult Edit(int? id)
        {
            BOL.Repository.CommonRepository repo = new BOL.Repository.CommonRepository();

            ViewBag.Contacts = new SelectList(db.GetContactsBySiteCoID(siteusercompanyid), "ViewID", "Customer");
            ViewBag.Purpose = new SelectList(db.CoCallPurposes.Where(p => p.SiteCoID == siteusercompanyid).OrderBy(p => p.CallPurposeOrder), "CallPurposeID", "PurposeName");
            ViewBag.Resources = new SelectList(db.SiteUsers.Where(p => p.SiteCoID == siteusercompanyid).OrderBy(p => p.UserDisplayName), "SiteUserID", "UserDisplayName");
            ViewBag.Status = new SelectList(repo.GetActivityStatus(siteusercompanyid), "StatusID", "StatusName");

            CallModels.NewCall model = new CallModels.NewCall();
            ActivitiesCalls call = db.ActivitiesCalls.Where(p => p.CallID == (id ?? 0)).FirstOrDefault();
            if (call != null)
            {
                model = new CallModels.NewCall()
                {
                    CallID = call.CallID,
                    ContactID = call.ContactID,
                    Date = call.CallDate.HasValue ? call.CallDate.Value.Add(call.CallTime ?? new TimeSpan()) : call.CallDate,
                    Duration = call.CallDuration,
                    JobID = call.ProjectID,
                    Notes = call.Notes,
                    PurposeID = call.CallPurposeID,
                    ResourceID = call.SiteUserID,
                    Subject = call.Subject,
                    StatusID = call.StatusID
                };
            }

            var contactData = FetchContactData(call.ContactID ?? 0);
            model.Phone = contactData.Item1;
            model.Mobile = contactData.Item2;
            ViewBag.Jobs = new SelectList(contactData.Item3.Select(p => new { p.ViewID, p.Project }), "ViewID", "Project", model.JobID);

            return View("_Edit", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CallModels.NewCall Model)
        {
            var errorList = new List<string>();

            if (ModelState.IsValid)
            {
                if (SaveCall(Model) > 0)
                {
                    return Json(new { status = "success", contactID = Model.ContactID });
                }
                else
                {
                    errorList.Add("Call couldn't be updated. Please retry.");
                }
            }

            errorList.AddRange((from item in ModelState.Values
                                from error in item.Errors
                                select error.ErrorMessage).ToList()
                             );

            return Json(new { status = "error", errors = errorList });
        }

        public ActionResult UpdateCallNotes(int CallID, string Note)
        {
            var repo = new BOL.Repository.CommonRepository();
            bool res = repo.UpdateCallNotes(CallID, siteusercompanyid, Note);

            return Json(new { result = res }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetContactData(int Id/*contactid*/)
        {
            var data = FetchContactData(Id);

            var res = new
            {
                phone = data.Item1,
                mobile = data.Item2,
                jobs = data.Item3.Select(p => new { p.ViewID, p.Project })
            };

            return Json(res, JsonRequestBehavior.AllowGet);
        }

        public Tuple<string, string, ObjectResult<GetJobsByContactID_Result>> FetchContactData(int ContactID)
        {
            var phone = db.GetPhoneByContactID(ContactID, (int)EnumWrapper.PhoneTypes.Phone).FirstOrDefault();
            var mobile = db.GetPhoneByContactID(ContactID, (int)EnumWrapper.PhoneTypes.Mobile).FirstOrDefault();
            var jobs = db.GetJobsByContactID(ContactID);

            var res = Tuple.Create(
                (phone ?? new GetPhoneByContactID_Result()).Phone,
                (mobile ?? new GetPhoneByContactID_Result()).Phone,
                jobs
            );

            return res;
        }

    }
}
