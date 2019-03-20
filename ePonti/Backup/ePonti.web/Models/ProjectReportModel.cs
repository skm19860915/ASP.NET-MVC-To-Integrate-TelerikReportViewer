using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ePonti.web.Models
{
    public class ProjectReportModel
    {
        public int ViewID { get; set; }
        public string Logo { get; set; }
        public string Company { get; set; }
        public string CoAddress { get; set; }
        public string CoAddress2 { get; set; }
        public string CoPhone { get; set; }
        

        public DateTime Date { get; set; }

        public string Customer { get; set; }
        public string CustomerAddress1 { get; set; }
        public string CustomerAddress2 { get; set; }

        public string ProjectName { get; set; }
        public string ProjectAddress { get; set; }
        public string ProjectAddress2 { get; set; }

        public string Area { get; set; }
        public string AreaDescription { get; set; }

        public List<ProjectReportItem> ProjectReportItemList { get; set; }
        public int TotalRecord { get; set; }
    }
    public class ProjectReportItem
    {
        public string Division { get; set; }
        public string Item { get; set; }
        public string ItemDesription { get; set; }
        public decimal? Qty { get; set; }
        public decimal? Hrs { get; set; }
        public decimal? Price { get; set; }
    }
}