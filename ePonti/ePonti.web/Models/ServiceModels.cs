using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using ePonti.BOL;

namespace ePonti.web.Models
{
    public class ServiceModels
    {
        public class NewService
        {
            public int? ServiceID { get; set; }

            public int? ParentQuoteID { get; set; }

            [Required(ErrorMessage="Enter service name")]
            [MaxLength(255)]
            [DisplayName("Service Name")]
            public string ServiceName { get; set; }

            [MaxLength(255)]
            [DisplayName("Service#")]
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

            [DisplayName("Service Phone")]
            public string ServicePhone { get; set; }

            [MaxLength(300)]
            [UIHint("name@company.com")]
            [EmailAddress(ErrorMessage = "Email is not valid")]
            public string Email { get; set; }

            [DisplayName("Status")]
            public int? StatusID { get; set; }

            [DisplayName("Start Date")]
            public DateTime? StartDate { get; set; }

            [DisplayName("Sales Person")]
            public int? SalesPersonID { get; set; }

            [DisplayName("Project Manager")]
            public int? ProjectManagerID { get; set; }

            [DisplayName("Designer")]
            public int? DesignerID { get; set; }
        }
    }
}