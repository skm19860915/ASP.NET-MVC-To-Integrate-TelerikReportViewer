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
    
    public partial class ProjectCorItems
    {
        public int CorItemID { get; set; }
        public Nullable<int> CorID { get; set; }
        public string Action { get; set; }
        public int ItemID { get; set; }
        public Nullable<int> MasterItemID { get; set; }
        public string ComponentID { get; set; }
        public Nullable<decimal> Qty { get; set; }
        public Nullable<int> StageID { get; set; }
        public Nullable<int> DivisionID { get; set; }
        public Nullable<int> AreaID { get; set; }
        public Nullable<System.DateTime> UploadedDate { get; set; }
        public string SerialNumber { get; set; }
        public string IpAddress { get; set; }
        public Nullable<int> ProjectKitID { get; set; }
        public Nullable<int> OptionID { get; set; }
        public Nullable<bool> ExcludePOR { get; set; }
        public string ProductDescription { get; set; }
        public string SalesDescription { get; set; }
        public string SKU { get; set; }
        public Nullable<bool> WarranteePart { get; set; }
        public Nullable<bool> OneOffPart { get; set; }
        public Nullable<decimal> UnitCost { get; set; }
        public Nullable<decimal> UnitPrice { get; set; }
        public Nullable<decimal> UnitHours { get; set; }
        public Nullable<int> SalesTaxID { get; set; }
        public Nullable<decimal> Margin { get; set; }
        public Nullable<decimal> Markup { get; set; }
        public Nullable<bool> SalesTaxed { get; set; }
        public Nullable<bool> LaborTaxed { get; set; }
        public Nullable<bool> CostTaxed { get; set; }
        public Nullable<int> ProjectCostCodeID { get; set; }
    }
}
