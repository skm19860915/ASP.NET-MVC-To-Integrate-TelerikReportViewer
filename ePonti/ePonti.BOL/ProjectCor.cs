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
    
    public partial class ProjectCor
    {
        public int CorID { get; set; }
        public Nullable<int> CorTypeID { get; set; }
        public Nullable<int> CreatedByUserID { get; set; }
        public Nullable<int> ProjectID { get; set; }
        public string CorNumber { get; set; }
        public Nullable<int> CorStatusID { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public string Reason { get; set; }
        public Nullable<bool> Approved { get; set; }
        public string Subject { get; set; }
        public Nullable<System.DateTime> ApprovedDate { get; set; }
        public Nullable<int> ApproverID { get; set; }
        public Nullable<bool> Cor { get; set; }
    }
}