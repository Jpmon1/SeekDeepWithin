using System.Web.Mvc;

namespace SeekDeepWithin.Controllers
{
   public class HomeController : Controller
   {
      /// <summary>
      /// Gets the main index page of seek deep within.
      /// </summary>
      /// <returns>The main index page.</returns>
      public ActionResult Index (string data)
      {
         /*if (!string.IsNullOrWhiteSpace (data)) {
            var lights = Helper.Base64Decode (data + "==");
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
