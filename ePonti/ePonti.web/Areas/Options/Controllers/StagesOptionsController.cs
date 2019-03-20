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
    public class StagesOptionsController : ePonti.web.Controllers._baseMVCController
    {
        private ePontiv2Entities db = new ePontiv2Entities();
        BOL.Repository.CommonRepository repo = new BOL.Repository.CommonRepository();

        // GET: Options/StagesOptions
        public ActionResult Index()
        {
            ViewBag.Masters = db.CoStagesMaster.Where(p => p.SiteCoID == siteusercompanyid).OrderBy(p => p.StageOrder).ToList();
            ViewBag.Subs = db.CoStagesSubs.Where(p => p.SiteCoID == siteusercompanyid).OrderBy(p => p.StageSubOrder).ToList();

            return View();
        }

        public ActionResult DeleteOption(int ItemID, string Type)
        {
            var res = repo.DeleteOption(Type, ItemID, siteusercompanyid);
            return Json(new { status = (res == "" ? "success" : res) });
        }

        public ActionResult UpdateStageMaster(int ItemID, string Name, int Order)
        {
            CoStagesMaster item;
            if (ItemID == 0)
            {
                item = new CoStagesMaster() { SiteCoID = siteusercompanyid };
                db.CoStagesMaster.Add(item);
            }
            else
            {
                item = db.CoStagesMaster.Where(p => p.StageMasterID == ItemID && p.SiteCoID == siteusercompanyid).FirstOrDefault();

            }

            if (item != null)
            {
                item.StageName = Name;
                item.StageOrder = Order;
            }
            db.SaveChanges();
            return Json(new { status = "success", itemID = item.StageMasterID });
        }

        public ActionResult UpdateStageSub(int ItemID, string Name, int Order)
        {
            CoStagesSubs item;
            if (ItemID == 0)
            {
                item = new CoStagesSubs() { SiteCoID = siteusercompanyid };
                db.CoStagesSubs.Add(item);
            }
            else
            {
                item = db.CoStagesSubs.Where(p => p.StageSubID == ItemID && p.SiteCoID == siteusercompanyid).FirstOrDefault();

            }

            if (item != null)
            {
                item.StageSubName = Name;
                item.StageSubOrder = Order;
                item.Display = true;
            }
            db.SaveChanges();
            return Json(new { status = "success", itemID = item.StageSubID });
        }

        public ActionResult Edit(int id)//stage master id
        {
            var stage = db.GetStageMasterEditInfoByID(id).FirstOrDefault();
            if (stage == null)
            {
                return HttpNotFound();
            }

            var subLabors = db.GetSubLaborInfoByMasterStageID(id).ToList();

            var model = new StageModels.StageMasterEdit()
            {
                CostCodeId = stage.CostCodeID,
                MiscEquip = stage.MiscEqp,
                MiscParts = stage.MiscParts,
                Order = stage.Order,
                StageMasterID = stage.StageMasterID,
                StageName = stage.Stage,
            };

            foreach (var sub in subLabors)
            {
                model.StageSubDetails.Add(new StageModels.StageDetails()
                {
                    StageMasterID = id,
                    SubStageID = sub.SubID,
                    SubName = sub.SubStage,
                    CostPerHour = sub.Cost,
                    PricePerHour = sub.Price,
                    Percent = sub.Factor,
                    SellPerHour = sub.Sell
                });
            }

            ViewBag.CostCodes = new SelectList(db.GetCoCostCodesBySiteCoID(siteusercompanyid).ToList(), nameof(GetCoCostCodesBySiteCoID_Result.CostCodeID), nameof(GetCoCostCodesBySiteCoID_Result.CostCode));

            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(StageModels.StageMasterEdit Model)
        {
            var errorList = new List<string>();

            if (ModelState.IsValid)
            {
                var stageMaster = new CoStagesMaster()
                {
                    StageMasterID = Model.StageMasterID,
                    SiteCoID = siteusercompanyid,
                    StageOrder = Model.Order,
                    CostCodeID = Model.CostCodeId,
                    StageMiscEquipAdjt = Model.MiscEquip,
                    StageMiscPartsAdjt = Model.MiscParts
                };
                repo.UpdateStageMaster(stageMaster);

                foreach (var price in Model.StageSubDetails)
                {
                   // price.Percent = price.Percent / 100;
                    var stagePricing = new CoStagesPricing()
                    {
                        StageMasterID = price.StageMasterID,
                        StageSubID = price.SubStageID ?? 0,
                        StageLaborCost = price.CostPerHour,
                        StageLaborPrice = price.PricePerHour,
                        StageLaborFactor = price.Percent / 100
                    };

                    repo.UpdateStagePricing(stagePricing);
                }

                return Json(new { status = "success" });
            }

            errorList.AddRange((from item in ModelState.Values
                                from error in item.Errors
                                select error.ErrorMessage).ToList()
                             );

            return Json(new { status = "error", errors = errorList });
        }
    }
}