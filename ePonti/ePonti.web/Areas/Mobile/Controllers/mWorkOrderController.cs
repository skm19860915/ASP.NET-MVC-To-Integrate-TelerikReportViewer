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
using ePonti.BLL.Common;
using ePonti.BOL.Repository;
using ePonti.BOL.Models;

namespace ePonti.web.Areas.Mobile.Controllers
{
    public class mWorkOrderController : web.Controllers._baseMVCController
    {
        private ePontiv2Entities db = new ePontiv2Entities();
        BOL.Repository.CommonRepository repo = new BOL.Repository.CommonRepository();

        #region Assign
        // GET: Common/WorkOrders/Create
        public ActionResult Assign(string Items, int WOType = 1)
        {
            var itemIDs = (Items ?? "").Split(',').ToList();
            if (!itemIDs.Any())
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            WorkOrderModels.AssignWorkOrder model = new WorkOrderModels.AssignWorkOrder()
            {
                EstimatedHours = repo.GetWorkOrderEstHours(itemIDs.Select(p => Convert.ToInt32(p)), (EnumWrapper.WorkOrderTypes)WOType)
            };

            ViewBag.WOType = WOType;
            ViewBag.Status = new SelectList(repo.GetWorkOrderStatus(siteusercompanyid), nameof(GetCoWoStatusBySiteCoID_Result.ViewID), nameof(GetCoWoStatusBySiteCoID_Result.Status));
            ViewBag.Resources = db.GetCoUsersBySiteCoID(siteusercompanyid).ToList();

            return View(model);
        }

        [HttpPost]
        public ActionResult Assign(WorkOrderModels.AssignWorkOrder Model)
        {
            var errorList = new List<string>();

            if (ModelState.IsValid)
            {
                bool status = SaveWorkOrderAssign(Model);
                if (status)
                {
                    return Json(new { status = "success" });
                }
                else
                {
                    errorList.Add("Work Order couldn't be saved. Please retry.");
                }
            }

            errorList.AddRange((from item in ModelState.Values
                                from error in item.Errors
                                select error.ErrorMessage).ToList()
                             );

            return Json(new { status = "error", errors = errorList });
        }

        public JsonResult getFilteredAssignEvents(List<int> SiteUserIdArray)
        {
            List<CalendarActivities> dashboardActivities = new List<CalendarActivities>();
            foreach (int id in SiteUserIdArray)
            {
                var ActivitiesList = db.GetCalendarBySiteUserID(id).ToList();
                string colorcode = db.SiteUserOptions.Where(p => p.SiteUserID == id).Select(p => p.ColorCode).FirstOrDefault();
                foreach (var q in ActivitiesList)
                {
                    CalendarActivities da = new CalendarActivities();
                    da.id = Convert.ToString(q.ViewID);
                    da.title = q.Title;
                    da.start = q.DateTime;
                    DateTime ed = Convert.ToDateTime(q.DateTime);
                    for (decimal i = 0.25M; i <= q.Duration; i = i + (decimal)0.25)
                    {
                        ed = ed.AddMinutes(15);
                    }
                    da.end = ed.ToString();
                    da.color = colorcode;
                    da.description = q.ActivityType;
                    dashboardActivities.Add(da);
                }
            }
            var rows = dashboardActivities.ToArray();
            return Json(rows, JsonRequestBehavior.AllowGet);
        }

        private bool SaveWorkOrderAssign(WorkOrderModels.AssignWorkOrder Model)
        {
            if (Model.Resources.Count == 0)
            {
                return false;
            }

            var itemIDs = new List<int>();
            try
            {
                itemIDs = Model.Items.Split(',').ToList().Select(p => Convert.ToInt32(p)).ToList();
            }
            catch (Exception ex)
            {
                LogRepository.LogException(ex);
            }

            //SiteUserID | List of dates for this user
            List<Tuple<int, List<DateTime>>> resources = new List<Tuple<int, List<DateTime>>>();
            foreach (var resource in Model.Resources)
            {
                List<DateTime> dates = new List<DateTime>();
                try
                {
                    dates = resource.Dates.Split(',').Select(p => DateTime.Parse(p)).ToList();
                }
                catch (Exception ex)
                {
                    LogRepository.LogException(ex);
                }
                resources.Add(Tuple.Create(resource.SiteUserID, dates));
            }

            bool status = repo.SaveWorkOrderAssign(siteuserid, Model.ProjectID, itemIDs, Model.StatusID, Model.WorkOrderTypeID, resources);
            return status;
        }

        #endregion

        public ActionResult Edit(int? id)//WoID
        {
            ProjectWorkOrders wo = db.ProjectWorkOrders.Where(p => p.WoID == id).FirstOrDefault();
            if (wo == null)
            {
                return HttpNotFound();
            }

            var projectInfo = db.ProjectInfo.Where(p => p.ProjectID == wo.ProjectID).FirstOrDefault();
            if (projectInfo == null)
            {
                return HttpNotFound();
            }

            var model = new WorkOrderModels.EditWorkOrder()
            {
                WoID = wo.WoID,
                WorkOrderNumber = wo.WoNumber,
                ResourceID = wo.SiteUserID,
                JobName = projectInfo.ProjectName,
                Phone = projectInfo.ProjectPhone,
                Address1 = projectInfo.ProjectAddress1,
                Address2 = projectInfo.ProjectAddress2,
                City = projectInfo.ProjectCity,
                State = projectInfo.ProjectState,
                Zip = projectInfo.ProjectZip,
                Country = projectInfo.ProjectCountry,
                StatusID = wo.WoStatusID,
                Assigned = wo.WoDate,
                EstimatedHours = wo.AllottedTime
            };

            ViewBag.Resources = new SelectList(db.GetSiteUsersBySiteCoID(siteusercompanyid).ToList(), nameof(GetSiteUsersBySiteCoID_Result.ViewID), nameof(GetSiteUsersBySiteCoID_Result.User), model.ResourceID);
            ViewBag.Status = new SelectList(repo.GetWorkOrderStatus(siteusercompanyid), nameof(GetCoWoStatusBySiteCoID_Result.ViewID), nameof(GetCoWoStatusBySiteCoID_Result.Status), model.StatusID);

            return View("_Edit", model);
        }

        [HttpPost]
        public ActionResult Edit(WorkOrderModels.EditWorkOrder Model)
        {
            var errorList = new List<string>();

            if (ModelState.IsValid)
            {
                bool status = repo.UpdateWorkOrder(Model.WoID, Model.ResourceID, Model.StatusID, Model.Assigned, Model.EstimatedHours);
                if (status)
                {
                    return Json(new { status = "success" });
                }
                else
                {
                    errorList.Add("Work Order couldn't be updated. Please retry.");
                }
            }

            errorList.AddRange((from item in ModelState.Values
                                from error in item.Errors
                                select error.ErrorMessage).ToList()
                             );

            return Json(new { status = "error", errors = errorList });
        }

        public ActionResult Details(int? id)//WoID
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var wo = db.GetWorkOrderInfoByWoID(id).FirstOrDefault();
            var woTypeID = db.ProjectWorkOrders.Where(p => p.WoID == id).Select(p => p.WoTypeID).FirstOrDefault() ?? ((int)EnumWrapper.WorkOrderTypes.Scheduled);

            ViewBag.ProjectID = db.ProjectWorkOrders.Where(p => p.WoID == id).Select(p => p.ProjectID).FirstOrDefault() ?? 0;
            ViewBag.WOTypeID = woTypeID;

            if (woTypeID == (int)EnumWrapper.WorkOrderTypes.Punchlist)
            {
                ViewBag.Tasks = db.GetWorkOrderPunchListTasksByWoID(id).ToList();
            }
            else
            {
                ViewBag.Tasks = db.GetWorkOrderTasksByWoID(id).ToList();
            }

            if (wo == null)
            {
                return HttpNotFound();
            }
            return View(wo);
        }

        [HttpGet]
        public ActionResult AddItem(int? id /*woid*/)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                ViewBag.Items = db.GetSelectItemsByWoID(id).ToList();
                ViewBag.WoID = id;

                return View();
            }
            catch (Exception ex)
            {
                LogRepository.LogException(ex);
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost]
        public ActionResult AddItem(int WoID, List<int> Items)
        {
            var errorList = new List<string>();

            if (Items.Any())
            {
                bool status = repo.AddWorkOrderItems(WoID, Items);
                if (status)
                {
                    return Json(new { status = "success" });
                }
                else
                {
                    errorList.Add("Items couldn't be added to work order. Please retry.");
                }
            }
            else
            {
                errorList.Add("Select at least one item to add to work order");
            }

            errorList.AddRange((from item in ModelState.Values
                                from error in item.Errors
                                select error.ErrorMessage).ToList()
                             );

            return Json(new { status = "error", errors = errorList });
        }

        [HttpPost]
        public ActionResult UpdateActuals(int WoID, List<int> AssignmentIDs, DateTime? ActualDate, decimal? ActualHours, double? PercentCompleted, int WoTypeID = (int)EnumWrapper.WorkOrderTypes.Scheduled)
        {
            var errorList = new List<string>();
            if (AssignmentIDs != null)
            {
                bool status = repo.UpdateWorkOrderActuals(siteusercompanyid, AssignmentIDs, ActualDate, ActualHours, PercentCompleted, (EnumWrapper.WorkOrderTypes)WoTypeID);
                //bool status =rep
                if (status)
                {
                    if (WoTypeID == (int)EnumWrapper.WorkOrderTypes.Punchlist)
                    {
                        var tasks = db.GetWorkOrderPunchListTasksByWoID(WoID).ToList();
                        return Json(new { status = "success", tasks });
                    }
                    else
                    {
                        var tasks = db.GetWorkOrderTasksByWoID(WoID).ToList();
                        return Json(new { status = "success", tasks });
                    }
                }
                else
                {
                    errorList.Add("Some error occurred. Please retry.");
                }
            }
            else
            {
                errorList.Add("Please select a Task from the list before saving");
            }

            errorList.AddRange((from item in ModelState.Values
                                from error in item.Errors
                                select error.ErrorMessage).ToList()
                             );

            return Json(new { status = "error", errors = errorList });
        }

        [HttpPost]
        public ActionResult RemoveTasks(int WoID, List<int> AssignmentIDs)
        {
            var errorList = new List<string>();

            bool status = repo.RemoveWorkOrderTasks(WoID, AssignmentIDs);
            if (status)
            {
                var tasks = db.GetWorkOrderTasksByWoID(WoID).ToList();
                return Json(new { status = "success", tasks });
            }
            else
            {
                errorList.Add("Some error occurred. Please retry.");
            }

            errorList.AddRange((from item in ModelState.Values
                                from error in item.Errors
                                select error.ErrorMessage).ToList()
                             );

            return Json(new { status = "error", errors = errorList });
        }

        public ActionResult ItemInfo(int? id)//ItemID
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var woItem = db.GetWorkOrderItemInfoByItemID(id).FirstOrDefault();
            ViewBag.WoItem = woItem;
            ViewBag.Assignments = db.GetItemInfoAssignmentsByItemID(id).ToList();

            if (woItem == null)
            {
                return HttpNotFound();
            }

            return View();
        }

    }
}
