using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ePonti.BOL;
using ePonti.web.Models;
using ePonti.web.Areas.Options.Models;

namespace ePonti.web.Areas.Options.Controllers
{
    [Authorize]
    public class ReportingOptionsController : ePonti.web.Controllers._baseMVCController
    {
        private ePontiv2Entities db = new ePontiv2Entities();

        // GET: Options/ReportingOptions
        public ActionResult Index()
        {
            int siteCoID = base.siteusercompanyid;

            ViewBag.CoReports = db.GetReportsBySiteCoID(siteCoID);
            var projects = db.GetProjectsBySiteCoID(siteCoID);
            var workorders = new List<WorkOrderListModel>();
            var pors = new List<PorListModel>();
            var deliveries = new List<DeliveryListModel>();
            var requests = new List<RequestListModel>();
            foreach(var item in projects)
            {
                var workorder_list = db.GetProjectWorkByProjectID(item.ViewID);
                var por_list = db.GetProjectPorListByProjectID(item.ViewID);
                var delivery_list = db.GetProjectDeliveryListByProjectID(item.ViewID);
                var request_list = db.GetProjectCorByProjectID(item.ViewID);
                foreach(var value in workorder_list)
                {
                    var record = new WorkOrderListModel()
                    {
                        Name = value.WO_,
                        ViewID = value.ViewID
                    };
                    workorders.Add(record);
                }
                foreach(var value in por_list)
                {
                    var record = new PorListModel()
                    {
                        Name = value.Por_,
                        ViewID = value.ViewID
                    };
                    pors.Add(record);
                }
                foreach(var value in delivery_list)
                {
                    var record = new DeliveryListModel()
                    {
                        Name = value.Delivery_,
                        ViewID = value.ViewID
                    };
                    deliveries.Add(record);
                }
                foreach(var value in request_list)
                {
                    var record = new RequestListModel()
                    {
                        Name = value.Name,
                        ViewID = value.View
                    };
                    requests.Add(record);
                }
            }
            ViewBag.WorkOrderList = workorders;
            ViewBag.PorList = pors;
            ViewBag.DeliveryList = deliveries;
            ViewBag.RequestList = requests;

            return View();
        }

        public ActionResult Save(int woid, int porid, int deliveryid, int reqid)
        {
            return View();
        }
    }
}