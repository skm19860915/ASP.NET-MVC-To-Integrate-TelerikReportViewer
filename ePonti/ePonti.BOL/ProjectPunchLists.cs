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
    
    public partial class ProjectPunchLists
    {
        public int ProjectPunchListID { get; set; }
        public Nullable<int> ProjectID { get; set; }
        public Nullable<long> Number { get; set; }
        public string Subject { get; set; }
        public Nullable<int> TypeID { get; set; }
        public Nullable<int> PriorityID { get; set; }
        public Nullable<int> StatusID { get; set; }
        public Nullable<int> CreatedByUserID { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<int> DepartmentID { get; set; }
        public Nullable<int> DivisionID { get; set; }
        public Nullable<int> AreaID { get; set; }
        public string Note { get; set; }
        public Nullable<decimal> EstHrs { get; set; }
        public Nullable<System.DateTime> DueDate { get; set; }
        public Nullable<int> SiteCoID { get; set; }
    }
}
