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
    
    public partial class ProjectDeliveryRequests
    {
        public int RequestID { get; set; }
        public int ProjectID { get; set; }
        public string RequestNumber { get; set; }
        public Nullable<System.DateTime> DateCreated { get; set; }
        public Nullable<System.DateTime> DeliveryDate { get; set; }
        public string RequestNotes { get; set; }
        public Nullable<int> DeliveryStatusID { get; set; }
        public Nullable<int> DeliveryTypeID { get; set; }
        public int SiteCoID { get; set; }
        public Nullable<System.TimeSpan> DeliveryTime { get; set; }
    }
}
