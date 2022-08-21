using iFinco.UI.Util;
using System.Web;
using System.Web.Mvc;

namespace iFinco.UI
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new ExceptionHandler());
        }
    }
}
