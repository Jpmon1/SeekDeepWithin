using System.Web.Mvc;

namespace SeekDeepWithin.Controllers
{
   public class HomeController : Controller
   {
      /// <summary>
      /// Gets the main index page of seek deep within.
      /// </summary>
      /// <returns>The main index page.</returns>
      public ActionResult Index (string love)
      {
         /*if (!string.IsNullOrWhiteSpace (love)) {
            var lights = Helper.Base64Decode (love + "==");
         }*/
         return View ();
      }

      /// <summary>
      /// Gets the about page.
      /// </summary>
      /// <returns>The about page.</returns>
      public ActionResult About ()
      {
         return View ();
      }
   }
}
