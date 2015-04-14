using System.Collections.ObjectModel;
using System.Linq;
using System.Web.Mvc;
using SeekDeepWithin.DataAccess;
using SeekDeepWithin.Pocos;
using SeekDeepWithin.Models;

namespace SeekDeepWithin.Controllers
{
   /// <summary>
   /// Controller for versions.
   /// </summary>
   public class VersionController : Controller
   {
      private readonly ISdwDatabase m_Db;

      /// <summary>
      /// Initializes a new book controller.
      /// </summary>
      public VersionController ()
      {
         this.m_Db = new SdwDatabase ();
      }

      /// <summary>
      /// Initializes a new book controller with the given db info.
      /// </summary>
      /// <param name="db">Database object.</param>
      public VersionController (ISdwDatabase db)
      {
         this.m_Db = db;
      }

      /// <summary>
      /// Gets the edit version contents page.
      /// </summary>
      /// <param name="id">The id of the version to edit.</param>
      /// <returns>The edit contents page.</returns>
      [Authorize (Roles = "Creator")]
      public ActionResult Contents (int id)
      {
         var viewModel = new VersionViewModel (this.m_Db.Versions.Get (id));
         if (Request.UrlReferrer != null) TempData["RefUrl"] = Request.UrlReferrer.ToString ();
         return View (viewModel);
      }

      /// <summary>
      /// Gets the edit version contents page.
      /// </summary>
      /// <param name="id">The id of the version to edit.</param>
      /// <param name="contents">The new contents.</param>
      /// <returns>The edit contents page.</returns>
      [HttpPost]
      [ValidateAntiForgeryToken]
      [Authorize (Roles = "Creator")]
      public ActionResult UpdateContents (int id, string contents)
      {
         var version = this.m_Db.Versions.Get (id);
         version.Contents = contents;
         this.m_Db.Save ();
         return Json ("Success");
      }

      /// <summary>
      /// Gets the edit version page.
      /// </summary>
      /// <param name="id">The id of the version to edit.</param>
      /// <returns>The edit page.</returns>
      [Authorize (Roles = "Editor")]
      public ActionResult Edit (int id)
      {
         if (Request.UrlReferrer != null) TempData["RefUrl"] = Request.UrlReferrer.ToString ();
         if (TempData.ContainsKey ("ErrorMessage"))
            ViewBag.ErrorMessage = TempData["ErrorMessage"];
         ViewBag.Writers = new SelectList (this.m_Db.Writers.All (), "Id", "Name");
         var viewModel = new VersionViewModel (this.m_Db.Versions.Get (id));
         return View (viewModel);
      }

      /// <summary>
      /// Gets the edit version Page.
      /// </summary>
      /// <param name="viewModel">The view model with edits.</param>
      /// <returns>The edit version page.</returns>
      [HttpPost]
      [ValidateAntiForgeryToken]
      [Authorize (Roles = "Editor")]
      public ActionResult Edit (VersionViewModel viewModel)
      {
         if (ModelState.IsValid)
         {
            var version = this.m_Db.Versions.Get (viewModel.Id);
            this.m_Db.SetValues (version, viewModel);
            this.m_Db.Save ();
            if (TempData.ContainsKey ("RefUrl"))
            {
               TempData.Remove ("RefUrl");
               return Redirect (TempData["RefUrl"].ToString ());
            }
            return RedirectToAction ("Index", "Read", new { id = viewModel.DefaultReadChapter });
         }
         return View (viewModel);
      }

      /// <summary>
      /// Assigns a writer to a sub book.
      /// </summary>
      /// <returns>The result.</returns>
      [HttpPost]
      [ValidateAntiForgeryToken]
      [Authorize (Roles = "Editor")]
      public ActionResult AssignWriter (int id, int writerId, bool isTranslator)
      {
         var version = this.m_Db.Versions.Get (id);
         var author = this.m_Db.Writers.Get (writerId);
         var writer = new VersionWriter
         {
            Version = version,
            IsTranslator = isTranslator,
            Writer = author
         };
         version.Writers.Add (writer);
         this.m_Db.Save ();
         return Json (new { writerId = writer.Id, id, writer = author.Name });
         //return RedirectToAction ("Details", new { id = subBook.Id });
      }

      /// <summary>
      /// Removes a writer from a sub book.
      /// </summary>
      /// <returns>The result.</returns>
      [HttpPost]
      [ValidateAntiForgeryToken]
      [Authorize (Roles = "Editor")]
      public ActionResult RemoveWriter (int subBookId, int writerId)
      {
         var version = this.m_Db.Versions.Get (subBookId);
         if (version == null)
         {
            Response.StatusCode = 500;
            return Json ("Unknown version");
         }
         var writer = version.Writers.FirstOrDefault (w => w.Id == writerId);
         if (writer == null)
         {
            Response.StatusCode = 500;
            return Json ("Unknown writer");
         }
         version.Writers.Remove (writer);
         this.m_Db.Save ();
         return Json ("success");
      }

      /// <summary>
      /// Gets the create new version view.
      /// </summary>
      /// <param name="bookId">Id of book to add version for.</param>
      /// <returns>Create version view.</returns>
      [Authorize (Roles = "Creator")]
      public ActionResult Create (int bookId)
      {
         ViewBag.BookTitle = this.m_Db.Books.Get (bookId).Title;
         return View (new VersionViewModel { BookId = bookId });
      }

      /// <summary>
      /// Sets the default read chapter for the given version..
      /// </summary>
      /// <param name="id">The version id.</param>
      /// <param name="chapterId">The chapter id.</param>
      /// <returns>Create version view.</returns>
      [HttpPost]
      [ValidateAntiForgeryToken]
      [Authorize (Roles = "Creator")]
      public ActionResult DefaultReadChapter (int id, int chapterId)
      {
         var version = this.m_Db.Versions.Get (id);
         if (version == null)
         {
            Response.StatusCode = 500;
            return Json ("Unknown version");
         }
         version.DefaultReadChapter = chapterId;
         this.m_Db.Save ();
         return Json("success");
      }

      /// <summary>
      /// Gets the create new version view.
      /// </summary>
      /// <param name="viewModel">The version view model.</param>
      /// <returns>Create version view.</returns>
      [HttpPost]
      [ValidateAntiForgeryToken]
      [Authorize (Roles = "Creator")]
      public ActionResult Create (VersionViewModel viewModel)
      {
         var book = this.m_Db.Books.Get (viewModel.BookId);
         var version = new Version
         {
            Book = book,
            Title = viewModel.Title,
            PublishDate = viewModel.PublishDate,
            Abbreviation = viewModel.Abbreviation,
            SubBooks = new Collection <VersionSubBook> ()
         };
         book.Versions.Add (version);
         this.m_Db.Versions.Insert (version);
         this.m_Db.Save ();
         return RedirectToAction ("Contents", new { id = version.Id });
      }

      /// <summary>
      /// Sets the default version for a book.
      /// </summary>
      /// <param name="bookId"></param>
      /// <param name="versionId"></param>
      /// <returns></returns>
      [HttpPost]
      [ValidateAntiForgeryToken]
      [Authorize (Roles = "Editor")]
      public ActionResult SetDefaultVersion (int bookId, int versionId)
      {
         var book = this.m_Db.Books.Get (bookId);
         foreach (var version in book.Versions)
            version.IsDefault = version.Id == versionId;
         this.m_Db.Save();
         return Json ("success");
      }

      /// <summary>
      /// Gets auto complete items.
      /// </summary>
      /// <param name="title">Title to get auto complete items for.</param>
      /// <param name="bookId">The id of the book to look up versions for.</param>
      /// <returns>The list of possible items.</returns>
      public ActionResult AutoComplete (string title, int bookId)
      {
         var result = new
         {
            suggestions = this.m_Db.Versions.Get (v => v.Book.Id == bookId && v.Title.Contains (title))
                                                 .Select (v => new { value = v.Title, data = v.Id })
         };
         return Json (result, JsonRequestBehavior.AllowGet);
      }
   }
}
