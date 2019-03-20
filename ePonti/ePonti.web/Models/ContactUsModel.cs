using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ePonti.web.Models
{
    public class ContactUsModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Email { get; set; }

        public string Subject { get; set; }

        [Required]
        public string Message { get; set; }
    }
}