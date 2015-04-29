using System.Web.Mvc;

namespace SeekDeepWithin.Controllers
{
   public class HomeController : Controller
   {
      /// <summary>
      /// Gets the main index page of seek deep within.
      /// </summary>
      /// <returns>The main index page.</returns>
      public ActionResult Index ()
      {
         ViewBag.Message = "Study Spiritual Texts from all over the World.";
         var controller = new PassageController ();
         ViewBag.RandomPassage = controller.GetRandomPassage ();
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
