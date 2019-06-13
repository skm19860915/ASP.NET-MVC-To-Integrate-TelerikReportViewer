using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using ePonti.BOL;

namespace ePonti.web.Models
{
    public class CORModels
    {
        public class NewCOR
        {
            public int? CorID { get; set; }

            [Required]
            [DisplayName("Change Order Request Name")]
            public string Name { get; set; }

            [MaxLength(255)]
            [DisplayName("COR #")]
            public string CORNumber { get; set; }

            [DisplayName("Job")]
            public int? JobID { get; set; }

            [DisplayName("Type")]
            public int? TypeID { get; set; }

            [DisplayName("Status")]
            public int? StatusID { get; set; }

            [DisplayName("Date")]
            public DateTime? Date { get; set; }

            public string Creator { get; set; }
        }
    }
}