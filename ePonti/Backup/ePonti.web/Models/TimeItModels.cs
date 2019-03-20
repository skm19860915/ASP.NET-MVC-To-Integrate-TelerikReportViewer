using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using ePonti.BOL;

namespace ePonti.web.Models
{
    public class TimeItModels
    {
        public class NewTimeIt
        {
            public int? TimeItID { get; set; }

            [DisplayName("Job")]
            public int? ProjectID { get; set; }

            [MaxLength(255)]
            public string Task { get; set; }

            [DisplayName("Resource")]
            public int? ResourceID { get; set; }

            [DisplayName("Stage")]
            [Required(ErrorMessage ="Select project stage")]
            public int? StageID { get; set; }

            [DisplayName("Paytype")]
            [Required(ErrorMessage = "Select project paytype")]
            public int? PaytypeID { get; set; }

            public DateTime? Date { get; set; }

            public Decimal? Hours { get; set; }

            public string Notes { get; set; }
        }
    }
}