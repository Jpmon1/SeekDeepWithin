using System.Linq;
using System.Web.Mvc;
using PagedList;
using SeekDeepWithin.DataAccess;
using SeekDeepWithin.Pocos;
using SeekDeepWithin.Models;

namespace SeekDeepWithin.Controllers
{
   /// <summary>
   /// Controller for tag operations.
   /// </summary>
   public class TagController : Controller
   {
      private readonly ISdwDatabase m_Db;

      /// <summary>
      /// Initializes a new controller.
      /// </summary>
      public TagController ()
      {
         this.m_Db = new SdwDatabase ();
      }

      /// <summary>
      /// Initializes a new controller with the given db info.
      /// </summary>
      /// <param name="db">Database object.</param>
      public TagController (ISdwDatabase db)
      {
         this.m_Db = db;
      }

      /// <summary>
      /// Gets the list of tags.
      /// </summary>
      /// <returns>The view for the list of tags</returns>
      public ActionResult Index (int? page)
      {
         var pageNumber = page ?? 1;
         return View (this.m_Db.Tags.All ()
            .Select (t => new TagViewModel { Id = t.Id, Name = t.Name })
            .ToPagedList (pageNumber, 75));
      }

      /// <summary>
      /// Gets the view to create a new tag.
      /// </summary>
      /// <returns>The create tag view.</returns>
      [Authorize (Roles = "Creator")]
      public ActionResult Create (string returnUrl)
      {
         ViewBag.ReturnUrl = returnUrl;
         return View (new TagViewModel ());
      }

      /// <summary>
      /// Posts a new tag.
      /// </summary>
      /// <param name="viewModel">View model.</param>
      /// <param name="returnUrl">Url to return to.</param>
      /// <returns>Creation result.</returns>
      [HttpPost]
      [ValidateAntiForgeryToken]
      [Authorize (Roles = "Creator")]
      public ActionResult Create (TagViewModel viewModel, string returnUrl)
      {
         if (ModelState.IsValid)
         {
            var foundTag = this.m_Db.Tags.Get (t => t.Name == viewModel.Name).FirstOrDefault ();
            if (foundTag != null)
            {
               ViewBag.ErrorMessage = "A tag with that name already exists.";
               return View (viewModel);
            }
            this.m_Db.Tags.Insert (new Tag {Name = viewModel.Name});
            this.m_Db.Save ();
            if (!string.IsNullOrWhiteSpace (returnUrl))
               return Redirect (returnUrl);
            this.RedirectToAction ("Index");
         }
         return View (viewModel);
      }

      /// <summary>
      /// Gets the details page of a tag.
      /// </summary>
      /// <param name="id"></param>
      /// <returns></returns>
      public ActionResult Details (int id)
      {
         var tag = this.m_Db.Tags.Get (id);
         var viewModel = new TagViewModel {Id = tag.Id, Name = tag.Name};
         var books = this.m_Db.Books.Get (b => b.Tags.Any (t => t.Tag.Id == id));
         foreach (var book in books)
            viewModel.Books.Add(book.Id, book.Title);
         var terms = this.m_Db.GlossaryTerms.Get (b => b.Tags.Any (t => t.Tag.Id == id));
         foreach (var term in terms)
            viewModel.Terms.Add (term.Id, term.Name);
         return View (viewModel);
      }
   }
}
