using ePonti.BOL;
using Intuit.Ipp.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace ePonti.web.Models
{
    public class QBSyncdto
    {
        public bool IsVendorSync { get; set; }
        public bool IsCustomerSync { get; set; }
        public bool IsItemSync { get; set; }
        public QBAuthorizationdto OauthToken { get; set; }
        public string ConnectionString
        {
            get { return ConfigurationManager.ConnectionStrings["ePontiv2Entities"].ToString(); }        
        }
        public List<Vendor> VendorList { get; set; }
        public List<Customer> CustomerList { get; set; }
        public List<Item> ItemList { get; set; }
        public Account account { get; set; }

        public Int64 Id { get; set; }
        public string QboId { get; set; }
        public string CompanyId { get; set; }
        private string viewInQbo = string.Empty;
        public string ViewInQbo { get; set; }
        public string DeepLink
        {
            get
            {
                return ConfigurationManager.AppSettings["DeepLink"];
            }

        }
    }
}