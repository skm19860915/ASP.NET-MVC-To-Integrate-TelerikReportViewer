using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using ePonti.BOL;

namespace ePonti.web.Models
{
    public class NoteModels
    {
        public class NewNote
        {
            public int? NoteID { get; set; }

            [MaxLength(255)]
            public string Subject { get; set; }

            [DisplayName("Contact")]
            public int? ContactID { get; set; }

            [DisplayName("Job")]
            public int? JobID { get; set; }

            public DateTime? Date { get; set; }

            [DisplayName("Creator")]
            public int? CreatorID { get; set; }

            public string Notes { get; set; }
        }
    }
}