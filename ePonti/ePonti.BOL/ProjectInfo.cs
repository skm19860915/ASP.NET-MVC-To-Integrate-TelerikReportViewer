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
    
    public partial class ProjectInfo
    {
        public ProjectInfo()
        {
            this.ActivitiesCalls = new HashSet<ActivitiesCalls>();
        }
    
        public int ProjectID { get; set; }
        public Nullable<int> ProjectTypeID { get; set; }
        public Nullable<int> JobTypeID { get; set; }
        public string ProjectName { get; set; }
        public string ProjectNumber { get; set; }
        public Nullable<int> ProjectStatusID { get; set; }
        public Nullable<int> ContactID { get; set; }
        public string ProjectAddress1 { get; set; }
        public string ProjectAddress2 { get; set; }
        public string ProjectCity { get; set; }
        public string ProjectState { get; set; }
        public string ProjectZip { get; set; }
        public string ProjectCountry { get; set; }
        public Nullable<System.DateTime> ProjectStartDate { get; set; }
        public string ProjectPhone { get; set; }
        public string ProjectEmail { get; set; }
        public string ProjectNotes { get; set; }
        public Nullable<System.DateTime> DateUploaded { get; set; }
        public string AcctJobID { get; set; }
        public Nullable<int> LinkID { get; set; }
        public Nullable<int> SalesID { get; set; }
        public Nullable<int> PmID { get; set; }
        public Nullable<int> DesignerID { get; set; }
        public Nullable<bool> QuoteApproved { get; set; }
        public Nullable<int> LeadID { get; set; }
        public Nullable<int> SiteCoID { get; set; }
        public string ApiLinkID { get; set; }
        public Nullable<int> BuilderID { get; set; }
        public string Site { get; set; }
        public string Lot { get; set; }
        public Nullable<System.DateTime> CloseDate { get; set; }
    
        public virtual ICollection<ActivitiesCalls> ActivitiesCalls { get; set; }
        public virtual CoContacts CoContacts { get; set; }
    }
}
