using ePonti.BLL.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace ePonti.BOL.Repository
{
    public class CommonRepository
    {
        private ePontiv2Entities db;

        #region Contructors

        public CommonRepository() : this(new ePontiv2Entities())
        {

        }
        public CommonRepository(ePontiv2Entities Entity)
        {
            this.db = Entity;
        }

        #endregion



        public CoContactCompanies AddCompany(int SiteCoID, string ContactCoName)
        {
            var comp = db.CoContactCompanies.Where(p => p.SiteCoID == SiteCoID && p.ContactCoName == ContactCoName).FirstOrDefault();
            if (comp == null)
            {
                comp = new CoContactCompanies()
                {
                    SiteCoID = SiteCoID,
                    ContactCoName = ContactCoName
                };
                db.CoContactCompanies.Add(comp);
            }

            db.SaveChanges();

            return comp;
        }



        public int SaveProjectInfo(ProjectInfo ProjectInfoToSave)
        {
            bool isNew = ProjectInfoToSave.ProjectID == 0;
            if (ProjectInfoToSave.ProjectID == 0)
            {
                db.ProjectInfo.Add(ProjectInfoToSave);
            }
            else
            {
                var dbData = db.ProjectInfo.Where(p => p.ProjectID == ProjectInfoToSave.ProjectID).FirstOrDefault();
                if (dbData != null)
                {
                    dbData.SiteCoID = ProjectInfoToSave.SiteCoID;
                    dbData.ProjectName = ProjectInfoToSave.ProjectName;
                    dbData.ProjectNumber = ProjectInfoToSave.ProjectNumber;
                    dbData.ProjectStatusID = ProjectInfoToSave.ProjectStatusID;
                    dbData.ContactID = ProjectInfoToSave.ContactID;
                    dbData.ProjectAddress1 = ProjectInfoToSave.ProjectAddress1;
                    dbData.ProjectAddress2 = ProjectInfoToSave.ProjectAddress2;
                    dbData.ProjectCity = ProjectInfoToSave.ProjectCity;
                    dbData.ProjectState = ProjectInfoToSave.ProjectState;
                    dbData.ProjectCountry = ProjectInfoToSave.ProjectCountry;
                    dbData.ProjectZip = ProjectInfoToSave.ProjectZip;
                    dbData.ProjectPhone = ProjectInfoToSave.ProjectPhone;
                    dbData.ProjectEmail = ProjectInfoToSave.ProjectEmail;
                    dbData.DateUploaded = DateTime.Now;
                    dbData.SalesID = ProjectInfoToSave.SalesID;
                    dbData.ProjectStartDate = ProjectInfoToSave.ProjectStartDate;
                    dbData.PmID = ProjectInfoToSave.PmID;
                    dbData.DesignerID = ProjectInfoToSave.DesignerID;
                    dbData.ProjectTypeID = ProjectInfoToSave.ProjectTypeID;
                    dbData.BuilderID = ProjectInfoToSave.BuilderID;
                    dbData.Site = ProjectInfoToSave.Site;
                    dbData.Lot = ProjectInfoToSave.Lot;
                }
            }
            db.SaveChanges();

            return ProjectInfoToSave.ProjectID;
        }



        public bool UpdateProjectInfo(int ItemID, int AreaID, int DivisionID)
        {
            var item = db.ProjectItems.Where(p => p.ItemID == ItemID).FirstOrDefault();
            if (item == null)
            {
                return true;
            }

            item.AreaID = AreaID;
            item.DivisionID = DivisionID;

            db.SaveChanges();

            return true;
        }



        public bool AddProjectItems(int SiteCoID, int ProjectID, List<int> AreaIDs, int DivisionID, int Quantity, List<int> ItemIDs)
        {
            List<ProjectItemPricing> itemPricings = new List<ProjectItemPricing>();

            try
            {

                foreach (var masterItemID in ItemIDs)
                {
                    var coData = db.CoData.Where(p => p.MasterItemID == masterItemID).FirstOrDefault();
                    if (coData == null)
                    {
                        coData = new CoData();
                    }

                    int? stageMasterID = coData.StageID;

                    //if this stage doesn't exist in project then add it
                    if (stageMasterID.HasValue && !db.ProjectStages.Any(p => p.ProjectID == ProjectID && p.StageMasterID == stageMasterID))
                    {
                        AddProjectStages(ProjectID, new List<int>() { stageMasterID.Value });
                    }

                    //if item's CostCode doesn't exist in project then add it
                    if (coData.CostCodeID.HasValue && !db.ProjectCostCodes.Any(p => p.ProjectID == ProjectID && p.CostCodeID == coData.CostCodeID))
                    {
                        SaveProjectCostCode(coData.CostCodeID.Value, ProjectID);
                    }

                    var itemData = db.GetDataForNewProjectItem(ProjectID, masterItemID).ToList();

                    int? projectStageID = (int?)itemData.Where(p => p.DataName == "StageID").Select(p => p.Value).FirstOrDefault();
                    int? costCodeID = (int?)itemData.Where(p => p.DataName == "CostCodeID").Select(p => p.Value).FirstOrDefault();
                    decimal? unitCost = itemData.Where(p => p.DataName == "UnitCost").Select(p => p.Value).FirstOrDefault();
                    decimal? unitPrice = itemData.Where(p => p.DataName == "UnitPrice").Select(p => p.Value).FirstOrDefault();
                    int? projectTaxID = (int?)itemData.Where(p => p.DataName == "ProjectTaxID").Select(p => p.Value).FirstOrDefault();

                    //This needs to be updated for [InsertProjectItem] Stored Procedure
                    foreach (var areaID in AreaIDs)
                    {
                        try
                        {
                            var pi = new ProjectItems()
                            {
                                SiteCoID = SiteCoID,
                                ProjectID = ProjectID,
                                MasterItemID = masterItemID,
                                ComponentID = "0",
                                Qty = Quantity,
                                StageID = projectStageID,
                                DivisionID = DivisionID,
                                AreaID = areaID,
                                Action = "New",
                                UploadedDate = DateTime.Now,
                                SerialNumber = null,
                                IpAddress = null,
                                ProjectKitID = 0,
                                OptionID = 0,
                                ExcludePOR = coData.ExcludePOR,
                                ProductDescription = coData.ProductDescription,
                                SalesDescription = coData.SalesDescription,
                                SKU = coData.SKU,
                                WarranteePart = false,
                                OneOffPart = false
                            };
                            db.InsertProjectItem(pi.SiteCoID, pi.ProjectID, pi.MasterItemID, pi.Qty, pi.DivisionID, pi.AreaID);
                            // db.ProjectItems.Add(pi);
                            db.SaveChanges();

                            //int newItemID = pi.ItemID;
                            //var pip = new ProjectItemPricing()
                            //{
                            //    ItemID = newItemID,
                            //    UnitCost = unitCost,
                            //    UnitPrice = unitPrice,
                            //    UnitHours = coData.EstHrs,
                            //    SalesTaxID = projectTaxID,
                            //    Margin = ((unitPrice - unitCost) / unitPrice),
                            //    Markup = 100 * ((unitPrice - unitCost) / unitCost),
                            //    SalesTaxed = coData.SalesTaxed,
                            //    LaborTaxed = coData.LaborTaxed,
                            //    CostTaxed = coData.PurchasedTaxed,
                            //};

                            //itemPricings.Add(pip);
                        }
                        catch (Exception ex)
                        {
                            ex.Log();
                        }
                    }
                }
                //This needs to be updated / removed based on[InsertProjectItem]
                //foreach (var pip in itemPricings)
                //{
                //  // db.InsertProjectItem(SiteCoID,ProjectID, masterItemID,pip.q
                //}
            }
            catch (Exception ex)
            {
                ex.Log();
                return false;
            }

            return true;
        }



        public int GetProjectIDByItemID(int ItemID)
        {
            return db.ProjectItems.Where(p => p.ItemID == ItemID).Select(p => p.ProjectID).FirstOrDefault();
        }

        public List<int> GetMasterItemIDsByProjectItemIDs(List<int> ProjectItemIDs)
        {
            return db.ProjectItems.Where(p => ProjectItemIDs.Contains(p.ItemID))
                .Select(p => p.MasterItemID ?? 0).ToList();
        }

        public int AddNewSiteCompany(SiteCompanies SiteCompany)
        {
            if (SiteCompany == null || string.IsNullOrWhiteSpace(SiteCompany.CoName))
            {
                throw new ArgumentNullException(nameof(SiteCompany));
            }

            var siteCompanyInDb = db.SiteCompanies.Where(p => p.CoName == SiteCompany.CoName && p.CoZip == SiteCompany.CoZip).FirstOrDefault();
            if (siteCompanyInDb != null)//duplicate company
            {
                return -1;
            }

            SiteCompany.CoVersionID = 1;
            SiteCompany.CoStatusID = 1;
            SiteCompany.CoDateCreated = DateTime.Now;

            db.SiteCompanies.Add(SiteCompany);

            db.SaveChanges();

            return SiteCompany.SiteCoID;
        }

        public bool DuplicateProjectItems(int ProjectID, List<int> ItemIDs)
        {
            if (ItemIDs == null)
            {
                return true;
            }

            var projectItems = db.ProjectItems.Where(p => ItemIDs.Contains(p.ItemID)).AsNoTracking().ToList();
            var projectItemPricings = db.ProjectItemPricing.Where(p => ItemIDs.Contains(p.ItemID)).AsNoTracking().ToList();

            //old, new
            List<Tuple<int, int>> projectItemsMapping = new List<Tuple<int, int>>();

            foreach (var item in projectItems)
            {
                var oldID = item.ItemID;
                db.InsertProjectItem(item.SiteCoID, item.ProjectID, item.MasterItemID, item.Qty, item.DivisionID, item.AreaID);
                // db.ProjectItems.Add(item);
                db.SaveChanges();

                var newID = item.ItemID;
                projectItemsMapping.Add(Tuple.Create(oldID, newID));
            }
            //Update because new SP [InsertProjectItem]
            //foreach (var item in projectItemPricings)
            //{
            //    item.ItemID = projectItemsMapping.Where(p => p.Item1 == item.ItemID).Select(p => p.Item2).FirstOrDefault();
            //    InsertProjectItemPricing(item);
            //}

            return true;
        }

        public bool DeleteProjectItems(int ProjectID, List<int> ItemIDs)
        {
            if (ItemIDs == null)
            {
                return true;
            }

            var projectItems = db.ProjectItems.Where(p => ItemIDs.Contains(p.ItemID)).ToList();
            //var projectItemPricings = db.ProjectItemPricing.Where(p => ItemIDs.Contains(p.ItemID));

            foreach (var item in projectItems)
            {
                item.Action = "Delete";
            }

            db.SaveChanges();

            return true;
        }

        /// <summary>
        /// Returns new job id
        /// </summary>
        public int CopyQuoteTojob(int QuoteId)
        {
            var projectInfo = db.ProjectInfo.Where(p => p.ProjectID == QuoteId).AsNoTracking().FirstOrDefault();
            if (projectInfo == null)
            {
                return GenerateNewProjectID();
            }
            projectInfo.ProjectTypeID = 4;
            projectInfo.LinkID = QuoteId;
            db.ProjectInfo.Add(projectInfo);
            db.SaveChanges();

            var jobID = projectInfo.ProjectID;

            #region Contacts

            var projectContacts = db.ProjectContacts.Where(p => p.ProjectID == QuoteId).AsNoTracking().ToList();
            projectContacts.ForEach(p => { p.ProjectID = jobID; p.ProjectContactID = 0; });
            db.ProjectContacts.AddRange(projectContacts);
            db.SaveChanges();

            #endregion

            #region Areas

            var projectAreas = db.ProjectAreas.Where(p => p.ProjectID == QuoteId).AsNoTracking().ToList();
            var areaMapping = new List<Tuple<int, int>>();      //Old id, New id
            foreach (var item in projectAreas)
            {
                int oldId = item.ProjectAreaID;
                item.ProjectID = jobID;
                db.ProjectAreas.Add(item);
                db.SaveChanges();
                areaMapping.Add(Tuple.Create(oldId, item.ProjectAreaID));
            }

            #endregion


            #region Billing Info
            var projectBillingInfo = db.ProjectBillingInfo.Where(p => p.ProjectID == QuoteId).AsNoTracking().ToList();
            projectBillingInfo.ForEach(p => { p.ProjectID = jobID; p.ProjectBillingID = 0; });
            db.ProjectBillingInfo.AddRange(projectBillingInfo);
            db.SaveChanges();
            #endregion

            #region Communications
            var projectCommunications = db.ProjectCommunications.Where(p => p.ProjectID == QuoteId).AsNoTracking().ToList();
            projectCommunications.ForEach(p => { p.ProjectID = jobID; p.ProjectCommID = 0; });
            db.ProjectCommunications.AddRange(projectCommunications);
            #endregion


            #region Cost Codes
            var projectCostCodes = db.ProjectCostCodes.Where(p => p.ProjectID == QuoteId).AsNoTracking().ToList();
            var costMapping = new List<Tuple<int, int>>();
            foreach (var item in projectCostCodes)
            {
                int oldId = item.ProjectCostCodeID;
                item.ProjectID = jobID;
                db.ProjectCostCodes.Add(item);
                db.SaveChanges();
                costMapping.Add(Tuple.Create(oldId, item.ProjectCostCodeID));
            }
            #endregion

            #region Divisions
            var projectDivisions = db.ProjectDivisions.Where(p => p.ProjectID == QuoteId).AsNoTracking().ToList();
            var divMapping = new List<Tuple<int, int>>();
            foreach (var item in projectDivisions)
            {
                int oldId = item.ProjectDivisionID;
                item.ProjectID = jobID;
                db.ProjectDivisions.Add(item);
                db.SaveChanges();
                divMapping.Add(Tuple.Create(oldId, item.ProjectDivisionID));
            }
            #endregion

            #region Options
            var projectOptions = db.ProjectOptions.Where(p => p.ProjectID == QuoteId).AsNoTracking().ToList();
            projectOptions.ForEach(p => { p.ProjectID = jobID; p.ProjectOptionID = 0; });
            db.ProjectOptions.AddRange(projectOptions);
            #endregion

            #region PayTypes
            var projectPayTypes = db.ProjectPayTypes.Where(p => p.ProjectID == QuoteId).AsNoTracking().ToList();
            projectPayTypes.ForEach(p => { p.ProjectID = jobID; p.ProjectPayTypeID = 0; });
            db.ProjectPayTypes.AddRange(projectPayTypes);
            #endregion

            #region Stages
            var projectStages = db.ProjectStages.Where(p => p.ProjectID == QuoteId).AsNoTracking().ToList();
            var stageMapping = new List<Tuple<int, int>>();
            foreach (var item in projectStages)
            {
                int oldId = item.ProjectStageID;
                item.ProjectID = jobID;
                db.ProjectStages.Add(item);
                db.SaveChanges();
                stageMapping.Add(Tuple.Create(oldId, item.ProjectStageID));
            }
            #endregion

            #region Taxes
            var projectTaxes = db.ProjectTaxes.Where(p => p.ProjectID == QuoteId).AsNoTracking().ToList();
            projectTaxes.ForEach(p => { p.ProjectID = jobID; p.ProjectTaxID = 0; });
            db.ProjectTaxes.AddRange(projectTaxes);
            #endregion

            #region Items
            var projectItems = db.ProjectItems.Where(p => p.ProjectID == QuoteId).AsNoTracking().ToList();
            var itemIDs = projectItems.Select(q => q.ItemID).ToList();
            var projectItemPricing = db.ProjectItemPricing.Where(p => itemIDs.Contains(p.ItemID)).AsNoTracking().ToList();
            var itemMapping = new List<Tuple<int, int>>();
            foreach (var item in projectItems)
            {
                int oldId = item.ItemID;
                item.ProjectID = jobID;
                item.StageID = stageMapping.Where(p => p.Item1 == item.StageID).Select(p => p.Item2).FirstOrDefault();
                item.DivisionID = divMapping.Where(p => p.Item1 == item.DivisionID).Select(p => p.Item2).FirstOrDefault();
                item.AreaID = areaMapping.Where(p => p.Item1 == item.AreaID).Select(p => p.Item2).FirstOrDefault();
                // db.ProjectItems.Add(item);
                db.InsertProjectItem(item.SiteCoID, item.ProjectID, item.MasterItemID, item.Qty, item.DivisionID, item.AreaID);
                db.SaveChanges();
                itemMapping.Add(Tuple.Create(oldId, item.ItemID));
            }
            #endregion
            //this needs to be updated for the Store Procedure modification as there is now just [InsertProjectItem]

            //foreach (var pricing in projectItemPricing)
            //{
            //    var newItemID = itemMapping.Where(p => p.Item1 == pricing.ItemID).Select(p => p.Item2).FirstOrDefault();

            //    db.InsertProjectItemPricing(newItemID, pricing.UnitCost, pricing.UnitPrice, pricing.EquipmentAdjustment,
            //        pricing.MiscPartsAdjustment, pricing.UnitHours, pricing.FixedLaborPrice, pricing.FixedLaborCost, pricing.LaborCalcMethod,
            //        pricing.SalesTaxID, pricing.Margin, pricing.Markup, pricing.SalesTaxed, pricing.LaborTaxed, pricing.CostTaxed);
            //}

            #region Project Kits
            var projectKits = db.ProjectKits.Where(p => p.ProjectID == QuoteId).AsNoTracking().ToList();
            var projectKitItems = db.ProjectKitItems.Where(p => projectKits.Select(q => q.ProjectKitID).ToList().Contains(p.ProjectKitID)).AsNoTracking().ToList();
            var kitMapping = new List<Tuple<int, int>>();
            foreach (var item in projectKits)
            {
                int oldId = item.ProjectKitID;
                item.ProjectID = jobID;
                db.ProjectKits.Add(item);
                db.SaveChanges();
                kitMapping.Add(Tuple.Create(oldId, item.ProjectKitID));
            }
            #endregion

            #region Project Kit Items
            foreach (var item in projectKitItems)
            {
                int oldId = item.ProjectKitID;
                item.ProjectKitID = kitMapping.Where(p => p.Item1 == item.ProjectKitID).Select(p => p.Item2).FirstOrDefault();
                item.ItemID = itemMapping.Where(p => p.Item1 == item.ItemID).Select(p => p.Item2).FirstOrDefault();
                db.ProjectKitItems.Add(item);
                db.SaveChanges();
            }
            #endregion

            db.SaveChanges();

            return projectInfo.ProjectID;
        }

        /// <summary>
        /// Returns new job id
        /// </summary>
        public int CopyProjectToService(int ProjectId)
        {
            var projectInfo = db.ProjectInfo.Where(p => p.ProjectID == ProjectId).AsNoTracking().FirstOrDefault();
            if (projectInfo == null)
            {
                return GenerateNewProjectID();
            }
            projectInfo.ProjectTypeID = 3;
            projectInfo.LinkID = ProjectId;
            db.ProjectInfo.Add(projectInfo);
            db.SaveChanges();

            var jobID = projectInfo.ProjectID;

            #region Areas

            var projectAreas = db.ProjectAreas.Where(p => p.ProjectID == ProjectId).AsNoTracking().ToList();
            var areaMapping = new List<Tuple<int, int>>();      //Old id, New id
            foreach (var item in projectAreas)
            {
                int oldId = item.ProjectAreaID;
                item.ProjectID = jobID;
                db.ProjectAreas.Add(item);
                db.SaveChanges();
                areaMapping.Add(Tuple.Create(oldId, item.ProjectAreaID));
            }

            #endregion


            #region Billing Info
            var projectBillingInfo = db.ProjectBillingInfo.Where(p => p.ProjectID == ProjectId).AsNoTracking().ToList();
            projectBillingInfo.ForEach(p => { p.ProjectID = jobID; p.ProjectBillingID = 0; });
            db.ProjectBillingInfo.AddRange(projectBillingInfo);
            db.SaveChanges();
            #endregion

            #region Communications
            var projectCommunications = db.ProjectCommunications.Where(p => p.ProjectID == ProjectId).AsNoTracking().ToList();
            projectCommunications.ForEach(p => { p.ProjectID = jobID; p.ProjectCommID = 0; });
            db.ProjectCommunications.AddRange(projectCommunications);
            #endregion


            #region Cost Codes
            var projectCostCodes = db.ProjectCostCodes.Where(p => p.ProjectID == ProjectId).AsNoTracking().ToList();
            var costMapping = new List<Tuple<int, int>>();
            foreach (var item in projectCostCodes)
            {
                int oldId = item.ProjectCostCodeID;
                item.ProjectID = jobID;
                db.ProjectCostCodes.Add(item);
                db.SaveChanges();
                costMapping.Add(Tuple.Create(oldId, item.ProjectCostCodeID));
            }
            #endregion

            #region Divisions
            var projectDivisions = db.ProjectDivisions.Where(p => p.ProjectID == ProjectId).AsNoTracking().ToList();
            var divMapping = new List<Tuple<int, int>>();
            foreach (var item in projectDivisions)
            {
                int oldId = item.ProjectDivisionID;
                item.ProjectID = jobID;
                db.ProjectDivisions.Add(item);
                db.SaveChanges();
                divMapping.Add(Tuple.Create(oldId, item.ProjectDivisionID));
            }
            #endregion

            #region Options
            var projectOptions = db.ProjectOptions.Where(p => p.ProjectID == ProjectId).AsNoTracking().ToList();
            projectOptions.ForEach(p => { p.ProjectID = jobID; p.ProjectOptionID = 0; });
            db.ProjectOptions.AddRange(projectOptions);
            #endregion

            #region PayTypes
            var projectPayTypes = db.ProjectPayTypes.Where(p => p.ProjectID == ProjectId).AsNoTracking().ToList();
            projectPayTypes.ForEach(p => { p.ProjectID = jobID; p.ProjectPayTypeID = 0; });
            db.ProjectPayTypes.AddRange(projectPayTypes);
            #endregion

            #region Stages
            var projectStages = db.ProjectStages.Where(p => p.ProjectID == ProjectId).AsNoTracking().ToList();
            var stageMapping = new List<Tuple<int, int>>();
            foreach (var item in projectStages)
            {
                int oldId = item.ProjectStageID;
                item.ProjectID = jobID;
                db.ProjectStages.Add(item);
                db.SaveChanges();
                stageMapping.Add(Tuple.Create(oldId, item.ProjectStageID));
            }
            #endregion

            #region Taxes
            var projectTaxes = db.ProjectTaxes.Where(p => p.ProjectID == ProjectId).AsNoTracking().ToList();
            projectTaxes.ForEach(p => { p.ProjectID = jobID; p.ProjectTaxID = 0; });
            db.ProjectTaxes.AddRange(projectTaxes);
            #endregion

            #region Items
            var projectItems = db.ProjectItems.Where(p => p.ProjectID == ProjectId).AsNoTracking().ToList();
            var itemIDs = projectItems.Select(q => q.ItemID).ToList();
            var projectItemPricing = db.ProjectItemPricing.Where(p => itemIDs.Contains(p.ItemID)).AsNoTracking().ToList();
            var itemMapping = new List<Tuple<int, int>>();
            foreach (var item in projectItems)
            {
                int oldId = item.ItemID;
                item.ProjectID = jobID;
                item.StageID = stageMapping.Where(p => p.Item1 == item.StageID).Select(p => p.Item2).FirstOrDefault();
                item.DivisionID = divMapping.Where(p => p.Item1 == item.DivisionID).Select(p => p.Item2).FirstOrDefault();
                item.AreaID = areaMapping.Where(p => p.Item1 == item.AreaID).Select(p => p.Item2).FirstOrDefault();
                // db.ProjectItems.Add(item);
                db.InsertProjectItem(item.SiteCoID, item.ProjectID, item.MasterItemID, item.Qty, item.DivisionID, item.AreaID);
                db.SaveChanges();
                itemMapping.Add(Tuple.Create(oldId, item.ItemID));
            }
            #endregion
            //this needs to be updated for the Store Procedure modification as there is now just [InsertProjectItem]

            //foreach (var pricing in projectItemPricing)
            //{
            //    var newItemID = itemMapping.Where(p => p.Item1 == pricing.ItemID).Select(p => p.Item2).FirstOrDefault();

            //    db.InsertProjectItemPricing(newItemID, pricing.UnitCost, pricing.UnitPrice, pricing.EquipmentAdjustment,
            //        pricing.MiscPartsAdjustment, pricing.UnitHours, pricing.FixedLaborPrice, pricing.FixedLaborCost, pricing.LaborCalcMethod,
            //        pricing.SalesTaxID, pricing.Margin, pricing.Markup, pricing.SalesTaxed, pricing.LaborTaxed, pricing.CostTaxed);
            //}

            #region Project Kits
            var projectKits = db.ProjectKits.Where(p => p.ProjectID == ProjectId).AsNoTracking().ToList();
            var projectKitItems = db.ProjectKitItems.Where(p => projectKits.Select(q => q.ProjectKitID).ToList().Contains(p.ProjectKitID)).AsNoTracking().ToList();
            var kitMapping = new List<Tuple<int, int>>();
            foreach (var item in projectKits)
            {
                int oldId = item.ProjectKitID;
                item.ProjectID = jobID;
                db.ProjectKits.Add(item);
                db.SaveChanges();
                kitMapping.Add(Tuple.Create(oldId, item.ProjectKitID));
            }
            #endregion

            #region Project Kit Items
            foreach (var item in projectKitItems)
            {
                int oldId = item.ProjectKitID;
                item.ProjectKitID = kitMapping.Where(p => p.Item1 == item.ProjectKitID).Select(p => p.Item2).FirstOrDefault();
                item.ItemID = itemMapping.Where(p => p.Item1 == item.ItemID).Select(p => p.Item2).FirstOrDefault();
                db.ProjectKitItems.Add(item);
                db.SaveChanges();
            }
            #endregion

            db.SaveChanges();

            return projectInfo.ProjectID;
        }



        public string GetUserDisplayName(int SiteUserID)
        {
            return db.SiteUsers.Where(p => p.SiteUserID == SiteUserID).Select(p => p.UserDisplayName).FirstOrDefault();
        }

        public int GenerateNewProjectID()
        {
            var proj = new ProjectInfo();
            proj.ProjectName = "NEW PROJECT";

            db.ProjectInfo.Add(proj);
            db.SaveChanges();

            return proj.ProjectID;
        }

        public List<CoProjectRelationships> GetProjectRelationshipsBySiteCoID(int SiteCoID)
        {
            return db.CoProjectRelationships.Where(p => p.SiteCoID == SiteCoID).ToList();
        }

        public int AddProjectLeadInfo(ProjectLeadInfo ProjectLeadInfoToSave, int ProjectId)
        {
            ProjectLeadInfoToSave.ProjectID = ProjectId;
            var dbData = db.ProjectLeadInfo.Where(p => p.ProjectID == ProjectLeadInfoToSave.ProjectID).FirstOrDefault();
            if (dbData == null)
            {
                db.ProjectLeadInfo.Add(ProjectLeadInfoToSave);
            }
            else
            {
                dbData.SiteCoID = ProjectLeadInfoToSave.SiteCoID;
                //dbData.ProjectID = ProjectLeadInfoToSave.ProjectID
                dbData.StatusID = ProjectLeadInfoToSave.StatusID;
                dbData.LeadTypeID = ProjectLeadInfoToSave.LeadTypeID;
                dbData.LeadBudget = ProjectLeadInfoToSave.LeadBudget;
                dbData.RatingID = ProjectLeadInfoToSave.RatingID;
                dbData.LeadProbabilityID = ProjectLeadInfoToSave.LeadProbabilityID;
                dbData.LeadCloseDate = ProjectLeadInfoToSave.LeadCloseDate;
                dbData.StageID = ProjectLeadInfoToSave.StageID;
                dbData.LeadSystemTypeID = ProjectLeadInfoToSave.LeadSystemTypeID;
                dbData.LeadDateModified = ProjectLeadInfoToSave.LeadDateModified;
            }
            db.SaveChanges();

            return ProjectLeadInfoToSave.ProjectLeadID;
        }

        public int GetContactIdByProjectID(int ProjectID)
        {
            return db.ProjectInfo.Where(p => p.ProjectID == ProjectID).Select(p => p.ContactID).FirstOrDefault() ?? 0;
        }

        public List<CoProjectStatus> GetCoProjectStatus(int SiteCoID, int? ProejctTypeID)
        {
            return db.CoProjectStatus
                .Where(p => p.SiteCoID == SiteCoID
                    && (!ProejctTypeID.HasValue || ProejctTypeID.Value == p.ProjectTypeID))
                .OrderBy(p => p.ProjectStatusOrder).ToList();
        }

        public List<CoLeadTypes> GetCoLeadTypes(int SiteCoID)
        {
            return db.CoLeadTypes.Where(p => p.SiteCoID == SiteCoID).OrderBy(p => p.LeadTypeOrder).ToList();
        }

        public IEnumerable<CoCaseTypes> GetCaseTypesBySiteCoID(int SiteCoId)
        {
            return db.CoCaseTypes.Where(p => p.SiteCoID == SiteCoId).OrderByDescending(p => p.CaseTypeOrder);
        }



        public List<CoPunchListTypes> GetPunchListTypesBySiteCoID(int SiteCoId)
        {
            return db.CoPunchListTypes.Where(p => p.SiteCoID == SiteCoId)
                //.OrderByDescending(p => (p.Default ?? false))
                .OrderBy(p => p.PunchListTypeOrder).ToList();
        }

        public void SaveProjectCommunications(int ProjectID, IEnumerable<int> SiteUserIDs)
        {
            var newProjComms = SiteUserIDs.Select(p => new ProjectCommunications()
            {
                ProjectID = ProjectID,
                SiteUserID = p
            }).ToList();

            var commInDB = db.ProjectCommunications.Where(p => p.ProjectID == ProjectID).ToList();

            //add items, that are new
            foreach (var comm in newProjComms)
            {
                var isCurrentInDB = commInDB.Any(p => p.SiteUserID == comm.SiteUserID);
                if (!isCurrentInDB)
                {
                    db.ProjectCommunications.Add(comm);
                }
            }

            //delete item, that are not present in new list
            foreach (var inDB in commInDB)
            {
                var isCurrentItemDeleted = !newProjComms.Any(p => p.SiteUserID == inDB.SiteUserID);

                if (isCurrentItemDeleted)
                {
                    db.ProjectCommunications.Remove(inDB);
                }
            }

            db.SaveChanges();
        }


        public List<CoPunchListDepartments> GetPunchListDepartmentsBySiteCoID(int SiteCoId)
        {
            return db.CoPunchListDepartments.Where(p => p.SiteCoID == SiteCoId).OrderBy(p => p.ItemDepartmentOrder).ToList();
        }

        public List<ProjectDivisions> GetDivisionsByProjectID(int ProjectID)
        {
            return db.ProjectDivisions.Where(p => p.ProjectID == ProjectID).OrderBy(p => p.DivisionOrder).ToList();
        }

        public List<ProjectAreas> GetAreasByProjectID(int ProjectID)
        {
            return db.ProjectAreas.Where(p => p.ProjectID == ProjectID).OrderBy(p => p.AreaOrder).ToList();
        }



        public IEnumerable<Tuple<int, string>> GetPrioritiesList()
        {
            return new List<Tuple<int, string>>() {
                Tuple.Create((int)EnumWrapper.Priority.Low,EnumWrapper.Priority.Low.ToString()),
                Tuple.Create((int)EnumWrapper.Priority.Medium,EnumWrapper.Priority.Medium.ToString()),
                Tuple.Create((int)EnumWrapper.Priority.High,EnumWrapper.Priority.High.ToString())
            };
        }

        public int AddContact(CoContacts coContact)
        {
            if (coContact == null)
            {
                return 0;
            }

            db.CoContacts.Add(coContact);
            db.SaveChanges();
            return coContact.ContactID;
        }

        public void AddProjectContact(int ProjectID, int ContactID, int RelationshipID)
        {
            db.ProjectContacts.Add(
                new ProjectContacts()
                {
                    ContactID = ContactID,
                    ProjectID = ProjectID,
                    ProjectRelationshipID = RelationshipID
                }
                );

            db.SaveChanges();
        }

        public void AddProjectContacts(List<ProjectContacts> ProjectContacts, int ProjectID)
        {
            ProjectContacts.ForEach(p => p.ProjectID = ProjectID);

            db.ProjectContacts.AddRange(ProjectContacts);
            db.SaveChanges();
        }

        public IQueryable<ProjectContacts> GetProjectContactsByProjectID(int ProjectID)
        {
            return db.ProjectContacts.Where(p => p.ProjectID == ProjectID);
        }



        public void UpdatePhone(int ContactID, int PhoneTypeID, string Phone, bool IsDefault)
        {
            var phone = db.CoContactPhones.Where(p => p.ContactID == ContactID && p.PhoneTypeID == PhoneTypeID).FirstOrDefault();
            if (phone == null)
            {
                phone = new CoContactPhones()
                {
                    ContactID = ContactID
                };
                db.CoContactPhones.Add(phone);
            }
            phone.PhoneTypeID = PhoneTypeID;
            phone.Phone = Phone;
            phone.IsDefault = IsDefault;

            db.SaveChanges();
        }



        public void UpdateEmail(int ContactID, string Email, bool IsDefault)
        {
            var email = db.CoContactEmails.Where(p => p.ContactID == ContactID).FirstOrDefault();
            if (email == null)
            {
                email = new CoContactEmails()
                {
                    ContactID = ContactID
                };
                db.CoContactEmails.Add(email);
            }
            email.EmailAddress = Email;
            email.IsDefault = true;

            db.SaveChanges();
        }

        public void AddProjectStages(int ProjectID, IEnumerable<int> StageIDs)
        {
            if (StageIDs == null || !StageIDs.Any())
            {
                return;
            }

            //var itemsInDB = db.ProjectStages.Where(p => p.ProjectID == ProjectID).ToList();
            //var newItems = StageIDs.Select(p => new ProjectStages()
            //{
            //    ProjectID = ProjectID,
            //    StageMasterID = p,

            //}).ToList();

            //check all new items. If some is not in db then add
            //foreach (var item in newItems)
            //{
            //    if (!itemsInDB.Any(p => p.StageMasterID == item.StageMasterID))
            //    {
            //        db.ProjectStages.Add(item);
            //    }
            //}


            foreach (var stageID in StageIDs)
            {
                var stages = CreateStages(ProjectID, stageID);
                db.ProjectStages.AddRange(stages);

                //if project doesn't have CostCode related to this stage, then add it
                int? costCodeID = db.CoStagesMaster.Where(p => p.StageMasterID == stageID).Select(p => p.CostCodeID).FirstOrDefault();
                if (costCodeID.HasValue && !db.ProjectCostCodes.Any(p => p.ProjectID == ProjectID && p.CostCodeID == costCodeID.Value))
                {
                    SaveProjectCostCode(costCodeID.Value, ProjectID);
                }
            }

            db.SaveChanges();
        }
        private List<ProjectStages> CreateStages(int ProjectID, int StageMasterID)
        {
            List<ProjectStages> res = new List<ProjectStages>();

            var coStageMaster = db.CoStagesMaster.Where(p => p.StageMasterID == StageMasterID).FirstOrDefault();
            if (coStageMaster == null)
            {
                coStageMaster = new CoStagesMaster();
            }

            var pricings = db.CoStagesPricing.Where(p => p.StageMasterID == StageMasterID).ToList();
            foreach (var pricing in pricings)
            {
                var stage = new ProjectStages()
                {
                    ProjectID = ProjectID,
                    StageMasterID = StageMasterID,
                    StageSubID = pricing.StageSubID,
                    StageLaborCost = pricing.StageLaborCost,
                    StageLaborPrice = pricing.StageLaborPrice,
                    StageLaborFactor = pricing.StageLaborFactor,
                    StageDifficultyFactor = 1,
                    StageMiscPartsFactor = 1,
                    StageMiscProductFactor = 1,
                    StageIgnoreLabor = false,
                    StageMiscEquipAdjt = coStageMaster.StageMiscEquipAdjt,
                    StageMiscPartsAdjt = coStageMaster.StageMiscPartsAdjt
                };

                res.Add(stage);
            }

            return res;
        }

        public void AddEventInvitees(int EventID, List<ActivitiesEventInvitees> Invitees)
        {
            Invitees.ForEach(p => p.EventID = EventID);

            db.ActivitiesEventInvitees.AddRange(Invitees);
            db.SaveChanges();
        }

        public void UpdateEventInvitees(int EventID, List<ActivitiesEventInvitees> Invitees)
        {
            Invitees.ForEach(p => p.EventID = EventID);

            var inviteesInDB = db.ActivitiesEventInvitees.Where(p => p.EventID == EventID).ToList();

            //add data that is not already present
            foreach (var newInvitee in Invitees)
            {
                var newData = inviteesInDB.Where(p => p.SiteUserID == newInvitee.SiteUserID && p.ContactID == newInvitee.SiteUserID).FirstOrDefault();
                if (newData == null)
                {
                    db.ActivitiesEventInvitees.Add(newInvitee);
                }
            }

            //udpate data already in db
            foreach (var inviteeInDB in inviteesInDB)
            {
                var newData = Invitees.Where(p => p.SiteUserID == inviteeInDB.SiteUserID && p.ContactID == inviteeInDB.SiteUserID).FirstOrDefault();

                //if data present then update it
                if (newData != null)
                {
                    inviteeInDB.SiteUserID = newData.SiteUserID;
                    inviteeInDB.ContactID = newData.ContactID;
                }
                else // else delete it
                {
                    db.ActivitiesEventInvitees.Remove(inviteeInDB);
                }
            }

            db.SaveChanges();
        }



        public int SavePunchList(ProjectPunchLists PunchListToSave)
        {
            if (PunchListToSave.ProjectPunchListID == 0)
            {
                db.ProjectPunchLists.Add(PunchListToSave);

                PunchListToSave.Number = db.ProjectPunchLists.Max(p => p.Number ?? 0) + 1;
            }
            else
            {
                var dbCase = db.ProjectPunchLists.Where(p => p.ProjectPunchListID == PunchListToSave.ProjectPunchListID).FirstOrDefault();
                if (dbCase != null)
                {
                    dbCase.SiteCoID = PunchListToSave.SiteCoID;
                    dbCase.ProjectID = PunchListToSave.ProjectID;
                    dbCase.Subject = PunchListToSave.Subject;
                    dbCase.TypeID = PunchListToSave.TypeID;
                    dbCase.PriorityID = PunchListToSave.PriorityID;
                    dbCase.StatusID = PunchListToSave.StatusID;
                    dbCase.CreatedByUserID = PunchListToSave.CreatedByUserID;
                    dbCase.DepartmentID = PunchListToSave.DepartmentID;
                    dbCase.DivisionID = PunchListToSave.DivisionID;
                    dbCase.AreaID = PunchListToSave.AreaID;
                    dbCase.Note = PunchListToSave.Note;
                    dbCase.EstHrs = PunchListToSave.EstHrs;
                    dbCase.DueDate = PunchListToSave.DueDate;
                }
            }

            db.SaveChanges();

            return PunchListToSave.ProjectPunchListID;
        }

        public int SaveProjectStageDetails(ProjectStages ProjectStageToSave)
        {
            if (ProjectStageToSave.ProjectStageID == 0)
            {
                db.ProjectStages.Add(ProjectStageToSave);
            }
            else
            {
                var dbItem = db.ProjectStages.Where(p => p.ProjectStageID == ProjectStageToSave.ProjectStageID).FirstOrDefault();
                if (dbItem != null)
                {
                    dbItem.ProjectID = ProjectStageToSave.ProjectID;
                    dbItem.ProjectStageID = ProjectStageToSave.ProjectStageID;
                    dbItem.StageSubID = ProjectStageToSave.StageSubID > 0 ? ProjectStageToSave.StageSubID : dbItem.StageSubID;
                    dbItem.StageLaborCost = ProjectStageToSave.StageLaborCost;
                    dbItem.StageLaborPrice = ProjectStageToSave.StageLaborPrice;
                    dbItem.StageMasterID = ProjectStageToSave.StageMasterID;
                    dbItem.StageLaborFactor = ProjectStageToSave.StageLaborFactor;
                    dbItem.StageDifficultyFactor = ProjectStageToSave.StageDifficultyFactor ?? dbItem.StageDifficultyFactor;
                    dbItem.StageIgnoreLabor = ProjectStageToSave.StageIgnoreLabor != null ? ProjectStageToSave.StageIgnoreLabor : dbItem.StageIgnoreLabor;
                    dbItem.StageMiscEquipAdjt = ProjectStageToSave.StageMiscEquipAdjt ?? dbItem.StageMiscEquipAdjt;
                    dbItem.StageMiscPartsAdjt = ProjectStageToSave.StageMiscPartsAdjt ?? dbItem.StageMiscPartsAdjt;
                    dbItem.StageMiscPartsFactor = ProjectStageToSave.StageMiscPartsFactor ?? dbItem.StageMiscPartsFactor;
                    dbItem.StageMiscProductFactor = ProjectStageToSave.StageMiscProductFactor ?? dbItem.StageMiscProductFactor;
                }
            }
            db.SaveChanges();

            return ProjectStageToSave.ProjectStageID;
        }

        public int SaveEvent(ActivitiesEvents EventToSave)
        {
            if (EventToSave.EventID == 0)
            {
                db.ActivitiesEvents.Add(EventToSave);
            }
            else
            {
                var dbCase = db.ActivitiesEvents.Where(p => p.EventID == EventToSave.EventID).FirstOrDefault();
                if (dbCase != null)
                {
                    //dbCase.SiteCoID = EventToSave.SiteCoID;
                    //dbCase.SiteUserID = EventToSave.SiteUserID;
                    dbCase.EventName = EventToSave.EventName;
                    //dbCase.ProjectID = EventToSave.ProjectID;
                    dbCase.StatusID = EventToSave.StatusID;
                    dbCase.EventDate = EventToSave.EventDate;
                    dbCase.EventTime = EventToSave.EventTime;
                    dbCase.EventHours = EventToSave.EventHours;
                    dbCase.Notes = EventToSave.Notes;
                    dbCase.AllDay = EventToSave.AllDay;
                }
            }
            db.SaveChanges();

            return EventToSave.EventID;
        }

        public int SaveTimeIt(ProjectTimeIts TimeItToSave)
        {
            if (TimeItToSave.TimeItID == 0)
            {
                db.ProjectTimeIts.Add(TimeItToSave);
            }
            else
            {
                var dbTimeIt = db.ProjectTimeIts.Where(p => p.TimeItID == TimeItToSave.TimeItID).FirstOrDefault();
                if (dbTimeIt != null)
                {
                    dbTimeIt.SiteCoID = TimeItToSave.SiteCoID;
                    dbTimeIt.ProjectID = TimeItToSave.ProjectID;
                    dbTimeIt.SiteUserID = TimeItToSave.SiteUserID;
                    dbTimeIt.Subject = TimeItToSave.Subject;
                    dbTimeIt.Hours = TimeItToSave.Hours;
                    dbTimeIt.TimeItDate = TimeItToSave.TimeItDate;
                    dbTimeIt.TimeItTime = TimeItToSave.TimeItTime;
                    dbTimeIt.Notes = TimeItToSave.Notes;
                    dbTimeIt.ProjectStageID = TimeItToSave.ProjectStageID;
                    dbTimeIt.ProjectPayTypeID = TimeItToSave.ProjectPayTypeID;
                    dbTimeIt.ProjectCostCodeID = TimeItToSave.ProjectCostCodeID;
                }
            }
            db.SaveChanges();

            return TimeItToSave.TimeItID;
        }

        public int SaveCase(ProjectCases CaseToSave)
        {
            if (CaseToSave.CaseID == 0)
            {
                db.ProjectCases.Add(CaseToSave);
            }
            else
            {
                var dbCase = db.ProjectCases.Where(p => p.CaseID == CaseToSave.CaseID).FirstOrDefault();
                if (dbCase != null)
                {
                    //dbCase.SiteCoID = CaseToSave.SiteCoID;
                    //dbCase.CreatedByUserID = CaseToSave.CreatedByUserID;
                    //dbCase.ProjectID = CaseToSave.ProjectID;
                    dbCase.SiteUserID = CaseToSave.SiteUserID;
                    dbCase.Subject = CaseToSave.Subject;
                    dbCase.StatusID = CaseToSave.StatusID;
                    dbCase.DueDate = CaseToSave.DueDate;
                    //dbCase.Notes = CaseToSave.Notes;
                    dbCase.Priority = CaseToSave.Priority;
                    //dbCase.DateCreated = CaseToSave.DateCreated;
                    dbCase.CaseTypeID = CaseToSave.CaseTypeID;
                }
            }
            db.SaveChanges();

            return CaseToSave.CaseID;
        }
        public bool UpdateCaseDetails(int CaseID, int ResourceID)
        {
            var caseInfo = db.ProjectCases.Where(p => p.CaseID == CaseID).FirstOrDefault();
            if (caseInfo == null)
            {
                return false;
            }

            caseInfo.SiteUserID = ResourceID;
            db.SaveChanges();
            return true;
        }




        public int SaveCall(ActivitiesCalls CallToSave)
        {
            if (CallToSave.CallID == 0)
            {
                db.ActivitiesCalls.Add(CallToSave);
            }
            else
            {
                var dbCall = db.ActivitiesCalls.Where(p => p.CallID == CallToSave.CallID).FirstOrDefault();
                if (dbCall != null)
                {
                    dbCall.CallDate = CallToSave.CallDate;
                    dbCall.CallTime = CallToSave.CallTime;
                    dbCall.CallDuration = CallToSave.CallDuration;
                    dbCall.CallPurposeID = CallToSave.CallPurposeID;
                    //dbCall.SiteCoID = CallToSave.SiteCoID;
                    //dbCall.SiteUserID = CallToSave.SiteUserID;
                    //dbCall.ProjectID = CallToSave.ProjectID;
                    dbCall.Subject = CallToSave.Subject;
                    //dbCall.ContactID = CallToSave.ContactID;
                    dbCall.Notes = CallToSave.Notes;
                    dbCall.CallID = CallToSave.CallID;
                    dbCall.StatusID = CallToSave.StatusID;
                }
            }
            db.SaveChanges();

            return CallToSave.CallID;
        }

        public int SaveNote(ActivitiesNotes NoteToSave)
        {
            if (NoteToSave.NoteID == 0)
            {
                db.ActivitiesNotes.Add(NoteToSave);
            }
            else
            {
                var dbNote = db.ActivitiesNotes.Where(p => p.NoteID == NoteToSave.NoteID).FirstOrDefault();
                //if (dbNote != null)
                //{
                //    db.ActivitiesNotes.Remove(dbNote);
                //}
                //NoteToSave.NoteID = 0;
                //db.ActivitiesNotes.Add(NoteToSave);
                dbNote.Notes = NoteToSave.Notes;
                //dbNote.ProjectID = NoteToSave.ProjectID ?? 0;
                dbNote.Subject = NoteToSave.Subject;
                //dbNote.ContactID = NoteToSave.ContactID;
                dbNote.DateCreated = NoteToSave.DateCreated ?? DateTime.Now;
            }
            db.SaveChanges();

            return NoteToSave.NoteID;
        }

        public int? GetDefaultActivityStatusId(int SiteCoId)
        {
            var status = db.CoActivitiesStatus.Where(p => p.SiteCoID == SiteCoId).OrderBy(p => p.StatusOrder).FirstOrDefault();

            if (status != null)
            {
                return status.StatusID;
            }
            return null;
        }



        public IEnumerable<CoActivitiesStatus> GetActivityStatus(int SiteCoId)
        {
            return db.CoActivitiesStatus.Where(p => p.SiteCoID == SiteCoId).OrderBy(p => p.StatusOrder);
        }

        public void UpdateAddress(int ContactID, CoContactAddresses Address)
        {
            var address = db.CoContactAddresses.Where(p => p.ContactID == ContactID).FirstOrDefault();
            if (address == null)
            {
                address = new CoContactAddresses()
                {
                    ContactID = ContactID
                };
                db.CoContactAddresses.Add(address);
            }
            address.Address1 = Address.Address1;
            address.Address2 = Address.Address2;
            address.City = Address.City;
            address.State = Address.State;
            address.Zip = Address.Zip;
            address.CountryID = Address.CountryID;
            address.AddressTypeID = Address.AddressTypeID;
            db.SaveChanges();
        }

        public bool UpdateCallNotes(int CallID, int SiteCoID, string Note)
        {
            var call = db.ActivitiesCalls.Where(p => p.CallID == CallID).FirstOrDefault();
            if (call == null ||
                call.SiteCoID != SiteCoID)//Check if some outsider trying to update 
            {
                return false;
            }

            call.Notes = Note;
            db.SaveChanges();

            return true;
        }

        public bool UpdateNoteNotes(int NoteID, int SiteCoID, string Note)
        {
            var note = db.ActivitiesNotes.Where(p => p.NoteID == NoteID).FirstOrDefault();
            if (note == null ||
                note.SiteCoID != SiteCoID)//Check if some outsider trying to update 
            {
                return false;
            }

            note.Notes = Note;
            db.SaveChanges();

            return true;
        }

        public bool AddCaseComment(int CaseID, int ResourceID, string Comment)
        {
            var pcc = new ProjectCasesComments()
            {
                CaseID = CaseID,
                Comment = Comment,
                CommentDate = DateTime.Now,
                CommentTime = DateTime.Now.TimeOfDay,
                SiteUserID = ResourceID
            };

            db.ProjectCasesComments.Add(pcc);
            db.SaveChanges();

            return true;
        }

        public bool AddPunchListComment(int ProjectPunchListID, int UserID, string Comment)
        {
            var pplh = new ProjectPunchListHistory()
            {
                ProjectPunchItemID = ProjectPunchListID,
                ItemHistoryNote = Comment,
                ItemHistoryUserID = UserID,
                ItemHistoryCreateDate = DateTime.Now,
                ItemHistoryCreateTime = DateTime.Now.TimeOfDay,

            };

            db.ProjectPunchListHistory.Add(pplh);
            db.SaveChanges();

            return true;
        }

        public bool UpdateEventNotes(int EventID, int SiteCoID, string Note)
        {
            var _event = db.ActivitiesEvents.Where(p => p.EventID == EventID).FirstOrDefault();
            if (_event == null ||
                _event.SiteCoID != SiteCoID)//Check if some outsider trying to update 
            {
                return false;
            }

            _event.Notes = Note;
            db.SaveChanges();

            return true;
        }


        public bool UpdateTimeItNotes(int TimeItID, int SiteCoID, string Note)
        {
            var timeIt = db.ProjectTimeIts.Where(p => p.TimeItID == TimeItID).FirstOrDefault();
            if (timeIt == null ||
                timeIt.SiteCoID != SiteCoID)//Check if some outsider trying to update 
            {
                return false;
            }

            timeIt.Notes = Note;
            db.SaveChanges();

            return true;
        }

        #region Project Code Code
        public int SaveProjectCostCode(int ProjectCostCodeID, int ProjectID)
        {
            var data = new ProjectCostCodes() { CostCodeID = ProjectCostCodeID, ProjectID = ProjectID };
            db.ProjectCostCodes.Add(data);
            db.SaveChanges();
            return data.ProjectCostCodeID;
        }

        public void DeleteProjectCostCode(int ProjectCostCodeID, int ProjectID)
        {
            var item = db.ProjectCostCodes.Where(p => p.ProjectCostCodeID == ProjectCostCodeID && p.ProjectID == ProjectID).FirstOrDefault();
            if (item != null)
            {
                db.ProjectCostCodes.Remove(item);
            }
            db.SaveChanges();
        }
        #endregion

        #region Project Pay Type
        public int SaveProjectPayType(int PayTypeID, int ProjectID, int SiteCoID, bool IsDefault)
        {
            var data = new ProjectPayTypes() { PayTypeID = PayTypeID, ProjectID = ProjectID, IsDefault = IsDefault };

            var coPayType = db.CoPayTypes.Where(p => p.PayTypeID == PayTypeID && p.SiteCoID == SiteCoID).FirstOrDefault();
            if (coPayType != null)
            {
                data.PayTypeFactor = coPayType.PayTypeFactor;
            }

            db.ProjectPayTypes.Add(data);
            db.SaveChanges();
            int newProjectPayTypeID = data.ProjectPayTypeID;

            //if being set as default then make other items non-default
            if (IsDefault)
            {
                ResetProjectPayTypeDefault(ProjectID, newProjectPayTypeID);
            }

            return newProjectPayTypeID;
        }

        public void ResetProjectPayTypeDefault(int ProjectID, int DefaultProjectPayTypeID)
        {
            var allPays = db.ProjectPayTypes.Where(p => p.ProjectID == ProjectID).ToList();
            foreach (var item in allPays)
            {
                if (item.ProjectPayTypeID == DefaultProjectPayTypeID)
                {
                    item.IsDefault = true;
                }
                else
                {
                    item.IsDefault = false;
                }
            }
            db.SaveChanges();
        }

        public void DeleteProjectPayType(int ProjectPayTypeID, int ProjectID)
        {
            var item = db.ProjectPayTypes.Where(p => p.ProjectPayTypeID == ProjectPayTypeID && p.ProjectID == ProjectID).FirstOrDefault();
            if (item != null)
            {
                db.ProjectPayTypes.Remove(item);
            }
            db.SaveChanges();
        }

        public void UpdateProjectPayType(int ProjectPayTypeID, bool IsDefault, decimal Factor)
        {
            var item = db.ProjectPayTypes.Where(p => p.ProjectPayTypeID == ProjectPayTypeID).FirstOrDefault();
            if (item != null)
            {
                item.IsDefault = IsDefault;
                item.PayTypeFactor = Factor;
                db.SaveChanges();
            }
        }
        #endregion

        #region Project Tax
        public int SaveProjectTax(int TaxID, int ProjectID, int SiteCoID)
        {
            var data = new ProjectTaxes() { ProjectID = ProjectID };

            var coTax = db.CoTaxes.Where(p => p.MasterTaxID == TaxID && p.SiteCoID == SiteCoID).FirstOrDefault();
            if (coTax != null)
            {
                data.CostRate = coTax.CostRate;
                data.LaborRate = coTax.LaborRate;
                data.SalesRate = coTax.SalesRate;
                data.TaxCode = coTax.TaxCode;
                data.TaxDescription = coTax.TaxDescription;
            }

            db.ProjectTaxes.Add(data);
            db.SaveChanges();
            return data.ProjectTaxID;
        }

        public void DeleteProjectTax(int ProjectTaxID, int ProjectID)
        {
            var item = db.ProjectTaxes.Where(p => p.ProjectTaxID == ProjectTaxID && p.ProjectID == ProjectID).FirstOrDefault();
            if (item != null)
            {
                db.ProjectTaxes.Remove(item);
            }
            db.SaveChanges();
        }

        public void UpdateProjectTax(int ProjectTaxID, decimal Sales, decimal Labor)
        {
            var item = db.ProjectTaxes.Where(p => p.ProjectTaxID == ProjectTaxID).FirstOrDefault();
            if (item != null)
            {
                item.SalesRate = Sales;
                item.LaborRate = Labor;
            }
            db.SaveChanges();
        }

        #endregion

        #region Project Area
        public int SaveProjectArea(int? AreaID, int ProjectID, int SiteCoID, string CustomAreaName, int Order)
        {
            var data = new ProjectAreas() { ProjectID = ProjectID, AreaOrder = Order.ToString() };

            if (!string.IsNullOrWhiteSpace(CustomAreaName))
            {
                data.AreaName = CustomAreaName;
            }
            else
            {
                var coArea = db.CoAreas.Where(p => p.AreaID == AreaID && p.SiteCoID == SiteCoID).FirstOrDefault();
                if (coArea != null)
                {
                    data.AreaName = coArea.AreaName;
                }
            }

            db.ProjectAreas.Add(data);
            db.SaveChanges();
            return data.ProjectAreaID;
        }

        public void DeleteProjectArea(int ProjectAreaID, int ProjectID)
        {
            var item = db.ProjectAreas.Where(p => p.ProjectAreaID == ProjectAreaID && p.ProjectID == ProjectID).FirstOrDefault();
            if (item != null)
            {
                db.ProjectAreas.Remove(item);
            }
            db.SaveChanges();
        }

        public void UpdateProjectArea(int ProjectAreaID, int Order)
        {
            var item = db.ProjectAreas.Where(p => p.ProjectAreaID == ProjectAreaID).FirstOrDefault();
            if (item != null)
            {
                item.AreaOrder = Order.ToString();
            }
            db.SaveChanges();
        }
        #endregion

        #region Project Division
        public int SaveProjectDivision(int DivisionID, int ProjectID, int Order, int SiteCoID)
        {
            var data = new ProjectDivisions() { ProjectID = ProjectID, DivisionOrder = Order };

            var coDivision = db.CoDivisions.Where(p => p.DivisionID == DivisionID && p.SiteCoID == SiteCoID).FirstOrDefault();
            if (coDivision != null)
            {
                data.DivisionName = coDivision.DivisionName;
                //data.DivisionOrder = coDivision.DivisionOrder ?? 1;
            }

            db.ProjectDivisions.Add(data);
            db.SaveChanges();
            return data.ProjectDivisionID;
        }

        public void DeleteProjectDivision(int ProjectDivisionID, int ProjectID)
        {
            var item = db.ProjectDivisions.Where(p => p.ProjectDivisionID == ProjectDivisionID && p.ProjectID == ProjectID).FirstOrDefault();
            if (item != null)
            {
                db.ProjectDivisions.Remove(item);
            }
            db.SaveChanges();
        }

        public void UpdateProjectDivision(int ProjectDivisionID, int Order)
        {
            var item = db.ProjectDivisions.Where(p => p.ProjectDivisionID == ProjectDivisionID).FirstOrDefault();
            if (item != null)
            {
                item.DivisionOrder = Order;
            }
            db.SaveChanges();
        }
        #endregion

        #region Project Communication
        public int SaveProjectCommunication(int SiteUserID, int ProjectID, int SiteCoID)
        {
            var data = new ProjectCommunications() { ProjectID = ProjectID, SiteUserID = SiteUserID };

            db.ProjectCommunications.Add(data);
            db.SaveChanges();
            return data.ProjectCommID;
        }

        public void DeleteProjectCommunication(int ProjectCommunicationID, int ProjectID)
        {
            var item = db.ProjectCommunications.Where(p => p.ProjectCommID == ProjectCommunicationID && p.ProjectID == ProjectID).FirstOrDefault();
            if (item != null)
            {
                db.ProjectCommunications.Remove(item);
            }
            db.SaveChanges();
        }
        #endregion


        #region Country

        public static List<SiteCountries> CountriesCache = new List<SiteCountries>();
        public List<SiteCountries> GetCountries()
        {
            if (CountriesCache != null && CountriesCache.Any())
            {
                return CountriesCache;
            }

            CountriesCache = db.SiteCountries.OrderBy(p => p.Country).ToList();
            return CountriesCache;
        }

        public string GetCountryNameByID(int CountryID)
        {
            var countries = GetCountries();
            var country = countries.Where(p => p.CountryID == CountryID).FirstOrDefault();

            if (country != null)
            {
                return country.Country;
            }
            else
            {
                return string.Empty;
            }
        }
        public string GetCountryNameByID(string CountryID)
        {
            int temp = 0;
            if (int.TryParse(CountryID, out temp))
            {
                return GetCountryNameByID(temp);
            }
            else
            {
                return string.Empty;
            }
        }

        public int GetCountryIDByName(string CountryName)
        {
            try
            {
                return GetCountries().Where(p => p.Country.Equals(CountryName, StringComparison.OrdinalIgnoreCase)).Select(p => p.CountryID).FirstOrDefault();
            }
            catch (Exception ex)
            {
                ex.Log();
                return 0;
            }
        }

        #endregion


        #region Options

        public string DeleteOption(string Type, int ItemID, int SiteCoID)
        {
            string status = "";
            try
            {
                var res = db.DeleteOption(Type, ItemID, SiteCoID);
                db.SaveChanges();
                //Not sure why this is in error?
                status = res.ToString();
            }
            catch (Exception ex)
            {
                LogRepository.LogException(ex);
                return "ERROR";
            }

            return status;
        }

        public int UpdateStageMaster(CoStagesMaster StageMasterToSave)
        {
            var dbData = db.CoStagesMaster.Where(p => p.StageMasterID == StageMasterToSave.StageMasterID).FirstOrDefault();

            if (dbData == null)
            {
                LogRepository.LogException(new Exception("StageMaster data not found. StageMasterID:" + StageMasterToSave.StageMasterID));
            }

            dbData.SiteCoID = StageMasterToSave.SiteCoID;
            dbData.StageOrder = StageMasterToSave.StageOrder;
            dbData.CostCodeID = StageMasterToSave.CostCodeID;
            dbData.StageMiscEquipAdjt = StageMasterToSave.StageMiscEquipAdjt;
            dbData.StageMiscPartsAdjt = StageMasterToSave.StageMiscPartsAdjt;
            db.SaveChanges();

            return StageMasterToSave.StageMasterID;
        }

        public int UpdateStagePricing(CoStagesPricing CoStagesPricingToSave)
        {
            var dbItem = db.CoStagesPricing.Where(p => p.StageMasterID == CoStagesPricingToSave.StageMasterID && p.StageSubID == CoStagesPricingToSave.StageSubID).FirstOrDefault();

            if (dbItem == null)
            {
                db.CoStagesPricing.Add(CoStagesPricingToSave);
            }
            else
            {
                //dbItem.StageMasterID = CoStagesPricingToSave.StageMasterID;
                //dbItem.StageSubID = CoStagesPricingToSave.StageSubID;
                dbItem.StageLaborCost = CoStagesPricingToSave.StageLaborCost;
                dbItem.StageLaborPrice = CoStagesPricingToSave.StageLaborPrice;
                dbItem.StageLaborFactor = CoStagesPricingToSave.StageLaborFactor;
            }

            db.SaveChanges();

            return CoStagesPricingToSave.StagePriceID;
        }


        #endregion

        #region POR

        public string GetNextPurchaseOrderNumber(int ProjectID, string ProjectNumber)
        {
            try
            {
                var oldPON = db.ProjectPurchaseOrders.Where(p => p.ProjectID == ProjectID && p.PurchaseOrderNumber.Length > 0).OrderByDescending(p => p.PurchaseOrderID).Select(p => p.PurchaseOrderNumber).FirstOrDefault()
                    ?? "";

                if (string.IsNullOrWhiteSpace(oldPON))
                {
                    return String.Format("{0}-{1}", ProjectNumber, 0);
                }

                string prefix = oldPON.Substring(0, Math.Max(oldPON.LastIndexOf("-"), 0));
                if (string.IsNullOrEmpty(prefix))
                {
                    prefix = ProjectNumber;
                }

                int oldNum = 0;
                int newNum = 0;

                string oldNumStr = oldPON.Substring(oldPON.LastIndexOf("-") + 1);
                int.TryParse(oldNumStr, out oldNum);

                newNum = oldNum + 1;

                return String.Format("{0}-{1}", prefix, newNum);
            }
            catch (Exception ex)
            {
                LogRepository.LogException(ex);
                return string.Empty;
            }
        }

        public bool CreatePOR(int CreatorID, int ProjectID, DateTime RequestedDate, List<int> ItemIDs, int? ShipToID)
        {
            if (ItemIDs == null || ProjectID <= 0)
            {
                return false;
            }

            #region check if items already added to POR

            var alreadyAddedItemIDs = new List<int>();
            foreach (int itemID in ItemIDs)
            {
                var alreadyAdded = db.Procurement.Any(p => p.ItemID == itemID);
                if (alreadyAdded)
                {
                    alreadyAddedItemIDs.Add(itemID);
                }
            }

            ItemIDs = ItemIDs.Except(alreadyAddedItemIDs).ToList();

            if (!ItemIDs.Any())
            {
                return true;
            }

            #endregion

            var projectInfo = db.ProjectInfo.Where(p => p.ProjectID == ProjectID).FirstOrDefault();
            if (projectInfo == null)
            {
                return false;
            }

            var siteCoID = projectInfo.SiteCoID;
            var procurementStatusID = db.CoProcurementStatus.Where(p => p.SiteCoID == siteCoID && p.ProcurementStatusOrder == 1).Select(p => p.ProcurementStatusID).FirstOrDefault();


            //vendorID | porID
            List<Tuple<int?, int>> porIDMapping = new List<Tuple<int?, int>>();

            var vendorIDs = db.GetVendorIDsByItemIDs(string.Join(",", ItemIDs)).ToList();
            var vendorsGrouped = vendorIDs.GroupBy(p => p.VendorID).ToList();
            vendorsGrouped.FirstOrDefault().FirstOrDefault();

            foreach (var vendor in vendorsGrouped)
            {
                var vendorID = vendor.FirstOrDefault().VendorID;
                var por = new ProjectPurchaseOrders()
                {
                    SiteCoID = siteCoID,
                    CreatorID = CreatorID,
                    ProjectID = ProjectID,
                    PurchaseOrderNumber = GetNextPurchaseOrderNumber(ProjectID, projectInfo.ProjectNumber),
                    VendorID = vendorID,
                    ProcurementStatusID = procurementStatusID,
                    DateCreated = DateTime.Now,
                    Priority = EnumWrapper.Priority.Medium.ToString(),
                    RequestedDate = RequestedDate,
                    ShipToID = ShipToID
                };
                db.ProjectPurchaseOrders.Add(por);
                db.SaveChanges();
                porIDMapping.Add(Tuple.Create(vendorID, por.PurchaseOrderID));
            }

            //MasterItemID | poqID
            List<Tuple<int?, int>> poqIDMapping = new List<Tuple<int?, int>>();
            var masterItemIDs = db.GetItemMasterIDs(string.Join(",", ItemIDs)).ToList();
            var masterItemsGrouped = masterItemIDs.GroupBy(p => p.MasterItemID).ToList();
            foreach (var masterItem in masterItemsGrouped)
            {
                var masterItemID = masterItem.FirstOrDefault().MasterItemID;
                var itemID = masterItem.FirstOrDefault().ItemID;

                var vendorID = vendorIDs.Where(p => p.ItemID == itemID).Select(p => p.VendorID).FirstOrDefault();
                var porID = porIDMapping.Where(p => p.Item1 == vendorID).Select(p => p.Item2).FirstOrDefault();

                var projectItemQuantity = db.ProjectItems.Where(p => p.ItemID == itemID).Select(p => p.Qty).FirstOrDefault() ?? 1;
                var masterOrderQuantity = db.CoData.Where(p => p.MasterItemID == masterItemID).Select(p => p.OrderQuantity).FirstOrDefault() ?? 1;
                var quantityOrdered = projectItemQuantity / masterOrderQuantity;

                var costOrderted = db.GetCostByMasterItemID(masterItemID).FirstOrDefault();

                var poq = new ProjectOrderQuantity()
                {
                    PurchaseOrderID = porID,
                    MasterItemID = masterItemID,
                    QuantityOrdered = (int)Math.Ceiling(quantityOrdered),
                    CostOrdered = costOrderted,
                    ActualCost = costOrderted
                };
                db.ProjectOrderQuantity.Add(poq);
                db.SaveChanges();
                poqIDMapping.Add(Tuple.Create(masterItemID, poq.OrderQuantityID));
            }

            foreach (int itemID in ItemIDs)
            {
                var masterItemID = masterItemIDs.Where(p => p.ItemID == itemID).Select(p => p.MasterItemID).FirstOrDefault();
                var vendorID = vendorIDs.Where(p => p.ItemID == itemID).Select(p => p.VendorID).FirstOrDefault();
                var porID = porIDMapping.Where(p => p.Item1 == vendorID).Select(p => p.Item2).FirstOrDefault();
                var poqID = poqIDMapping.Where(p => p.Item1 == masterItemID).Select(p => p.Item2).FirstOrDefault();

                var procurement = new Procurement()
                {
                    PurchaseOrderID = porID,
                    ItemID = itemID,
                    RequestDate = DateTime.Now,
                    OrderQuantityID = poqID
                };
                db.Procurement.Add(procurement);
            }

            db.SaveChanges();

            return true;
        }

        public int SaveProjectPurchaseOrder(ProjectPurchaseOrders ProjectPurchaseOrdersToSave)
        {
            bool isNew = ProjectPurchaseOrdersToSave.PurchaseOrderID == 0;
            if (ProjectPurchaseOrdersToSave.PurchaseOrderID == 0)
            {
                db.ProjectPurchaseOrders.Add(ProjectPurchaseOrdersToSave);
            }
            else
            {
                var dbData = db.ProjectPurchaseOrders.Where(p => p.PurchaseOrderID == ProjectPurchaseOrdersToSave.PurchaseOrderID).FirstOrDefault();
                if (dbData != null)
                {
                    dbData.PurchaseOrderNumber = ProjectPurchaseOrdersToSave.PurchaseOrderNumber;
                    dbData.VendorID = ProjectPurchaseOrdersToSave.VendorID;
                    dbData.Priority = ProjectPurchaseOrdersToSave.Priority;
                    dbData.RequestedDate = ProjectPurchaseOrdersToSave.RequestedDate;
                    dbData.ProcurementStatusID = ProjectPurchaseOrdersToSave.ProcurementStatusID;
                    dbData.DateOrdered = ProjectPurchaseOrdersToSave.DateOrdered;
                    dbData.ArrivalDate = ProjectPurchaseOrdersToSave.ArrivalDate;
                    dbData.ShippingMethodID = ProjectPurchaseOrdersToSave.ShippingMethodID;
                    dbData.ShipToID = ProjectPurchaseOrdersToSave.ShipToID;
                    dbData.TrackingNumber = ProjectPurchaseOrdersToSave.TrackingNumber;
                    dbData.Notes = ProjectPurchaseOrdersToSave.Notes;
                }
            }
            db.SaveChanges();

            return ProjectPurchaseOrdersToSave.PurchaseOrderID;
        }

        public bool UpdateItemCustody(Procurement ProcurementToSave, decimal? ActualCost)
        {
            var dbProcurement = db.Procurement.Where(p => p.ItemID == ProcurementToSave.ItemID).FirstOrDefault();
            if (dbProcurement != null)
            {
                //procurement
                dbProcurement.ItemID = ProcurementToSave.ItemID;
                dbProcurement.SerialNumber = ProcurementToSave.SerialNumber;
                dbProcurement.ArrivalDate = ProcurementToSave.ArrivalDate ?? dbProcurement.ArrivalDate;
                dbProcurement.RequestDate = ProcurementToSave.RequestDate ?? dbProcurement.RequestDate;
                dbProcurement.DeliveredDate = ProcurementToSave.DeliveredDate ?? dbProcurement.DeliveredDate;
                dbProcurement.CustodyLocationID = ProcurementToSave.CustodyLocationID;
                dbProcurement.PackingSlipNumber = ProcurementToSave.PackingSlipNumber;

                //Product order quantity
                var dbPOQ = db.ProjectOrderQuantity.Where(p => p.OrderQuantityID == dbProcurement.OrderQuantityID).FirstOrDefault();
                if (dbPOQ != null)
                {
                    dbPOQ.ActualCost = ActualCost;
                }
            }

            db.SaveChanges();

            return true;
        }


        #endregion

        #region Work Order

        public bool SaveWorkOrderAssign(int SiteUserID, int? ProjectID, List<int> ItemIDs, int? StatusID, int? WorkOrderTypeID, List<Tuple<int, List<DateTime>>> Resources)
        {
            var projectInfo = db.ProjectInfo.Where(p => p.ProjectID == ProjectID).FirstOrDefault();
            if (projectInfo == null)
            {
                return false;
            }

            //schedule items
            int projectScheduleID = 0;
            if (WorkOrderTypeID == (int)EnumWrapper.WorkOrderTypes.Scheduled)
            {

                #region Project Schedule

                var ps = new ProjectSchedule()
                {
                    SiteCoID = projectInfo.SiteCoID,
                    ProjectID = ProjectID
                };
                db.ProjectSchedule.Add(ps);
                db.SaveChanges();

                projectScheduleID = ps.ScheduleID;

                #endregion

                #region Project Schedule Items

                //ItemID | ScheduleItemID
                List<Tuple<int, int>> schItemIDMapping = new List<Tuple<int, int>>();
                foreach (var itemID in ItemIDs)
                {
                    var psi = new ProjectScheduleItems()
                    {
                        ScheduleID = ps.ScheduleID,
                        ItemID = itemID
                    };

                    db.ProjectScheduleItems.Add(psi);
                    db.SaveChanges();
                    schItemIDMapping.Add(Tuple.Create(itemID, psi.ScheduleItemID));
                }
                #endregion

            }

            #region ProjectWorkOrders

            var lastWoNumber = "";
            foreach (var resource in Resources)
            {
                var newWO = new List<ProjectWorkOrders>();
                foreach (var date in resource.Item2)
                {
                    //foreach (var itemID in ItemIDs)
                    //{

                    var woNumber = GetNextWorkOrderNumber(projectInfo.ProjectID, projectInfo.ProjectNumber, lastWoNumber);

                    var wo = new ProjectWorkOrders()
                    {
                        SiteCoID = projectInfo.SiteCoID,
                        ProjectID = projectInfo.ProjectID,
                        WoNumber = woNumber,
                        WoDate = date,
                        WoTime = new TimeSpan(8, 0, 0),
                        WoStatusID = StatusID,
                        WoTypeID = WorkOrderTypeID,
                        WoNote = string.Empty,
                        DateCreated = DateTime.Now,
                        ScheduleID = projectScheduleID,//for punchlist it will be 0
                        SiteUserID = resource.Item1,
                    };

                    if (WorkOrderTypeID == (int)EnumWrapper.WorkOrderTypes.Scheduled)
                    {
                        wo.AllottedTime = db.GetAllottedTimeByItemIDs(string.Join(",", ItemIDs)).FirstOrDefault();
                    }
                    else if (WorkOrderTypeID == (int)EnumWrapper.WorkOrderTypes.Punchlist)
                    {
                        wo.AllottedTime = db.ProjectPunchLists.Sum(p => p.EstHrs);
                    }

                    newWO.Add(wo);
                    lastWoNumber = wo.WoNumber;
                    //}
                }

                foreach (var wo in newWO)
                {
                    db.ProjectWorkOrders.Add(wo);
                }
                db.SaveChanges();

                //if punch list, then also add items. In pounchlist, items cant be added later
                if (WorkOrderTypeID == (int)EnumWrapper.WorkOrderTypes.Punchlist)
                {
                    foreach (var wo in newWO)
                    {
                        AddWorkOrderItems(wo.WoID, ItemIDs, EnumWrapper.WorkOrderTypes.Punchlist);
                    }
                }

            }
            #endregion

            #region Validate POR

            List<int> porMissingItems = new List<int>();
            foreach (var item in ItemIDs)
            {
                var validationStatus = db.ValidatePOR(item).FirstOrDefault();
                if (validationStatus != null && validationStatus.ToLower() == "false")
                {
                    porMissingItems.Add(item);
                }
            }

            if (porMissingItems.Any())
            {
                // request por one day before work starts
                var requestedDate = Resources.SelectMany(p => p.Item2).Min().AddDays(-1);

                CreatePOR(SiteUserID, projectInfo.ProjectID, requestedDate, porMissingItems, null);
            }

            #endregion




            return true;
        }

        public bool AddWorkOrderItems(int WoID, List<int> ItemIDs, EnumWrapper.WorkOrderTypes WOType = EnumWrapper.WorkOrderTypes.Scheduled)
        {
            if (!ItemIDs.Any())
            {
                return false;
            }

            var woData = db.ProjectWorkOrders.Where(p => p.WoID == WoID).Select(p => new { p.ProjectID, p.SiteUserID }).FirstOrDefault();
            if (woData == null)
            {
                return false;
            }

            var projectInfo = db.ProjectInfo.Where(p => p.ProjectID == woData.ProjectID).FirstOrDefault();
            if (projectInfo == null)
            {
                return false;
            }

            foreach (var itemID in ItemIDs)
            {
                var itemsAdded = 0;
                if (WOType == EnumWrapper.WorkOrderTypes.Scheduled)
                {
                    itemsAdded = db.ProjectAssignments.Where(p => p.ItemID == itemID).Count();
                }
                itemsAdded++;//add current item

                var pa = new ProjectAssignments()
                {
                    ItemID = itemID,
                    SiteUserID = woData.SiteUserID,
                    WoID = WoID,
                    Units = (decimal)1 / itemsAdded,//for punchlist it will be always 1
                    ActHrs = 0,
                    PercentComplete = 0
                };

                if (WOType == EnumWrapper.WorkOrderTypes.Scheduled)
                {
                    pa.EstHrs = db.GetProjectAssignmentEstHours(itemID).FirstOrDefault();
                }
                else if (WOType == EnumWrapper.WorkOrderTypes.Punchlist)
                {
                    pa.EstHrs = db.ProjectPunchLists.Where(p => p.ProjectPunchListID == itemID).Select(p => p.EstHrs).FirstOrDefault();
                }

                db.ProjectAssignments.Add(pa);
            }

            db.SaveChanges();

            //update units and estHrs of all old items 
            db.UpdateAssignmentItemData(WoID, string.Join(",", ItemIDs));

            return true;
        }

        public bool UpdateWorkOrderActuals(int SiteCoID, List<int> ProjectAssignmentIDs, DateTime? ActualDate, decimal? ActualHours, double? PercentCompleted, EnumWrapper.WorkOrderTypes WOType = EnumWrapper.WorkOrderTypes.Scheduled, string constring="")
        {
            var assignments = db.ProjectAssignments.Where(p => ProjectAssignmentIDs.Contains(p.AssignmentID)).ToList();
            var woID = assignments.Select(p => p.WoID).FirstOrDefault();

            var wo = db.ProjectWorkOrders.Where(p => p.WoID == woID).FirstOrDefault();
            if (wo == null)
            {
                wo = new ProjectWorkOrders();
                db.ProjectWorkOrders.Add(wo);
            }

            #region WO Update
            wo.ActualHours = ActualHours;
            if (ActualDate.HasValue)
            {
                wo.ActualDate = ActualDate.Value.Date;
                wo.ActualTime = ActualDate.Value.TimeOfDay;
            }
            #endregion

            #region Assignment update
            var actualHourMultiplier = Math.Max(wo.ActualHours ?? 1, 1) / Math.Max(wo.AllottedTime ?? 1, 1);
            foreach (var assignment in assignments)
            {
                assignment.ActDate = ActualDate;
                assignment.PercentComplete = PercentCompleted;

                if (WOType == EnumWrapper.WorkOrderTypes.Punchlist)
                {
                    assignment.ActHrs = ActualHours;
                }
                else
                {
                    var calculatedActualHours = actualHourMultiplier * ActualHours;
                    assignment.ActHrs = calculatedActualHours;
                }
            }

            var itemIDs = assignments.Select(p => p.ItemID).Distinct().ToList();
            //foreach (var itemID in itemIDs)
            //{
            //    db.UpdateProjectAssignmentsOnWOUpdate(woID, itemID);
            //}

            db.SaveChanges();
            if (ActualDate.HasValue && ActualHours.HasValue)
            {
                db.UpdateWorkOrderToCompleted(woID, string.Join(",", itemIDs), SiteCoID, ActualDate.Value.Date, ActualDate.Value.TimeOfDay, ActualHours.Value);
            }
            #endregion

            return true;
        }

        public bool RemoveWorkOrderTasks(int WoID, List<int> ProjectAssignmentIDs)
        {
            var assignments = db.ProjectAssignments.Where(p => p.WoID == WoID && ProjectAssignmentIDs.Contains(p.AssignmentID)).ToList();

            db.ProjectAssignments.RemoveRange(assignments);

            db.SaveChanges();

            return true;
        }

        public List<GetCoWoStatusBySiteCoID_Result> GetWorkOrderStatus(int SiteCoID)
        {
            //return db.CoWoStatus.Where(p => p.SiteCoID == SiteCoID).OrderBy(p => p.WoStatusOrder).ToList();
            return db.GetCoWoStatusBySiteCoID(SiteCoID).ToList();
        }

        public bool UpdateWorkOrder(int? WoID, int? ResourceID, int? WorkOrderStatusID, DateTime? AssignedDate, decimal? EstimatedHours)
        {
            var wo = db.ProjectWorkOrders.Where(p => p.WoID == WoID).FirstOrDefault();
            if (wo == null)
            {
                return false;
            }

            wo.AllottedTime = EstimatedHours;
            wo.SiteUserID = ResourceID;
            wo.WoStatusID = WorkOrderStatusID;
            wo.WoDate = AssignedDate!=null ? (DateTime?)AssignedDate.Value.Date : null;
            wo.WoTime = AssignedDate != null ? (TimeSpan?)AssignedDate.Value.TimeOfDay : null;

            db.SaveChanges();

            return true;
        }

        public string GetNextWorkOrderNumber(int ProjectID, string ProjectNumber, string LastWoNumber)
        {
            try
            {
                var oldWON = LastWoNumber;

                if (string.IsNullOrWhiteSpace(oldWON))
                {
                    oldWON = db.ProjectWorkOrders.Where(p => p.ProjectID == ProjectID && p.WoNumber.Length > 0).OrderByDescending(p => p.WoID).Select(p => p.WoNumber).AsNoTracking().FirstOrDefault()
                        ?? "";
                }

                if (string.IsNullOrWhiteSpace(oldWON))
                {
                    return String.Format("WO - {0}-{1}", ProjectNumber, 0);
                }

                string prefix = oldWON.Substring(0, Math.Max(oldWON.LastIndexOf("-"), 0));
                if (string.IsNullOrEmpty(prefix))
                {
                    prefix = ProjectNumber;
                }

                int oldNum = 0;
                int newNum = 0;

                string oldNumStr = oldWON.Substring(oldWON.LastIndexOf("-") + 1);
                int.TryParse(oldNumStr, out oldNum);

                newNum = oldNum + 1;

                return String.Format("{0}-{1}", prefix, newNum);
            }
            catch (Exception ex)
            {
                LogRepository.LogException(ex);
                return string.Empty;
            }
        }

        public decimal? GetWorkOrderEstHours(IEnumerable<int> ItemIDs, EnumWrapper.WorkOrderTypes WOType)
        {
            if (WOType == EnumWrapper.WorkOrderTypes.Scheduled)
            {
                return db.GetWorkOrderEstHours(string.Join(",", ItemIDs)).FirstOrDefault();
            }
            else if (WOType == EnumWrapper.WorkOrderTypes.Punchlist)
            {
                return db.ProjectPunchLists.Sum(p => p.EstHrs);
            }
            else
            {
                return null;
            }
        }

        #endregion

        #region Project Delivery Request

        public string GetNextDeliveryRequestNumber(int ProjectID)
        {
            try
            {
                var oldDRN = db.ProjectDeliveryRequests.Where(p => p.ProjectID == ProjectID && p.RequestNumber.Length > 0).OrderByDescending(p => p.RequestID).Select(p => p.RequestNumber).AsNoTracking().FirstOrDefault()
                        ?? "";

                if (string.IsNullOrWhiteSpace(oldDRN))
                {
                    var projectNumber = db.ProjectInfo.Where(p => p.ProjectID == ProjectID).Select(p => p.ProjectNumber).FirstOrDefault();
                    return String.Format("DL - {0}-{1}", projectNumber, 0);
                }

                string prefix = oldDRN.Substring(0, Math.Max(oldDRN.LastIndexOf("-"), 0));
                if (string.IsNullOrEmpty(prefix))
                {
                    prefix = db.ProjectInfo.Where(p => p.ProjectID == ProjectID).Select(p => p.ProjectNumber).FirstOrDefault();
                }

                int oldNum = 0;
                int newNum = 0;

                string oldNumStr = oldDRN.Substring(oldDRN.LastIndexOf("-") + 1);
                int.TryParse(oldNumStr, out oldNum);

                newNum = oldNum + 1;

                return String.Format("{0}-{1}", prefix, newNum);
            }
            catch (Exception ex)
            {
                LogRepository.LogException(ex);
                return string.Empty;
            }
        }

        public bool CreateDeliveryInfo(int SiteCoID, int ProjectID, List<int> ItemIDs, DateTime DeliveryDate, int DeliveryTypeID)
        {
            if (!ItemIDs.Any())
            {
                return false;
            }

            #region check if items already added to delivery request

            var alreadyAddedItemIDs = new List<int>();
            foreach (int itemID in ItemIDs)
            {
                var alreadyAdded = db.ProjectDeliveryRequestItems.Any(p => p.ItemID == itemID);
                if (alreadyAdded)
                {
                    alreadyAddedItemIDs.Add(itemID);
                }
            }

            ItemIDs = ItemIDs.Except(alreadyAddedItemIDs).ToList();

            if (!ItemIDs.Any())
            {
                return true;
            }

            #endregion

            int deliveryStatusID = db.CoDeliveryStatus.Where(p => p.SiteCoID == SiteCoID && p.DeliveryStatusOrder == 1).Select(p => p.DeliveryStatusID).FirstOrDefault();

            var pdr = new ProjectDeliveryRequests()
            {
                SiteCoID = SiteCoID,
                ProjectID = ProjectID,
                RequestNumber = GetNextDeliveryRequestNumber(ProjectID),
                DateCreated = DateTime.Now,
                DeliveryDate = DeliveryDate,
                DeliveryTime = DeliveryDate.TimeOfDay,
                DeliveryStatusID = deliveryStatusID,
                DeliveryTypeID = DeliveryTypeID
            };
            db.ProjectDeliveryRequests.Add(pdr);
            db.SaveChanges();

            var procurements = db.Procurement.Where(p => ItemIDs.Contains(p.ItemID ?? 0))
                .Select(p => new { p.ItemID, p.ArrivalDate, p.DeliveredDate })
                .ToList();

            foreach (var itemID in ItemIDs)
            {
                //add only received items

                bool isItemNotReceived = procurements.Any(p => p.ItemID == itemID && (p.ArrivalDate != null || p.DeliveredDate != null));
                if (isItemNotReceived)
                {
                    continue;
                }

                var pdri = new ProjectDeliveryRequestItems()
                {
                    RequestID = pdr.RequestID,
                    ItemID = itemID,
                    DeliveryStatusID = deliveryStatusID
                };
                db.ProjectDeliveryRequestItems.Add(pdri);
            }

            db.SaveChanges();

            return true;
        }

        public bool UpdateDeliveryRequestItems(List<int> RequestItemIDs, int? CustodyID, DateTime? Delivered, int? StatusID)
        {
            var requestItems = db.ProjectDeliveryRequestItems.Where(p => RequestItemIDs.Contains(p.RequestItemID)).ToList();

            //StatusID
            if (StatusID.HasValue)
            {
                if (requestItems.Any())
                {
                    requestItems.ForEach(p =>
                    {
                        p.DeliveryStatusID = StatusID;
                    });
                }
            }

            var itemIDs = requestItems.Select(p => p.ItemID).ToList();
            var procurement = db.Procurement.Where(p => itemIDs.Contains(p.ItemID)).ToList();
            if (procurement.Any())
            {
                procurement.ForEach(p =>
                {
                    if (Delivered.HasValue)
                    {
                        p.DeliveredDate = Delivered;
                    }

                    if (CustodyID.HasValue)
                    {
                        p.CustodyLocationID = CustodyID;
                    }
                });
            }

            db.SaveChanges();

            return true;
        }

        public bool UpdateDeliveryRequest(int? RequestID, DateTime? Delivery, int? TypeID, int? StatusID, string Note)
        {
            var request = db.ProjectDeliveryRequests.Where(p => p.RequestID == RequestID).FirstOrDefault();
            if (request == null)
            {
                return false;
            }

            request.DeliveryDate = Delivery;
            request.DeliveryTypeID = TypeID;
            request.DeliveryStatusID = StatusID;
            request.RequestNotes = Note;

            db.SaveChanges();

            return true;
        }

        #endregion

        public string GetNextNumberInSeries(string CurrentNumber, string Prefix)
        {
            try
            {
                var oldNumber = CurrentNumber ?? "";

                if (string.IsNullOrWhiteSpace(oldNumber))
                {
                    return String.Format("{0}-{1}", Prefix, 1);
                }

                string prefix = oldNumber.Substring(0, Math.Max(oldNumber.LastIndexOf("-"), 0));
                if (string.IsNullOrEmpty(prefix))
                {
                    prefix = prefix + "-";
                }

                int oldNum = 0;
                int newNum = 0;

                string oldNumStr = oldNumber.Substring(oldNumber.LastIndexOf("-") + 1);
                int.TryParse(oldNumStr, out oldNum);

                newNum = oldNum + 1;

                return String.Format("{0}-{1}", prefix, newNum);
            }
            catch (Exception ex)
            {
                LogRepository.LogException(ex);
                return string.Empty;
            }
        }

        #region COR

        public int SaveCor(ProjectCor CORToSave)
        {
            if (CORToSave.CorID == 0)
            {
                db.ProjectCor.Add(CORToSave);
            }
            else
            {
                var dbCor = db.ProjectCor.Where(p => p.CorID == CORToSave.CorID).FirstOrDefault();
                if (dbCor != null)
                {
                    dbCor.CorTypeID = CORToSave.CorTypeID;
                    dbCor.CreatedByUserID = CORToSave.CreatedByUserID;
                    dbCor.ProjectID = CORToSave.ProjectID;
                    dbCor.CorNumber = CORToSave.CorNumber;
                    dbCor.CorStatusID = CORToSave.CorStatusID;
                    dbCor.Date = CORToSave.Date;
                    dbCor.Subject = CORToSave.Subject;
                }
            }

            db.SaveChanges();

            return CORToSave.CorID;
        }

        public string GetNextCorNumber()
        {
            var oldCorNum = db.ProjectCor.OrderByDescending(p => p.CorID).Select(p => p.CorNumber).FirstOrDefault();
            return GetNextNumberInSeries(oldCorNum, "COR");
        }

        public bool UpdateCorReason(int CorID, string Reason)
        {
            var cor = db.ProjectCor.Where(p => p.CorID == CorID).FirstOrDefault();
            if (cor == null)
            {
                return false;
            }

            cor.Reason = Reason;
            db.SaveChanges();
            return true;
        }

        public bool AddCorItems(int SiteCoID, int CorID, int ProjectID, List<int> AreaIDs, int DivisionID, int Quantity, List<int> MasterItemIDs)
        {
            List<ProjectItemPricing> itemPricings = new List<ProjectItemPricing>();

            try
            {
                foreach (var masterItemID in MasterItemIDs)
                {
                    var coData = db.CoData.Where(p => p.MasterItemID == masterItemID).FirstOrDefault();
                    if (coData == null)
                    {
                        coData = new CoData();
                    }

                    int? stageMasterID = coData.StageID;

                    //if this stage doesn't exist in project then add it
                    //if (stageMasterID.HasValue && !db.ProjectStages.Any(p => p.ProjectID == ProjectID && p.StageMasterID == stageMasterID))
                    //{
                    //    AddProjectStages(ProjectID, new List<int>() { stageMasterID.Value });
                    //}

                    //if item's CostCode doesn't exist in project then add it
                    //if (coData.CostCodeID.HasValue && !db.ProjectCostCodes.Any(p => p.ProjectID == ProjectID && p.CostCodeID == coData.CostCodeID))
                    //{
                    //    SaveProjectCostCode(coData.CostCodeID.Value, ProjectID);
                    //}

                    var itemData = db.GetDataForNewProjectItem(ProjectID, masterItemID).ToList();

                    int? projectStageID = (int?)itemData.Where(p => p.DataName == "StageID").Select(p => p.Value).FirstOrDefault();
                    int? costCodeID = (int?)itemData.Where(p => p.DataName == "CostCodeID").Select(p => p.Value).FirstOrDefault();
                    decimal? unitCost = itemData.Where(p => p.DataName == "UnitCost").Select(p => p.Value).FirstOrDefault();
                    decimal? unitPrice = itemData.Where(p => p.DataName == "UnitPrice").Select(p => p.Value).FirstOrDefault();
                    int? projectTaxID = (int?)itemData.Where(p => p.DataName == "ProjectTaxID").Select(p => p.Value).FirstOrDefault();

                    foreach (var areaID in AreaIDs)
                    {
                        try
                        {
                            var pci = new ProjectCorItems()
                            {
                                CorID = CorID,
                                MasterItemID = masterItemID,
                                ComponentID = "0",
                                Qty = Quantity,
                                StageID = projectStageID,
                                DivisionID = DivisionID,
                                AreaID = areaID,
                                Action = "Added",
                                UploadedDate = DateTime.Now,
                                SerialNumber = null,
                                IpAddress = null,
                                ProjectKitID = 0,
                                OptionID = 0,
                                ExcludePOR = coData.ExcludePOR,
                                ProductDescription = coData.ProductDescription,
                                SalesDescription = coData.SalesDescription,
                                SKU = coData.SKU,
                                WarranteePart = false,
                                OneOffPart = false,

                                //pricing
                                UnitCost = unitCost,
                                UnitPrice = unitPrice,
                                UnitHours = coData.EstHrs,
                                SalesTaxID = projectTaxID,
                                Margin = ((unitPrice - unitCost) / unitPrice),
                                Markup = 100 * ((unitPrice - unitCost) / unitCost),
                                SalesTaxed = coData.SalesTaxed,
                                LaborTaxed = coData.LaborTaxed,
                                CostTaxed = coData.PurchasedTaxed
                            };
                            // db.ProjectCorItems.Add(pci);
                            db.InsertCorItems(pci.CorID, pci.Action, pci.ItemID, ProjectID, pci.MasterItemID, pci.Qty, pci.DivisionID, pci.AreaID);
                        }
                        catch (Exception ex)
                        {
                            ex.Log();
                        }
                    }
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                ex.Log();
                return false;
            }

            return true;
        }

        public bool RemoveCorItems(int SiteCoID, int CorID, int ProjectID, List<int> ItemIDs)
        {
            try
            {
                var items = db.ProjectItems.Where(p => ItemIDs.Contains(p.ItemID)).ToList();
                var itemPricings = db.ProjectItemPricing.Where(p => ItemIDs.Contains(p.ItemID)).ToList();

                foreach (var item in items)
                {
                    var pricing = itemPricings.Where(p => p.ItemID == item.ItemID).FirstOrDefault();
                    if (pricing == null)
                    {
                        pricing = new ProjectItemPricing();
                    }

                    var pci = new ProjectCorItems()
                    {
                        CorID = CorID,
                        ItemID = item.ItemID,
                        MasterItemID = item.MasterItemID,
                        ComponentID = item.ComponentID,
                        Qty = item.Qty,
                        StageID = item.StageID,
                        DivisionID = item.DivisionID,
                        AreaID = item.AreaID,
                        Action = "Removed",
                        UploadedDate = DateTime.Now,
                        SerialNumber = item.SerialNumber,
                        IpAddress = item.IpAddress,
                        ProjectKitID = item.ProjectKitID,
                        OptionID = item.OptionID,
                        ExcludePOR = item.ExcludePOR,
                        ProductDescription = item.ProductDescription,
                        SalesDescription = item.SalesDescription,
                        SKU = item.SKU,
                        WarranteePart = item.WarranteePart,
                        OneOffPart = item.OneOffPart,

                        //pricing
                        UnitCost = pricing.UnitCost,
                        UnitPrice = pricing.UnitPrice,
                        UnitHours = pricing.UnitPrice,
                        SalesTaxID = pricing.SalesTaxID,
                        Margin = pricing.Margin,
                        Markup = pricing.Markup,
                        SalesTaxed = pricing.SalesTaxed,
                        LaborTaxed = pricing.LaborTaxed,
                        CostTaxed = pricing.CostTaxed
                    };
                    db.ProjectCorItems.Add(pci);
                }

                db.SaveChanges();
            }
            catch (Exception ex)
            {
                ex.Log();
                return false;
            }

            return true;
        }

        public bool ApproveCOR(int SiteUserID, int SiteCoID, int CorID, int ProjectID)
        {
            var cor = db.ProjectCor.Where(p => p.CorID == CorID && p.ProjectID == ProjectID).FirstOrDefault();
            if (cor == null)
            {
                return false;
            }

            cor.ApprovedDate = DateTime.Now;
            cor.ApproverID = SiteUserID;
            cor.Approved = true;
            cor.CorStatusID = db.CoCorStatus.Where(p => p.SiteCoID == SiteCoID && p.CorStatus == "Approved").Select(p => p.CorStatusID).FirstOrDefault();

            UpdateCorItemsInProject(SiteCoID, CorID, ProjectID);

            return true;
        }

        public bool UpdateCorItemsInProject(int SiteCoID, int CorID, int ProjectID)
        {
            string ADDED = "Added",
                REMOVED = "Removed";

            var actions = new List<string>() { ADDED, REMOVED };
            var corItems = db.ProjectCorItems.Where(p => p.CorID == CorID && actions.Contains(p.Action)).ToList();

            //Removed
            var removedItemIDs = corItems.Where(p => p.Action == REMOVED).Select(p => p.ItemID).ToList();
            if (removedItemIDs.Any())
            {
                var removedProjectItems = db.ProjectItems.Where(p => removedItemIDs.Contains(p.ItemID)).ToList();
                foreach (var item in removedProjectItems)
                {
                    item.Action = REMOVED;
                }

                db.SaveChanges();
            }

            //Added
            //var itemIDMappings = new List<Tuple<int, int>>();//corId | new ItemID
            var addedItems = corItems.Where(p => p.Action == ADDED).ToList();
            if (addedItems.Any())
            {
                List<ProjectItemPricing> itemPricings = new List<ProjectItemPricing>();

                foreach (var corItem in addedItems)
                {
                    var coData = db.CoData.Where(p => p.MasterItemID == corItem.MasterItemID).FirstOrDefault();
                    if (coData == null)
                    {
                        coData = new CoData();
                    }

                    int? stageMasterID = coData.StageID;

                    //if this stage doesn't exist in project then add it
                    if (stageMasterID.HasValue && !db.ProjectStages.Any(p => p.ProjectID == ProjectID && p.StageMasterID == stageMasterID))
                    {
                        AddProjectStages(ProjectID, new List<int>() { stageMasterID.Value });
                    }

                    //if item's CostCode doesn't exist in project then add it
                    if (coData.CostCodeID.HasValue && !db.ProjectCostCodes.Any(p => p.ProjectID == ProjectID && p.CostCodeID == coData.CostCodeID))
                    {
                        SaveProjectCostCode(coData.CostCodeID.Value, ProjectID);
                    }

                    //var itemData = db.GetDataForNewProjectItem(ProjectID, corItem.MasterItemID).ToList();
                    //int? projectStageID = (int?)itemData.Where(p => p.DataName == "StageID").Select(p => p.Value).FirstOrDefault();
                    //int? costCodeID = (int?)itemData.Where(p => p.DataName == "CostCodeID").Select(p => p.Value).FirstOrDefault();
                    //decimal? unitCost = itemData.Where(p => p.DataName == "UnitCost").Select(p => p.Value).FirstOrDefault();
                    //decimal? unitPrice = itemData.Where(p => p.DataName == "UnitPrice").Select(p => p.Value).FirstOrDefault();
                    //int? projectTaxID = (int?)itemData.Where(p => p.DataName == "ProjectTaxID").Select(p => p.Value).FirstOrDefault();

                    var pi = new ProjectItems()
                    {
                        SiteCoID = SiteCoID,
                        ProjectID = ProjectID,
                        MasterItemID = corItem.MasterItemID,
                        ComponentID = "0",
                        Qty = corItem.Qty,
                        StageID = corItem.StageID,
                        DivisionID = corItem.DivisionID,
                        AreaID = corItem.AreaID,
                        Action = "Added",
                        UploadedDate = DateTime.Now,
                        SerialNumber = null,
                        IpAddress = null,
                        ProjectKitID = corItem.ProjectKitID,
                        OptionID = corItem.OptionID,
                        ExcludePOR = corItem.ExcludePOR,
                        ProductDescription = corItem.ProductDescription,
                        SalesDescription = corItem.SalesDescription,
                        SKU = corItem.SKU,
                        WarranteePart = corItem.WarranteePart,
                        OneOffPart = corItem.OneOffPart
                    };
                    // db.ProjectItems.Add(pi);
                    db.InsertProjectItem(pi.SiteCoID, pi.ProjectID, pi.MasterItemID, pi.Qty, pi.DivisionID, pi.AreaID);
                    db.SaveChanges();

                    int newItemID = pi.ItemID;
                    var pip = new ProjectItemPricing()
                    {
                        ItemID = newItemID,
                        UnitCost = corItem.UnitCost,
                        UnitPrice = corItem.UnitPrice,
                        UnitHours = corItem.UnitHours,
                        SalesTaxID = corItem.SalesTaxID,
                        Margin = corItem.Margin,
                        Markup = corItem.Markup,
                        SalesTaxed = corItem.SalesTaxed,
                        LaborTaxed = corItem.LaborTaxed,
                        CostTaxed = corItem.CostTaxed
                    };

                    itemPricings.Add(pip);
                }
                //Another modification [InsertProjectItem]
                //foreach (var pip in itemPricings)
                //{
                //    InsertProjectItemPricing(pip);
                //}
            }

            db.SaveChanges();

            return true;
        }

        public bool UpdateCorInfo(int CorItemID, int AreaID, int DivisionID)
        {
            var item = db.ProjectCorItems.Where(p => p.CorItemID == CorItemID).FirstOrDefault();
            if (item == null)
            {
                return true;
            }

            item.AreaID = AreaID;
            item.DivisionID = DivisionID;

            db.SaveChanges();

            return true;
        }

        #endregion

        #region Sales Order

        public string GetNextSONumber()
        {
            var oldNum = db.ProjectSOs.OrderByDescending(p => p.SoID).Select(p => p.SoNumber).FirstOrDefault();
            return GetNextNumberInSeries(oldNum, "SO");
        }

        public int SaveSO(ProjectSOs SOToSave)
        {
            if (SOToSave.SoID == 0)
            {
                db.ProjectSOs.Add(SOToSave);
            }
            else
            {
                var dbSO = db.ProjectSOs.Where(p => p.SoID == SOToSave.SoID).FirstOrDefault();
                if (dbSO != null)
                {
                    dbSO.SiteCoID = SOToSave.SiteCoID;
                    dbSO.ProjectID = SOToSave.ProjectID;
                    dbSO.SoName = SOToSave.SoName;
                    dbSO.SoNumber = SOToSave.SoNumber;
                    dbSO.SoDate = SOToSave.SoDate;
                    dbSO.SoStatusID = SOToSave.SoStatusID;
                    dbSO.SoTermID = SOToSave.SoTermID;
                    dbSO.CreatedByUserID = SOToSave.CreatedByUserID;
                }
            }

            db.SaveChanges();

            return SOToSave.SoID;
        }

        public bool ApproveSO(int SiteUserID, int SOID, int ProjectID)
        {
            var so = db.ProjectSOs.Where(p => p.SoID == SOID && p.ProjectID == ProjectID).FirstOrDefault();
            if (so == null)
            {
                return false;
            }

            so.ApprovedDate = DateTime.Now;
            so.ApproverID = SiteUserID;
            so.Approved = true;

            db.SaveChanges();

            return true;
        }

        public bool AddSOItems(int SiteCoID, int SOID, int ProjectID, int Quantity, List<int> ItemIDs)
        {
            if (ItemIDs == null || !ItemIDs.Any())
            {
                return false;
            }

            try
            {
                var alreadyAddedItemIDs = db.ProjectSoItems.Where(p => ItemIDs.Contains(p.ItemID ?? 0)).Select(p => p.ItemID ?? 0).ToList();
                ItemIDs = ItemIDs.Except(alreadyAddedItemIDs).ToList();

                foreach (var itemID in ItemIDs)
                {
                    var soItem = new ProjectSoItems()
                    {
                        Action = "Add",
                        ItemID = itemID,
                        Qty = Quantity,
                        SoID = SOID
                    };

                    db.ProjectSoItems.Add(soItem);
                }

                db.SaveChanges();

            }
            catch (Exception ex)
            {
                ex.Log();
                return false;
            }

            return true;
        }

        #endregion

        #region Transfer

        public string GetNextTransferNumber()
        {
            var oldNum = db.ProcurementTransfers.OrderByDescending(p => p.TransferID).Select(p => p.TransferNumber).FirstOrDefault();
            return GetNextNumberInSeries(oldNum, "TFR");
        }

        public int SaveTransferRequest(int SiteCoID, int SiteUserID, int PorID, string TrasnferNumber, string Reason, List<int> PorItemIDs,
            int FromProjectID, List<int> FromProjectItemIDs, List<int> ToProjectItemIDs)
        {
            if (FromProjectItemIDs == null || ToProjectItemIDs == null ||
                FromProjectItemIDs.Count() != ToProjectItemIDs.Count())
            {
                return 0;
            }

            var toPorInfo = db.ProjectPurchaseOrders.Where(p => p.PurchaseOrderID == PorID).FirstOrDefault();
            if (toPorInfo == null)
            {
                return 0;
            }

            var toProjectInfo = db.ProjectInfo.Where(p => p.ProjectID == toPorInfo.ProjectID).FirstOrDefault();
            if (toProjectInfo == null)
            {
                return 0;
            }

            #region Transfer
            var q = db.GetTransferTypeByProjectID(toPorInfo.ProjectID).FirstOrDefault();
            var transfer = new ProcurementTransfers()
            {
                SiteCoID = SiteCoID,
                SiteUserID = SiteUserID,
                TransferTypeID = Convert.ToInt32(q.TransferTypeID),
                //  TransferTypeID = (toProjectInfo.ProjectTypeID == 3 || toProjectInfo.ProjectTypeID == 4) ? 1 : 2,
                Subject = "",           //no field in view ??
                TransferNumber = TrasnferNumber,
                TransferDate = DateTime.Now,
                TransferStatusID = null, //no field in view ??
                Reason = Reason
            };

            db.ProcurementTransfers.Add(transfer);
            db.SaveChanges();
            int newTransferID = transfer.TransferID;

            #endregion

            #region Transfer Items

            //get masterItemIDs for items
            var toItems = db.ProjectItems.Where(p => ToProjectItemIDs.Contains(p.ItemID)).Select(p => new { p.ItemID, p.MasterItemID }).OrderBy(p => p.MasterItemID).ToList();
            var fromItems = db.ProjectItems.Where(p => FromProjectItemIDs.Contains(p.ItemID)).Select(p => new { p.ItemID, p.MasterItemID }).OrderBy(p => p.MasterItemID).ToList();

            //get items' other data
            var toItemData = db.GetTransferToList(PorID, string.Join(",", PorItemIDs)).ToList();
            var fromItemData = db.GetTransferFromItems(FromProjectID, string.Join(",", toItems.Select(p => p.MasterItemID))).ToList();

            //combine itemIDs
            var items = fromItems.Zip(toItems, (f, t) => new { fromItemID = f.ItemID, toItemID = t.ItemID, fromMasterItemID = f.MasterItemID, toMasterItemID = t.MasterItemID });

            foreach (var item in items)
            {
                var toItem = toItemData.Where(p => p.ToItemID == item.toItemID).FirstOrDefault();
                var fromItem = fromItemData.Where(p => p.FromItemID == item.fromItemID).FirstOrDefault();
                if (toItem == null || fromItem == null)
                {
                    continue;
                }

                var trItem = new ProcurementTransferItems()
                {
                    TransferID = newTransferID,

                    FromProjectID = FromProjectID,
                    FromPorID = fromItem.FromPorID,
                    FromProcurementID = fromItem.FromProcurementID,
                    FromItemID = fromItem.FromItemID,
                    FromCustodyLocationID = fromItem.FromCustodyLocationID,

                    ToPorID = toItem.ToPorID,
                    ToProcurementID = toItem.ToProcurementID,
                    ToItemID = toItem.ToItemID,
                    ToCustodyLocationID = toItem.ToCustodyLocationID

                };

                db.ProcurementTransferItems.Add(trItem);


                if (transfer.TransferTypeID == 2)
                {
                    //FROM 
                    var poq = db.ProjectOrderQuantity.Where(p => p.PurchaseOrderID == fromItem.FromPorID && p.MasterItemID == item.fromMasterItemID).FirstOrDefault();
                    if (poq != null)
                    {
                        if (poq.QuantityOrdered == 1)
                        {
                            db.ProjectOrderQuantity.Remove(poq);
                        }
                        else if (poq.QuantityOrdered > 1)
                        {
                            poq.QuantityOrdered = poq.QuantityOrdered - ((int?)toItem.Qty);
                        }
                    }

                    var pro = db.Procurement.Where(p => p.PurchaseOrderID == fromItem.FromPorID && p.ItemID == item.fromItemID).FirstOrDefault();
                    if (pro != null && poq.QuantityOrdered == (int?)toItem.Qty)
                    {
                        db.Procurement.Remove(pro);
                    }

                    var projItem = db.ProjectItems.Where(p => p.ItemID == item.fromItemID).FirstOrDefault();
                    if (projItem != null && poq.QuantityOrdered == (int?)toItem.Qty)
                    {
                        projItem.Action = "Removed";
                    }

                    //TO
                    var toPro = db.Procurement.Where(p => p.PurchaseOrderID == toItem.ToPorID && p.ItemID == item.toItemID).FirstOrDefault();
                    if (toPro != null)
                    {
                        toPro.ArrivalDate = pro.ArrivalDate;
                        toPro.RequestDate = pro.RequestDate;
                        toPro.ReceivedDate = pro.ReceivedDate;
                        toPro.CustodyLocationID = pro.CustodyLocationID;
                    }
                }
                else if (transfer.TransferTypeID == 1)
                {
                    //FROM
                    int newPorID = 0;
                    var por = db.ProjectPurchaseOrders.Where(p => p.PurchaseOrderID == fromItem.FromPorID).FirstOrDefault();
                    if (por != null)
                    {
                        var newPor = new ProjectPurchaseOrders()
                        {
                            AcctPorID = por.AcctPorID,
                            ArrivalDate = por.ArrivalDate,
                            CreatorID = por.CreatorID,
                            DateCreated = por.DateCreated,
                            DateOrdered = por.DateOrdered,
                            Notes = por.Notes,
                            Priority = por.Priority,
                            ProcurementStatusID = por.ProcurementStatusID,
                            ProjectID = por.ProjectID,
                            PurchaseOrderNumber = GetNextPurchaseOrderNumber(por.ProjectID ?? 0, ""),
                            RequestedDate = por.RequestedDate,
                            ShippingMethodID = por.ShippingMethodID,
                            ShipToID = por.ShipToID,
                            SiteCoID = por.SiteCoID,
                            TrackingNumber = por.TrackingNumber,
                            VendorID = por.VendorID
                        };
                        db.ProjectPurchaseOrders.Add(newPor);

                        db.SaveChanges();

                        newPorID = newPor.PurchaseOrderID;
                    }

                    var poq = db.ProjectOrderQuantity.Where(p => p.PurchaseOrderID == fromItem.FromPorID && p.MasterItemID == item.fromMasterItemID).FirstOrDefault();
                    if (poq != null)
                    {
                        poq.PurchaseOrderID = newPorID;
                    }

                    var pro = db.Procurement.Where(p => p.PurchaseOrderID == fromItem.FromPorID && p.ItemID == item.fromItemID).FirstOrDefault();
                    if (pro != null)
                    {
                        pro.PurchaseOrderID = newPorID;
                        pro.ArrivalDate = null;
                        pro.RequestDate = DateTime.Now;
                        pro.ReceivedDate = null;
                        pro.CustodyLocationID = null;
                    }

                    //TO
                    var toPro = db.Procurement.Where(p => p.PurchaseOrderID == toItem.ToPorID && p.ItemID == item.toItemID).FirstOrDefault();
                    if (toPro != null)
                    {
                        toPro.ArrivalDate = pro.ArrivalDate;
                        toPro.RequestDate = pro.RequestDate;
                        toPro.ReceivedDate = pro.ReceivedDate;
                        toPro.CustodyLocationID = pro.CustodyLocationID;
                    }
                }
            }

            #endregion

            return transfer.TransferID;
        }

        #endregion

        #region Return

        public string GetNextReturnNumber()
        {
            return GetNextTransferNumber();
        }

        public int SaveReturnRequest(int SiteCoID, int SiteUserID, int FromProjectID, int? ToProjectID,
            DateTime? Date, string TransferNumber, int? TransferStatusID, string Subject, string Reason,
            List<int> FromItemIDs)
        {
            if (FromItemIDs == null || FromItemIDs.Count() == 0)
            {
                return 0;
            }

            var fromProjectInfo = db.ProjectInfo.Where(p => p.ProjectID == FromProjectID).FirstOrDefault();
            if (fromProjectInfo == null)
            {
                return 0;
            }

            var transfer = new ProcurementTransfers()
            {
                SiteCoID = SiteCoID,
                SiteUserID = SiteUserID,
                TransferTypeID = (fromProjectInfo.ProjectTypeID == 3 || fromProjectInfo.ProjectTypeID == 4) ? 1 : 2,
                Subject = Subject,
                TransferNumber = TransferNumber,
                TransferDate = Date ?? DateTime.Now,
                TransferStatusID = TransferStatusID,
                Reason = Reason
            };
            db.SaveChanges();

            int newTransferID = transfer.TransferID;

            var fromItemsData = db.GetPendingItemsByProjectID(FromProjectID).ToList();

            foreach (var itemID in FromItemIDs)
            {
                var fromItem = fromItemsData.Where(p => p.ViewID == itemID).FirstOrDefault();

                var trItem = new ProcurementTransferItems()
                {
                    TransferID = newTransferID,

                    FromProjectID = fromItem.ViewID,
                    FromPorID = fromItem.FromPorID,
                    FromProcurementID = fromItem.FromProcurementID,
                    FromItemID = fromItem.FromItemID,
                    FromCustodyLocationID = fromItem.FromCustodyLocationID,

                    ToProjectID = ToProjectID,
                    ToPorID = null,             //TODO: Assign correct values
                    ToProcurementID = null,     //TODO: Assign correct values
                    ToItemID = null,            //TODO: Assign correct values
                    ToCustodyLocationID = null  //TODO: Assign correct values
                };
                db.ProcurementTransferItems.Add(trItem);

                if (transfer.TransferTypeID == 3)
                {
                    //RTI

                    //TODO: Update various tables like ProjectCor, ProjectCorItems... etc


                }
                else if (transfer.TransferTypeID == 4)
                {
                    //RTM

                    //TODO: Update various tables like ProjectCor, ProjectCorItems... etc
                }
            }

            return transfer.TransferID;
        }

        #endregion

        #region Salez Toolz
        public string GetAccessTokenByUserId(int userId)
        {
            string AccessToken = string.Empty;
            string SalezToolzUserID = Convert.ToString(userId);
            AccessToken = db.CoSalezToolz.Where(x => x.SalezToolzUserID == SalezToolzUserID).FirstOrDefault().OAuthAccessToken;
            return AccessToken;
        }
        public string GetRefreshTokenByUserId(int userId)
        {
            string AccessToken = string.Empty;
            string SalezToolzUserID = Convert.ToString(userId);
            AccessToken = db.CoSalezToolz.Where(x => x.SalezToolzUserID == SalezToolzUserID).FirstOrDefault().OAuthAccessTokenSecret;
            return AccessToken;
        }

        public void UpdateAccessToken(int userId, string OAuthAccessToken, string OAuthAccessTokenSecret)
        {
            string SalezToolzUserID = Convert.ToString(userId);
            var STAccessToken = db.CoSalezToolz.Where(x => x.SalezToolzUserID == SalezToolzUserID).FirstOrDefault();
            if (STAccessToken != null)
            {
                STAccessToken.OAuthAccessToken = OAuthAccessToken;
                STAccessToken.OAuthAccessTokenSecret = OAuthAccessTokenSecret;
                db.SaveChanges();
            }
        }
        public CoSalezToolz GetCoSalezToolzData(int userId)
        {
            string SalezToolzUserID = Convert.ToString(userId);
            CoSalezToolz data = db.CoSalezToolz.Where(x => x.SalezToolzUserID == SalezToolzUserID).FirstOrDefault();
            return data;
        }
        #endregion
    }
}
