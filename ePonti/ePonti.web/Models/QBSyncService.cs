using ePonti.BOL;
using Intuit.Ipp.Core;
using Intuit.Ipp.DataService;
using System;
using System.Collections.Generic;
using System.Linq;
using Intuit.Ipp.QueryFilter;
using Intuit.Ipp.Data;

namespace ePonti.web.Models
{
    public class QBSyncService : ePonti.web.Controllers._baseMVCController
    {
        public delegate IntuitBatchResponse DelegateBatchCompleted(IntuitBatchRequest batchRequest);
        DataserviceFactory dataserviceFactory = null;
        DataService dataService = null;
        QBSyncdto syncObjects = null;
        ePontiv2Entities db = new ePontiv2Entities();
        // private SyncRepository syncRepository = null;
        /// <summary>
        /// Fire up the repos and service context in the constructor.
        /// </summary>
        /// <param name="oAuthorization"></param>
        public QBSyncService(QBAuthorizationdto oAuthorization)
        {
            dataserviceFactory = new DataserviceFactory(oAuthorization);
            dataService = dataserviceFactory.getDataService();
            syncObjects = new QBSyncdto();
            // syncRepository = new SyncRepository();
        }
        #region <<Prop>>
        public ServiceContext ServiceContext { get { return dataserviceFactory.getServiceContext; } }
        #endregion        
        internal QBSyncdto GetDatafromDBCustomer(QBSyncdto syncObjects, int siteCoID)
        {
            try
            {
                List<Customer> custList = new List<Customer>();
                var customerList = db.GetQbCustomersBySiteCoID(siteCoID).ToList();
                foreach (var cust in customerList)
                {
                    Customer qboCust = new Customer();
                    qboCust.SyncToken = "1";
                    if (cust.Id != null && cust.Id != "")
                        qboCust.Id = cust.Id.ToString();
                    qboCust.GivenName = cust.GivenName;
                    //if (cust.Title != null && cust.Title != "")
                    //   qboCust.Title = cust.Title;
                    // if (cust.MiddleName != null && cust.MiddleName != "")
                    //  qboCust.MiddleName = cust.MiddleName;
                    if (cust.FamilyName != null && cust.FamilyName != "")
                        qboCust.FamilyName = cust.FamilyName;
                    //  if (cust.Suffix != null && cust.Suffix != "")
                    //    qboCust.Suffix = cust.Suffix;
                    if (cust.DisplayName != null && cust.DisplayName != "")
                        // cust.DisplayName = cust.DisplayName.Replace(",", " ");
                        qboCust.DisplayName = cust.DisplayName;
                    if (cust.CompanyName != null && cust.CompanyName != "")
                    {
                        if (cust.CompanyName.Contains("'"))
                            cust.CompanyName = cust.CompanyName.Replace("'", "\'");
                        qboCust.CompanyName = cust.CompanyName;
                    }
                    if (cust.PrimaryPhone != null && cust.PrimaryPhone != "")
                        qboCust.PrimaryPhone = new TelephoneNumber { FreeFormNumber = cust.PrimaryPhone.ToString() };
                    if (cust.Mobile != null && cust.Mobile != "")
                        qboCust.Mobile = new TelephoneNumber { FreeFormNumber = cust.Mobile.ToString() };
                    if (cust.PrimaryEmailAddr != null && cust.PrimaryEmailAddr != "")
                        qboCust.PrimaryEmailAddr = new EmailAddress { Address = cust.PrimaryEmailAddr.ToString() };
                    qboCust.BillAddr = new PhysicalAddress();
                    if (cust.BillAddrId != 0)
                        qboCust.BillAddr.Id = Convert.ToString(cust.BillAddrId);
                    if (cust.BillAddrLine1 != null && cust.BillAddrLine1 != "")
                        qboCust.BillAddr.Line1 = cust.BillAddrLine1;
                    if (cust.BillAddrLine2 != null && cust.BillAddrLine2 != "")
                        qboCust.BillAddr.Line2 = cust.BillAddrLine2;
                    if (cust.BillAddrCity != null && cust.BillAddrCity != "")
                        qboCust.BillAddr.City = cust.BillAddrCity;
                    if (cust.BillAddrCountry != null && cust.BillAddrCountry != "")
                        qboCust.BillAddr.Country = cust.BillAddrCountry;
                    if (cust.BillAddrCountrySubDivisionCode != null && cust.BillAddrCountrySubDivisionCode != "")
                        qboCust.BillAddr.CountrySubDivisionCode = cust.BillAddrCountrySubDivisionCode;
                    if (cust.BillAddrPostalCode != null && cust.BillAddrPostalCode != "")
                        qboCust.BillAddr.PostalCode = cust.BillAddrPostalCode;
                    if (cust.BillAddrLine1 != null && cust.BillAddrLine1 != "")
                        qboCust.BillAddr.Line1 = cust.BillAddrLine1;

                    custList.Add(qboCust);
                }
                syncObjects.CustomerList = custList;
                return syncObjects;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        internal QBSyncdto GetDatafromDBVendor(QBSyncdto syncObjects, int siteCoID)
        {
            try
            {
                List<Vendor> vendorList = new List<Vendor>();
                var VendorList = db.GetQbVendorsBySiteCoID(siteCoID).ToList();
                foreach (var vendor in VendorList)
                {
                    Vendor qbovendor = new Vendor();
                    qbovendor.SyncToken = "1";
                    if (vendor.Id != null && vendor.Id != "")
                        qbovendor.Id = vendor.Id.ToString();
                    qbovendor.GivenName = vendor.GivenName;
                    //if (cust.Title != null && cust.Title != "")
                    //   qboCust.Title = cust.Title;
                    // if (cust.MiddleName != null && cust.MiddleName != "")
                    //  qboCust.MiddleName = cust.MiddleName;
                    if (vendor.FamilyName != null && vendor.FamilyName != "")
                        qbovendor.FamilyName = vendor.FamilyName;
                    //  if (cust.Suffix != null && cust.Suffix != "")
                    //    qboCust.Suffix = cust.Suffix;
                    if (vendor.DisplayName != null && vendor.DisplayName != "")
                        // cust.DisplayName = cust.DisplayName.Replace(",", " ");
                        qbovendor.DisplayName = vendor.DisplayName;
                    if (vendor.CompanyName != null && vendor.CompanyName != "")
                    {
                        if (vendor.CompanyName.Contains("'"))
                            vendor.CompanyName = vendor.CompanyName.Replace("'", "\'");
                        qbovendor.CompanyName = vendor.CompanyName;
                    }
                    if (vendor.PrimaryPhone != null && vendor.PrimaryPhone != "")
                        qbovendor.PrimaryPhone = new TelephoneNumber { FreeFormNumber = vendor.PrimaryPhone.ToString() };
                    if (vendor.Mobile != null && vendor.Mobile != "")
                        qbovendor.Mobile = new TelephoneNumber { FreeFormNumber = vendor.Mobile.ToString() };
                    if (vendor.PrimaryEmailAddr != null && vendor.PrimaryEmailAddr != "")
                        qbovendor.PrimaryEmailAddr = new EmailAddress { Address = vendor.PrimaryEmailAddr.ToString() };
                    qbovendor.BillAddr = new PhysicalAddress();
                    if (vendor.BillAddrId != 0)
                        qbovendor.BillAddr.Id = Convert.ToString(vendor.BillAddrId);
                    if (vendor.BillAddrLine1 != null && vendor.BillAddrLine1 != "")
                        qbovendor.BillAddr.Line1 = vendor.BillAddrLine1;
                    if (vendor.BillAddrLine2 != null && vendor.BillAddrLine2 != "")
                        qbovendor.BillAddr.Line2 = vendor.BillAddrLine2;
                    if (vendor.BillAddrCity != null && vendor.BillAddrCity != "")
                        qbovendor.BillAddr.City = vendor.BillAddrCity;
                    if (vendor.BillAddrCountry != null && vendor.BillAddrCountry != "")
                        qbovendor.BillAddr.Country = vendor.BillAddrCountry;
                    if (vendor.BillAddrCountrySubDivisionCode != null && vendor.BillAddrCountrySubDivisionCode != "")
                        qbovendor.BillAddr.CountrySubDivisionCode = vendor.BillAddrCountrySubDivisionCode;
                    if (vendor.BillAddrPostalCode != null && vendor.BillAddrPostalCode != "")
                        qbovendor.BillAddr.PostalCode = vendor.BillAddrPostalCode;
                    if (vendor.BillAddrLine1 != null && vendor.BillAddrLine1 != "")
                        qbovendor.BillAddr.Line1 = vendor.BillAddrLine1;

                    vendorList.Add(qbovendor);
                }
                syncObjects.VendorList = vendorList;
                return syncObjects;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        internal QBSyncdto SyncCustomer(object controller, QBSyncdto syncObjects)
        {
            try
            {
                Dictionary<OperationEnum, object> batchEntries = new Dictionary<OperationEnum, object>();
                for (int i = 0; i < syncObjects.CustomerList.Count; i++)
                {
                    int? siteCoID = base.siteusercompanyid;
                    string EXISTING_CUSTOMER_QUERY = "";
                    if (syncObjects.CustomerList[i].Id == "" || syncObjects.CustomerList[i].Id == null)
                    {
                        EXISTING_CUSTOMER_QUERY = string.Format("select * from customer where familyName = '{0}' and givenName='{1}'", syncObjects.CustomerList[i].FamilyName, syncObjects.CustomerList[i].GivenName);
                        QueryService<Customer> queryService = new QueryService<Customer>(dataserviceFactory.getServiceContext);
                        Customer resultFound = queryService.ExecuteIdsQuery(EXISTING_CUSTOMER_QUERY).FirstOrDefault<Customer>();
                        if (resultFound == null)
                        {
                            createcustomer(syncObjects, i);
                            // batchEntries.Add(OperationEnum.create, entity);
                        }
                        else
                        {
                            if (resultFound.FamilyName.ToUpper() == syncObjects.CustomerList[i].FamilyName.ToUpper())
                            {
                                if (resultFound.GivenName.ToUpper() == syncObjects.CustomerList[i].GivenName.ToUpper())
                                {
                                    syncObjects.CustomerList[i] = resultFound;
                                }
                                else
                                {
                                    createcustomer(syncObjects, i);
                                    //batchEntries.Add(OperationEnum.create, entity);
                                }
                            }
                            else
                            {
                                createcustomer(syncObjects, i);
                                // batchEntries.Add(OperationEnum.create, entity);
                            }
                        }
                    }
                }
                // ReadOnlyCollection<IntuitBatchResponse> batchResponses = Batch<Customer>(ServiceContext, batchEntries);

                return syncObjects;
            }
            catch (Intuit.Ipp.Exception.FaultException ex)
            {
                throw ex;
            }
            catch (Intuit.Ipp.Exception.InvalidTokenException ex)
            {
                throw ex;
            }
            catch (Intuit.Ipp.Exception.SdkException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                SyncCustomer(controller, syncObjects);
                return syncObjects;
            }
        }
        private void createcustomer(QBSyncdto syncObjects, int i)
        {
            // Customer entity = syncObjects.CustomerList[i];
            Customer entity = dataService.Add<Customer>(syncObjects.CustomerList[i]);
            syncObjects.CustomerList[i] = entity;
            syncObjects.IsCustomerSync = true;
            string fn = syncObjects.CustomerList[i].GivenName;
            string ln = syncObjects.CustomerList[i].FamilyName;
            var q = db.CoContacts.Where(p => p.ContactFirstName == fn && p.ContactLastName == ln).FirstOrDefault();
            db.UpdateQbContactID(entity.Id, ln, fn);
            q.AcctCustomerID = entity.Id;
            db.SaveChanges();
        }

        static void CallbackMethod(IAsyncResult result)
        {
            DelegateBatchCompleted delegateBatch = (DelegateBatchCompleted)result.AsyncState;
            IntuitBatchResponse batchResponse = delegateBatch.EndInvoke(result);
        }
        internal QBSyncdto SyncVendor(object controller, QBSyncdto syncObjects)
        {
            try
            {
                for (int i = 0; i < syncObjects.VendorList.Count; i++)
                {
                    int? siteCoID = base.siteusercompanyid;
                    string EXISTING_VENDOR_QUERY = "";
                    if (syncObjects.VendorList[i].Id == "" || syncObjects.VendorList[i].Id == null)
                    {
                        EXISTING_VENDOR_QUERY = string.Format("select * from Vendor where familyName = '{0}' and givenName='{1}'", syncObjects.VendorList[i].FamilyName, syncObjects.VendorList[i].GivenName);
                        QueryService<Vendor> queryService = new QueryService<Vendor>(dataserviceFactory.getServiceContext);
                        Vendor resultFound = queryService.ExecuteIdsQuery(EXISTING_VENDOR_QUERY).FirstOrDefault<Vendor>();
                        if (resultFound == null)
                        {
                            createVendor(syncObjects, i);
                        }
                        else
                        {
                            if (resultFound.FamilyName.ToUpper() == syncObjects.VendorList[i].FamilyName.ToUpper())
                            {
                                if (resultFound.GivenName.ToUpper() == syncObjects.VendorList[i].GivenName.ToUpper())
                                {
                                    syncObjects.VendorList[i] = resultFound;
                                }
                                else
                                {
                                    createVendor(syncObjects, i);
                                }
                            }
                            else
                                createVendor(syncObjects, i);
                        }
                    }

                }
                // syncObjects =controller.Save(controller, syncObjects);
                return syncObjects;
            }
            catch (Intuit.Ipp.Exception.FaultException ex)
            {
                throw ex;
            }
            catch (Intuit.Ipp.Exception.InvalidTokenException ex)
            {
                throw ex;
            }
            catch (Intuit.Ipp.Exception.SdkException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void createVendor(QBSyncdto syncObjects, int i)
        {
            Vendor entity = dataService.Add<Vendor>(syncObjects.VendorList[i]);
            syncObjects.VendorList[i] = entity;
            syncObjects.IsVendorSync = true;
            string fn = syncObjects.VendorList[i].GivenName;
            string ln = syncObjects.VendorList[i].FamilyName;
            // var q = db.CoContacts.Where(p => p.ContactFirstName == fn && p.ContactLastName == ln).FirstOrDefault();
            db.UpdateQbContactID(entity.Id, ln, fn);
            //q.AcctCustomerID = entity.Id;
            db.SaveChanges();
        }
        public QBSyncdto IsCustSync(QBSyncdto syncObjects, QBSyncService service, int siteCoID)
        {
            try
            {
                Dictionary<string, bool> isSync = new Dictionary<string, bool>();
                var custDataInDb = service.GetDatafromDBCustomer(syncObjects, siteCoID);

                if (custDataInDb.CustomerList.Count > 0)
                {
                    for (int i = 0; i < custDataInDb.CustomerList.Count; i++)
                    {
                        string EXISTING_CUSTOMER_QUERY = "";
                        if (custDataInDb.CustomerList[i].Id == "" || custDataInDb.CustomerList[i].Id == null)
                        {
                            EXISTING_CUSTOMER_QUERY = string.Format("select * from customer where familyName = '{0}' and givenName='{1}'", custDataInDb.CustomerList[i].FamilyName, custDataInDb.CustomerList[i].GivenName);
                            QueryService<Customer> queryService = new QueryService<Customer>(service.ServiceContext);
                            Customer resultFound = queryService.ExecuteIdsQuery(EXISTING_CUSTOMER_QUERY).FirstOrDefault<Customer>();
                            if (resultFound != null)
                            {
                                if (resultFound.FamilyName.ToUpper() == custDataInDb.CustomerList[i].FamilyName.ToUpper())
                                {
                                    if (resultFound.GivenName.ToUpper() == custDataInDb.CustomerList[i].GivenName.ToUpper())
                                    {
                                        custDataInDb.CustomerList[i].Id = resultFound.Id;
                                        if (!isSync.ContainsKey(custDataInDb.CustomerList[i].GivenName))
                                            isSync.Add(custDataInDb.CustomerList[i].GivenName, true);
                                    }
                                    else
                                    {
                                        if (!isSync.ContainsKey(custDataInDb.CustomerList[i].GivenName))
                                            isSync.Add(custDataInDb.CustomerList[i].GivenName, false);
                                    }
                                }
                                else
                                {
                                    if (!isSync.ContainsKey(custDataInDb.CustomerList[i].GivenName))
                                        isSync.Add(custDataInDb.CustomerList[i].GivenName, false);
                                }
                            }
                            else
                            {
                                if (!isSync.ContainsKey(custDataInDb.CustomerList[i].GivenName))
                                    isSync.Add(custDataInDb.CustomerList[i].GivenName, false);
                            }
                        }


                    }
                    if (isSync.Where(x => x.Value == false).Any())
                    {
                        custDataInDb.IsCustomerSync = false;
                    }
                    else
                    {
                        custDataInDb.IsCustomerSync = true;
                    }
                }
                else
                {
                    custDataInDb.IsCustomerSync = false;
                }
                return custDataInDb;
            }
            catch (Exception ex)
            {
                //string filePath = @"d:/log.txt";

                //using (StreamWriter writer = new StreamWriter(filePath, true))
                //{
                //    writer.WriteLine("Message :" + ex.Message + "<br/>" + ex.InnerException + "<br/>" + Environment.NewLine + "StackTrace :" + ex.StackTrace +
                //       "" + Environment.NewLine + "Date :" + DateTime.Now.ToString());
                //    writer.WriteLine(Environment.NewLine + "-----------------------------------------------------------------------------" + Environment.NewLine);
                //}
                throw ex;
            }
        }
        public QBSyncdto IsVendorSync(QBSyncdto syncObjects, QBSyncService service, int siteCoID)
        {
            try
            {
                Dictionary<string, bool> isSync = new Dictionary<string, bool>();
                var vendorDataInDb = service.GetDatafromDBVendor(syncObjects, siteCoID);

                if (vendorDataInDb.VendorList.Count > 0)
                {
                    for (int i = 0; i < vendorDataInDb.VendorList.Count; i++)
                    {
                        string EXISTING_VENDOR_QUERY = "";
                        if (vendorDataInDb.VendorList[i].Id == "" || vendorDataInDb.VendorList[i].Id == null)
                        {
                            EXISTING_VENDOR_QUERY = string.Format("select * from Vendor where familyName = '{0}' and givenName='{1}'", vendorDataInDb.VendorList[i].FamilyName, vendorDataInDb.VendorList[i].GivenName);
                            QueryService<Vendor> queryService = new QueryService<Vendor>(service.ServiceContext);
                            Vendor resultFound = queryService.ExecuteIdsQuery(EXISTING_VENDOR_QUERY).FirstOrDefault<Vendor>();
                            if (resultFound != null)
                            {
                                if (resultFound.FamilyName.ToUpper() == vendorDataInDb.VendorList[i].FamilyName.ToUpper())
                                {
                                    if (resultFound.GivenName.ToUpper() == vendorDataInDb.VendorList[i].GivenName.ToUpper())
                                    {
                                        vendorDataInDb.VendorList[i].Id = resultFound.Id;
                                        if (!isSync.ContainsKey(vendorDataInDb.VendorList[i].GivenName))
                                            isSync.Add(vendorDataInDb.VendorList[i].GivenName, true);
                                    }
                                    else
                                    {
                                        if (!isSync.ContainsKey(vendorDataInDb.VendorList[i].GivenName))
                                            isSync.Add(vendorDataInDb.VendorList[i].GivenName, false);
                                    }
                                }
                                else
                                {
                                    if (!isSync.ContainsKey(vendorDataInDb.VendorList[i].GivenName))
                                        isSync.Add(vendorDataInDb.VendorList[i].GivenName, false);
                                }
                            }
                            else
                            {
                                if (!isSync.ContainsKey(vendorDataInDb.VendorList[i].GivenName))
                                    isSync.Add(vendorDataInDb.VendorList[i].GivenName, false);
                            }
                        }


                    }
                    if (isSync.Where(x => x.Value == false).Any())
                    {
                        vendorDataInDb.IsVendorSync = false;
                    }
                    else
                    {
                        vendorDataInDb.IsVendorSync = true;
                    }
                }
                else
                {
                    vendorDataInDb.IsVendorSync = false;
                }
                return vendorDataInDb;
            }
            catch (Exception ex)
            {
                //string filePath = @"d:/log.txt";

                //using (StreamWriter writer = new StreamWriter(filePath, true))
                //{
                //    writer.WriteLine("Message :" + ex.Message + "<br/>" + ex.InnerException + "<br/>" + Environment.NewLine + "StackTrace :" + ex.StackTrace +
                //       "" + Environment.NewLine + "Date :" + DateTime.Now.ToString());
                //    writer.WriteLine(Environment.NewLine + "-----------------------------------------------------------------------------" + Environment.NewLine);
                //}
                throw ex;
            }
        }
        //public QBSyncdto IsItemSync(QBSyncdto syncObjects, QBSyncService service, int siteCoID)
        //{
        //    try
        //    {
        //        Dictionary<string, bool> isSync = new Dictionary<string, bool>();
        //        var vendorDataInDb = service.GetDatafromDBVendor(syncObjects, siteCoID);

        //        if (vendorDataInDb.VendorList.Count > 0)
        //        {
        //            for (int i = 0; i < vendorDataInDb.VendorList.Count; i++)
        //            {
        //                string EXISTING_VENDOR_QUERY = "";
        //                if (vendorDataInDb.VendorList[i].Id == "" || vendorDataInDb.VendorList[i].Id == null)
        //                {
        //                    EXISTING_VENDOR_QUERY = string.Format("select * from Vendor where familyName = '{0}' and givenName='{1}'", vendorDataInDb.VendorList[i].FamilyName, vendorDataInDb.VendorList[i].GivenName);
        //                    QueryService<Vendor> queryService = new QueryService<Vendor>(service.ServiceContext);
        //                    Vendor resultFound = queryService.ExecuteIdsQuery(EXISTING_VENDOR_QUERY).FirstOrDefault<Vendor>();
        //                    if (resultFound != null)
        //                    {
        //                        if (resultFound.FamilyName.ToUpper() == vendorDataInDb.VendorList[i].FamilyName.ToUpper())
        //                        {
        //                            if (resultFound.GivenName.ToUpper() == vendorDataInDb.VendorList[i].GivenName.ToUpper())
        //                            {
        //                                vendorDataInDb.VendorList[i].Id = resultFound.Id;
        //                                if (!isSync.ContainsKey(vendorDataInDb.VendorList[i].GivenName))
        //                                    isSync.Add(vendorDataInDb.VendorList[i].GivenName, true);
        //                            }
        //                            else
        //                            {
        //                                if (!isSync.ContainsKey(vendorDataInDb.VendorList[i].GivenName))
        //                                    isSync.Add(vendorDataInDb.VendorList[i].GivenName, false);
        //                            }
        //                        }
        //                        else
        //                        {
        //                            if (!isSync.ContainsKey(vendorDataInDb.VendorList[i].GivenName))
        //                                isSync.Add(vendorDataInDb.VendorList[i].GivenName, false);
        //                        }
        //                    }
        //                    else
        //                    {
        //                        if (!isSync.ContainsKey(vendorDataInDb.VendorList[i].GivenName))
        //                            isSync.Add(vendorDataInDb.VendorList[i].GivenName, false);
        //                    }
        //                }


        //            }
        //            if (isSync.Where(x => x.Value == false).Any())
        //            {
        //                vendorDataInDb.IsVendorSync = false;
        //            }
        //            else
        //            {
        //                vendorDataInDb.IsVendorSync = true;
        //            }
        //        }
        //        else
        //        {
        //            vendorDataInDb.IsVendorSync = false;
        //        }
        //        return vendorDataInDb;
        //    }
        //    catch (Exception ex)
        //    {
        //        //string filePath = @"d:/log.txt";

        //        //using (StreamWriter writer = new StreamWriter(filePath, true))
        //        //{
        //        //    writer.WriteLine("Message :" + ex.Message + "<br/>" + ex.InnerException + "<br/>" + Environment.NewLine + "StackTrace :" + ex.StackTrace +
        //        //       "" + Environment.NewLine + "Date :" + DateTime.Now.ToString());
        //        //    writer.WriteLine(Environment.NewLine + "-----------------------------------------------------------------------------" + Environment.NewLine);
        //        //}
        //        throw ex;
        //    }
        //}
        internal QBSyncdto GetDatafromModelCustomer(QBSyncdto syncObjects, int siteCoID)
        {
            try
            {
                List<Customer> custList = new List<Customer>();
                var cust = db.GetQbCustomersBySiteCoID(siteCoID).OrderByDescending(p => p.ViewID).Take(1).FirstOrDefault();
                Customer qboCust = new Customer();
                qboCust.SyncToken = "1";
                //  qboCust.Id = cust.ViewID.ToString();
                qboCust.GivenName = cust.GivenName;
                //if (cust.Title != null && cust.Title != "")
                //   qboCust.Title = cust.Title;
                // if (cust.MiddleName != null && cust.MiddleName != "")
                //  qboCust.MiddleName = cust.MiddleName;
                if (cust.FamilyName != null && cust.FamilyName != "")
                    qboCust.FamilyName = cust.FamilyName;
                //  if (cust.Suffix != null && cust.Suffix != "")
                //    qboCust.Suffix = cust.Suffix;
                if (cust.DisplayName != null && cust.DisplayName != "")
                    qboCust.DisplayName = cust.DisplayName;
                if (cust.CompanyName != null && cust.CompanyName != "")
                    qboCust.CompanyName = cust.CompanyName;
                if (cust.PrimaryPhone != null && cust.PrimaryPhone != "")
                    qboCust.PrimaryPhone = new TelephoneNumber { FreeFormNumber = cust.PrimaryPhone.ToString() };
                if (cust.Mobile != null && cust.Mobile != "")
                    qboCust.Mobile = new TelephoneNumber { FreeFormNumber = cust.Mobile.ToString() };
                if (cust.PrimaryEmailAddr != null && cust.PrimaryEmailAddr != "")
                    qboCust.PrimaryEmailAddr = new EmailAddress { Address = cust.PrimaryEmailAddr.ToString() };
                qboCust.BillAddr = new PhysicalAddress();
                if (cust.BillAddrId != 0)
                    qboCust.BillAddr.Id = Convert.ToString(cust.BillAddrId);
                if (cust.BillAddrLine1 != null && cust.BillAddrLine1 != "")
                    qboCust.BillAddr.Line1 = cust.BillAddrLine1;
                if (cust.BillAddrLine2 != null && cust.BillAddrLine2 != "")
                    qboCust.BillAddr.Line2 = cust.BillAddrLine2;
                if (cust.BillAddrCity != null && cust.BillAddrCity != "")
                    qboCust.BillAddr.City = cust.BillAddrCity;
                if (cust.BillAddrCountry != null && cust.BillAddrCountry != "")
                    qboCust.BillAddr.Country = cust.BillAddrCountry;
                if (cust.BillAddrCountrySubDivisionCode != null && cust.BillAddrCountrySubDivisionCode != "")
                    qboCust.BillAddr.CountrySubDivisionCode = cust.BillAddrCountrySubDivisionCode;
                if (cust.BillAddrPostalCode != null && cust.BillAddrPostalCode != "")
                    qboCust.BillAddr.PostalCode = cust.BillAddrPostalCode;
                if (cust.BillAddrLine1 != null && cust.BillAddrLine1 != "")
                    qboCust.BillAddr.Line1 = cust.BillAddrLine1;
                custList.Add(qboCust);
                syncObjects.CustomerList = custList;
                return syncObjects;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        internal QBSyncdto GetDatafromModelJobs(QBSyncdto syncObjects, int siteCoID, int ProjectID, out int parentid)
        {
            try
            {
                List<Customer> custList = new List<Customer>();
                var job = db.ProjectInfo.Where(p => p.ProjectID == ProjectID).FirstOrDefault();
                if (job != null)
                {
                    var customer = db.GetQbCustomersBySiteCoID(siteCoID).Where(x => x.ViewID == job.ContactID).FirstOrDefault();
                    if (customer != null)
                    {
                        Intuit.Ipp.Data.Customer qboCust = new Intuit.Ipp.Data.Customer();
                        qboCust.Job = true;
                        qboCust.ParentRef = new ReferenceType { Value = string.IsNullOrEmpty(customer.Id) ? "0" : customer.Id };
                        qboCust.SyncToken = "1";
                        //if (job.ProjectName != null && job.ProjectName != "")
                        qboCust.DisplayName = job.ProjectName + ' ' + job.ProjectNumber;
                        if (customer.CompanyName != null && customer.CompanyName != "")
                        {
                            if (customer.CompanyName.Contains("'"))
                                customer.CompanyName = customer.CompanyName.Replace("'", "\'");
                            qboCust.CompanyName = customer.CompanyName;
                        }
                        if (job.ProjectPhone != null && job.ProjectPhone != "")
                            qboCust.PrimaryPhone = new Intuit.Ipp.Data.TelephoneNumber { FreeFormNumber = job.ProjectPhone };
                        qboCust.Mobile = new Intuit.Ipp.Data.TelephoneNumber { FreeFormNumber = job.ProjectPhone };

                        //if (job.ProjectAddress1 != null && job.ProjectAddress1 != "")
                        //    qboCust.PrimaryEmailAddr = new Intuit.Ipp.Data.EmailAddress { Address = job.ProjectAddress1 };
                        qboCust.BillAddr = new Intuit.Ipp.Data.PhysicalAddress();
                        if (job.ProjectAddress1 != null && job.ProjectAddress1 != "")
                            qboCust.BillAddr.Line1 = job.ProjectAddress1;
                        if (job.ProjectAddress2 != null && job.ProjectAddress2 != "")
                            qboCust.BillAddr.Line2 = job.ProjectAddress2;
                        if (job.ProjectCity != null && job.ProjectCity != "")
                            qboCust.BillAddr.City = job.ProjectCity;
                        if (job.ProjectCountry != null && job.ProjectCountry != "")
                            qboCust.BillAddr.Country = job.ProjectCountry;
                        if (job.ProjectZip != null && job.ProjectZip != "")
                            qboCust.BillAddr.PostalCode = job.ProjectZip;
                        custList.Add(qboCust);
                    }
                    syncObjects.CustomerList = custList;
                }
                parentid = (int)job.ContactID;
                return syncObjects;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        internal Customer GetParentCustomerFromDB(int parentid, int siteusercompanyid)
        {
            var qboCust = new Customer();
            var cust = db.GetQbCustomersBySiteCoID(siteusercompanyid).Where(x => x.ViewID == parentid).FirstOrDefault();
            if (cust != null)
            {
                qboCust.SyncToken = "1";
                if (cust.Id != null && cust.Id != "")
                    qboCust.Id = cust.Id.ToString();
                qboCust.GivenName = cust.GivenName;
                if (cust.FamilyName != null && cust.FamilyName != "")
                    qboCust.FamilyName = cust.FamilyName;
                if (cust.DisplayName != null && cust.DisplayName != "")
                    qboCust.DisplayName = cust.DisplayName;
                if (cust.CompanyName != null && cust.CompanyName != "")
                {
                    if (cust.CompanyName.Contains("'"))
                        cust.CompanyName = cust.CompanyName.Replace("'", "\'");
                    qboCust.CompanyName = cust.CompanyName;
                }
                if (cust.PrimaryPhone != null && cust.PrimaryPhone != "")
                    qboCust.PrimaryPhone = new Intuit.Ipp.Data.TelephoneNumber { FreeFormNumber = cust.PrimaryPhone.ToString() };
                if (cust.Mobile != null && cust.Mobile != "")
                    qboCust.Mobile = new Intuit.Ipp.Data.TelephoneNumber { FreeFormNumber = cust.Mobile.ToString() };
                if (cust.PrimaryEmailAddr != null && cust.PrimaryEmailAddr != "")
                    qboCust.PrimaryEmailAddr = new Intuit.Ipp.Data.EmailAddress { Address = cust.PrimaryEmailAddr.ToString() };
                qboCust.BillAddr = new Intuit.Ipp.Data.PhysicalAddress();
                if (cust.BillAddrId != 0)
                    qboCust.BillAddr.Id = Convert.ToString(cust.BillAddrId);
                if (cust.BillAddrLine1 != null && cust.BillAddrLine1 != "")
                    qboCust.BillAddr.Line1 = cust.BillAddrLine1;
                if (cust.BillAddrLine2 != null && cust.BillAddrLine2 != "")
                    qboCust.BillAddr.Line2 = cust.BillAddrLine2;
                if (cust.BillAddrCity != null && cust.BillAddrCity != "")
                    qboCust.BillAddr.City = cust.BillAddrCity;
                if (cust.BillAddrCountry != null && cust.BillAddrCountry != "")
                    qboCust.BillAddr.Country = cust.BillAddrCountry;
                if (cust.BillAddrCountrySubDivisionCode != null && cust.BillAddrCountrySubDivisionCode != "")
                    qboCust.BillAddr.CountrySubDivisionCode = cust.BillAddrCountrySubDivisionCode;
                if (cust.BillAddrPostalCode != null && cust.BillAddrPostalCode != "")
                    qboCust.BillAddr.PostalCode = cust.BillAddrPostalCode;
                if (cust.BillAddrLine1 != null && cust.BillAddrLine1 != "")
                    qboCust.BillAddr.Line1 = cust.BillAddrLine1;
                return qboCust;
            }
            return null;
        }
        internal QBSyncdto SyncJob(object controller, QBSyncdto syncObjects, int id, int parentid, int siteusercompanyid)
        {
            try
            {
                Dictionary<OperationEnum, object> batchEntries = new Dictionary<OperationEnum, object>();
                for (int i = 0; i < syncObjects.CustomerList.Count; i++)
                {
                    int? siteCoID = base.siteusercompanyid;
                    string EXISTING_CUSTOMER_QUERY = "";
                    EXISTING_CUSTOMER_QUERY = string.Format("select * from customer where id = '{0}'", syncObjects.CustomerList[i].ParentRef.Value);
                    QueryService<Customer> queryService = new QueryService<Customer>(dataserviceFactory.getServiceContext);
                    Customer parentcust = queryService.ExecuteIdsQuery(EXISTING_CUSTOMER_QUERY).FirstOrDefault<Customer>();
                    if (parentcust == null)
                    {
                        var Customer = GetParentCustomerFromDB(parentid, siteusercompanyid);
                        if (Customer != null)
                        {
                            Customer.Id = null;
                            EXISTING_CUSTOMER_QUERY = string.Format("select * from customer where familyName = '{0}' and givenName='{1}'", Customer.FamilyName, Customer.GivenName);
                            queryService = new QueryService<Customer>(dataserviceFactory.getServiceContext);
                            Customer resultFound = queryService.ExecuteIdsQuery(EXISTING_CUSTOMER_QUERY).FirstOrDefault<Customer>();
                            if (resultFound == null)
                            {
                                EXISTING_CUSTOMER_QUERY = string.Format("select * from customer where displayname='{0}'", Customer.DisplayName);
                                queryService = new QueryService<Customer>(dataserviceFactory.getServiceContext);
                                resultFound = queryService.ExecuteIdsQuery(EXISTING_CUSTOMER_QUERY).FirstOrDefault<Customer>();
                            }
                            if (resultFound == null)
                            {
                                parentcust = dataService.Add<Customer>(Customer);
                                var query = db.CoContacts.Where(x => x.ContactID == parentid).FirstOrDefault();
                                if (query != null)
                                {
                                    query.AcctCustomerID = parentcust.Id;
                                    db.SaveChanges();
                                }
                            }
                            else
                            {
                                if (resultFound.FamilyName.ToUpper() == Customer.FamilyName.ToUpper())
                                {
                                    if (resultFound.GivenName.ToUpper() == Customer.GivenName.ToUpper())
                                    {
                                        parentcust = resultFound;
                                    }
                                    else
                                    {
                                        parentcust = dataService.Add<Customer>(Customer);
                                    }
                                }
                                else
                                {
                                    parentcust = dataService.Add<Customer>(Customer);
                                }
                                var query = db.CoContacts.Where(x => x.ContactID == parentid).FirstOrDefault();
                                if (query != null)
                                {
                                    query.AcctCustomerID = parentcust.Id;
                                    db.SaveChanges();
                                }
                            }
                        }
                    }
                    if (parentcust != null)
                    {
                        EXISTING_CUSTOMER_QUERY = string.Format("select * from customer where job = true and FullyQualifiedName = '{0}'", parentcust.DisplayName + ":" + syncObjects.CustomerList[i].DisplayName);
                        queryService = new QueryService<Customer>(dataserviceFactory.getServiceContext);
                        Customer resultFound = queryService.ExecuteIdsQuery(EXISTING_CUSTOMER_QUERY).FirstOrDefault<Customer>();
                        if (resultFound == null)
                        {
                            syncObjects.CustomerList[i].ParentRef = null;
                            syncObjects.CustomerList[i].Job = false;
                            Customer entity = dataService.Add<Customer>(syncObjects.CustomerList[i]);
                            if (entity != null)
                            {
                                entity.Job = true;
                                entity.ParentRef = new ReferenceType { Value = parentcust.Id, name = parentcust.DisplayName };
                                Customer updates = dataService.Update<Customer>(entity);
                                var job = db.ProjectInfo.Where(x => x.ProjectID == id).FirstOrDefault();
                                if (job != null)
                                {
                                    job.AcctJobID = updates.Id;
                                    db.SaveChanges();
                                }
                            }
                        }
                    }

                }
                return syncObjects;
            }
            catch (Intuit.Ipp.Exception.FaultException ex)
            {
                throw ex;
            }
            catch (Intuit.Ipp.Exception.InvalidTokenException ex)
            {
                throw ex;
            }
            catch (Intuit.Ipp.Exception.SdkException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                //SyncJob(controller, syncObjects);
                return syncObjects;
            }
        }
        internal QBSyncdto GetCustomersFromQB(object controller, QBSyncdto syncObjects)
        {
            try
            {
                string EXISTING_CUSTOMER_QUERY = "";
                EXISTING_CUSTOMER_QUERY = string.Format("select * from customer where job = false");
                QueryService<Customer> queryService = new QueryService<Customer>(dataserviceFactory.getServiceContext);
                List<Customer> resultFound = queryService.ExecuteIdsQuery(EXISTING_CUSTOMER_QUERY).ToList();
                if (resultFound != null)
                {
                    syncObjects.CustomerList = resultFound;
                }
                return syncObjects;
            }
            catch (Intuit.Ipp.Exception.FaultException ex)
            {
                throw ex;
            }
            catch (Intuit.Ipp.Exception.InvalidTokenException ex)
            {
                throw ex;
            }
            catch (Intuit.Ipp.Exception.SdkException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                SyncCustomer(controller, syncObjects);
                return syncObjects;
            }
        }
        internal QBSyncdto GetVendorsFromQB(object controller, QBSyncdto syncObjects)
        {
            try
            {
                string EXISTING_CUSTOMER_QUERY = "";
                EXISTING_CUSTOMER_QUERY = string.Format("select * from vendor");
                QueryService<Vendor> queryService = new QueryService<Vendor>(dataserviceFactory.getServiceContext);
                List<Vendor> resultFound = queryService.ExecuteIdsQuery(EXISTING_CUSTOMER_QUERY).ToList();
                if (resultFound != null)
                {
                    syncObjects.VendorList = resultFound;
                }
                return syncObjects;
            }
            catch (Intuit.Ipp.Exception.FaultException ex)
            {
                throw ex;
            }
            catch (Intuit.Ipp.Exception.InvalidTokenException ex)
            {
                throw ex;
            }
            catch (Intuit.Ipp.Exception.SdkException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                SyncCustomer(controller, syncObjects);
                return syncObjects;
            }
        }
        internal QBSyncdto GetDatafromModelVendor(QBSyncdto syncObjects, int siteCoID)
        {
            try
            {
                List<Vendor> vendorList = new List<Vendor>();
                var vendor = db.GetQbVendorsBySiteCoID(siteCoID).OrderByDescending(p => p.ViewID).Take(1).FirstOrDefault();
                Vendor qboVendor = new Vendor();
                qboVendor.SyncToken = "1";
                qboVendor.GivenName = vendor.GivenName;
                if (vendor.FamilyName != null && vendor.FamilyName != "")
                    qboVendor.FamilyName = vendor.FamilyName;
                if (vendor.DisplayName != null && vendor.DisplayName != "")
                    qboVendor.DisplayName = vendor.DisplayName;
                if (vendor.CompanyName != null && vendor.CompanyName != "")
                    qboVendor.CompanyName = vendor.CompanyName;
                if (vendor.PrimaryPhone != null && vendor.PrimaryPhone != "")
                    qboVendor.PrimaryPhone = new TelephoneNumber { FreeFormNumber = vendor.PrimaryPhone.ToString() };
                if (vendor.Mobile != null && vendor.Mobile != "")
                    qboVendor.Mobile = new TelephoneNumber { FreeFormNumber = vendor.Mobile.ToString() };
                if (vendor.PrimaryEmailAddr != null && vendor.PrimaryEmailAddr != "")
                    qboVendor.PrimaryEmailAddr = new EmailAddress { Address = vendor.PrimaryEmailAddr.ToString() };
                qboVendor.BillAddr = new PhysicalAddress();
                if (vendor.BillAddrId != 0)
                    qboVendor.BillAddr.Id = Convert.ToString(vendor.BillAddrId);
                if (vendor.BillAddrLine1 != null && vendor.BillAddrLine1 != "")
                    qboVendor.BillAddr.Line1 = vendor.BillAddrLine1;
                if (vendor.BillAddrLine2 != null && vendor.BillAddrLine2 != "")
                    qboVendor.BillAddr.Line2 = vendor.BillAddrLine2;
                if (vendor.BillAddrCity != null && vendor.BillAddrCity != "")
                    qboVendor.BillAddr.City = vendor.BillAddrCity;
                if (vendor.BillAddrCountry != null && vendor.BillAddrCountry != "")
                    qboVendor.BillAddr.Country = vendor.BillAddrCountry;
                if (vendor.BillAddrCountrySubDivisionCode != null && vendor.BillAddrCountrySubDivisionCode != "")
                    qboVendor.BillAddr.CountrySubDivisionCode = vendor.BillAddrCountrySubDivisionCode;
                if (vendor.BillAddrPostalCode != null && vendor.BillAddrPostalCode != "")
                    qboVendor.BillAddr.PostalCode = vendor.BillAddrPostalCode;
                if (vendor.BillAddrLine1 != null && vendor.BillAddrLine1 != "")
                    qboVendor.BillAddr.Line1 = vendor.BillAddrLine1;
                vendorList.Add(qboVendor);
                syncObjects.VendorList = vendorList;
                return syncObjects;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        internal QBSyncdto GetDatafromModelItems(QBSyncdto syncObjects, int siteCoID, int? ItemID, string type)
        {
            try
            {

                //Account act1 = new Account();
                //act1.Name = "Inventory Asset";
                //act1.AccountType = AccountTypeEnum.OtherCurrentAsset;
                //act1.AccountSubType = "OtherCurrentAssets";
                //syncObjects.account = act1;
                //Account accnt1 = createAccount(syncObjects);

                List<Item> itemList = new List<Item>();
                //  var itemadded =db.ProjectItems.Where(p=>p.SiteCoID==siteCoID).OrderByDescending(p => p.ItemID).Take(1).FirstOrDefault();
                var item = db.GetQbItemInfoByItemID(ItemID).FirstOrDefault();
                Item qboItem = new Item();
                qboItem.SyncToken = "1";
                qboItem.ItemCategoryType = item.Category;
                // type for tesing only
                if (type == "Service")
                {
                    qboItem.Type = ItemTypeEnum.Service;
                }
                else if (type == "Inventory")
                {
                    qboItem.TrackQtyOnHand = true;
                    qboItem.TrackQtyOnHandSpecified = true;
                    qboItem.QtyOnHandSpecified = true;
                    qboItem.InvStartDate = DateTime.Now;
                    qboItem.QtyOnHand = 1;
                    qboItem.Type = ItemTypeEnum.Inventory;
                }
                else if (type == "NonInventory")
                {
                    qboItem.Type = ItemTypeEnum.NonInventory;
                }
                qboItem.Name = item.Name;

                if (item.PurchaseCost != null)
                {
                    qboItem.PurchaseCost = (decimal)item.PurchaseCost;
                    qboItem.PurchaseCostSpecified = true;
                }
                qboItem.PurchaseDesc = item.PurchaseDesc==null ? "" : item.PurchaseDesc;
                qboItem.Description = item.Description == null ? "" : item.Description;
                if (item.ReorderPoint != null)
                    qboItem.ReorderPoint = (decimal)item.ReorderPoint;
                if (item.UnitPrice != null)
                {
                    qboItem.UnitPrice = (decimal)item.UnitPrice;
                    qboItem.UnitPriceSpecified = true;
                }
                qboItem.Sku = item.Sku;
                qboItem.IncomeAccountRef = new ReferenceType();
                qboItem.IncomeAccountRef.Value = "79";
                qboItem.IncomeAccountRef.name = "Sales of Product Income";
                qboItem.TypeSpecified = true;
                qboItem.InvStartDateSpecified = true;
                qboItem.AssetAccountRef = new ReferenceType();
                qboItem.AssetAccountRef.name = "Inventory Asset";
                qboItem.AssetAccountRef.Value = "1049";
                qboItem.ExpenseAccountRef = new ReferenceType();
                qboItem.ExpenseAccountRef.name = "Cost of Goods Sold";
                qboItem.ExpenseAccountRef.Value = "0";

                itemList.Add(qboItem);
                syncObjects.ItemList = itemList;
                return syncObjects;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        internal QBSyncdto SyncItems(object controller, QBSyncdto syncObjects)
        {
            try
            {
                for (int i = 0; i < syncObjects.ItemList.Count; i++)
                {
                    int? siteCoID = base.siteusercompanyid;
                    string EXISTING_CUSTOMER_QUERY = "";

                    if (syncObjects.ItemList[i].Id == "" || syncObjects.ItemList[i].Id == null)
                    {
                        QueryService<Account> queryService1 = new QueryService<Account>(dataserviceFactory.getServiceContext);
                        var resultFound1 = queryService1.ExecuteIdsQuery("select * from Account").ToList<Account>();
                        EXISTING_CUSTOMER_QUERY = string.Format("select * from Item where Name ='" + syncObjects.ItemList[i].Name + "'");
                        QueryService<Item> queryService = new QueryService<Item>(dataserviceFactory.getServiceContext);
                        Item resultFound = queryService.ExecuteIdsQuery(EXISTING_CUSTOMER_QUERY).FirstOrDefault<Item>();

                        if (resultFound == null)
                        {
                            syncObjects.ItemList[i].IncomeAccountRef.Value = resultFound1.Where(x => x.AccountType == AccountTypeEnum.Income && x.Name == "Sales of Product Income").FirstOrDefault().Id;
                            syncObjects.ItemList[i].AssetAccountRef.Value = resultFound1.Where(x => x.AccountType == AccountTypeEnum.OtherCurrentAsset).FirstOrDefault().Id;
                            syncObjects.ItemList[i].ExpenseAccountRef.Value = resultFound1.Where(x => x.AccountType == AccountTypeEnum.CostofGoodsSold).FirstOrDefault().Id;
                            createItems(syncObjects, i);
                        }
                        else
                        {
                            syncObjects.ItemList[i] = resultFound;
                        }
                    }
                }
                return syncObjects;
            }
            catch (Intuit.Ipp.Exception.FaultException ex)
            {
                throw ex;
            }
            catch (Intuit.Ipp.Exception.InvalidTokenException ex)
            {
                throw ex;
            }
            catch (Intuit.Ipp.Exception.SdkException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void createItems(QBSyncdto syncObjects, int i)
        {
            Item entity = dataService.Add<Item>(syncObjects.ItemList[i]);
            syncObjects.ItemList[i] = entity;
            syncObjects.IsItemSync = true;
            string Sku = syncObjects.ItemList[i].Sku;
            var q = db.CoData.Where(p => p.SKU == Sku).FirstOrDefault();
            if (q != null)
            {
                q.AcctItemID = entity.Id;
                db.SaveChanges();
            }
        }
        public Vendor GetVenderFromDB(int contactID, int siteCoID)
        {
            var vendor = db.GetQbVendorsBySiteCoID(siteCoID).Where(p => p.ViewID == contactID).FirstOrDefault();
            if (vendor != null)
            {
                Intuit.Ipp.Data.Vendor qboVendor = new Intuit.Ipp.Data.Vendor();
                qboVendor.SyncToken = "1";
                qboVendor.GivenName = vendor.GivenName;
                if (vendor.FamilyName != null && vendor.FamilyName != "")
                    qboVendor.FamilyName = vendor.FamilyName;
                if (vendor.DisplayName != null && vendor.DisplayName != "")
                    qboVendor.DisplayName = vendor.DisplayName;
                if (vendor.CompanyName != null && vendor.CompanyName != "")
                    qboVendor.CompanyName = vendor.CompanyName;
                if (vendor.PrimaryPhone != null && vendor.PrimaryPhone != "")
                    qboVendor.PrimaryPhone = new Intuit.Ipp.Data.TelephoneNumber { FreeFormNumber = vendor.PrimaryPhone.ToString() };
                if (vendor.Mobile != null && vendor.Mobile != "")
                    qboVendor.Mobile = new Intuit.Ipp.Data.TelephoneNumber { FreeFormNumber = vendor.Mobile.ToString() };
                if (vendor.PrimaryEmailAddr != null && vendor.PrimaryEmailAddr != "")
                    qboVendor.PrimaryEmailAddr = new Intuit.Ipp.Data.EmailAddress { Address = vendor.PrimaryEmailAddr.ToString() };
                qboVendor.BillAddr = new Intuit.Ipp.Data.PhysicalAddress();
                if (vendor.BillAddrId != 0)
                    qboVendor.BillAddr.Id = Convert.ToString(vendor.BillAddrId);
                if (vendor.BillAddrLine1 != null && vendor.BillAddrLine1 != "")
                    qboVendor.BillAddr.Line1 = vendor.BillAddrLine1;
                if (vendor.BillAddrLine2 != null && vendor.BillAddrLine2 != "")
                    qboVendor.BillAddr.Line2 = vendor.BillAddrLine2;
                if (vendor.BillAddrCity != null && vendor.BillAddrCity != "")
                    qboVendor.BillAddr.City = vendor.BillAddrCity;
                if (vendor.BillAddrCountry != null && vendor.BillAddrCountry != "")
                    qboVendor.BillAddr.Country = vendor.BillAddrCountry;
                if (vendor.BillAddrCountrySubDivisionCode != null && vendor.BillAddrCountrySubDivisionCode != "")
                    qboVendor.BillAddr.CountrySubDivisionCode = vendor.BillAddrCountrySubDivisionCode;
                if (vendor.BillAddrPostalCode != null && vendor.BillAddrPostalCode != "")
                    qboVendor.BillAddr.PostalCode = vendor.BillAddrPostalCode;
                if (vendor.BillAddrLine1 != null && vendor.BillAddrLine1 != "")
                    qboVendor.BillAddr.Line1 = vendor.BillAddrLine1;
                return qboVendor;
            }
            return null;
        }

        public QBAndDb_POR_Items GetItemFromDB(int PORid, ItemTypeEnum type)
        {
            //  var itemadded =db.ProjectItems.Where(p=>p.SiteCoID==siteCoID).OrderByDescending(p => p.ItemID).Take(1).FirstOrDefault();
            List<Item> items = new List<Item>();
            QBAndDb_POR_Items mainitems = new QBAndDb_POR_Items();
            var Items = db.GetQbPoItemsByPorID(PORid).ToList();
            mainitems.PORItems = Items;
            foreach (var item in Items)
            {
                Item qboItem = new Item();
                qboItem.SyncToken = "1";
                qboItem.ItemCategoryType = item.Category;
                // type for tesing only

                qboItem.Type = type;
                qboItem.Name = item.Name;
                qboItem.Description = item.Description;
                if (item.PurchaseCost != null)
                    qboItem.AvgCost = (decimal)item.PurchaseCost;
                qboItem.PurchaseDesc = item.PurchaseDesc;
                if (item.ReorderPoint != null)
                    qboItem.ReorderPoint = (decimal)item.ReorderPoint;
                qboItem.Sku = item.Sku;
                qboItem.Active = true;
                qboItem.ActiveSpecified = true;
                qboItem.IncomeAccountRef = new ReferenceType();
                qboItem.IncomeAccountRef.Value = "79";
                qboItem.IncomeAccountRef.name = "Sales of Product Income";
                qboItem.TypeSpecified = true;
                qboItem.InvStartDateSpecified = true;
                qboItem.AssetAccountRef = new ReferenceType();
                qboItem.AssetAccountRef.name = "Inventory Asset";
                qboItem.AssetAccountRef.Value = "1049";
                qboItem.ExpenseAccountRef = new ReferenceType();
                qboItem.ExpenseAccountRef.name = "Cost of Goods Sold";
                qboItem.ExpenseAccountRef.Value = "0";
                if (type == ItemTypeEnum.Inventory)
                {
                    qboItem.TrackQtyOnHand = true;
                    qboItem.TrackQtyOnHandSpecified = true;
                    qboItem.QtyOnHandSpecified = true;
                    qboItem.InvStartDate = DateTime.UtcNow.AddDays(-1);
                    qboItem.QtyOnHand = 1;
                }
                items.Add(qboItem);
                //return items;
            }
            mainitems.QbItems = items;
            return mainitems;
        }

        public QBAndDb_SO_Items GetSOItemsFromDB(int SOId, ItemTypeEnum type)
        {
            //  var itemadded =db.ProjectItems.Where(p=>p.SiteCoID==siteCoID).OrderByDescending(p => p.ItemID).Take(1).FirstOrDefault();
            List<Item> items = new List<Item>();
            QBAndDb_SO_Items mainitems = new QBAndDb_SO_Items();
            var Items = db.GetQbItemsBySoID(SOId).ToList();
            mainitems.SOItems = Items;
            foreach (var item in Items)
            {
                if(item.Category!=null && item.Category.Trim().ToLower()=="labor")
                {
                    type = ItemTypeEnum.Service;
                }
                Item qboItem = new Item();
                qboItem.SyncToken = "1";
                qboItem.ItemCategoryType = item.Category;
                // type for tesing only

                qboItem.Type = type;
                qboItem.Name = item.Item;
                qboItem.Description = item.Description;
                if (item.UnitPrice.HasValue)
                {
                    qboItem.UnitPrice = item.UnitPrice.Value;
                    qboItem.UnitPriceSpecified = true;
                }
                if (item.PurchaseCost.HasValue)
                {
                    qboItem.AvgCost = item.PurchaseCost.Value;
                    qboItem.PurchaseCost = item.PurchaseCost.Value;
                    qboItem.PurchaseCostSpecified = true;
                }
                qboItem.PurchaseDesc = item.PurchaseDesc;
                qboItem.Sku = item.Sku;
                qboItem.Active = true;
                qboItem.ActiveSpecified = true;
                qboItem.IncomeAccountRef = new ReferenceType();
                qboItem.IncomeAccountRef.Value = "79";
                qboItem.IncomeAccountRef.name = "Sales of Product Income";
                qboItem.TypeSpecified = true;
                qboItem.InvStartDateSpecified = true;
                qboItem.AssetAccountRef = new ReferenceType();
                qboItem.AssetAccountRef.name = "Inventory Asset";
                qboItem.AssetAccountRef.Value = "1049";
                qboItem.ExpenseAccountRef = new ReferenceType();
                qboItem.ExpenseAccountRef.name = "Cost of Goods Sold";
                qboItem.ExpenseAccountRef.Value = "0";
                //Just to check
                qboItem.Overview = item.ViewID.ToString();
                if (type == ItemTypeEnum.Inventory)
                {
                    qboItem.TrackQtyOnHand = true;
                    qboItem.TrackQtyOnHandSpecified = true;
                    qboItem.QtyOnHandSpecified = true;
                    qboItem.InvStartDate = DateTime.UtcNow.AddDays(-1);
                    qboItem.QtyOnHand = 1;
                }
                items.Add(qboItem);
                //return items;
            }
            mainitems.QbItems = items;
            return mainitems;
        }
        public QBSyncdto SyncPorder(QBSyncdto syncObjects, int siteCoID, int PORid, string ItemType)
        {
            try
            {

                QueryService<Account> _Accounts = new QueryService<Account>(dataserviceFactory.getServiceContext);
                var Accounts = _Accounts.ExecuteIdsQuery("select * from Account").ToList<Account>();

                var DBPOR = db.ProjectPurchaseOrders.Where(x => x.PurchaseOrderID == PORid).FirstOrDefault();
                if (DBPOR != null)
                {
                    if (string.IsNullOrEmpty(DBPOR.AcctPorID))
                    {
                        var EpVendor = GetVenderFromDB(Convert.ToInt32(DBPOR.VendorID), siteCoID);
                        if (EpVendor != null)
                        {
                            var Vendor = new Vendor();
                            var Items = new List<Item>();
                            var Item = new Item();
                            if (!string.IsNullOrEmpty(EpVendor.Id))
                            {
                                QueryService<Vendor> _Vendor = new QueryService<Vendor>(dataserviceFactory.getServiceContext);
                                Vendor = _Vendor.ExecuteIdsQuery("select * from Vendor where id=" + EpVendor.Id).FirstOrDefault<Vendor>();
                            }
                            else
                            {
                                string EXISTING_VENDOR_QUERY = string.Format("select * from Vendor where familyName = '{0}' and givenName='{1}'", EpVendor.FamilyName, EpVendor.GivenName, EpVendor.DisplayName);
                                QueryService<Vendor> queryService = new QueryService<Vendor>(dataserviceFactory.getServiceContext);
                                Vendor IsVendorExists = queryService.ExecuteIdsQuery(EXISTING_VENDOR_QUERY).FirstOrDefault<Vendor>();
                                if (IsVendorExists == null)
                                {
                                    EXISTING_VENDOR_QUERY = string.Format("select * from Vendor where displayname='{0}'", EpVendor.DisplayName);
                                    queryService = new QueryService<Vendor>(dataserviceFactory.getServiceContext);
                                    IsVendorExists = queryService.ExecuteIdsQuery(EXISTING_VENDOR_QUERY).FirstOrDefault<Vendor>();
                                }
                                if (IsVendorExists == null)
                                {
                                    Vendor = dataService.Add<Vendor>(EpVendor);
                                    string fn = Vendor.GivenName;
                                    string ln = Vendor.FamilyName;
                                    db.UpdateQbContactID(Vendor.Id, ln, fn);
                                }
                                else
                                {
                                    Vendor = IsVendorExists;
                                }
                            }
                            int parentid = 0;
                            string jobid = "";
                            if (DBPOR.ProjectID!=null)
                            {
                                syncObjects = GetDatafromModelJobs(syncObjects, siteCoID, Convert.ToInt32(DBPOR.ProjectID), out parentid);
                                if (syncObjects.CustomerList.Count > 0)
                                {
                                    syncObjects = SyncJob(this, syncObjects, Convert.ToInt32(DBPOR.ProjectID), parentid, siteCoID);
                                }
                                var job = db.ProjectInfo.Where(x => x.ProjectID == DBPOR.ProjectID).FirstOrDefault();
                                jobid = job != null && job.AcctJobID != null ? job.AcctJobID : "";
                            }
                            if (!string.IsNullOrEmpty(Vendor.Id))
                            {
                                //var contact = db.CoContacts.Where(x => x.ContactID == parentid).FirstOrDefault();
                                QBAndDb_POR_Items porItems = GetItemFromDB(PORid, (ItemType == "NonInventory" ? ItemTypeEnum.NonInventory : ItemType == "Service" ? ItemTypeEnum.Service : ItemTypeEnum.Inventory));

                                var EpItems = porItems.QbItems;
                                foreach (var EpItem in EpItems)
                                {
                                    if (!string.IsNullOrEmpty(EpItem.Id))
                                    {
                                        QueryService<Item> _Items = new QueryService<Item>(dataserviceFactory.getServiceContext);
                                        Item = _Items.ExecuteIdsQuery("select * from Item where Id ='" + EpItem.Id + "'").FirstOrDefault<Item>();
                                    }
                                    if(string.IsNullOrEmpty(EpItem.Id) || Item==null)
                                    {
                                        string EXISTING_CUSTOMER_QUERY = string.Format("select * from Item where Name ='" + EpItem.Name + "'");
                                        QueryService<Item> queryService = new QueryService<Item>(dataserviceFactory.getServiceContext);
                                        Item IsItemExists = queryService.ExecuteIdsQuery(EXISTING_CUSTOMER_QUERY).FirstOrDefault<Item>();
                                        if (IsItemExists == null)
                                        {
                                            //EpItem.IncomeAccountRef.Value = Accounts.Where(x => x.AccountType == AccountTypeEnum.Income && x.Name == "Sales of Product Income").FirstOrDefault().Id;
                                            //EpItem.AssetAccountRef.Value = Accounts.Where(x => x.AccountType == AccountTypeEnum.OtherCurrentAsset).FirstOrDefault().Id;
                                            //EpItem.ExpenseAccountRef.Value = Accounts.Where(x => x.AccountType == AccountTypeEnum.CostofGoodsSold).FirstOrDefault().Id;
                                            Account income = CheckAddGetAccount(Accounts, AccountTypeEnum.Income, "Sales of Product Income");
                                            if(Accounts.Where(s=>s.Id==income.Id).FirstOrDefault()==null)
                                            {
                                                Accounts.Add(income);
                                            }
                                            Account asset = CheckAddGetAccount(Accounts, AccountTypeEnum.OtherCurrentAsset, "Inventory Asset");
                                            if (Accounts.Where(s => s.Id == asset.Id).FirstOrDefault() == null)
                                            {
                                                Accounts.Add(asset);
                                            }
                                            Account costgoods = CheckAddGetAccount(Accounts, AccountTypeEnum.CostofGoodsSold, "Cost of Goods Sold");
                                            if (Accounts.Where(s => s.Id == costgoods.Id).FirstOrDefault() == null)
                                            {
                                                Accounts.Add(costgoods);
                                            }
                                            EpItem.IncomeAccountRef.Value = income.Id;
                                            EpItem.AssetAccountRef.Value = asset.Id;
                                            EpItem.ExpenseAccountRef.Value = costgoods.Id;
                                            Item = dataService.Add<Item>(EpItem);
                                            porItems.PORItems.Where(w => w.Name == EpItem.Name).ToList().ForEach(s => s.ID = Item.Id);
                                            string Sku = EpItem.Sku;
                                            int itemid = porItems.PORItems.Where(w => w.Name == EpItem.Name).FirstOrDefault().ViewID;
                                            var q = db.CoData.Where(p => p.MasterItemID == itemid).FirstOrDefault();
                                            if (q != null)
                                            {
                                                q.AcctItemID = Item.Id;
                                                db.SaveChanges();
                                            }
                                        }
                                        else
                                        {
                                            Item = IsItemExists;
                                        }
                                    }
                                    Items.Add(Item);
                                }
                                if (Items.Count > 0)
                                {
                                    //var act = Accounts.Where(x => x.AccountType == AccountTypeEnum.AccountsPayable);
                                    PurchaseOrder POR = new PurchaseOrder();
                                    POR.VendorRef = new ReferenceType { Value = Vendor.Id };
                                    //POR.APAccountRef = new ReferenceType { Value = Accounts.Where(x => x.AccountType == AccountTypeEnum.AccountsPayable && x.Name == "Accounts Payable (A/P)").FirstOrDefault().Id };
                                    Account account_p = CheckAddGetAccount(Accounts, AccountTypeEnum.AccountsPayable, "Accounts Payable (A/P)");
                                    if (Accounts.Where(s => s.Id == account_p.Id).FirstOrDefault() == null)
                                    {
                                        Accounts.Add(account_p);
                                    }
                                    POR.APAccountRef = new ReferenceType { Value = account_p.Id };
                                    var _line = new Line[Items.Count];
                                    int count = 0;
                                    foreach (var item in Items)
                                    {
                                        decimal qty = 1;
                                        decimal amt = 10;
                                        GetQbPoItemsByPorID_Result item_res = porItems.PORItems.Where(s => s.ID == item.Id).FirstOrDefault();
                                        if (item_res != null)
                                        {
                                            qty = item_res.Qty == null ? 1 : item_res.Qty.Value;
                                            amt = item_res.PurchaseCost == null ? 10 : item_res.PurchaseCost.Value;
                                        }
                                        var anyIntuitObject = new ItemBasedExpenseLineDetail
                                        {
                                            ItemRef = new ReferenceType { Value = item.Id },
                                            Qty = qty,
                                            QtySpecified = true
                                        };
                                        if (jobid != "")
                                        {
                                            anyIntuitObject.CustomerRef = new ReferenceType { Value = jobid };
                                        }

                                        Line l = new Line
                                        {
                                            Amount = amt,
                                            AmountSpecified = true,
                                            Description = item_res.PurchaseDesc==null ? "" : item_res.PurchaseDesc,
                                            AnyIntuitObject = anyIntuitObject,
                                            DetailType = LineDetailTypeEnum.ItemBasedExpenseLineDetail,
                                            DetailTypeSpecified = true,
                                        };
                                        _line[count] = l;
                                        count++;
                                    }
                                 
                                    POR.Line = _line;
                                    var _entity = dataService.Add<PurchaseOrder>(POR);
                                    DBPOR.AcctPorID = _entity.Id;
                                    db.SaveChanges();
                                }
                            }
                        }
                    }

                }
            }
            catch (Exception)
            {

                throw;
            }
            return syncObjects;
        }
        public QBSyncdto SyncSalesOrder(QBSyncdto syncObjects, int siteCoID, int SOid, string EstInvType, string ItemType)
        {
            try
            {

                QueryService<Account> _Accounts = new QueryService<Account>(dataserviceFactory.getServiceContext);
                var Accounts = _Accounts.ExecuteIdsQuery("select * from Account").ToList<Account>();

                var DBSO = db.ProjectSOs.Where(x => x.SoID == SOid).FirstOrDefault();
                if (DBSO != null)
                {
                    if (string.IsNullOrEmpty(DBSO.AcctSoID))
                    {
                        int parentid = 0;
                        string jobid = "";
                        var Items = new List<Item>();
                        var Item = new Item();
                        if (DBSO.ProjectID != null)
                        {
                            syncObjects = GetDatafromModelJobs(syncObjects, siteCoID, Convert.ToInt32(DBSO.ProjectID), out parentid);
                            if (syncObjects.CustomerList.Count > 0)
                            {
                                syncObjects = SyncJob(this, syncObjects, Convert.ToInt32(DBSO.ProjectID), parentid, siteCoID);
                            }
                            var job = db.ProjectInfo.Where(x => x.ProjectID == DBSO.ProjectID).FirstOrDefault();
                            jobid = job != null && job.AcctJobID != null ? job.AcctJobID : "";
                        }
                        //var contact = db.CoContacts.Where(x => x.ContactID == parentid).FirstOrDefault();
                        QBAndDb_SO_Items soItems = GetSOItemsFromDB(SOid, (ItemType == "NonInventory" ? ItemTypeEnum.NonInventory : ItemType == "Service" ? ItemTypeEnum.Service : ItemTypeEnum.Inventory));

                        var EpItems = soItems.QbItems;
                        foreach (var EpItem in EpItems)
                        {
                            string ViewId = EpItem.Overview;
                            EpItem.Overview = "";
                            if (!string.IsNullOrEmpty(EpItem.Id))
                            {
                                QueryService<Item> _Items = new QueryService<Item>(dataserviceFactory.getServiceContext);
                                Item = _Items.ExecuteIdsQuery("select * from Item where Id ='" + EpItem.Id + "'").FirstOrDefault<Item>();
                            }
                            if (string.IsNullOrEmpty(EpItem.Id) || Item == null)
                            {
                                string EXISTING_CUSTOMER_QUERY = string.Format("select * from Item where Name ='" + EpItem.Name + "'");
                                QueryService<Item> queryService = new QueryService<Item>(dataserviceFactory.getServiceContext);
                                Item IsItemExists = queryService.ExecuteIdsQuery(EXISTING_CUSTOMER_QUERY).FirstOrDefault<Item>();
                                if (IsItemExists == null)
                                {
                                    if(Items.Where(s=>s.Name== EpItem.Name).FirstOrDefault()!=null)
                                    {
                                        Item.Id = Items.Where(s => s.Name == EpItem.Name).FirstOrDefault().Id;
                                        Item.Name = EpItem.Name;
                                        soItems.SOItems.Where(w => w.Item == EpItem.Name).ToList().ForEach(s => s.AcctItemID = Item.Id);
                                        continue;
                                    }
                                    //EpItem.IncomeAccountRef.Value = Accounts.Where(x => x.AccountType == AccountTypeEnum.Income && x.Name == "Sales of Product Income").FirstOrDefault().Id;
                                    //EpItem.AssetAccountRef.Value = Accounts.Where(x => x.AccountType == AccountTypeEnum.OtherCurrentAsset).FirstOrDefault().Id;
                                    //EpItem.ExpenseAccountRef.Value = Accounts.Where(x => x.AccountType == AccountTypeEnum.CostofGoodsSold).FirstOrDefault().Id;
                                    Account income = CheckAddGetAccount(Accounts, AccountTypeEnum.Income, "Sales of Product Income");
                                    if (Accounts.Where(s => s.Id == income.Id).FirstOrDefault() == null)
                                    {
                                        Accounts.Add(income);
                                    }
                                    Account asset = CheckAddGetAccount(Accounts, AccountTypeEnum.OtherCurrentAsset, "Inventory Asset");
                                    if (Accounts.Where(s => s.Id == asset.Id).FirstOrDefault() == null)
                                    {
                                        Accounts.Add(asset);
                                    }
                                    Account costgoods = CheckAddGetAccount(Accounts, AccountTypeEnum.CostofGoodsSold, "Cost of Goods Sold");
                                    if (Accounts.Where(s => s.Id == costgoods.Id).FirstOrDefault() == null)
                                    {
                                        Accounts.Add(costgoods);
                                    }
                                    EpItem.IncomeAccountRef.Value = income.Id;
                                    EpItem.AssetAccountRef.Value = asset.Id;
                                    EpItem.ExpenseAccountRef.Value = costgoods.Id;
                                    Item = dataService.Add<Item>(EpItem);
                                    soItems.SOItems.Where(w => w.Item == EpItem.Name).ToList().ForEach(s => s.AcctItemID = Item.Id);
                                    Item.Name = EpItem.Name;
                                    string Sku = EpItem.Sku;
                                    GetQbItemsBySoID_Result soitem = soItems.SOItems.Where(w => w.Item == EpItem.Name && (w.Category == null || w.Category.Trim().ToLower() != "labor")).FirstOrDefault();
                                    if (soitem != null)
                                    {
                                        var q = db.CoData.Where(p => p.MasterItemID == soitem.ViewID).FirstOrDefault();
                                        if (q != null)
                                        {
                                            q.AcctItemID = Item.Id;
                                            db.SaveChanges();
                                        }
                                    }
                                }
                                else
                                {
                                    Item = IsItemExists;
                                    soItems.SOItems.Where(w => w.Item == EpItem.Name).ToList().ForEach(s => s.AcctItemID = Item.Id);
                                }
                            }
                            Item.Overview = ViewId;
                            Items.Add(Item);
                        }
                        if (Items.Count > 0)
                        {
                            //var act = Accounts.Where(x => x.AccountType == AccountTypeEnum.AccountsPayable);
                            var _line = new Line[Items.Count];
                            int count = 0;
                            decimal totalprice = 0;
                            foreach (var item in Items)
                            {
                                decimal qty = 1;
                                decimal amt = 10;
                                GetQbItemsBySoID_Result item_res = soItems.SOItems.Where(s => s.AcctItemID == item.Id && s.ViewID.ToString() == item.Overview).FirstOrDefault();
                                if (item_res != null)
                                {
                                    qty = item_res.Qty == null ? 1 : item_res.Qty.Value;
                                    amt = item_res.UnitPrice == null ? 10 : item_res.UnitPrice.Value;
                                }
                                var anyIntuitObject = new SalesItemLineDetail
                                {
                                    ItemRef = new ReferenceType { Value = item.Id },
                                    Qty = qty,
                                    QtySpecified = true
                                };

                                Line l = new Line
                                {
                                    Amount = amt * qty,
                                    AmountSpecified = true,
                                    Description = item_res.PurchaseDesc == null ? "" : item_res.PurchaseDesc,
                                    AnyIntuitObject = anyIntuitObject,
                                    DetailType = LineDetailTypeEnum.SalesItemLineDetail,
                                    DetailTypeSpecified = true,
                                };
                                _line[count] = l;
                                totalprice += (qty * amt);
                                count++;
                            }
                            if (EstInvType == "Invoice")
                            {
                                Invoice order = new Invoice();
                                order.DocNumber = SOid.ToString();
                                order.CustomerRef = new ReferenceType { Value = jobid };
                                order.Line = _line;
                                order.TotalAmt = totalprice;
                                order.TotalAmtSpecified = true;
                                var _entity = dataService.Add<Invoice>(order);
                                DBSO.AcctSoID = _entity.Id;
                                db.SaveChanges();
                            }
                            else
                            {
                                Estimate order = new Estimate();
                                order.DocNumber = SOid.ToString();
                                order.CustomerRef = new ReferenceType { Value = jobid };
                                order.Line = _line;
                                order.TotalAmt = totalprice;
                                order.TotalAmtSpecified = true;
                                var _entity = dataService.Add<Estimate>(order);
                                DBSO.AcctSoID = _entity.Id;
                                db.SaveChanges();
                            }
                        }
                    }

                }
            }
            catch (Exception)
            {

                throw;
            }
            return syncObjects;
        }
        private Account createAccount(QBSyncdto syncObjects)
        {
            Account entity = dataService.Add<Account>(syncObjects.account);
            syncObjects.account = entity;
            return entity;
            // syncObjects.IsItemSync = true;
            //  string sku = syncObjects.ItemList[i].Sku;
            //var q = db.CoData.Where(p => p.SKU == sku).FirstOrDefault();           
            //q.AcctItemID = entity.Id;
            //db.SaveChanges();
        }
        private Account CheckAddGetAccount(List<Account> Accounts, AccountTypeEnum type, string ActName="")
        {
            if(Accounts.Count==0)
            {
                QueryService<Account> _Accounts = new QueryService<Account>(dataserviceFactory.getServiceContext);
                Accounts = _Accounts.ExecuteIdsQuery("select * from Account").ToList<Account>();
            }
            Account entity = null;
            if (ActName.Trim() != "")
            {
                entity = Accounts.Where(x => x.AccountType == type && x.Name == ActName).FirstOrDefault();
            }
            else
            {
                entity = Accounts.Where(x => x.AccountType == type).FirstOrDefault();
            }
            if (entity == null)
            {
                Account act1 = new Account();
                if (ActName != "")
                {
                    Random rn = new Random();
                    act1.Name = ActName;
                    //act1.AccountSubType = ActName + " " + rn.Next().ToString();
                }
                else
                {
                    Random rn = new Random();
                    ActName = type.ToString() + " " + rn.Next().ToString();
                    act1.Name = ActName;
                    //act1.AccountSubType = type.ToString() + " " + rn.Next().ToString();
                }
                act1.AccountType = type;
                if (type == AccountTypeEnum.OtherCurrentAsset)
                {
                    act1.AccountSubType = AccountSubTypeEnum.Inventory.ToString();
                }
                else if(type == AccountTypeEnum.CostofGoodsSold)
                {
                    act1.AccountSubType = AccountSubTypeEnum.SuppliesMaterialsCogs.ToString();
                }
                else if(type == AccountTypeEnum.Income)
                {
                    act1.AccountSubType = AccountSubTypeEnum.SalesOfProductIncome.ToString();
                }
                act1.Active = true;
                //Set if Active Specified
                act1.ActiveSpecified = true;
                //Set if Account Type Specified
                act1.AccountTypeSpecified = true;
                entity = dataService.Add<Account>(act1);
                //syncObjects.account = entity;
                entity.AccountType = type;
                entity.Name = ActName;
            }
            return entity;
        }
       

    }

    public class QBAndDb_POR_Items
    {
        public List<Item> QbItems { get; set; }
        public List<GetQbPoItemsByPorID_Result> PORItems { get; set; }
    }

    public class QBAndDb_SO_Items
    {
        public List<Item> QbItems { get; set; }
        public List<GetQbItemsBySoID_Result> SOItems { get; set; }
    }
}