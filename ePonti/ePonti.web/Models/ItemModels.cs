using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using ePonti.BOL;
using ePonti.web.Common.ModelAttributes;

namespace ePonti.web.Models
{
    public class ItemModels
    {
        public class EditShortItem
        {
            public PriceModel Price { get; set; }

            public EditShortItem()
            {
                this.Price = new PriceModel();
            }


            public class PriceModel
            {
                public int? ItemID { get; set; }

                public decimal? Qty { get; set; }

                [DisplayName("Unit Cost")]
                public decimal? UnitCost { get; set; }

                [DisplayName("Unit Price")]
                public decimal? UnitPrice { get; set; }

                public decimal? Extension { get; set; }

                [Percentage]
                public decimal? Margin { get; set; }

                [Percentage]
                public decimal? Markup { get; set; }

                public decimal? Tax { get; set; }

                public bool Taxable { get; set; }

                public decimal? Total { get; set; }

                [DisplayName("Exclude POR")]
                public bool ExcludePOR { get; set; }

                [DisplayName("Warrantee Part")]
                public bool WarranteePart { get; set; }

                [DisplayName("One-Off Item")]
                public bool OneOffItem { get; set; }
            }
        }

        public class ItemDetails
        {
            public int ViewID { get; set; }
            public string Label { get; set; }
            public string ProductDescription { get; set; }
            public string SalesDescription { get; set; }
            public int AreaID { get; set; }
            public string Area { get; set; }
            public int DivisionID { get; set; }
            public string Division { get; set; }
            public string Por_ { get; set; }
            public Nullable<int> CustodyID { get; set; }
            public string Custody { get; set; }
            public Nullable<decimal> Qty { get; set; }
            public int ProjectCostCodeID { get; set; }
            public string CostCode { get; set; }
            public Nullable<decimal> UnitCost { get; set; }
            public Nullable<decimal> UnitPrice { get; set; }
            public Nullable<decimal> Extension { get; set; }
            public Nullable<decimal> Margin { get; set; }
            public Nullable<decimal> Markup { get; set; }
            public Nullable<decimal> Tax { get; set; }
            public Nullable<decimal> TaxRate { get; set; }
            public Nullable<bool> Taxable { get; set; }
            public Nullable<decimal> Total { get; set; }
            public Nullable<bool> ExcludePor { get; set; }
            public Nullable<bool> WarranteePart { get; set; }
            public Nullable<bool> OneOffItem { get; set; }
            public Nullable<int> StageID { get; set; }
            public string Stage { get; set; }
            public Nullable<decimal> Hours { get; set; }
            public Nullable<decimal> ProjectTotalHours { get; set; }
        }
    }
}