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

namespace ePonti.web.Areas.Options.Controllers
{
    [Authorize]
    public class ActivitiesOptionsController : ePonti.web.Controllers._baseMVCController
    {
        private ePontiv2Entities db = new ePontiv2Entities();
        BOL.Repository.CommonRepository repo = new BOL.Repository.CommonRepository();

        public ActionResult Index()
        {
            int siteCoID = base.siteusercompanyid;

            ViewBag.CoActivitiesStatus = db.GetActivitiesStatusBySiteCoID(siteCoID);
            ViewBag.CoCaseTypes = db.GetCaseTypesBySiteCoID(siteCoID);
            ViewBag.CoPurposes = db.GetPurposesListBySiteCoID(siteCoID);
            ViewBag.CoPunchListTypes = db.GetPunchListTypesBySiteCoID(siteCoID);
            ViewBag.CoPunchListDept = db.GetPunchListDeptBySiteCoID(siteCoID);
            ViewBag.CoMilestones = db.GetMilestonesBySiteCoID(siteCoID);
            ViewBag.CoMilestonesStatus = db.GetMilestonesStatusBySiteCoID(siteCoID);
            ViewBag.CoMilestonesTemplates = db.GetMilestonesTemplatesBySiteCoID(siteCoID);

            return View();
        }

        public ActionResult DeleteOption(int ItemID, string Type)
        {
            var res = repo.DeleteOption(Type, ItemID, siteusercompanyid);
            return Json(new { status = (res == "" ? "success" : res) });
        }

        #region Options Update
        public ActionResult UpdateActivities(int ItemID, string Name, int Order)
        {
            CoActivitiesStatus item;
            if (ItemID == 0)
            {
                item = new CoActivitiesStatus() { SiteCoID = siteusercompanyid };
                db.CoActivitiesStatus.Add(item);
            }
            else
            {
                item = db.CoActivitiesStatus.Where(p => p.StatusID == ItemID && p.SiteCoID == siteusercompanyid).FirstOrDefault();

            }

            if (item != null)
            {
                item.StatusName = Name;
                item.StatusOrder = Order;
            }
            db.SaveChanges();
            return Json(new { status = "success", itemID = item.StatusID });
        }

        public ActionResult UpdatePurposeList(int ItemID, string Name, int Order)
        {
            CoCallPurposes item;
            if (ItemID == 0)
            {
                item = new CoCallPurposes() { SiteCoID = siteusercompanyid };
                db.CoCallPurposes.Add(item);
            }
            else
            {
                item = db.CoCallPurposes.Where(p => p.CallPurposeID == ItemID && p.SiteCoID == siteusercompanyid).FirstOrDefault();
            }

            if (item != null)
            {
                item.PurposeName = Name;
                item.CallPurposeOrder = Order;
            }
            db.SaveChanges();
            return Json(new { status = "success", itemID = item.CallPurposeID });
        }

        public ActionResult UpdatePunchListType(int ItemID, string Name, int Order)
        {
            CoPunchListTypes item;
            if (ItemID == 0)
            {
                item = new CoPunchListTypes() { SiteCoID = siteusercompanyid };
                db.CoPunchListTypes.Add(item);
            }
            else
            {
                item = db.CoPunchListTypes.Where(p => p.PunchListTypeID == ItemID && p.SiteCoID == siteusercompanyid).FirstOrDefault();
            }

            if (item != null)
            {
                item.PunchListType = Name;
                item.PunchListTypeOrder = Order;
            }
            db.SaveChanges();
            return Json(new { status = "success", itemID = item.PunchListTypeID });
        }

        public ActionResult UpdatePurposeListDepartment(int ItemID, string Name, int Order)
        {
            CoPunchListDepartments item;
            if (ItemID == 0)
            {
                item = new CoPunchListDepartments() { SiteCoID = siteusercompanyid };
                db.CoPunchListDepartments.Add(item);
            }
            else
            {
                item = db.CoPunchListDepartments.Where(p => p.PunchItemDepartmentID == ItemID && p.SiteCoID == siteusercompanyid).FirstOrDefault();
            }

            if (item != null)
            {
                item.ItemDepartment = Name;
                item.ItemDepartmentOrder = Order;
            }
            db.SaveChanges();
            return Json(new { status = "success", itemID = item.PunchItemDepartmentID });
        }

        public ActionResult UpdateCaseType(int ItemID, string Name, int Order)
        {
            CoCaseTypes item;
            if (ItemID == 0)
            {
                item = new CoCaseTypes() { SiteCoID = siteusercompanyid };
                db.CoCaseTypes.Add(item);
            }
            else
            {
                item = db.CoCaseTypes.Where(p => p.CaseTypeID == ItemID && p.SiteCoID == siteusercompanyid).FirstOrDefault();
            }

            if (item != null)
            {
                item.CaseType = Name;
                item.CaseTypeOrder = Order;
            }
            db.SaveChanges();
            return Json(new { status = "success", itemID = item.CaseTypeID });
        }

        //public ActionResult UpdateMilestone(int ItemID, string Name, int Order)
        //{
        //    CoCaseTypes item;
        //    if (ItemID == 0)
        //    {
        //        item = new CoCaseTypes() { SiteCoID = siteusercompanyid };
        //        db.CoCaseTypes.Add(item);
        //    }
        //    else
        //    {
        //        item = db.CoCaseTypes.Where(p => p.CaseTypeID == ItemID && p.SiteCoID == siteusercompanyid).FirstOrDefault();
        //    }

        //    if (item != null)
        //    {
        //        item.CaseType = Name;
        //        item.CaseTypeOrder = Order;
        //    }
        //    db.SaveChanges();
        //    return Json(new { status = "success", itemID = item.CaseTypeID });
        //}

        public ActionResult UpdateMilestoneStatus(int ItemID, string Name, int Order)
        {
            CoMilestoneStatus item;
            if (ItemID == 0)
            {
                item = new CoMilestoneStatus() { SiteCoID = siteusercompanyid };
                db.CoMilestoneStatus.Add(item);
            }
            else
            {
                item = db.CoMilestoneStatus.Where(p => p.MasterMilestoneStatusID == ItemID && p.SiteCoID == siteusercompanyid).FirstOrDefault();
            }

            if (item != null)
            {
                item.MasterMilestoneStatusName = Name;
                item.MasterMilestoneStatusOrder = Order;
            }
            db.SaveChanges();
            return Json(new { status = "success", itemID = item.MasterMilestoneStatusID });
        }

        public ActionResult UpdateMilestoneTemplate(int ItemID, string Name, string Description)
        {
            CoMilestoneTemplates item;
            if (ItemID == 0)
            {
                item = new CoMilestoneTemplates() { SiteCoID = siteusercompanyid };
                db.CoMilestoneTemplates.Add(item);
            }
            else
            {
                item = db.CoMilestoneTemplates.Where(p => p.MilestoneTemplateID == ItemID && p.SiteCoID == siteusercompanyid).FirstOrDefault();
            }

            if (item != null)
            {
                item.MilestoneTemplateName = Name;
                item.MilestoneDescription = Description;
            }
            db.SaveChanges();
            return Json(new { status = "success", itemID = item.MilestoneTemplateID });
        }


        #endregion


    }
}