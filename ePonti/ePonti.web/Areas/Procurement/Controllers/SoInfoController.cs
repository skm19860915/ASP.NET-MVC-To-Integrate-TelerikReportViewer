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

namespace ePonti.web.Areas.Procurement.Controllers
{
    public class SOInfoController : web.Controllers._baseMVCController
    {
        private ePontiv2Entities db = new ePontiv2Entities();
        BOL.Repository.CommonRepository repo = new BOL.Repository.CommonRepository();
        QBSyncService syncService = null;
        QBSyncdto syncObjects = null;
        QBModels qbModels = null;

        public ActionResult Details(int? id)//SOID
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var soInfo = db.GetSoInfoBySoID(id).FirstOrDefault();
            var so = db.ProjectSOs.Where(p => p.SoID == id).FirstOrDefault();
            if (soInfo == null || so == null)
            {
                return HttpNotFound();
            }
            if (TempData["SyncSuccessMessage"] != null)
            {
                ViewBag.SyncSuccessMessage = TempData["SyncSuccessMessage"];
                TempData.Remove("SyncSuccessMessage");
            }
            if (TempData["SyncErrorMessage"] != null)
            {
                ViewBag.SyncErrorMessage = TempData["SyncErrorMessage"];
                TempData.Remove("SyncErrorMessage");
            }
            ViewBag.Parts = db.GetSoInfoPartsBySoID(id).ToList();

            ViewBag.ProjectID = so.ProjectID;
            ViewBag.IsApproved = so.Approved;
            qbModels = new QBModels();
            qbModels.SyncObjectsModel = new QBSyncdto();
            qbModels.OAuthorizationModel = new QBAuthorizationdto();
            qbModels.IsReadySync = false;
            var oAuthModel = new QBOAuthService(qbModels.OAuthorizationModel).IsTokenAvailable(this, siteusercompanyid);
            if (oAuthModel.IsConnected)
            {
                ViewBag.IsQBConnected = true;
            }
            if (string.IsNullOrEmpty(so.AcctSoID))
            {
                ViewBag.IsNotSynced = true;
            }

            return View(soInfo);
        }

        public ActionResult Create(int? projectid)
        {
            var siteCoID = siteusercompanyid;

            ViewBag.Jobs = new SelectList(db.GetJobsBySiteCoID(siteCoID), nameof(GetJobsBySiteCoID_Result.ViewID), nameof(GetJobsBySiteCoID_Result.Project), projectid);
            ViewBag.Terms = new SelectList(db.GetSoTermsBySiteCoID(siteCoID), nameof(GetSoTermsBySiteCoID_Result.ViewID), nameof(GetSoTermsBySiteCoID_Result.Name));
            ViewBag.Status = new SelectList(db.GetSoStatusBySiteCoID(siteCoID), nameof(GetSoStatusBySiteCoID_Result.ViewID), nameof(GetSoStatusBySiteCoID_Result.Name));

            var model = new SOModels.NewSO()
            {
                Date = DateTime.Now,
                Creator = repo.GetUserDisplayName(siteuserid),
                SONumber = repo.GetNextSONumber()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SOModels.NewSO Model)
        {
            var errorList = new List<string>();

            if (ModelState.IsValid)
            {
                int corId = SaveCOR(Model);
                if (corId > 0)
                {
                    return Json(new { status = "success", CorID = corId });
                }
                else
                {
                    errorList.Add("Sales Order couldn't be saved. Please retry.");
                }
            }

            errorList.AddRange((from item in ModelState.Values
                                from error in item.Errors
                                select error.ErrorMessage).ToList()
                             );

            return Json(new { status = "error", errors = errorList });
        }

        private int SaveCOR(SOModels.NewSO Model)
        {
            int siteCoID = siteusercompanyid;
            var so = new ProjectSOs()
            {
                SoID = Model.SOID ?? 0,
                SiteCoID = siteusercompanyid,
                ProjectID = Model.JobID,
                SoName = Model.Name,
                SoNumber = Model.SONumber,
                SoDate = Model.Date,
                SoStatusID = Model.StatusID,
                SoTermID = Model.TermID,
                CreatedByUserID = siteuserid
            };

            var soId = repo.SaveSO(so);
            return soId;
        }

        public ActionResult Edit(int? id)//SoID
        {
            ProjectSOs so = db.ProjectSOs.Where(p => p.SoID == (id ?? 0)).FirstOrDefault();
            if (so == null)
            {
                return HttpNotFound();
            }

            var model = new SOModels.NewSO()
            {
                SOID = so.SoID,
                Name = so.SoName,
                SONumber = so.SoNumber,
                JobID = so.ProjectID,
                TermID = so.SoTermID,
                StatusID = so.SoStatusID,
                Date = so.SoDate,
                Creator = repo.GetUserDisplayName(so.CreatedByUserID ?? siteuserid)
            };

            int siteCoID = siteusercompanyid;
            ViewBag.Jobs = new SelectList(db.GetJobsBySiteCoID(siteCoID), nameof(GetJobsBySiteCoID_Result.ViewID), nameof(GetJobsBySiteCoID_Result.Project), model.JobID);
            ViewBag.Terms = new SelectList(db.GetSoTermsBySiteCoID(siteCoID), nameof(GetSoTermsBySiteCoID_Result.ViewID), nameof(GetSoTermsBySiteCoID_Result.Name), model.TermID);
            ViewBag.Status = new SelectList(db.GetSoStatusBySiteCoID(siteCoID), nameof(GetSoStatusBySiteCoID_Result.ViewID), nameof(GetSoStatusBySiteCoID_Result.Name), model.StatusID);

            return View("_Edit", model);
        }

        public ActionResult ApproveSO(int SOID, int ProjectID)
        {
            bool status = repo.ApproveSO(siteuserid, SOID, ProjectID);
            return Json(new { status = (status ? "success" : "error") });
        }

        [HttpGet]
        public ActionResult AddItems(int ID, int ProjectID)
        {
            ViewBag.SOID = ID;
            ViewBag.ProjectID = ProjectID;
            ViewBag.Action = "Added";

            ViewBag.Items = db.AddSoItemsByProjectID(ProjectID).ToList();
            return View("ListItems");
        }

        [HttpPost]
        public ActionResult AddItems(int SOID, int ProjectID, int Quantity, List<int> Items)
        {
            var res = new BOL.Repository.CommonRepository().AddSOItems(siteusercompanyid, SOID, ProjectID, Quantity, Items);
            return Json(new { status = (res ? "success" : "error") });
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        public ActionResult SyncSO(int SOID, string EstInvType, string ItemType = "NonInventory", int count = 1)
        {
            try
            {
                qbModels = new QBModels();
                int siteCoID = base.siteusercompanyid;
                if (SOID!=0)
                {
                    QBAuthorizationdto oAuthDetails = new QBOAuthService(new QBAuthorizationdto()).GetAccessToken(this);
                    syncService = new QBSyncService(oAuthDetails);
                    syncObjects = new QBSyncdto();
                    syncObjects.OauthToken = oAuthDetails;
                    syncObjects.CompanyId = oAuthDetails.Realmid;
                    syncObjects = syncService.SyncSalesOrder(syncObjects, siteCoID, SOID, EstInvType, ItemType);

                }
                TempData["SyncSuccessMessage"] = "Synchronization Process Completed";

                return RedirectToAction("details", new { id = SOID });
            }
            catch (Exception ex)
            {
                if (count < 5)
                {
                    return Redirect("SyncSO?SOID=" + SOID + "&EstInvType=" + EstInvType + "&ItemType=" + ItemType + "&count=" + (count + 1));
                }
                else
                {
                    //throw;
                    TempData["SyncErrorMessage"] = ex.Message;
                    return RedirectToAction("details", new { id = SOID });
                }
            }
        }
    }
}
