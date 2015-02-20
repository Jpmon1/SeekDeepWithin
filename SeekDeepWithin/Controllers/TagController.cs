using System.Linq;
using System.Web.Mvc;
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
      public ActionResult Index ()
      {
         return View (this.m_Db.Tags.All().Select(t => new TagViewModel {Id = t.Id, Name = t.Name}));
      }

      /// <summary>
      /// Gets the view to create a new tag.
      /// </summary>
      /// <returns>The create tag view.</returns>
      public ActionResult Create ()
      {
         return View (new TagViewModel ());
      }

      /// <summary>
      /// Posts a new tag.
      /// </summary>
      /// <param name="viewModel">View model.</param>
      /// <returns>Creation result.</returns>
      [HttpPost]
      [ValidateAntiForgeryToken]
      public ActionResult Create (TagViewModel viewModel)
      {
         if (ModelState.IsValid)
         {
            this.m_Db.Tags.Insert (new Tag {Name = viewModel.Name});
            this.m_Db.Save ();
            this.RedirectToAction ("Index");
         }
         return View (viewModel);
      }
   }
}
