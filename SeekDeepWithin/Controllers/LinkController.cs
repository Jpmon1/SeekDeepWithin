using System.Web.Mvc;
using SeekDeepWithin.DataAccess;

namespace SeekDeepWithin.Controllers
{
   /// <summary>
   /// Controller for links.
   /// </summary>
   public class LinkController : Controller
   {
      private readonly ISdwDatabase m_Db;

      /// <summary>
      /// Initializes a new controller.
      /// </summary>
      public LinkController ()
      {
         this.m_Db = new SdwDatabase ();
      }

      /// <summary>
      /// Initializes a new controller with the given db info.
      /// </summary>
      /// <param name="db">Database object.</param>
      public LinkController (ISdwDatabase db)
      {
         this.m_Db = db;
      }

      public ActionResult Index ()
      {
         return View ();
      }
   }
}
