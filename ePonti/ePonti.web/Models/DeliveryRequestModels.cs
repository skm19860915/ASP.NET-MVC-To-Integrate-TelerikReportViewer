using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using ePonti.BOL;

namespace ePonti.web.Models
{
    public class DeliveryRequestModels
    {
        public class EditDeliveryRequest
        {
            public int? RequestID { get; set; }

            public string RequestNumber { get; set; }

            public string ProjectName { get; set; }

            public DateTime? CreatedOn { get; set; }


            //Editable

            public DateTime? Delivery { get; set; }

            [DisplayName("Type")]
            public int? TypeID { get; set; }

            [DisplayName("Status")]
            public int? StatusID { get; set; }

            public string Note { get; set; }
        }
    }
}