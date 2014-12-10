using System.Web;
using System.Web.Mvc;

namespace SEWebiste_OwenAttard
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}