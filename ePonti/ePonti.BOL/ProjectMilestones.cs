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
    
    public partial class ProjectMilestones
    {
        public int ProjectMilestoneID { get; set; }
        public Nullable<int> ProjectID { get; set; }
        public Nullable<int> ProjectManagerID { get; set; }
        public Nullable<int> MilestoneID { get; set; }
        public Nullable<int> ProjectMilestoneOrder { get; set; }
        public Nullable<int> ProjectMilestoneNumber { get; set; }
        public string PreMilestoneNumber { get; set; }
        public Nullable<int> ProjectMilestoneLag { get; set; }
        public Nullable<System.DateTime> ProjectMilestoneDueDate { get; set; }
        public Nullable<int> ProjectMilestoneStatusID { get; set; }
        public Nullable<System.DateTime> ProjectMilestoneCompletedDate { get; set; }
        public Nullable<int> ProjectMilestoneCompletedUserID { get; set; }
    }
}
