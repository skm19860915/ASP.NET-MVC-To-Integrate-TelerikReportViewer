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
    
    public partial class CoContactSubtypes
    {
        public CoContactSubtypes()
        {
            this.CoContacts = new HashSet<CoContacts>();
        }
    
        public int ContactSubtypeID { get; set; }
        public Nullable<int> ContactTypeID { get; set; }
        public string SubtypeName { get; set; }
        public Nullable<int> ContactSubtypeOrder { get; set; }
        public Nullable<int> SiteCoID { get; set; }
    
        public virtual ICollection<CoContacts> CoContacts { get; set; }
    }
}
