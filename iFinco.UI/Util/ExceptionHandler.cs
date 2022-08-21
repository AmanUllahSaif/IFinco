using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace iFinco.UI.Util
{
    public class ExceptionHandler : IExceptionFilter
    {


        public void OnException(ExceptionContext filterContext)
        {
            if (filterContext.Exception != null)
            {
                LogException(filterContext.Exception);
            }
        }

        private void LogException(Exception ex)
        {
            string msg = "<h5>" + ex.Message + "</h5>";
            string trace = "<p>" + ex.StackTrace + "</p>";

            string mailMessage = "<h4>An Exception Found </h4> <b> Date :" + DateTime.Now.ToString("dd/MM/yyyy - HH:mm") + "</b> <br />";
            mailMessage += "<b>Error :</b> " + ex.Message;
            mailMessage += "<br /><b>Stack Trace :</b> " + trace + "<br />";
            if (ex.InnerException != null)
            {
                mailMessage += "<h4> Inner Exception </h4>";
                mailMessage += "<h5>" + ex.InnerException.Message + "</h5>";
                mailMessage += "<p>" + ex.InnerException.StackTrace + "</p>";
            }
            string to = ConfigurationManager.AppSettings["logEmail"];
            MailManager.SendMail(to, mailMessage, "An Exception Found " + DateTime.Now.ToString("dd/MM/yyyy"));
        }
    }
}