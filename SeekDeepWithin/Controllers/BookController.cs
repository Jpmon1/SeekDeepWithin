using System.Linq;
using System.Web.Mvc;
using PagedList;
using SeekDeepWithin.DataAccess;
using SeekDeepWithin.Pocos;
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
      public ActionResult Index (int? page)
      {
         int pageNumber = page ?? 1;
         if (TempData.ContainsKey ("ErrorMessage"))
            ViewBag.ErrorMessage = TempData["ErrorMessage"];
         ViewBag.Tags = this.m_Db.Tags.All (q => q.OrderBy (t => t.Name));
         return View (this.m_Db.Books.All (q => q.OrderBy (b => b.Title.StartsWith("The")?b.Title.Substring(3).Trim():b.Title))
            .Select (book => new BookViewModel (book, true))
            .ToPagedList (pageNumber, 10));
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
            var foundBook = this.m_Db.Books.Get (b => b.Title == viewModel.Title).FirstOrDefault();
            if (foundBook != null)
            {
               ViewBag.ErrorMessage = "A book with that title already exists, maybe you need to add a version?";
               return View (viewModel);
            }
            this.m_Db.Books.Insert (new Book { Summary = viewModel.Summary, Title = viewModel.Title, SubTitle = viewModel.SubTitle });
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
            ViewBag.Tags = new SelectList (this.m_Db.Tags.All (q => q.OrderBy (t => t.Name)), "Id", "Name");
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

      /// <summary>
      /// Adds the given tag to the given book.
      /// </summary>
      /// <param name="tagId">Id of tag to add.</param>
      /// <param name="id">Id of book to add tag to.</param>
      /// <returns></returns>
      [HttpPost]
      [ValidateAntiForgeryToken]
      [Authorize (Roles = "Editor")]
      public ActionResult AddTag (int tagId, int id)
      {
         var book = this.m_Db.Books.Get (id);
         var foundTag = book.Tags.FirstOrDefault (bt => bt.Tag.Id == tagId);
         if (foundTag != null)
         {
            Response.StatusCode = 500;
            return Json ("That tag is already assigned to the book.");
         }
         var tag = this.m_Db.Tags.Get (tagId);
         book.Tags.Add (new BookTag { Book = book, Tag = tag});
         this.m_Db.Save ();
         return Json ("success");
      }

      /// <summary>
      /// Removes the tag with the given id from the given book.
      /// </summary>
      /// <param name="tagId">Id of tag to remove.</param>
      /// <param name="id">Id of book to remove tag from.</param>
      /// <returns></returns>
      [HttpPost]
      [ValidateAntiForgeryToken]
      [Authorize (Roles = "Editor")]
      public ActionResult RemoveTag (int tagId, int id)
      {
         var book = this.m_Db.Books.Get (id);
         if (book == null)
         {
            Response.StatusCode = 500;
            return Json ("Unable to get book - " + id);
         }
         var bookTag = book.Tags.FirstOrDefault (bt => bt.Id == tagId);
         if (bookTag == null)
         {
            Response.StatusCode = 500;
            return Json ("Unable to find the given tag.");
         }
         book.Tags.Remove (bookTag);
         this.m_Db.Save ();
         return Json ("success");
      }

      /// <summary>
      /// Gets the detail for a book with the given id.
      /// </summary>
      /// <param name="id"></param>
      /// <returns></returns>
      public ActionResult Details (int id)
      {
         var book = this.m_Db.Books.Get (id);
         return View(new BookViewModel(book, true));
      }

      /// <summary>
      /// Gets auto complete items.
      /// </summary>
      /// <param name="title">Title to get auto complete items for.</param>
      /// <returns>The list of possible terms for the given item.</returns>
      public ActionResult AutoComplete (string title)
      {
         var result = new
         {
            suggestions = this.m_Db.Books.Get (t => t.Title.Contains (title))
                                                 .Select (t => new { value = t.Title, data = t.Id })
         };
         return Json (result, JsonRequestBehavior.AllowGet);
      }
   }
}
