using System.Web.Mvc;

namespace iFinco.UI.Areas.CompanyManager
{
    public class CompanyManagerAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "CompanyManager";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "CompanyManager_default",
                "CompanyManager/{controller}/{action}/{id}",
                new { controller="Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}