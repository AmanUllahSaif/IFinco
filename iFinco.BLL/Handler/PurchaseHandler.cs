using iFinco.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iFinco.BLL.Handler
{
    public class PurchaseHandler : GenericRepository<Purchase>
    {
        private iFincoDBEntities _context;

        public PurchaseHandler(iFincoDBEntities context) : base(context)
        {
            _context = context;
        }

        public string GenerateInvoiceNo(long ComapnyId, long BranchId)
        {
            var purchase = _context.Purchases.Where(x => x.IsActive == true && x.CompanyId == ComapnyId && x.BranchId == BranchId).OrderByDescending(x => x.Id).FirstOrDefault();
            if (purchase == null)
            {
                return "1";
            }
            else
            {
                long invoiceNum = Convert.ToInt64(purchase.InvoiceNo);
                invoiceNum++;
                return invoiceNum.ToString();
            }
        }
        public Purchase FindByInvoiceNum(string InvoiceNumber, long companyId, long? branchId = null)
        {
            return _context.Purchases.SingleOrDefault(x => x.InvoiceNo.Equals(InvoiceNumber) && x.CompanyId == companyId && x.BranchId == branchId);
        }

    }
}
