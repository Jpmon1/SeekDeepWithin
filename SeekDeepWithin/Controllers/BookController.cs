using System.Collections.ObjectModel;
using System.Linq;
using System.Web.Mvc;
using SeekDeepWithin.DataAccess;
using SeekDeepWithin.Pocos;
using SeekDeepWithin.Models;
using SeekDeepWithin.SdwSearch;

namespace SeekDeepWithin.Controllers
{
   /// <summary>
   /// Controller used for book actions.
   /// </summary>
   public class BookController : SdwController
   {
      /// <summary>
      /// Initializes a new book controller.
      /// </summary>
      public BookController () : base (new SdwDatabase ()) { }

      /// <summary>
      /// Initializes a new book controller with the given db info.
      /// </summary>
      /// <param name="db">Database object.</param>
      public BookController (ISdwDatabase db) : base (db) { }

      /// <summary>
      /// Gets the index page for the books
      /// </summary>
      public ActionResult Index (int? page)
      {
         if (TempData.ContainsKey ("ErrorMessage"))
            ViewBag.ErrorMessage = TempData["ErrorMessage"];
         var books = this.Database.Books.All (q => q.OrderBy (b => b.Title.StartsWith ("The") ? b.Title.Substring (3).Trim () : b.Title));
         var viewModel = new PagedViewModel<BookViewModel> { PageNumber = page ?? 1, ItemsOnPage = 12, TotalHits = books.Count};
         viewModel.AddRange (books.Skip ((viewModel.PageNumber - 1) * viewModel.ItemsOnPage)
            .Take (viewModel.ItemsOnPage)
            .Select (book => new BookViewModel (book, true)));
         return View (viewModel);
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
      /// <param name="title">The title of the book.</param>
      /// <param name="subTitle">The sub title of the book.</param>
      /// <param name="summary">A summary for the book.</param>
      /// <param name="termId">The id of the term to associate.</param>
      /// <returns>Json results</returns>
      [HttpPost]
      [ValidateAntiForgeryToken]
      [Authorize (Roles = "Creator")]
      public ActionResult Create (string title, string subTitle, string summary, int termId)
      {
         if (string.IsNullOrWhiteSpace (title))
            return this.Fail ("A title must be specified.");
         if (string.IsNullOrWhiteSpace (summary))
            return this.Fail ("A summary must be specified.");

         var term = this.Database.Terms.Get (termId);
         var foundBook = this.Database.Books.Get (b => b.Title == title).FirstOrDefault ();
         if (foundBook != null)
            return this.Fail ("A book with that title already exists, maybe you need to add a version?");
         if (term == null)
            return this.Fail ("Unable to determine the associated term.");

         var book = new Book { Summary = summary, Title = title, SubTitle = subTitle, Term = term };
         this.Database.Books.Insert (book);
         this.Database.Save ();
         if (term.Links == null) term.Links = new Collection<TermLink> ();
         term.Links.Add (new TermLink { LinkType = (int)TermLinkType.Book, RefId = book.Id, Name = title });
         this.Database.Save ();
         BookSearch.AddOrUpdateIndex (book);
         return Json ("success");
      }

      /// <summary>
      /// Begins an edit for a book.
      /// </summary>
      /// <param name="id">The id of the book to edit.</param>
      /// <returns>The edit view for a book.</returns>
      [Authorize (Roles = "Editor")]
      public ActionResult Edit (int id)
      {
         return View (new BookViewModel (this.Database.Books.Get (id), true));
      }

      /// <summary>
      /// Posts a book edit in the database.
      /// </summary>
      /// <param name="id">Id of the book to edit.</param>
      /// <param name="title">The title of the book.</param>
      /// <param name="subTitle">The sub title of the book.</param>
      /// <param name="summary">A summary for the book.</param>
      /// <param name="termId">The id of the term to associate.</param>
      /// <returns>Json results</returns>
      [HttpPost]
      [ValidateAntiForgeryToken]
      [Authorize (Roles = "Editor")]
      public ActionResult Edit (int id, string title, string subTitle, string summary, int termId)
      {
         var book = this.Database.Books.Get (id);
         if (book == null)
            return this.Fail ("Unable to determine the book to edit.");
         book.Title = title;
         book.SubTitle = subTitle;
         book.Summary = summary;
         var term = book.Term;
         var link = term.Links.FirstOrDefault (l => l.LinkType == (int)TermLinkType.Book && l.RefId == id);
         if (link != null) term.Links.Remove (link);
         if (book.Term.Id != termId)
         {
            term = this.Database.Terms.Get (termId);
            book.Term = term;
         }
         term.Links.Add (new TermLink { LinkType = (int)TermLinkType.Book, RefId = book.Id });
         this.Database.Save ();
         BookSearch.AddOrUpdateIndex (book);
         return Json ("success");
      }

      /// <summary>
      /// Sets the default version for a book.
      /// </summary>
      /// <param name="bookId">The id of the book to set the default version for.</param>
      /// <param name="versionId">The id of the version to set as default.</param>
      /// <returns>Json results</returns>
      [HttpPost]
      [ValidateAntiForgeryToken]
      [Authorize (Roles = "Editor")]
      public ActionResult DefaultVersion (int bookId, int versionId)
      {
         var book = this.Database.Books.Get (bookId);
         var version = this.Database.Versions.Get (versionId);
         if (book == null) return this.Fail ("Unable to determine the book.");
         if (version == null) return this.Fail ("Unable to determine the version.");
         book.DefaultVersion = version;
         this.Database.Save ();
         return Json ("success");
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
            suggestions = this.Database.Books.Get (t => t.Title.Contains (title))
                                             .Select (t => new { value = t.Title, data = t.Id })
         };
         return Json (result, JsonRequestBehavior.AllowGet);
      }
   }
}
