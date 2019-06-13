using ePonti.BOL;
using ePonti.BOL.Repository;
using ePonti.web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ePonti.web.Areas.Procurement.Controllers
{
    public class ReturnInfoController : web.Controllers._baseMVCController
    {
        private ePontiv2Entities db = new ePontiv2Entities();
        CommonRepository repo = new CommonRepository();

        //Return View - to create
        public ActionResult Create(int porid, int projectID, string PorItemIDs)
        {
            var porItemIDsInt = new List<int>();
            try
            {
                porItemIDsInt = PorItemIDs.Split(',').Select(p => Convert.ToInt32(p)).ToList();
            }
            catch (Exception ex)
            {
                LogRepository.LogException(ex);
            }

            var masterItemIDs = repo.GetMasterItemIDsByProjectItemIDs(porItemIDsInt);
            ReturnModels.NewReturn model = new ReturnModels.NewReturn()
            {
                Date = DateTime.Now,
                FromProjectID = (int)projectID,
                ReturnNumber = repo.GetNextReturnNumber()
            };

            ViewBag.FromProjects = db.GetProjectsWithPendingItems(projectID).ToList();//new SelectList(db.GetProjectsWithPendingItems(projectID).ToList(), "ProjectID", "Project");
            TempData["FromItemIDs"] = 
            ViewBag.fromItems = db.GetPendingItemsByProjectID(projectID).Where(s => masterItemIDs.Contains(s.ViewID)).ToList();

            //ViewBag.ToItems = db.GetTransferToList(porid, string.Join(",", masterItemIDs)).ToList();
           // ViewBag.Locations = new SelectList(db.GetCustodyLocationsBySiteCoID(siteusercompanyid).ToList(), nameof(GetCustodyLocationsBySiteCoID_Result.ViewID), nameof(GetCustodyLocationsBySiteCoID_Result.Name));
           // ViewBag.LookupProject = new SelectList(db.GetTransferFromProjectList(projectID, string.Join(",", masterItemIDs)).ToList(), nameof(GetTransferFromProjectList_Result.ProjectID), nameof(GetTransferFromProjectList_Result.Project));
            return View(model);
        }

        //Returns the list of pending items based on the selected project
        public ActionResult GetPendingItems(int ProjectID)
        {
            var items = db.GetPendingItemsByProjectID(ProjectID).ToList();

            return Json(items, JsonRequestBehavior.AllowGet);
        }

        //Save the return request
        [HttpPost]
        public ActionResult CreateReturn(ReturnModels.NewReturn Model)
        {
            var errorList = new List<string>();

            if (ModelState.IsValid)
            {
                int transferID = SaveReturn(Model);
                if (transferID > 0)
                {
                    return Json(new { status = "success", jobid = transferID });
                }
                else
                {
                    errorList.Add("Return request couldn't be saved. Please retry.");
                }
            }

            errorList.AddRange((from item in ModelState.Values
                                from error in item.Errors
                                select error.ErrorMessage).ToList()
                             );

            return Json(new { status = "error", errors = errorList });
        }

        private int SaveReturn(ReturnModels.NewReturn Model)
        {
            var returnId = repo.SaveReturnRequest(siteusercompanyid, siteuserid, Model.FromProjectID, Model.ToProjectID,
                Model.Date, Model.ReturnNumber, Model.StatusID, Model.Subject, Model.Reason, 
                Model.FromItemIDs);

            return returnId;
        }


    }
}