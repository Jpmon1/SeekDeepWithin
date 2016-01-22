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
      private readonly UsersContext m_UserDb = new UsersContext ();

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
         var userId = WebSecurity.GetUserId (User.Identity.Name);
         ViewBag.LoadOnScroll = 1;
         if (OAuthWebSecurity.HasLocalAccount (userId)) {
            ViewBag.HasAccount = true;
            var user = this.m_UserDb.UserProfiles.Find (userId);
            ViewBag.LoadOnScroll = user.UserData.LoadOnScroll.HasValue && user.UserData.LoadOnScroll.Value ? 1 : 0;
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
