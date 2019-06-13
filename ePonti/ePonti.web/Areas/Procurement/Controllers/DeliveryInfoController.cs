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

namespace ePonti.web.Areas.Procurement.Controllers
{
    public class DeliveryInfoController : web.Controllers._baseMVCController
    {
        private ePontiv2Entities db = new ePontiv2Entities();
        BOL.Repository.CommonRepository repo = new BOL.Repository.CommonRepository();

        public ActionResult Create()
        {
            ViewBag.DeliveryTypes = new SelectList(db.GetDeliveryTypesBySiteCoID(siteusercompanyid).ToList(), nameof(GetDeliveryTypesBySiteCoID_Result.ViewID), nameof(GetDeliveryTypesBySiteCoID_Result.Name));
            return View();
        }

        [HttpPost]
        public ActionResult Create(int ProjectID, List<int> Items, DateTime? DeliveryDate, int DeliveryTypeID)
        {
            var errorList = new List<string>();

            if (DeliveryDate.HasValue && Items.Any())
            {
                bool status = repo.CreateDeliveryInfo(siteusercompanyid, ProjectID, Items, DeliveryDate.Value, DeliveryTypeID);
                if (status)
                {
                    return Json(new { status = "success" });
                }
                else
                {
                    errorList.Add("Delivery Request couldn't be saved. Please retry.");
                }
            }

            errorList.AddRange((from item in ModelState.Values
                                from error in item.Errors
                                select error.ErrorMessage).ToList()
                             );

            return Json(new { status = "error", errors = errorList });
        }

        public ActionResult Details(int? id)//Delivery Request ID
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }


            var deliveryInfo = db.GetDeliveryInfoByRequestID(id).FirstOrDefault();
            if (deliveryInfo == null)
            {
                return HttpNotFound();
            }

            var items = db.GetDeliveryInfoItemsByRequestID(id).ToList();

            ViewBag.DeliveryInfo = deliveryInfo;
            ViewBag.Items = items;
            ViewBag.ProjectID = db.ProjectDeliveryRequests.Where(p => p.RequestID == id).Select(p => p.ProjectID).FirstOrDefault();

            ViewBag.CustodyLocations = new SelectList(db.GetCustodyLocationsBySiteCoID(siteusercompanyid).ToList(), nameof(GetCustodyLocationsBySiteCoID_Result.ViewID), nameof(GetCustodyLocationsBySiteCoID_Result.Name));
            ViewBag.DeliveryStatus = new SelectList(db.GetDeliveryStatusBySiteCoID(siteusercompanyid).ToList(), nameof(GetDeliveryStatusBySiteCoID_Result.ViewID), nameof(GetDeliveryStatusBySiteCoID_Result.Name));

            return View();
        }

        public ActionResult UpdateDeliveryRequest(int RequestID, List<int> RequestItemIDs, int? CustodyID, DateTime? Delivered, int? StatusID)
        {
            var errorList = new List<string>();

            bool status = repo.UpdateDeliveryRequestItems(RequestItemIDs, CustodyID, Delivered, StatusID);
            if (status)
            {
                var items = db.GetDeliveryInfoItemsByRequestID(RequestID).ToList();
                return Json(new { status = "success", items });
            }
            else
            {
                errorList.Add("Failed to update. Please retry.");
            }

            errorList.AddRange((from item in ModelState.Values
                                from error in item.Errors
                                select error.ErrorMessage).ToList()
                             );

            return Json(new { status = "error", errors = errorList });
        }

        public ActionResult Edit(int? id)//request id
        {
            ProjectDeliveryRequests delivery = db.ProjectDeliveryRequests.Where(p => p.RequestID == (id ?? 0)).FirstOrDefault();
            if (delivery == null)
            {
                return HttpNotFound();
            }

            string projectName = db.ProjectInfo.Where(p => p.ProjectID == delivery.ProjectID).Select(p => p.ProjectName).FirstOrDefault() ?? "";

            var model = new DeliveryRequestModels.EditDeliveryRequest()
            {
                RequestID = id,
                RequestNumber = delivery.RequestNumber,
                ProjectName = projectName,
                CreatedOn = delivery.DateCreated,
                Delivery = delivery.DeliveryDate,
                TypeID = delivery.DeliveryTypeID,
                StatusID = delivery.DeliveryStatusID,
                Note = delivery.RequestNotes
            };

            ViewBag.CustodyLocations = new SelectList(db.GetCustodyLocationsBySiteCoID(siteusercompanyid).ToList(), nameof(GetCustodyLocationsBySiteCoID_Result.ViewID), nameof(GetCustodyLocationsBySiteCoID_Result.Name), model.TypeID);
            ViewBag.DeliveryStatus = new SelectList(db.GetDeliveryStatusBySiteCoID(siteusercompanyid).ToList(), nameof(GetDeliveryStatusBySiteCoID_Result.ViewID), nameof(GetDeliveryStatusBySiteCoID_Result.Name), model.StatusID);

            return View("_Edit", model);
        }

        [HttpPost]
        public ActionResult Edit(DeliveryRequestModels.EditDeliveryRequest Model)
        {
            var errorList = new List<string>();

            if (ModelState.IsValid)
            {
                bool status = repo.UpdateDeliveryRequest(Model.RequestID,Model.Delivery,Model.TypeID,Model.StatusID,Model.Note);
                if (status)
                {
                    return Json(new { status = "success" });
                }
                else
                {
                    errorList.Add("Delivery Request couldn't be updated. Please retry.");
                }
            }

            errorList.AddRange((from item in ModelState.Values
                                from error in item.Errors
                                select error.ErrorMessage).ToList()
                             );

            return Json(new { status = "error", errors = errorList });
        }


    }
}