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
    
    public partial class CoContactEmails
    {
        public int ContactEmailID { get; set; }
        public Nullable<int> ContactID { get; set; }
        public int EmailTypeID { get; set; }
        public string EmailAddress { get; set; }
        public Nullable<bool> IsDefault { get; set; }
    
        public virtual CoContacts CoContacts { get; set; }
        public virtual SiteContactEmailTypes SiteContactEmailTypes { get; set; }
    }
}
