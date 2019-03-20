using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ePonti.BOL;

namespace ePonti.web.Models
{
    public class ProcurementResult
    {
        public ProcurementResult()
        {
            this.PurchaseList = new List<GetPurchasingBySiteCoID_Result>();
            this.CustodyList = new List<GetCustodyBySiteCoID_Result>();
            this.DeliveryList = new List<GetDeliveriesBySiteCoID_Result>();
            this.TransferList = new List<GetTransfersBySiteCoID_Result>();
            this.PendingList = new List<GetPendingBySiteCoID_Result>();
            this.InventoryList = new List<GetInventoryBySiteCoID_Result>();
        }
        public List<GetPurchasingBySiteCoID_Result> PurchaseList { get; set; }
        public List<GetCustodyBySiteCoID_Result> CustodyList { get; set; }
        public List<GetDeliveriesBySiteCoID_Result> DeliveryList { get; set; }
        public List<GetTransfersBySiteCoID_Result> TransferList { get; set; }
        public List<GetPendingBySiteCoID_Result> PendingList { get; set; }
        public List<GetInventoryBySiteCoID_Result> InventoryList { get; set; }
    }
}