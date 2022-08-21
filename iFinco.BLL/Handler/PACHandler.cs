using iFinco.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iFinco.BLL.Handler
{

    public class PACHandler : GenericRepository<ProductAttributeCombination>
    {
        private iFincoDBEntities _context;
        public PACHandler(iFincoDBEntities context) : base(context)
        {
            _context = context;
        }

        public IEnumerable<ProductAttributeCombination> GetByProdId(long Id)
        {
            return List.Where(x => x.ProductId == Id && x.IsActive);
        }
    }
}
