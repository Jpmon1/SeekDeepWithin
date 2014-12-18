using System.Web.Mvc;
using SeekDeepWithin.DataAccess;
using SeekDeepWithin.Domain;
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
      /// Gets the about Page.
      /// </summary>
      /// <param name="id">The id of the version.</param>
      /// <returns>The about page.</returns>
      public ActionResult About (int id)
      {
         return View (this.m_Db.Versions.Get (id).ToViewModel ());
      }

      /// <summary>
      /// Gets the edit version Page.
      /// </summary>
      /// <param name="id">The id of the version to edit.</param>
      /// <returns>The edit page.</returns>
      [Authorize (Roles = "Editor")]
      public ActionResult Edit (int id)
      {
         if (Request.UrlReferrer != null) TempData ["RefUrl"] = Request.UrlReferrer.ToString ();
         return View (this.m_Db.Versions.Get (id).ToViewModel ());
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
               return Redirect (TempData ["RefUrl"].ToString());
            }
            return RedirectToAction ("About", new { id = viewModel.Id });
         }
         return View (viewModel);
      }

      /// <summary>
      /// Gets the create new version view.
      /// </summary>
      /// <param name="bookId">Id of book to add version for.</param>
      /// <returns>Create version view.</returns>
      [Authorize (Roles = "Creator")]
      public ActionResult Create (int bookId)
      {
         ViewBag.BookId = bookId;
         ViewBag.Book = this.m_Db.Books.Get (bookId).ToViewModel (false);
         return View (new VersionViewModel { BookId = bookId });
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
            Name = viewModel.Name,
            PublishDate = viewModel.PublishDate,
            Summary = viewModel.Summary,
            TitleFormat = viewModel.TitleFormat
         };
         book.Versions.Add (version);
         this.m_Db.Versions.Insert (version);
         this.m_Db.Save ();
         return RedirectToAction ("About", new { id = version.Id });
      }

      /// <summary>
      /// Gets the assign writer view.
      /// </summary>
      /// <param name="id">Id of version to assign a writer to.</param>
      /// <returns>The assign writer view.</returns>
      [Authorize (Roles = "Editor")]
      public ActionResult AssignWriter (int id)
      {
         ViewBag.VersionId = id;
         if (Request.UrlReferrer != null) TempData["RefUrl"] = Request.UrlReferrer.ToString ();
         var version = this.m_Db.Versions.Get (id);
         ViewBag.VersionTitle = version.ToViewModel (false).Title;
         ViewBag.Authors = new SelectList (this.m_Db.Authors.All (), "Id", "Name");
         return View ();
      }

      /// <summary>
      /// Assigns a writer to a version.
      /// </summary>
      /// <returns>The assignment result.</returns>
      [HttpPost]
      [ValidateAntiForgeryToken]
      [Authorize (Roles = "Editor")]
      public ActionResult AssignWriter (int versionId, int authorId, bool isTranslator)
      {
         var version = this.m_Db.Versions.Get (versionId);
         var author = this.m_Db.Authors.Get (authorId);
         var writer = new Writer
         {
            Version = version,
            IsTranslator = isTranslator,
            Author = author
         };
         version.Writers.Add (writer);
         this.m_Db.Save ();
         return RedirectToAction ("About", new { id = versionId });
      }
   }
}
