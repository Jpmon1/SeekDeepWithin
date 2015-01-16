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
            return Json ("Success");
         }
         return Json ("Data is not valid");
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
            Version = version
         };
         version.SubBooks.Add (subBook);
         this.m_Db.SubBooks.Insert (subBook);
         this.m_Db.Save ();
         return Json (subBook);
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
         ViewBag.Title = subBook.Name;
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
         return RedirectToAction ("Read", "Chapter", new { id = subBook.Version.DefaultReadChapter });
      }
   }
}
