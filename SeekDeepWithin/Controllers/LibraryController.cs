using System.Linq;
using System.Web.Mvc;
using SeekDeepWithin.DataAccess;
using SeekDeepWithin.Models;
using SeekDeepWithin.Pocos;

namespace SeekDeepWithin.Controllers
{
   public class LibraryController : SdwController
   {
      /// <summary>
      /// Initializes a new controller.
      /// </summary>
      public LibraryController () : base (new SdwDatabase ()) { }

      /// <summary>
      /// Initializes a new controller with the given db info.
      /// </summary>
      /// <param name="db">Database object.</param>
      public LibraryController (ISdwDatabase db) : base (db) { }

      /// <summary>
      /// The library index.
      /// </summary>
      /// <returns></returns>
      public ActionResult Index (int? page)
      {
         var books = this.Database.Light.Get (l => true/*l.IsBook*/, q => q.OrderBy (l => l.Text)).ToList ();
         var viewModel = new PagedViewModel<Light> { PageNumber = page ?? 1, ItemsOnPage = 12, TotalHits = books.Count };
         viewModel.AddRange (books.Skip ((viewModel.PageNumber - 1) * viewModel.ItemsOnPage)
            .Take (viewModel.ItemsOnPage)
            /*.Select (book => new BookViewModel (book, true))*/);
         return View (viewModel);
      }
   }
}
