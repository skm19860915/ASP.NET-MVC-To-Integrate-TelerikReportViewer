using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using ePonti.BOL;

namespace ePonti.web.Models
{
    public class PORModels
    {
        public class NewPOR
        {
            [DisplayName("Requested Date")]
            public DateTime RequestedDate { get; set; }

            [DisplayName("Ship To")]
            public int? ShipToID { get; set; }

            public int? ProjectID { get; set; }
            public List<int> Items { get; set; }
        }

        public class EditPOR
        {
            public int? PorID { get; set; }

            //[DisplayName("POR#")]
            //public string PORNumber { get; set; }

            [DisplayName("Job")]
            public int? JobID { get; set; }

            [DisplayName("Vendor")]
            public int? VendorID { get; set; }

            [DisplayName("Creator")]
            public string Creator { get; set; }

            [DisplayName("Created")]
            public DateTime? CreatedDate { get; set; }

            [DisplayName("Priority")]
            public string Priority { get; set; }

            [DisplayName("Requested")]
            public DateTime? RequestedDate { get; set; }

            [DisplayName("Status")]
            public int? StatusID { get; set; }

            [DisplayName("Order Date")]
            public DateTime? OrderDate { get; set; }

            [DisplayName("Arrival")]
            public DateTime? ArrivalDate { get; set; }

            [DisplayName("Shipping")]
            public int? ShippingID { get; set; }

            [DisplayName("PO#")]
            public string PONumber { get; set; }

            [DisplayName("Ship To")]
            public int? ShipToID { get; set; }

            public string Tracking { get; set; }

            public string Notes { get; set; }
        }

        public class UpdateCustody
        {
            public int? ItemID { get; set; }

            public string PORNumber { get; set; }
            public string Vendor { get; set; }
            public string Manufacturer{ get; set; }
            public string Model { get; set; }
            public string ProductDescription{ get; set; }

            [DisplayName("Arrival")]
            public DateTime? ArrivalDate { get; set; }

            [DisplayName("Requested")]
            public DateTime? RequestedDate { get; set; }

            [DisplayName("Delivered")]
            public DateTime? DeliveredDate { get; set; }

            [DisplayName("Actual Cost")]
            public decimal? ActualCost{ get; set; }

            [DisplayName("Location")]
            public int? LocationID { get; set; }

            [DisplayName("Packing Slip#")]
            public string PackingSlipNumber { get; set; }

            [DisplayName("Serial#")]
            public string SerialNumber { get; set; }
        }
    }
}