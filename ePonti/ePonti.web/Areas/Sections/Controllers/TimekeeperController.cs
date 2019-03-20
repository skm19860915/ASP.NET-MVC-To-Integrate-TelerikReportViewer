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
using ePonti.BOL.Models;

namespace ePonti.web.Areas.Sections.Controllers
{
    [Authorize]
    public class TimekeeperController : ePonti.web.Controllers._baseMVCController
    {
        private ePontiv2Entities db = new ePontiv2Entities();

        // GET: Sections/Timekeeper
        public ActionResult Index()
        {
            int siteCoID = base.siteusercompanyid;
            int siteUserID = base.siteuserid;

            //this will show/hide various areas on the view
            ViewBag.Security = db.GetUserSecurityBySiteUserID(siteUserID).FirstOrDefault();

            //This displays the list of Pay Periods displayed in the Pay Period combo box
            BindpayPeriods(siteCoID);
            customResource();
            //This is utilized when creating a TimeIt Record off the Time Sheet view
            ViewBag.projects = new SelectList(db.GetTimeItProjectsBySiteCoID(siteCoID).ToList(), nameof(GetTimeItProjectsBySiteCoID_Result.ViewID), nameof(GetTimeItProjectsBySiteCoID_Result.Project));

            //This display the list of Projects on the Projects combo box 
            ViewBag.PayProjectList = new SelectList(db.GetTimeProjectsListBySiteCoID(siteCoID).ToList(), nameof(GetTimeProjectsListBySiteCoID_Result.ProjectID), nameof(GetTimeProjectsListBySiteCoID_Result.Job));

            return View();
        }

        private void BindpayPeriods(int siteCoID)
        {
            ViewBag.PayPeriods = new SelectList(db.GetTimekeeperPayPeriodsBySiteCoID(siteCoID).ToList(), nameof(GetTimekeeperPayPeriodsBySiteCoID_Result.ViewID), nameof(GetTimekeeperPayPeriodsBySiteCoID_Result.Label));
            ViewBag.PayPeriodsProjects = new SelectList(db.GetTimekeeperPayPeriodsBySiteCoID(siteCoID).ToList(), nameof(GetTimekeeperPayPeriodsBySiteCoID_Result.ViewID), nameof(GetTimekeeperPayPeriodsBySiteCoID_Result.Label));
            ViewBag.PayPeriodsEmployees = new SelectList(db.GetTimekeeperPayPeriodsBySiteCoID(siteCoID).ToList(), nameof(GetTimekeeperPayPeriodsBySiteCoID_Result.ViewID), nameof(GetTimekeeperPayPeriodsBySiteCoID_Result.Label));
        }

        private void customResource()
        {
            List<CustomResourceDrp> crdlist = new List<CustomResourceDrp>();
            CustomResourceDrp crd = new CustomResourceDrp();
            crd.ViewID = base.siteuserid;
            crd.Resource = base.displayusername;
            crdlist.Add(crd);
            ViewBag.PayResources = new SelectList(crdlist, "ViewID", "Resource", base.siteuserid);
        }

        #region PayItemInfo
        public ActionResult PayItemInfo(string tab,int? id, int? projectid, int? PayrollID, int? PayrollTypeID,int? PayrollPeriodItemID)
        {
            int? viewID = id;
            ViewBag.tab = tab;
            //These will be based on what Type these are
            var q = db.GetPayrollItemInfoByViewID(viewID).FirstOrDefault();
            PayrollInfo pinfo = new PayrollInfo();
            pinfo.ActivityType = q.ActivityType;
            pinfo.CostCode = q.CostCode;
            pinfo.Date = q.Date;
            pinfo.Hours = q.Hours;
            pinfo.Job = q.Job;
            pinfo.PayrollPeriodItemID = PayrollPeriodItemID;            
            pinfo.PayrollTypeID = PayrollTypeID;
            pinfo.PayType = q.PayType;
            pinfo.ProjectCostCodeID = q.ProjectCostCodeID;
            pinfo.ProjectID = q.ProjectID;
            pinfo.ProjectPayTypeID = q.ProjectPayTypeID;
            pinfo.Resource = q.Resource;
            pinfo.Task = q.Task;
            pinfo.ViewID = q.ViewID;
            pinfo.UserID = q.UserID;
            ViewBag.ViewID = q;
            ViewBag.CostCode = new SelectList(db.GetProjectCostCodesByProjectID(q.ProjectID).ToList(), nameof(GetProjectCostCodesByProjectID_Result.ViewID), nameof(GetProjectCostCodesByProjectID_Result.CostCode));
            ViewBag.PayTypes = new SelectList(db.GetProjectPayTypesByProjectID(q.ProjectID).ToList(), nameof(GetProjectPayTypesByProjectID_Result.ViewID), nameof(GetProjectPayTypesByProjectID_Result.Pay_Type));
            ViewBag.PayReasonInfo = db.GetPayReasonInfo(PayrollID).ToList();
            ViewBag.PayrollID = PayrollID;
            return View(pinfo);
        }
        [HttpPost]
        public ActionResult SavePayInfo(PayrollInfo data)
        {
            var q = db.Payroll.Where(p => p.PayrollEventID == data.ViewID).FirstOrDefault();
            if (q != null)
            {
                db.UpdatePayrollItem(q.PayrollID, data.ProjectCostCodeID, data.ProjectPayTypeID, data.Hours);
                db.SaveChanges();
                return Json(new { status = "success" });
            }
            else
            {
                var model = db.GetPayrollItemInfoByViewID(data.ViewID).FirstOrDefault();
                db.InsertPayrollItem(data.PayrollPeriodItemID, data.PayrollTypeID, data.ViewID, data.ProjectCostCodeID, data.ProjectPayTypeID, data.Hours);
                db.SaveChanges();
                return Json(new { status = "success" });
            }            
        }
        #endregion

        #region PayReasonInfo
        public ActionResult PayReasonInfo(int? id)
        {
            int? viewID = id;

            ViewBag.PayReasonInfo = db.GetPayReasonInfo(viewID).FirstOrDefault();

            return View();
        }
        [HttpPost]
        public ActionResult SaveReasonInfo(InsertRejectionInfo data)
        {
            if (data.PayrollID != 0)
            {               
                    db.InsertPayItemRejected(data.PayrollID, base.siteuserid, data.PayrollReasonNote);
                    db.SaveChanges();               
                return Json(new { status = "success" });
            }
            return Json(new { status = "error" });

        }
        #endregion

        #region Timekeeper
        [HttpPost]
        public ActionResult Index(int? projectId,int? PayrollPeriodItemIDEmp,int? PayrollPeriodItemID,int? PayrollPeriodItemIDProject, int? ResourceID)
        {
            int siteCoID = base.siteusercompanyid;
            int siteUserID = base.siteuserid;
            customResource();
            ViewBag.Security = db.GetUserSecurityBySiteUserID(siteUserID).FirstOrDefault();
            ViewBag.projects = new SelectList(db.GetProjectsBySiteCoID(siteCoID).ToList(), nameof(GetProjectsBySiteCoID_Result.ViewID), nameof(GetProjectsBySiteCoID_Result.Project));
            ViewBag.PayProjectList = new SelectList(db.GetTimeProjectsListBySiteCoID(siteCoID).ToList(), nameof(GetTimeProjectsListBySiteCoID_Result.ProjectID), nameof(GetTimeProjectsListBySiteCoID_Result.Job));
            BindpayPeriods(siteCoID);
            if (PayrollPeriodItemID != null)
            {          
                var q = db.GetTimekeeperPayPeriodsBySiteCoID(siteCoID).Where(p => p.ViewID == PayrollPeriodItemID).FirstOrDefault();
                var timesheet = db.GetTimeSheetBySiteUserIDnDates(ResourceID, q.PayStart, q.PayEnd).ToList();
                ViewBag.TimeSheet = timesheet;
                if(Convert.ToBoolean(Session["IsAdmin"]) == true)
                ViewBag.PayResources = new SelectList(db.GetTimeEmployeesListBySiteCoID(siteCoID, q.PayStart, q.PayEnd).ToList(), nameof(GetTimeEmployeesListBySiteCoID_Result.ViewID), nameof(GetTimeEmployeesListBySiteCoID_Result.Resource), ResourceID);
                ViewBag.PayPeriods = new SelectList(db.GetTimekeeperPayPeriodsBySiteCoID(siteCoID).ToList(), nameof(GetTimekeeperPayPeriodsBySiteCoID_Result.ViewID), nameof(GetTimekeeperPayPeriodsBySiteCoID_Result.Label), PayrollPeriodItemID);
                return PartialView("_Timesheet", timesheet);
            }
            if(projectId!=null && PayrollPeriodItemIDProject != null)
            {
                var PayProjects = db.GetTimeProjectsBySiteCoIDnDates(siteCoID, PayrollPeriodItemIDProject).ToList();
                PayProjects= PayProjects.Where(p => p.ProjectID == projectId).ToList();
                ViewBag.PayProjects = PayProjects.Where(p => p.ProjectID == projectId).ToList();
                ViewBag.PayPeriodsProjects = new SelectList(db.GetTimekeeperPayPeriodsBySiteCoID(siteCoID).ToList(), nameof(GetTimekeeperPayPeriodsBySiteCoID_Result.ViewID), nameof(GetTimekeeperPayPeriodsBySiteCoID_Result.Label), PayrollPeriodItemIDProject);
                return PartialView("_Projects", PayProjects);
            }
            if(PayrollPeriodItemIDProject != null)
            {
                var PayProjects = db.GetTimeProjectsBySiteCoIDnDates(siteCoID, PayrollPeriodItemIDProject).ToList();
                ViewBag.PayProjects = PayProjects;
                ViewBag.PayPeriodsProjects = new SelectList(db.GetTimekeeperPayPeriodsBySiteCoID(siteCoID).ToList(), nameof(GetTimekeeperPayPeriodsBySiteCoID_Result.ViewID), nameof(GetTimekeeperPayPeriodsBySiteCoID_Result.Label), PayrollPeriodItemIDProject);
                return PartialView("_Projects", PayProjects);
            }
            if (PayrollPeriodItemIDEmp != null)
            {
                var PayEmployees = db.GetTimeEmployeesBySiteCoIDnDates(siteCoID, PayrollPeriodItemIDEmp).ToList();
                ViewBag.PayEmployees = PayEmployees;                
                ViewBag.PayPeriodsEmployees = new SelectList(db.GetTimekeeperPayPeriodsBySiteCoID(siteCoID).ToList(), nameof(GetTimekeeperPayPeriodsBySiteCoID_Result.ViewID), nameof(GetTimekeeperPayPeriodsBySiteCoID_Result.Label), PayrollPeriodItemIDEmp);
                return PartialView("_Employees", PayEmployees);
            }

            return View();
        }

        public ActionResult _Timesheet()
        {
            return PartialView("_Timesheet");
        }
        public ActionResult _Projects()
        {
            return PartialView("_Projects");
        }
        public ActionResult _Employees()
        {
            return PartialView("_Employees");
        }

        public ActionResult getresources(int? PayrollPeriodItemID)
        {
            int siteCoID = base.siteusercompanyid;
            int siteUserID = base.siteuserid;
            //This displays the list of Resources on the Resources combo box - by default needs to display Logged In User            
            var q = db.GetTimekeeperPayPeriodsBySiteCoID(siteCoID).Where(p => p.ViewID == PayrollPeriodItemID).FirstOrDefault();
            //if (Convert.ToBoolean(Session["IsAdmin"]) == true)
            //{
            //    var Resources = db.GetTimeEmployeesListBySiteCoID(siteCoID, q.PayStart, q.PayEnd).ToList();
            //    return Json(new { Resources = Resources }, JsonRequestBehavior.AllowGet);
            //}
            //else
            //{
                List<CustomResourceDrp> crdlist = new List<CustomResourceDrp>();
                CustomResourceDrp crd = new CustomResourceDrp();
                crd.ViewID = base.siteuserid;
                crd.Resource = base.displayusername;
                crdlist.Add(crd);
                return Json(new { Resources = crdlist }, JsonRequestBehavior.AllowGet);
           // }
        }
       public ActionResult gettimekeeper(int? PayrollPeriodItemID,int? ResourceID)
        {
            int siteCoID = base.siteusercompanyid;
            int siteUserID = base.siteuserid;
            //This displays the list of Resources on the Resources combo box - by default needs to display Logged In User            
            var q = db.GetTimekeeperPayPeriodsBySiteCoID(siteCoID).Where(p => p.ViewID == PayrollPeriodItemID).FirstOrDefault();                     
            ViewBag.TimeSheet = db.GetTimeSheetBySiteUserIDnDates(ResourceID, q.PayStart, q.PayEnd).ToList();
            ViewBag.Security = db.GetUserSecurityBySiteUserID(siteUserID).FirstOrDefault();
            ViewBag.projects = new SelectList(db.GetProjectsBySiteCoID(siteCoID).ToList(), nameof(GetProjectsBySiteCoID_Result.ViewID), nameof(GetProjectsBySiteCoID_Result.Project));
            ViewBag.PayProjectList = new SelectList(db.GetTimeProjectsListBySiteCoID(siteCoID).ToList(), nameof(GetTimeProjectsListBySiteCoID_Result.ProjectID), nameof(GetTimeProjectsListBySiteCoID_Result.Job));
            ViewBag.PayPeriods = new SelectList(db.GetTimekeeperPayPeriodsBySiteCoID(siteCoID).ToList(), nameof(GetTimekeeperPayPeriodsBySiteCoID_Result.ViewID), nameof(GetTimekeeperPayPeriodsBySiteCoID_Result.Label),PayrollPeriodItemID);
            ViewBag.PayResources = new SelectList(db.GetTimeEmployeesListBySiteCoID(siteCoID, q.PayStart, q.PayEnd).ToList(), nameof(GetTimeEmployeesListBySiteCoID_Result.ViewID), nameof(GetTimeEmployeesListBySiteCoID_Result.Resource), ResourceID);
            return Json(new { status = "success" }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult InsertPaySubmit(List<TimeKeeperModel> Data)
        {
            foreach (var data in Data)
            {
                db.UpdateTimeSheetSubmit(data.PayrollID, base.siteuserid, data.SubmittedHours, data.SubmitSigned);
                db.SaveChanges();
            }               
            return Json(new { res = "success" }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Projects
        public ActionResult PayProjects(int? id, int? ab)
        {
            int? payrollPeriodItemID = id;
            int? projectID = ab;

            ViewBag.PayProjects = db.GetTimeProjectsBySiteCoIDnDates(payrollPeriodItemID, projectID);

            return View();
        }
        [HttpPost]
        public ActionResult InsertTimeProjectApprove(List<UpdateTimeKeeper> Data)
        {
            foreach (var data in Data)
            {
                db.UpdateTimekeeperProject(data.PayrollID, base.siteuserid);
                db.SaveChanges();
            }
            return Json(new { status = "success" });
        }
        #endregion

        #region Employees
        public ActionResult PayEmployees(int? id, int? ab)
        {
            int? siteCoID = id;
            int? payrollPeriodItemID = ab;         
            ViewBag.PayEmployees = db.GetTimeEmployeesBySiteCoIDnDates(siteCoID, payrollPeriodItemID);

            return View();
        }
        [HttpPost]
        public ActionResult InsertTimeEmployeeApprove(List<UpdateTimeKeeper> Data)
        {        
            foreach (var q in Data)
            {
                var info = db.GetTimeEmployeeInfo(q.ViewID, q.ItemPayPeriodId).ToList();
                foreach (var data in info)
                {
                    db.UpdatePayItemEmployeeApprove(base.siteuserid, data.PayrollID);
                    db.SaveChanges();
                }
            }
            return Json(new { status = "success" });
        }
        #endregion

        #region Employee Info
        //This opens to the Employee Info view
        public ActionResult PayEmployeeInfo(int? id, int? ab,string tab)
        {
            int? ViewID = id;
            int? payrollPeriodID = ab;
            ViewBag.tab = tab;
            ViewBag.pid = ab;
            ViewBag.Resource = db.GetTimeEmployeeInfo(ViewID, payrollPeriodID).Select(p=>p.Resource).FirstOrDefault();
            ViewBag.PayEmployeeInfo = db.GetTimeEmployeeInfo(ViewID, payrollPeriodID).ToList();
            return View();
        }
        #endregion
    }

}
