using iFinco.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iFinco.BLL.Handler
{
    public class BranchHandler : GenericRepository<Branch>
    {
        private iFincoDBEntities _context;
        public BranchHandler(iFincoDBEntities context) : base(context)
        {
            _context = context;
        }

        public Branch GetHeadOffice(long companyId)
        {
            return _context.Branches.FirstOrDefault(x => x.CompanyId == companyId && x.IsActive && x.IsHeadOffice);
        }
    }
}
