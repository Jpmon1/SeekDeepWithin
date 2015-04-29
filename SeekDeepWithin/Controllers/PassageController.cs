using System;
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
            var chapter = this.m_Db.SubBookChapters.Get (viewModel.ParentId);
            if (viewModel.IsInsert)
            {
               foreach (var passage in chapter.Passages)
               {
                  if (passage.Order >= viewModel.Order)
                     passage.Order++;
                  if (passage.Number >= viewModel.Number)
                     passage.Number++;
               }
            }
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
      /// Performs an edit for the given passage.
      /// </summary>
      /// <param name="entryId">The passage entry to edit.</param>
      /// <param name="text">The passage text.</param>
      /// <param name="order">The passage order.</param>
      /// <param name="number">The passage number.</param>
      /// <returns>The edit page.</returns>
      [HttpPost]
      [ValidateAntiForgeryToken]
      [Authorize (Roles = "Editor")]
      public ActionResult Update (int entryId, string text, int? order, int? number)
      {
         var passage = this.m_Db.PassageEntries.Get (entryId);
         if (passage == null)
         {
            Response.StatusCode = 500;
            return Json ("Data is not valid.");
         }
         passage.Passage.Text = text;
         if (order != null)
            passage.Order = order.Value;
         if (number != null)
            passage.Number = number.Value;
         this.m_Db.Save ();
         return Json ("Success");
      }

      /// <summary>
      /// Deletes the given passage.
      /// </summary>
      /// <param name="entryId">The passage entry to delete.</param>
      /// <returns>The edit page.</returns>
      [HttpPost]
      [ValidateAntiForgeryToken]
      [Authorize (Roles = "Creator")]
      public ActionResult Delete (int entryId)
      {
         if (ModelState.IsValid)
         {
            var passage = this.m_Db.PassageEntries.Get (entryId);
            passage.Chapter.Passages.Remove (passage);
            if (passage.Passage.Entries.Count == 1)
            {
               this.m_Db.Passages.Delete(passage.Passage);
            }
            this.m_Db.Save();
            return Json ("Success");
         }
         Response.StatusCode = 500;
         return Json ("Data is not valid.");
      }

      /// <summary>
      /// Gets the entry data for the given entry id.
      /// </summary>
      /// <param name="id">Id of entry to get text for.</param>
      /// <returns>The text of the entry.</returns>
      [AllowAnonymous]
      public ActionResult Get (int id)
      {
         var entry = this.m_Db.PassageEntries.Get (id);
         var result = new
         {
            entryId = id,
            order = entry.Order,
            passageId = entry.PassageId,
            passageNumber = entry.Number,
            passageText = entry.Passage.Text
         };
         return Json (result, JsonRequestBehavior.AllowGet);
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
      /// Gets a random passage.
      /// </summary>
      /// <returns></returns>
      public PassageViewModel GetRandomPassage ()
      {
         return new PassageViewModel(this.m_Db.PassageEntries.All (q => q.OrderBy (r => Guid.NewGuid ())).Take (1).FirstOrDefault());
      }
   }
}
