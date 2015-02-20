using System.Collections.ObjectModel;
using System.Linq;
using System.Web.Mvc;
using SeekDeepWithin.DataAccess;
using SeekDeepWithin.Pocos;
using SeekDeepWithin.Models;

namespace SeekDeepWithin.Controllers
{
   public class GlossaryController : Controller
   {
      private readonly ISdwDatabase m_Db;

      /// <summary>
      /// Initializes a new controller.
      /// </summary>
      public GlossaryController ()
      {
         this.m_Db = new SdwDatabase ();
      }

      /// <summary>
      /// Initializes a new controller with the given db info.
      /// </summary>
      /// <param name="db">Database object.</param>
      public GlossaryController (ISdwDatabase db)
      {
         this.m_Db = db;
      }

      /// <summary>
      /// Gets the glossary index page.
      /// </summary>
      /// <returns>The glossary index view.</returns>
      public ActionResult Index ()
      {
         return View (this.m_Db.GlossaryTerms.All (q => q.OrderBy (t => t.Name)).Select (t => new GlossaryTermViewModel { Id = t.Id, Name = t.Name }));
      }

      /// <summary>
      /// Gets the create new glossary item view.
      /// </summary>
      /// <returns>Create new glossary item view.</returns>
      [Authorize (Roles = "Creator")]
      public ActionResult CreateTerm ()
      {
         if (Request.UrlReferrer != null) TempData["RefUrl"] = Request.UrlReferrer.ToString ();
         return View ();
      }

      /// <summary>
      /// Creates the given glossary item.
      /// </summary>
      /// <returns>Create new glossary item view.</returns>
      [HttpPost]
      [ValidateAntiForgeryToken]
      [Authorize (Roles = "Creator")]
      public ActionResult CreateTerm (GlossaryTermViewModel viewModel)
      {
         if (ModelState.IsValid)
         {
            var item = new GlossaryTerm { Name = viewModel.Name };
            this.m_Db.GlossaryTerms.Insert (item);
            this.m_Db.Save ();
            return RedirectToAction ("Term", new { id = item.Id });
         }
         return View (viewModel);
      }

      /// <summary>
      /// Gets the create new entry view.
      /// </summary>
      /// <param name="termId">The term id to add an entry for.</param>
      /// <returns>Create new entry view.</returns>
      [Authorize (Roles = "Creator")]
      public ActionResult CreateItem (int termId)
      {
         var term = this.m_Db.GlossaryTerms.Get (termId);
         if (Request.UrlReferrer != null) TempData["RefUrl"] = Request.UrlReferrer.ToString ();
         return View (new GlossaryItemViewModel { Term = new GlossaryTermViewModel { Id = termId, Name = term.Name } });
      }

      /// <summary>
      /// Posts a new entry.
      /// </summary>
      /// <param name="viewModel">Entry data.</param>
      /// <returns>Creation result</returns>
      [HttpPost]
      [ValidateAntiForgeryToken]
      [Authorize (Roles = "Creator")]
      public ActionResult CreateItem (GlossaryItemViewModel viewModel)
      {
         if (ModelState.IsValid)
         {
            var term = this.m_Db.GlossaryTerms.Get (viewModel.Term.Id);
            var source = SourceController.GetSource (viewModel.SourceName, viewModel.SourceUrl, this.m_Db);
            var item = new GlossaryItem { Term = term };
            if (item.Sources == null)
               item.Sources = new Collection<GlossaryItemSource> ();
            item.Sources.Add (new GlossaryItemSource { GlossaryItem = item, Source = source });
            this.m_Db.GlossaryItems.Insert (item);
            this.m_Db.Save ();
            return RedirectToAction ("Term", new { id = viewModel.Term.Id });
         }
         return View (viewModel);
      }

      /// <summary>
      /// Gets the add entry view for a glossary item.
      /// </summary>
      /// <param name="id">The id of the glossary item to add entries for.</param>
      /// <param name="termId">The id of the parent term.</param>
      /// <returns>The add entry view.</returns>
      [Authorize (Roles = "Creator")]
      public ActionResult Add (int id, int termId)
      {
         if (Request.UrlReferrer != null) TempData["RefUrl"] = Request.UrlReferrer.ToString ();
         var viewModel = new AddItemViewModel { ParentId = id, ItemType = ItemType.Entry };
         var glossaryItem = this.m_Db.GlossaryItems.Get (id);
         if (glossaryItem.Entries.Count > 0)
            viewModel.Order = glossaryItem.Entries.Max (p => p.Order) + 1;
         else
            viewModel.Order = 1;
         ViewBag.TermId = termId;
         return View (viewModel);
      }

      /// <summary>
      /// Gets the add entry view for a glossary item.
      /// </summary>
      /// <param name="viewModel">The view model for the add.</param>
      /// <returns>The add entry view.</returns>
      [HttpPost]
      [ValidateAntiForgeryToken]
      [Authorize (Roles = "Creator")]
      public ActionResult Add (AddItemViewModel viewModel)
      {
         if (ModelState.IsValid)
         {
            var item = this.m_Db.GlossaryItems.Get (viewModel.ParentId);
            var glossaryEntry = new GlossaryEntry
            {
               Order = viewModel.Order,
               Text = viewModel.Text
            };
            this.m_Db.GlossaryEntries.Insert (glossaryEntry);
            item.Entries.Add (glossaryEntry);
            this.m_Db.Save ();
            return Json ("Success");
         }
         Response.StatusCode = 500;
         return Json ("Data is not valid.");
      }

      /// <summary>
      /// Gets the edit entry view.
      /// </summary>
      /// <param name="id">The id of the entry to edit.</param>
      /// <param name="termId">The id of the parent term.</param>
      /// <returns>The ecit entry view.</returns>
      [Authorize (Roles = "Editor")]
      public ActionResult Edit (int id, int termId)
      {
         if (Request.UrlReferrer != null) TempData["RefUrl"] = Request.UrlReferrer.ToString ();
         var item = this.m_Db.GlossaryItems.Get (id);
         var term = this.m_Db.GlossaryTerms.Get (termId);
         return View (new GlossaryItemViewModel (item, new SdwRenderer()) { Term = new GlossaryTermViewModel {Id = term.Id, Name = term.Name}});
      }

      /// <summary>
      /// Posts edits to a glossary entry.
      /// </summary>
      /// <param name="viewModel">The view model with edits.</param>
      /// <returns></returns>
      [HttpPost]
      [ValidateAntiForgeryToken]
      [Authorize (Roles = "Editor")]
      public ActionResult Edit (GlossaryItemViewModel viewModel)
      {
         if (ModelState.IsValid)
         {
            var entry = this.m_Db.GlossaryItems.Get (viewModel.Id);
            this.m_Db.SetValues (entry, viewModel);
            this.m_Db.Save ();
            return RedirectToAction ("Term", new { id = viewModel.Term.Id });
         }
         return View (viewModel);
      }

      /// <summary>
      /// Shows the given glossary term.
      /// </summary>
      /// <param name="id">Id of glossary term to show.</param>
      /// <returns>The view that show the given glossary term.</returns>
      public ActionResult Term (int id)
      {
         var term = this.m_Db.GlossaryTerms.Get (id);
         if (term.Items.Count == 1)
         {
            var source = term.Items.First ().Sources.FirstOrDefault ();
            if (source != null && source.Source.Name == "|REDIRECT|")
               return Redirect (source.Source.Url);
         }
         var viewModel = new GlossaryTermViewModel (term);
         return View (viewModel);
      }

      /// <summary>
      /// Gets details about the entry with the given id.
      /// </summary>
      /// <param name="id"></param>
      /// <returns></returns>
      [AllowAnonymous]
      public ActionResult GetEntry (int id)
      {
         var entry = this.m_Db.GlossaryEntries.Get (id);
         var result = new
         {
            entryId = id,
            order = entry.Order,
            text = entry.Text,
            headers = entry.Headers.Select (h => new { text = h.Text, id = h.Id }),
            footers = entry.Footers.Select (f => new { text = f.Text, index = f.Index, id = f.Id })
         };
         return Json (result, JsonRequestBehavior.AllowGet);
      }

      /// <summary>
      /// Gets auto complete items for the given term.
      /// </summary>
      /// <param name="term">Term to get auto complet items for.</param>
      /// <returns>The list of possible terms for the given item.</returns>
      public ActionResult AutoComplete (string term)
      {
         var result = new
         {
            suggestions = this.m_Db.GlossaryTerms.Get (t => t.Name.Contains (term))
                                                 .Select (t => new { value = t.Name, data = t.Id })
         };
         return Json (result, JsonRequestBehavior.AllowGet);
      }
   }
}
