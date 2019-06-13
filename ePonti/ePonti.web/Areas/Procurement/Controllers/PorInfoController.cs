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
using ePonti.BOL.Repository;

namespace ePonti.web.Areas.Procurement.Controllers
{
    public class PorInfoController : web.Controllers._baseMVCController
    {
        private ePontiv2Entities db = new ePontiv2Entities();
        BOL.Repository.CommonRepository repo = new BOL.Repository.CommonRepository();

        // GET: Procurement/PorInfo
        //public ActionResult Index()
        //{
        //    return View();
        //}

        public ActionResult Create()
        {
            var model = new PORModels.NewPOR();
            ViewBag.ShipTo = new SelectList(db.GetShipToListBySiteCoID(siteusercompanyid).ToList(), nameof(GetShipToListBySiteCoID_Result.ViewID), nameof(GetShipToListBySiteCoID_Result.Name));

            return View(model);
        }

        [HttpPost]
        public ActionResult Create(PORModels.NewPOR Model)
        {
            var errorList = new List<string>();

            if (ModelState.IsValid)
            {
                bool status = repo.CreatePOR(siteuserid, Model.ProjectID ?? 0, Model.RequestedDate, Model.Items, Model.ShipToID);
                if (status)
                {
                    return Json(new { status = "success" });
                }
                else
                {
                    errorList.Add("POR couldn't be saved. Please retry.");
                }
            }

            errorList.AddRange((from item in ModelState.Values
                                from error in item.Errors
                                select error.ErrorMessage).ToList()
                             );

            return Json(new { status = "error", errors = errorList });
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var porDetails = db.GetPorInfoByPorID(id).FirstOrDefault();
            if (porDetails == null)
            {
                return HttpNotFound();
            }

            var porItems = db.GetPorItemsByPorID(id).ToList();

            ViewBag.PORDetails = porDetails;
            ViewBag.PORItems = porItems;
            ViewBag.ProjectID = db.ProjectPurchaseOrders.Where(p => p.PurchaseOrderID == id).Select(p => p.ProjectID).FirstOrDefault();

            return View();
        }

        public ActionResult Edit(int? id)
        {
            var por = db.ProjectPurchaseOrders.Where(p => p.PurchaseOrderID == id).FirstOrDefault();
            if (por == null)
            {
                return HttpNotFound();
            }

            var model = new Models.PORModels.EditPOR()
            {
                PorID = por.PurchaseOrderID,
                PONumber = por.PurchaseOrderNumber,
                JobID = por.ProjectID,
                VendorID = por.VendorID,
                Creator = repo.GetUserDisplayName(por.CreatorID ?? 0),//TODO
                CreatedDate = por.DateCreated,
                Priority = por.Priority,
                RequestedDate = por.RequestedDate,
                StatusID = por.ProcurementStatusID,
                OrderDate = por.DateOrdered,
                ArrivalDate = por.ArrivalDate,
                ShippingID = por.ShippingMethodID,
                ShipToID = por.ShipToID,
                Tracking = por.TrackingNumber,
                Notes = por.Notes
            };
            if (model.VendorID != null)
                ViewBag.Vendors = new SelectList(db.GetVendorsBySiteCoID(base.siteusercompanyid).ToList(), nameof(GetVendorsBySiteCoID_Result.ViewID), nameof(GetVendorsBySiteCoID_Result.Vendor), model.VendorID);
            else
                ViewBag.Vendors = new SelectList(db.GetVendorsBySiteCoID(base.siteusercompanyid).ToList(), nameof(GetVendorsBySiteCoID_Result.ViewID), nameof(GetVendorsBySiteCoID_Result.Vendor));

            //if (model.VendorID!=null)
            //model.VendorID = db.CoContacts.Where(p => p.ContactID == model.VendorID).Select(p => p.ContactCoID).FirstOrDefault();
            ViewBag.Jobs = new SelectList(db.GetJobsBySiteCoID(siteusercompanyid).ToList(), nameof(GetJobsBySiteCoID_Result.ViewID), nameof(GetJobsBySiteCoID_Result.Project), por.ProjectID);
            //ViewBag.Vendors = new SelectList(db.CoContactCompanies.Where(p => p.SiteCoID == siteusercompanyid), nameof(CoContactCompanies.ContactCoID), nameof(CoContactCompanies.ContactCoName), model.VendorID);
            ViewBag.Priorities = new SelectList(repo.GetPrioritiesList(), "Item2", "Item2", por.Priority);
            ViewBag.Status = new SelectList(db.CoProcurementStatus.Where(p => p.SiteCoID == siteusercompanyid).OrderBy(p => p.ProcurementStatusOrder).ToList(), nameof(CoProcurementStatus.ProcurementStatusID), nameof(CoProcurementStatus.StatusName), por.ProcurementStatusID);
            ViewBag.Shipping = new SelectList(db.CoShippingMethods.Where(p => p.SiteCoID == siteusercompanyid).ToList(), nameof(CoShippingMethods.ShippingMethodID), nameof(CoShippingMethods.ShippingMethod), por.ShippingMethodID);
            ViewBag.ShipTo = new SelectList(db.GetShipToListBySiteCoID(siteusercompanyid).ToList(), nameof(GetShipToListBySiteCoID_Result.ViewID), nameof(GetShipToListBySiteCoID_Result.Name), por.ShipToID);
            ViewBag.ProjectName = db.ProjectInfo.Where(p => p.ProjectID == por.ProjectID).Select(p => p.ProjectName).FirstOrDefault();

            //ViewBag.Ven 

            return View("_Edit", model);
        }

        [HttpPost]
        public ActionResult Edit(PORModels.EditPOR Model)
        {
            var errorList = new List<string>();

            if (ModelState.IsValid)
            {
                int projectId = SaveProjectPurchaseOrder(Model);
                if (projectId > 0)
                {
                    return Json(new { status = "success" });
                }
                else
                {
                    errorList.Add("Purchase Order Request couldn't be updated. Please retry.");
                }
            }

            errorList.AddRange((from item in ModelState.Values
                                from error in item.Errors
                                select error.ErrorMessage).ToList()
                             );

            return Json(new { status = "error", errors = errorList });
        }

        private int SaveProjectPurchaseOrder(PORModels.EditPOR Model)
        {
            int siteCoID = siteusercompanyid;
            int? VendorId = db.CoContacts.Where(p => p.ContactCoID == Model.VendorID).Select(p=>p.ContactID).FirstOrDefault();
            var por = new ProjectPurchaseOrders()
            {
                PurchaseOrderID = Model.PorID ?? 0,
                PurchaseOrderNumber = Model.PONumber,
                VendorID = VendorId,
                Priority = Model.Priority,
                RequestedDate = Model.RequestedDate,
                ProcurementStatusID = Model.StatusID,
                DateOrdered = Model.OrderDate,
                ArrivalDate = Model.ArrivalDate,
                ShippingMethodID = Model.ShippingID,
                ShipToID = Model.ShipToID,
                TrackingNumber = Model.Tracking,
                Notes = Model.Notes
            };

            var projectId = repo.SaveProjectPurchaseOrder(por);
            return projectId;
        }

        public ActionResult Update(int? id)
        {
            var item = db.GetUpdateInfoByItemID(id).FirstOrDefault();
            if (item == null)
            {
                return HttpNotFound();
            }

            var model = new PORModels.UpdateCustody()
            {
                ItemID = item.ViewID,

                PORNumber = item.Por_,
                Vendor = item.Vendor,
                Manufacturer = item.Manufacturer,
                Model = item.Model,
                ProductDescription = item.ProductDescription,

                ArrivalDate = item.Arrival,
                RequestedDate = item.Requested,
                DeliveredDate = item.Delivered,
                ActualCost = item.ActCost,
                LocationID = item.LocationID,
                PackingSlipNumber = item.Packing_,
                SerialNumber = item.Serial_
            };

            ViewBag.Locations = new SelectList(db.GetCustodyLocationsBySiteCoID(siteusercompanyid).ToList(), nameof(GetCustodyLocationsBySiteCoID_Result.ViewID), nameof(GetCustodyLocationsBySiteCoID_Result.Name), item.LocationID);

            return View(model);
        }

        [HttpPost]
        public ActionResult Update(PORModels.UpdateCustody Model)
        {
            var errorList = new List<string>();

            if (ModelState.IsValid)
            {
                int status = db.UpdateCustodyProcurement(Model.ItemID, Model.ArrivalDate, Model.RequestedDate, DateTime.Now, Model.DeliveredDate, Model.LocationID, Model.PackingSlipNumber, Model.SerialNumber);
                db.SaveChanges();
                if (status==1)
                {
                    return Json(new { status = "success" });
                }
                else
                {
                    errorList.Add("Custody couldn't be updated. Please retry.");
                }
            }

            errorList.AddRange((from item in ModelState.Values
                                from error in item.Errors
                                select error.ErrorMessage).ToList()
                             );

            return Json(new { status = "error", errors = errorList });
        }

        //private bool SaveItemCustody(PORModels.UpdateCustody Model)
        //{
        //    int siteCoID = siteusercompanyid;

        //    var procurement = new ePonti.BOL.Procurement()
        //    {
        //        ItemID = Model.ItemID,
        //        SerialNumber = Model.SerialNumber.ToString(),
        //        ArrivalDate = Model.ArrivalDate,
        //        RequestDate = Model.RequestedDate,
        //        DeliveredDate = Model.DeliveredDate,
        //        CustodyLocationID = Model.LocationID,
        //        PackingSlipNumber = Model.PackingSlipNumber
        //    };

        //    var status = 
        //    return status;
        //}

        #region UpdateSelected
        public ActionResult UpdateSelected(string PorItemIDs)
        {

            //PORModels.UpdateCustody model=null;
            //foreach (int q in porItemIDsInt)
            //{
            //    var item = db.GetUpdateInfoByItemID((int?)q).FirstOrDefault();
            //    if (item == null)
            //    {
            //        return HttpNotFound();
            //    }


            //    model = new PORModels.UpdateCustody()
            //    {
            //        ItemID = item.ViewID,

            //        PORNumber = item.Por_,
            //        Vendor = item.Vendor,
            //        Manufacturer = item.Manufacturer,
            //        Model = item.Model,
            //        ProductDescription = item.ProductDescription,

            //        ArrivalDate = item.Arrival,
            //        RequestedDate = item.Requested,
            //        DeliveredDate = item.Delivered,
            //        ActualCost = item.ActCost,
            //        LocationID = item.LocationID,
            //        PackingSlipNumber = item.Packing_,
            //        SerialNumber = item.Serial_
            //    };
                ViewBag.PorItemIDs = PorItemIDs;
                ViewBag.Locations = new SelectList(db.GetCustodyLocationsBySiteCoID(siteusercompanyid).ToList(), nameof(GetCustodyLocationsBySiteCoID_Result.ViewID), nameof(GetCustodyLocationsBySiteCoID_Result.Name));
           // }
            return View();
        }

        [HttpPost]
        public ActionResult UpdateSelected(PORModels.UpdateCustody Model, string PorItemIDs)
        {
            var errorList = new List<string>();
            var porItemIDsInt = new List<int>();
            try
            {
                porItemIDsInt = PorItemIDs.Split(',').Select(p => Convert.ToInt32(p)).ToList();
            }
            catch (Exception ex)
            {
                LogRepository.LogException(ex);
            }
            if (ModelState.IsValid)
            {
                foreach (int itemid in porItemIDsInt)
                {
                    var item = db.GetUpdateInfoByItemID((int?)itemid).FirstOrDefault();
                    int status = db.UpdateCustodyProcurement(itemid, Model.ArrivalDate, item.Requested, DateTime.Now, Model.DeliveredDate, Model.LocationID, Model.PackingSlipNumber, item.Serial_);
                    db.SaveChanges();
                    if (status == 1)
                    {
                        return Json(new { status = "success" });
                    }
                    else
                    {
                        errorList.Add("Custody couldn't be updated. Please retry.");
                    }
                }
                }

                errorList.AddRange((from item in ModelState.Values
                                    from error in item.Errors
                                    select error.ErrorMessage).ToList()
                                 );

                return Json(new { status = "error", errors = errorList });
            }
       // }
        #endregion
    }
}