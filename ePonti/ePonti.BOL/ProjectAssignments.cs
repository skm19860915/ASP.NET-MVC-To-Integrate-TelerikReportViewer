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
    
    public partial class ProjectAssignments
    {
        public int AssignmentID { get; set; }
        public int ItemID { get; set; }
        public Nullable<int> SiteUserID { get; set; }
        public int WoID { get; set; }
        public Nullable<decimal> Units { get; set; }
        public Nullable<decimal> EstHrs { get; set; }
        public Nullable<System.DateTime> ActDate { get; set; }
        public Nullable<decimal> ActHrs { get; set; }
        public Nullable<double> PercentComplete { get; set; }
    }
}
