using System.Web.Mvc;

namespace SeekDeepWithin.Controllers
{
   /// <summary>
   /// Controller used for the editing.
   /// </summary>
   public class EditController : Controller
   {
      /// <summary>
      /// Gets the index of the controller.
      /// </summary>
      /// <returns>The index view.</returns>
      public ActionResult Index ()
      {
         return View ();
      }
   }
}
