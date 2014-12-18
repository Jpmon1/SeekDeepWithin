using System.Web.Mvc;

namespace SeekDeepWithin.Controllers
{
   /// <summary>
   /// Controller used for administration.
   /// </summary>
   public class AdminController : Controller
   {
      /// <summary>
      /// Gets the index of the controller.
      /// </summary>
      /// <returns>The index view.</returns>
      [Authorize (Roles = "Administrator")]
      public ActionResult Index ()
      {
         return View ();
      }
   }
}
