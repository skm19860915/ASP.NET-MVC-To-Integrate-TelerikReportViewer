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
    
    public partial class CoKits
    {
        public int MasterKitID { get; set; }
        public Nullable<int> GroupID { get; set; }
        public string KitName { get; set; }
        public string KitSku { get; set; }
        public string KitDescription { get; set; }
        public Nullable<decimal> KitDiscount { get; set; }
        public Nullable<bool> KitSummarize { get; set; }
        public string AcctKitID { get; set; }
        public byte[] KitImage { get; set; }
        public Nullable<bool> Discontinued { get; set; }
        public Nullable<int> SiteCoID { get; set; }
    }
}
