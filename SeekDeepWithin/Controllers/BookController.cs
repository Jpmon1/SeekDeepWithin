using System.Linq;
using System.Web.Mvc;
using SeekDeepWithin.DataAccess;
using SeekDeepWithin.Domain;
using SeekDeepWithin.Models;

namespace SeekDeepWithin.Controllers
{
   /// <summary>
   /// Controller used for book actions.
   /// </summary>
   public class BookController : Controller
   {
      private readonly ISdwDatabase m_Db;

      /// <summary>
      /// Initializes a new book controller.
      /// </summary>
      public BookController ()
      {
         this.m_Db = new SdwDatabase ();
      }

      /// <summary>
      /// Initializes a new book controller with the given db info.
      /// </summary>
      /// <param name="db">Database object.</param>
      public BookController (ISdwDatabase db)
      {
         this.m_Db = db;
      }

      /// <summary>
      /// Gets the index page for the books
      /// </summary>
      public ActionResult Index ()
      {
         if (TempData.ContainsKey ("ErrorMessage"))
            ViewBag.ErrorMessage = TempData["ErrorMessage"];
         return View (this.m_Db.Books.All (q => q.OrderBy (b => b.Title)).Select (book => new BookViewModel (book, true)).ToList ());
      }

      /// <summary>
      /// Gets the page to create a new book.
      /// </summary>
      [Authorize (Roles = "Creator")]
      public ActionResult Create ()
      {
         return View (new BookViewModel ());
      }

      /// <summary>
      /// Posts a new book in the database.
      /// </summary>
      /// <param name="viewModel">The book information to create.n</param>
      /// <returns></returns>
      [HttpPost]
      [ValidateAntiForgeryToken]
      [Authorize (Roles = "Creator")]
      public ActionResult Create (BookViewModel viewModel)
      {
         if (ModelState.IsValid)
         {
            this.m_Db.Books.Insert (new Book { Summary = viewModel.Summary, Title = viewModel.Title });
            this.m_Db.Save ();
            return RedirectToAction ("Index");
         }
         return View (viewModel);
      }

      /// <summary>
      /// Begins an edit for a book.
      /// </summary>
      /// <param name="id">The id of the book to edit.</param>
      /// <returns>The edit view for a book.</returns>
      [Authorize (Roles = "Editor")]
      public ActionResult Edit (int id)
      {
         if (Request.IsAuthenticated)
         {
            var book = this.m_Db.Books.Get (id);
            if (book == null)
               return RedirectToAction ("Index");

            if (Request.UrlReferrer != null) TempData["RefUrl"] = Request.UrlReferrer.ToString ();
            return View (new BookViewModel (book));
         }
         TempData["ErrorMessage"] = "You must login to edit a book!";
         return RedirectToAction ("Index");
      }

      /// <summary>
      /// Posts a book edit in the database.
      /// </summary>
      /// <param name="bookViewModel">The book information to create.</param>
      /// <returns></returns>
      [HttpPost]
      [ValidateAntiForgeryToken]
      [Authorize (Roles = "Editor")]
      public ActionResult Edit (BookViewModel bookViewModel)
      {
         if (ModelState.IsValid)
         {
            var book = this.m_Db.Books.Get (bookViewModel.Id);
            this.m_Db.SetValues (book, bookViewModel);
            this.m_Db.Save ();
            return RedirectToAction ("Index");
         }
         return View (bookViewModel);
      }
   }
}
