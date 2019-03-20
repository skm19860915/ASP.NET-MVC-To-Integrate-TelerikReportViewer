using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ePonti.web.Models
{
    public class TransferModels
    {
        public class NewTransfer
        {
            public int PorID { get; set; }

            public DateTime? Date { get; set; }

            [DisplayName("Transfer #")]
            public string TransferNumber { get; set; }

            public string Reason { get; set; }

            public List<int> ToProjectItemIDs { get; set; }

            [Required(ErrorMessage = "Please select the 'from' project")]
            public int FromProjectID { get; set; }

            public List<int> FromProjectItemIDs { get; set; }

            public List<int> PorItemIDs { get; set; }
        }
    }
}