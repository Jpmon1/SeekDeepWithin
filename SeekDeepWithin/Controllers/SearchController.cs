using System.Linq;
using System.Web.Mvc;
using SeekDeepWithin.DataAccess;
using SeekDeepWithin.Models;

namespace SeekDeepWithin.Controllers
{
   public class SearchController : Controller
   {
      private readonly ISdwDatabase m_Db;

      /// <summary>
      /// Initializes a new controller.
      /// </summary>
      public SearchController ()
      {
         this.m_Db = new SdwDatabase ();
      }

      /// <summary>
      /// Initializes a new controller with the given db info.
      /// </summary>
      /// <param name="db">Database object.</param>
      public SearchController (ISdwDatabase db)
      {
         this.m_Db = db;
      }

      /// <summary>
      /// Gets the search index page.
      /// </summary>
      /// <returns>The search view.</returns>
      public ActionResult Index ()
      {
         return View ();
      }

      /// <summary>
      /// Gets the search page.
      /// </summary>
      /// <param name="searchFor">The string to search for.</param>
      /// <returns>The search view.</returns>
      public ActionResult Query (string searchFor)
      {
         var passages = this.m_Db.Passages.Get (p => p.Text.Contains (searchFor));
         return View (passages.Select (p => p.ToViewModel ()));
      }
   }
}
