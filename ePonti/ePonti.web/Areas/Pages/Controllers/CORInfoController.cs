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
using Dropbox.Api.Files;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Text;
using Newtonsoft.Json.Serialization;
using System.IO;

namespace ePonti.web.Areas.Pages.Controllers
{
    public class CORInfoController : web.Controllers._baseMVCController
    {
        private ePontiv2Entities db = new ePontiv2Entities();
        BOL.Repository.CommonRepository repo = new BOL.Repository.CommonRepository();

        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var corInfo = db.GetCorInfoByCorID(id).FirstOrDefault();           
            var cor = db.ProjectCor.Where(p => p.CorID == id).FirstOrDefault();
            if (corInfo == null || cor == null)
            {
                return HttpNotFound();
            }

            ViewBag.Scope = cor.Reason;
            ViewBag.Additions = db.GetCorInfoPartsAddedByCorID(id).ToList();
            ViewBag.Removals = db.GetCorInfoPartsRemovedByCorID(id).ToList();
            int projectId = cor.ProjectID.HasValue ? cor.ProjectID.Value : 0;
            ViewBag.ProjectID = projectId;
            ViewBag.IsApproved = cor.Approved;
            var ProjectInfo = db.GetProjectInfoByProjectID(projectId).FirstOrDefault();
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
                if (db.ProjectFiles.Where(s => s.ProjectID == projectId).FirstOrDefault() != null)
                {
                    List<ProjectFilesBOL> files = new List<ProjectFilesBOL>();
                    foreach (var ProjectFile in db.ProjectFiles.Where(s => s.ProjectID == projectId))
                    {
                        ProjectFilesBOL mt = new ProjectFilesBOL();
                        mt.Name = ProjectFile.File;
                        mt.FilePath = "files/" + siteusercompanyid + "/" + projectId + "/" + ProjectFile.File;
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
            return View(corInfo);
        }
        //POST: Pages/CORInfo/Details/
        [HttpPost]
        public async Task<ActionResult> Details(int? id, int? ProjectId, IEnumerable<HttpPostedFileBase> drpfile)
        {
            if (ProjectId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var dbx = DropboxConnection();
            if (dbx != null)
            {
                var ProjectInfo = db.GetProjectInfoByProjectID(ProjectId).FirstOrDefault();
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
                var ProjectInfo = db.GetProjectInfoByProjectID(ProjectId).FirstOrDefault();
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
                                db.InsertProjectFile(ProjectId, null, siteusercompanyid, files.FileName);
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
            return RedirectToAction("Details", new { id = id });
        }

        public ActionResult Create(int? projectid,string Type)
        {
            var siteCoID = siteusercompanyid;
            ViewBag.typelabel = Type;
            TempData["type"] = Type;
            if (Type == "Change Order Request Info")
            {
                ViewBag.Jobs = new SelectList(db.GetJobsBySiteCoID(siteCoID), nameof(GetJobsBySiteCoID_Result.ViewID), nameof(GetJobsBySiteCoID_Result.Project), projectid);
                ViewBag.Type = new SelectList(db.GetCorTypesBySiteCoID(siteCoID), nameof(GetCorTypesBySiteCoID_Result.ViewID), nameof(GetCorTypesBySiteCoID_Result.Name));
            }
            else if (Type == "Service Quote Info")
            {
                ViewBag.Jobs = new SelectList(db.GetServiceBySiteCoID(siteCoID), nameof(GetServiceBySiteCoID_Result.ViewID), nameof(GetServiceBySiteCoID_Result.Project), projectid);
                ViewBag.Type = new SelectList(db.GetCorTypesBySiteCoID(siteCoID), nameof(GetCorTypesBySiteCoID_Result.ViewID), nameof(GetCorTypesBySiteCoID_Result.Name));
            }
           
            ViewBag.Status = new SelectList(db.GetCorStatusBySiteCoID(siteCoID), nameof(GetCorStatusBySiteCoID_Result.ViewID), nameof(GetCorStatusBySiteCoID_Result.Name));

            var model = new CORModels.NewCOR()
            {
                Date = DateTime.Now,
                Creator = repo.GetUserDisplayName(siteuserid),
                CORNumber = repo.GetNextCorNumber()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CORModels.NewCOR Model)
        {
            var errorList = new List<string>();
            string Type = TempData["type"].ToString();
            if (ModelState.IsValid)
            {
                int corId = SaveCOR(Model, Type);
                if (corId > 0)
                {
                    return Json(new { status = "success", CorID = corId });
                }
                else
                {
                    errorList.Add("COR couldn't be saved. Please retry.");
                }
            }

            errorList.AddRange((from item in ModelState.Values
                                from error in item.Errors
                                select error.ErrorMessage).ToList()
                             );

            return Json(new { status = "error", errors = errorList });
        }

        private int SaveCOR(CORModels.NewCOR Model,string Type)
        {
            int siteCoID = siteusercompanyid;
            var cor = new ProjectCor()
            {
                CorID = Model.CorID ?? 0,
                CorTypeID = Model.TypeID,
                CreatedByUserID = siteuserid,
                ProjectID = Model.JobID,
                CorNumber = Model.CORNumber,
                CorStatusID = Model.StatusID,
                Date = Model.Date,
                Subject = Model.Name                
            };
            if (Type == "Change Order Request Info")
                cor.Cor = true;
                var corId = repo.SaveCor(cor);
           // db.InsertCorItems(corId,)
            return corId;
        }

        public ActionResult Edit(int? id)//CorID
        {
            var corInfo = db.GetCorInfoByCorID(id).FirstOrDefault();
            ViewBag.typelabel = corInfo.TypeLabel;
            ProjectCor cor = db.ProjectCor.Where(p => p.CorID == (id ?? 0)).FirstOrDefault();
            if (cor == null)
            {
                return HttpNotFound();
            }

            var model = new CORModels.NewCOR()
            {
                CorID = cor.CorID,
                CORNumber = cor.CorNumber,
                Creator = repo.GetUserDisplayName(cor.CreatedByUserID ?? siteuserid),
                Date = cor.Date,
                JobID = cor.ProjectID,
                Name = cor.Subject,
                StatusID = cor.CorStatusID,
                TypeID = cor.CorTypeID
            };

            ViewBag.Jobs = new SelectList(db.GetJobsBySiteCoID(siteusercompanyid), nameof(GetJobsBySiteCoID_Result.ViewID), nameof(GetJobsBySiteCoID_Result.Project), model.JobID);
            ViewBag.Type = new SelectList(db.GetCorTypesBySiteCoID(siteusercompanyid), nameof(GetCorTypesBySiteCoID_Result.ViewID), nameof(GetCorTypesBySiteCoID_Result.Name), model.TypeID);
            ViewBag.Status = new SelectList(db.GetCorStatusBySiteCoID(siteusercompanyid), nameof(GetCorStatusBySiteCoID_Result.ViewID), nameof(GetCorStatusBySiteCoID_Result.Name), model.StatusID);

            return View("_Edit", model);
        }

        public ActionResult UpdateScope(int CorID, string Scope)
        {
            bool status = repo.UpdateCorReason(CorID, Scope);
            return Json(new { status = (status ? "success" : "error") }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult AddItems(int ID, int ProjectID)//CorID
        {
            ViewBag.CorID = ID;
            ViewBag.ProjectID = ProjectID;
            ViewBag.Action = "Add";

            ViewBag.Items = db.GetProjectInfoPartsAddBySiteCoID(siteusercompanyid).ToList();
            ViewBag.Divisions = new SelectList(db.GetDivisionsByProjectID(ProjectID), nameof(GetDivisionsByProjectID_Result.ViewID), nameof(GetDivisionsByProjectID_Result.Division));
            ViewBag.Areas = new MultiSelectList(db.GetAreasByProjectID(ProjectID), nameof(GetAreasByProjectID_Result.ViewID), nameof(GetAreasByProjectID_Result.Division));

            return View("ListItems");
        }

        [HttpPost]
        public ActionResult AddItems(int CorID, int ProjectID, List<int> AreaIDs, int DivisionID, int Quantity, List<int> Items)
        {
            var res = new BOL.Repository.CommonRepository().AddCorItems(siteusercompanyid, CorID, ProjectID, AreaIDs, DivisionID, Quantity, Items);
            return Json(new { status = (res ? "success" : "error") });
        }

        [HttpGet]
        public ActionResult RemoveItems(int ID, int ProjectID)//CorID
        {
            ViewBag.CorID = ID;
            ViewBag.ProjectID = ProjectID;
            ViewBag.Action = "Remove";

            ViewBag.Items = db.GetProjectInfoPartsByProjectID(ProjectID).ToList();
            return View("ListItems");
        }

        [HttpPost]
        public ActionResult RemoveItems(int CorID, int ProjectID, List<int> Items)
        {
            var res = new BOL.Repository.CommonRepository().RemoveCorItems(siteusercompanyid, CorID, ProjectID, Items);
            return Json(new { status = (res ? "success" : "error") });
        }

        public ActionResult ApproveCor(int CorID, int ProjectID)
        {
            bool status = repo.ApproveCOR(siteuserid, siteusercompanyid, CorID, ProjectID);
            return Json(new { status = (status ? "success" : "error") });
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

    }
}
