using System.Linq;
using System.Web.Mvc;
using SeekDeepWithin.DataAccess;
using SeekDeepWithin.Pocos;
using SeekDeepWithin.Models;

namespace SeekDeepWithin.Controllers
{
   /// <summary>
   /// Controller for passages.
   /// </summary>
   public class PassageController : Controller
   {
      private readonly ISdwDatabase m_Db;

      /// <summary>
      /// Initializes a new controller.
      /// </summary>
      public PassageController ()
      {
         this.m_Db = new SdwDatabase ();
      }

      /// <summary>
      /// Initializes a new controller with the given db info.
      /// </summary>
      /// <param name="db">Database object.</param>
      public PassageController (ISdwDatabase db)
      {
         this.m_Db = db;
      }

      /// <summary>
      /// Gets the details page for the given item.
      /// </summary>
      /// <param name="entryId">The id of the entry we are getting details for.</param>
      /// <returns>The details page.</returns>
      public ActionResult Index (int entryId)
      {
         var entry = this.m_Db.PassageEntries.Get (entryId);
         return View (new PassageViewModel (entry));
      }

      /// <summary>
      /// Performs an edit for the given model.
      /// </summary>
      /// <param name="viewModel">The view model with edits.</param>
      /// <returns>The edit page.</returns>
      [HttpPost]
      [ValidateAntiForgeryToken]
      [Authorize (Roles = "Editor")]
      public ActionResult Edit (PassageViewModel viewModel)
      {
         if (ModelState.IsValid)
         {
            var passage = this.m_Db.Passages.Get (viewModel.Id);
            this.m_Db.SetValues (passage, viewModel);
            this.m_Db.Save ();
            return Json ("Success");
         }
         Response.StatusCode = 500;
         return Json ("Data is not valid.");
      }

      /// <summary>
      /// Creates a passage for the given chapter.
      /// </summary>
      /// <param name="viewModel">View model with data.</param>
      /// <returns>Create results.</returns>
      [HttpPost]
      [ValidateAntiForgeryToken]
      [Authorize (Roles = "Creator")]
      public ActionResult Create (AddItemViewModel viewModel)
      {
         if (ModelState.IsValid)
         {
            var chapter = this.m_Db.Chapters.Get (viewModel.ParentId);
            var passageEntry = new PassageEntry
            {
               Chapter = chapter,
               ChapterId = viewModel.ParentId,
               Number = viewModel.Number,
               Order = viewModel.Order,
               Passage = this.GetPassage (viewModel.Text)
            };
            this.m_Db.Passages.Insert (passageEntry.Passage);
            chapter.Passages.Add (passageEntry);
            this.m_Db.Save ();
            return Json ("Success");
         }
         Response.StatusCode = 500;
         return Json ("Data is not valid.");
      }

      /// <summary>
      /// Gets the passage in the database with the given text.
      /// </summary>
      /// <param name="text">The text of the passage to get.</param>
      /// <returns>The requested passage, a new passage if does not exist.</returns>
      private Passage GetPassage (string text)
      {
         var passages = this.m_Db.Passages.Get (h => h.Text == text);
         var passage = passages.FirstOrDefault ();
         if (passage == null)
         {
            passage = new Passage { Text = text };
            this.m_Db.Passages.Insert (passage);
            this.m_Db.Save ();
         }
         return passage;
      }

      /// <summary>
      /// Gets the entry data for the given entry id.
      /// </summary>
      /// <param name="id">Id of entry to get text for.</param>
      /// <returns>The text of the entry.</returns>
      [AllowAnonymous]
      public ActionResult GetEntry (int id)
      {
         var entry = this.m_Db.PassageEntries.Get (id);
         var result = new
         {
            entryId = id,
            order = entry.Order,
            passageId = entry.PassageId,
            passageNumber = entry.Number,
            passageText = entry.Passage.Text,
            headers = entry.Headers.Select(h => new { text = h.Text, id = h.Id }),
            footers = entry.Footers.Select(f => new { text = f.Text, index = f.Index, id = f.Id })
         };
         return Json (result, JsonRequestBehavior.AllowGet);
      }
   }
}
