using iFinco.BLL.Handler;
using iFinco.DAL;
using iFinco.DAL.Enum;
using iFinco.UI.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using iFinco.UI.Models;

namespace iFinco.UI.Areas.CompanyManager.Controllers
{
    [Authorize]
    public class TemplateDesignerController : Controller
    {
        iFincoDBEntities context;
        TemplateHandler tempHandler;
        PlaceholderHandler placeholderHandler;
        public TemplateDesignerController()
        {
            context = new iFincoDBEntities();
            tempHandler = new TemplateHandler(context);
            placeholderHandler = new PlaceholderHandler(context);
        }
        // GET: CompanyManager/TemplateDesigner
        public ActionResult Index()
        {
            var list = tempHandler.List.Where(x => x.IsActive && x.CompanyId == User.Identity.GetCompanyId() && x.BranchId == User.Identity.GetBranchId());
            return View(list);
        }
        public ActionResult Create(int? id)
        {
            InvoiceTemplate invoiceTemplete = new InvoiceTemplate();
            if (id != null && id > 0)
            {
                invoiceTemplete.TemplateType = id;
                var template = tempHandler.List.Where(x => x.IsActive && x.TemplateType == id && x.CompanyId == User.Identity.GetCompanyId() && x.BranchId == User.Identity.GetBranchId()).FirstOrDefault();
                if (template != null)
                {
                    return RedirectToAction("Edit", new { id = template.Id });
                }
                ViewBag.placeholdr = placeholderHandler.List.Where(x => x.IsActive && x.TemplateType == id);
            }
            InvoiceTempleteVM model = new InvoiceTempleteVM();
            model.InvoiceTemplate = invoiceTemplete;
            return View(model);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(InvoiceTempleteVM model)
        {
             
            string name = ((TemplateType)model.InvoiceTemplate.TemplateType).ToString();
            string path = FileManager.CreateFile(name, model.content);
            InvoiceTemplate temp = new InvoiceTemplate();
            temp.FileUrl = path;
            temp.TemplateType = model.InvoiceTemplate.TemplateType;
            temp.IsActive = true;
            temp.CreatedOn = DateTime.UtcNow;
            temp.CreatedBy = User.Identity.GetCustomId();
            temp.CompanyId = User.Identity.GetCompanyId().GetValueOrDefault();
            temp.BranchId = User.Identity.GetBranchId().GetValueOrDefault();
            tempHandler.Add(temp);
            return RedirectToAction("Index");
        }
        public ActionResult Edit(long? id)
        {
            if (id == null || id < 0)
            {
                return View("error");
            }
            InvoiceTempleteVM invoiceTempleteVM = new InvoiceTempleteVM();
            var template = tempHandler.List.Where(x => x.IsActive && x.Id == id && x.CompanyId == User.Identity.GetCompanyId() && x.BranchId == User.Identity.GetBranchId()).FirstOrDefault();
            invoiceTempleteVM.InvoiceTemplate = template;
            invoiceTempleteVM.content = FileManager.ReadFile(template.FileUrl);
            ViewBag.placeholdr = placeholderHandler.List.Where(x => x.IsActive && x.TemplateType == template.TemplateType);
            return View(invoiceTempleteVM);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(InvoiceTempleteVM model)
        {
            var temp = tempHandler.FindById(model.InvoiceTemplate.Id);
            temp.FileUrl = FileManager.UpdateFile(model.InvoiceTemplate.FileUrl, model.content);
            temp.TemplateType = model.InvoiceTemplate.TemplateType;
            temp.ModifiedOn = DateTime.UtcNow;
            temp.ModifiedBy = User.Identity.GetCustomId();
            tempHandler.Update(temp);
            return RedirectToAction("index");
        }

        [HttpGet]
        public ActionResult IsFileAlreadyExist(long id)
        {
            var template = tempHandler.List.Where(x => x.IsActive && x.TemplateType == id && x.CompanyId == User.Identity.GetCompanyId() && x.BranchId == User.Identity.GetBranchId());
            return Json(template, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public ActionResult SearchByTempType(long id)
        {
            var placeholdr = placeholderHandler.List.Where(x => x.TemplateType == id);
            return Json(placeholdr, JsonRequestBehavior.AllowGet);

        }
    }
}