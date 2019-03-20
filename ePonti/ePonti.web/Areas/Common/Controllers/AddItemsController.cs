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

namespace ePonti.web.Areas.Common.Controllers
{
    public class AddItemsController : ePonti.web.Controllers._baseMVCController
    {
        private ePontiv2Entities db = new ePontiv2Entities();
        BOL.Repository.CommonRepository repo = new BOL.Repository.CommonRepository();

        // This list displays Items to the Project
        public ActionResult List(int ProjectID)
        {
            int siteCoID = base.siteusercompanyid;
            ViewBag.ProjectID = ProjectID;

            // ViewBag.CoItems = db.GetItemsBySiteCoID(siteCoID).ToList();

            ViewBag.Divisions = new SelectList(db.GetDivisionsByProjectID(Convert.ToInt32(ProjectID)), nameof(GetDivisionsByProjectID_Result.ViewID), nameof(GetDivisionsByProjectID_Result.Division));
            ViewBag.Areas = new MultiSelectList(db.GetAreasByProjectID(Convert.ToInt32(ProjectID)), nameof(GetAreasByProjectID_Result.ViewID), nameof(GetAreasByProjectID_Result.Division));
            ViewBag.GroupList = db.GetGroupsBySiteCoID(siteCoID).Where(s => s.Group != null && s.Group != "").ToList();
            ViewBag.ManufacturerList = db.GetManufacturersBySiteCoID(siteCoID).Where(s => s.Name != null && s.Name != "").ToList();
            ViewBag.StageList = db.GetMasterStagesBySiteID(siteCoID).Where(s => s.Stage != null && s.Stage != "").ToList();

            return View();
        }

        [HttpPost]
        public ActionResult GetList([DataSourceRequest]DataSourceRequest request,int ProjectID, string SearchText, string SelectedGroups, string SelectedManufacturers, string SelectedStages)
        {
            try
            {
                int siteCoID = base.siteusercompanyid;
                
                
                ViewBag.Divisions = new SelectList(db.GetDivisionsByProjectID(Convert.ToInt32(ProjectID)), nameof(GetDivisionsByProjectID_Result.ViewID), nameof(GetDivisionsByProjectID_Result.Division));
                ViewBag.Areas = new MultiSelectList(db.GetAreasByProjectID(Convert.ToInt32(ProjectID)), nameof(GetAreasByProjectID_Result.ViewID), nameof(GetAreasByProjectID_Result.Division));
                ViewBag.GroupList = db.GetGroupsBySiteCoID(siteCoID).Where(s => s.Group != null && s.Group != "").ToList();
                ViewBag.ManufacturerList = db.GetManufacturersBySiteCoID(siteCoID).Where(s => s.Name != null && s.Name != "").ToList();
                ViewBag.StageList = db.GetMasterStagesBySiteID(siteCoID).Where(s=>s.Stage !=null && s.Stage !="").ToList();

              

                var CoItems = db.GetItemsBySiteCoID(siteCoID).ToList();
                if (SearchText != null && SearchText != "")
                {
                    SearchText = SearchText.Trim().ToLower();
                    CoItems = CoItems.Where(p => (p.Model == null ? "" : p.Model).ToLower().Contains(SearchText)).ToList();
                }
                char[] sep = new char[] { ',' };
                if (SelectedGroups != null && SelectedGroups != "")

                    CoItems = CoItems.Where(p => SelectedGroups.Split(sep).Contains(Convert.ToString(p.Group))).ToList();

                if (SelectedManufacturers != null && SelectedManufacturers != "")

                    CoItems = CoItems.Where(p => SelectedManufacturers.Split(sep).Contains(Convert.ToString(p.Manufacturer))).ToList();

                if (SelectedStages != null && SelectedStages != "")

                    CoItems = CoItems.Where(p => SelectedStages.Split(sep).Contains(Convert.ToString(p.Stage))).ToList();

                ViewBag.CoItems = CoItems;
                DataSourceResult result = new DataSourceResult();
                result = CoItems.ToDataSourceResult(request);

                return Json(result, JsonRequestBehavior.AllowGet);
               // return View();
                //  return Json(new { status = "success", leads = leads });
            }
            catch (Exception ex)
            {
                return Json(new { status = "error", message = ex.Message });
            }
        }
        
        // Displays Items to add to Kit
        public ActionResult AddKitItems()
        {
            int siteCoID = base.siteusercompanyid;

            ViewBag.CoItems = db.GetItemsBySiteCoID(siteCoID);

            return View();
        }

        // This updates the Co Kits Items once user clicks the [Update] button
        [HttpPost]
        public ActionResult UpdateCoKitItems(int? id,List<int> Items)
        {
            var errorList = new List<string>();
            if (Items.Count > 0)
            {
                foreach (int i in Items)
                {
                    decimal? qty = db.CoData.Where(p => p.MasterItemID == i).Select(p => p.OrderQuantity).FirstOrDefault();
                    CoKitItems cki = new CoKitItems();
                    cki.MasterKitID = (int)id;
                    cki.MasterItemID = i;
                    cki.KitItemQuantity = qty;
                    //db.CoKitItems.Add(cki);
                    db.InsertMasterKitItem(cki.MasterKitID, cki.MasterItemID, cki.KitItemQuantity);
                    db.SaveChanges();
                }
                return Json(new { status = "success", KitID = id });
            }
            else
            {
                errorList.Add("No items selected to add in kits ");
            }
            return Json(new { status = "error", errors = errorList });
        }
        // This updates the Project Items once user clicks the [Replace] button
        [HttpPost]
        public ActionResult UpdateProjectItems(int ProjectID, List<int> AreaIDs, int DivisionID, int Quantity, List<int> Items, List<int> ReplaceItems)
        {
            var res = new BOL.Repository.CommonRepository().AddProjectItems(siteusercompanyid, ProjectID, AreaIDs, DivisionID, Quantity, Items);
            if (res == true && ReplaceItems != null && ReplaceItems.Any())
            {
                new BOL.Repository.CommonRepository().DeleteProjectItems(ProjectID, ReplaceItems);
            }
            return Json(new { status = res });
        }

        public ActionResult DuplicateProjectItems(int ProjectID, List<int> ItemIDs)
        {
            bool res = repo.DuplicateProjectItems(ProjectID, ItemIDs);
            return Json(new { status = res });
        }

        public ActionResult DeleteProjectItems(int ProjectID, List<int> ItemIDs)
        {
            bool res = repo.DeleteProjectItems(ProjectID, ItemIDs);
            return Json(new { status = res });
        }
    }
}