using System.Linq;
using System.Web.Mvc;
using SeekDeepWithin.DataAccess;
using SeekDeepWithin.Models;
using SeekDeepWithin.Pocos;

namespace SeekDeepWithin.Controllers
{
   public class GlossaryItemController : Controller
   {
      private readonly ISdwDatabase m_Db;

      /// <summary>
      /// Initializes a new controller.
      /// </summary>
      public GlossaryItemController ()
      {
         this.m_Db = new SdwDatabase ();
      }

      /// <summary>
      /// Initializes a new controller with the given db info.
      /// </summary>
      /// <param name="db">Database object.</param>
      public GlossaryItemController (ISdwDatabase db)
      {
         this.m_Db = db;
      }

      /// <summary>
      /// Gets the create new entry view.
      /// </summary>
      /// <param name="termId">The term id to add an entry for.</param>
      /// <returns>Create new entry view.</returns>
      [Authorize (Roles = "Creator")]
      public ActionResult Create (int termId)
      {
         var term = this.m_Db.GlossaryTerms.Get (termId);
         if (Request.UrlReferrer != null) TempData["RefUrl"] = Request.UrlReferrer.ToString ();
         ViewBag.Sources = new SelectList (this.m_Db.GlossaryItemSources.All (q => q.OrderBy (s => s.Name)), "Id", "Name");
         return View (new GlossaryItemViewModel { Term = new GlossaryTermViewModel { Id = termId, Name = term.Name } });
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
         var term = this.m_Db.GlossaryTerms.Get (termId);
         var item = new GlossaryItem { Term = term, Source = this.m_Db.GlossaryItemSources.Get (sourceId) };
         this.m_Db.GlossaryItems.Insert (item);
         this.m_Db.Save ();
         return RedirectToAction ("Index", "Term", new { id = termId });
      }

      /// <summary>
      /// Gets the edit entry view.
      /// </summary>
      /// <param name="id">The id of the entry to edit.</param>
      /// <returns>The ecit entry view.</returns>
      [Authorize (Roles = "Editor")]
      public ActionResult Edit (int id)
      {
         if (Request.UrlReferrer != null) TempData["RefUrl"] = Request.UrlReferrer.ToString ();
         var item = this.m_Db.GlossaryItems.Get (id);
         var sources = this.m_Db.GlossaryItemSources.All (q => q.OrderBy (s => s.Name));
         ViewBag.Sources = new SelectList (sources, "Id", "Name", item.Source);
         return View (new GlossaryItemViewModel (item, new SdwRenderer ())
         {
            Term = new GlossaryTermViewModel { Id = item.Term.Id, Name = item.Term.Name }
         });
      }

      /// <summary>
      /// Sets the source for the given glossary item.
      /// </summary>
      /// <param name="id">Id of glossary item to set the source for.</param>
      /// <param name="sourceId">Id of source to set.</param>
      /// <returns>Results</returns>
      [HttpPost]
      [ValidateAntiForgeryToken]
      [Authorize (Roles = "Editor")]
      public ActionResult SetSource (int id, int sourceId)
      {
         var source = this.m_Db.GlossaryItemSources.Get (sourceId);
         if (source == null)
         {
            Response.StatusCode = 500;
            return Json ("Unknown source!");
         }
         var item = this.m_Db.GlossaryItems.Get (id);
         if (item == null)
         {
            Response.StatusCode = 500;
            return Json ("Unknown item!");
         }
         item.Source = source;
         this.m_Db.Save();
         return Json ("success");
      }
   }
}
