using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace AGLProgrammingTest
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapMvcAttributeRoutes();

            //routes.MapRoute(
            //    name: "Pets",
            //    url: "{controller}/{action}/{petType}",
            //    defaults: new { controller = "pets", action = "Get", petType = "Cat" }
            //);

        }
    }
}
