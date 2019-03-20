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
using ePonti.BLL.Common;

namespace ePonti.web.Areas.Common.Controllers
{
    public class CasesController : ePonti.web.Controllers._baseMVCController
    {
        private ePontiv2Entities db = new ePontiv2Entities();
        BOL.Repository.CommonRepository repo = new BOL.Repository.CommonRepository();

        public ActionResult Details(int? id)
        {            
            var data = db.GetProjectCaseByID(id).FirstOrDefault();

            ViewBag.CaseComments = db.GetCaseCommentsByCaseID(id).ToList();

            var _case = db.ProjectCases.Where(p => p.CaseID == id).FirstOrDefault() ?? new ProjectCases();

            if (_case != null)
            {
                ViewBag.Created = _case.DateCreated;
                ViewBag.ProjectID = _case.ProjectID;
            }

            var siteUsers = db.SiteUsers.Where(p => p.SiteCoID == siteusercompanyid).OrderBy(p => p.UserDisplayName);
            ViewBag.Resource = new SelectList(siteUsers, "SiteUserID", "UserDisplayName", _case != null ? _case.SiteUserID : 0);
            

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

            ViewBag.Jobs = new SelectList(db.GetJobsByContactID_WithLockFalse(contactid), "ViewID", "Project", projectid);
            ViewBag.CaseTypes = new SelectList(repo.GetCaseTypesBySiteCoID(siteusercompanyid), "CaseTypeID", "CaseType");
            ViewBag.Priorities = new SelectList(repo.GetPrioritiesList(), "Item2", "Item2");
            ViewBag.Status = new SelectList(repo.GetActivityStatus(siteusercompanyid), "StatusID", "StatusName");

            var siteUsers = db.SiteUsers.Where(p => p.SiteCoID == siteusercompanyid).OrderBy(p => p.UserDisplayName);
            ViewBag.Resource = new SelectList(siteUsers, "SiteUserID", "UserDisplayName");
            ViewBag.Creator = new SelectList(siteUsers, "SiteUserID", "UserDisplayName", siteuserid);

            ViewBag.SelectedContactID = contactid;
            ViewBag.IsFieldsReadOnly = false;

            var model = new Models.CaseModels.NewCase();
            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CaseModels.NewCase Model)
        {
            var errorList = new List<string>();

            if (ModelState.IsValid)
            {
                int caseID = SaveCase(Model);
                if (caseID > 0)
                {
                    return Json(new { status = "success", caseID });
                }
                else
                {
                    errorList.Add("Case couldn't be saved. Please retry.");
                }
            }

            errorList.AddRange((from item in ModelState.Values
                                from error in item.Errors
                                select error.ErrorMessage).ToList()
                             );

            return Json(new { status = "error", errors = errorList });
        }

        private int SaveCase(CaseModels.NewCase Model)
        {
            int siteCoID = siteusercompanyid;

            ProjectCases newCase = new ProjectCases()
            {
                CaseID = Model.CaseID ?? 0,
                SiteCoID = siteusercompanyid,
                CreatedByUserID = Model.CreatorID,
                ProjectID = Model.JobID,
                SiteUserID = Model.ResourceID ?? siteuserid,
                Subject = Model.Concern,
                StatusID = Model.StatusID,
                DueDate = Model.Due,
                Notes = Model.Notes,
                Priority = Model.Priority,
                DateCreated = Model.Created ?? DateTime.Now,
                CaseTypeID = Model.TypeID
            };

            BOL.Repository.CommonRepository repo = new BOL.Repository.CommonRepository();

            int caseID = repo.SaveCase(newCase);
            if (caseID > 0)
            {
                return caseID;
            }
            else
            {
                return 0;
            }
        }


        public ActionResult Edit(int? id, int? contactid, int? projectid)
        {
            ProjectCases _case = db.ProjectCases.Where(p => p.CaseID == (id ?? 0)).FirstOrDefault();
            if (_case == null)
            {
                return HttpNotFound();
            }

            if (projectid.HasValue)
            {
                BOL.Repository.CommonRepository repo = new BOL.Repository.CommonRepository();
                contactid = repo.GetContactIdByProjectID(projectid.Value);
            }


            ViewBag.Jobs = new SelectList(db.GetJobsByContactID_WithLockFalse(contactid).ToList(), "ViewID", "Project", _case.ProjectID);
            ViewBag.CaseTypes = new SelectList(repo.GetCaseTypesBySiteCoID(siteusercompanyid), "CaseTypeID", "CaseType", _case.CaseTypeID);
            ViewBag.Priorities = new SelectList(repo.GetPrioritiesList(), "Item2", "Item2", _case.Priority);
            ViewBag.Status = new SelectList(repo.GetActivityStatus(siteusercompanyid), "StatusID", "StatusName", _case.StatusID);

            var siteUsers = db.SiteUsers.Where(p => p.SiteCoID == siteusercompanyid).OrderBy(p => p.UserDisplayName);
            ViewBag.Resource = new SelectList(siteUsers, "SiteUserID", "UserDisplayName");
            ViewBag.Creator = new SelectList(siteUsers, "SiteUserID", "UserDisplayName", siteuserid);
            ViewBag.IsFieldsReadOnly = true;

            CaseModels.NewCase model = new CaseModels.NewCase();

            model = new CaseModels.NewCase()
            {
                CaseID = _case.CaseID,
                CreatorID = _case.CreatedByUserID,
                ResourceID = _case.SiteUserID,
                JobID = _case.ProjectID,
                Concern = _case.Subject,
                StatusID = _case.StatusID,
                Due = _case.DueDate,
                Notes = _case.Notes,
                Priority = _case.Priority,
                Created = _case.DateCreated
            };

            ViewBag.SelectedContactID = contactid;
            return View("_Edit", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CaseModels.NewCase Model)
        {
            var errorList = new List<string>();

            if (ModelState.IsValid)
            {
                var newCaseID = SaveCase(Model);
                if (newCaseID > 0)
                {
                    return Json(new { status = "success", caseID = newCaseID });
                }
                else
                {
                    errorList.Add("Case couldn't be updated. Please retry.");
                }
            }

            errorList.AddRange((from item in ModelState.Values
                                from error in item.Errors
                                select error.ErrorMessage).ToList()
                             );

            return Json(new { status = "error", errors = errorList });
        }

        public ActionResult AddCaseComment(int CaseID, string Comment)
        {
            var repo = new BOL.Repository.CommonRepository();
            bool res = repo.AddCaseComment(CaseID, siteuserid, Comment);
            string userName = repo.GetUserDisplayName(siteuserid);

            return Json(new { result = res, name = userName, date = DateTime.Now.ToCustomDateTimeString() }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UpdateCaseDetails(int CaseID, int ResourceID)
        {
            var res = repo.UpdateCaseDetails(CaseID, ResourceID);
            return Json(new { status = res });
        }
    }
}
