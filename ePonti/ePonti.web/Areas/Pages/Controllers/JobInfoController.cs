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
using System.Threading.Tasks;
using Dropbox.Api;
using Dropbox.Api.Files;
using System.IO;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;

namespace ePonti.web.Areas.Pages.Controllers
{
    public class JobInfoController : web.Controllers._baseMVCController
    {
        QBSyncService syncService = null;
        QBSyncdto syncObjects = null;
        QBModels qbModels = null;
        private ePontiv2Entities db = new ePontiv2Entities();
        BOL.Repository.CommonRepository repo = new BOL.Repository.CommonRepository();

        public async Task<ActionResult> Details(int? id)
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
            ViewBag.id = id;
           // ViewBag.Activities = db.GetProjectInfoActivitiesByProjectID(id).ToList();
            //ViewBag.ProjectContacts = db.GetProjectContactsByProjectID(id).ToList();
            //ViewBag.Parts = db.GetJobInfoPartsByProjectID(id).ToList();
            //ViewBag.Cor = db.GetProjectCorByProjectID(id).ToList();
            //ViewBag.Work = db.GetProjectWorkByProjectID(id).ToList();
           // ViewBag.PunchList = db.GetProjectPunchListByProjectID(id).ToList();
            ViewBag.Milestones = db.GetProjectMilestonesListByProjectID(id).ToList();
            //ViewBag.Por = db.GetProjectPorListByProjectID(id).ToList();
            //ViewBag.Deliviries = db.GetProjectDeliveryListByProjectID(id).ToList();
            //ViewBag.Sales = db.GetProjectSoByProjectID(id).ToList();
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
            return View(jobInfo);
        }


        [HttpPost]
        public ActionResult GetDetails([DataSourceRequest]DataSourceRequest request,int ?id,  string List)
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
                    ActivityType= p.ActivityType =="EV" ?"events" : p.ActivityType =="CL" ? "calls" : p.ActivityType=="CA" ?"cases" :p.ActivityType =="NT" ?"notes" : "N/A"  ,
                    ViewID = p.ViewID,
                    ViewImage= p.ViewImage.TrimStart('.'),
                    Date = p.Date,
                    Info = p.Info,
                    Status = p.Status
                  });
               
            }
            if (List == "Parts") {
                result = db.GetJobInfoPartsByProjectID(id).ToList().ToDataSourceResult(request);
            }
            
            if (List == "Changes")
            {
                result = db.GetProjectCorByProjectID(id).ToList().ToDataSourceResult(request);
            }
            if (List == "Work")
            {
                result = db.GetProjectWorkByProjectID(id).ToList().ToDataSourceResult(request);
            }
                if (List == "PunchList") { 
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
               



            //ViewBag.Cor = db.GetProjectCorByProjectID(id).ToList();
            //ViewBag.Work = db.GetProjectWorkByProjectID(id).ToList();
            // ViewBag.PunchList = db.GetProjectPunchListByProjectID(id).ToList();
             ViewBag.Milestones = db.GetProjectMilestonesListByProjectID(id).ToList();
            //ViewBag.Por = db.GetProjectPorListByProjectID(id).ToList();
           // ViewBag.Deliviries = db.GetProjectDeliveryListByProjectID(id).ToList();
           // ViewBag.Sales = db.GetProjectSoByProjectID(id).ToList();
            ViewBag.Contacts = new SelectList(db.GetContactsBySiteCoID(siteusercompanyid).ToList(), nameof(GetContactsBySiteCoID_Result.ViewID), nameof(GetContactsBySiteCoID_Result.Customer));
            ViewBag.Relationships = new SelectList(repo.GetProjectRelationshipsBySiteCoID(siteusercompanyid).ToList(), nameof(CoProjectRelationships.RelationshipID), nameof(CoProjectRelationships.Relationship));
            var ProjectInfo = db.GetProjectInfoByProjectID(id).FirstOrDefault();


            return Json(result, JsonRequestBehavior.AllowGet);
           
        }
        //POST: Pages/JobInfo/Details/
        [HttpPost]
        public async Task<ActionResult> Details(int? id,string type="")
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var dbx = DropboxConnection();
            if (dbx != null)
            {
                var ProjectInfo = db.GetProjectInfoByProjectID(id).FirstOrDefault();
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

        #region Sync single job
        public ActionResult SyncSingleJob(int? id, string nav)
        {
            QBAuthorizationdto oAuthDetails = new QBOAuthService(new QBAuthorizationdto()).GetAccessToken(this);
            syncService = new QBSyncService(oAuthDetails);
            syncObjects = new QBSyncdto();
            syncObjects.OauthToken = oAuthDetails;
            syncObjects.CompanyId = oAuthDetails.Realmid;
            int parentid = 0;
            syncObjects = syncService.GetDatafromModelJobs(syncObjects, siteusercompanyid, Convert.ToInt32(id), out parentid);
            if (syncObjects.CustomerList.Count > 0)
            {
                syncObjects = syncService.SyncJob(this, syncObjects, (int)id, parentid, siteusercompanyid);
            }
            return RedirectToAction("details", new { id = id, nav = nav });
        }
        #endregion
        public ActionResult Create(int? contactid, int? quoteid, string from)
        {
            var siteCoID = siteusercompanyid;
            var siteUsers = db.GetSiteUsersBySiteCoID(siteCoID).ToList();
            ViewBag.Clients = new SelectList(db.GetContactsBySiteCoID(siteCoID).ToList(), "ViewID", "Customer", contactid);
            ViewBag.ProjectStatus = new SelectList(db.GetJobStatusBySiteCoID(siteCoID), "ViewID", "Name");
            ViewBag.SiteUsers = new SelectList(siteUsers, "ViewID", "User");
            ViewBag.Builders = new SelectList(db.GetBuildersBySiteCoID(siteCoID).ToList(), "ViewID", "Builder", contactid);
            qbModels = new QBModels();
            qbModels.SyncObjectsModel = new QBSyncdto();
            qbModels.OAuthorizationModel = new QBAuthorizationdto();
            qbModels.IsReadySync = false;
            var oAuthModel = new QBOAuthService(qbModels.OAuthorizationModel).IsTokenAvailable(this, siteusercompanyid);
            ViewBag.IsQBConnected = "false";
            if (oAuthModel.IsConnected)
            {
                ViewBag.IsQBConnected = "true";
            }
            LoadAdditionalDropdownData();
            JobModels.NewJob model = new JobModels.NewJob();
            var newJobId = 0;
            if (quoteid.HasValue)
            {
                newJobId = repo.CopyQuoteTojob(quoteid.Value);
                model = LoadProjectInfo(newJobId);
            }
            else
            {
                newJobId = repo.GenerateNewProjectID();
                model.SalesPersonID = siteuserid;
                model.ProjectManagerID = siteuserid;
                model.DesignerID = siteuserid;
            }

            model.JobID = newJobId;
            model.Date = DateTime.Now;
            model.JobNumber = db.GetNewJobNumFormat(siteusercompanyid).FirstOrDefault();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Models.JobModels.NewJob Model)
        {
            var errorList = new List<string>();

            if (ModelState.IsValid)
            {
                int projectId = SaveJob(Model);
                if (projectId > 0)
                {
                    var ProjectInfo = db.GetProjectInfoByProjectID(projectId).FirstOrDefault();
                    var checkdropboxauth = db.CoDropbox.Where(p => p.SiteCoID == siteusercompanyid).FirstOrDefault();
                    if (checkdropboxauth != null)
                    {
                        using (var dbx = new DropboxClient(System.Text.Encoding.UTF8.GetString(checkdropboxauth.AccessToken)))
                        {
                            var full = await dbx.Users.GetCurrentAccountAsync();
                            var list = await dbx.Files.ListFolderAsync(string.Empty);
                            bool flag = true;
                            // get folders and sub folders
                            foreach (var item in list.Entries.Where(i => i.IsFolder))
                            {
                                if (item.Name == ProjectInfo.Project)
                                    flag = false;
                            }
                            if (flag == true)
                                await dbx.Files.CreateFolderAsync("/" + ProjectInfo.Project, true);
                        }
                    }
                    return Json(new { status = "success", jobid = projectId });
                }
                else
                {
                    errorList.Add("Job couldn't be saved. Please retry.");
                }
            }

            errorList.AddRange((from item in ModelState.Values
                                from error in item.Errors
                                select error.ErrorMessage).ToList()
                             );

            return Json(new { status = "error", errors = errorList });
        }

        private int SaveJob(JobModels.NewJob Model)
        {
            int siteCoID = siteusercompanyid;
            var quote = new ProjectInfo()
            {
                ProjectID = Model.JobID ?? 0,
                LinkID = Model.ParentQuoteID,
                SiteCoID = siteCoID,
                ProjectName = Model.JobName,
                ProjectNumber = Model.JobNumber ?? "",
                ProjectStatusID = Model.StatusID,
                ContactID = Model.ClientID,
                ProjectAddress1 = Model.Address1,
                ProjectAddress2 = Model.Address2,
                ProjectCity = Model.City,
                ProjectState = Model.State,
                ProjectCountry = Model.Country,
                ProjectZip = Model.Zip,
                ProjectPhone = Model.JobPhone,
                ProjectEmail = Model.Email,
                DateUploaded = DateTime.Now,
                SalesID = Model.SalesPersonID,
                ProjectStartDate = Model.Date,
                PmID = Model.ProjectManagerID,
                DesignerID = Model.DesignerID,
                BuilderID = Model.BuilderID,
                Site = Model.Site,
                Lot = Model.Lot,
                ProjectTypeID = 4
            };

            var projectId = repo.SaveProjectInfo(quote);
            return projectId;
        }

        public ActionResult Edit(int? id)
        {
            var siteCoID = siteusercompanyid;
            ProjectInfo proj = db.ProjectInfo.Where(p => p.ProjectID == (id ?? 0)).FirstOrDefault();
            ViewBag.Clients = new SelectList(db.GetContactsBySiteCoID(siteCoID).ToList(), "ViewID", "Customer");
            ViewBag.Builders = new SelectList(db.GetBuildersBySiteCoID(siteCoID).ToList(), "ViewID", "Builder");
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

        private JobModels.NewJob LoadProjectInfo(int ProjectID)
        {
            var model = new JobModels.NewJob();
            model.JobID = ProjectID;

            ProjectInfo proj = db.ProjectInfo.Where(p => p.ProjectID == ProjectID).FirstOrDefault();
            if (proj == null)
            {
                return model;
            }

            model = new JobModels.NewJob()
            {
                JobID = proj.ProjectID,
                Address1 = proj.ProjectAddress1,
                Address2 = proj.ProjectAddress2,
                City = proj.ProjectCity,
                ClientID = proj.ContactID,
                Email = proj.ProjectEmail,
                JobPhone = proj.ProjectPhone,
                JobNumber = proj.ProjectNumber,
                SalesPersonID = proj.SalesID,
                State = proj.ProjectState,
                Country = proj.ProjectCountry,
                StatusID = proj.ProjectStatusID,
                Zip = proj.ProjectZip,
                DesignerID = proj.DesignerID,
                ProjectManagerID = proj.PmID,
                JobName = proj.ProjectName,
                BuilderID = proj.BuilderID,
                Site = proj.Site,
                Lot = proj.Lot,
                Date = proj.ProjectStartDate
            };

            var siteCoID = siteusercompanyid;
            var siteUsers = db.GetSiteUsersBySiteCoID(siteCoID).ToList();

            ViewBag.Clients = new SelectList(db.GetContactsBySiteCoID(siteCoID).ToList(), "ViewID", "Customer");
            ViewBag.ProjectStatus = new SelectList(db.GetJobStatusBySiteCoID(siteCoID), "ViewID", "Name");
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

        public ActionResult AddJobContact(int JobID, int ContactID, int RelationshipID)
        {
            repo.AddProjectContact(JobID, ContactID, RelationshipID);

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

        public ActionResult GetJobParts(int ProjectID)
        {
            var res = db.GetJobInfoPartsByProjectID(ProjectID).ToList();

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
        public FileResult DownloadLocalFiles()
        {
            try
            {
                string path = Request.QueryString["path"].ToString();
                string contentType = string.Empty;

                if (path.Contains(".pdf"))
                {
                    contentType = "application/pdf";
                }
                else if (path.Contains(".docx"))
                {
                    contentType = "application/docx";
                }
                else if (path.Contains(".jpg"))
                {
                    contentType = "image/jpg";
                }
                else if (path.Contains(".jpeg"))
                {
                    contentType = "image/jpeg";
                }
                else if (path.Contains(".png"))
                {
                    contentType = "image/png";
                }
                path = Server.MapPath("~/" + path);
                return File(path, contentType, Path.GetFileName(path));
            }
            catch(Exception ex)
            {
                throw;
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
    }
}
