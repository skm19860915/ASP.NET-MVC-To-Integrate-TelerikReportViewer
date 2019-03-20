using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using ePonti.BOL;

namespace ePonti.web.Models
{
    public class SOModels
    {
        public class NewSO
        {
            public int? SOID { get; set; }

            [Required]
            [DisplayName("Sales Order Name")]
            public string Name { get; set; }

            [MaxLength(255)]
            [DisplayName("SO #")]
            public string SONumber { get; set; }

            [DisplayName("Job")]
            public int? JobID { get; set; }

            [DisplayName("Term")]
            public int? TermID { get; set; }

            [DisplayName("Status")]
            public int? StatusID { get; set; }

            [DisplayName("Date")]
            public DateTime? Date { get; set; }

            public string Creator { get; set; }
        }
    }
}