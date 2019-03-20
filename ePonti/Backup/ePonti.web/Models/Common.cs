using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Kendo.Mvc.UI;

namespace ePonti.web.Models
{
    public class CommonCls
    {
        public static int PageSize = 20;
    }
    public class ForSelect
    {
        public int ID { get; set; }

        public string Name { get; set; }

    }
    public class SchedulerEvents : ISchedulerEvent
    {
        public string Title { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string Description { get; set; }
        public bool IsAllDay { get; set; }
        public string Recurrence { get; set; }
        public string RecurrenceRule { get; set; }
        public string RecurrenceException { get; set; }
        public DateTime StartTimeZone { get; set; }
        public string StartTimezone { get; set; }
        public string EndTimezone { get; set; }
        public string color { get; set; }
        public string id { get; set; }
    }
    public class EmployeeViewModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public String LastName { get; set; }
        //public String gender { get; set; }
        //public String designation { get; set; }
        // public String department { get; set; }
        //public DateTime dob { get; set; }
    }
}