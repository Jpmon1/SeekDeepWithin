using System.Web.Mvc;
using SeekDeepWithin.DataAccess;
using SeekDeepWithin.Domain;
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
      /// <param name="id">The id of the item.</param>
      /// <param name="entryId">The id of the entry we are getting details for.</param>
      /// <returns>The details page.</returns>
      public ActionResult Details (int id, int entryId)
      {
         ViewBag.PassageEntry = this.m_Db.PassageEntries.Get (entryId).ToViewModel ();
         return View (this.m_Db.Passages.Get (id).ToViewModel ());
      }

      /// <summary>
      /// Gets the edit Page.
      /// </summary>
      /// <param name="id">The id of the item to edit.</param>
      /// <returns>The edit page.</returns>
      [Authorize (Roles = "Editor")]
      public ActionResult Edit (int id)
      {
         if (Request.UrlReferrer != null) TempData["RefUrl"] = Request.UrlReferrer.ToString();
         var passage = this.m_Db.Passages.Get (id);
         return View (passage.ToViewModel ());
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
            return RedirectToAction ("Details", new { id = viewModel.Id });
         }
         return View (viewModel);
      }

      /// <summary>
      /// Gets the create view.
      /// </summary>
      /// <param name="chapterId">The id of the chapter to create a passage for.</param>
      /// <returns>The create view.</returns>
      [Authorize (Roles = "Editor")]
      public ActionResult Create (int chapterId)
      {
         if (Request.UrlReferrer != null) TempData["RefUrl"] = Request.UrlReferrer.ToString ();
         ViewBag.ChapterId = chapterId;
         return View ();
      }

      /// <summary>
      /// Creates a passage for the given chapter.
      /// </summary>
      /// <param name="chapterId">Chapter to create passage for.</param>
      /// <param name="order">Order of passage.</param>
      /// <param name="number">Number of passage.</param>
      /// <param name="text">Text of passage.</param>
      /// <returns>Create results.</returns>
      [HttpPost]
      [ValidateAntiForgeryToken]
      [Authorize (Roles = "Editor")]
      public ActionResult Create (int chapterId, int order, int number, string text)
      {
         var chapter = this.m_Db.Chapters.Get (chapterId);
         var passageEntry = new PassageEntry
         {
            Chapter = chapter,
            ChapterId = chapterId,
            Number = number,
            Order = order,
            Passage = new Passage {Text = text}
         };
         this.m_Db.Passages.Insert (passageEntry.Passage);
         chapter.Passages.Add (passageEntry);
         this.m_Db.Save ();
         return RedirectToAction("Read", "Chapter", new { id = chapterId });
      }
   }
}
