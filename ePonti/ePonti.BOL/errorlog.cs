//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ePonti.BOL
{
    using System;
    using System.Collections.Generic;
    
    public partial class errorlog
    {
        public int errorlogid { get; set; }
        public System.DateTimeOffset datecreated { get; set; }
        public string sourcetype { get; set; }
        public string errorsource { get; set; }
        public int errornumber { get; set; }
        public int linenumber { get; set; }
        public int userid { get; set; }
        public int severity { get; set; }
        public string errormessage { get; set; }
        public string errordescription { get; set; }
    }
}
