using iFinco.BLL.Handler;
using iFinco.DAL;
using iFinco.UI.Util;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace iFinco.UI.Controllers
{
    public class CompanyController : Controller
    {
        iFincoDBEntities context;
        CompanyHandler companyHandler;
        public CompanyController()
        {
            context = new iFincoDBEntities();
            companyHandler = new CompanyHandler(context);
        }
        // GET: Company
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(Company model, HttpPostedFileBase logo)
        {
            var email = companyHandler.List.Where(x => x.Email.ToLower().Equals(model.Email.ToLower())).FirstOrDefault();
            if (email != null)
            {
                ModelState.AddModelError("","Email already exist.");
                return View(model);
            }

            model.CreateOn = DateTime.UtcNow;
            model.IsActive = true;
            if (logo != null)
            {
                model.LogoUrl = FileManager.SaveImage(logo);
            }
            model.SecurityStamp = EncryptionManager.GenerateSecurityStamp();
            companyHandler.Add(model);
            string callbackUrl = Url.Action("ConfrimEmail", "Company", new { i = model.Id, e = EncryptionManager.HashText(model.Email, model.SecurityStamp) }, protocol: Request.Url.Scheme);
            string subject = "Company Email Confimation";
            string message = "Congrats you have successfully registered your company. We will review on your Company registeration request and let you know when it is approved. For email confirmation please click <a href='" + callbackUrl + "'> here </a>";
            MailManager.SendMail(model.Email, message, subject);
            return RedirectToAction("RegisterConfirmation", new { id = model.Id });
        }

        public ActionResult RegisterConfirmation(long? id)
        {
            if (id == null || id < 1)
            {
                return HttpNotFound();
            }
            var model = companyHandler.FindById(id.GetValueOrDefault());

            return View(model);
        }
        public ActionResult ConfrimEmail(long i, string e)
        {
            var company = companyHandler.FindById(i);
            if (EncryptionManager.Validate(e, company.SecurityStamp, company.Email))
            {
                company.IsEmailConfirmed = true;
                companyHandler.Update(company);
            }
            else
            {
                return View("Error");
            }
            return View();
        }
        [HttpGet]
        public ActionResult Edit(long? id)
        {
            if (id == null || id < 1)
            {
                return HttpNotFound();
            }
            var company = companyHandler.FindById(id.GetValueOrDefault());

            return View(company);
        }
        [HttpPost]
        public ActionResult Edit(Company model, HttpPostedFileBase logo)
        {
            var company = companyHandler.FindById(model.Id);
            company.Title = model.Title;
            company.Contact = model.Contact;
            company.NTN_STN = model.NTN_STN;
            company.Email = model.Email;
            company.Adress = model.Adress;
            if (logo != null)
            {
                company.LogoUrl = model.LogoUrl = FileManager.SaveImage(logo);
            }

            companyHandler.Update(company);
            return RedirectToAction("RegisterConfirmation", new { id = model.Id });
        }

         
    }
}