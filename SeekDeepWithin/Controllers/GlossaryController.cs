using System.Linq;
using System.Web.Mvc;
using SeekDeepWithin.DataAccess;
using SeekDeepWithin.Models;

namespace SeekDeepWithin.Controllers
{
   public class GlossaryController : SdwController
   {
      /// <summary>
      /// Initializes a new controller.
      /// </summary>
      public GlossaryController () : base (new SdwDatabase ()) { }

      /// <summary>
      /// Initializes a new controller with the given db info.
      /// </summary>
      /// <param name="db">Database object.</param>
      public GlossaryController (ISdwDatabase db) : base (db) { }

      /// <summary>
      /// Gets the glossary index page.
      /// </summary>
      /// <returns>The glossary index view.</returns>
      public ActionResult Index (int? page, int? sourceId)
      {
         var viewModel = new PagedViewModel<TermViewModel> { PageNumber = page ?? 1, ItemsOnPage = 75 };
         if (sourceId.HasValue)
         {
            var source = this.Database.TermItemSources.Get (sourceId.Value);
            viewModel.TotalHits =  source.GlossaryItems.Count;
            viewModel.AddRange (source.GlossaryItems.OrderBy(i => i.Term.Name)
               .Skip ((viewModel.PageNumber - 1) * viewModel.ItemsOnPage)
               .Take (viewModel.ItemsOnPage)
               .Select (item => new TermViewModel {Id = item.Term.Id, Name = item.Term.Name}));
            return View (new GlossaryIndexViewModel{SourceName = source.Name , Terms = viewModel});
         }
         var terms = this.Database.Terms.All (q => q.OrderBy (t => t.Name));
         viewModel.TotalHits = terms.Count;
         viewModel.AddRange (terms.Skip ((viewModel.PageNumber - 1) * viewModel.ItemsOnPage)
            .Take (viewModel.ItemsOnPage)
            .Select (term => new TermViewModel {Id = term.Id, Name = term.Name}));
         return View (new GlossaryIndexViewModel { Terms = viewModel });
      }
   }
}
