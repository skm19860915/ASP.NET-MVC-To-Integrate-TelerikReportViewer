using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ePonti.BOL.Models
{
   public class UserProfile
    {
        public int ViewID { get; set; }
        public string UserDisplayName { get; set; }
        public string Job_Title { get; set; }
        public string Calendar_Color { get; set; }
        public string Phone { get; set; }
        public string E_Mail { get; set; }
        public Nullable<int> Profile { get; set; }
        public Nullable<int> PayrollSecurity { get; set; }
        public Nullable<int> License { get; set; }        
            
    }
}
