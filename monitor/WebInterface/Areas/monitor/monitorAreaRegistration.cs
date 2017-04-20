using System.Web.Mvc;

namespace WebInterface.Areas.monitor
{
    public class monitorAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "monitor";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "monitor_default",
                "monitor/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}