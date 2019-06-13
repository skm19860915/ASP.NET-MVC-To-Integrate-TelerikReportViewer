using DevDefined.OAuth.Consumer;
using DevDefined.OAuth.Framework;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace ePonti.web.Models
{
    public class QBAuthorizationdto
    {
            #region <<DTO>>
            public string ResponseFormat = "{0}?oauth_token={1}&oauth_callback={2}";
            public int Id { get; set; }
            public string OauthLink
            {
                get
                {
                    return ConfigurationManager.AppSettings["OauthLink"];
                }
            }
            public string AuthorizeUrl
            {
                get
                {
                    return ConfigurationManager.AppSettings["AuthorizeUrl"];
                }
            }
            public string GET_REQUEST_TOKEN
            {
                get
                {
                    return ConfigurationManager.AppSettings["GET_REQUEST_TOKEN"];
                }
            }
            public string GET_ACCESS_TOKEN
            {
                get
                {
                    return ConfigurationManager.AppSettings["GET_ACCESS_TOKEN"];
                }
            }
            public string ResponseLink { get; set; }
            public string CallBackUrl { get; set; }
            public string TokenSecret { get; set; }

            public IToken Token { get; set; }
            public IOAuthSession OAuthSession
            {
                get
                {
                    OAuthConsumerContext oAuthConsumerContext = new OAuthConsumerContext
                    {
                        ConsumerKey = ConsumerKey,
                        ConsumerSecret = ConsumerSecret,
                        SignatureMethod = SignatureMethod.HmacSha1
                    };
                    return new OAuthSession(oAuthConsumerContext,
                                                GET_REQUEST_TOKEN,
                                                OauthLink,
                                               GET_ACCESS_TOKEN);
                }
            }
            public string OauthVerifyer { get; set; }
            public string Realmid { get; set; }
            public string DataSource { get; set; }
            public string AccessToken { get; set; }
            public string AccessTokenSecret { get; set; }
            public string SecurityKey
            {
                get
                {
                    return ConfigurationManager.AppSettings["securityKey"];
                }
            }
            public string ConsumerKey
            {
                get
                {
                    return ConfigurationManager.AppSettings["QBAppKey"];
                }
            }

            public string ConsumerSecret
            {
                get
                {
                    return ConfigurationManager.AppSettings["QBAppSecret"];
                }
            }
            public bool IsConnected { get; set; }
            #endregion
            //
            #region <<Functions>>
            #endregion
        }

    }