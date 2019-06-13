using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ePonti.BOL.Models
{
   public class PayrollInfo
    {
        public string ActivityType { get; set; }
        public int ViewID { get; set; }
        public Nullable<int> UserID { get; set; }
        public Nullable<int> ProjectID { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public string Job { get; set; }
        public string Resource { get; set; }
        public string Task { get; set; }
        public Nullable<decimal> Hours { get; set; }
        public string CostCode { get; set; }
        public Nullable<int> ProjectCostCodeID { get; set; }
        public string PayType { get; set; }
        public Nullable<int> ProjectPayTypeID { get; set; }
        public Nullable<int> Reason { get; set; }
        public Nullable<int> PayrollPeriodItemID { get; set; }
        public Nullable<int> PayrollTypeID { get; set; }
    }
}
