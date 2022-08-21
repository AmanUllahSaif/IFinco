using iFinco.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iFinco.BLL.Handler
{
    public class TemplateHandler : GenericRepository<InvoiceTemplate>
    {
         
            private iFincoDBEntities _context;
            public TemplateHandler(iFincoDBEntities context) : base(context)
            {
                _context = context;
            }
        }
    }