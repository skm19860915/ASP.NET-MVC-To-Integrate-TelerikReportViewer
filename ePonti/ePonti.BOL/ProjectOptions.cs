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
    
    public partial class ProjectOptions
    {
        public int ProjectOptionID { get; set; }
        public Nullable<int> ProjectID { get; set; }
        public Nullable<int> MarginMarkUpRule { get; set; }
        public Nullable<decimal> Margin { get; set; }
        public Nullable<decimal> Markup { get; set; }
        public Nullable<decimal> EquipAdjust { get; set; }
        public string EquipAdjustLabel { get; set; }
        public Nullable<decimal> LaborAdjust { get; set; }
        public string LaborAdjustLabel { get; set; }
        public Nullable<decimal> OtherAdjust { get; set; }
        public string OtherAdjustLabel { get; set; }
        public string OtherAdjustType { get; set; }
        public Nullable<decimal> ProfileAdjust { get; set; }
        public string ProfileAdjustLabel { get; set; }
        public string ProfileAdjustType { get; set; }
    }
}
