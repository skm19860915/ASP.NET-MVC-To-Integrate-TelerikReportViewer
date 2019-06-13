using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using ePonti.BOL;

namespace ePonti.web.Models
{
    public class LeadModels
    {
        public class NewLead
        {
            public int? LeadID { get; set; }

            [Required(ErrorMessage="Enter lead name")]
            [MaxLength(255)]
            [DisplayName("Label")]
            public string LeadName { get; set; }

            [MaxLength(255)]
            [DisplayName("Lead#")]
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

            public string Phone { get; set; }

            [MaxLength(300)]
            [UIHint("name@company.com")]
            [EmailAddress(ErrorMessage = "Email is not valid")]
            public string Email { get; set; }

            [DisplayName("Status")]
            public int? StatusID { get; set; }

            [DisplayName("Close Date")]
            public DateTime? CloseDate { get; set; }

            [DisplayName("Source")]
            public int? TypeID { get; set; }

            [DisplayName("Probability")]
            public int? ProbabilityID { get; set; }

            public decimal? Budget{ get; set; }

            [DisplayName("Stage")]
            public int? LeadPhaseID { get; set; }

            [DisplayName("Priority")]
            public int? PriorityID { get; set; }

            [DisplayName("Sales Person")]
            public int? SalesPersonID { get; set; }

            [DisplayName("System")]
            public int? SystemID { get; set; }

            [DisplayName("Builder")]
            public int? BuilderID { get; set; }

            [MaxLength(100)]
            [DisplayName("Site")]
            public string Site { get; set; }

            [MaxLength(50)]
            [DisplayName("Lot#")]
            public string Lot { get; set; }

            public List<int> ProjectCommunicationIDs { get; set; }

            public NewLead()
            {
                this.ProjectCommunicationIDs = new List<int>();
            }
        }
    }
}