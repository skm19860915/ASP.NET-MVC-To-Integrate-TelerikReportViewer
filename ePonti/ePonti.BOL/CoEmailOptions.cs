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
    
    public partial class CoEmailOptions
    {
        public int EmailOptionsID { get; set; }
        public Nullable<int> EmailTypeID { get; set; }
        public string EmailFrom { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string EMailCC { get; set; }
        public Nullable<int> SiteCoID { get; set; }
    }
}