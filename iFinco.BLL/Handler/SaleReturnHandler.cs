using iFinco.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iFinco.BLL.Handler
{
    public class SaleReturnHandler : GenericRepository<SaleReturn>
    {
        private iFincoDBEntities _context;
        public SaleReturnHandler(iFincoDBEntities context) : base(context)
        {
            _context = context;
        }

        public string GenerateInvoiceNo(long ComapnyId, long? BranchId = null)
        {
            var saleReturn = _context.SaleReturns.Where(x => x.IsActive == true && x.CompanyId == ComapnyId && x.BranchId == BranchId).OrderByDescending(x => x.Id).FirstOrDefault();
            if (saleReturn == null)
            {
                return "1";
            }
            else
            {
                long invoiceNum = Convert.ToInt64(saleReturn.InvoiceNo);
                invoiceNum++;
                return invoiceNum.ToString();
            }
        }
    }
}
