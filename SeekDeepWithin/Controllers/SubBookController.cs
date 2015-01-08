using System.Linq;
using System.Web.Mvc;
using SeekDeepWithin.DataAccess;
using SeekDeepWithin.Domain;
using SeekDeepWithin.Models;

namespace SeekDeepWithin.Controllers
{
   public class SubBookController : Controller
   {
      private readonly ISdwDatabase m_Db;

      /// <summary>
      /// Initializes a new book controller.
      /// </summary>
      public SubBookController ()
      {
         this.m_Db = new SdwDatabase ();
      }

      /// <summary>
      /// Initializes a new book controller with the given db info.
      /// </summary>
      /// <param name="db">Database object.</param>
      public SubBookController (ISdwDatabase db)
      {
         this.m_Db = db;
      }

      /// <summary>
      /// Gets the edit sub book Page.
      /// </summary>
      /// <param name="id">The id of the sub book to edit.</param>
      /// <returns>The edit page.</returns>
      [Authorize (Roles = "Editor")]
      public ActionResult Edit (int id)
      {
         if (Request.UrlReferrer != null) TempData["RefUrl"] = Request.UrlReferrer.ToString ();
         var subBook = this.m_Db.SubBooks.Get (id);
         return View (GetViewModel (subBook));
      }

      /// <summary>
      /// Performs an edit for the given model.
      /// </summary>
      /// <param name="viewModel">The view model with edits.</param>
      /// <returns>The edit page.</returns>
      [HttpPost]
      [ValidateAntiForgeryToken]
      [Authorize (Roles = "Editor")]
      public ActionResult Edit (SubBookViewModel viewModel)
      {
         if (ModelState.IsValid)
         {
            var subBook = this.m_Db.SubBooks.Get (viewModel.Id);
            this.m_Db.SetValues (subBook, viewModel);
            this.m_Db.Save ();
            return RedirectToAction ("About", new { id = viewModel.Id });
         }
         return View (viewModel);
      }

      /// <summary>
      /// Gets the create new sub book form.
      /// </summary>
      /// <param name="versionId">Version id the sub book is for.</param>
      /// <returns>The create new sub book form.</returns>
      [Authorize (Roles = "Creator")]
      public ActionResult Create (int versionId)
      {
         if (Request.UrlReferrer != null) TempData["RefUrl"] = Request.UrlReferrer.ToString ();
         return View (new SubBookViewModel {VersionId = versionId, Name = "default"});
      }

      /// <summary>
      /// Creates a new sub book.
      /// </summary>
      /// <returns>The results.</returns>
      [HttpPost]
      [ValidateAntiForgeryToken]
      [Authorize (Roles = "Creator")]
      public ActionResult Create (SubBookViewModel viewModel)
      {
         var version = this.m_Db.Versions.Get (viewModel.VersionId);
         var subBook = new SubBook
         {
            Name = viewModel.Name,
            Order = viewModel.Order,
            Version = version
         };
         version.SubBooks.Add (subBook);
         this.m_Db.SubBooks.Insert (subBook);
         this.m_Db.Save ();
         return RedirectToAction ("Edit", "Version", new {id = viewModel.VersionId});
      }

      /// <summary>
      /// Gets the assign writer view.
      /// </summary>
      /// <param name="id">Id of sub book to assign a writer to.</param>
      /// <returns>The assign writer view.</returns>
      [Authorize (Roles = "Editor")]
      public ActionResult AssignWriter (int id)
      {
         ViewBag.SubBookId = id;
         var subBook = this.m_Db.SubBooks.Get (id);
         ViewBag.Title = GetViewModel (subBook).Name;
         ViewBag.Authors = new SelectList (this.m_Db.Authors.All (), "Id", "Name");
         if (Request.UrlReferrer != null) TempData["RefUrl"] = Request.UrlReferrer.ToString ();
         return View ();
      }

      /// <summary>
      /// Assigns a writer to a version.
      /// </summary>
      /// <returns>The assignment result.</returns>
      [HttpPost]
      [ValidateAntiForgeryToken]
      [Authorize (Roles = "Editor")]
      public ActionResult AssignWriter (int subBookId, int authorId, bool isTranslator)
      {
         var subBook = this.m_Db.SubBooks.Get (subBookId);
         var author = this.m_Db.Authors.Get (authorId);
         var writer = new Writer
         {
            SubBook = subBook,
            IsTranslator = isTranslator,
            Author = author
         };
         subBook.Writers.Add (writer);
         this.m_Db.Save ();
         return RedirectToAction ("Edit", new { id = subBookId });
      }

      /// <summary>
      /// Converts the given sub book to a view model.
      /// </summary>
      /// <param name="subBook">Sub Book to convert to view model.</param>
      /// <param name="ignoreChapter">-1 if not to copy chapters, otherwise an id to ignore.</param>
      /// <param name="version">The sub book's version</param>
      /// <returns>The view model representation of the Sub book.</returns>
      public static SubBookViewModel GetViewModel (SubBook subBook, ChapterViewModel ignoreChapter = null, VersionViewModel version = null)
      {
         var viewModel = new SubBookViewModel
         {
            Id = subBook.Id,
            Name = subBook.Name,
            Order = subBook.Order,
            VersionId = subBook.Version.Id,
            Version = version
         };

         if (version == null)
            viewModel.Version = VersionController.GetViewModel (subBook.Version, ignoreChapter == null ? null : viewModel);

         foreach (var writer in subBook.Writers)
         {
            viewModel.Writers.Add (new WriterViewModel
            {
               IsTranslator = writer.IsTranslator,
               Id = writer.Author.Id,
               Name = writer.Author.Name
            });
         }

         if (ignoreChapter != null || version != null)
         {
            foreach (var chapter in subBook.Chapters.OrderBy (pe => pe.Order))
            {
               if (ignoreChapter != null)
               {
                  viewModel.Chapters.Add (chapter.Id == ignoreChapter.Id
                     ? ignoreChapter
                     : ChapterController.GetViewModel (chapter, false, viewModel));
               }
               else
                  viewModel.Chapters.Add (ChapterController.GetViewModel (chapter, false, viewModel));
            }
         }
         return viewModel;
      }
   }
}
