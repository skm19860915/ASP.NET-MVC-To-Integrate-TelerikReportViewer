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
    
    public partial class ProjectPayTypes
    {
        public int ProjectPayTypeID { get; set; }
        public int ProjectID { get; set; }
        public int PayTypeID { get; set; }
        public Nullable<bool> IsDefault { get; set; }
        public Nullable<decimal> PayTypeFactor { get; set; }
    }
}