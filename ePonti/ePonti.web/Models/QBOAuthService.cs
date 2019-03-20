using DevDefined.OAuth.Framework;
using ePonti.BOL;
using ePonti.web.Areas.Options.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace ePonti.web.Models
{
   
    public class QBOAuthService
    {
        private QBAuthorizationdto qbAuthorizationdto = null;
        Controller integrationsOptionsController = null;
        Dictionary<string, QBAuthorizationdto> oAuthRepo = null;
        public QBOAuthService(QBAuthorizationdto oAuthDto)
        {
            qbAuthorizationdto = oAuthDto;
        }
        public string GrantUrl(object integratedoptionsController)
        {
            try
            {
                qbAuthorizationdto.Token = qbAuthorizationdto.OAuthSession.GetRequestToken();
                qbAuthorizationdto.ResponseLink = string.Format(qbAuthorizationdto.ResponseFormat,
                    qbAuthorizationdto.AuthorizeUrl,
                    qbAuthorizationdto.Token.Token,
                    UriUtility.UrlEncode(qbAuthorizationdto.CallBackUrl));
                Save(integratedoptionsController, qbAuthorizationdto);
                return qbAuthorizationdto.ResponseLink;
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
        }      
        internal QBAuthorizationdto GetAccessTokenFromServer(object IntegratedoptionsController, QBAuthorizationdto qbAuthorizationdto)
        {
            try
            {
                IToken accessToken = qbAuthorizationdto.OAuthSession.ExchangeRequestTokenForAccessToken(qbAuthorizationdto.Token, qbAuthorizationdto.OauthVerifyer);
                qbAuthorizationdto.AccessToken = accessToken.Token;
                qbAuthorizationdto.AccessTokenSecret = accessToken.TokenSecret;
                qbAuthorizationdto.IsConnected = true;
                return qbAuthorizationdto;
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
        }
        
        internal QBAuthorizationdto GetRequestToken(object controller)
        {
            integrationsOptionsController = controller as System.Web.Mvc.Controller;
            string secretKey = integrationsOptionsController.TempData["secretKey"] as string;
            Dictionary<string, QBAuthorizationdto> oAuthRepo = integrationsOptionsController.TempData["OAuthorization"] as Dictionary<string,QBAuthorizationdto>;
            integrationsOptionsController.TempData.Keep();
            return oAuthRepo[secretKey];
        }
        internal QBAuthorizationdto GetAccessToken(object controller)
        {
            integrationsOptionsController = controller as System.Web.Mvc.Controller;
            string secretKey = integrationsOptionsController.TempData["secretKey"] as string;
            Dictionary<string, QBAuthorizationdto> oAuthRepo = integrationsOptionsController.TempData["OAuthorization"] as Dictionary<string, QBAuthorizationdto>;
            integrationsOptionsController.TempData.Keep();
            if (secretKey != null)
                return oAuthRepo[secretKey];
            else return null;
        }
        internal bool Save(object controller, QBAuthorizationdto qbAuthorizationdto)
        {
            oAuthRepo=new Dictionary<string, QBAuthorizationdto>();
            integrationsOptionsController = controller as System.Web.Mvc.Controller;
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            Random random = new Random();
            string secretKey = new string(
                Enumerable.Repeat(chars, 8)
                          .Select(s => s[random.Next(s.Length)])
                          .ToArray());
            integrationsOptionsController.TempData["secretKey"] = secretKey;
            oAuthRepo.Add(secretKey, qbAuthorizationdto);
            integrationsOptionsController.TempData["OAuthorization"] = oAuthRepo;
            integrationsOptionsController.TempData.Keep();
            return true;
        }

        public QBAuthorizationdto IsTokenAvailable(object oauthController,int siteCoID)
        {
            var oAuthDetails = new QBAuthorizationdto();
             using (var db = new ePontiv2Entities())
            {       
                    CoQuickBooks currentIndex = db.CoQuickBooks.Where(p => p.SiteCoID == siteCoID).FirstOrDefault();
                if (currentIndex != null)
                {
                    string testAccesToken = Utility.Decrypt(Encoding.UTF8.GetString(currentIndex.AccessToken), oAuthDetails.SecurityKey);
                    oAuthDetails.AccessToken = testAccesToken;
                    oAuthDetails.AccessTokenSecret = currentIndex.apiSecret;
                    oAuthDetails.IsConnected = true;
                    oAuthDetails.DataSource = currentIndex.DataSource;
                    oAuthDetails.Realmid = currentIndex.RealmID;
                    Save(oauthController, oAuthDetails);
                }          
            }
            return oAuthDetails;
        }
    }
}