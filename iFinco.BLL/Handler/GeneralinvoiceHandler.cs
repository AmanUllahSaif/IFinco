using iFinco.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VNS.Accounts.DAL;

namespace iFinco.BLL.Handler
{
    public class GeneralinvoiceHandler : GenericRepository<Generalinvoice>
    {
      private iFincoDBEntities _context;
        public GeneralinvoiceHandler(iFincoDBEntities context) : base(context)
        {
            _context = context;
        }
        public long GenerateInvoiceNo(long ComapnyId, long? BranchId = null)
        {
            var invoice = _context.Generalinvoices.Where(x => x.IsActive == true && x.CompanyId == ComapnyId && x.BranchId == BranchId).OrderByDescending(x => x.Id).FirstOrDefault();
            if (invoice == null)
            {
                return 1;
            }
            else
            {
                long invoiceNo = Convert.ToInt64(invoice.InvoiceNum);
                invoiceNo++;
                return invoiceNo;
            }
        }
    }
}
