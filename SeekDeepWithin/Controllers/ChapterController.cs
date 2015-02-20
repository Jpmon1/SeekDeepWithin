using System.Linq;
using System.Web.Mvc;
using SeekDeepWithin.DataAccess;
using SeekDeepWithin.Pocos;
using SeekDeepWithin.Models;

namespace SeekDeepWithin.Controllers
{
   public class ChapterController : Controller
   {
      private readonly ISdwDatabase m_Db;

      /// <summary>
      /// Initializes a new chapter controller.
      /// </summary>
      public ChapterController ()
      {
         this.m_Db = new SdwDatabase ();
      }

      /// <summary>
      /// Initializes a new chapter controller with the given db info.
      /// </summary>
      /// <param name="db">Database object.</param>
      public ChapterController (ISdwDatabase db)
      {
         this.m_Db = db;
      }

      /// <summary>
      /// Gets the read chapter Page.
      /// </summary>
      /// <param name="id">The id of the chapter to read.</param>
      /// <returns>The read page.</returns>
      public ActionResult Read (int id)
      {
         var chapter = this.m_Db.Chapters.Get (id);
         return View (new ChapterViewModel (chapter));
      }

      /// <summary>
      /// Gets the edit chapter Page.
      /// </summary>
      /// <param name="id">The id of the chapter to edit.</param>
      /// <returns>The edit page.</returns>
      [Authorize (Roles = "Editor")]
      public ActionResult Edit (int id)
      {
         if (Request.UrlReferrer != null) TempData["RefUrl"] = Request.UrlReferrer.ToString ();
         var chapter = this.m_Db.Chapters.Get (id);
         return View (new ChapterViewModel (chapter));
      }

      /// <summary>
      /// Performs an edit for the given model.
      /// </summary>
      /// <param name="viewModel">The view model with edits.</param>
      /// <returns>The edit page.</returns>
      [HttpPost]
      [ValidateAntiForgeryToken]
      [Authorize (Roles = "Editor")]
      public ActionResult Edit (ChapterViewModel viewModel)
      {
         if (ModelState.IsValid)
         {
            var chapter = this.m_Db.Chapters.Get (viewModel.Id);
            this.m_Db.SetValues (chapter, viewModel);
            this.m_Db.Save ();
            return Json ("Success");
         }
         return Json ("Data is not valid.");
      }

      /// <summary>
      /// Gets the add passage view for a chapter.
      /// </summary>
      /// <param name="id">The id of the chapter to add passages for.</param>
      /// <returns>The add passge view.</returns>
      [Authorize (Roles = "Creator")]
      public ActionResult Add (int id)
      {
         if (Request.UrlReferrer != null) TempData["RefUrl"] = Request.UrlReferrer.ToString ();
         var viewModel = new AddItemViewModel { ParentId = id, ItemType = ItemType.Passage };
         var chapter = this.m_Db.Chapters.Get (id);
         if (chapter.Passages.Count > 0)
         {
            viewModel.Order = chapter.Passages.Max (p => p.Order) + 1;
            viewModel.Number = chapter.Passages.Max (p => p.Number) + 1;
         }
         else
         {
            viewModel.Order = 1;
            viewModel.Number = 1;
         }
         return View (viewModel);
      }

      /// <summary>
      /// Creates a new chapter.
      /// </summary>
      /// <returns>The results.</returns>
      [HttpPost]
      [ValidateAntiForgeryToken]
      [Authorize (Roles = "Creator")]
      public ActionResult Create (int subBookId, string name)
      {
         var subBook = this.m_Db.SubBooks.Get (subBookId);
         var chapter = new Chapter
         {
            Name = name,
            SubBook = subBook
         };
         subBook.Chapters.Add (chapter);
         this.m_Db.Chapters.Insert (chapter);
         this.m_Db.Save ();
         return Json (new {id=chapter.Id});
      }
   }
}
