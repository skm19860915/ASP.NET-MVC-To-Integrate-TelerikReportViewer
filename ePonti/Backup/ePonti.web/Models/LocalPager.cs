using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ePonti.web.Models
{
    public class LocalPager
    {
        public int DefaultPageSize = CommonCls.PageSize;

        public LocalPager()
        {

        }
        public int PageCount { get; protected set; }
        public int ItemsCount { get; set; }
        public int CurrentPage { get; set; }
    }
}