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
    public class TransferInfoController : web.Controllers._baseMVCController
    {
        private ePontiv2Entities db = new ePontiv2Entities();
        BOL.Repository.CommonRepository repo = new BOL.Repository.CommonRepository();

        public ActionResult Create(int? porid, string PorItemIDs)
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

            var masterItemIDs =  repo.GetMasterItemIDsByProjectItemIDs(porItemIDsInt);

            TransferModels.NewTransfer model = new TransferModels.NewTransfer()
            {
                Date = DateTime.Now,
                PorID =(int)porid,
                TransferNumber = repo.GetNextTransferNumber(),
            };

            var porinfo = db.GetTransferToInfoByPorID(porid).FirstOrDefault();
            ViewBag.PorInfo = porinfo;
            ViewBag.ToItems = db.GetTransferToList(porid, string.Join(",", masterItemIDs)).ToList();
            ViewBag.Locations = new SelectList(db.GetCustodyLocationsBySiteCoID(siteusercompanyid).ToList(), nameof(GetCustodyLocationsBySiteCoID_Result.ViewID), nameof(GetCustodyLocationsBySiteCoID_Result.Name));
            ViewBag.LookupProject = new SelectList(db.GetTransferFromProjectList(porinfo.ProjectID, string.Join(",", masterItemIDs)).ToList(), nameof(GetTransferFromProjectList_Result.ProjectID), nameof(GetTransferFromProjectList_Result.Project));            
            return View(model);
        }

        public ActionResult GetTransferFromProjectList(int ProjectID, string ToMasterItemIDs)
        {
            var projects = db.GetTransferFromProjectList(ProjectID, ToMasterItemIDs).ToList();
            
            return Json(new { Projects = projects }, JsonRequestBehavior.AllowGet);
          //  return Json(new { status =  res) });
        }

        public ActionResult GetTransferFromItems(int ProjectID, string ToMasterItemIDs)
        {
            var items = db.GetTransferFromItems(ProjectID, ToMasterItemIDs).ToList();
            
            return Json(new { Items = items }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult CreateTransfer(TransferModels.NewTransfer Model)
        {
            var errorList = new List<string>();

            //This is needed to post to [ProcurementTransfers].[TransferTypeID] as it is required to determine if
            // the Transfer from Inventory or Exchange. Here the Inventory just removes the Items where the Exchange
            // replaces it in another POR when taking utilzing Stored Procedure - [GetTransferTypeByProjectID]
            //This also provide the Label too
           
            if (ModelState.IsValid)
            {
                int transferID = SaveTransfer(Model);
                if (transferID > 0)
                {
                    return Json(new { status = "success", jobid = transferID });
                }
                else
                {
                    errorList.Add("Transfer request couldn't be saved. Please retry.");
                }
            }

            errorList.AddRange((from item in ModelState.Values
                                from error in item.Errors
                                select error.ErrorMessage).ToList()
                             );

            return Json(new { status = "error", errors = errorList });
        }

        private int SaveTransfer(TransferModels.NewTransfer Model)
        {
            var transferId = repo.SaveTransferRequest(siteusercompanyid, siteuserid, Model.PorID, Model.TransferNumber, Model.Reason,
                Model.PorItemIDs, Model.FromProjectID, Model.FromProjectItemIDs, Model.ToProjectItemIDs);
            return transferId;
        }
    }
}