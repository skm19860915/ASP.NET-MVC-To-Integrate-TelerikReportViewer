using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ePonti.web.Models
{
    public class ReturnModels
    {
        public class NewReturn
        {
            public DateTime? Date { get; set; }

            [DisplayName("RMA #")]
            public string ReturnNumber { get; set; }

            [DisplayName("Status")]
            public int? StatusID { get; set; }

            public string Subject { get; set; }

            public string Reason { get; set; }

            public int? ToProjectID { get; set; }

            [DisplayName("Project")]
            [Required(ErrorMessage = "Please select the 'from' project")]
            public int FromProjectID { get; set; }

            public List<int> FromItemIDs { get; set; }

        }
    }
}