using System.Web.Mvc;
using DotNetOpenAuth.Messaging;
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
      /// <param name="log">True if we want to see the parsing log.</param>
      /// <returns>The search view.</returns>
      public ActionResult Results (string searchFor, bool? log)
      {
         var viewModel = new SearchViewModel {Query = searchFor, ShowLog = log != null && log.Value};
         var parser = new PassageParser (this.m_Db);
         parser.Parse(searchFor);
         viewModel.ParserLog = parser.Log;
         if (parser.PassageList.Count > 0)
            viewModel.Passages.AddRange(parser.PassageList);
         var passages = this.m_Db.Passages.Get (p => p.Text.Contains (searchFor));
         foreach (var passage in passages)
         {
            foreach (var entry in passage.PassageEntries)
               viewModel.Passages.Add (new PassageViewModel(entry));
         }
         return View (viewModel);
      }
   }
}
