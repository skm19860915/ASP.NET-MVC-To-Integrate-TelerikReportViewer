using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ePonti.web.Models
{
    public class ProjectFilesBOL
    {
        public int? FileId { get; set; }
        public string Name { get; set; }
        public string FilePath { get; set; }
        public DateTime ClientModified { get; set; }
    }
}