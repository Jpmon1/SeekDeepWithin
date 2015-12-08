using System.Web.Mvc;
using System.Web.Routing;

namespace SeekDeepWithin
{
   /// <summary>
   /// Represents the routng information for SDW.
   /// </summary>
   public class RouteConfig
   {
      /// <summary>
      /// Maps the proper routes for the website.
      /// </summary>
      /// <param name="routes"></param>
      public static void RegisterRoutes (RouteCollection routes)
      {
         routes.IgnoreRoute ("{resource}.axd/{*pathInfo}");

         /*routes.MapRoute ("DefaultApi", "api/{controller}/{id}",
            new { id = UrlParameter.Optional });*/
         routes.MapRoute (
            "Read",
            "Read/{id}",
            new { controller = "Read", action = "Index", id = UrlParameter.Optional },
            new { id = @"\d+" }
            );
         routes.MapRoute ("ReadAction", "Read/{action}/{id}",
            new { controller = "Read", action = "Index", id = UrlParameter.Optional });
         routes.MapRoute (
            "Term",
            "Term/{id}",
            new { controller = "Term", action = "Index", id = UrlParameter.Optional },
            new { id = @"\d+" }
            );
         routes.MapRoute ("TermAction", "Term/{action}/{id}",
            new {controller = "Term", action = "Index", id = UrlParameter.Optional});
         routes.MapRoute ("Default", "{controller}/{action}/{id}",
            new {controller = "Home", action = "Index", id = UrlParameter.Optional});
      }
   }
}