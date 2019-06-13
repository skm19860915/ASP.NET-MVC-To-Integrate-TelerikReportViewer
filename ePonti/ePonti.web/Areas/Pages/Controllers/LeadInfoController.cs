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
using System.Transactions;
using Dropbox.Api;
using System.Threading.Tasks;
using System.Globalization;
using System.Text;
using System.IO;
using Dropbox.Api.Files;

namespace ePonti.web.Areas.Pages.Controllers
{
    [Authorize]
    public class LeadInfoController : ePonti.web.Controllers._baseMVCController
    {
        private ePontiv2Entities db = new ePontiv2Entities();
        BOL.Repository.CommonRepository repo = new BOL.Repository.CommonRepository();

        // GET: Pages/LeadInfo/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var leadInfo = db.GetLeadInfoByProjectId(id).FirstOrDefault();
            if (leadInfo == null)
            {
                return HttpNotFound();
            }
            ViewBag.Activities = db.GetActivitiesByProjectId(id);
            ViewBag.ProjectContacts = db.GetProjectContactsByProjectID(id);
            ViewBag.ContactId = repo.GetContactIdByProjectID(id ?? 0);
            ViewBag.Contacts = new SelectList(db.GetContactsBySiteCoID(siteusercompanyid).ToList(), "ViewID", "Customer");
            ViewBag.Relationships = new SelectList(repo.GetProjectRelationshipsBySiteCoID(siteusercompanyid).ToList(), "RelationshipID", "Relationship");
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
            return View(leadInfo);
        }

        //POST: Pages/LeadInfo/Details/
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
        public ActionResult Create(int? contactid)
        {
            var siteCoID = siteusercompanyid;

            ViewBag.Clients = new SelectList(db.GetContactsBySiteCoID(siteCoID).ToList(), "ViewID", "Customer", contactid);
            ViewBag.ProjectStatus = new SelectList(db.GetLeadStatusBySiteCoID(siteCoID), "ViewID", "Status");
            ViewBag.Types = new SelectList(repo.GetCoLeadTypes(siteCoID), "LeadTypeID", "LeadType");
            ViewBag.Probabilities = new SelectList(db.GetLeadProbabilityBySiteCoID(siteCoID), "ViewID", "LeadProp");
            ViewBag.Phases = new SelectList(db.GetLeadStagesBySiteCoID(siteCoID), "ViewID", "LeadStage");
            ViewBag.Priorities = new SelectList(db.GetLeadRatingsBySiteCoID(siteCoID), "ViewID", "LeadRating");
            ViewBag.Systems = new SelectList(db.GetLeadSystemsBySiteCoID(siteCoID), "ViewID", "LeadSystems");
            ViewBag.Builders = new SelectList(db.GetBuildersBySiteCoID(siteCoID).ToList(), "ViewID", "Builder", contactid);

            var siteUsers = db.GetSiteUsersBySiteCoID(siteCoID).ToList();
            ViewBag.SiteUsers = new SelectList(siteUsers, "ViewID", "User", siteuserid);
            ViewBag.ContactAddress = db.GetContactAddressByContactID(contactid).FirstOrDefault();
            LeadModels.NewLead model = new LeadModels.NewLead()
            {
                JobNumber = db.GetNewLeadNumFormat(siteusercompanyid).FirstOrDefault()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Models.LeadModels.NewLead Model)
        {
            var errorList = new List<string>();
            if (ModelState.IsValid)
            {
                int projectId = SaveLead(Model);
                if (projectId > 0)
                {
                    var ProjectInfo = db.GetProjectInfoByProjectID(projectId).FirstOrDefault();
                    var dbx = DropboxConnection();
                    if (dbx != null)
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
                    return Json(new { status = "success", leadId = projectId });
                }
                else
                {
                    errorList.Add("Lead couldn't be saved. Please retry.");
                }
            }

            errorList.AddRange((from item in ModelState.Values
                                from error in item.Errors
                                select error.ErrorMessage).ToList()
                             );

            return Json(new { status = "error", errors = errorList });
        }

        private int SaveLead(Models.LeadModels.NewLead Model)
        {
            int siteCoID = siteusercompanyid;

            var lead = new ProjectInfo()
            {
                ProjectID = Model.LeadID ?? 0,
                SiteCoID = siteCoID,
                //ProjectTypeID = Model.TypeID,
                ProjectName = Model.LeadName,
                ProjectNumber = Model.JobNumber ?? "",
                ProjectStatusID = Model.StatusID,
                ContactID = Model.ClientID,
                ProjectAddress1 = Model.Address1,
                ProjectAddress2 = Model.Address2,
                ProjectCity = Model.City,
                ProjectState = Model.State,
                ProjectCountry = Model.Country,
                ProjectZip = Model.Zip,
                ProjectPhone = Model.Phone,
                ProjectEmail = Model.Email,
                DateUploaded = DateTime.Now,
                BuilderID = Model.BuilderID,
                Site = Model.Site,
                Lot = Model.Lot,
                SalesID = Model.SalesPersonID,
                ProjectTypeID = 6
            };

            ProjectLeadInfo pli = new ProjectLeadInfo()
            {
                SiteCoID = siteCoID,
                ProjectID = Model.LeadID,
                StatusID = Model.StatusID,
                LeadTypeID = Model.TypeID,
                LeadBudget = Model.Budget,
                RatingID = Model.PriorityID,
                LeadProbabilityID = Model.ProbabilityID,
                LeadCloseDate = Model.CloseDate,
                StageID = Model.LeadPhaseID,
                LeadSystemTypeID = Model.SystemID,
                LeadDateModified = DateTime.Now
            };

            using (TransactionScope tran = new TransactionScope())
            {
                var projectId = repo.SaveProjectInfo(lead);
                var pliId = repo.AddProjectLeadInfo(pli, projectId);
                repo.SaveProjectCommunications(projectId, Model.ProjectCommunicationIDs.Where(p => p > 0));

                tran.Complete();

                return projectId;
            }
        }

        public ActionResult Edit(int? id)
        {
            ProjectInfo proj = db.ProjectInfo.Where(p => p.ProjectID == (id ?? 0)).FirstOrDefault();
            if (proj == null)
            {
                return HttpNotFound();
            }
            ProjectLeadInfo pli = db.ProjectLeadInfo.Where(p => p.ProjectID == (id ?? 0)).FirstOrDefault() ?? new ProjectLeadInfo();

            var siteCoID = siteusercompanyid;

            ViewBag.Clients = new SelectList(db.GetContactsBySiteCoID(siteCoID).ToList(), "ViewID", "Customer");
            ViewBag.Builders = new SelectList(db.GetBuildersBySiteCoID(siteCoID).ToList(), "ViewID", "Builder");
            ViewBag.ProjectStatus = new SelectList(db.GetLeadStatusBySiteCoID(siteCoID), "ViewID", "Status");
            ViewBag.Types = new SelectList(repo.GetCoLeadTypes(siteCoID), "LeadTypeID", "LeadType");
            ViewBag.Probabilities = new SelectList(db.GetLeadProbabilityBySiteCoID(siteCoID), "ViewID", "LeadProp");
            ViewBag.Phases = new SelectList(db.GetLeadStagesBySiteCoID(siteCoID), "ViewID", "LeadStage");
            ViewBag.Priorities = new SelectList(db.GetLeadRatingsBySiteCoID(siteCoID), "ViewID", "LeadRating");
            ViewBag.Systems = new SelectList(db.GetLeadSystemsBySiteCoID(siteCoID), "ViewID", "LeadSystems");
            ViewBag.ProjectCommunications = db.GetProjectCommByProjectID(id ?? 0).ToList();

            var siteUsers = db.GetSiteUsersBySiteCoID(siteCoID).ToList();
            ViewBag.SiteUsers = new SelectList(siteUsers, "ViewID", "User", siteusercompanyid);

            LeadModels.NewLead model = new LeadModels.NewLead();

            model = new LeadModels.NewLead()
            {
                Address1 = proj.ProjectAddress1,
                Address2 = proj.ProjectAddress2,
                Budget = pli.LeadBudget,
                City = proj.ProjectCity,
                ClientID = proj.ContactID,
                CloseDate = pli.LeadCloseDate,
                BuilderID = proj.BuilderID,
                Site = proj.Site,
                Lot = proj.Lot,
                Email = proj.ProjectEmail,
                Phone = proj.ProjectPhone,
                JobNumber = proj.ProjectNumber,
                LeadID = proj.ProjectID,
                LeadName = proj.ProjectName,
                LeadPhaseID = pli.StageID,
                PriorityID = pli.RatingID,
                ProbabilityID = pli.LeadProbabilityID,
                SalesPersonID = proj.SalesID,
                State = proj.ProjectState,
                Country = proj.ProjectCountry,
                StatusID = proj.ProjectStatusID,
                TypeID = pli.LeadTypeID,
                Zip = proj.ProjectZip,
                SystemID = pli.LeadSystemTypeID
            };

            return View("_Edit", model);
        }

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

        public ActionResult GetContactData(int ContactID)
        {
            var addr = db.GetContactAddressByContactID(ContactID).FirstOrDefault();
            var phone = db.GetContactPhoneByContactID(ContactID).FirstOrDefault();
            var email = db.GetContactEmailByContactID(ContactID).FirstOrDefault();

            return Json(new { addr, phone, email }, JsonRequestBehavior.AllowGet);
        }
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
