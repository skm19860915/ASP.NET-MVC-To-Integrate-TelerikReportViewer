using ePonti.BLL.Common;
using ePonti.BOL.Repository;
using ePonti.web.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ePonti.web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Solution()
        {
            ViewBag.Message = "Your Solution page";

            return View();
        }

        public ActionResult blog()
        {
            ViewBag.Message = "Your blog";

            return View();
        }

        public ActionResult pricingbox()
        {
            ViewBag.Message = "ePonti Prices";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page";

            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Contact(ContactUsModel Model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await Mailer.Execute("New Message", ConfigurationManager.AppSettings["Email.SiteAdminMail"], "ePonti Admin",

                        string.Format(@"Hi,<br>New message received from ePonti Contact form - <br><br>
                                    Name: {0}<br>
                                    Email: {1}<br>
                                    Subject: {2}<br>
                                    Message: {3}<br>",
                                        Model.Name, Model.Email, Model.Subject, Model.Message)
                        );

                    //reset form data
                    Model = new ContactUsModel() { };

                    ViewBag.Status = "Your message has been sent. Thank you!";
                    return View(Model);
                }
                else
                {
                    ViewBag.Status = "* Please fill all required form fields";
                    return View(Model);
                }
            }
            catch (Exception ex)
            {
                LogRepository.LogException(ex);
            }

            ViewBag.Status = "There was some error while sending your message. Please retry.";
            return View(Model);
        }

    }
}