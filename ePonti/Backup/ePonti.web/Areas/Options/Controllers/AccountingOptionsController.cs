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

namespace ePonti.web.Areas.Options.Controllers
{
    [Authorize]
    public class AccountingOptionsController : ePonti.web.Controllers._baseMVCController
    {
        private ePontiv2Entities db = new ePontiv2Entities();
        BOL.Repository.CommonRepository repo = new BOL.Repository.CommonRepository();

        // GET: Options/AccountingOptions
        public ActionResult Index()
        {
            int siteCoID = base.siteusercompanyid;

            ViewBag.CoSalesOrderStatus = db.GetSalesOrderStatusBySiteCoID(siteCoID);
            ViewBag.CoPorStatus = db.GetPorStatusBySiteCoID(siteCoID);
            ViewBag.CoDeliveryTypes = db.GetDeliveryTypesBySiteCoID(siteCoID);
            ViewBag.CoCreditMemoStatus = db.GetCreditMemoStatusBySiteCoID(siteCoID);
            ViewBag.CoDeliveryStatus = db.GetDeliveryStatusBySiteCoID(siteCoID);
            ViewBag.CoRmaStatus = db.GetRmaStatusBySiteCoID(siteCoID);
            ViewBag.CoShippingMethods = db.GetShippingMethodsBySiteCoID(siteCoID);
            ViewBag.CoCostCodes = db.GetCostCodesBySiteCoID(siteCoID);
            ViewBag.CoInventoryTemplates = db.GetInventoryTemplatesBySiteCoID(siteCoID);
            ViewBag.CoShipToList = db.GetShipToListBySiteCoID(siteCoID);
            ViewBag.CoCustodyLocations = db.GetCustodyLocationsBySiteCoID(siteCoID);
            ViewBag.CoTaxes = db.GetTaxesBySiteCoID(siteCoID);

            return View();
        }

        public ActionResult DeleteOption(int ItemID, string Type)
        {
            var res = repo.DeleteOption(Type, ItemID, siteusercompanyid);
            return Json(new { status = (res == "" ? "success" : res) });
        }

        #region Options Update
        public ActionResult UpdateSalesOrderStatus(int ItemID, string Name, int Order)
        {
            CoSalesOrderStatus item;
            if (ItemID == 0)
            {
                item = new CoSalesOrderStatus() { SiteCoID = siteusercompanyid };
                db.CoSalesOrderStatus.Add(item);
            }
            else
            {
                item = db.CoSalesOrderStatus.Where(p => p.SalesOrderStatusID == ItemID && p.SiteCoID == siteusercompanyid).FirstOrDefault();

            }

            if (item != null)
            {
                item.SalesOrderStatus = Name;
                item.StatusOrder = Order;
            }
            db.SaveChanges();
            return Json(new { status = "success", itemID = item.SalesOrderStatusID });
        }

        public ActionResult UpdatePORStatus(int ItemID, string Name, int Order)
        {
            CoProcurementStatus item;
            if (ItemID == 0)
            {
                item = new CoProcurementStatus() { SiteCoID = siteusercompanyid };
                db.CoProcurementStatus.Add(item);
            }
            else
            {
                item = db.CoProcurementStatus.Where(p => p.ProcurementStatusID == ItemID && p.SiteCoID == siteusercompanyid).FirstOrDefault();

            }

            if (item != null)
            {
                item.StatusName = Name;
                item.ProcurementStatusOrder = Order;
            }
            db.SaveChanges();
            return Json(new { status = "success", itemID = item.ProcurementStatusID });
        }

        public ActionResult UpdateDeliveryType(int ItemID, string Name, int Order)
        {
            CoDeliveryTypes item;
            if (ItemID == 0)
            {
                item = new CoDeliveryTypes() { SiteCoID = siteusercompanyid };
                db.CoDeliveryTypes.Add(item);
            }
            else
            {
                item = db.CoDeliveryTypes.Where(p => p.DeliveryTypeID == ItemID && p.SiteCoID == siteusercompanyid).FirstOrDefault();

            }

            if (item != null)
            {
                item.DeliveryType = Name;
                item.DeliveryTypeOrder= Order;
            }
            db.SaveChanges();
            return Json(new { status = "success", itemID = item.DeliveryTypeID });
        }

        public ActionResult UpdateShippingMethod(int ItemID, string Name)
        {
            CoShippingMethods item;
            if (ItemID == 0)
            {
                item = new CoShippingMethods() { SiteCoID = siteusercompanyid };
                db.CoShippingMethods.Add(item);
            }
            else
            {
                item = db.CoShippingMethods.Where(p => p.ShippingMethodID == ItemID && p.SiteCoID == siteusercompanyid).FirstOrDefault();

            }

            if (item != null)
            {
                item.ShippingMethod = Name;
            }
            db.SaveChanges();
            return Json(new { status = "success", itemID = item.ShippingMethodID });
        }

        public ActionResult UpdateCostCode(int ItemID, string Name)
        {
            CoCostCodes item;
            if (ItemID == 0)
            {
                item = new CoCostCodes() { SiteCoID = siteusercompanyid };
                db.CoCostCodes.Add(item);
            }
            else
            {
                item = db.CoCostCodes.Where(p => p.CostCodeID == ItemID && p.SiteCoID == siteusercompanyid).FirstOrDefault();

            }

            if (item != null)
            {
                item.CostCode = Name;
            }
            db.SaveChanges();
            return Json(new { status = "success", itemID = item.CostCodeID });
        }

        //public ActionResult UpdateInventoryTemplate(int ItemID, string Name, string Description)
        //{
        //    CoActivitiesStatus item;
        //    if (ItemID == 0)
        //    {
        //        item = new CoActivitiesStatus() { SiteCoID = siteusercompanyid };
        //        db.CoActivitiesStatus.Add(item);
        //    }
        //    else
        //    {
        //        item = db.CoActivitiesStatus.Where(p => p.StatusID == ItemID && p.SiteCoID == siteusercompanyid).FirstOrDefault();

        //    }

        //    if (item != null)
        //    {
        //        item.StatusName = Name;
        //        item.StatusOrder = Order;
        //    }
        //    db.SaveChanges();
        //    return Json(new { status = "success", itemID = item.StatusID });
        //}

        public ActionResult UpdateCreditMemoStatus(int ItemID, string Name, int Order)
        {
            CoCreditMemoStatus item;
            if (ItemID == 0)
            {
                item = new CoCreditMemoStatus() { SiteCoID = siteusercompanyid };
                db.CoCreditMemoStatus.Add(item);
            }
            else
            {
                item = db.CoCreditMemoStatus.Where(p => p.CreditMemoStatusID == ItemID && p.SiteCoID == siteusercompanyid).FirstOrDefault();

            }

            if (item != null)
            {
                item.CreditMemoStatus = Name;
                item.CreditMemoStatusOrder = Order;
            }
            db.SaveChanges();
            return Json(new { status = "success", itemID = item.CreditMemoStatusID });
        }

        public ActionResult UpdateDeliveryStatus(int ItemID, string Name, int Order)
        {
            CoDeliveryStatus item;
            if (ItemID == 0)
            {
                item = new CoDeliveryStatus() { SiteCoID = siteusercompanyid };
                db.CoDeliveryStatus.Add(item);
            }
            else
            {
                item = db.CoDeliveryStatus.Where(p => p.DeliveryStatusID == ItemID && p.SiteCoID == siteusercompanyid).FirstOrDefault();

            }

            if (item != null)
            {
                item.DeliveryStatusName = Name;
                item.DeliveryStatusOrder= Order;
            }
            db.SaveChanges();
            return Json(new { status = "success", itemID = item.DeliveryStatusID });
        }

        public ActionResult UpdateRMAStatus(int ItemID, string Name, int Order)
        {
            CoRmaStatus item;
            if (ItemID == 0)
            {
                item = new CoRmaStatus() { SiteCoID = siteusercompanyid };
                db.CoRmaStatus.Add(item);
            }
            else
            {
                item = db.CoRmaStatus.Where(p => p.RMAStatusID == ItemID && p.SiteCoID == siteusercompanyid).FirstOrDefault();

            }

            if (item != null)
            {
                item.RMAStatus= Name;
                item.RMAStatusOrder= Order;
            }
            db.SaveChanges();
            return Json(new { status = "success", itemID = item.RMAStatusID });
        }

        public ActionResult UpdateShipToList(int ItemID, string Name, string Address1, string Address2, string City, string State, string Zip, string Option)
        {
            CoShipTo item;
            if (ItemID == 0)
            {
                item = new CoShipTo() { SiteCoID = siteusercompanyid };
                db.CoShipTo.Add(item);
            }
            else
            {
                item = db.CoShipTo.Where(p => p.ShipToID == ItemID && p.SiteCoID == siteusercompanyid).FirstOrDefault();

            }

            if (item != null)
            {
                item.ShipToName = Name;
                item.ShipToAddress1 = Address1;
                item.ShipToAddress2 = Address2;
                item.ShipToCity= City;
                item.ShipToState= State;
                item.ShipToZip= Zip;
            }
            db.SaveChanges();
            return Json(new { status = "success", itemID = item.ShipToID });
        }

        public ActionResult UpdateCustodyLocation(int ItemID, string Name)
        {
            CoCustodyLocations item;
            if (ItemID == 0)
            {
                item = new CoCustodyLocations() { SiteCoID = siteusercompanyid };
                db.CoCustodyLocations.Add(item);
            }
            else
            {
                item = db.CoCustodyLocations.Where(p => p.CustodyLocationID == ItemID && p.SiteCoID == siteusercompanyid).FirstOrDefault();

            }

            if (item != null)
            {
                item.CustodyLocation = Name;
            }
            db.SaveChanges();
            return Json(new { status = "success", itemID = item.CustodyLocationID });
        }

        public ActionResult UpdateTaxes(int ItemID, string Code, string Description, decimal Sales, decimal Labor)
        {
            CoTaxes item;
            if (ItemID == 0)
            {
                item = new CoTaxes() { SiteCoID = siteusercompanyid };
                db.CoTaxes.Add(item);
            }
            else
            {
                item = db.CoTaxes.Where(p => p.MasterTaxID == ItemID && p.SiteCoID == siteusercompanyid).FirstOrDefault();

            }

            if (item != null)
            {
                item.TaxCode = Code;
                item.TaxDescription= Description;
                item.SalesRate = Sales;
                item.LaborRate = Labor;
            }
            db.SaveChanges();
            return Json(new { status = "success", itemID = item.MasterTaxID });
        }
        #endregion
    }
}