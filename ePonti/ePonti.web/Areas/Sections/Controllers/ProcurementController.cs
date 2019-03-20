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
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;

namespace ePonti.web.Areas.Sections.Controllers
{
    [Authorize]
    public class ProcurementController : ePonti.web.Controllers._baseMVCController
    {
        private ePontiv2Entities db = new ePontiv2Entities();
        QBSyncService syncService = null;
        QBSyncdto syncObjects = null;
        QBModels qbModels = null;
        // GET: Sections/Procurement
        public ActionResult Index()
        {
            int siteCoID = base.siteusercompanyid;
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
            ViewBag.VendorList = db.GetVendorsByPors(siteusercompanyid).Where(s => s.Vendor != null && s.Vendor.Trim() != "").ToList();
            ViewBag.JobList = db.GetProjectsByPors(siteusercompanyid).ToList();
            ViewBag.StatusList = db.GetStatusByPors(siteusercompanyid).ToList();
            ViewBag.SelectPORList = db.GetPorNumberByPors(siteCoID).ToList();

            ProcurementResult result = new ProcurementResult();
            result.PurchaseList = db.GetPurchasingBySiteCoID(siteCoID).ToList();
            result.CustodyList = db.GetCustodyBySiteCoID(siteCoID).ToList();
            result.DeliveryList = db.GetDeliveriesBySiteCoID(siteCoID).ToList();
            result.TransferList = db.GetTransfersBySiteCoID(siteCoID).ToList();
            result.PendingList = db.GetPendingBySiteCoID(siteCoID).ToList();
            result.InventoryList = db.GetInventoryBySiteCoID(siteCoID).ToList();
            qbModels = new QBModels();
            qbModels.SyncObjectsModel = new QBSyncdto();
            qbModels.OAuthorizationModel = new QBAuthorizationdto();
            qbModels.IsReadySync = false;
            var oAuthModel = new QBOAuthService(qbModels.OAuthorizationModel).IsTokenAvailable(this, siteusercompanyid);
            if (oAuthModel.IsConnected)
            {
                ViewBag.IsQBConnected = true;
            }
            return View(result);
        }
        // GET: Sections/Procurement
        [HttpPost]
        public ActionResult Index(string SearchText, string SelectedType, string SelectedVendors, string SelectedJobs, string SelectedStatus = "", string SelectedPOR = "")
        {
            int siteCoID = base.siteusercompanyid;
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
            ViewBag.VendorList = db.GetVendorsByPors(siteusercompanyid).Where(s => s.Vendor != null && s.Vendor.Trim() != "").ToList();
            ViewBag.JobList = db.GetProjectsByPors(siteusercompanyid).ToList();
            ViewBag.StatusList = db.GetStatusByPors(siteusercompanyid).ToList();
            ViewBag.SelectPORList = db.GetPorNumberByPors(siteCoID).ToList();

            ProcurementResult result = new ProcurementResult();
            var PurchaseList = db.GetPurchasingBySiteCoID(siteCoID).ToList();
            if (SelectedType == "PO")
            {
                char[] sep = new char[] { ',' };
                if (SelectedVendors != null && SelectedVendors != "")
                    PurchaseList = PurchaseList.Where(p => SelectedVendors.Split(sep).Contains(Convert.ToString(p.VendorID))).ToList();
                if (SelectedJobs != null && SelectedJobs != "")
                    PurchaseList = PurchaseList.Where(p => SelectedJobs.Split(sep).Contains(Convert.ToString(p.ProjectID))).ToList();
                if (SelectedStatus != null && SelectedStatus != "")
                    PurchaseList = PurchaseList.Where(p => SelectedStatus.Split(sep).Contains(Convert.ToString(p.StatusID))).ToList();
            }
            var CustodyList = db.GetCustodyBySiteCoID(siteCoID).ToList();
            if (SelectedType == "Custody")
            {
                char[] sep = new char[] { ',' };
                if (SelectedVendors != null && SelectedVendors != "")
                    CustodyList = CustodyList.Where(p => SelectedVendors.Split(sep).Contains(Convert.ToString(p.VendorID))).ToList();
                if (SelectedJobs != null && SelectedJobs != "")
                    CustodyList = CustodyList.Where(p => SelectedJobs.Split(sep).Contains(Convert.ToString(p.ProjectID))).ToList();
                if (SelectedPOR != null && SelectedPOR != "")
                    CustodyList = CustodyList.Where(p => SelectedPOR.Split(sep).Contains(Convert.ToString(p.PorID))).ToList();
            }
            result.DeliveryList = db.GetDeliveriesBySiteCoID(siteCoID).ToList();
            result.TransferList = db.GetTransfersBySiteCoID(siteCoID).ToList();
            result.PendingList = db.GetPendingBySiteCoID(siteCoID).ToList();
            result.InventoryList = db.GetInventoryBySiteCoID(siteCoID).ToList();
            result.CustodyList = CustodyList.ToList();
            result.PurchaseList = PurchaseList.ToList();
            qbModels = new QBModels();
            qbModels.SyncObjectsModel = new QBSyncdto();
            qbModels.OAuthorizationModel = new QBAuthorizationdto();
            qbModels.IsReadySync = false;
            var oAuthModel = new QBOAuthService(qbModels.OAuthorizationModel).IsTokenAvailable(this, siteusercompanyid);
            if (oAuthModel.IsConnected)
            {
                ViewBag.IsQBConnected = true;
            }
            return View(result);
        }

        [HttpPost]
        public ActionResult GetProcurement([DataSourceRequest]DataSourceRequest request, string ProcurementType, string SearchText, string SelectedType, string SelectedVendors, string SelectedJobs, string SelectedStatus = "", string SelectedPOR = "")
        {
            int siteCoID = base.siteusercompanyid;
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
            ViewBag.VendorList = db.GetVendorsByPors(siteusercompanyid).Where(s => s.Vendor != null && s.Vendor.Trim() != "").ToList();
            ViewBag.JobList = db.GetProjectsByPors(siteusercompanyid).ToList();
            ViewBag.StatusList = db.GetStatusByPors(siteusercompanyid).ToList();
            ViewBag.SelectPORList = db.GetPorNumberByPors(siteCoID).ToList();

            ProcurementResult result = new ProcurementResult();
            var PurchaseList = db.GetPurchasingBySiteCoID(siteCoID).ToList();
            if (SelectedType == "PO")
            {
                char[] sep = new char[] { ',' };
                if (SelectedVendors != null && SelectedVendors != "")
                    PurchaseList = PurchaseList.Where(p => SelectedVendors.Split(sep).Contains(Convert.ToString(p.VendorID))).ToList();
                if (SelectedJobs != null && SelectedJobs != "")
                    PurchaseList = PurchaseList.Where(p => SelectedJobs.Split(sep).Contains(Convert.ToString(p.ProjectID))).ToList();
                if (SelectedStatus != null && SelectedStatus != "")
                    PurchaseList = PurchaseList.Where(p => SelectedStatus.Split(sep).Contains(Convert.ToString(p.StatusID))).ToList();
            }
            var CustodyList = db.GetCustodyBySiteCoID(siteCoID).ToList();
            if (SelectedType == "Custody")
            {
                char[] sep = new char[] { ',' };
                if (SelectedVendors != null && SelectedVendors != "")
                    CustodyList = CustodyList.Where(p => SelectedVendors.Split(sep).Contains(Convert.ToString(p.VendorID))).ToList();
                if (SelectedJobs != null && SelectedJobs != "")
                    CustodyList = CustodyList.Where(p => SelectedJobs.Split(sep).Contains(Convert.ToString(p.ProjectID))).ToList();
                if (SelectedPOR != null && SelectedPOR != "")
                    CustodyList = CustodyList.Where(p => SelectedPOR.Split(sep).Contains(Convert.ToString(p.PorID))).ToList();
            }
            result.DeliveryList = db.GetDeliveriesBySiteCoID(siteCoID).ToList();
            result.TransferList = db.GetTransfersBySiteCoID(siteCoID).ToList();
            result.PendingList = db.GetPendingBySiteCoID(siteCoID).ToList();
            result.InventoryList = db.GetInventoryBySiteCoID(siteCoID).ToList();

            result.CustodyList = CustodyList.ToList();
            result.PurchaseList = PurchaseList.ToList();
            qbModels = new QBModels();
            qbModels.SyncObjectsModel = new QBSyncdto();
            qbModels.OAuthorizationModel = new QBAuthorizationdto();
            qbModels.IsReadySync = false;
            var oAuthModel = new QBOAuthService(qbModels.OAuthorizationModel).IsTokenAvailable(this, siteusercompanyid);
            if (oAuthModel.IsConnected)
            {
                ViewBag.IsQBConnected = true;
            }
            DataSourceResult result1 = new DataSourceResult();
            if (ProcurementType == "Purchasing" || ProcurementType == "")
            {
                result1 = result.PurchaseList.ToDataSourceResult(request);

            }
            if (ProcurementType == "Custody")
            {
                result1 = result.CustodyList.ToDataSourceResult(request);

            }
          
            if (ProcurementType == "Deliveries")
            {
                result1 = result.DeliveryList.ToDataSourceResult(request);

            }
            if (ProcurementType == "Transfers")
            {
                result1 = result.TransferList.ToDataSourceResult(request);

            }
            if (ProcurementType == "Inventory")
            {
                result1 = result.InventoryList.ToDataSourceResult(request);

            }
            return Json(result1, JsonRequestBehavior.AllowGet);

            // return View(result);
        }
        public ActionResult SyncPOR(string PORs,string ItemType= "NonInventory", int count=1)
        {
            try
            {
                qbModels = new QBModels();
                int siteCoID = base.siteusercompanyid;
                int[] PORS = Array.ConvertAll(PORs.Remove(PORs.Length - 1, 1).Split(','), s => int.Parse(s));
                foreach (int PORid in PORS)
                {
                    QBAuthorizationdto oAuthDetails = new QBOAuthService(new QBAuthorizationdto()).GetAccessToken(this);
                    syncService = new QBSyncService(oAuthDetails);
                    syncObjects = new QBSyncdto();
                    syncObjects.OauthToken = oAuthDetails;
                    syncObjects.CompanyId = oAuthDetails.Realmid;
                    syncObjects = syncService.SyncPorder(syncObjects, siteCoID, PORid, ItemType);

                }
                TempData["SyncSuccessMessage"] = "Synchronization Process Completed";

                return RedirectToAction("Index");
            }
            catch(Exception ex)
            {
                if (count < 5)
                {
                    return Redirect("SyncPOR?PORs=" + PORs + "&ItemType=" + ItemType + "&count=" + (count + 1));
                }
                else
                {
                    //throw;
                    TempData["SyncErrorMessage"] = ex.Message;
                    return RedirectToAction("Index");
                }
            }
        }
        [HttpPost]
        public ActionResult CheckNewItems(string PORs)
        {
            try
            {
                int siteCoID = base.siteusercompanyid;
                int[] PORS = Array.ConvertAll(PORs.Remove(PORs.Length - 1, 1).Split(','), s => int.Parse(s));
                bool isNew = false;
                foreach (int PORid in PORS)
                {
                    var ItemsCount = db.GetQbPoItemsByPorID(PORid).Where(s => s.ID == null || s.ID.Trim() == "").Count();
                    if (ItemsCount > 0)
                    {
                        isNew = true;
                        break;
                    }
                }
                return Json(isNew);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}