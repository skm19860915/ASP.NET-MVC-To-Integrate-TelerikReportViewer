using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ePonti.BLL.Common;
using System.Web.Routing;
using System.Reflection;

namespace ePonti.web.Common
{
    public static class Extensions
    {

        #region View Url Helper
        //public static string ActionNext(this UrlHelper Url, string Action, string Controller = "", object RouteValues = null, EnumWrapper.Pages CurrentPage = EnumWrapper.Pages._NONE, int? CurrentID = null)
        //{
        //    if (string.IsNullOrWhiteSpace(Controller))
        //    {
        //        Controller = (Url.RequestContext.RouteData.Values["controller"] ?? "").ToString();
        //    }

        //    var dict = RouteValues.GetType()
        //                .GetProperties()
        //                .ToDictionary(p => p.Name, p => p.GetValue(RouteValues, null));

        //    var nav = Url.RequestContext.HttpContext.Request.QueryString["nav"];
        //    if (dict.ContainsKey("nav"))
        //    {
        //        //nav = (dict["nav"] != null ? dict["nav"].ToString() : "");
        //    }
        //    else
        //    {
        //        dict.Add("nav", "");
        //    }
        //    dict["nav"] = NavigationHelper.NextNav(nav, CurrentPage, CurrentID);

        //    var rv = new RouteValueDictionary(dict);
        //    var finalRes = Url.Action(Action, Controller, rv);
        //    return finalRes;
        //}

        public static string Nav(this UrlHelper Url, EnumWrapper.Pages CurrentPage = EnumWrapper.Pages._NONE, int? CurrentID = null)
        {
            var nav = Url.RequestContext.HttpContext.Request.QueryString["nav"] ?? "";
            return NavigationHelper.NextNav(nav, CurrentPage, CurrentID);
        }

        public static string ActionPrev(this UrlHelper Url)
        {
            var currentNav = Url.RequestContext.HttpContext.Request.QueryString["nav"];
            var prevNav = NavigationHelper.PrevNav(currentNav);

            if (prevNav == null)
            {
                //return some default url
                return "javascript:history.go(-1)";//Url.Action("index", "people", new { area = "sections" });
            }

            const string ac_index = "index",
                         ac_details = "details",

                         ar_sections = "sections",
                         ar_common = "common",
                         ar_pages = "pages",
                         ar_procurement = "procurement",
                         ar_mobile = "mobile";

            switch (prevNav.Page)
            {
                case EnumWrapper.Pages.PeopleGrid:
                    return Url.Action(ac_index, "people", new { area = ar_sections, nav = prevNav.Nav });
                case EnumWrapper.Pages.ContactDetails:
                    return Url.Action(ac_details, "contactinfo", new { area = ar_pages, nav = prevNav.Nav, id = prevNav.PageID });
                case EnumWrapper.Pages.CallDetails:
                    return Url.Action(ac_details, "calls", new { area = ar_common, nav = prevNav.Nav, id = prevNav.PageID });
                case EnumWrapper.Pages.EventDetails:
                    return Url.Action(ac_details, "events", new { area = ar_common, nav = prevNav.Nav, id = prevNav.PageID });
                case EnumWrapper.Pages.NoteDetails:
                    return Url.Action(ac_details, "notes", new { area = ar_common, nav = prevNav.Nav, id = prevNav.PageID });
                case EnumWrapper.Pages.CaseDetails:
                    return Url.Action(ac_details, "cases", new { area = ar_common, nav = prevNav.Nav, id = prevNav.PageID });
                case EnumWrapper.Pages.PunchItemDetails:
                    return Url.Action(ac_details, "punchlists", new { area = ar_common, nav = prevNav.Nav, id = prevNav.PageID });
                case EnumWrapper.Pages.TimeItDetails:
                    return Url.Action(ac_details, "timeits", new { area = ar_common, nav = prevNav.Nav, id = prevNav.PageID });
                case EnumWrapper.Pages.LeadGrid:
                    return Url.Action(ac_index, "leads", new { area = ar_sections, nav = prevNav.Nav });
                case EnumWrapper.Pages.LeadDetails:
                    return Url.Action(ac_details, "leadinfo", new { area = ar_pages, nav = prevNav.Nav, id = prevNav.PageID });
                case EnumWrapper.Pages.QuoteGrid:
                    return Url.Action(ac_index, "quotes", new { area = ar_sections, nav = prevNav.Nav });
                case EnumWrapper.Pages.QuoteDetails:
                    return Url.Action(ac_details, "quoteinfo", new { area = ar_pages, nav = prevNav.Nav, id = prevNav.PageID });
                case EnumWrapper.Pages.JobGrid:
                    return Url.Action(ac_index, "jobs", new { area = ar_sections, nav = prevNav.Nav });
                case EnumWrapper.Pages.JobDetails:
                    return Url.Action(ac_details, "jobinfo", new { area = ar_pages, nav = prevNav.Nav, id = prevNav.PageID });
                case EnumWrapper.Pages.ServiceGrid:
                    return Url.Action(ac_index, "service", new { area = ar_sections, nav = prevNav.Nav });
                case EnumWrapper.Pages.ServiceDetails:
                    return Url.Action(ac_details, "serviceinfo", new { area = ar_pages, nav = prevNav.Nav, id = prevNav.PageID });
                case EnumWrapper.Pages.Dashboard:
                    return Url.Action(ac_index, "dashboard", new { area = ar_sections, nav = prevNav.Nav });
                case EnumWrapper.Pages.PorGrid:
                    return Url.Action(ac_index, "procurement", new { area = ar_sections, nav = prevNav.Nav });
                case EnumWrapper.Pages.PorDetails:
                    return Url.Action(ac_details, "porinfo", new { area = ar_procurement, nav = prevNav.Nav, id = prevNav.PageID });
                case EnumWrapper.Pages.WorkOrderbDetails:
                    return Url.Action(ac_details, "workorders", new { area = ar_common, nav = prevNav.Nav, id = prevNav.PageID });
                case EnumWrapper.Pages.DeliveryRequestDetails:
                    return Url.Action(ac_details, "deliveryinfo", new { area = ar_procurement, nav = prevNav.Nav, id = prevNav.PageID });
                case EnumWrapper.Pages.CorDetails:
                    return Url.Action(ac_details, "corinfo", new { area = ar_pages, nav = prevNav.Nav, id = prevNav.PageID });
                case EnumWrapper.Pages.SoDetails:
                    return Url.Action(ac_details, "soinfo", new { area = ar_procurement, nav = prevNav.Nav, id = prevNav.PageID });
                case EnumWrapper.Pages.mCallDetails:
                    return Url.Action(ac_index, "mcall", new { area = ar_mobile, nav = prevNav.Nav });
                case EnumWrapper.Pages._NONE:
                default:
                    //return some default url
                    //return Url.Action("index", "people", new { area = "sections", nav = prevNav.Nav });
                    return "javascript:history.go(-1)";
                    //break;
            }
        }
        #endregion

        public static dynamic ToDynamic(this object value)
        {
            IDictionary<string, object> expando = new ExpandoObject();

            foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(value.GetType()))
                expando.Add(property.Name, property.GetValue(value));

            return expando as ExpandoObject;
        }

        public static string GetDescription<T>(this T source)
        {
            FieldInfo fi = source.GetType().GetField(source.ToString());

            DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(
                typeof(DescriptionAttribute), false);

            if (attributes != null && attributes.Length > 0) return attributes[0].Description;
            else return source.ToString();
        }
    }
}