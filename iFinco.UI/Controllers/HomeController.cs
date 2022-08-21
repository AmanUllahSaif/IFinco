using iFinco.UI.Models;
using iFinco.UI.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace iFinco.UI.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        [HttpPost]
        public ActionResult Contact(ContactVM model)
        {
            ViewBag.Message = "Your contact page.";
            string msg = string.Empty;
            msg += "Name :" + model.Name+ "<br />";
            msg += "Phone :" + model.Phone+ "<br />";
            if (!String.IsNullOrEmpty(model.Email))
            {
                msg += "Email :" + model.Email + "<br />";
            }
            string subject = "Contatc Us Email";
            if (!string.IsNullOrEmpty(model.Subject))
            {
                subject += "(" + model.Subject + ")";
            }
            subject += "|"+DateTime.Now.Date;
            msg += model.Message;
            MailManager.SendMail(model.Email,msg, subject);
            return RedirectToAction("Index");
        }
    }
}