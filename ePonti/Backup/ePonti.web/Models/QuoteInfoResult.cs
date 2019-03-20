using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ePonti.BOL;

namespace ePonti.web.Models
{
    public class QuoteInfoResult
    {
        public QuoteInfoResult()
        {
            this.ProjectInfo = new GetProjectInfoByProjectID_Result();
            this.ActivitiesList = new List<GetProjectInfoActivitiesByProjectID_Result>();
            this.PartsList = new List<GetProjectInfoPartsByProjectID_Result>();
            this.ContactsList = new List<GetProjectContactsByProjectID_Result>();
        }
        public GetProjectInfoByProjectID_Result ProjectInfo { get; set; }
        public List<GetProjectInfoActivitiesByProjectID_Result> ActivitiesList { get; set; }
        public List<GetProjectInfoPartsByProjectID_Result> PartsList { get; set; }
        public List<GetProjectContactsByProjectID_Result> ContactsList { get; set; }
    }
}