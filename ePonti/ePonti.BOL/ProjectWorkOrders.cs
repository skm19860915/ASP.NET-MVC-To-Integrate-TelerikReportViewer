//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ePonti.BOL
{
    using System;
    using System.Collections.Generic;
    
    public partial class ProjectWorkOrders
    {
        public int WoID { get; set; }
        public Nullable<int> SiteCoID { get; set; }
        public Nullable<int> ProjectID { get; set; }
        public Nullable<System.DateTime> DateCreated { get; set; }
        public Nullable<int> WoTypeID { get; set; }
        public Nullable<int> ScheduleID { get; set; }
        public string WoNumber { get; set; }
        public Nullable<System.DateTime> WoDate { get; set; }
        public Nullable<System.TimeSpan> WoTime { get; set; }
        public Nullable<decimal> AllottedTime { get; set; }
        public Nullable<int> WoStatusID { get; set; }
        public string WoNote { get; set; }
        public Nullable<int> SiteUserID { get; set; }
        public Nullable<System.DateTime> ActualDate { get; set; }
        public Nullable<System.TimeSpan> ActualTime { get; set; }
        public Nullable<System.TimeSpan> ArrivalTime { get; set; }
        public Nullable<decimal> ActualHours { get; set; }
        public Nullable<int> ProjectCostCodeID { get; set; }
        public Nullable<int> ProjectPayTypeID { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public Nullable<System.TimeSpan> EndTime { get; set; }
        public Nullable<bool> IncludeMiscHours { get; set; }
        public Nullable<int> ApprovalUserID { get; set; }
        public Nullable<int> PayrollStatusID { get; set; }
        public Nullable<decimal> PercentCompleted { get; set; }
        public string WoConcern { get; set; }
        public string AcctWoID { get; set; }
    }
}
