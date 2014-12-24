using System.Linq;
using System.Web.Mvc;
using SeekDeepWithin.DataAccess;
using SeekDeepWithin.Domain;
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
         return View (chapter.ToViewModel ());
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
         return View (chapter.ToViewModel ());
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
            return RedirectToAction ("Read", new { id = viewModel.Id });
         }
         return View (viewModel);
      }

      /// <summary>
      /// Gets the add passage for chapter Page.
      /// </summary>
      /// <param name="id">The id of the chapter to add passages for.</param>
      /// <returns>The add page.</returns>
      [Authorize (Roles = "Creator")]
      public ActionResult Add (int id)
      {
         if (Request.UrlReferrer != null) TempData["RefUrl"] = Request.UrlReferrer.ToString ();
         var viewModel = new AddPassageViewModel {ChapterId = id};
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
      /// Gets the create new chapter view.
      /// </summary>
      /// <param name="versionId">Version id to create the chapter for.</param>
      /// <returns>The create chapter view.</returns>
      [Authorize (Roles = "Creator")]
      public ActionResult Create (int versionId)
      {
         if (Request.UrlReferrer != null) TempData["RefUrl"] = Request.UrlReferrer.ToString ();
         var subBooks = this.m_Db.SubBooks.Get (sb => sb.Version.Id == versionId);
         ViewBag.VersionId = versionId;
         ViewBag.SubBooks = new SelectList (subBooks, "Id", "Name");
         return View ();
      }

      /// <summary>
      /// Creates a new chapter.
      /// </summary>
      /// <returns>The results.</returns>
      [HttpPost]
      [ValidateAntiForgeryToken]
      [Authorize (Roles = "Creator")]
      public ActionResult Create (int subBookId, string name, int order, int versionId)
      {
         var subBook = this.m_Db.SubBooks.Get (subBookId);
         var chapter = new Chapter
         {
            Name = name,
            Order = order,
            SubBook = subBook
         };
         subBook.Chapters.Add (chapter);
         this.m_Db.Chapters.Insert (chapter);
         this.m_Db.Save ();
         return RedirectToAction ("About", "Version", new {id = versionId});
      }
   }
}
