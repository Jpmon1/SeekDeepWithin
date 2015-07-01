using System.Linq;
using System.Web.Mvc;
using SeekDeepWithin.DataAccess;
using SeekDeepWithin.Models;
using SeekDeepWithin.Pocos;

namespace SeekDeepWithin.Controllers
{
   public class TermItemController : SdwController
   {
      /// <summary>
      /// Initializes a new controller.
      /// </summary>
      public TermItemController () : base (new SdwDatabase ()) { }

      /// <summary>
      /// Initializes a new controller with the given db info.
      /// </summary>
      /// <param name="db">Database object.</param>
      public TermItemController (ISdwDatabase db) : base (db) { }

      /// <summary>
      /// Gets the create new entry view.
      /// </summary>
      /// <param name="termId">The term id to add an entry for.</param>
      /// <returns>Create new entry view.</returns>
      [Authorize (Roles = "Creator")]
      public ActionResult Create (int termId)
      {
         var term = this.Database.Terms.Get (termId);
         return View (new TermItemViewModel { Term = new TermViewModel (term) });
      }

      /// <summary>
      /// Creates a new glossary item.
      /// </summary>
      /// <param name="termId">The item's term id.</param>
      /// <param name="sourceId">The source id for the item.</param>
      /// <returns>Creation result</returns>
      [HttpPost]
      [ValidateAntiForgeryToken]
      [Authorize (Roles = "Creator")]
      public ActionResult Create (int termId, int sourceId)
      {
         var term = this.Database.Terms.Get (termId);
         var source = this.Database.TermItemSources.Get (sourceId);
         if (term == null) return this.Fail ("Unable to determine the term.");
         if (source == null) return this.Fail ("Unable to determine the source.");
         var item = new TermItem { Term = term, Source = source };
         this.Database.TermItems.Insert (item);
         this.Database.Save ();
         return Json ("success");
      }

      /// <summary>
      /// Gets the edit entry view.
      /// </summary>
      /// <param name="id">The id of the entry to edit.</param>
      /// <returns>The ecit entry view.</returns>
      [Authorize (Roles = "Editor")]
      public ActionResult Edit (int id)
      {
         var item = this.Database.TermItems.Get (id);
         return View (new TermItemViewModel (item, new SdwRenderer ())
         {
            Term = new TermViewModel { Id = item.Term.Id, Name = item.Term.Name }
         });
      }

      /// <summary>
      /// Deletes the given term item.
      /// </summary>
      /// <param name="id"></param>
      /// <returns></returns>
      public ActionResult Delete (int id)
      {
         var item = this.Database.TermItems.Get (id);
         var term = item.Term;
         term.Items.Remove (item);
         this.Database.TermItems.Delete(item);
         this.Database.Save ();
         return RedirectToAction ("Index", "Term", new {id = term.Id});
      }

      /// <summary>
      /// Creates a new glossary item.
      /// </summary>
      /// <param name="name">The name of the source.</param>
      /// <param name="url">The url of the source.</param>
      /// <param name="termId">The item's term id.</param>
      /// <returns>Creation result</returns>
      [HttpPost]
      [ValidateAntiForgeryToken]
      [Authorize (Roles = "Creator")]
      public ActionResult CreateSource (string name, string url, int? termId)
      {
         Term term = null;
         if (termId.HasValue && termId.Value > 0)
            term = this.Database.Terms.Get (termId.Value);
         var source = new TermItemSource {Name = name, Url = url, Term = term};
         this.Database.TermItemSources.Insert (source);
         this.Database.Save ();
         return Json ("success");
      }

      /// <summary>
      /// Gets auto complete items for the given term.
      /// </summary>
      /// <param name="source">Term Item source to get auto complete items for.</param>
      /// <returns>The list of possible terms for the given item.</returns>
      public ActionResult AutoComplete (string source)
      {
         var result = new
         {
            suggestions = this.Database.TermItemSources.Get (t => t.Name.Contains (source)).Select (t => new { value = t.Name, data = t.Id })
         };
         return Json (result, JsonRequestBehavior.AllowGet);
      }
   }
}
