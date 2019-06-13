using ePonti.BOL.Repository;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Web.Configuration;
using System.Web.Script.Serialization;

namespace ePonti.web.Models
{
    public class STServices
    {
        CommonRepository repo = new CommonRepository();
        int UserId = 0;
        string STAccessToken = string.Empty;
        JavaScriptSerializer js = new JavaScriptSerializer();
        /// <summary>
        /// Default Constructor.
        /// </summary>
        public STServices()
        {
        }
        public STServices(int userId)
        {
            this.UserId = userId;
            this.STAccessToken = repo.GetAccessTokenByUserId(UserId);
        }
        public string RegisterUser(string JSON)
        {
            string responseData = string.Empty;
            string requestURL = EndPointURL.RegisterUser;
            HttpWebResponse response = MakeRequest(requestURL, Method.POST, JSON);
            if (response != null && response.StatusCode == HttpStatusCode.OK)
            {
                string contentType = response.ContentType;
                Stream content = response.GetResponseStream();
                if (content != null)
                {
                    StreamReader contentReader = new StreamReader(content);
                    responseData = (contentReader.ReadToEnd());
                }
            }
            return responseData;
        }
        public string GetAuthorizationKey(string JSON)
        {
            string responseData = string.Empty;
            string requestURL = EndPointURL.AUTHENTICATE;
            HttpWebResponse response = MakeRequest(requestURL, Method.POST, JSON);
            if (response != null && response.StatusCode == HttpStatusCode.OK)
            {
                string contentType = response.ContentType;
                Stream content = response.GetResponseStream();
                if (content != null)
                {
                    StreamReader contentReader = new StreamReader(content);
                    responseData = (contentReader.ReadToEnd());
                }
            }
            return responseData;
        }
        public string GetClientSecret(string MasterKey)
        {
            string responseData = string.Empty;
            string requestURL = EndPointURL.CLIENT_SECRET + MasterKey;
            HttpWebResponse response = MakeRequest(requestURL, Method.GET, string.Empty);
            if (response != null && response.StatusCode == HttpStatusCode.OK)
            {
                string contentType = response.ContentType;
                Stream content = response.GetResponseStream();
                if (content != null)
                {
                    StreamReader contentReader = new StreamReader(content);
                    responseData = (contentReader.ReadToEnd());
                }
            }
            return responseData;
        }
        public Token GetAccessToken(string JSON)
        {
            string responseData = string.Empty;
            string requestURL = EndPointURL.EXCHANGE;
            Token token = null;

            HttpWebResponse response = MakeRequest(requestURL, Method.POST, JSON);
            if (response != null && response.StatusCode == HttpStatusCode.OK)
            {
                string contentType = response.ContentType;
                Stream content = response.GetResponseStream();
                if (content != null)
                {
                    StreamReader contentReader = new StreamReader(content);
                    string JSONData = (contentReader.ReadToEnd());
                    token = js.Deserialize<Token>(JSONData);
                }
            }
            return token;
        }
        private void RefreshToken(object exchangeData)
        {
            string JSON = JsonConvert.SerializeObject(exchangeData);
            string responseData = string.Empty;
            string requestURL = EndPointURL.REFRESH_TOKEN;
            Token token = null;
            HttpWebResponse response = MakeRequest(requestURL, Method.POST, JSON);
            if (response != null && response.StatusCode == HttpStatusCode.OK)
            {
                string contentType = response.ContentType;
                Stream content = response.GetResponseStream();
                if (content != null)
                {
                    StreamReader contentReader = new StreamReader(content);
                    string JSONData = (contentReader.ReadToEnd());
                    token = js.Deserialize<Token>(JSONData);
                    STAccessToken = token.AccessToken;
                    repo.UpdateAccessToken(UserId, token.AccessToken, token.RefreshToken);
                }
            }
        }

        #region MakeRequest
        private HttpWebResponse MakeRequest(string requestURL, string method, string JSON, string header = null)
        {
            HttpWebResponse response = null;
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(requestURL);
                request.Method = method;
                if (header != null)
                {
                    request.Headers.Add("Authorization", "Bearer " + header);
                }
                if (method.Equals(Method.POST))
                {
                    //StringBuilder postData = new StringBuilder();
                    //postData.Append("{}");
                    //ASCIIEncoding ascii = new ASCIIEncoding();
                    //byte[] postBytes = ascii.GetBytes(JSON);
                    request.ContentType = "application/json";

                    // Add post data to request
                    using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                    {
                        streamWriter.Write(JSON);
                        streamWriter.Flush();
                        streamWriter.Close();
                    }
                    //request.ContentLength = postBytes.Length;
                    //Stream postStream = request.GetRequestStream();
                    //postStream.Write(postBytes, 0, postBytes.Length);
                    //postStream.Flush();
                    //postStream.Close();
                }
                response = (HttpWebResponse)request.GetResponse();
            }
            catch (WebException ex)
            {
                response = (HttpWebResponse)ex.Response;
                //throw;
            }
            return response;
        }


        private string MakeSecureRequest(string requestUrl, string data, bool isfirst=true)
        {
            string responseData = string.Empty;
            string requestURL = requestUrl + data;
            HttpWebResponse response = MakeRequest(requestURL, Method.GET, string.Empty, STAccessToken);
            if (response != null && response.StatusCode == HttpStatusCode.OK)
            {
                string contentType = response.ContentType;
                Stream content = response.GetResponseStream();
                if (content != null)
                {
                    StreamReader contentReader = new StreamReader(content);
                    responseData = (contentReader.ReadToEnd());
                }
            }
            if (response != null && response.StatusCode == HttpStatusCode.NotAcceptable)
            {
                if (isfirst)
                {
                    string masterKey = WebConfigurationManager.AppSettings["SalezToolzMasterKey"];
                    string ClientSecret = WebConfigurationManager.AppSettings["SalezToolzClientSecret"];  // GetClientSecret(masterKey);
                    string refreshToken = repo.GetRefreshTokenByUserId(UserId);// Get Refresh token. 
                    var refreshData = new
                    {
                        refreshToken = refreshToken,
                        masterKey = masterKey,
                        clientSecret = ClientSecret
                    };
                    RefreshToken(refreshData);
                    responseData = MakeSecureRequest(requestUrl, data, false);
                }
            }
            return responseData;
        }
        #endregion MakeRequest


        #region Fetch Salez Toolz Data

        public List<STHostModel> GetHosts(string license)
        {
            string responseData = string.Empty;
            responseData = MakeSecureRequest(EndPointURL.HOSTS, license);
            List<STHostModel> hostList = js.Deserialize<List<STHostModel>>(responseData);
            return hostList;
        }

        public List<STSessionModel> GetSessions()
        {
            string responseData = string.Empty;
            responseData = MakeSecureRequest(EndPointURL.SESSIONS, "");
            List<STSessionModel> hostList = js.Deserialize<List<STSessionModel>>(responseData);
            return hostList;
        }
        public SessionInfoModel GetSessionInfo(string fileid)
        {
            string responseData = string.Empty;
            responseData = MakeSecureRequest(EndPointURL.SESSIONINFO, fileid);
            SessionInfoModel hostList = js.Deserialize<SessionInfoModel>(responseData);
            return hostList;
        }

        public List<STFolderModel> GetFolders(string hostId)
        {
            string responseData = string.Empty;
            responseData = MakeSecureRequest(EndPointURL.FOLDERS, hostId);
            List<STFolderModel> folderList = js.Deserialize<List<STFolderModel>>(responseData);
            return folderList;
        }

        public List<STFileModel> GetFileListing(string folderId)
        {
            string responseData = string.Empty;
            responseData = MakeSecureRequest(EndPointURL.FILE_LISTING, folderId);
            List<STFileModel> fileList = js.Deserialize<List<STFileModel>>(responseData);
            return fileList;
        }

        public STClientModel GetClientInfo(string fileId)
        {
            string responseData = string.Empty;
            responseData = MakeSecureRequest(EndPointURL.CLIENT_INFO, fileId);
            STClientModel client = js.Deserialize<STClientModel>(responseData);
            return client;
        }

        public STAdjustmentModel GetAdjustmemt(string fileId)
        {
            string responseData = string.Empty;
            responseData = MakeSecureRequest(EndPointURL.ADJUSTMEMT, fileId);
            STAdjustmentModel adjustment = js.Deserialize<STAdjustmentModel>(responseData);
            return adjustment;
        }

        public STTaxProfileModel GetTaxProfile(string fileId)
        {
            string responseData = string.Empty;
            responseData = MakeSecureRequest(EndPointURL.TAX_PROFILE, fileId);
            STTaxProfileModel taxProfile = js.Deserialize<STTaxProfileModel>(responseData);
            return taxProfile;
        }

        public List<STScheduleModel> GetScheduleProfiles(string fileId)
        {
            string responseData = string.Empty;
            responseData = MakeSecureRequest(EndPointURL.SCHEDULE_PROFILES, fileId);
            List<STScheduleModel> scheduleList = js.Deserialize<List<STScheduleModel>>(responseData);
            return scheduleList;
        }

        public List<STPackageModel> GetSelectedPackages(string fileId)
        {
            string responseData = string.Empty;
            responseData = MakeSecureRequest(EndPointURL.SELECTED_PACKAGES, fileId);
            List<STPackageModel> packageList = js.Deserialize<List<STPackageModel>>(responseData);
            return packageList;
        }

        public List<string> ListPackageGroupsInAFile(string fileId)
        {
            string responseData = string.Empty;
            responseData = MakeSecureRequest(EndPointURL.PACKAGE_GROUPS__IN_FILE, fileId);
            List<string> packageGroupList = js.Deserialize<List<string>>(responseData);
            return packageGroupList;
        }

        public STPackageGroupsModel GetPackageGroup(string fileId)
        {
            string responseData = string.Empty;
            responseData = MakeSecureRequest(EndPointURL.PACKAGE_GROUP, fileId);
            STPackageGroupsModel packageGroup = js.Deserialize<STPackageGroupsModel>(responseData);
            return packageGroup;
        }

        public List<string> ListPackagesInAPackageGroup(string fileId)
        {
            string responseData = string.Empty;
            responseData = MakeSecureRequest(EndPointURL.PACKAGES_IN_PACKAGE_GROUP, fileId);
            List<string> packageList = js.Deserialize<List<string>>(responseData);
            return packageList;
        }

        public STPackageContentsModel GetPackageContents(string fileId)
        {
            string responseData = string.Empty;
            responseData = MakeSecureRequest(EndPointURL.PACKAGE_CONTENTS, fileId);
            STPackageContentsModel packageContent = js.Deserialize<STPackageContentsModel>(responseData);
            return packageContent;
        }

        public STCommentsModel GetComments(string fileId)
        {
            string responseData = string.Empty;
            responseData = MakeSecureRequest(EndPointURL.COMMENTS, fileId);
            STCommentsModel comments = js.Deserialize<STCommentsModel>(responseData);
            return comments;
        }

        public string GetFileJson(string fileId)
        {
            string responseData = string.Empty;
            responseData = MakeSecureRequest(EndPointURL.FILE_JSON, fileId);
            return responseData;
        }

        public STJobInfoModel GetJobInfo(string fileId)
        {
            string responseData = string.Empty;
            responseData = MakeSecureRequest(EndPointURL.JOB_INFO, fileId);
            STJobInfoModel jobInfo = js.Deserialize<STJobInfoModel>(responseData);
            return jobInfo;
        }

        #endregion Fetch Salez Toolz Data

        public bool IsConnected(int userId)
        {
            var data = repo.GetCoSalezToolzData(userId);
            if (data == null)
            {
                return false;
            }
            return true;
        }

    } // STServices

    public class Method
    {
        public static string GET = "GET";
        public static string POST = "POST";
        public static string PUT = "PUT";
        public static string DELETE = "DELETE";
    }
    public class EndPointURL
    {
        private static readonly string _baseURL = "https://integration.thematrixexchange.com";

        #region Public
        public static readonly string RegisterUser = _baseURL + "/api/public/registerUser";
        public static readonly string AUTHENTICATE = _baseURL + "/api/public/authenticate";
        public static readonly string CLIENT_SECRET = _baseURL + "/api/public/getClientSecret?masterKey=";
        public static readonly string EXCHANGE = _baseURL + "/api/public/exchange";
        public static readonly string REFRESH_TOKEN = _baseURL + "/api/public/refresh";
        #endregion

        #region Secure
        public static readonly string SESSIONS = _baseURL + "/api/secure/sessionsList";
        public static readonly string SESSIONINFO = _baseURL + "/api/secure/session?sessionId=";
        public static readonly string HOSTS = _baseURL + "/api/secure/hosts?license=";
        public static readonly string FOLDERS = _baseURL + "/api/secure/folders?hostId=";
        public static readonly string FILE_LISTING = _baseURL + "/api/secure/files?folderId=";
        public static readonly string CLIENT_INFO = _baseURL + "/api/secure/clientInfo?fileId=";
        public static readonly string ADJUSTMEMT = _baseURL + "/api/secure/adjustment?fileId=";
        public static readonly string TAX_PROFILE = _baseURL + "/api/secure/taxProfile?fileId=";
        public static readonly string SCHEDULE_PROFILES = _baseURL + "/api/secure/scheduleProfiles?fileId=";
        public static readonly string SELECTED_PACKAGES = _baseURL + "/api/secure/selectedPackages?fileId=";
        public static readonly string PACKAGE_GROUPS__IN_FILE = _baseURL + "/api/secure/packageGroupNames?fileId=";
        public static readonly string PACKAGE_GROUP = _baseURL + "/api/secure/packageGroup?packageGroupName=";
        public static readonly string PACKAGES_IN_PACKAGE_GROUP = _baseURL + "/api/secure/packageNames?packageGroupName=";
        public static readonly string PACKAGE_CONTENTS = _baseURL + "/api/secure/package?packageName=";
        public static readonly string COMMENTS = _baseURL + "/api/secure/comments?fileId=";
        public static readonly string FILE_JSON = _baseURL + "/api/secure/file?fileId=";
        public static readonly string JOB_INFO = _baseURL + "/api/secure/jobInfo?fileId=";
        #endregion
    }
    public class Token
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
    public class RequestData
    {
        public string masterKey { get; set; }
        public string clientSecret { get; set; }
        public string authorizationKey { get; set; }
    }
}