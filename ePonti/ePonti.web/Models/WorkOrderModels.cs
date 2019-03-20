using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using ePonti.BOL;

namespace ePonti.web.Models
{
    public class WorkOrderModels
    {
        public class AssignWorkOrder
        {
            public string Items { get; set; }
            public int? ProjectID { get; set; }

            [DisplayName("Est. Hours")]
            public decimal? EstimatedHours { get; set; }

            [DisplayName("Status")]
            public int? StatusID { get; set; }



            public int? WorkOrderTypeID { get; set; }

            public List<Resource> Resources { get; set; }
            public class Resource
            {
                public int SiteUserID { get; set; }

                public string Dates { get; set; }
            }
        }

        public class EditWorkOrder
        {
            public int? WoID { get; set; }

            public string WorkOrderNumber { get; set; }

            public string JobName { get; set; }

            public string Phone { get; set; }

            public string Address1 { get; set; }

            public string Address2 { get; set; }

            public string City { get; set; }

            public string State { get; set; }

            public string Zip { get; set; }

            public string Country { get; set; }

            public decimal? EstimatedHours { get; set; }

            public TimeSpan? StartTime { get; set; }

            //Editable

            [DisplayName("Resource")]
            public int? ResourceID { get; set; }

            [DisplayName("Status")]
            public int? StatusID { get; set; }

            public DateTime? Assigned { get; set; }

            
        }
    }
}