using iFinco.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iFinco.BLL.Handler
{
    public class PurchaseReturnHandler : GenericRepository<PurchaseReturn>
    {
        private iFincoDBEntities _context;
        public PurchaseReturnHandler(iFincoDBEntities context) : base(context)
        {
            _context = context;
        }

        public string GenerateInvoiceNo(long ComapnyId, long? BranchId = null)
        {
            var purchaseReturn = _context.PurchaseReturns.Where(x => x.IsActive == true && x.CompanyId == ComapnyId && x.BranchId == BranchId).OrderByDescending(x => x.Id).FirstOrDefault();
            if (purchaseReturn == null)
            {
                return "1";
            }
            else
            {
                long invoiceNum = Convert.ToInt64(purchaseReturn.InvoiceNo);
                invoiceNum++;
                return invoiceNum.ToString();
            }
        }
    }

}
