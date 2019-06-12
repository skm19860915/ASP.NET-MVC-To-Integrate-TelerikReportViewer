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
    
    public partial class CoContacts
    {
        public CoContacts()
        {
            this.ActivitiesCalls = new HashSet<ActivitiesCalls>();
            this.CoContactAddresses = new HashSet<CoContactAddresses>();
            this.CoContactEmails = new HashSet<CoContactEmails>();
            this.CoContactPhones = new HashSet<CoContactPhones>();
            this.ProjectInfo = new HashSet<ProjectInfo>();
        }
    
        public int ContactID { get; set; }
        public int ContactTypeID { get; set; }
        public Nullable<int> ContactSubtypeID { get; set; }
        public Nullable<int> ContactStatusID { get; set; }
        public string ContactFirstName { get; set; }
        public string ContactLastName { get; set; }
        public string ContactTitle { get; set; }
        public Nullable<int> ContactCoID { get; set; }
        public string FaceBook { get; set; }
        public string LinkedIn { get; set; }
        public string Skype { get; set; }
        public string ContactReferredBy { get; set; }
        public string AcctCustomerID { get; set; }
        public Nullable<int> ModifyUserID { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<int> ContactManager { get; set; }
        public Nullable<bool> CoAcct { get; set; }
        public int SiteCoID { get; set; }
        public string ApiLinkID { get; set; }
        public string Custom1 { get; set; }
        public string Custom2 { get; set; }
        public string Custom3 { get; set; }
        public string Custom4 { get; set; }
    
        public virtual ICollection<ActivitiesCalls> ActivitiesCalls { get; set; }
        public virtual ICollection<CoContactAddresses> CoContactAddresses { get; set; }
        public virtual CoContactCompanies CoContactCompanies { get; set; }
        public virtual ICollection<CoContactEmails> CoContactEmails { get; set; }
        public virtual ICollection<CoContactPhones> CoContactPhones { get; set; }
        public virtual CoContactSubtypes CoContactSubtypes { get; set; }
        public virtual CoContactTypes CoContactTypes { get; set; }
        public virtual SiteContactStatus SiteContactStatus { get; set; }
        public virtual ICollection<ProjectInfo> ProjectInfo { get; set; }
    }
}