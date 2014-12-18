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
      /// Gets the details page for the given sub book.
      /// </summary>
      /// <param name="id">The id of the sub book.</param>
      /// <returns>The details page.</returns>
      public ActionResult Details (int id)
      {
         return View (this.m_Db.SubBooks.Get (id).ToViewModel ());
      }

      /// <summary>
      /// Gets the edit sub book Page.
      /// </summary>
      /// <param name="id">The id of the sub book to edit.</param>
      /// <returns>The edit page.</returns>
      [Authorize (Roles = "EditBook")]
      public ActionResult Edit (int id)
      {
         if (Request.UrlReferrer != null) TempData["RefUrl"] = Request.UrlReferrer.ToString ();
         var subBook = this.m_Db.SubBooks.Get (id);
         return View (subBook.ToViewModel ());
      }

      /// <summary>
      /// Performs an edit for the given model.
      /// </summary>
      /// <param name="viewModel">The view model with edits.</param>
      /// <returns>The edit page.</returns>
      [HttpPost]
      [ValidateAntiForgeryToken]
      [Authorize (Roles = "EditBook")]
      public ActionResult Edit (SubBookViewModel viewModel)
      {
         if (ModelState.IsValid)
         {
            var subBook = this.m_Db.SubBooks.Get (viewModel.Id);
            this.m_Db.SetValues (subBook, viewModel);
            this.m_Db.Save ();
            return RedirectToAction ("Details", new { id = viewModel.Id });
         }
         return View (viewModel);
      }

      /// <summary>
      /// Gets the create new sub book form.
      /// </summary>
      /// <param name="versionId">Version id the sub book is for.</param>
      /// <returns>The create new sub book form.</returns>
      [Authorize (Roles = "AddBook")]
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
      [Authorize (Roles = "AddBook")]
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
         return RedirectToAction ("About", "Version", new {id = viewModel.VersionId});
      }
   }
}
