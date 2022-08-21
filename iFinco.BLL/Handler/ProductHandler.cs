using iFinco.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iFinco.BLL.Handler
{
  public  class ProductHandler :GenericRepository<Product>
    {
        private iFincoDBEntities _context;
        public ProductHandler(iFincoDBEntities context):base (context)
        {
            _context = context;
        }
    }
}
