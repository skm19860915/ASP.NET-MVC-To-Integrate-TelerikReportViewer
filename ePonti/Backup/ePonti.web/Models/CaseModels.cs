using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using ePonti.BOL;

namespace ePonti.web.Models
{
    public class CaseModels
    {
        public class NewCase
        {
            public int? CaseID { get; set; }

            [DisplayName("Job")]
            public int? JobID { get; set; }

            [DisplayName("Type")]
            public int? TypeID { get; set; }

            [MaxLength(255)]
            public string Concern { get; set; }

            [DisplayName("Priority")]
            public string Priority { get; set; }

            [DisplayName("Status")]
            public int? StatusID { get; set; }

            public DateTime? Created { get; set; }
            public DateTime? Due { get; set; }

            [DisplayName("Resource")]
            public int? ResourceID { get; set; }

            [DisplayName("Creator")]
            public int? CreatorID { get; set; }

            public string Notes { get; set; }
        }
    }
}