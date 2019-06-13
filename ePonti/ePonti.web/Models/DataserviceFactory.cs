using Intuit.Ipp.Core;
using Intuit.Ipp.DataService;
using Intuit.Ipp.Security;
using System;
using System.Configuration;


namespace ePonti.web.Models
{
    public class DataserviceFactory
    {
        private OAuthRequestValidator oAuthRequestValidator = null;
        private DataService dataService = null;
        IntuitServicesType intuitServicesType = new IntuitServicesType();
        private ServiceContext serviceContext = null;
        public ServiceContext getServiceContext { get; set; }
        /// <summary>
        /// allocate memory for service context objects
        /// </summary>
        /// <param name="oAuthorization"></param>
        public DataserviceFactory(QBAuthorizationdto oAuthorization)
        {
            try
            {
                oAuthRequestValidator = new OAuthRequestValidator(
                oAuthorization.AccessToken,
                oAuthorization.AccessTokenSecret,
                oAuthorization.ConsumerKey,
                oAuthorization.ConsumerSecret);
                intuitServicesType = oAuthorization.DataSource == "QBO" ? IntuitServicesType.QBO : IntuitServicesType.None;
                serviceContext = new ServiceContext(oAuthorization.Realmid.ToString(), intuitServicesType, oAuthRequestValidator);
                serviceContext.IppConfiguration.BaseUrl.Qbo = ConfigurationManager.AppSettings["ServiceContext.BaseUrl.Qbo"];
                serviceContext.IppConfiguration.MinorVersion.Qbo = "4";
                //serviceContext.IppConfiguration.Logger.RequestLog.EnableRequestResponseLogging = true;
                // serviceContext.IppConfiguration.Logger.RequestLog.ServiceRequestLoggingLocation = ConfigurationManager.AppSettings["ServiceRequestLoggingLocation"];
                serviceContext.RequestId = GetGuid();               
                getServiceContext = serviceContext;
                dataService = new DataService(serviceContext);
            }
            catch (Intuit.Ipp.Exception.FaultException ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// return the current data service
        /// </summary>
        /// <returns></returns>
        public DataService getDataService()
        {
            return dataService;
        }
        internal static String GetGuid()
        {
            return Guid.NewGuid().ToString("N");
        }
        internal static String getVersion()
        {
            return "4";
        }
    }
}