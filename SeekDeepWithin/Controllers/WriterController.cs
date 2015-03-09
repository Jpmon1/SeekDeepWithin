using System.Linq;
using System.Web.Mvc;
using PagedList;
using SeekDeepWithin.DataAccess;
using SeekDeepWithin.Pocos;
using SeekDeepWithin.Models;

namespace SeekDeepWithin.Controllers
{
   public class WriterController : Controller
   {
      private readonly ISdwDatabase m_Db;

      /// <summary>
      /// Initializes a new controller.
      /// </summary>
      public WriterController ()
      {
         this.m_Db = new SdwDatabase ();
      }

      /// <summary>
      /// Initializes a new controller with the given db info.
      /// </summary>
      /// <param name="db">Database object.</param>
      public WriterController (ISdwDatabase db)
      {
         this.m_Db = db;
      }

      /// <summary>
      /// Gets the index of authors.
      /// </summary>
      /// <returns></returns>
      public ActionResult Index (int? page)
      {
         var pageNumber = page ?? 1;
         return View (this.m_Db.Writers.All ()
            .Select (a => new AuthorViewModel { Id = a.Id, Name = a.Name })
            .ToPagedList(pageNumber, 75));
      }

      /// <summary>
      /// Gets the details view of the given author.
      /// </summary>
      /// <param name="id">Id of author to get details for.</param>
      /// <returns>The details view.</returns>
      public ActionResult Details (int id)
      {
         var writer = this.m_Db.Writers.Get (id);
         var subBooks = this.m_Db.SubBookWriters.Get (sb => sb.Writer.Id == id);
         var viewModel = new AuthorViewModel {
            Id = writer.Id,
            Name = writer.Name,
            About = writer.About
         };
         foreach (var subBook in subBooks)
         {
            if (subBook.SubBook != null && subBook.SubBook.Versions != null)
            {
               foreach (var version in subBook.SubBook.Versions)
               {
                  if (viewModel.Written.All (w => w.VersionId != version.Version.Id))
                     viewModel.Written.Add (new SubBookViewModel (version));
               }
            }
         }
         return View (viewModel);
      }

      /// <summary>
      /// Gets the edit author view.
      /// </summary>
      /// <param name="id">The id of the author to edit.</param>
      /// <returns>The edit author view.</returns>
      [Authorize (Roles = "Editor")]
      public ActionResult Edit (int id)
      {
         var author = this.m_Db.Writers.Get (id);
         if (Request.UrlReferrer != null) TempData["RefUrl"] = Request.UrlReferrer.ToString ();
         return View (new AuthorViewModel {About = author.About, Id = author.Id, Name = author.Name});
      }

      /// <summary>
      /// Gets the edit version Page.
      /// </summary>
      /// <param name="viewModel">The view model with edits.</param>
      /// <returns>The edit version page.</returns>
      [HttpPost]
      [ValidateAntiForgeryToken]
      [Authorize (Roles = "Editor")]
      public ActionResult Edit (AuthorViewModel viewModel)
      {
         if (ModelState.IsValid)
         {
            var author = this.m_Db.Writers.Get (viewModel.Id);
            this.m_Db.SetValues (author, viewModel);
            this.m_Db.Save ();
            if (TempData.ContainsKey ("RefUrl"))
            {
               TempData.Remove ("RefUrl");
               return Redirect (TempData["RefUrl"].ToString ());
            }
            return RedirectToAction ("Details", new { id = viewModel.Id });
         }
         return View (viewModel);
      }

      /// <summary>
      /// Gets the create view.
      /// </summary>
      /// <returns>The create view.</returns>
      [Authorize (Roles = "Editor")]
      public ActionResult Create ()
      {
         if (Request.UrlReferrer != null) TempData["RefUrl"] = Request.UrlReferrer.ToString ();
         return View ();
      }

      /// <summary>
      /// Creates a new author.
      /// </summary>
      /// <returns>The create view.</returns>
      [HttpPost]
      [ValidateAntiForgeryToken]
      [Authorize (Roles = "Editor")]
      public ActionResult Create (AuthorViewModel viewModel, string refUrl)
      {
         var foundAuthor = this.m_Db.Writers.Get (a => a.Name == viewModel.Name).FirstOrDefault ();
         if (foundAuthor != null)
         {
            ViewBag.ErrorMessage = "A writer with that name already exists.";
            return View (viewModel);
         }
         this.m_Db.Writers.Insert (new Writer { Name = viewModel.Name, About = viewModel.About });
         this.m_Db.Save ();
         return Redirect (refUrl);
      }
   }
}
