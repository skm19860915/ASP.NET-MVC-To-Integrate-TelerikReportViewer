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
    
    public partial class ActivitiesEvents
    {
        public int EventID { get; set; }
        public string EventName { get; set; }
        public Nullable<int> SiteUserID { get; set; }
        public Nullable<int> ProjectID { get; set; }
        public Nullable<int> StatusID { get; set; }
        public Nullable<System.DateTime> EventDate { get; set; }
        public decimal EventHours { get; set; }
        public bool AllDay { get; set; }
        public string Notes { get; set; }
        public string AcctTaskID { get; set; }
        public string RecurrenceRule { get; set; }
        public Nullable<int> SiteCoID { get; set; }
        public Nullable<System.TimeSpan> EventTime { get; set; }
    }
}
