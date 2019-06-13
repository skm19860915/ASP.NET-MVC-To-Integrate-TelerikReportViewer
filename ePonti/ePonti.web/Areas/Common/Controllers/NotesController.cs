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

namespace ePonti.web.Areas.Common.Controllers
{
    public class NotesController : ePonti.web.Controllers._baseMVCController
    {
        private ePontiv2Entities db = new ePontiv2Entities();

        public ActionResult Details(int? id)
        {
            var data = db.GetActivitiesNoteByID(id, null, null).FirstOrDefault();
            if (data == null)
            {
                return HttpNotFound();
            }

            return View(data);
        }

        public ActionResult Create(int? contactid, int? projectid)
        {
            if (projectid.HasValue)
            {
                BOL.Repository.CommonRepository repo = new BOL.Repository.CommonRepository();
                contactid = repo.GetContactIdByProjectID(projectid.Value);
            }

            ViewBag.Contacts = new SelectList(db.GetContactsBySiteCoID(siteusercompanyid), "ViewID", "Customer", contactid);
            ViewBag.Creator = new SelectList(db.SiteUsers.Where(p => p.SiteCoID == siteusercompanyid).OrderBy(p => p.UserDisplayName), "SiteUserID", "UserDisplayName", siteuserid);
            ViewBag.IsFieldsReadOnly = false;

            var model = new Models.NoteModels.NewNote() {
                Date = DateTime.Now
            };

            var contactData = FetchContactData(contactid ?? 0);
            ViewBag.Jobs = new SelectList(contactData.Item1.Select(p => new { p.ViewID, p.Project }), "ViewID", "Project", projectid);
            ViewBag.SelectedContactID = contactid;

            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(NoteModels.NewNote Model)
        {
            var errorList = new List<string>();

            if (ModelState.IsValid)
            {
                int noteID = SaveNote(Model);
                if (noteID > 0)
                {
                    return Json(new { status = "success", noteID });
                }
                else
                {
                    errorList.Add("Note couldn't be saved. Please retry.");
                }
            }

            errorList.AddRange((from item in ModelState.Values
                                from error in item.Errors
                                select error.ErrorMessage).ToList()
                             );

            return Json(new { status = "error", errors = errorList });
        }

        private int SaveNote(NoteModels.NewNote Model)
        {
            int siteCoID = siteusercompanyid;

            ActivitiesNotes newNote = new ActivitiesNotes()
            {
                SiteCoID = siteusercompanyid,
                SiteUserID = Model.CreatorID ?? siteuserid,
                ProjectID = Model.JobID ?? 0,
                Subject = Model.Subject,
                ContactID = Model.ContactID,
                Notes = Model.Notes,
                NoteID = Model.NoteID ?? 0,
                DateCreated = Model.Date ?? DateTime.Now
            };

            BOL.Repository.CommonRepository repo = new BOL.Repository.CommonRepository();

            int noteID = repo.SaveNote(newNote);
            if (noteID > 0)
            {
                return noteID;
            }
            else
            {
                return 0;
            }
        }


        public ActionResult Edit(int? id)
        {
            ViewBag.Contacts = new SelectList(db.GetContactsBySiteCoID(siteusercompanyid), "ViewID", "Customer");
            ViewBag.Creator = new SelectList(db.SiteUsers.Where(p => p.SiteCoID == siteusercompanyid).OrderBy(p => p.UserDisplayName), "SiteUserID", "UserDisplayName", siteuserid);
            ViewBag.IsFieldsReadOnly = true;

            NoteModels.NewNote model = new NoteModels.NewNote();
            ActivitiesNotes note = db.ActivitiesNotes.Where(p => p.NoteID == (id ?? 0)).FirstOrDefault();
            if (note != null)
            {
                model = new NoteModels.NewNote()
                {
                    NoteID = note.NoteID,
                    ContactID = note.ContactID,
                    Date = note.DateCreated,
                    JobID = note.ProjectID,
                    Notes = note.Notes,
                    CreatorID = note.SiteUserID,
                    Subject = note.Subject
                };
            }

            var contactData = FetchContactData(note.ContactID ?? 0);
            ViewBag.Jobs = new SelectList(contactData.Item1.Select(p => new { p.ViewID, p.Project }), "ViewID", "Project", model.JobID);

            return View("_Edit", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(NoteModels.NewNote Model)
        {
            var errorList = new List<string>();

            if (ModelState.IsValid)
            {
                var newNoteID = SaveNote(Model);
                if (newNoteID > 0)
                {
                    return Json(new { status = "success", noteID = newNoteID });
                }
                else
                {
                    errorList.Add("Note couldn't be updated. Please retry.");
                }
            }

            errorList.AddRange((from item in ModelState.Values
                                from error in item.Errors
                                select error.ErrorMessage).ToList()
                             );

            return Json(new { status = "error", errors = errorList });
        }

        public ActionResult UpdateNoteNotes(int NoteID, string Note)
        {
            var repo = new BOL.Repository.CommonRepository();
            bool res = repo.UpdateNoteNotes(NoteID, siteusercompanyid, Note);

            return Json(new { result = res }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetContactData(int Id/*contactid*/)
        {
            var data = FetchContactData(Id);

            var res = new
            {
                jobs = data.Item1.Select(p => new { p.ViewID, p.Project })
            };

            return Json(res, JsonRequestBehavior.AllowGet);
        }

        public Tuple<ObjectResult<GetJobsByContactID_Result>> FetchContactData(int ContactID)
        {
            var jobs = db.GetJobsByContactID(ContactID);

            var res = Tuple.Create(
                jobs
            );

            return res;
        }

    }
}
