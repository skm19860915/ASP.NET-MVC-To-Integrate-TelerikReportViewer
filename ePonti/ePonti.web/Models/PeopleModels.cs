using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using ePonti.BOL;

namespace ePonti.web.Models
{
    public class PeopleModels
    {
        public class NewContact
        {
            public int? ContactID { get; set; }

            [MaxLength(255)]
            public string First { get; set; }

            [MaxLength(255)]
            public string Last { get; set; }

            [MaxLength(255)]
            public string Company { get; set; }

            public string Phone { get; set; }

            public string Mobile { get; set; }

            [MaxLength(300)]
            [UIHint("name@company.com")]
            [EmailAddress(ErrorMessage = "Email is not valid")]
            public string Email { get; set; }

            [MaxLength(300)]
            [DisplayName("Address")]
            public string Address1 { get; set; }

            [MaxLength(300)]
            [DisplayName(" ")]
            public string Address2 { get; set; }

            [MaxLength(100)]
            public string City { get; set; }

            [MaxLength(100)]
            public string State { get; set; }

            [MaxLength(50)]
            public string Zip { get; set; }

            [DisplayName("Country")]
            public int? CountryID { get; set; }

            [DisplayName("Contact Type")]
            public int? TypeID { get; set; }

            [DisplayName("Contact Status")]
            public int? StatusID { get; set; }

            [DisplayName("Owner")]
            public int? OwnerID { get; set; }

            [DisplayName("Contact Subtype")]
            public int? ContactSubtypeID { get; set; }

            public string FaceBook { get; set; }

            public string LinkedIn { get; set; }

            public string Skype { get; set; }

            public string Custom1 { get; set; }

            public string Custom2 { get; set; }

            public string Custom3 { get; set; }

            public string Custom4 { get; set; }

            public string CustomLabel1 { get; set; }

            public string CustomLabel2 { get; set; }

            public string CustomLabel3 { get; set; }

            public string CustomLabel4 { get; set; }

        }
    }
    public class ContactTypes
    {
        public int ID { get; set; }

        public string Name { get; set; }

    }
}