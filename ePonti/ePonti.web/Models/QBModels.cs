using ePonti.BOL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ePonti.web.Models
{
    public class QBModels
    {
            public List<CoData> coData { get; set; }
            public List<CoContacts> coContacts { get; set; }
            public QBAuthorizationdto OAuthorizationModel { get; set; }
            public QBSyncdto SyncObjectsModel { get; set; }          
            public bool IsReadyTimeentry { get; set; }
            public bool IsReadytoInvoice { get; set; }
            public bool IsReadySync { get; set; }
            public bool IsConnected { get; set; }  
           
    }

    public class ContactList {
        public Int64 ContactID { get; set; }
        public string ContactLastName { get; set; }
        public string ContactFirstName { get; set; }
        public string ContactCoName { get; set; }
        public string ContactTypeName { get; set; }
        public string SubtypeName { get; set; }

    }
}