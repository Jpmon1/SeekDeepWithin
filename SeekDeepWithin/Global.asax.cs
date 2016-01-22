using System;
using System.Data.Entity;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using SeekDeepWithin.Controllers;
using SeekDeepWithin.DataAccess;
using SeekDeepWithin.Migrations;
using WebMatrix.WebData;

namespace SeekDeepWithin
{
   // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
   // visit http://go.microsoft.com/?LinkId=9394801
   public class MvcApplication : HttpApplication
   {
      protected void Application_Start ()
      {
         AreaRegistration.RegisterAllAreas ();

         WebApiConfig.Register (GlobalConfiguration.Configuration);
         FilterConfig.RegisterGlobalFilters (GlobalFilters.Filters);
         RouteConfig.RegisterRoutes (RouteTable.Routes);
         BundleConfig.RegisterBundles (BundleTable.Bundles);
         AuthConfig.RegisterAuth ();
      }

      /*protected void Application_Error (object sender, EventArgs e)
      {
         var lastError = Server.GetLastError ();
         Server.ClearError ();
         int statusCode = lastError.GetType () == typeof (HttpException) ? ((HttpException)lastError).GetHttpCode () : 500;
         var contextWrapper = new HttpContextWrapper (this.Context);
         var routeData = new RouteData ();
         routeData.Values.Add ("controller", "Error");
         routeData.Values.Add ("action", "Index");
         routeData.Values.Add ("statusCode", statusCode);
         routeData.Values.Add ("exception", lastError);
         routeData.Values.Add ("isAjaxRequet", contextWrapper.Request.IsAjaxRequest ());
         IController controller = new ErrorController ();
         var requestContext = new RequestContext (contextWrapper, routeData);
         controller.Execute (requestContext);
         Response.End ();
      }*/
   }
}