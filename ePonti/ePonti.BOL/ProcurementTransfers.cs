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
    
    public partial class ProcurementTransfers
    {
        public int TransferID { get; set; }
        public Nullable<int> TransferTypeID { get; set; }
        public string Subject { get; set; }
        public string TransferNumber { get; set; }
        public Nullable<System.DateTime> TransferDate { get; set; }
        public Nullable<int> TransferStatusID { get; set; }
        public string Reason { get; set; }
        public int SiteCoID { get; set; }
        public int SiteUserID { get; set; }
    }
}