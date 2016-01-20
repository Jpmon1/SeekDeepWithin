using System.Web.Mvc;
using Microsoft.Web.WebPages.OAuth;
using SeekDeepWithin.DataAccess;
using SeekDeepWithin.Filters;
using WebMatrix.WebData;

namespace SeekDeepWithin.Controllers
{
   [InitializeSimpleMembership]
   public class HomeController : SdwController
   {
      /// <summary>
      /// Initializes a new controller.
      /// </summary>
      public HomeController () : base (new SdwDatabase ()) { }

      /// <summary>
      /// Initializes a new controller with the given db info.
      /// </summary>
      /// <param name="db">Database object.</param>
      public HomeController (ISdwDatabase db) : base (db) { }

      /// <summary>
      /// Gets the main index page of seek deep within.
      /// </summary>
      /// <returns>The main index page.</returns>
      public ActionResult Index (string l)
      {
         ViewBag.HasAccount = OAuthWebSecurity.HasLocalAccount (WebSecurity.GetUserId (User.Identity.Name));
         if (User.IsInRole ("Creator")) {
            ViewBag.Regexs = this.Database.RegexFormats.All ();
         }
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
