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
    
    public partial class ProjectDeposits
    {
        public int ProjectDepositID { get; set; }
        public Nullable<int> ProjectID { get; set; }
        public Nullable<int> SiteUserID { get; set; }
        public Nullable<System.DateTime> DepositDate { get; set; }
        public string DepositInfo { get; set; }
        public Nullable<decimal> DepositAmount { get; set; }
        public Nullable<int> SoID { get; set; }
    }
}
