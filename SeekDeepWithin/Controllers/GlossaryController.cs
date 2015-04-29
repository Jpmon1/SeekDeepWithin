using System.Collections.ObjectModel;
using System.Linq;
using System.Web.Mvc;
using PagedList;
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
      public ActionResult Index (int? page, int? sourceId)
      {
         int pageNumber = page ?? 1;
         if (sourceId.HasValue)
         {
            var terms = new Collection <GlossaryTermViewModel> ();
            var source = this.m_Db.GlossaryItemSources.Get (sourceId.Value);
            var items = source.GlossaryItems;
            foreach (var glossaryItem in items)
            {
               if (terms.Any (t => t.Id == glossaryItem.Term.Id))
                  continue;
               terms.Add (new GlossaryTermViewModel { Id = glossaryItem.Term.Id, Name = glossaryItem.Term.Name });
            }
            return View (new GlossaryIndexViewModel{SourceName = source.Name , Terms = terms.OrderBy(t => t.Name).ToPagedList (pageNumber, 75)});
         }
         return View (new GlossaryIndexViewModel{Terms =this.m_Db.GlossaryTerms
            .All (q => q.OrderBy (t => t.Name))
            .Select (t => new GlossaryTermViewModel { Id = t.Id, Name = t.Name })
            .ToPagedList (pageNumber, 75)});
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
            var foundTerm = this.m_Db.GlossaryTerms.Get (t => t.Name == viewModel.Name).FirstOrDefault();
            if (foundTerm != null)
            {
               ViewBag.ErrorMessage = "A term with that name already exists:";
               ViewBag.FoundTermId = foundTerm.Id;
               return View (viewModel);
            }
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
         ViewBag.Sources = new SelectList (this.m_Db.GlossaryItemSources.All (q => q.OrderBy (s => s.Name)), "Id", "Name");
         return View (new GlossaryItemViewModel { Term = new GlossaryTermViewModel {Id = termId, Name = term.Name} });
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
      public ActionResult CreateItem (int termId, int sourceId)
      {
         var term = this.m_Db.GlossaryTerms.Get (termId);
         var item = new GlossaryItem { Term = term, Source = this.m_Db.GlossaryItemSources.Get(sourceId) };
         this.m_Db.GlossaryItems.Insert (item);
         this.m_Db.Save ();
         return RedirectToAction ("Term", new { id = termId });
      }

      /// <summary>
      /// Gets the add entry view for a glossary item.
      /// </summary>
      /// <param name="id">The id of the glossary item to add entries for.</param>
      /// <returns>The add entry view.</returns>
      [Authorize (Roles = "Creator")]
      public ActionResult Add (int id)
      {
         if (Request.UrlReferrer != null) TempData["RefUrl"] = Request.UrlReferrer.ToString ();
         var viewModel = new AddItemViewModel { ParentId = id, ItemType = ItemType.Entry };
         var glossaryItem = this.m_Db.GlossaryItems.Get (id);
         if (glossaryItem.Entries.Count > 0)
            viewModel.Order = glossaryItem.Entries.Max (p => p.Order) + 1;
         else
            viewModel.Order = 1;
         viewModel.Title = glossaryItem.Term.Name;
         ViewBag.TermId = glossaryItem.Term.Id;
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
            if (viewModel.IsInsert)
            {
               foreach (var entry in item.Entries)
               {
                  if (entry.Order >= viewModel.Order)
                     entry.Order++;
               }
            }
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
      /// <returns>The ecit entry view.</returns>
      [Authorize (Roles = "Editor")]
      public ActionResult Edit (int id)
      {
         if (Request.UrlReferrer != null) TempData["RefUrl"] = Request.UrlReferrer.ToString ();
         var item = this.m_Db.GlossaryItems.Get (id);
         return View (new GlossaryItemViewModel (item, new SdwRenderer ())
            {
               Term = new GlossaryTermViewModel {Id = item.Term.Id, Name = item.Term.Name}
            });
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
         var viewModel = new GlossaryTermViewModel (term);
         return View (viewModel);
      }

      /// <summary>
      /// Gets information for the given term.
      /// </summary>
      /// <param name="termName">Term name to get information for.</param>
      /// <returns>Term information.</returns>
      public ActionResult Get (string termName)
      {
         var term = this.m_Db.GlossaryTerms.Get (t => t.Name == termName).FirstOrDefault ();
         if (term == null)
         {
            Response.StatusCode = 500;
            return Json ("Tag not found.");
         }
         return Json (new { id = term.Id, name = term.Name }, JsonRequestBehavior.AllowGet);
      }

      #region Entries

      /// <summary>
      /// Performs an edit for the given entry.
      /// </summary>
      /// <param name="entryId">The entry to edit.</param>
      /// <param name="text">The entry text.</param>
      /// <param name="order">The entry order.</param>
      /// <returns>Results.</returns>
      [HttpPost]
      [ValidateAntiForgeryToken]
      [Authorize (Roles = "Editor")]
      public ActionResult UpdateEntry (int entryId, string text, int? order)
      {
         var entry = this.m_Db.GlossaryEntries.Get (entryId);
         if (entry == null)
         {
            Response.StatusCode = 500;
            return Json ("Data is not valid.");
         }
         entry.Text = text;
         if (order != null)
            entry.Order = order.Value;
         this.m_Db.Save ();
         return Json ("Success");
      }

      /// <summary>
      /// Deletes the given entry.
      /// </summary>
      /// <param name="entryId">The entry to delete.</param>
      /// <returns>Results.</returns>
      [HttpPost]
      [ValidateAntiForgeryToken]
      [Authorize (Roles = "Creator")]
      public ActionResult DeleteEntry (int entryId)
      {
         if (ModelState.IsValid)
         {
            var entry = this.m_Db.GlossaryEntries.Get (entryId);
            entry.Item.Entries.Remove (entry);
            this.m_Db.GlossaryEntries.Delete (entry);
            this.m_Db.Save ();
            return Json ("Success");
         }
         Response.StatusCode = 500;
         return Json ("Data is not valid.");
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
            text = entry.Text,
            order = entry.Order
         };
         return Json (result, JsonRequestBehavior.AllowGet);
      }

      #endregion

      /// <summary>
      /// Gets auto complete items for the given term.
      /// </summary>
      /// <param name="term">Term to get auto complete items for.</param>
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

      /// <summary>
      /// Edits a glossary term.
      /// </summary>
      /// <param name="id">The id of the term to edit.</param>
      /// <returns></returns>
      [Authorize (Roles = "Editor")]
      public ActionResult EditTerm (int id)
      {
         var term = this.m_Db.GlossaryTerms.Get (id);
         if (Request.UrlReferrer != null) TempData["RefUrl"] = Request.UrlReferrer.ToString ();
         ViewBag.Tags = new SelectList (this.m_Db.Tags.All (q => q.OrderBy (t => t.Name)), "Id", "Name");
         return View (new GlossaryTermViewModel (term));
      }

      /// <summary>
      /// Edits a glossary term.
      /// </summary>
      /// <param name="viewModel">The view model with data.</param>
      /// <returns></returns>
      [HttpPost]
      [ValidateAntiForgeryToken]
      [Authorize (Roles = "Editor")]
      public ActionResult EditTerm (GlossaryTermViewModel viewModel)
      {
         if (ModelState.IsValid)
         {
            var term = this.m_Db.GlossaryTerms.Get (viewModel.Id);
            this.m_Db.SetValues (term, viewModel);
            this.m_Db.Save();
            return RedirectToAction ("Term", new {id = viewModel.Id});
         }
         return View (viewModel);
      }

      /// <summary>
      /// Adds the given tag to the given term.
      /// </summary>
      /// <param name="tagId">Id of tag to add.</param>
      /// <param name="id">Id of term to add tag to.</param>
      /// <returns></returns>
      [HttpPost]
      [ValidateAntiForgeryToken]
      [Authorize (Roles = "Editor")]
      public ActionResult AddTag (int tagId, int id)
      {
         var term = this.m_Db.GlossaryTerms.Get (id);
         var foundTag = term.Tags.FirstOrDefault (bt => bt.Tag.Id == tagId);
         if (foundTag != null)
         {
            Response.StatusCode = 500;
            return Json ("That tag is already assigned to the term.");
         }
         var tag = this.m_Db.Tags.Get (tagId);
         term.Tags.Add (new GlossaryTermTag { Term = term, Tag = tag });
         this.m_Db.Save ();
         return Json ("success");
      }

      /// <summary>
      /// Removes the tag with the given id from the given term.
      /// </summary>
      /// <param name="tagId">Id of tag to remove.</param>
      /// <param name="id">Id of term to remove tag from.</param>
      /// <returns></returns>
      [HttpPost]
      [ValidateAntiForgeryToken]
      [Authorize (Roles = "Editor")]
      public ActionResult RemoveTag (int tagId, int id)
      {
         var term = this.m_Db.GlossaryTerms.Get (id);
         if (term == null)
         {
            Response.StatusCode = 500;
            return Json ("Unable to get term - " + id);
         }
         var termTag = term.Tags.FirstOrDefault (bt => bt.Id == tagId);
         if (termTag == null)
         {
            Response.StatusCode = 500;
            return Json ("Unable to find the given tag.");
         }
         term.Tags.Remove (termTag);
         this.m_Db.Save ();
         return Json ("success");
      }
   }
}
