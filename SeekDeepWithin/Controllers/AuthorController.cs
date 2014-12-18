using System.Collections.ObjectModel;
using System.Linq;
using System.Web.Mvc;
using SeekDeepWithin.DataAccess;
using SeekDeepWithin.Domain;
using SeekDeepWithin.Models;

namespace SeekDeepWithin.Controllers
{
   public class AuthorController : Controller
   {
      private readonly ISdwDatabase m_Db;

      /// <summary>
      /// Initializes a new controller.
      /// </summary>
      public AuthorController ()
      {
         this.m_Db = new SdwDatabase ();
      }

      /// <summary>
      /// Initializes a new controller with the given db info.
      /// </summary>
      /// <param name="db">Database object.</param>
      public AuthorController (ISdwDatabase db)
      {
         this.m_Db = db;
      }

      /// <summary>
      /// Gets the index of authors.
      /// </summary>
      /// <returns></returns>
      public ActionResult Index ()
      {
         return View (this.m_Db.Authors.All ().Select (a => new AuthorViewModel { Id = a.Id, Name = a.Name }));
      }

      /// <summary>
      /// Gets the details view of the given author.
      /// </summary>
      /// <param name="id">Id of author to get details for.</param>
      /// <returns>The details view.</returns>
      public ActionResult Details (int id)
      {
         var author = this.m_Db.Authors.Get (id);
         var viewModel = new AuthorViewModel {
            Id = author.Id,
            Name = author.Name,
            About = author.About,
            Written = new Collection <VersionViewModel> ()
         };
         foreach (var writer in author.Writers) {
            viewModel.Written.Add (writer.Version.ToViewModel(false ));
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
         var author = this.m_Db.Authors.Get (id);
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
            var author = this.m_Db.Authors.Get (viewModel.Id);
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
      public ActionResult Create (AuthorViewModel viewModel)
      {
         this.m_Db.Authors.Insert (new Author { Name = viewModel.Name, About = viewModel.About });
         this.m_Db.Save ();
         return RedirectToAction ("Index");
      }
   }
}
