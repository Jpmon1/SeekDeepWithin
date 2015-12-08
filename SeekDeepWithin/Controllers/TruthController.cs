using System.Web.Mvc;
using SeekDeepWithin.DataAccess;

namespace SeekDeepWithin.Controllers
{
   public class TruthController : SdwController
   {
      /// <summary>
      /// Initializes a new controller.
      /// </summary>
      public TruthController () : base (new SdwDatabase ()) { }

      /// <summary>
      /// Initializes a new controller with the given db info.
      /// </summary>
      /// <param name="db">Database object.</param>
      public TruthController (ISdwDatabase db) : base (db) { }

      /// <summary>
      /// Creates truth.
      /// </summary>
      /// <returns></returns>
      [HttpPost]
      [Authorize (Roles = "Creator")]
      public ActionResult Create (string parents, string list)
      {
         return this.Success ();
      }

      [HttpPost]
      [Authorize (Roles = "Creator")]
      public ActionResult Format (string list)
      {
         return this.Success ();
      }

      /// <summary>
      /// Gets the HTML for a new truth.
      /// </summary>
      /// <returns>The HTML used for adding truth.</returns>
      [Authorize (Roles = "Creator")]
      public ActionResult New ()
      {
         return PartialView ();
      }
   }
}