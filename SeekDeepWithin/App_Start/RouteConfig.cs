using System.Web.Mvc;
using System.Web.Routing;

namespace SeekDeepWithin
{
   public class RouteConfig
   {
      public static void RegisterRoutes (RouteCollection routes)
      {
         routes.IgnoreRoute ("{resource}.axd/{*pathInfo}");

         routes.MapRoute (
             "Read",
             "Read/{id}",
             new { controller = "Read", action = "Index", id = UrlParameter.Optional },
             new { id = @"\d+" }
         );
         routes.MapRoute (
             "Term",
             "Term/{id}",
             new { controller = "Term", action = "Index", id = UrlParameter.Optional },
             new { id = @"\d+" }
         );
         routes.MapRoute (
             name: "TermAction",
             url: "Term/{action}/{id}",
             defaults: new { controller = "Term", action = "Index", id = UrlParameter.Optional }
         );
         routes.MapRoute (
             name: "Default",
             url: "{controller}/{action}/{id}",
             defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
         );
      }
   }
}