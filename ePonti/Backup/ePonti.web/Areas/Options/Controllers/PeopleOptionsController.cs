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
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Configuration;
using OfficeOpenXml;
using System.Data.Entity.Core.Objects;

namespace ePonti.web.Areas.Options.Controllers
{
    [Authorize]
    public class PeopleOptionsController : ePonti.web.Controllers._baseMVCController
    {
        private ePontiv2Entities db = new ePontiv2Entities();       
        BOL.Repository.CommonRepository repo = new BOL.Repository.CommonRepository();
     
        // GET: Options/PeopleOptions
        public ActionResult Index()
        {
            int siteCoID = base.siteusercompanyid;
            ViewBag.CoPeopleTypes = db.GetPeopleTypesBySiteCoID(siteCoID);
            ViewBag.CoPeopleSubTypes = db.GetContactSubTypesBySiteCoID(siteCoID);
            ViewBag.CoPeopleRelationship = db.GetPeopleRelationshipsBySiteCoID(siteCoID);
            GetContactCustomFieldsBySiteCoID_Result customResults  = db.GetContactCustomFieldsBySiteCoID(siteCoID).FirstOrDefault();

            return View(customResults);
        }
        public ActionResult DeleteOption(int ItemID, string Type)
        {
            var res = repo.DeleteOption(Type, ItemID, siteusercompanyid);
            return Json(new { status = (res == "" ? "success" : res) });
        }
        #region Options Update
        public ActionResult UpdatePeopleType(int ItemID, string Name, int Order, bool IsVendor, bool IsBuilder)
        {
            CoContactTypes item;
            if (ItemID == 0)
            {
                item = new CoContactTypes() { SiteCoID = siteusercompanyid };
                db.CoContactTypes.Add(item);
            }
            else
            {
                item = db.CoContactTypes.Where(p => p.ContactTypeID == ItemID && p.SiteCoID == siteusercompanyid).FirstOrDefault();
            }
            if (item != null)
            {
                item.ContactTypeName = Name;
                item.ContactTypeOrder = Order;
                item.IsVendor = IsVendor;
                item.IsBuilder = IsBuilder;
            }
            db.SaveChanges();
            return Json(new { status = "success", itemID = item.ContactTypeID });
        }

        public ActionResult UpdateRelationship(int ItemID, string Name, int Order)
        {
             CoProjectRelationships item;
            if (ItemID == 0)
            {
                item = new  CoProjectRelationships() { SiteCoID = siteusercompanyid };
                db. CoProjectRelationships.Add(item);
            }
            else
            {
                item = db. CoProjectRelationships.Where(p => p.RelationshipID == ItemID && p.SiteCoID == siteusercompanyid).FirstOrDefault();
            }
            if (item != null)
            {
                item.Relationship = Name;
                item.RelationshipOrder = Order;
            }
            db.SaveChanges();
            return Json(new { status = "success", itemID = item.RelationshipID });
        }
        public ActionResult UpdatePeopleSubType(int ItemID, string Name, int Order)
        {
            CoContactSubtypes item;
            if (ItemID == 0)
            {
                item = new CoContactSubtypes() { SiteCoID = siteusercompanyid };
                db.CoContactSubtypes.Add(item);
            }
            else
            {
                item = db.CoContactSubtypes.Where(p => p.ContactSubtypeID == ItemID && p.SiteCoID == siteusercompanyid).FirstOrDefault();
            }
            if (item != null)
            {
                item.SubtypeName = Name;
                item.ContactSubtypeOrder = Order;
            }
            db.SaveChanges();
            return Json(new { status = "success", itemID = item.ContactSubtypeID });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdatePeopleCustom(GetContactCustomFieldsBySiteCoID_Result Model)
        {
            var errorList = new List<string>();
            if (ModelState.IsValid)
            {
                int ViewId = Model.ViewID;
                if (ViewId > 0)
                {
                    int siteCoID = base.siteusercompanyid;
                    int ret = db.UpdateContactCustomFields(ViewId, siteCoID, (Model.CustomField1.Trim() != "" ? Model.CustomField1.Trim() : null), Model.Show1, (Model.CustomField2.Trim() != "" ? Model.CustomField2.Trim() : null), Model.Show2, (Model.CustomField3.Trim() != "" ? Model.CustomField3.Trim() : null), Model.Show3, (Model.CustomField4.Trim() != "" ? Model.CustomField4.Trim() : null), Model.Show4);
                    return Json(new { status = "success", ViewId });
                }
                else
                {
                    errorList.Add("Contact couldn't be saved. Please retry.");
                }
            }
            errorList.AddRange((from item in ModelState.Values
                                from error in item.Errors
                                select error.ErrorMessage).ToList()
                             );
            return Json(new { status = "error", errors = errorList });
        }

        public FileResult DownloadExcel()
        {
            string path = "/Doc/ImportContacts.xlsx";
            return File(path, "application/vnd.ms-excel", "ImportContacts.xlsx");
        }

        [HttpPost]
        public JsonResult UploadExcelContact(HttpPostedFileBase file)
        {
            int ct = 0;
            try
            {
                var fileitem = Request.Files;
                List<string> data = new List<string>();
                if (file != null)
                {
                    if (file.ContentType == "application/vnd.ms-excel" || file.ContentType == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                    {
                        int siteCoID = base.siteusercompanyid;
                        int siteuserid = base.siteuserid;
                        string filename = file.FileName;

                        if (filename.EndsWith(".xlsx"))
                        {
                            string[] proCodesArray = new string[1];
                            int i = 0;
                            int RowNumber = 0;
                            var tbl = new System.Data.DataTable();
                            using (var excel = new ExcelPackage(file.InputStream))   //Error is not finding ExcelPackage?
                            {

                                var ws = excel.Workbook.Worksheets.First();
                                var ts = excel.Workbook.Worksheets;
                                var hasHeader = true;  // adjust accordingly
                                                       // add DataColumns to DataTable
                                                       //foreach (var firstRowCell in ws.Cells[1, 1, 1, ws.Dimension.End.Column])
                                                       //    tbl.Columns.Add(hasHeader ? firstRowCell.Text
                                                       //        : String.Format("Column {0}", firstRowCell.Start.Column));

                                foreach (var firstRowCell in ws.Cells[2, 1, 2, ws.Dimension.End.Column])
                                    tbl.Columns.Add(hasHeader ? firstRowCell.Text
                                        : String.Format("Column {0}", firstRowCell.Start.Column));

                                // add DataRows to DataTable
                                int startRow = hasHeader ? 3 : 1;
                                for (int rowNum = startRow; rowNum <= ws.Dimension.End.Row; rowNum++)
                                {
                                    var wsRow = ws.Cells[rowNum, 1, rowNum, ws.Dimension.End.Column];
                                    System.Data.DataRow row = tbl.NewRow();
                                    foreach (var cell in wsRow)   //Error is not finding wsRow?
                                        row[cell.Start.Column - 1] = cell.Text;
                                    tbl.Rows.Add(row);

                                    Array.Resize(ref proCodesArray, i + 1);
                                    proCodesArray[i] = Convert.ToString(row.ItemArray[0]).Trim();
                                    i++;
                                    RowNumber++;
                                }
                            }
                            
                            foreach (DataRow a in tbl.Rows)
                            {
                                ct++;
                                try
                                {
                                    SqlConnection con = null;

                                    if (a["@ContactTypeID"].ToString() != "")
                                    {
                                        con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString());

                                        int p = 0;


                                        string result = "";
                                        SqlCommand cmd = new SqlCommand("ImportContacts", con);
                                        cmd.CommandType = CommandType.StoredProcedure;

                                        cmd.Parameters.AddWithValue("@SiteUserID", siteuserid);
                                        int contacttypeid = checkintval(Convert.ToString(a["@ContactTypeID"]), "Type");
                                        int contactsubtypeid = checkintval(Convert.ToString(a["@ContactSubtypeID"]), "SubType", contacttypeid);
                                        cmd.Parameters.AddWithValue("@SiteCoID", siteCoID);
                                        cmd.Parameters.AddWithValue("@ContactCoName", (a["@ContactCoName"] == null ? "" : Convert.ToString(a["@ContactCoName"])));
                                        cmd.Parameters.AddWithValue("@ContactTypeID", contacttypeid);
                                        cmd.Parameters.AddWithValue("@ContactSubtypeID", contactsubtypeid);
                                        cmd.Parameters.AddWithValue("@ContactFirstName", (a["@ContactFirstName"]==null ? "" : Convert.ToString(a["@ContactFirstName"])));
                                        cmd.Parameters.AddWithValue("@ContactLastName", (a["@ContactLastName"] == null ? "" : Convert.ToString(a["@ContactLastName"])));

                                        cmd.Parameters.AddWithValue("@ContactTitle", a["@ContactTitle"]);
                                        cmd.Parameters.AddWithValue("@FaceBook", a["@FaceBook"]);
                                        cmd.Parameters.AddWithValue("@LinkedIn", a["@LinkedIn"]);
                                        cmd.Parameters.AddWithValue("@Skype", a["@Skype"]);
                                        cmd.Parameters.AddWithValue("@Custom1", a["@Custom1"]);
                                        cmd.Parameters.AddWithValue("@Custom2", a["@Custom2"]);
                                        cmd.Parameters.AddWithValue("@Custom3", a["@Custom3"]);
                                        cmd.Parameters.AddWithValue("@Custom4", a["@Custom4"]);
                                        cmd.Parameters.AddWithValue("@ContactReferredBy", a["@ContactReferredBy"]);
                                        cmd.Parameters.AddWithValue("@Address1", a["@Address1"]);
                                        cmd.Parameters.AddWithValue("@Address2", a["@Address2"]);

                                        cmd.Parameters.AddWithValue("@City", a["@City"]);
                                        cmd.Parameters.AddWithValue("@State", a["@State"]);
                                        cmd.Parameters.AddWithValue("@Zip", a["@Zip"]);
                                        cmd.Parameters.AddWithValue("@CountryID", checkintval(Convert.ToString(a["@CountryID"])));
                                        cmd.Parameters.AddWithValue("@EmailAddress", a["@EmailAddress"]);
                                        cmd.Parameters.AddWithValue("@Phone", a["@Phone"]);
                                        cmd.Parameters.AddWithValue("@Ext", a["@Ext"]);

                                        cmd.Parameters.AddWithValue("@MobPhone", a["@OtherPhone"]);
                                        con.Open();
                                        //var result1 = cmd.ExecuteScalar().ToString();
                                        object obj = cmd.ExecuteScalar();
                                        if (obj != null)
                                            result = obj.ToString();

                                        con.Close();

                                    }
                                    else
                                    {
                                        data.Add("<ul>");
                                        if (a["@ContactTypeID"].ToString() == "" || a["@ContactTypeID"].ToString() == null) data.Add(" <li> Record Details Not Valid</li>");
                                        if (a["@ContactFirstName"].ToString() == "" || a["@ContactFirstName"] == null) data.Add(" <li> ContactFirstName is required</li>");
                                        if (a["@ContactTitle"].ToString() == "" || a["@ContactTitle"] == null) data.Add(" <li>ContactTitle is required</li>");

                                        data.Add("</ul>");
                                        data.ToArray();
                                        return Json(data, JsonRequestBehavior.AllowGet);
                                    }

                                }
                                catch (DbEntityValidationException ex)
                                {
                                    foreach (var entityValidationErrors in ex.EntityValidationErrors)
                                    {

                                        foreach (var validationError in entityValidationErrors.ValidationErrors)
                                        {
                                            Response.Write("Property: " + validationError.PropertyName + " Error: " + validationError.ErrorMessage);

                                        }

                                    }
                                }
                            }
                        }

                        return Json("Contacts have been imported successfully", JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        //alert message for invalid file format  
                        data.Add("<ul>");
                        data.Add("<li>Only Excel file format is allowed</li>");
                        data.Add("</ul>");
                        data.ToArray();
                        return Json(data, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    data.Add("<ul>");
                    if (file == null) data.Add("<li>Please choose Excel file</li>");
                    data.Add("</ul>");
                    data.ToArray();
                    return Json(data, JsonRequestBehavior.AllowGet);

                }
            }
            catch(Exception ex)
            {
                Response.Write(ct.ToString());
                throw ex;
            }
        }

        public int checkintval(string val, string type="", int TypeId=0)
        {
            try
            {
                if (type == "")
                {
                    int IntVal = 0;
                    int.TryParse(val, out IntVal);
                    return IntVal;
                }
                else if(type=="Type")
                {
                    CoContactTypes dt = db.CoContactTypes.Where(s => s.ContactTypeName == val).FirstOrDefault();
                    if(dt==null)
                    {
                        dt = new CoContactTypes();
                        dt.ContactTypeName = val;
                        dt.SiteCoID = base.siteusercompanyid;
                        dt.IsBuilder = val.Trim().ToLower() == "builder";
                        dt.IsVendor = val.Trim().ToLower() == "vendor";
                        db.CoContactTypes.Add(dt);
                        db.SaveChanges();
                    }
                    return dt.ContactTypeID;
                }
                else if (type == "SubType")
                {
                    CoContactSubtypes dt = db.CoContactSubtypes.Where(s => s.SubtypeName == val).FirstOrDefault();
                    if (dt == null)
                    {
                        dt = new CoContactSubtypes();
                        dt.SubtypeName = val;
                        dt.SiteCoID = base.siteusercompanyid;
                        dt.ContactTypeID = TypeId;
                        db.CoContactSubtypes.Add(dt);
                        db.SaveChanges();
                    }
                    return dt.ContactSubtypeID;
                }
                else
                {
                    return 0;
                }
            }
            catch
            {
                return 0;
            }
        }

        #endregion

    }

}
