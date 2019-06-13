using ePonti.BLL.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ePonti.web.Common
{
    public static class NavigationHelper
    {
        private static readonly string itemSeparator = "-";

        private static readonly List<Tuple<EnumWrapper.Pages, string>> navMapper = new List<Tuple<EnumWrapper.Pages, string>>();
        //    new List<Tuple<EnumWrapper.Pages, string>>() {
        //    Tuple.Create(EnumWrapper.Pages.PeopleGrid,"pg"),
        //    Tuple.Create(EnumWrapper.Pages.ContactDetails,"cd"),

        //};

        static NavigationHelper()
        {
            var allPages = Enum.GetValues(typeof(EnumWrapper.Pages)).Cast<EnumWrapper.Pages>();
            foreach (var page in allPages)
            {
                navMapper.Add(Tuple.Create(page, page.GetDescription()));
            }
        }

        public class NavData
        {
            public int? PageID { get; set; }
            public EnumWrapper.Pages Page { get; set; }
            public string Nav { get; set; }
        }

        public static string NextNav(string CurrentNav, EnumWrapper.Pages CurrentPage, int? CurrentID)
        {
            var nav = CurrentNav ?? string.Empty;
            if (!string.IsNullOrWhiteSpace(nav))
            {
                nav += itemSeparator;
            }
            nav += navMapper.Where(p => p.Item1 == CurrentPage).Select(p => p.Item2).FirstOrDefault() + (CurrentID.HasValue ? CurrentID.Value.ToString() : "");

            return nav;
        }

        public static NavData PrevNav(string CurrentNav)
        {
            var nav = CurrentNav;
            if (string.IsNullOrWhiteSpace(nav))
            {
                return null;
            }
            nav = nav.Trim(new char[] { ' ', ',' });

            var prevPageData = nav.Substring(nav.LastIndexOf(itemSeparator) + 1);

            var prevPageShortName = prevPageData.Substring(0, 2);
            var prevPage = navMapper.Where(p => p.Item2 == prevPageShortName).Select(p => p.Item1).FirstOrDefault();

            int? prevPageId = null;
            if (prevPageData.Length > 2)
            {
                int temp = 0;
                var prevPageIdString = prevPageData.Substring(2);
                if (int.TryParse(prevPageIdString, out temp))
                {
                    prevPageId = temp;
                }
            }

            var prevNav = nav.Substring(0, Math.Max(nav.LastIndexOf(itemSeparator), 0));

            return new NavData()
            {
                PageID = prevPageId,
                Page = prevPage,
                Nav = prevNav
            };
        }

        //private static readonly string NavCookie = "ePonti_nav";
        //private static readonly string urlSeparator = "**||**";

        //public static void PushUrl(string Url)
        //{
        //    var cookie = HttpContext.Current.Request.Cookies[NavCookie];
        //    var urls = "";

        //    if (cookie != null)
        //    {
        //        urls = cookie.Value;
        //    }
        //    else
        //    {
        //        cookie = new HttpCookie(NavCookie);
        //    }

        //    urls += Url + urlSeparator;
        //    cookie.Value = urls;
        //    cookie.Expires = DateTime.Now.AddDays(1);

        //    HttpContext.Current.Response.Cookies.Add(cookie);
        //}

        //public static string PopUrl()
        //{
        //    var cookie = HttpContext.Current.Request.Cookies[NavCookie];
        //    var urls = "";

        //    if (cookie != null)
        //    {
        //        urls = cookie.Value;
        //    }
        //    else
        //    {
        //        return "";
        //    }

        //    var urlList = urls.Split(new[] { urlSeparator }, StringSplitOptions.RemoveEmptyEntries).ToList();
        //    if (urlList.Count == 0)
        //    {
        //        return "";
        //    }

        //    var previousUrl = urlList.LastOrDefault();
        //    urlList.RemoveAt(urlList.Count - 1);

        //    //if last several urls are same, then ignore all but one
        //    if (previousUrl == urlList.LastOrDefault())
        //    {
        //        //return PopUrl();
        //        return previousUrl;
        //    }
        //    else
        //    {
        //        cookie.Value = urls;
        //        cookie.Expires = DateTime.Now.AddDays(1);
        //        HttpContext.Current.Response.Cookies.Add(cookie);
        //        return previousUrl;
        //    }


        //}
    }




}