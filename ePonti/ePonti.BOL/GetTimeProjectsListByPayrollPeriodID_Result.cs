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
    
    public partial class GetTimeProjectsListByPayrollPeriodID_Result
    {
        public Nullable<int> PayrollID { get; set; }
        public Nullable<int> PayrollTypeID { get; set; }
        public int PayrollEventID { get; set; }
        public Nullable<int> ProjectID { get; set; }
        public string Job { get; set; }
        public string Resource { get; set; }
        public string Task { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public Nullable<decimal> Hours { get; set; }
        public string CostCode { get; set; }
        public Nullable<int> ProjectCostCodeID { get; set; }
        public string PayType { get; set; }
        public Nullable<int> ProjectPayTypeID { get; set; }
        public Nullable<int> Reason { get; set; }
    }
}