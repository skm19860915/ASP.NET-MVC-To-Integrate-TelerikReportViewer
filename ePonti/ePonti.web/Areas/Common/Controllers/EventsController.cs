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
using System.Data.Entity.Core.Objects;
using System.Transactions;
using ePonti.BLL.Common;
using ePonti.BOL.Repository;

namespace ePonti.web.Areas.Common.Controllers
{
    public class EventsController : ePonti.web.Controllers._baseMVCController
    {
        private ePontiv2Entities db = new ePontiv2Entities();
        BOL.Repository.CommonRepository repo = new BOL.Repository.CommonRepository();

        public ActionResult Details(int? id)
        {
            var data = db.GetActivitiesEventByID(id, null, null).FirstOrDefault();
            if (data == null)
            {
                return HttpNotFound();
            }

            ViewBag.EventInvitees = db.GetEventInvitees(id);
            ViewBag.ProjectId = db.ActivitiesEvents.Where(p=>p.EventID == id).Select(p=>p.ProjectID).FirstOrDefault();

            return View(data);
        }

        public ActionResult Create(int? contactid, int? projectid)
        {
            if (projectid.HasValue)
            {
                BOL.Repository.CommonRepository repo = new BOL.Repository.CommonRepository();
                contactid = repo.GetContactIdByProjectID(projectid.Value);
            }

            var siteUsers = db.SiteUsers.Where(p => p.SiteCoID == siteusercompanyid).OrderBy(p => p.UserDisplayName);
            ViewBag.Owners = new SelectList(siteUsers, "SiteUserID", "UserDisplayName", siteuserid);
            ViewBag.Jobs = new SelectList(db.GetJobsByContactID_WithLockFalse(contactid), "ViewID", "Project", projectid);
            ViewBag.Status = new SelectList(repo.GetActivityStatus(siteusercompanyid), "StatusID", "StatusName");
            ViewBag.Invitees = new SelectList(GetUsersForEventInvitation(siteusercompanyid), "Item1", "Item2");

            ViewBag.SelectedContactID = contactid;
            ViewBag.IsFieldsReadOnly = false;

            var model = new Models.EventModels.NewEvent();
            model.InviteeIds = new List<string>() {"U"+siteuserid.ToString()};
            if (contactid.HasValue)
            {
                model.InviteeIds.Add("C"+contactid.Value.ToString());
            }

            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(EventModels.NewEvent Model)
        {
            var errorList = new List<string>();

            if (ModelState.IsValid)
            {
                int eventID = SaveEvent(Model);
                if (eventID > 0)
                {
                    return Json(new { status = "success", eventID });
                }
                else
                {
                    errorList.Add("Event couldn't be saved. Please retry.");
                }
            }

            errorList.AddRange((from item in ModelState.Values
                                from error in item.Errors
                                select error.ErrorMessage).ToList()
                             );

            return Json(new { status = "error", errors = errorList });
        }

        private int SaveEvent(EventModels.NewEvent Model)
        {
            int siteCoID = siteusercompanyid;

            var isNewEvent = (Model.EventID ?? 0) == 0;
            ActivitiesEvents newEvent = new ActivitiesEvents()
            {
                EventID = Model.EventID ?? 0,
                SiteCoID = siteusercompanyid,
                SiteUserID = Model.OwnerID ?? siteuserid,
                EventName = Model.Event ?? "",
                ProjectID = Model.JobID??0,
                StatusID = Model.StatusID,
                EventDate = Model.Date,
                EventTime = Model.Date.HasValue ? Model.Date.Value.TimeOfDay : new TimeSpan(),
                EventHours = Model.Duration ?? 1,
                AllDay = Model.Duration >= 24,
                Notes = Model.Notes ?? "",
                AcctTaskID = "",
                RecurrenceRule = ""
            };

            BOL.Repository.CommonRepository repo = new BOL.Repository.CommonRepository();

            int eventID = Model.EventID ?? 0;
            using (TransactionScope scope = new TransactionScope())
            {
                eventID = repo.SaveEvent(newEvent);

                if (isNewEvent)
                {
                    repo.AddEventInvitees(eventID, ExtractInviteeIdsData(Model.InviteeIds));
                }
                else
                {
                    repo.UpdateEventInvitees(eventID, ExtractInviteeIdsData(Model.InviteeIds));
                }

                scope.Complete();
            }

            if (eventID > 0)
            {
                return eventID;
            }
            else
            {
                return 0;
            }
        }


        public ActionResult Edit(int? id, int? contactid, int? projectid)
        {
            if (projectid.HasValue)
            {
                BOL.Repository.CommonRepository repo = new BOL.Repository.CommonRepository();
                contactid = repo.GetContactIdByProjectID(projectid.Value);
            }

            var siteUsers = db.SiteUsers.Where(p => p.SiteCoID == siteusercompanyid).OrderBy(p => p.UserDisplayName);
            ViewBag.Owners = new SelectList(siteUsers, "SiteUserID", "UserDisplayName", siteuserid);
            ViewBag.Jobs = new SelectList(db.GetJobsByContactID_WithLockFalse(contactid), "ViewID", "Project");
            ViewBag.Status = new SelectList(repo.GetActivityStatus(siteusercompanyid), "StatusID", "StatusName");
            ViewBag.IsFieldsReadOnly = true;

            var invitees = GetEventInvitees(id ?? 0);
            ViewBag.Invitees = new MultiSelectList(GetUsersForEventInvitation(siteusercompanyid), "Item1", "Item2", invitees);

            EventModels.NewEvent model = new EventModels.NewEvent();
            ActivitiesEvents _event = db.ActivitiesEvents.Where(p => p.EventID == (id ?? 0)).FirstOrDefault();
            if (_event != null)
            {
                model = new EventModels.NewEvent()
                {
                    EventID = _event.EventID,
                    Date = _event.EventDate.HasValue ? _event.EventDate.Value.Add(_event.EventTime ?? new TimeSpan()) : new Nullable<DateTime>(),
                    Duration = _event.EventHours,
                    Event = _event.EventName ?? "",
                    JobID = _event.ProjectID,
                    Notes = _event.Notes,
                    OwnerID = _event.SiteUserID,
                    StatusID = _event.StatusID,
                    InviteeIds = invitees
                };
            }

            ViewBag.SelectedContactID = contactid;
            return View("_Edit", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EventModels.NewEvent Model)
        {
            var errorList = new List<string>();

            if (ModelState.IsValid)
            {
                var newEventID = SaveEvent(Model);
                if (newEventID > 0)
                {
                    return Json(new { status = "success", eventID = newEventID });
                }
                else
                {
                    errorList.Add("Event couldn't be updated. Please retry.");
                }
            }

            errorList.AddRange((from item in ModelState.Values
                                from error in item.Errors
                                select error.ErrorMessage).ToList()
                             );

            return Json(new { status = "error", errors = errorList });
        }

        public ActionResult UpdateEventNotes(int EventID, string Note)
        {
            var repo = new BOL.Repository.CommonRepository();
            bool res = repo.UpdateEventNotes(EventID, siteusercompanyid, Note);

            return Json(new { result = res }, JsonRequestBehavior.AllowGet);
        }

        public List<string> GetEventInvitees(int EventID)
        {
            var invitees = db.ActivitiesEventInvitees.Where(p => p.EventID == EventID).ToList();

            return invitees.Select(p =>

            (p.ContactID.HasValue ? ("C" + p.ContactID.Value) : ("U" + p.SiteUserID))

            ).ToList();
        }

        public List<Tuple<string, string>> GetUsersForEventInvitation(int SiteCoId)
        {
            var res = db.GetInviteesBySiteCoID(SiteCoId);
            return res.Select(p => Tuple.Create(p.Type + p.ViewID, p.Contact)).ToList();
        }

        public List<ActivitiesEventInvitees> ExtractInviteeIdsData(IEnumerable<string> InviteeIds)
        {
            try
            {
                var res = (from invitee in InviteeIds
                           let isContact = invitee.StartsWith("C", StringComparison.OrdinalIgnoreCase)
                           select new ActivitiesEventInvitees()
                           {
                               ContactID = isContact ? Convert.ToInt32(invitee.Substring(1)) : (int?)null,
                               SiteUserID = !isContact ? Convert.ToInt32(invitee.Substring(1)) : 0
                           }).ToList();

                return res;
            }
            catch (Exception ex)
            {
                ex.Log();
                return new List<ActivitiesEventInvitees>();
            }
        }
    }
}
