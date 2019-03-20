using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using ePonti.BOL;

namespace ePonti.web.Models
{
    public class EventModels
    {
        public class NewEvent
        {
            public int? EventID { get; set; }

            [MaxLength(255)]
            [Required(ErrorMessage ="Enter event name")]
            public string Event { get; set; }

            [DisplayName("Owner")]
            public int? OwnerID { get; set; }

            [DisplayName("Job")]
            public int? JobID { get; set; }

            [DisplayName("Status")]
            public int? StatusID { get; set; }

            [DisplayName("Date/Time")]
            public DateTime? Date { get; set; }

            public Decimal? Duration { get; set; }

            [DisplayName("Invitees")]
            public List<string> InviteeIds { get; set; }

            public string Notes { get; set; }
        }
    }
}