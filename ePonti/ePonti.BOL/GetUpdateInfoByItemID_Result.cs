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
    
    public partial class GetUpdateInfoByItemID_Result
    {
        public Nullable<int> ViewID { get; set; }
        public string Por_ { get; set; }
        public string Vendor { get; set; }
        public string Manufacturer { get; set; }
        public string Model { get; set; }
        public string ProductDescription { get; set; }
        public Nullable<System.DateTime> Requested { get; set; }
        public Nullable<System.DateTime> Order { get; set; }
        public Nullable<System.DateTime> Arrival { get; set; }
        public Nullable<System.DateTime> Delivered { get; set; }
        public Nullable<decimal> EstCost { get; set; }
        public Nullable<decimal> ActCost { get; set; }
        public string CustodyLocation { get; set; }
        public string Packing_ { get; set; }
        public string Serial_ { get; set; }
        public int LocationID { get; set; }
    }
}