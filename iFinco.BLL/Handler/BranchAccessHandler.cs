using iFinco.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iFinco.BLL.Handler
{
    public class BranchAccessHandler : GenericRepository<BranchAccess>
    {
        iFincoDBEntities _context;
        public BranchAccessHandler(iFincoDBEntities context) : base(context)
        {
            _context = context;
        }

        public void RemoveAllAccess(long uid)
        {
            var acess = List.Where(x => x.IsActive && x.UId == uid);
            foreach (var item in acess)
            {
                item.IsActive = false;
                Update(item);
            }
        }
    }
}
