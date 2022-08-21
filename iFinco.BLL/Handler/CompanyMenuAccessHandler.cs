using iFinco.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iFinco.BLL.Handler
{
    public class CompanyMenuAccessHandler: GenericRepository<CompanyMenusAccess>
    {
        private iFincoDBEntities _context;
        public CompanyMenuAccessHandler(iFincoDBEntities context) : base(context)
        {
            _context = context;
        }

        public IEnumerable<CompanyMenusAccess> GetAccessByCompany(long companyId)
        {
            return List.Where(x => x.CompanyId == companyId && x.IsActive);
        }
    }
}
