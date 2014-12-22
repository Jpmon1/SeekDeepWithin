using System.Linq;
using System.Web.Mvc;
using SeekDeepWithin.DataAccess;
using SeekDeepWithin.Domain;
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
         return View (this.m_Db.GlossaryTerms.All ().Select (t => new GlossaryTermViewModel { Id = t.Id, Name = t.Name }));
      }

      /// <summary>
      /// Gets the create new glossary item view.
      /// </summary>
      /// <returns>Create new glossary item view.</returns>
      [Authorize (Roles = "EditGlossary")]
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
      [Authorize (Roles = "EditGlossary")]
      public ActionResult CreateTerm (GlossaryTermViewModel viewModel)
      {
         if (ModelState.IsValid)
         {
            var item = new GlossaryTerm {Name = viewModel.Name};
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
      /// <param name="termName">The name of the term.</param>
      /// <returns>Create new entry view.</returns>
      [Authorize (Roles = "EditGlossary")]
      public ActionResult CreateEntry (int termId, string termName)
      {
         if (Request.UrlReferrer != null) TempData["RefUrl"] = Request.UrlReferrer.ToString ();
         return View (new GlossaryEntryViewModel { TermId = termId, TermName = termName});
      }

      /// <summary>
      /// Posts a new entry.
      /// </summary>
      /// <param name="viewModel">Entry data.</param>
      /// <returns>Creation result</returns>
      [HttpPost]
      [ValidateAntiForgeryToken]
      [Authorize (Roles = "EditGlossary")]
      public ActionResult CreateEntry (GlossaryEntryViewModel viewModel)
      {
         if (ModelState.IsValid)
         {
            var term = this.m_Db.GlossaryTerms.Get (viewModel.TermId);
            var entry = new GlossaryEntry {GlossaryTerm = term, Text = viewModel.Text };
            this.m_Db.GlossaryEntries.Insert (entry);
            this.m_Db.Save ();
            return RedirectToAction ("Term", new {id = viewModel.TermId});
         }
         return View (viewModel);
      }

      /// <summary>
      /// Gets the edit entry view.
      /// </summary>
      /// <param name="id">The id of the entry to edit.</param>
      /// <param name="termId">The id of the parent term.</param>
      /// <param name="termName">The name of the parent term.</param>
      /// <returns>The ecit entry view.</returns>
      [Authorize (Roles = "EditGlossary")]
      public ActionResult EditEntry (int id, int termId, string termName)
      {
         if (Request.UrlReferrer != null) TempData["RefUrl"] = Request.UrlReferrer.ToString ();
         var entry = this.m_Db.GlossaryEntries.Get (id);
         var source = entry.GlossaryEntrySources.FirstOrDefault ();
         return View (new GlossaryEntryViewModel
         {
            Id = entry.Id,
            Text = entry.Text,
            TermId = termId,
            TermName = termName,
            SourceName = source == null ? string.Empty : source.Source.Name,
            SourceUrl = source == null ? string.Empty : source.Source.Url
         });
      }

      /// <summary>
      /// Posts edits to a glossary entry.
      /// </summary>
      /// <param name="viewModel">The view model with edits.</param>
      /// <returns></returns>
      [HttpPost]
      [ValidateAntiForgeryToken]
      [Authorize (Roles = "EditGlossary")]
      public ActionResult EditEntry (GlossaryEntryViewModel viewModel)
      {
         if (ModelState.IsValid)
         {
            var entry = this.m_Db.GlossaryEntries.Get (viewModel.Id);
            this.m_Db.SetValues (entry, viewModel);
            this.m_Db.Save ();
            return RedirectToAction ("Term", new { id = viewModel.TermId });
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
         var viewModel = new GlossaryTermViewModel { Id = term.Id, Name = term.Name };
         foreach (var entry in term.Entries)
         {
            var source = entry.GlossaryEntrySources.FirstOrDefault ();
            var entryViewModel = new GlossaryEntryViewModel
            {
               Id = entry.Id,
               Text = entry.Text,
               TermId = entry.GlossaryTerm.Id,
               SourceName = source == null ? string.Empty : source.Source.Name,
               SourceUrl = source == null ? string.Empty : source.Source.Url
            };
            viewModel.Entries.Add (entryViewModel);
         }
         return View (viewModel);
      }

      /// <summary>
      /// Gets auto complete items for the given term.
      /// </summary>
      /// <param name="term">Term to get auto complet items for.</param>
      /// <returns>The list of possible terms for the given item.</returns>
      public ActionResult AutoComplete (string term)
      {
         var result = new { suggestions = this.m_Db.GlossaryTerms.Get (t => t.Name.Contains (term))
                                                                 .Select (t => new { value = t.Name, data = t.Id }) };
         return Json (result, JsonRequestBehavior.AllowGet);
      }
   }
}
