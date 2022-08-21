using iFinco.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iFinco.BLL.Handler
{
    public class SaleHandler : GenericRepository<Sale>
    {
        private iFincoDBEntities _context;

        public SaleHandler(iFincoDBEntities context) : base(context)
        {
            _context = context;
        }

        public string GenerateInvoiceNo(long ComapnyId, long? BranchId = null)
        {
            var sale = _context.Sales.Where(x => x.IsActive == true && x.CompanyId == ComapnyId && x.BranchId == BranchId).OrderByDescending(x => x.Id).FirstOrDefault();
            if (sale == null)
            {
                return "1";
            }
            else
            {
                long invoiceNum = Convert.ToInt64(sale.InvoiceNo);
                invoiceNum++;
                return invoiceNum.ToString();
            }
        }

        public Sale FindByInvoiceNum(string InvoiceNumber, long companyId, long? branchId = null)
        {
            return _context.Sales.SingleOrDefault(x => x.InvoiceNo.Equals(InvoiceNumber) && x.CompanyId == companyId && x.BranchId == branchId);
        }
    }
}
