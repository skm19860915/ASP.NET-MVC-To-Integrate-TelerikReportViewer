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
    
    public partial class SiteContactPhoneTypes
    {
        public SiteContactPhoneTypes()
        {
            this.CoContactPhones = new HashSet<CoContactPhones>();
        }
    
        public int PhoneTypeID { get; set; }
        public string PhoneTypeName { get; set; }
    
        public virtual ICollection<CoContactPhones> CoContactPhones { get; set; }
    }
}
