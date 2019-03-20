using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using ePonti.BOL;

namespace ePonti.web.Models
{
    public class PunchListModels
    {
        public class NewPunchList
        {
            public int? PunchListID { get; set; }

            [DisplayName("Job")]
            public int? JobID { get; set; }

            [DisplayName("Type")]
            public int? TypeID { get; set; }

            [MaxLength(255)]
            public string Description { get; set; }

            [Required(ErrorMessage = "Select department")]
            [DisplayName("Department")]
            public int? DepartmentID { get; set; }

            [DisplayName("Division")]
            public int? DivisionID { get; set; }

            [DisplayName("Area")]
            public int? AreaID { get; set; }

            [DisplayName("Priority")]
            public int? PriorityID { get; set; }

            [DisplayName("Status")]
            public int? StatusID { get; set; }

            public DateTime? Due { get; set; }

            [DisplayName("Est Hrs")]
            public Decimal? Hours { get; set; }

            [DisplayName("Creator")]
            public int? CreatorID { get; set; }

            public string Notes { get; set; }
        }
    }
}