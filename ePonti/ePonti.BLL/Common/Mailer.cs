using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ePonti.BLL.Common
{
    public class Mailer
    {
        public static async Task Execute(string Subject, string To, string ToName, string Content)
        {
            try
            {
                var apiKey = ConfigurationManager.AppSettings["SendGridApiKey"];
                var client = new SendGridClient(apiKey);
                var from = new EmailAddress(ConfigurationManager.AppSettings["Email.SendFrom"], ConfigurationManager.AppSettings["Email.SendFromName"]);
                var to = new EmailAddress(To, ToName);

                //Some master template can be applied here to 'Content'

                var msg = MailHelper.CreateSingleEmail(from, to, Subject, "", Content);
                var response = await client.SendEmailAsync(msg);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
