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
    
    public partial class ActivitiesCalls
    {
        public int CallID { get; set; }
        public int SiteUserID { get; set; }
        public Nullable<int> ProjectID { get; set; }
        public string Subject { get; set; }
        public Nullable<int> ContactID { get; set; }
        public Nullable<int> CallPurposeID { get; set; }
        public Nullable<int> StatusID { get; set; }
        public string Notes { get; set; }
        public int SiteCoID { get; set; }
        public Nullable<System.DateTime> CallDate { get; set; }
        public Nullable<System.TimeSpan> CallTime { get; set; }
        public Nullable<decimal> CallDuration { get; set; }
    
        public virtual CoActivitiesStatus CoActivitiesStatus { get; set; }
        public virtual CoCallPurposes CoCallPurposes { get; set; }
        public virtual CoContacts CoContacts { get; set; }
        public virtual ProjectInfo ProjectInfo { get; set; }
        public virtual SiteCompanies SiteCompanies { get; set; }
        public virtual SiteUsers SiteUsers { get; set; }
    }
}