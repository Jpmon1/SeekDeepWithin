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
      /// Shows the given glossary term.
      /// </summary>
      /// <param name="id">Id of glossary term to show.</param>
      /// <returns>The view that show the given glossary term.</returns>
      public ActionResult Term (int id)
      {
         return RedirectToAction ("Index", "Term", new {id});
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
            return Json ("Term not found.");
         }
         return Json (new { id = term.Id, name = term.Name }, JsonRequestBehavior.AllowGet);
      }

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
   }
}
