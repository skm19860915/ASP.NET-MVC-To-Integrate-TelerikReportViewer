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

namespace ePonti.web.Areas.Common.Controllers
{
    public class ItemsController : web.Controllers._baseMVCController
    {
        private ePontiv2Entities db = new ePontiv2Entities();
        BOL.Repository.CommonRepository repo = new BOL.Repository.CommonRepository();
        Controller itemsController = null;
        Dictionary<Int64, QBSyncdto> syncRepo = null;
        QBSyncService syncService = null;
        QBSyncdto syncObjects = null;
        QBModels qbModels = null;
        static long SyncObjectsModelId = 0;
        public ItemsController()
        {
            syncRepo = new Dictionary<Int64, QBSyncdto>();
        }

        public ActionResult Index()
        {
            return View(db.CoData.ToList());
        }

        #region Short Item

        // GET: Common/Items/Short - this displays Items within a Project
        public ActionResult Short(int? id, int IsCorItem = 0)
        {
            if (IsCorItem == 1)
            {
                return CorItemShort(id);
            }

            var item = db.GetProjectItemInfoByItemID(id).FirstOrDefault();
            if (item == null)
            {
                return HttpNotFound();
            }

            var projectID = db.ProjectItems.Where(p => p.ItemID == id).Select(p => p.ProjectID).FirstOrDefault();
            var masterStageID = db.ProjectStages.Where(p => p.ProjectStageID == item.StageID).Select(p => p.StageMasterID).FirstOrDefault();

            ViewBag.Areas = new SelectList(db.GetAreasByProjectID(projectID).ToList(), nameof(GetAreasByProjectID_Result.ViewID), nameof(GetAreasByProjectID_Result.Division), item.AreaID);
            ViewBag.Divisions = new SelectList(db.GetDivisionsByProjectID(projectID).ToList(), nameof(GetDivisionsByProjectID_Result.ViewID), nameof(GetDivisionsByProjectID_Result.Division), item.DivisionID);
            ViewBag.CoStages = new SelectList(db.GetMasterStagesBySiteID(siteusercompanyid).ToList(), nameof(GetMasterStagesBySiteID_Result.ViewID), nameof(GetMasterStagesBySiteID_Result.Stage));
            ViewBag.ProjectStages = new SelectList(db.GetProjectStagesByProjectID(projectID).ToList(), nameof(GetProjectStagesByProjectID_Result.ProjectStageID), nameof(GetProjectStagesByProjectID_Result.StageName), item.StageID);
            ViewBag.ProjectID = repo.GetProjectIDByItemID(id ?? 0);

            var priceModel = new Models.ItemModels.EditShortItem.PriceModel()
            {
                ItemID = id,
                Qty = item.Qty,
                UnitCost = item.UnitCost,
                UnitPrice = item.UnitPrice,
                Extension = item.Extension,
                Margin = item.Margin,
                Markup = item.Markup,
                Tax = item.Tax,
                Taxable = item.Taxable ?? false,
                Total = item.Total,
                ExcludePOR = item.ExcludePor ?? false,
                WarranteePart = item.WarranteePart ?? false,
                OneOffItem = item.OneOffItem ?? false
            };

            var itemDetails = new ItemModels.ItemDetails()
            {
                ViewID = item.ViewID,
                Label = item.Label,
                ProductDescription = item.ProductDescription,
                SalesDescription = item.SalesDescription,
                AreaID = item.AreaID,
                Area = item.Area,
                DivisionID = item.DivisionID,
                Division = item.Division,
                Por_ = item.Por_,
                CustodyID = item.CustodyID,
                Custody = item.Custody,
                Qty = item.Qty,
                ProjectCostCodeID = item.ProjectCostCodeID,
                CostCode = item.CostCode,
                UnitCost = item.UnitCost,
                UnitPrice = item.UnitPrice,
                Extension = item.Extension,
                Margin = item.Margin,
                Markup = item.Markup,
                Tax = item.Tax,
                TaxRate = item.TaxRate,
                Taxable = item.Taxable,
                Total = item.Total,
                ExcludePor = item.ExcludePor,
                WarranteePart = item.WarranteePart,
                OneOffItem = item.OneOffItem,
                StageID = item.StageID,
                Stage = item.Stage,
                Hours = item.Hours,
                ProjectTotalHours = item.ProjectTotalHours,
            };
            ViewBag.Item = itemDetails;

            var model = new Models.ItemModels.EditShortItem()
            {
                Price = priceModel
            };

            var subLabors = db.GetSubLaborInfoByProjectItemID(id, projectID, masterStageID).ToList();
            var Labors = new StageModels.StageMasterEdit();
            if (subLabors != null)
            {
                foreach (var sub in subLabors)
                {
                    Labors.StageSubDetails.Add(new StageModels.StageDetails()
                    {
                        StageMasterID = (int)masterStageID,
                        SubStageID = sub.ViewID,
                        SubName = sub.SubName,
                        CostPerHour = sub.LaborCost,
                        PricePerHour = sub.LaborPrice,
                        Percent = sub.LaborFactor,
                        SellPerHour = sub.SellPrice

                    });
                }
            }
            ViewBag.Labors = Labors;

            return View(model);
        }

        [HttpPost]
        public ActionResult UpdateProjectInfo(int ItemID, int AreaID, int DivisionID)
        {
            var errorList = new List<string>();

            bool status = repo.UpdateProjectInfo(ItemID, AreaID, DivisionID);
            if (status)
            {
                return Json(new { status = "success" });
            }
            else
            {
                errorList.Add("Project Info couldn't be updated. Please retry.");
            }

            errorList.AddRange((from item in ModelState.Values
                                from error in item.Errors
                                select error.ErrorMessage).ToList()
                             );

            return Json(new { status = "error", errors = errorList });
        }

        [HttpPost]
        public ActionResult UpdateItemPriceInfo(Models.ItemModels.EditShortItem Model)
        {
            var errorList = new List<string>();

            if (ModelState.IsValid && Model != null && Model.Price != null)
            {
                int itemID = Model.Price.ItemID ?? 0;
                var item = db.ProjectItems.Where(p => p.ItemID == itemID).FirstOrDefault();
                var itemPrice = db.ProjectItemPricing.Where(p => p.ItemID == itemID).FirstOrDefault();

                var modelPrice = Model.Price;
                if (item != null && itemPrice != null)
                {
                    item.Qty = modelPrice.Qty;
                    item.ExcludePOR = modelPrice.ExcludePOR;
                    item.WarranteePart = modelPrice.WarranteePart;
                    item.OneOffPart = modelPrice.OneOffItem;

                    db.SaveChanges();

                    db.UpdateProjectItemPricing(itemID,
                        modelPrice.UnitCost, modelPrice.UnitPrice, null, null, modelPrice.Margin, modelPrice.Markup,
                        modelPrice.Taxable, null, null, null);

                    return Json(new { status = "success" });

                }
                else
                {
                    errorList.Add("Item price couldn't be updated. Please retry.");
                }
            }

            errorList.AddRange((from item in ModelState.Values
                                from error in item.Errors
                                select error.ErrorMessage).ToList()
                             );

            return Json(new { status = "error", errors = errorList });
        }
        [HttpPost]
        public ActionResult UpdateItemDescription(int? ViewID, string ProductDescription, string SalesDescription)
        {
            var errorList = new List<string>();

            var q = db.CoData.Where(p => p.MasterItemID == ViewID).FirstOrDefault();
            if (q != null)
            {
                q.ProductDescription = ProductDescription;
                q.SalesDescription = SalesDescription;
                db.SaveChanges();
                return Json(new { status = "success" });
            }
            else
            {
                errorList.Add("Item Info couldn't be updated. Please retry.");
            }

            errorList.AddRange((from item in ModelState.Values
                                from error in item.Errors
                                select error.ErrorMessage).ToList()
                             );

            return Json(new { status = "error", errors = errorList });
        }
        [HttpPost]
        public ActionResult UpdateProjectItemDescription(int? ViewID, string ProductDescription, string SalesDescription)
        {
            var errorList = new List<string>();

            var q = db.ProjectItems.Where(p => p.ItemID == ViewID).FirstOrDefault();
            if (q != null)
            {
                q.ProductDescription = ProductDescription;
                q.SalesDescription = SalesDescription;
                db.SaveChanges();
                return Json(new { status = "success" });
            }
            else
            {
                errorList.Add("Item Info couldn't be updated. Please retry.");
            }

            errorList.AddRange((from item in ModelState.Values
                                from error in item.Errors
                                select error.ErrorMessage).ToList()
                             );

            return Json(new { status = "error", errors = errorList });
        }
        [HttpPost]
        public ActionResult UpdateItemPriceDetails(CoData Model, int? ViewID, int? CostCodeID, bool? SalesTax, bool? PurchasedTaxed, int? ContactID, decimal? UnitCost, decimal? UnitPrice, decimal? Margin, decimal? Markup, bool? ExcludePor, bool? WarranteePart, bool? OneOffItem, int? SaleUomID, int? OrderUomID, int? LeadTime, int? OrderQty, decimal? OrderPrice)
        {
            var errorList = new List<string>();
            Model.MasterItemID = (int)ViewID;

            int MasteritemID = (int?)Model.MasterItemID ?? 0;
            var item = db.CoData.Where(p => p.MasterItemID == MasteritemID).FirstOrDefault();
            var itemPrice = db.CoDataPricing.Where(p => p.MasterItemID == item.MasterItemID).FirstOrDefault();

            if (item != null && itemPrice != null)
            {
                db.UpdateItemPrice(ViewID, ContactID, item.Discontinued, DateTime.Now.Date, CostCodeID, SaleUomID, OrderUomID, LeadTime, OrderQty, item.OrderingName, item.UseOrderingName, OrderPrice, ExcludePor, item.ExcludeSO, OrderQty, SalesTax, PurchasedTaxed, WarranteePart, OneOffItem, UnitCost, UnitPrice);
                db.SaveChanges();
                itemPrice.Cost = UnitCost;
                itemPrice.Price = UnitPrice;
                db.SaveChanges();
                return Json(new { status = "success" });
            }
            else
            {
                errorList.Add("Item couldn't be updated. Please retry.");
            }

            return Json(new { status = "error", errors = errorList });
        }
        [HttpPost]
        public ActionResult UpdateLaborInfoItem(string flagval, int? ViewID, int? StageID, bool? LaborTax, decimal? Hours)
        {
            if (flagval == "2")
            {
                var stageinfo = db.GetStageMasterEditInfoByID(StageID).FirstOrDefault();
                if (stageinfo != null)
                {
                    var subLabors = db.GetSubLaborInfoByMasterStageID(stageinfo.StageMasterID).ToList();
                    var Labors = new StageModels.StageMasterEdit();
                    foreach (var sub in subLabors)
                    {
                        Labors.StageSubDetails.Add(new StageModels.StageDetails()
                        {
                            StageMasterID = stageinfo.StageMasterID,
                            SubStageID = sub.SubID,
                            SubName = sub.SubStage,
                            CostPerHour = sub.Cost,
                            PricePerHour = sub.Price,
                            Percent = sub.Factor,
                            SellPerHour = sub.Sell
                        });
                    }
                    ViewBag.Labors = Labors;
                    return PartialView("_ItemLaborPartial", Labors);
                }
            }
            else
            {
                db.UpdateItemLabor(ViewID, StageID, DateTime.Now.Date, Hours);
                db.SaveChanges();
                return Json(new { status = "success", ViewID = ViewID });
            }

            return Json(new { status = "ERROR" });
        }

        public ActionResult _ItemLaborPartial()
        {
            return PartialView("_ItemLaborPartial");
        }

        public ActionResult _ProjectLaborView()
        {
            return PartialView("_ProjectLaborView");
        }

        public ActionResult GetStageDetails(int ItemID, int ProjectID, int StageID)
        {
            var masterStageID = db.ProjectStages.Where(p => p.ProjectStageID == StageID).Select(p => p.StageMasterID).FirstOrDefault();
            var res = db.GetSubLaborInfoByProjectItemID(ItemID, ProjectID, masterStageID).ToList();
            var type = "data";

            return Json(new { data = res, type }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UpdateLaborInfo(List<StageModels.StageDetails> Data)
        {
            if (Data != null && Data.Any())
            {
                var psd = Data.FirstOrDefault();

                var q = db.ProjectItemPricing.Where(p => p.ItemID == psd.ItemID).FirstOrDefault();
                db.UpdateProjectItemPricing(psd.ItemID, q.UnitCost, q.UnitPrice, psd.UnitHours, q.SalesTaxID, q.Margin, q.Markup, q.SalesTaxed, q.LaborTaxed, q.CostTaxed, q.ProjectCostCodeID);
                db.SaveChanges();
                foreach (var item in Data)
                {
                    var isNew = item.ProjectStageID == 0;

                    var ProjectStage = new ProjectStages()
                    {
                        ProjectID = item.ProjectID,
                        ProjectStageID = item.ProjectStageID ?? 0,
                        StageSubID = item.SubStageID ?? 0,
                        StageLaborCost = item.CostPerHour,
                        StageLaborPrice = item.PricePerHour,
                        StageMasterID = item.StageMasterID,
                        StageLaborFactor = item.Percent / 100,
                        StageDifficultyFactor = 1

                    };

                    var psid = repo.SaveProjectStageDetails(ProjectStage);
                }

                int projectID = 0, stageID = 0;
                if (psd != null)
                {
                    projectID = psd.ProjectID;
                    stageID = (int)(psd.ProjectStageID);
                    return GetStageDetails((int)psd.ItemID, projectID, stageID);
                }
                else
                {
                    return Json(new { status = "SOME_ERROR", });
                }

            }
            else
            {
                return Json(new { status = "NOT_SAVED", });
            }
        }

        #endregion

        #region Cor Short Item

        [NonAction]
        private ActionResult CorItemShort(int? id)//cor item id
        {
            var item = db.GetCorItemInfoByCorItemID(id).FirstOrDefault();
            if (item == null)
            {
                return HttpNotFound();
            }

            int? corID = 0, projectItemID = 0;
            var corItem = db.ProjectCorItems.Where(p => p.CorItemID == id).Select(p => new { p.CorID, p.ItemID }).FirstOrDefault();
            if (corItem != null)
            {
                corID = corItem.CorID;
                projectItemID = corItem.ItemID;
            }

            var projectID = db.ProjectCor.Where(p => p.CorID == corID).Select(p => p.ProjectID).FirstOrDefault();

            ViewBag.Areas = new SelectList(db.GetAreasByProjectID(projectID).ToList(), nameof(GetAreasByProjectID_Result.ViewID), nameof(GetAreasByProjectID_Result.Division), item.AreaID);
            ViewBag.Divisions = new SelectList(db.GetDivisionsByProjectID(projectID).ToList(), nameof(GetDivisionsByProjectID_Result.ViewID), nameof(GetDivisionsByProjectID_Result.Division), item.DivisionID);
            ViewBag.ProjectStages = new SelectList(db.GetProjectStagesByProjectID(projectID).ToList(), nameof(GetProjectStagesByProjectID_Result.ProjectStageID), nameof(GetProjectStagesByProjectID_Result.StageName), item.StageID);
            ViewBag.ProjectID = projectID;
            ViewBag.ProjectItemID = projectItemID;
            ViewBag.IsCorItem = true;

            var priceModel = new Models.ItemModels.EditShortItem.PriceModel()
            {
                ItemID = id,
                Qty = item.Qty,
                UnitCost = item.UnitCost,
                UnitPrice = item.UnitPrice,
                Extension = item.Extension,
                Margin = item.Margin,
                Markup = item.Markup,
                Tax = item.Tax,
                Taxable = item.Taxable ?? false,
                Total = item.Total,
                ExcludePOR = item.ExcludePor ?? false,
                WarranteePart = item.WarranteePart ?? false,
                OneOffItem = item.OneOffItem ?? false
            };

            var itemDetails = new ItemModels.ItemDetails()
            {
                ViewID = item.ViewID,
                Label = item.Label,
                ProductDescription = item.ProductDescription,
                SalesDescription = item.SalesDescription,
                AreaID = item.AreaID,
                Area = item.Area,
                DivisionID = item.DivisionID,
                Division = item.Division,
                Por_ = item.Por_,
                CustodyID = item.CustodyID,
                Custody = item.Custody,
                Qty = item.Qty,
                ProjectCostCodeID = item.ProjectCostCodeID,
                CostCode = item.CostCode,
                UnitCost = item.UnitCost,
                UnitPrice = item.UnitPrice,
                Extension = item.Extension,
                Margin = item.Margin,
                Markup = item.Markup,
                Tax = item.Tax,
                TaxRate = 0,
                Taxable = item.Taxable,
                Total = item.Total,
                ExcludePor = item.ExcludePor,
                WarranteePart = item.WarranteePart,
                OneOffItem = item.OneOffItem,
                StageID = item.StageID,
                Stage = item.Stage,
                Hours = item.Hours,
                ProjectTotalHours = item.ProjectTotalHours
            };
            ViewBag.Item = itemDetails;

            var model = new Models.ItemModels.EditShortItem()
            {
                Price = priceModel
            };

            return View("short", model);
        }

        [HttpPost]
        public ActionResult UpdateCorInfo(int CorItemID, int AreaID, int DivisionID)
        {
            var errorList = new List<string>();

            bool status = repo.UpdateCorInfo(CorItemID, AreaID, DivisionID);
            if (status)
            {
                return Json(new { status = "success" });
            }
            else
            {
                errorList.Add("Cor Info couldn't be updated. Please retry.");
            }

            errorList.AddRange((from item in ModelState.Values
                                from error in item.Errors
                                select error.ErrorMessage).ToList()
                             );

            return Json(new { status = "error", errors = errorList });
        }

        [HttpPost]
        public ActionResult UpdateCorItemPriceInfo(Models.ItemModels.EditShortItem Model)
        {
            var errorList = new List<string>();

            if (ModelState.IsValid && Model != null && Model.Price != null)
            {
                int corItemID = Model.Price.ItemID ?? 0;
                var item = db.ProjectCorItems.Where(p => p.CorItemID == corItemID).FirstOrDefault();
                //var itemPrice = db.ProjectItemPricing.Where(p => p.ItemID == itemID).FirstOrDefault();

                var modelPrice = Model.Price;
                if (item != null)
                {
                    item.Qty = modelPrice.Qty;
                    item.ExcludePOR = modelPrice.ExcludePOR;
                    item.WarranteePart = modelPrice.WarranteePart;
                    item.OneOffPart = modelPrice.OneOffItem;

                    item.UnitCost = modelPrice.UnitCost;
                    item.UnitPrice = modelPrice.UnitPrice;
                    item.Margin = modelPrice.Margin;
                    item.Markup = modelPrice.Markup;
                    item.SalesTaxed = modelPrice.Taxable;

                    db.SaveChanges();

                    //TODO
                    //modelPrice.Extension  
                    //modelPrice.Tax;
                    //modelPrice.Taxable;
                    //modelPrice.Total;

                    return Json(new { status = "success" });
                }
                else
                {
                    errorList.Add("Cor Item price couldn't be updated. Please retry.");
                }
            }

            errorList.AddRange((from item in ModelState.Values
                                from error in item.Errors
                                select error.ErrorMessage).ToList()
                             );

            return Json(new { status = "error", errors = errorList });
        }

        #endregion

        public ActionResult Details(int? id)
        {
            var data = db.GetItemInfoByMasterItemID(id).FirstOrDefault();
            if (data.Vendor != null)
                ViewBag.Vendors = new SelectList(db.GetVendorsBySiteCoID(base.siteusercompanyid).ToList(), nameof(GetVendorsBySiteCoID_Result.ViewID), nameof(GetVendorsBySiteCoID_Result.Vendor), data.Vendor);
            else
                ViewBag.Vendors = new SelectList(db.GetVendorsBySiteCoID(base.siteusercompanyid).ToList(), nameof(GetVendorsBySiteCoID_Result.ViewID), nameof(GetVendorsBySiteCoID_Result.Vendor));
            ViewBag.SalesUom = new SelectList(db.GetSiteDataUom().ToList(), nameof(GetSiteDataUom_Result.ViewID), nameof(GetSiteDataUom_Result.Uom), data.SaleUom);
            ViewBag.OrderUom = new SelectList(db.GetSiteDataUom().ToList(), nameof(GetSiteDataUom_Result.ViewID), nameof(GetSiteDataUom_Result.Uom), data.OrderUom);
            ViewBag.CostCodes = new SelectList(db.GetCoCostCodesBySiteCoID(siteusercompanyid).ToList(), nameof(GetCoCostCodesBySiteCoID_Result.CostCodeID), nameof(GetCoCostCodesBySiteCoID_Result.CostCode));
            var projectID = db.ProjectItems.Where(p => p.MasterItemID == id).Select(p => p.ProjectID).FirstOrDefault();
            ViewBag.CoStages = new SelectList(db.GetMasterStagesBySiteID(siteusercompanyid).ToList(), nameof(GetMasterStagesBySiteID_Result.ViewID), nameof(GetMasterStagesBySiteID_Result.Stage));
            var stage = db.GetStageMasterEditInfoByID(data.StageID).FirstOrDefault();
            if (stage != null)
            {
                var subLabors = db.GetSubLaborInfoByMasterItemID(id).ToList();
                var Labors = new StageModels.StageMasterEdit();
                if (subLabors != null)
                {
                    foreach (var sub in subLabors)
                    {
                        Labors.StageSubDetails.Add(new StageModels.StageDetails()
                        {
                            StageMasterID = stage.StageMasterID,
                            SubStageID = sub.SubID,
                            SubName = sub.SubStage,
                            CostPerHour = sub.Cost,
                            PricePerHour = sub.Price,
                            Percent = sub.Factor,
                            SellPerHour = sub.Sell
                        });
                    }
                }
                ViewBag.Labors = Labors;
            }
            if (data == null)
            {
                return HttpNotFound();
            }
            else
            {
                ViewBag.AcctItemID = data.AcctItemID;
            }
            int siteCoID = base.siteusercompanyid;

            qbModels = new QBModels();
            qbModels.SyncObjectsModel = new QBSyncdto();
            qbModels.OAuthorizationModel = new QBAuthorizationdto();
            qbModels.IsReadySync = false;
            var oAuthModel = new QBOAuthService(qbModels.OAuthorizationModel).IsTokenAvailable(this, siteusercompanyid);
            if (oAuthModel.IsConnected)
            {
                qbModels.IsReadySync = true;
                qbModels.OAuthorizationModel = oAuthModel;
                qbModels.IsConnected = oAuthModel.IsConnected;
                var syncService = new QBSyncService(oAuthModel);
                qbModels.SyncObjectsModel.OauthToken = oAuthModel;
                qbModels.SyncObjectsModel.CompanyId = oAuthModel.Realmid;
                Random random = new Random();
                qbModels.SyncObjectsModel = Save(this, qbModels.SyncObjectsModel);
                SyncObjectsModelId = qbModels.SyncObjectsModel.Id;
            }

            return View(data);
        }
        public ActionResult SyncToQBItems(int? id, string type)
        {
            int siteCoID = base.siteusercompanyid;
            Int64 id1 = SyncObjectsModelId;
            QBAuthorizationdto oAuthDetails = new QBOAuthService(new QBAuthorizationdto()).GetAccessToken(this);
            syncService = new QBSyncService(oAuthDetails);
            syncObjects = id1 > 0 ? GetSyncObjects(this, id1) : new QBSyncdto();
            syncObjects.OauthToken = oAuthDetails;
            syncObjects.CompanyId = oAuthDetails.Realmid;
            syncObjects = syncService.GetDatafromModelItems(syncObjects, siteCoID, id, type);
            if (syncObjects.ItemList.Count > 0)
            {
                syncObjects = syncService.SyncItems(this, syncObjects);
            }
            this.Save(this, syncObjects);

            return RedirectToAction("details", new { id = id });
        }

        public ActionResult Create()
        {
            qbModels = new QBModels();
            qbModels.SyncObjectsModel = new QBSyncdto();
            qbModels.OAuthorizationModel = new QBAuthorizationdto();
            qbModels.IsReadySync = false;
            var oAuthModel = new QBOAuthService(qbModels.OAuthorizationModel).IsTokenAvailable(this, siteusercompanyid);
            ViewBag.IsQBConnected = "false";
            if (oAuthModel.IsConnected)
            {
                ViewBag.IsConnected = "true";
                //qbModels.IsReadySync = true;
                //qbModels.OAuthorizationModel = oAuthModel;
                //qbModels.IsConnected = oAuthModel.IsConnected;
                //var syncService = new QBSyncService(oAuthModel);
                //qbModels.SyncObjectsModel.OauthToken = oAuthModel;
                //Random random = new Random();
                //SyncObjectsModelId = random.Next(1, 100);
                //ViewBag.IsQBConnected = "true";
                //SyncObjectsModelId = qbModels.SyncObjectsModel.Id;
            }
            ViewBag.Group = new SelectList(db.GetGroupsBySiteCoID(base.siteusercompanyid).ToList(), nameof(GetGroupsBySiteCoID_Result.ViewID), nameof(GetGroupsBySiteCoID_Result.Group));
            ViewBag.Manufacturer = new SelectList(db.GetManufacturersBySiteCoID(base.siteusercompanyid).ToList(), nameof(GetManufacturersBySiteCoID_Result.ViewID), nameof(GetManufacturersBySiteCoID_Result.Name));
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CoData coData)
        {
            if (ModelState.IsValid)
            {
                // db.InsertMasterItem(base.siteusercompanyid, coData.GroupID, coData.ManufacturerID, coData.Model, coData.SKU);                
                CoData cdata = new CoData();
                cdata.GroupID = coData.GroupID;
                cdata.ManufacturerID = coData.ManufacturerID;
                cdata.Model = coData.Model;
                cdata.SKU = coData.SKU;
                cdata.SiteCoID = base.siteusercompanyid;
                cdata.UseOrderingName = coData.UseOrderingName;
                cdata.Discontinued = coData.Discontinued;
                db.CoData.Add(cdata);
                db.SaveChanges();
                CoDataPricing coDataPricing = new CoDataPricing();
                coDataPricing.MasterItemID = cdata.MasterItemID;
                db.CoDataPricing.Add(coDataPricing);
                db.SaveChanges();
                return Json(new { status = "success", ViewID = cdata.MasterItemID });
            }

            return View(coData);
        }
        internal QBSyncdto Save(object controller, QBSyncdto syncObjects)
        {
            itemsController = controller as Controller;
            Random random = new Random();
            syncObjects.Id = random.Next(1, 100);
            syncRepo.Add(syncObjects.Id, syncObjects);
            itemsController.TempData["Sync"] = syncRepo;
            itemsController.TempData.Keep();
            return syncObjects;
        }
        internal QBSyncdto GetSyncObjects(object controller, Int64 id)
        {
            itemsController = controller as System.Web.Mvc.Controller;
            Dictionary<Int64, QBSyncdto> syncRepo = itemsController.TempData["Sync"] as Dictionary<Int64, QBSyncdto>;
            itemsController.TempData.Keep();
            if (syncRepo.ContainsKey(id))
            {
                return syncRepo[id];
            }
            else
            {
                return syncRepo[syncRepo.First().Key];
            }
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CoData coData = db.CoData.Find(id);
            ViewBag.Group = new SelectList(db.GetGroupsBySiteCoID(base.siteusercompanyid).ToList(), nameof(GetGroupsBySiteCoID_Result.ViewID), nameof(GetGroupsBySiteCoID_Result.Group), coData.GroupID);
            ViewBag.Manufacturer = new SelectList(db.GetManufacturersBySiteCoID(base.siteusercompanyid).ToList(), nameof(GetManufacturersBySiteCoID_Result.ViewID), nameof(GetManufacturersBySiteCoID_Result.Name), coData.ManufacturerID);
            var model = db.GetItemInfoByMasterItemID(coData.MasterItemID).FirstOrDefault();
            if (coData == null)
            {
                return HttpNotFound();
            }
            return PartialView("_Edit", model);
        }

        private void LoadAdditionalDropdownData()
        {
            var siteCoID = siteusercompanyid;

            ViewBag.CoStages = new SelectList(db.GetCoStagesBySiteCoID(siteCoID).ToList(), nameof(GetCoStagesBySiteCoID_Result.StageMasterID), nameof(GetCoStagesBySiteCoID_Result.Stage));
            ViewBag.CostCodes = new SelectList(db.GetCoCostCodesBySiteCoID(siteCoID).ToList(), nameof(GetCoCostCodesBySiteCoID_Result.CostCodeID), nameof(GetCoCostCodesBySiteCoID_Result.CostCode));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CoData coData, int? ViewID)
        {
            if (ModelState.IsValid)
            {
                var q = db.CoData.Where(p => p.MasterItemID == ViewID).FirstOrDefault();
                q.MasterItemID = (int)ViewID;
                q.SiteCoID = base.siteusercompanyid;
                q.GroupID = coData.GroupID;
                q.ManufacturerID = coData.ManufacturerID;
                q.Model = coData.Model;
                q.SKU = coData.SKU;
                db.SaveChanges();
                return Json(new { status = "success", ViewID = coData.MasterItemID });
            }
            return View(coData);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CoData coData = db.CoData.Find(id);
            if (coData == null)
            {
                return HttpNotFound();
            }
            return View(coData);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CoData coData = db.CoData.Find(id);
            db.CoData.Remove(coData);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


    }
}
