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
    
    public partial class CoContactTypes
    {
        public CoContactTypes()
        {
            this.CoContacts = new HashSet<CoContacts>();
        }
    
        public int ContactTypeID { get; set; }
        public string ContactTypeName { get; set; }
        public Nullable<int> ContactTypeOrder { get; set; }
        public Nullable<bool> IsVendor { get; set; }
        public Nullable<int> SiteCoID { get; set; }
        public Nullable<bool> IsBuilder { get; set; }
    
        public virtual ICollection<CoContacts> CoContacts { get; set; }
    }
}
