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
    
    public partial class ProjectOrderQuantity
    {
        public int OrderQuantityID { get; set; }
        public Nullable<int> PurchaseOrderID { get; set; }
        public Nullable<int> MasterItemID { get; set; }
        public Nullable<int> QuantityOrdered { get; set; }
        public Nullable<decimal> CostOrdered { get; set; }
        public Nullable<decimal> ActualCost { get; set; }
    }
}