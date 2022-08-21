using iFinco.BLL.Handler;
using iFinco.DAL;
using iFinco.UI.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;
using iFinco.UI.Models;
using VNS.Accounts;

namespace iFinco.UI.Areas.Admin.Controllers
{
    [Authorize(Roles ="Admin")]
    public class CompanyController : Controller
    {
        iFincoDBEntities context;
        CompanyHandler companyHandler;
        // GET: Admin/Company

        public CompanyController()
        {
            context = new iFincoDBEntities();
            companyHandler = new CompanyHandler(context);
        }
        public ActionResult Index(CompanySearchModel model,int page=1)
        {
            var list = companyHandler.List.Where(x => x.IsActive);
            if (!string.IsNullOrEmpty(model.Title))
            {
                list = list.Where(x => x.Title.ToLower().Contains(model.Title.ToLower()));
            }
            else if (!string.IsNullOrEmpty(model.Email))
            {
                list = list.Where(x => x.Email.ToLower().Contains(model.Email.ToLower()));
            }

            model.Data = list.OrderByDescending(x => x.Id).ToPagedList(page, PagingManager.GetPageSize());
            return View(model);
        }


        [HttpGet]
        public ActionResult Edit(long id)
        {
            var company = companyHandler.FindById(id);
            return View(company);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Company model, HttpPostedFileBase logo)
        {
            var company = companyHandler.FindById(model.Id);
            company.Title = model.Title;
            company.Contact = model.Contact;
            company.NTN_STN = model.NTN_STN;
            company.Email = model.Email;
            company.Adress = model.Adress;
            company.IsApproved = model.IsApproved;
            company.IsContactConfirmed = model.IsContactConfirmed;
            company.IsEmailConfirmed = model.IsEmailConfirmed;
            if (logo != null)
            {
                company.LogoUrl = model.LogoUrl = FileManager.SaveImage(logo);
            }
            companyHandler.Update(company);
            return RedirectToAction("Index");
        }

        public PartialViewResult Detail(long? id)
        {
            if (id == null || id < 1)
            {

            }
            var company = companyHandler.FindById(id.GetValueOrDefault());
            return PartialView("_DetailPartial", company);
        }

        public ActionResult Delete(long? id)
        {
            if (id == null || id < 1)
            {
                return HttpNotFound();
            }
            var company = companyHandler.FindById(id.GetValueOrDefault());
            company.IsActive = false;
            companyHandler.Update(company);
            return RedirectToAction("index");
        }

        public ActionResult Status(long? id)
        {
            if (id == null || id < 1)
            {
                return HttpNotFound();
            }
            var company = companyHandler.FindById(id.GetValueOrDefault());
            company.IsApproved = !company.IsApproved;
            Branch branch = new Branch();
            branch.Title = company.Title + "(Head Office)";
            branch.Status = (int)DAL.Enum.Status.Approve;
            branch.Contact = company.Contact;
            branch.Adress = company.Adress;
            branch.CreatedOn = DateTime.UtcNow;
            branch.IsActive = true;
            branch.IsHeadOffice = true;
            company.Branches.Add(branch);
            companyHandler.Update(company);
            if (company.IsApproved)
            {
                AccountsStartUpManager.CreateSetupAccounts(company.Id, branch.Id);
            }
            if (company.IsApproved)
            {
                string callbackUrl = Url.Action("Register", "Account", new { area = "", i = company.Id, e = EncryptionManager.HashText(company.Email, company.SecurityStamp) }, protocol: Request.Url.Scheme);
                string subject = "Company Approved";
                string message = "Congrats! your company have successfully Approved. Now you can register as owner please click <a href='" + callbackUrl + "'> here</a> for registration";
                MailManager.SendMail(company.Email, message, subject);
            }
            return RedirectToAction("index");
        }
    }
}
