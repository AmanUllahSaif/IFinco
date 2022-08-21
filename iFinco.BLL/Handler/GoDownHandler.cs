using iFinco.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iFinco.BLL.Handler
{
    public class GoDownHandler : GenericRepository<GoDown>
    {
        private iFincoDBEntities _context;
        public GoDownHandler(iFincoDBEntities context) : base(context)
        {
            _context = context;
        }
    }
}
