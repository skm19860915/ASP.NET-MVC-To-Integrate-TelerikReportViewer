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

namespace ePonti.web.Areas.Mobile.Controllers
{
    public class mPunchListController : ePonti.web.Controllers._baseMVCController
    {
        private ePontiv2Entities db = new ePontiv2Entities();
        BOL.Repository.CommonRepository repo = new BOL.Repository.CommonRepository();

        // GET: Mobile/mPunchList

        public ActionResult Details(int? id)
        {
            var data = db.GetProjectPunchListByID(id).FirstOrDefault();
            ViewBag.PunchListComments = db.GetCommentsByProjectPunchListID(id);

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
                contactid = repo.GetContactIdByProjectID(projectid.Value);
            }

            var jobs = db.GetJobsByContactID_WithLockFalse(contactid).ToList();
            var selectedJobId = projectid.HasValue ? projectid.Value : ((jobs.FirstOrDefault() ?? new GetJobsByContactID_WithLockFalse_Result()).ViewID);
            ViewBag.Jobs = new SelectList(jobs, "ViewID", "Project", selectedJobId);

            ViewBag.Types = new SelectList(repo.GetPunchListTypesBySiteCoID(siteusercompanyid), "PunchListTypeID", "PunchListType");
            ViewBag.Departments = new SelectList(repo.GetPunchListDepartmentsBySiteCoID(siteusercompanyid), "PunchItemDepartmentID", "ItemDepartment");
            ViewBag.Priorities = new SelectList(repo.GetPrioritiesList(), "Item1", "Item2");
            ViewBag.Status = new SelectList(repo.GetActivityStatus(siteusercompanyid), "StatusID", "StatusName");

            ViewBag.Divisions = new SelectList(repo.GetDivisionsByProjectID(selectedJobId), "ProjectDivisionID", "DivisionName");
            ViewBag.Areas = new SelectList(repo.GetAreasByProjectID(selectedJobId), "ProjectAreaID", "AreaName");

            var siteUsers = db.SiteUsers.Where(p => p.SiteCoID == siteusercompanyid).OrderBy(p => p.UserDisplayName);
            ViewBag.Creator = new SelectList(siteUsers, "SiteUserID", "UserDisplayName", siteuserid);

            ViewBag.SelectedContactID = contactid;
            var model = new Models.PunchListModels.NewPunchList();
            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PunchListModels.NewPunchList Model)
        {
            var errorList = new List<string>();

            if (ModelState.IsValid)
            {
                int punchListID = SavePunchList(Model);
                if (punchListID > 0)
                {
                    return Json(new { status = "success", punchListID });
                }
                else
                {
                    errorList.Add("Punch item couldn't be saved. Please retry.");
                }
            }

            errorList.AddRange((from item in ModelState.Values
                                from error in item.Errors
                                select error.ErrorMessage).ToList()
                             );

            return Json(new { status = "error", errors = errorList });
        }

        private int SavePunchList(PunchListModels.NewPunchList Model)
        {
            int siteCoID = siteusercompanyid;

            ProjectPunchLists newPunchList = new ProjectPunchLists()
            {
                ProjectPunchListID = Model.PunchListID ?? 0,
                SiteCoID = siteusercompanyid,
                ProjectID = Model.JobID,
                Subject = Model.Description,
                TypeID = Model.TypeID,
                PriorityID = Model.PriorityID,
                StatusID = Model.StatusID,
                CreatedByUserID = Model.CreatorID,
                DepartmentID = Model.DepartmentID,
                DivisionID = Model.DivisionID,
                AreaID = Model.AreaID,
                Note = Model.Notes,
                EstHrs = Model.Hours,
                DueDate = Model.Due
            };

            if (newPunchList.ProjectPunchListID == 0)
            {
                newPunchList.CreateDate = DateTime.Now;
            }

            int punchListID = repo.SavePunchList(newPunchList);
            if (punchListID > 0)
            {
                return punchListID;
            }
            else
            {
                return 0;
            }
        }


        public ActionResult Edit(int? id, int contactid)
        {
            ProjectPunchLists _punchList = db.ProjectPunchLists.Where(p => p.ProjectPunchListID == (id ?? 0)).FirstOrDefault();
            if (_punchList == null)
            {
                return HttpNotFound();
            }

            var jobs = db.GetJobsByContactID_WithLockFalse(contactid).ToList();
            ViewBag.Jobs = new SelectList(jobs, "ViewID", "Project", _punchList.ProjectID);

            ViewBag.Types = new SelectList(repo.GetPunchListTypesBySiteCoID(siteusercompanyid), "PunchListTypeID", "PunchListType");
            ViewBag.Departments = new SelectList(repo.GetPunchListDepartmentsBySiteCoID(siteusercompanyid), "PunchItemDepartmentID", "ItemDepartment");
            ViewBag.Priorities = new SelectList(repo.GetPrioritiesList(), "Item1", "Item2");
            ViewBag.Status = new SelectList(repo.GetActivityStatus(siteusercompanyid), "StatusID", "StatusName");

            ViewBag.Divisions = new SelectList(repo.GetDivisionsByProjectID(_punchList.ProjectID ?? 0), "ProjectDivisionID", "DivisionName");
            ViewBag.Areas = new SelectList(repo.GetAreasByProjectID(_punchList.ProjectID ?? 0), "ProjectAreaID", "AreaName");

            var siteUsers = db.SiteUsers.Where(p => p.SiteCoID == siteusercompanyid).OrderBy(p => p.UserDisplayName);
            ViewBag.Creator = new SelectList(siteUsers, "SiteUserID", "UserDisplayName", siteuserid);

            PunchListModels.NewPunchList model = new PunchListModels.NewPunchList();

            model = new PunchListModels.NewPunchList()
            {
                PunchListID = _punchList.ProjectPunchListID,
                AreaID = _punchList.AreaID,
                CreatorID = _punchList.CreatedByUserID,
                DepartmentID = _punchList.DepartmentID,
                Description = _punchList.Subject,
                DivisionID = _punchList.DivisionID,
                Due = _punchList.DueDate,
                Hours = _punchList.EstHrs,
                JobID = _punchList.PriorityID,
                Notes = _punchList.Note,
                PriorityID = _punchList.PriorityID,
                StatusID = _punchList.StatusID,
                TypeID = _punchList.TypeID
            };

            return View("_Edit", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PunchListModels.NewPunchList Model)
        {
            var errorList = new List<string>();

            if (ModelState.IsValid)
            {
                var newPunchListID = SavePunchList(Model);
                if (newPunchListID > 0)
                {
                    return Json(new { status = "success", punchListID = newPunchListID });
                }
                else
                {
                    errorList.Add("Punch item couldn't be updated. Please retry.");
                }
            }

            errorList.AddRange((from item in ModelState.Values
                                from error in item.Errors
                                select error.ErrorMessage).ToList()
                             );

            return Json(new { status = "error", errors = errorList });
        }

        public ActionResult AddPunchListComment(int PunchListID, string Comment)
        {
            var repo = new BOL.Repository.CommonRepository();
            bool res = repo.AddPunchListComment(PunchListID, siteuserid, Comment);

            return Json(new { result = res }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetProjectData(int Id/*projectid*/)
        {
            var data = FetchProjectData(Id);

            var res = new
            {
                divisions = data.Item1.Select(p => new { Id = p.ProjectDivisionID, Name = p.DivisionName }),
                areas = data.Item2.Select(p => new { Id = p.ProjectAreaID, Name = p.AreaName })
            };

            return Json(res, JsonRequestBehavior.AllowGet);
        }

        [NonAction]
        private Tuple<List<ProjectDivisions>, List<ProjectAreas>> FetchProjectData(int ProjectID)
        {
            var divisions = repo.GetDivisionsByProjectID(ProjectID);
            var areas = repo.GetAreasByProjectID(ProjectID);

            var res = Tuple.Create(
                divisions,
                areas
            );

            return res;
        }
    }
}
