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
      public ActionResult Results (string searchFor)
      {
         var viewModel = new SearchViewModel {Query = searchFor};
         var parser = new PassageParser (this.m_Db);
         parser.Parse(searchFor);
         viewModel.ParserLog = parser.Log;
         foreach (var entry in parser.PassageList)
         {
            viewModel.Passages.Add (new PassageViewModel
            {
               Text = entry.Passage.Text,
               Id = entry.Passage.Id,
               EntryId = entry.Id,
               Number = entry.Number,
               ChapterId = entry.Chapter.Id,
               ChapterName = entry.Chapter.Chapter.Name,
               SubBookId = entry.Chapter.SubBook.Id,
               SubBookName = entry.Chapter.SubBook.SubBook.Name,
               VersionId = entry.Chapter.SubBook.Version.Id,
               VersionName = entry.Chapter.SubBook.Version.Title
            });
         }
         var passages = this.m_Db.Passages.Get (p => p.Text.Contains (searchFor));
         foreach (var passage in passages)
         {
            foreach (var entry in passage.PassageEntries)
            {
               viewModel.Passages.Add (new PassageViewModel
               {
                  Text = passage.Text,
                  Id = passage.Id,
                  EntryId = entry.Id,
                  Number = entry.Number,
                  ChapterId = entry.Chapter.Id,
                  ChapterName = entry.Chapter.Chapter.Name,
                  SubBookId = entry.Chapter.SubBook.Id,
                  SubBookName = entry.Chapter.SubBook.SubBook.Name,
                  VersionId = entry.Chapter.SubBook.Version.Id,
                  VersionName = entry.Chapter.SubBook.Version.Title
               });
            }
         }
         return View (viewModel);
      }
   }
}
