using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ePonti.BOL.Models
{
 public  class TimeKeeperModel
    {
        public string SubmitSigned { get; set; }
        public decimal? SubmittedHours { get; set; }
        public int? PayrollID { get; set; }
    }
    public class UpdateTimeKeeper
    {       
        public int? PayrollID { get; set; }
        public int? ProjectApproverID { get; set; }
        public int? ItemPayPeriodId { get; set; }
        public int? ViewID { get; set; }
    }
    public class InsertRejectionInfo
    {
        public int? ViewID { get; set; }
        public int? PayrollID { get; set; }
        public int? SubmitUserID { get; set; }
        public string  PayrollReasonNote { get; set; }
    }
}
