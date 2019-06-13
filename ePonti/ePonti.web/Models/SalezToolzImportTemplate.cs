using ePonti.BOL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ePonti.web.Models
{
    public class SalezToolzImportTemplate
    {
        public int Version { get; set; }
        public ClientInfo ClientInfo { get; set; }

        public SalezToolzImportTemplate(GetSalezToolzExportByProjectID_Result STResult)
        {
            Version = 1;
            ClientInfo = new ClientInfo();
            ClientInfo.FirstName = STResult.FirstName;
            ClientInfo.LastName = STResult.LastName;
            ClientInfo.EmailAddress = STResult.emailAddress;
            ClientInfo.MobilePhone = STResult.MobilePhone;
            ClientInfo.HomeAddress = new HomeAddress
            {
                Label = "Project Address",
                Street = STResult.HomeStreet,
                City = STResult.HomeCity,
                State = STResult.HomeState,
                Zip = STResult.HomeZip,
                Phone = STResult.HomePhone
            };
            ClientInfo.WorkAddress = new WorkAddress
            {
                Label = "Billing Address",
                Street = STResult.WorkStreet,
                City = STResult.WorkCity,
                State = STResult.WorkState,
                Zip = STResult.WorkZip,
                Phone = STResult.WorkPhone
            }; 
        }
    }
    public class HomeAddress
    {
        public string Label { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Phone { get; set; }
    }

    public class WorkAddress
    {
        public string Label { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Phone { get; set; }
    }

    public class ClientInfo
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string MobilePhone { get; set; }
        public HomeAddress HomeAddress { get; set; }
        public WorkAddress WorkAddress { get; set; }
    }
}