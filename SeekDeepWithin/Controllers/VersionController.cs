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
         this.m_Db.Save();
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
         if (Request.UrlReferrer != null) TempData ["RefUrl"] = Request.UrlReferrer.ToString ();
         if (TempData.ContainsKey("ErrorMessage"))
            ViewBag.ErrorMessage = TempData["ErrorMessage"];
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
               return Redirect (TempData ["RefUrl"].ToString());
            }
            return RedirectToAction ("Read", "Chapter", new { id = viewModel.DefaultReadChapter });
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
         ViewBag.BookTitle = this.m_Db.Books.Get (bookId).Title;
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
            Title = viewModel.Title,
            PublishDate = viewModel.PublishDate
         };
         book.Versions.Add (version);
         this.m_Db.Versions.Insert (version);
         this.m_Db.Save ();

         var subBook = new SubBook { Name = "default", Version = version };
         this.m_Db.SubBooks.Insert (subBook);
         this.m_Db.Save ();

         var chapter = new Chapter { Name = "About", SubBook = subBook, DefaultToParagraph = true};
         this.m_Db.Chapters.Insert (chapter);
         this.m_Db.Save ();

         version.DefaultReadChapter = chapter.Id;
         version.Contents = "[{\"name\":\"default\"," +
                              "\"id\":" + subBook.Id + "," +
                              "\"hide\":true," +
                              "\"chapters\":[" +
                                    "{\"name\":\"About\"," +
                                     "\"id\":" + chapter.Id + "," +
                                     "\"show\":\"para\"}" +
                            "]}]";
         this.m_Db.Save ();

         return RedirectToAction ("Read", "Chapter", new { id = chapter.Id });
      }
   }
}
