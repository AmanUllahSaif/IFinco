using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace iFinco.UI.Util
{
    public class MailManager
    {
        public static void SendMail(string to, string message, string subject)
        {
            string from = ConfigurationManager.AppSettings["from"].ToString();
            string host = ConfigurationManager.AppSettings["host"].ToString();
            string password = ConfigurationManager.AppSettings["password"].ToString();
            int port = Convert.ToInt32(ConfigurationManager.AppSettings["port"]);

            MailMessage mail = new MailMessage(from, to);
            mail.Subject = subject;
            mail.Body = message;
            mail.IsBodyHtml = true;

            SmtpClient client = new SmtpClient();
            client.Port = port;
            client.Host = host;
            client.EnableSsl = false;
            client.Credentials = new NetworkCredential(from, password);
            client.Send(mail);
        }
    }
}