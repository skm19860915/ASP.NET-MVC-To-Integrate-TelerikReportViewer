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
    public class TimeItsController : ePonti.web.Controllers._baseMVCController
    {
        private ePontiv2Entities db = new ePontiv2Entities();
        BOL.Repository.CommonRepository repo = new BOL.Repository.CommonRepository();

        public ActionResult Details(int? id)
        {
            var data = db.GetTimeItByID(id).FirstOrDefault();
            if (data == null)
            {
                return HttpNotFound();
            }

            return View(data);
        }

        public ActionResult Create(int? contactid, int? projectid,int? PayrollPeriodItemID)
        {
            int siteCoID = base.siteusercompanyid;

            if (projectid.HasValue)
            {
                contactid = repo.GetContactIdByProjectID(projectid.Value);
            }
            ViewBag.PayrollPeriodItemID = PayrollPeriodItemID;
            var jobs = db.GetTimeItProjectsBySiteCoID(siteCoID).ToList();
            var selectedJobId = projectid.HasValue ? projectid.Value : ((jobs.FirstOrDefault() ?? new GetTimeItProjectsBySiteCoID_Result()).ViewID);
            ViewBag.Jobs = new SelectList(jobs, "ViewID", "Project", selectedJobId);

            var siteUsers = db.SiteUsers.Where(p => p.SiteCoID == siteusercompanyid).OrderBy(p => p.UserDisplayName);
            ViewBag.Resource = new SelectList(siteUsers, "SiteUserID", "UserDisplayName", siteuserid);

            var projectData = FetchProjectData(selectedJobId);
            ViewBag.Stages = new SelectList(projectData.Item1.Select(p => new { p.ProjectStageID, p.StageName }), "ProjectStageID", "StageName");
            ViewBag.PayTypes = new SelectList(projectData.Item2.Select(p => new { p.ViewID, p.Pay_Type }), "ViewID", "Pay_Type");

            ViewBag.SelectedContactID = contactid;
            var model = new Models.TimeItModels.NewTimeIt()
            {
                Date = DateTime.Now
            };
            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TimeItModels.NewTimeIt Model)
        {
            var errorList = new List<string>();

            if (ModelState.IsValid)
            {
                int caseID = SaveTimeIt(Model);
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

        private int SaveTimeIt(TimeItModels.NewTimeIt Model)
        {
            int siteCoID = siteusercompanyid;

            ProjectTimeIts newTimeIt = new ProjectTimeIts()
            {
                TimeItID = Model.TimeItID ?? 0,
                SiteCoID = siteusercompanyid,
                ProjectID = Model.ProjectID,
                SiteUserID = Model.ResourceID ?? siteuserid,
                Subject = Model.Task,
                Hours = Model.Hours,
                TimeItDate = Model.Date,
                TimeItTime = Model.Date.HasValue ? Model.Date.Value.TimeOfDay : new TimeSpan(),
                Notes = Model.Notes,
                ProjectStageID = Model.StageID,
                ProjectPayTypeID = Model.PaytypeID
            };

            newTimeIt.ProjectCostCodeID = db.GetProjectStagesByProjectID(Model.ProjectID).ToList().Where(p=>p.ProjectStageID == newTimeIt.ProjectStageID).Select(p=>p.ProjectCostCodeID).FirstOrDefault();

            if (newTimeIt.TimeItID == 0)
            {
                newTimeIt.StatusID = repo.GetDefaultActivityStatusId(siteusercompanyid);
            }

            int timeItID = repo.SaveTimeIt(newTimeIt);
            if (timeItID > 0)
            {
                return timeItID;
            }
            else
            {
                return 0;
            }
        }


        public ActionResult Edit(int? id, int? contactid, int? projectid)
        {
            if (!contactid.HasValue && projectid.HasValue)
            {
                contactid = new BOL.Repository.CommonRepository().GetContactIdByProjectID(projectid.Value);
            }

            ViewBag.Jobs = new SelectList(db.GetJobsByContactID_WithLockFalse(contactid), "ViewID", "Project");

            var siteUsers = db.SiteUsers.Where(p => p.SiteCoID == siteusercompanyid).OrderBy(p => p.UserDisplayName);
            ViewBag.Resource = new SelectList(siteUsers, "SiteUserID", "UserDisplayName");

            TimeItModels.NewTimeIt model = new TimeItModels.NewTimeIt();
            ProjectTimeIts timeIt = db.ProjectTimeIts.Where(p => p.TimeItID == (id ?? 0)).FirstOrDefault();
            if (timeIt != null)
            {
                model = new TimeItModels.NewTimeIt()
                {
                    TimeItID = timeIt.TimeItID,
                    Date = timeIt.TimeItDate.HasValue ? timeIt.TimeItDate.Value.Add(timeIt.TimeItTime ?? new TimeSpan()) : timeIt.TimeItDate,
                    Hours = timeIt.Hours,
                    Notes = timeIt.Notes,
                    PaytypeID = timeIt.ProjectPayTypeID,
                    ProjectID = timeIt.ProjectID,
                    ResourceID = timeIt.SiteUserID,
                    StageID = timeIt.ProjectStageID,
                    Task = timeIt.Subject
                };
            }

            var projectData = FetchProjectData(timeIt.ProjectID ?? 0);
            ViewBag.Stages = new SelectList(projectData.Item1.Select(p => new { p.ProjectStageID, p.StageName }), "ProjectStageID", "StageName", model.StageID);
            ViewBag.PayTypes = new SelectList(projectData.Item2.Select(p => new { p.ViewID, p.Pay_Type }), "ViewID", "Pay_Type", model.PaytypeID);

            return View("_Edit", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(TimeItModels.NewTimeIt Model)
        {
            var errorList = new List<string>();

            if (ModelState.IsValid)
            {
                var newTimeItID = SaveTimeIt(Model);
                if (newTimeItID > 0)
                {
                    return Json(new { status = "success", timeItID = newTimeItID });
                }
                else
                {
                    errorList.Add("TimeIt couldn't be updated. Please retry.");
                }
            }

            errorList.AddRange((from item in ModelState.Values
                                from error in item.Errors
                                select error.ErrorMessage).ToList()
                             );

            return Json(new { status = "error", errors = errorList });
        }

        public ActionResult UpdateTimeItNotes(int TimeItID, string Note)
        {
            var repo = new BOL.Repository.CommonRepository();
            bool res = repo.UpdateTimeItNotes(TimeItID, siteusercompanyid, Note);

            return Json(new { result = res }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetProjectData(int Id/*projectid*/)
        {
            var data = FetchProjectData(Id);

            var res = new
            {
                stages = data.Item1.Select(p => new { Id = p.ProjectStageID, Name = p.StageName }),
                payTypes = data.Item2.Select(p => new { Id = p.PayTypeID, Name = p.Pay_Type })
            };

            return Json(res, JsonRequestBehavior.AllowGet);
        }


        [NonAction]
        private Tuple<List<GetProjectStagesByProjectID_Result>, List<GetProjectPayTypesByProjectID_Result>> FetchProjectData(int ProjectID)
        {
            var stages = db.GetProjectStagesByProjectID(ProjectID).ToList();
            var payTypes = db.GetProjectPayTypesByProjectID(ProjectID).ToList();

            var res = Tuple.Create(
                stages,
                payTypes
            );

            return res;
        }

        //[NonAction]
        //private Tuple<List<GetProjectStagesByProjectID_Result>, List<GetPayTypesByProjectID_Result>> FetchProjectData(int ProjectID)
        //{
        //    var stages = db.GetProjectStagesByProjectID(ProjectID).ToList();
        //    var payTypes = db.GetPayTypesByProjectID(ProjectID).ToList();

        //    var res = Tuple.Create(
        //        stages,
        //        payTypes
        //    );

        //    return res;
        //}

    }
}
