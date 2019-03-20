using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using ePonti.BOL;

namespace ePonti.web.Models
{
    public class JobModels
    {
        public class NewJob
        {
            public int? JobID { get; set; }

            public int? ParentQuoteID { get; set; }

            [Required(ErrorMessage="Enter job name")]
            [MaxLength(255)]
            [DisplayName("Job Name")]
            public string JobName { get; set; }

            [MaxLength(255)]
            [DisplayName("Job#")]
            public string JobNumber { get; set; }

            [DisplayName("Client")]
            public int? ClientID { get; set; }

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

            [MaxLength(100)]
            public string Country { get; set; }

            [MaxLength(50)]
            public string Zip { get; set; }

            [DisplayName("Job Phone")]
            public string JobPhone { get; set; }

            [MaxLength(300)]
            [UIHint("name@company.com")]
            [EmailAddress(ErrorMessage = "Email is not valid")]
            public string Email { get; set; }

            [DisplayName("Status")]
            public int? StatusID { get; set; }

            [DisplayName("Start Date")]
            public DateTime? Date { get; set; }

            [DisplayName("Sales Person")]
            public int? SalesPersonID { get; set; }

            [DisplayName("Project Manager")]
            public int? ProjectManagerID { get; set; }

            [DisplayName("Designer")]
            public int? DesignerID { get; set; }

            [DisplayName("Builder")]
            public int? BuilderID { get; set; }

            [MaxLength(100)]
            [DisplayName("Site")]
            public string Site { get; set; }

            [MaxLength(50)]
            [DisplayName("Lot#")]
            public string Lot { get; set; }
        }
    }
}