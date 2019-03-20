using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using ePonti.BOL;

namespace ePonti.web.Models
{
    public class CallModels
    {
        public class NewCall
        {
            public int? CallID { get; set; }

            [MaxLength(255)]
            public string Subject { get; set; }

            [DisplayName("Contact")]
            public int? ContactID { get; set; }

            public string Phone { get; set; }

            public string Mobile { get; set; }
            
            [DisplayName("Job")]
            public int? JobID { get; set; }

            [DisplayName("Purpose")]
            public int? PurposeID { get; set; }

            [DisplayName("Status")]
            public int? StatusID { get; set; }

            [DisplayName("Date/Time")]
            [DataType(DataType.Date)]
            [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd hh:mm tt}")]
            public DateTime? Date{ get; set; }

            public Decimal? Duration{ get; set; }

            [DisplayName("Resource")]
            public int? ResourceID { get; set; }

            public string Notes { get; set; }
        }
    }
}