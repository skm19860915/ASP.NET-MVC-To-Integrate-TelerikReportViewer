using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ePonti.web.Models
{
    public class CompanyModels
    {
        public class NewCo
        {
            public int? SiteCoID { get; set; }

            [DisplayName("Company Name")]
            public string CoName { get; set; }

            [DisplayName("Version")]
            public int? CoVersionID { get; set; }

            [DisplayName("Status")]
            public int? CoStatusID { get; set; }

            [MaxLength(300)]
            [DisplayName("Address")]
            public string CoAddress1 { get; set; }

            [MaxLength(300)]
            [DisplayName(" ")]
            public string CoAddress2 { get; set; }

            [MaxLength(100)]
            public string CoCity { get; set; }

            [MaxLength(100)]
            public string CoState { get; set; }

            [MaxLength(100)]
            public string CoCountry { get; set; }

            [MaxLength(50)]
            public string CoZip { get; set; }

            [DisplayName("Company Phone")]
            public string CoPhone { get; set; }

            [DisplayName("Created Date")]
            public DateTime? CoDateCreated { get; set; }

            [DisplayName("Account#")]
            public string CoAcctNumber { get; set; }

        }
    }
}