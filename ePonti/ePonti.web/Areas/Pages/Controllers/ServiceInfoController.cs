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
using Dropbox.Api;
using System.Threading.Tasks;
using Dropbox.Api.Files;
using System.IO;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;

namespace ePonti.web.Areas.Pages.Controllers
{
    public class ServiceInfoController : web.Controllers._baseMVCController
    {
        private ePontiv2Entities db = new ePontiv2Entities();
        BOL.Repository.CommonRepository repo = new BOL.Repository.CommonRepository();

        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var projInfo = db.GetProjectInfoByProjectID(id).FirstOrDefault();
            if (projInfo == null)
            {
                return HttpNotFound();
            }
            ViewBag.id = id;
            ViewBag.Activities = db.GetProjectInfoActivitiesByProjectID(id).ToList();
            ViewBag.ProjectContacts = db.GetProjectContactsByProjectID(id).ToList();
            ViewBag.Parts = db.GetProjectInfoPartsByProjectID(id).ToList().ToList();
            ViewBag.Cor = db.GetProjectCorByProjectID(id).ToList().ToList();
            ViewBag.Work = db.GetProjectWorkByProjectID(id).ToList();
            ViewBag.PunchList = db.GetProjectPunchListByProjectID(id).ToList();
            ViewBag.Por = db.GetProjectPorListByProjectID(id).ToList();
            ViewBag.Deliviries = db.GetProjectDeliveryListByProjectID(id).ToList();
            ViewBag.Sales = db.GetProjectSoByProjectID(id).ToList();
            ViewBag.Contacts = new SelectList(db.GetContactsBySiteCoID(siteusercompanyid).ToList(), nameof(GetContactsBySiteCoID_Result.ViewID), nameof(GetContactsBySiteCoID_Result.Customer));
            ViewBag.Relationships = new SelectList(repo.GetProjectRelationshipsBySiteCoID(siteusercompanyid).ToList(), nameof(CoProjectRelationships.RelationshipID), nameof(CoProjectRelationships.Relationship));
            var ProjectInfo = db.GetProjectInfoByProjectID(id).FirstOrDefault();
            var dbx = DropboxConnection();
            if (dbx != null)
            {
                var full = await dbx.Users.GetCurrentAccountAsync();
                var list = await dbx.Files.ListFolderAsync(string.Empty);
                foreach (var item in list.Entries.Where(i => i.IsFolder))
                {
                    if (item.Name == ProjectInfo.Project)
                    {
                        var sublist = await dbx.Files.ListFolderAsync("/" + item.Name);
                        ViewBag.FilesList = sublist.Entries.Where(i => i.IsFile).ToList();
                    }
                }
            }
            else
            {
                if (db.ProjectFiles.Where(s => s.ProjectID == id).FirstOrDefault() != null)
                {
                    List<ProjectFilesBOL> files = new List<ProjectFilesBOL>();
                    foreach (var ProjectFile in db.ProjectFiles.Where(s => s.ProjectID == id))
                    {
                        ProjectFilesBOL mt = new ProjectFilesBOL();
                        mt.Name = ProjectFile.File;
                        mt.FilePath = "files/" + siteusercompanyid + "/" + id + "/" + ProjectFile.File;
                        mt.FileId = ProjectFile.ProjectFileID;
                        mt.ClientModified = ProjectFile.UploadedDateTime == null ? DateTime.Now : ProjectFile.UploadedDateTime.Value;
                        files.Add(mt);
                    }
                    if (files.Count > 0)
                    {
                        ViewBag.FilesList = files;
                        ViewBag.LocalFolder = true;
                    }
                }
            }
            return View(projInfo);
        }
        //POST: Pages/ServiceInfo/Details/
        [HttpPost]
        public async Task<ActionResult> Details(int? id, IEnumerable<HttpPostedFileBase> drpfile)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var dbx = DropboxConnection();
            if (dbx != null)
            {
                var ProjectInfo = db.GetProjectInfoByProjectID(id).FirstOrDefault();
                foreach (var files in drpfile)
                {
                    if (files != null)
                    {
                        if (files.ContentLength > 0)
                        {
                            var mbsize = (files.ContentLength / 1024f) / 1024f;
                            if (mbsize <= 150)
                            {
                                var updated = await dbx.Files.UploadAsync(
                                    "/" + ProjectInfo.Project + "/" + files.FileName,
                                    WriteMode.Overwrite.Instance,
                                    body: files.InputStream);
                            }
                            else
                            {
                                ViewBag.WarningMessage = "You can upload maximum 150 MB files only";
                            }
                        }
                    }
                }
            }
            else
            {
                var ProjectInfo = db.GetProjectInfoByProjectID(id).FirstOrDefault();
                string foldername = ProjectInfo.ProjectID.ToString();
                string path = Server.MapPath("~/files/" + siteusercompanyid + "/");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                path = Server.MapPath("~/files/" + siteusercompanyid + "/" + foldername + "/");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                HttpPostedFileBase files = null;
                foreach (string fileName in Request.Files)
                {
                    files = Request.Files[fileName];
                    if (files != null)
                    {
                        if (files.ContentLength > 0)
                        {
                            var mbsize = (files.ContentLength / 1024f) / 1024f;
                            if (mbsize <= 150)
                            {
                                files.SaveAs(path + files.FileName);
                                db.InsertProjectFile(id, null, siteusercompanyid, files.FileName);
                                db.SaveChanges();
                            }
                            else
                            {
                                ViewBag.WarningMessage = "You can upload maximum 150 MB files only";
                            }
                        }
                    }
                }
            }
            return RedirectToAction("Details");
        }

        [HttpPost]
        public ActionResult GetDetails([DataSourceRequest]DataSourceRequest request, int? id, string List)
        {


            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var jobInfo = db.GetProjectInfoByProjectID(id).FirstOrDefault();
            if (jobInfo == null)
            {
                return HttpNotFound();
            }
            var job = db.ProjectInfo.Where(x => x.ProjectID == id).FirstOrDefault();
            if (job != null)
            {
                if (!string.IsNullOrEmpty(job.AcctJobID))
                {
                    ViewBag.IsSynced = true;
                }
            }

            DataSourceResult result = new DataSourceResult();
            if (List == "Activities")
            {
                result = db.GetProjectInfoActivitiesByProjectID(id).ToList().ToDataSourceResult(request, p => new GetProjectInfoActivitiesByProjectID_Result
                {
                    ActivityType = p.ActivityType == "EV" ? "events" : p.ActivityType == "CL" ? "calls" : p.ActivityType == "CA" ? "cases" : p.ActivityType == "NT" ? "notes" : "N/A",
                    ViewID = p.ViewID,
                    ViewImage = p.ViewImage.TrimStart('.'),
                    Date = p.Date,
                    Info = p.Info,
                    Status = p.Status
                });

            }
            if (List == "Parts")
            {
                result = db.GetProjectInfoPartsByProjectID(id).ToList().ToDataSourceResult(request);
            }

            if (List == "Cor")
            {
                result = db.GetProjectCorByProjectID(id).ToList().ToDataSourceResult(request);
            }
            if (List == "Work")
            {
                result = db.GetProjectWorkByProjectID(id).ToList().ToDataSourceResult(request);
            }
            if (List == "PunchList")
            {
                result = db.GetProjectPunchListByProjectID(id).ToList().ToDataSourceResult(request);
            }

            //if (List == "Milestones")
            //    result = db.GetProjectMilestonesListByProjectID(id).ToList().ToDataSourceResult(request);

            if (List == "Contacts")
            {
                result = db.GetProjectContactsByProjectID(id).ToList().ToDataSourceResult(request);
            }
            if (List == "Purchasing")
            {
                result = db.GetProjectPorListByProjectID(id).ToList().ToDataSourceResult(request);
            }
            if (List == "Deliveries")
            {
                result = db.GetProjectDeliveryListByProjectID(id).ToList().ToDataSourceResult(request);
            }
            if (List == "Sales")
            {
                result = db.GetProjectSoByProjectID(id).ToList().ToDataSourceResult(request);

            }

            return Json(result, JsonRequestBehavior.AllowGet);

        }
        
        public ActionResult Create(int? contactid, int? servicetemplateid)
        {
            var siteCoID = siteusercompanyid;

            var siteUsers = db.GetSiteUsersBySiteCoID(siteCoID).ToList();

            ViewBag.Clients = new SelectList(db.GetContactsBySiteCoID(siteCoID), "ViewID", "Customer", contactid);
            ViewBag.ProjectStatus = new SelectList(db.GetServiceStatusBySiteCoID(siteCoID), "ViewID", "Name");
            ViewBag.SiteUsers = new SelectList(siteUsers, "ViewID", "User");

            LoadAdditionalDropdownData();
            ServiceModels.NewService model = new ServiceModels.NewService();
            var newServiceId = 0;
            if (servicetemplateid.HasValue)
            {
                newServiceId = repo.CopyProjectToService(servicetemplateid.Value);
                model = LoadProjectInfo(newServiceId);
            }
            else
            {
                newServiceId = repo.GenerateNewProjectID();
                model.SalesPersonID = siteuserid;
                model.ProjectManagerID = siteuserid;
                model.DesignerID = siteuserid;
            }

            model.ServiceID = newServiceId;
            model.StartDate = DateTime.Now;
            model.JobNumber = db.GetNewServiceNumFormat(siteusercompanyid).FirstOrDefault();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Models.ServiceModels.NewService Model)
        {
            var errorList = new List<string>();

            if (ModelState.IsValid)
            {
                WoReturn woret = SaveService(Model);
                if (woret.WoId > 0)
                {
                    string pagename = woret.HasItems ? "edit" : "AddItem";
                    string WO_URL = "";
                    if (Request.QueryString["nav"] == null)
                    {
                        WO_URL = Url.Action(pagename, "workorders", new { area = "common", id = woret.WoId });
                    }
                    else
                    {
                        WO_URL = Url.Action(pagename, "workorders", new { area = "common", nav= Request.QueryString["nav"], id = woret.WoId });
                    }
                    return Json(new { status = "success", WO_URL = WO_URL });
                    //var ProjectInfo = db.GetProjectInfoByProjectID(projectId).FirstOrDefault();
                    //var checkdropboxauth = db.CoDropbox.Where(p => p.SiteCoID == siteusercompanyid).FirstOrDefault();
                    //if (checkdropboxauth != null)
                    //{
                    //    using (var dbx = new DropboxClient(System.Text.Encoding.UTF8.GetString(checkdropboxauth.AccessToken)))
                    //    {
                    //        var full = await dbx.Users.GetCurrentAccountAsync();
                    //        var list = await dbx.Files.ListFolderAsync(string.Empty);
                    //        bool flag = true;
                    //        // get folders and sub folders
                    //        foreach (var item in list.Entries.Where(i => i.IsFolder))
                    //        {
                    //            if (item.Name == ProjectInfo.Project)
                    //            flag = false;                               
                    //        }
                    //        if (flag == true)
                    //            await dbx.Files.CreateFolderAsync("/" + ProjectInfo.Project, true);
                    //    }
                    //}
                    //return Json(new { status = "success", serviceid = projectId });
                }
                else
                {
                    errorList.Add("Service couldn't be saved. Please retry.");
                }
            }        
            errorList.AddRange((from item in ModelState.Values
                                from error in item.Errors
                                select error.ErrorMessage).ToList()
                             );

            return Json(new { status = "error", errors = errorList });
        }

        private WoReturn SaveService(ServiceModels.NewService Model)
        {
            int siteCoID = siteusercompanyid;

            var quote = new ProjectInfo()
            {
                ProjectID = Model.ServiceID ?? 0,
                LinkID = Model.ParentQuoteID,
                SiteCoID = siteCoID,
                ProjectName = Model.ServiceName,
                ProjectNumber = Model.JobNumber ?? "",
                ProjectStatusID = Model.StatusID,
                ContactID = Model.ClientID,
                ProjectAddress1 = Model.Address1,
                ProjectAddress2 = Model.Address2,
                ProjectCity = Model.City,
                ProjectState = Model.State,
                ProjectCountry = Model.Country,
                ProjectZip = Model.Zip,
                ProjectPhone = Model.ServicePhone,
                ProjectEmail = Model.Email,
                DateUploaded = DateTime.Now,
                SalesID = Model.SalesPersonID,
                ProjectStartDate = Model.StartDate,
                PmID = Model.ProjectManagerID,
                DesignerID = Model.DesignerID,
                ProjectTypeID = 3
            };

            var projectId = repo.SaveProjectInfo(quote);

            

            var woNumber = repo.GetNextWorkOrderNumber(projectId, Model.JobNumber, "");
            ProjectItems item = db.ProjectItems.Where(s => s.ProjectID == projectId).FirstOrDefault();
            int issaved = 0;
            bool HasItems = false;
            if (item == null)
            {
                issaved = db.InsertServiceWo(projectId, siteCoID, woNumber, null);
            }
            else
            {
                issaved = db.InsertServiceWo(projectId, siteCoID, woNumber, item.ItemID);
                HasItems = true;
            }
            int woid = 0;
            if(issaved>0)
            {
                woid = db.ProjectWorkOrders.Where(s => s.WoNumber == woNumber).OrderByDescending(s => s.WoID).FirstOrDefault() != null ? db.ProjectWorkOrders.Where(s => s.WoNumber == woNumber).OrderByDescending(s => s.WoID).FirstOrDefault().WoID : 0;
            }
            //db.GetServiceTemplatesBySiteCoID;

            var ret = new WoReturn { WoId = woid, HasItems = HasItems };
            return ret;
        }

        public class WoReturn
        {
            public int WoId { get; set; }
            public bool HasItems { get; set; }
        }

        public ActionResult Edit(int? id)
        {
            ProjectInfo proj = db.ProjectInfo.Where(p => p.ProjectID == (id ?? 0)).FirstOrDefault();
            if (proj == null)
            {
                return HttpNotFound();
            }
            var model = LoadProjectInfo(id ?? 0);

            return View("_Edit", model);
        }

        //dropdowns for other tabs
        private void LoadAdditionalDropdownData()
        {
            var siteCoID = siteusercompanyid;

            ViewBag.CoStages = new SelectList(db.GetCoStagesBySiteCoID(siteCoID).ToList(), nameof(GetCoStagesBySiteCoID_Result.StageMasterID), nameof(GetCoStagesBySiteCoID_Result.Stage));
            ViewBag.CostCodes = new SelectList(db.GetCoCostCodesBySiteCoID(siteCoID).ToList(), nameof(GetCoCostCodesBySiteCoID_Result.CostCodeID), nameof(GetCoCostCodesBySiteCoID_Result.CostCode));
            ViewBag.PayTypes = new SelectList(db.GetPayTypesBySiteCoID(siteCoID).ToList(), nameof(GetPayTypesBySiteCoID_Result.ViewID), nameof(GetPayTypesBySiteCoID_Result.Name));
            ViewBag.Taxes = new SelectList(db.GetCoTaxesBySiteCoID(siteCoID).ToList(), nameof(GetCoTaxesBySiteCoID_Result.ViewID), nameof(GetCoTaxesBySiteCoID_Result.Tax));
            ViewBag.Areas = new SelectList(db.GetCoAreasBySiteCoID(siteCoID).ToList(), nameof(GetCoAreasBySiteCoID_Result.ViewID), nameof(GetCoAreasBySiteCoID_Result.Area));
            ViewBag.Divisions = new SelectList(db.GetCoDivisionsBySiteCoID(siteCoID).ToList(), nameof(GetCoDivisionsBySiteCoID_Result.ViewID), nameof(GetCoDivisionsBySiteCoID_Result.Divisions));
        }

        //data for grids
        private void LoadAdditionalGridData(int ProjectID)
        {
            ViewBag.ProjectStages = db.GetProjectStagesByProjectID(ProjectID).ToList();
            ViewBag.ProjectCostCodes = db.GetProjectCostCodesByProjectID(ProjectID);
            ViewBag.ProjectPayTypes = db.GetProjectPayTypesByProjectID(ProjectID);
            ViewBag.ProjectTaxes = db.GetProjectTaxesByProjectID(ProjectID);
            ViewBag.ProjectAreas = db.GetProjectAreasByProjectID(ProjectID);
            ViewBag.ProjectDivisions = db.GetProjectDivisionsByProjectID(ProjectID);
            ViewBag.ProjectComm = db.GetProjectCommByProjectID(ProjectID);
        }

        private ServiceModels.NewService LoadProjectInfo(int ProjectID)
        {
            var model = new ServiceModels.NewService();
            model.ServiceID = ProjectID;

            ProjectInfo proj = db.ProjectInfo.Where(p => p.ProjectID == ProjectID).FirstOrDefault();
            if (proj == null)
            {
                return model;
            }

            model = new ServiceModels.NewService()
            {
                ServiceID = proj.ProjectID,
                Address1 = proj.ProjectAddress1,
                Address2 = proj.ProjectAddress2,
                City = proj.ProjectCity,
                ClientID = proj.ContactID,
                Email = proj.ProjectEmail,
                ServicePhone = proj.ProjectPhone,
                JobNumber = proj.ProjectNumber,
                SalesPersonID = proj.SalesID,
                State = proj.ProjectState,
                Country = proj.ProjectCountry,
                StatusID = proj.ProjectStatusID,
                Zip = proj.ProjectZip,
                DesignerID = proj.DesignerID,
                ProjectManagerID = proj.PmID,
                ServiceName = proj.ProjectName,
                StartDate = proj.ProjectStartDate
            };

            var siteCoID = siteusercompanyid;
            var siteUsers = db.GetSiteUsersBySiteCoID(siteCoID).ToList();

            ViewBag.Clients = new SelectList(db.GetContactsBySiteCoID(siteCoID), "ViewID", "Customer");
            ViewBag.ProjectStatus = new SelectList(db.GetServiceStatusBySiteCoID(siteCoID), "ViewID", "Name");
            ViewBag.SiteUsers = new SelectList(siteUsers, "ViewID", "User");

            LoadAdditionalDropdownData();
            LoadAdditionalGridData(ProjectID);

            return model;
        }

        #region Contacts
        public ActionResult GetContactData(int ContactID)
        {
            var addr = db.GetContactAddressByContactID(ContactID).FirstOrDefault();
            var phone = db.GetContactPhoneByContactID(ContactID).FirstOrDefault();
            var email = db.GetContactEmailByContactID(ContactID).FirstOrDefault();

            return Json(new { addr, phone, email }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AddServiceContact(int ServiceID, int ContactID, int RelationshipID)
        {
            repo.AddProjectContact(ServiceID, ContactID, RelationshipID);

            return Json(new { status = "success" });
        }
        #endregion

        #region Stages
        public ActionResult GetStageDetails(int ProjectID, int StageID)
        {
            var res = db.GetSubLaborInfoByProjectID(ProjectID, StageID).ToList();
            // var res = db.GetProjectStageDetailByStageID(ProjectID, StageID).ToList();
            var type = "data";

            //if no stage details added to the project then return default list
            if (!res.Any())
            {
                var stageSubs = db.GetStageSubByStageID(StageID).ToList();
                res = stageSubs.Select(p => new GetSubLaborInfoByProjectID_Result()
                {
                    SubID = p.SubID,
                    SubStage = p.SubStage,
                    Cost = p.Cost,
                    Factor = p.Factor,
                    Sell = p.Sell,
                    Price = p.Price
                }).ToList();
                type = "empty";
            }

            return Json(new { data = res, type }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SaveProjectStages(int ProjectID, List<int> StageIDs)
        {
            var costCodes = new List<GetProjectCostCodesByProjectID_Result>();
            if (ProjectID > 0 && StageIDs != null && StageIDs.Any())
            {
                repo.AddProjectStages(ProjectID, StageIDs.Where(p => p > 0).ToList());
                costCodes = db.GetProjectCostCodesByProjectID(ProjectID).ToList();
            }

            return Json(new { status = "success", costCodes });
        }

        public ActionResult SaveProjectStageDetails(List<StageModels.StageDetails> Data)
        {
            if (Data != null && Data.Any())
            {
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
                var psd = Data.FirstOrDefault();
                if (psd != null)
                {
                    projectID = psd.ProjectID;
                    stageID = psd.StageMasterID;
                    return GetStageDetails(projectID, stageID);
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

        #region Cost Code
        public ActionResult SaveCostCode(int ItemID, int ProjectID)
        {
            var id = repo.SaveProjectCostCode(ItemID, ProjectID);
            var res = db.GetProjectCostCodesByProjectID(ProjectID).Where(p => p.ViewID == id).FirstOrDefault();
            return Json(new { status = "success", data = new { res.ViewID, res.Project_, res.CostCode, itemID = ItemID } }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeleteCostCode(int ItemID, int ProjectID)
        {
            repo.DeleteProjectCostCode(ItemID, ProjectID);
            return Json(new { status = "success" }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Pay Type
        public ActionResult SavePayType(int ItemID, int ProjectID, bool IsDefault)
        {
            var id = repo.SaveProjectPayType(ItemID, ProjectID, siteusercompanyid, IsDefault);
            var res = db.GetProjectPayTypesByProjectID(ProjectID).Where(p => p.ViewID == id).FirstOrDefault();
            return Json(new { status = "success", data = new { res.ViewID, res.PayTypeID, res.Pay_Type, res.Factor, itemID = ItemID, res.Default } }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeletePayType(int ItemID, int ProjectID)
        {
            repo.DeleteProjectPayType(ItemID, ProjectID);
            return Json(new { status = "success" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UpdatePayType(int ItemID, decimal Default, decimal Factor)
        {
            repo.UpdateProjectTax(ItemID, Default, Factor);
            return Json(new { status = "success" }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Tax
        public ActionResult SaveTax(int ItemID, int ProjectID)
        {
            var id = repo.SaveProjectTax(ItemID, ProjectID, siteusercompanyid);
            var res = db.GetProjectTaxesByProjectID(ProjectID).Where(p => p.ViewID == id).FirstOrDefault();
            return Json(new { status = "success", data = new { res.ViewID, res.Tax, res.SalesRate, res.LaborRate, res.CostRate, itemID = ItemID } }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeleteTax(int ItemID, int ProjectID)
        {
            repo.DeleteProjectTax(ItemID, ProjectID);
            return Json(new { status = "success" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UpdateTaxData(int ItemID, decimal Sales, decimal Labor)
        {
            repo.UpdateProjectTax(ItemID, Sales, Labor);
            return Json(new { status = "success" }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Areas
        public ActionResult SaveArea(int? ItemID, int ProjectID, string CustomAreaName, int Order)
        {
            var id = repo.SaveProjectArea(ItemID, ProjectID, siteusercompanyid, CustomAreaName, Order);
            var res = db.GetProjectAreasByProjectID(ProjectID);
            return Json(new { status = "success", data = res }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeleteArea(int ItemID, int ProjectID)
        {
            repo.DeleteProjectArea(ItemID, ProjectID);
            return Json(new { status = "success" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UpdateArea(int ItemID, int Order, int ProjectID)
        {
            repo.UpdateProjectArea(ItemID, Order);
            var areas = db.GetProjectAreasByProjectID(ProjectID);
            return Json(new { status = "success", data = areas }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Division
        public ActionResult SaveDivision(int ItemID, int ProjectID, int Order)
        {
            var id = repo.SaveProjectDivision(ItemID, ProjectID, Order, siteusercompanyid);
            var res = db.GetProjectDivisionsByProjectID(ProjectID);
            return Json(new { status = "success", data = res }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeleteDivision(int ItemID, int ProjectID)
        {
            repo.DeleteProjectDivision(ItemID, ProjectID);
            return Json(new { status = "success" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UpdateDivision(int ItemID, int Order, int ProjectID)
        {
            repo.UpdateProjectDivision(ItemID, Order);
            var division = db.GetProjectDivisionsByProjectID(ProjectID);
            return Json(new { status = "success", data = division }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Communication
        public ActionResult SaveCommunication(int ItemID, int ProjectID)
        {
            var id = repo.SaveProjectCommunication(ItemID, ProjectID, siteusercompanyid);
            var res = db.GetProjectCommByProjectID(ProjectID).Where(p => p.ViewID == id).FirstOrDefault();
            return Json(new { status = "success", data = new { res.ViewID, res.User, itemID = ItemID } }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeleteCommunication(int ItemID, int ProjectID)
        {
            repo.DeleteProjectCommunication(ItemID, ProjectID);
            return Json(new { status = "success" }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        public ActionResult GetServiceParts(int ProjectID)
        {
            var res = db.GetProjectInfoPartsByProjectID(ProjectID).ToList();

            return Json(res, JsonRequestBehavior.AllowGet);
        }
        //Dropbox download
        public async Task<ActionResult> DownloadFiles()
        {
            var dbx = DropboxConnection();
            string path = Request.QueryString["path"].ToString();
            using (var response = await dbx.Files.DownloadAsync(path))
            {
                Byte[] s = await response.GetContentAsByteArrayAsync();
                string[] getextension = response.Response.Name.Split('.');
                return File(s, "application/" + getextension[getextension.Length - 1], response.Response.Name);
            }

        }
        //dropbox connection
        private DropboxClient DropboxConnection()
        {
            var checkdropboxauth = db.GetDropboxInfoBySiteCoID(siteusercompanyid).FirstOrDefault();
            if (checkdropboxauth != null)
            {
                var dbx = new DropboxClient(System.Text.Encoding.UTF8.GetString(checkdropboxauth.Token));
                return dbx;
            }
            else
                return null;
        }

        public ActionResult LaborPartial()
        {
            return PartialView("_LaborPartial");
        }
    }
}
