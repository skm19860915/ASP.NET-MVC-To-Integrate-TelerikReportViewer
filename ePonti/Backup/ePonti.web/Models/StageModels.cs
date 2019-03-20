using ePonti.web.Common.ModelAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace ePonti.web.Models
{
    public class StageModels
    {
        public class StageDetails
        {
            public int? Index { get; set; }//used for model binding

            public int StageMasterID { get; set; }
            public int ProjectID { get; set; }

            public int? ProjectStageID { get; set; }
            public int? SubStageID { get; set; }
            public string SubName { get; set; }
            [Currency]
            public decimal? CostPerHour { get; set; }
            [Currency]
            public decimal? PricePerHour { get; set; }
            [Percentage]
            public decimal? Percent { get; set; }
            public decimal? SellPerHour { get; set; }
            public decimal? Hours { get; set; }
            public int? ItemID { get; set; }
            public decimal? UnitHours { get; set; }

        }

        public class StageMasterEdit
        {
            public int StageMasterID { get; set; }
            public string StageName { get; set; }

            public int? Order { get; set; }

            [DisplayName("Cost Code")]
            public int? CostCodeId { get; set; }

            [Currency]
            [DisplayName("Misc Equip")]
            public decimal? MiscEquip { get; set; }

            [Currency]
            [DisplayName("Misc Parts")]
            public decimal? MiscParts { get; set; }

            public List<StageDetails> StageSubDetails { get; set; }

            public StageMasterEdit()
            {
                this.StageSubDetails = new List<StageDetails>();
            }
        }

    }
}
