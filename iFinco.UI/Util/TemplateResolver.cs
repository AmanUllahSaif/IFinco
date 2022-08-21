using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Text;
using iFinco.DAL;
using iFinco.DAL.Enum;
using System.Transactions;
using iFinco.UI.Models;
using iFinco.BLL.Handler;

namespace iFinco.UI.Util
{
    public class TemplateResolver
    {
        public static string ResolveSaleInovice(Sale model, string company, string branch)
        {
            string path = HttpContext.Current.Server.MapPath(@"~\Tamplats\invoice.txt");
            var dbtemplate = new TemplateHandler(new iFincoDBEntities()).List.FirstOrDefault(x => x.IsActive && x.TemplateType == (int)TemplateType.Invoice && x.CompanyId == model.CompanyId && x.BranchId == model.BranchId);
            if (dbtemplate != null)
            {
                path = HttpContext.Current.Server.MapPath(dbtemplate.FileUrl);
            }
            string template = File.ReadAllText(path);
            if (template.Contains("[[CompanyName]]"))
            {
                template = template.Replace("[[CompanyName]]", company);
            }
            if (template.Contains("[[BranchName]]"))
            {
                template = template.Replace("[[BranchName]]", branch);
            }
            if (template.Contains("[[invoiceType]]"))
            {
                template = template.Replace("[[invoiceType]]", "Sale Invoice");
            }
            if (template.Contains("[[orderNumb]]"))
            {
                template = template.Replace("[[orderNumb]]", model.InvoiceNo);
            }
            if (template.Contains("[[discount]]"))
            {
                template = template.Replace("[[discount]]", model.Discount.GetValueOrDefault().ToString());
            }
            if (template.Contains(" [[date]]"))
            {
                template = template.Replace("[[date]]", model.Date.ToString("dd/MM/yyyy"));
            }
            if (template.Contains("[[total]]"))
            {
                template = template.Replace("[[total]]", (model.Total + model.Discount).ToString());
            }

            if (template.Contains("[[gTotal]]"))
            {
                template = template.Replace("[[gTotal]]", model.Total.ToString());
            }
            if (template.Contains("[[Paid]]"))
            {
                template = template.Replace("[[Paid]]", model.PaidAmount.ToString());
            }
            if ((template.Contains("[[Paid]]")))
            {
                string customerInfo = "";
                if (model.PartyId == null)
                {
                    customerInfo = "Walkin";
                }
                else
                {
                    customerInfo = model.Party.Title;
                }
                if (model.ShippingDetails != null && model.ShippingDetails.Count > 0)
                {
                    var shipinfo = model.ShippingDetails.FirstOrDefault(x => x.IsActive);
                    customerInfo = shipinfo.Name + "<br />" + shipinfo.Address + "<br /> <b>#" + shipinfo.ShipmentNo + "<b>";
                }
                template = template.Replace("[[billedTo]]", customerInfo);

            }
            else
            {
                template = template.Replace("[[billedTo]]", "Walkin");
            }
            {

            }
            if (template.Contains("[[Table]]"))
            {
                string table = "<table class='table table-condensed'>"
                             + "<thead>"
                                 + "<tr>"
                                     + "<td><strong>Item</strong></td>"
                                     + "<td class='text-center'><strong>Price</strong></td>"
                                     + "<td class='text-center'><strong>Quantity</strong></td>"
                                     + "<td class='text-right'><strong>Discount %</strong></td>"
                                     + "<td class='text-right'><strong>Totals</strong></td>"
                                + " </tr>"
                             + "</thead>"
                             + "<tbody>";
                foreach (var item in model.SaleDetails)
                {
                    var total = item.Quantity * item.Price;
                    if (item.Discount != null)
                    {
                        total -= (total * item.Discount.GetValueOrDefault()) / 100;
                    }
                    table += "<tr>"
                                    + "<td>" + item.Product.Title + "</td>"
                                    + "<td class='text-center'>" + item.Price.ToString("0.##") + " </td>"
                                    + "<td class='text-center'>" + item.Quantity + "</td>"
                                    + "<td class='text-right'>" + item.Discount + "</td>"
                                     + "<td class='text-right'>" + total.ToString("0.##") + "</td>"
                                + "</tr>";
                }
                table += "</tbody> </table>";
                template = template.Replace("[[Table]]", table);
            }

            if (template.Contains("[[paymentMethod]]"))
            {
                template = template.Replace("[[paymentMethod]]", ((PaymentType)model.PaymentType).ToString());
            }
            return template;
        }
        public static string ResolvePurchaseInovice(Purchase model, string company, string branch)
        {
            string path = HttpContext.Current.Server.MapPath(@"~\Tamplats\invoice.txt");
            var dbtemplate = new TemplateHandler(new iFincoDBEntities()).List.FirstOrDefault(x => x.IsActive && x.TemplateType == (int)TemplateType.Invoice && x.CompanyId == model.CompanyId && x.BranchId == model.BranchId);
            if (dbtemplate != null)
            {
                path = HttpContext.Current.Server.MapPath(dbtemplate.FileUrl);
            }
            string template = File.ReadAllText(path);
            if (template.Contains("[[CompanyName]]"))
            {
                template = template.Replace("[[CompanyName]]", company);
            }
            if (template.Contains("[[BranchName]]"))
            {
                template = template.Replace("[[BranchName]]", branch);
            }
            if (template.Contains("[[invoiceType]]"))
            {
                template = template.Replace("[[invoiceType]]", "Purchase Invoice");
            }
            if (template.Contains("[[orderNumb]]"))
            {
                template = template.Replace("[[orderNumb]]", model.InvoiceNo);
            }
            if (template.Contains("[[discount]]"))
            {
                template = template.Replace("[[discount]]", model.Discount.GetValueOrDefault().ToString());
            }
            if (template.Contains(" [[date]]"))
            {
                template = template.Replace("[[date]]", model.Date.ToString("dd/MM/yyyy"));
            }
            if (template.Contains("[[total]]"))
            {
                template = template.Replace("[[total]]", (model.Total + model.Discount).ToString());
            }
            if (template.Contains("[[Paid]]"))
            {
                template = template.Replace("[[Paid]]", (model.PaidAmount).ToString());
            }
            if ((template.Contains("[[billedTo]]")))
            {
                string customerInfo = "";
                if (model.PartyId == null)
                {
                    customerInfo = "Walkin";
                }
                else
                {
                    customerInfo = model.Party.Title;
                }
                if (model.ShippingDetails != null && model.ShippingDetails.Count > 0)
                {
                    var shipinfo = model.ShippingDetails.FirstOrDefault(x => x.IsActive);
                    customerInfo = shipinfo.Name + "<br />" + shipinfo.Address + "<br /> <b>#" + shipinfo.ShipmentNo + "<b>";
                }
                template = template.Replace("[[billedTo]]", customerInfo);
            }

            if (template.Contains("[[gTotal]]"))
            {
                template = template.Replace("[[gTotal]]", model.Total.ToString());
            }
            if (template.Contains("[[Table]]"))
            {
                string table = "<table class='table table-condensed'>"
                             + "<thead>"
                                 + "<tr>"
                                     + "<td><strong>Item</strong></td>"
                                     + "<td class='text-center'><strong>Price</strong></td>"
                                     + "<td class='text-center'><strong>Quantity</strong></td>"
                                     + "<td class='text-right'><strong>Discount %</strong></td>"
                                     + "<td class='text-right'><strong>Totals</strong></td>"
                                + " </tr>"
                             + "</thead>"
                             + "<tbody>";
                foreach (var item in model.PurchaseDetails)
                {
                    var total = item.Quantity * item.Price;
                    if (item.Discount != null)
                    {
                        total -= (total * item.Discount.GetValueOrDefault()) / 100;
                    }
                    table += "<tr>"
                                    + "<td> " + item.Product.Title + "</td>"
                                    + "<td class='text-center'>" + item.Price.ToString("0.##") + "</td>"
                                    + "<td class='text-center'>" + item.Quantity + "</td>"
                                    + "<td class='text-right'>" + item.Discount + "</td>"
                                     + "<td class='text-right'>" + total.ToString("0.##") + "</td>"
                                + "</tr>";
                }
                table += "</tbody> </table>";
                template = template.Replace("[[Table]]", table);
            }

            if (template.Contains("[[paymentMethod]]"))
            {
                template = template.Replace("[[paymentMethod]]", ((PaymentType)model.PaymentType).ToString());
            }
            return template;
        }
        public static string ResolveSaleReturnInovice(SaleReturn model, string company, string branch)
        {
            string path = HttpContext.Current.Server.MapPath(@"~\Tamplats\invoice.txt");
            var dbtemplate = new TemplateHandler(new iFincoDBEntities()).List.FirstOrDefault(x => x.IsActive && x.TemplateType == (int)TemplateType.Invoice && x.CompanyId == model.CompanyId && x.BranchId == model.BranchId);
            if (dbtemplate != null)
            {
                path = HttpContext.Current.Server.MapPath(dbtemplate.FileUrl);
            }
            string template = File.ReadAllText(path);
            if (template.Contains("[[CompanyName]]"))
            {
                template = template.Replace("[[CompanyName]]", company);
            }
            if (template.Contains("[[BranchName]]"))
            {
                template = template.Replace("[[BranchName]]", branch);
            }
            if (template.Contains("[[invoiceType]]"))
            {
                template = template.Replace("[[invoiceType]]", "Sale Return Invoice");
            }
            if (template.Contains("[[orderNumb]]"))
            {
                template = template.Replace("[[orderNumb]]", model.InvoiceNo);
            }
            if (template.Contains("[[Paid]]"))
            {
                template = template.Replace("[[Paid]]", (model.PaidAmount).ToString());
            }
            if (template.Contains("[[discount]]"))
            {
                template = template.Replace("[[discount]]", model.Discount.GetValueOrDefault().ToString());
            }

            if (template.Contains(" [[date]]"))
            {
                template = template.Replace("[[date]]", model.Date.ToString("dd/MM/yyyy"));
            }
            if (template.Contains("[[total]]"))
            {
                template = template.Replace("[[total]]", (model.Discount + model.Total).ToString());
            }
            if ((template.Contains("[[billedTo]]")))
            {
                string customerInfo = "";
                if (model.PartyId == null)
                {
                    customerInfo = "Walkin";
                }
                else
                {
                    customerInfo = model.Party.Title;
                }
                template = template.Replace("[[billedTo]]", customerInfo);
            }

            if (template.Contains("[[gTotal]]"))
            {
                template = template.Replace("[[gTotal]]", model.Total.ToString());
            }
            if (template.Contains("[[Table]]"))
            {
                string table = "<table class='table table-condensed'>"
                             + "<thead>"
                                 + "<tr>"
                                     + "<td><strong>Item</strong></td>"
                                     + "<td class='text-center'><strong>Price</strong></td>"
                                     + "<td class='text-center'><strong>Quantity</strong></td>"
                                     + "<td class='text-right'><strong>Discount %</strong></td>"
                                     + "<td class='text-right'><strong>Total</strong></td>"
                                + " </tr>"
                             + "</thead>"
                             + "<tbody>";
                foreach (var item in model.SaleReturnDetails)
                {
                    var total = item.Quantity * item.Price;
                    if (item.Discount != null)
                    {
                        total -= (total * item.Discount.GetValueOrDefault()) / 100;
                    }
                    table += "<tr>"
                                    + "<td> " + item.Product.Title + " </td>"
                                    + "<td class='text-center'>" + item.Price.ToString("0.##") + "</td>"
                                    + "<td class='text-center'>" + item.Quantity + "</td>"
                                    + "<td class='text-right'>" + item.Discount + "</td>"
                                     + "<td class='text-right'>" + total.ToString("0.##") + "</td>"
                                + "</tr>";
                }
                table += "</tbody> </table>";
                template = template.Replace("[[Table]]", table);
            }

            if (template.Contains("[[paymentMethod]]"))
            {
                template = template.Replace("[[paymentMethod]]", ((PaymentType)model.PaymentType).ToString());
            }
            return template;
        }
        public static string ResolvePurchaseReturnInovice(PurchaseReturn model, string company, string branch)
        {
            string path = HttpContext.Current.Server.MapPath(@"~\Tamplats\invoice.txt");
            var dbtemplate = new TemplateHandler(new iFincoDBEntities()).List.FirstOrDefault(x => x.IsActive && x.TemplateType == (int)TemplateType.Invoice && x.CompanyId == model.CompanyId && x.BranchId == model.BranchId);
            if (dbtemplate != null)
            {
                path = HttpContext.Current.Server.MapPath(dbtemplate.FileUrl);
            }
            string template = File.ReadAllText(path);
            if (template.Contains("[[CompanyName]]"))
            {
                template = template.Replace("[[CompanyName]]", company);
            }
            if (template.Contains("[[BranchName]]"))
            {
                template = template.Replace("[[BranchName]]", branch);
            }
            if (template.Contains("[[invoiceType]]"))
            {
                template = template.Replace("[[invoiceType]]", "Purchase Return Invoice");
            }
            if (template.Contains("[[orderNumb]]"))
            {
                template = template.Replace("[[orderNumb]]", model.InvoiceNo);
            }
            if (template.Contains("[[discount]]"))
            {
                template = template.Replace("[[discount]]", model.Discount.GetValueOrDefault().ToString());
            }
            if (template.Contains("[[Paid]]"))
            {
                template = template.Replace("[[Paid]]", (model.PaidAmount).ToString());
            }
            if (template.Contains(" [[date]]"))
            {
                template = template.Replace("[[date]]", model.Date.ToString("dd/MM/yyyy"));
            }
            if (template.Contains("[[total]]"))
            {
                template = template.Replace("[[total]]", (model.Discount + model.Total).ToString());
            }
            if ((template.Contains("[[billedTo]]")))
            {
                string customerInfo = "";
                if (model.PartyId == null)
                {
                    customerInfo = "Walkin";
                }
                else
                {
                    customerInfo = model.Party.Title;
                }
                template = template.Replace("[[billedTo]]", customerInfo);
            }
            if (template.Contains("[[gTotal]]"))
            {
                template = template.Replace("[[gTotal]]", model.Total.ToString());
            }
            if (template.Contains("[[Table]]"))
            {
                string table = "<table class='table table-condensed'>"
                             + "<thead>"
                                 + "<tr>"
                                     + "<td><strong>Item</strong></td>"
                                     + "<td class='text-center'><strong>Price</strong></td>"
                                     + "<td class='text-center'><strong>Quantity %</strong></td>"
                                     + "<td class='text-right'><strong>Discount</strong></td>"
                                     + "<td class='text-right'><strong>Totals</strong></td>"
                                + " </tr>"
                             + "</thead>"
                             + "<tbody>";
                foreach (var item in model.PurchaseReturnDetails)
                {
                    var total = item.Quantity * item.Price;
                    if (item.Discount != null)
                    {
                        total -= (total * item.Discount.GetValueOrDefault()) / 100;

                    }
                    table += "<tr>"
                                    + "<td>" + item.Product.Title + " </td>"
                                    + "<td class='text-center'>" + item.Price.ToString("0.##") + "</td>"
                                    + "<td class='text-center'>" + item.Quantity + " </td>"
                                    + "<td class='text-right'>" + item.Discount + " </td>"
                                     + "<td class='text-right'>" + total.ToString("0.##") + "</td>"
                                + "</tr>";
                }
                table += "</tbody> </table>";
                template = template.Replace("[[Table]]", table);
            }

            if (template.Contains("[[paymentMethod]]"))
            {
                template = template.Replace("[[paymentMethod]]", ((PaymentType)model.PaymentType).ToString());
            }
            return template;
        }
        public static string ResolveTransactionReportTemplate(List<VNS.Accounts.DAL.Transaction> model, string company, DateTime from, DateTime to, string reportType, string branch)
        {
            string path = HttpContext.Current.Server.MapPath(@"~\Tamplats\report.txt");

            string template = File.ReadAllText(path);
            if (template.Contains("[[CompanyName]]"))
            {
                template = template.Replace("[[CompanyName]]", company);
            }
            if (template.Contains("[[BranchName]]"))
            {
                template = template.Replace("[[BranchName]]", branch);
            }
            if (template.Contains("[[invoiceType]]"))
            {
                template = template.Replace("[[invoiceType]]", "Report Invoice");
            }
            if (template.Contains("[[reportType]]"))
            {
                template = template.Replace("[[reportType]]", reportType);
            }

            if (template.Contains("[[date]]"))
            {
                string date = from.ToString("dd/MM/yyy") + "-----" + to.ToString("dd/MM/yyyy");
                template = template.Replace("[[date]]", date);
            }


            if (template.Contains("[[Table]]"))
            {
                string table = "<table class='table table-condensed'>"
                           + "<thead>"
                               + "<tr>"
                                    + "<td class='text-center'><strong>Accounts</strong></td>"
                                   + "<td class='text-center'><strong>Detail</strong></td>"
                                   + "<td class='text-center'><strong>Dr</strong></td>"
                                   + "<td class='text-center'><strong>Cr</strong></td>"

                              + " </tr>"
                           + "</thead>"
                           + "<tbody>";
                foreach (var item in model)
                {
                    item.AccountDetails = item.AccountDetails.Where(x => x.IsActive).ToList();
                    table += "<tr>"
                                   + "<td  class='text-center'>";
                    foreach (var det in item.AccountDetails)
                    {
                        table += det.Account.Name + "<br/>";
                    }


                    table += "</td><td  class='text-center'>" + item.Narration + "</td>";
                    table += "<td  class='text-center'>";
                    foreach (var det in item.AccountDetails)
                    {
                        if (det.Dr != null)
                        {
                            table += det.Dr.GetValueOrDefault().ToString("0.##");

                        }
                        table += "<br/>";
                    }
                    table += "</td><td  class='text-center'>";
                    foreach (var det in item.AccountDetails)
                    {
                        if (det.Cr != null)
                        {
                            table += det.Cr.GetValueOrDefault().ToString("0.##");
                        }
                        table += "<br/>";
                    }
                    table += "</td></tr>";


                }
                table += "</tbody> </table>";
                template = template.Replace("[[Table]]", table);
            }


            return template;
        }
        public static string ResolveProfitAndLostStatementReportTemplate(ProfitAndLossStatementSearchModel model, string company, DateTime from, DateTime to, string reportType, string branch)
        {
            string path = HttpContext.Current.Server.MapPath(@"~\Tamplats\Statement.txt");
            
            string template = File.ReadAllText(path);
            if (template.Contains("[[CompanyName]]"))
            {
                template = template.Replace("[[CompanyName]]", company);
            }
            if (template.Contains("[[BranchName]]"))
            {
                template = template.Replace("[[BranchName]]", branch);
            }
            if (template.Contains("[[invoiceType]]"))
            {
                template = template.Replace("[[invoiceType]]", "Report Invoice");
            }
            if (template.Contains("[[ReportType]]"))
            {
                template = template.Replace("[[ReportType]]", reportType);
            }

            if (template.Contains("[[date]]"))
            {
                string date = from.ToString("dd/MM/yyy") + "-----" + to.ToString("dd/MM/yyyy");
                template = template.Replace("[[date]]", date);
            }

            if (template.Contains("[[ExpenseTable]]"))
            {
                string table = "<table class='table table-condensed'>"
                           + "<thead>"
                               + "<tr>"
                                    + "<th><strong>Account Name</strong></th>"
                                   + "<th><strong>Balance</strong></th>"
                              + " </tr>"
                           + "</thead>"
                           + "<tbody>";
                foreach (var item in model.expenseAccount)
                {
                    table += "<tr>";
                    table += "<td>" + item.Name + "</td>";
                    table += "<td>";
                    if (item.Balance < 0)
                    {
                        item.Balance = -1 * item.Balance;
                        table += item.Balance;
                    }
                    else
                    {
                        table += item.Balance;
                    }
                    table += "</td></tr>";
                    table += "<tr>";
                    table += "<td><b>Total Expense</b></td>";
                    table += "<td><b>" + model.expenseAccount.Sum(x => x.Balance) + "</b></td>";
                    table += "</tr>";
                }
                table += "</table>";
                template = template.Replace("[[ExpenseTable]]", table);
            }
            if (template.Contains("[[RevneueTable]]"))
            {
                string table = "<table class='table table-condensed'>"
                           + "<thead>"
                               + "<tr>"
                                    + "<th><strong>Account Name</strong></th>"
                                   + "<th><strong>Balance</strong></th>"
                              + " </tr>"
                           + "</thead>"
                           + "<tbody>";
                foreach (var item in model.revenueAccount)
                {
                    table += "<tr>";
                    table += "<td>" + item.Name + "</td>";
                    table += "<td>";
                    if (item.Balance < 0)
                    {
                        item.Balance = -1 * item.Balance;
                        table += item.Balance;
                    }
                    else
                    {
                        table += item.Balance;
                    }
                    table += "</td></tr>";
                    table += "<tr>";
                    table += "<td><b>Total Revenue</b></td>";
                    table += "<td><b>" + model.revenueAccount.Sum(x => x.Balance) + "</b></td>";
                    table += "</tr>";
                }
                table += "</table>";
                template = template.Replace("[[RevneueTable]]", table);
            }
            if (template.Contains("[[NetProfitOrLoss]]"))
            {
                string table = "<tr>";
                decimal netIncome = model.revenueAccount.Sum(x => x.Balance) - model.expenseAccount.Sum(x => x.Balance);
                if (netIncome > 0)
                {
                    table += "<td><b> Net Profit </b></td>"
                             + "<td><b>" + netIncome + "</b></td>";
                }
                else
                {
                    table += "<td><b> Net Loss </b></td>"
                             + "<td><b>" + netIncome + "</b></td>";
                }
                table += "</tr>";
                template = template.Replace("[[NetProfitOrLoss]]", table);
            }
            if (template.Contains("[[User]]"))
            {
                //table += User
            }
            return template;
        }
        public static string ResolvePartyReport(List<Party> model, string company, DateTime from, DateTime to, string reportType, string branch)
        {
            string path = HttpContext.Current.Server.MapPath(@"~\Tamplats\report.txt");
             
            string template = File.ReadAllText(path);
            if (template.Contains("[[CompanyName]]"))
            {
                template = template.Replace("[[CompanyName]]", company);
            }
            if (template.Contains("[[BranchName]]"))
            {
                template = template.Replace("[[BranchName]]", branch);
            }

            if (template.Contains("[[reportType]]"))
            {
                template = template.Replace("[[reportType]]", reportType);
            }

            if (template.Contains("[[date]]"))
            {
                string date = from.ToString("dd/MM/yyy") + "-----" + to.ToString("dd/MM/yyyy");
                template = template.Replace("[[date]]", date);
            }


            if (template.Contains("[[Table]]"))
            {
                string table = "<table class='table table-condensed'>"
                           + "<thead>"
                               + "<tr>"
                                    + "<td class='text-center'><strong>Parties</strong></td>"
                                   + "<td class='text-center'><strong>Balance</strong></td>"


                              + " </tr>"
                           + "</thead>"
                           + "<tbody>";
                foreach (var item in model)
                {

                    table += "<tr>"
                                   + "<td  class='text-center'>";
                    foreach (var det in model)
                    {
                        table += det.Title + "<br/>";
                    }



                    table += "<td  class='text-center'>";
                    foreach (var det in model)
                    {
                        if (det.Balance < 0)
                        {
                            table += "(" + det.Balance * -1 + ")";

                        }
                        else
                        {

                            table += det.Balance.ToString();


                        }


                    }

                }
                table += "</td></tr>";



                table += "</tbody> </table>";
                template = template.Replace("[[Table]]", table);
            }


            return template;
        }
        public static string ResolveGeneralInvoice(Generalinvoice model, string company, DateTime from, DateTime to, string branch, string user)
        {
            string path = HttpContext.Current.Server.MapPath(@"~\Tamplats\GeneralVoucher.txt");
            var dbtemplate = new TemplateHandler(new iFincoDBEntities()).List.FirstOrDefault(x => x.IsActive && x.TemplateType == (int)TemplateType.GeneralVoucher && x.CompanyId == model.CompanyId && x.BranchId == model.BranchId);
            if (dbtemplate != null)
            {
                path = HttpContext.Current.Server.MapPath(dbtemplate.FileUrl);
            }
            string template = File.ReadAllText(path);
            if (template.Contains("[[CompanyName]]"))
            {
                template = template.Replace("[[CompanyName]]", company);
            }
            if (template.Contains("[[BranchName]]"))
            {
                template = template.Replace("[[BranchName]]", branch);
            }
            if (template.Contains("[[VoucherType]]"))
            {
                switch ((InvoiceType)model.Type)
                {
                    case InvoiceType.Recived:
                        template = template.Replace("[[VoucherType]]", "Recived Receipt");
                        break;
                    case InvoiceType.Paid:
                        template = template.Replace("[[VoucherType]]", "Paid Receipt");

                        break;
                    case InvoiceType.Expense:
                        template = template.Replace("[[VoucherType]]", "Expense");
                        break;
                    default:
                        break;
                }
            }
            if (template.Contains("[[InvoiceNumber]]"))
            {
                template = template.Replace("[[InvoiceNumber]]", model.InvoiceNum.ToString());
            }
            if (template.Contains("[[Amount]]"))
            {
                template = template.Replace("[[Amount]]", model.Amount.ToString());
            }

            if (template.Contains("[[Date]]"))
            {

                template = template.Replace("[[Date]]", model.Date.ToString("dd/MM/yyyy"));
            }
            if (template.Contains("[[Party]]"))
            {


                string partyInfo = model.Party.Title + "<br />" + model.Party.Contact + "<br /> <b>#" + model.Party.Adress + "<b>";

                template = template.Replace("[[Party]]", partyInfo);
            }
            if (template.Contains("[[User]]"))
            {
                template = template.Replace("[[User]]", user);
            }
            return template;

        }
        public static string ResolveSaleReport(List<Sale> model, string company, DateTime from, DateTime to, string reportType, string branch)
        {
            string path = HttpContext.Current.Server.MapPath(@"~\Tamplats\report.txt");

            string template = File.ReadAllText(path);
            if (template.Contains("[[CompanyName]]"))
            {
                template = template.Replace("[[CompanyName]]", company);
            }
            if (template.Contains("[[BranchName]]"))
            {
                template = template.Replace("[[BranchName]]", branch);
            }
            if (template.Contains("[[invoiceType]]"))
            {
                template = template.Replace("[[invoiceType]]", "Report Invoice");
            }
            if (template.Contains("[[reportType]]"))
            {
                template = template.Replace("[[reportType]]", reportType);
            }

            if (template.Contains("[[date]]"))
            {
                string date = from.ToString("dd/MM/yyy") + "-----" + to.ToString("dd/MM/yyyy");
                template = template.Replace("[[date]]", date);
            }


            if (template.Contains("[[Table]]"))
            {
                string table = "<table class='table table-condensed'>"
                           + "<thead>"
                               + "<tr>"
                                    + "<td class='text-center'><strong>Invoice No</strong></td>"
                                   + "<td class='text-center'><strong>Parties</strong></td>"
                                   + "<td class='text-center'><strong>Date</strong></td>"
                                   + "<td class='text-center'><strong>Total</strong></td>"
                                   + "<td class='text-center'><strong>Discount</strong></td>"
                              + " </tr>"
                           + "</thead>"
                           + "<tbody>";
                foreach (var item in model)
                {
                    item.SaleDetails = item.SaleDetails.Where(x => x.IsActive).ToList();
                    table += "<tr>"
                                   + "<td  class='text-center'>";
                    table += item.InvoiceNo + "<br/>";

                    table += "</td><td  class='text-center'>";
                    if (item.PartyId != null)
                    {
                        table += item.Party.Title;
                    }
                    else
                    {
                        table += "Walking Customer";
                    }
                    table += "</td>";

                    table += "<td  class='text-center'>";
                    table += item.Date.ToShortDateString();
                    table += "</td><td  class='text-center'>";
                    table += item.Total;
                    table += "</td>";
                    table += "</td><td  class='text-center'>";
                    table += item.Discount;
                    table += "</td>";
                    table += "</tr>";


                }
                table += "</tbody> </table>";
                template = template.Replace("[[Table]]", table);
            }


            return template;
        }
        public static string ResolveSaleReturnReport(List<SaleReturn> model, string company, DateTime from, DateTime to, string reportType, string branch)
        {
            string path = HttpContext.Current.Server.MapPath(@"~\Tamplats\report.txt");
            string template = File.ReadAllText(path);
            if (template.Contains("[[CompanyName]]"))
            {
                template = template.Replace("[[CompanyName]]", company);
            }
            if (template.Contains("[[BranchName]]"))
            {
                template = template.Replace("[[BranchName]]", branch);
            }
            if (template.Contains("[[invoiceType]]"))
            {
                template = template.Replace("[[invoiceType]]", "Report Invoice");
            }
            if (template.Contains("[[reportType]]"))
            {
                template = template.Replace("[[reportType]]", reportType);
            }

            if (template.Contains("[[date]]"))
            {
                string date = from.ToString("dd/MM/yyy") + "-----" + to.ToString("dd/MM/yyyy");
                template = template.Replace("[[date]]", date);
            }


            if (template.Contains("[[Table]]"))
            {
                string table = "<table class='table table-condensed'>"
                           + "<thead>"
                               + "<tr>"
                                    + "<td class='text-center'><strong>Invoice No</strong></td>"
                                   + "<td class='text-center'><strong>Parties</strong></td>"
                                   + "<td class='text-center'><strong>Date</strong></td>"
                                   + "<td class='text-center'><strong>Total</strong></td>"
                                   + "<td class='text-center'><strong>Discount</strong></td>"
                              + " </tr>"
                           + "</thead>"
                           + "<tbody>";
                foreach (var item in model)
                {
                    item.SaleReturnDetails = item.SaleReturnDetails.Where(x => x.IsActive).ToList();
                    table += "<tr>"
                                   + "<td  class='text-center'>";
                    table += item.InvoiceNo + "<br/>";

                    table += "</td><td  class='text-center'>";
                    if (item.PartyId != null)
                    {
                        table += item.Party.Title;
                    }
                    else
                    {
                        table += "Walking Customer";
                    }
                    table += "</td>";

                    table += "<td  class='text-center'>";
                    table += item.Date.ToShortDateString();
                    table += "</td><td  class='text-center'>";
                    table += item.Total;
                    table += "</td>";
                    table += "</td><td  class='text-center'>";
                    table += item.Discount;
                    table += "</td>";
                    table += "</tr>";


                }
                table += "</tbody> </table>";
                template = template.Replace("[[Table]]", table);
            }


            return template;
        }
        public static string ResolvePurchaseReport(List<Purchase> model, string company, DateTime from, DateTime to, string reportType, string branch)
        {
            string path = HttpContext.Current.Server.MapPath(@"~\Tamplats\report.txt");
            string template = File.ReadAllText(path);
            if (template.Contains("[[CompanyName]]"))
            {
                template = template.Replace("[[CompanyName]]", company);
            }
            if (template.Contains("[[BranchName]]"))
            {
                template = template.Replace("[[BranchName]]", branch);
            }
            if (template.Contains("[[invoiceType]]"))
            {
                template = template.Replace("[[invoiceType]]", "Report Invoice");
            }
            if (template.Contains("[[reportType]]"))
            {
                template = template.Replace("[[reportType]]", reportType);
            }

            if (template.Contains("[[date]]"))
            {
                string date = from.ToString("dd/MM/yyy") + "-----" + to.ToString("dd/MM/yyyy");
                template = template.Replace("[[date]]", date);
            }


            if (template.Contains("[[Table]]"))
            {
                string table = "<table class='table table-condensed'>"
                           + "<thead>"
                               + "<tr>"
                                    + "<td class='text-center'><strong>Invoice No</strong></td>"
                                   + "<td class='text-center'><strong>Parties</strong></td>"
                                   + "<td class='text-center'><strong>Date</strong></td>"
                                   + "<td class='text-center'><strong>Total</strong></td>"
                                   + "<td class='text-center'><strong>Discount</strong></td>"
                              + " </tr>"
                           + "</thead>"
                           + "<tbody>";
                foreach (var item in model)
                {
                    item.PurchaseDetails = item.PurchaseDetails.Where(x => x.IsActive).ToList();
                    table += "<tr>"
                                   + "<td  class='text-center'>";
                    table += item.InvoiceNo + "<br/>";

                    table += "</td><td  class='text-center'>";
                    if (item.PartyId != null)
                    {
                        table += item.Party.Title;
                    }
                    else
                    {
                        table += "Walking Customer";
                    }
                    table += "</td>";

                    table += "<td  class='text-center'>";
                    table += item.Date.ToShortDateString();
                    table += "</td><td  class='text-center'>";
                    table += item.Total;
                    table += "</td>";
                    table += "</td><td  class='text-center'>";
                    table += item.Discount;
                    table += "</td>";
                    table += "</tr>";
                }
                table += "</tbody> </table>";
                template = template.Replace("[[Table]]", table);
            }


            return template;
        }
        public static string ResolvePurchaseReturnReport(List<PurchaseReturn> model, string company, DateTime from, DateTime to, string reportType, string branch)
        {
            string path = HttpContext.Current.Server.MapPath(@"~\Tamplats\Statement.txt");
            string template = File.ReadAllText(path);
            if (template.Contains("[[CompanyName]]"))
            {
                template = template.Replace("[[CompanyName]]", company);
            }
            if (template.Contains("[[BranchName]]"))
            {
                template = template.Replace("[[BranchName]]", branch);
            }
            if (template.Contains("[[invoiceType]]"))
            {
                template = template.Replace("[[invoiceType]]", "Report Invoice");
            }
            if (template.Contains("[[reportType]]"))
            {
                template = template.Replace("[[reportType]]", reportType);
            }

            if (template.Contains("[[date]]"))
            {
                string date = from.ToString("dd/MM/yyy") + "-----" + to.ToString("dd/MM/yyyy");
                template = template.Replace("[[date]]", date);
            }

            if (template.Contains("[[Table]]"))
            {
                string table = "<table class='table table-condensed'>"
                           + "<thead>"
                               + "<tr>"
                                    + "<td class='text-center'><strong>Invoice No</strong></td>"
                                   + "<td class='text-center'><strong>Parties</strong></td>"
                                   + "<td class='text-center'><strong>Date</strong></td>"
                                   + "<td class='text-center'><strong>Total</strong></td>"
                                   + "<td class='text-center'><strong>Discount</strong></td>"
                              + " </tr>"
                           + "</thead>"
                           + "<tbody>";
                foreach (var item in model)
                {
                    item.PurchaseReturnDetails = item.PurchaseReturnDetails.Where(x => x.IsActive).ToList();
                    table += "<tr>"
                                   + "<td  class='text-center'>";
                    table += item.InvoiceNo + "<br/>";

                    table += "</td><td  class='text-center'>";
                    if (item.PartyId != null)
                    {
                        table += item.Party.Title;
                    }
                    else
                    {
                        table += "Walking Customer";
                    }
                    table += "</td>";

                    table += "<td  class='text-center'>";
                    table += item.Date.ToShortDateString();
                    table += "</td><td  class='text-center'>";
                    table += item.Total;
                    table += "</td>";
                    table += "</td><td  class='text-center'>";
                    table += item.Discount;
                    table += "</td>";
                    table += "</tr>";
                }
                table += "</tbody> </table>";
                template = template.Replace("[[Table]]", table);
            }
            return template;
        }
    }
}



