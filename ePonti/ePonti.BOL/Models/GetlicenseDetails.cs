using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ePonti.BOL.Models
{
 public   class GetlicenseDetails
    {
     
            public int Quantity { get; set; }
            public int LicenseID { get; set; }
            public int DurationID { get; set; }
            public string Duration { get; set; }
            public int LicensePriceID { get; set; }
            public string Version { get; set; }
            public string Description { get; set; }
            public Nullable<decimal> Price { get; set; }
        
    }
}
