using System.Linq;
using System.Web.Mvc;
using SeekDeepWithin.DataAccess;
using SeekDeepWithin.Models;
using SeekDeepWithin.Pocos;

namespace SeekDeepWithin.Controllers
{
   public class TermController : Controller
   {
      private readonly ISdwDatabase m_Db;

      /// <summary>
      /// Initializes a new controller.
      /// </summary>
      public TermController ()
      {
         this.m_Db = new SdwDatabase ();
      }

      /// <summary>
      /// Initializes a new controller with the given db info.
      /// </summary>
      /// <param name="db">Database object.</param>
      public TermController (ISdwDatabase db)
      {
         this.m_Db = db;
      }

      /// <summary>
      /// 
      /// </summary>
      /// <returns>The term view</returns>
      public ActionResult Index (int id)
      {
         var term = this.m_Db.GlossaryTerms.Get (id);
         var viewModel = new GlossaryTermViewModel (term);
         return View (viewModel);
      }

      /// <summary>
      /// Gets the create new glossary item view.
      /// </summary>
      /// <returns>Create new glossary item view.</returns>
      [Authorize (Roles = "Creator")]
      public ActionResult Create ()
      {
         if (Request.UrlReferrer != null) TempData["RefUrl"] = Request.UrlReferrer.ToString ();
         return View ();
      }

      /// <summary>
      /// Creates the given glossary item.
      /// </summary>
      /// <returns>Create new glossary item view.</returns>
      [HttpPost]
      [ValidateAntiForgeryToken]
      [Authorize (Roles = "Creator")]
      public ActionResult Create (GlossaryTermViewModel viewModel)
      {
         if (ModelState.IsValid)
         {
            var foundTerm = this.m_Db.GlossaryTerms.Get (t => t.Name == viewModel.Name).FirstOrDefault ();
            if (foundTerm != null)
            {
               ViewBag.ErrorMessage = "A term with that name already exists:";
               ViewBag.FoundTermId = foundTerm.Id;
               return View (viewModel);
            }
            var item = new GlossaryTerm { Name = viewModel.Name };
            this.m_Db.GlossaryTerms.Insert (item);
            this.m_Db.Save ();
            return RedirectToAction ("Index", new { id = item.Id });
         }
         return View (viewModel);
      }

      /// <summary>
      /// Edits a glossary term.
      /// </summary>
      /// <param name="id">The id of the term to edit.</param>
      /// <returns></returns>
      [Authorize (Roles = "Editor")]
      public ActionResult Edit (int id)
      {
         var term = this.m_Db.GlossaryTerms.Get (id);
         if (Request.UrlReferrer != null) TempData["RefUrl"] = Request.UrlReferrer.ToString ();
         ViewBag.Tags = new SelectList (this.m_Db.Tags.All (q => q.OrderBy (t => t.Name)), "Id", "Name");
         return View (new GlossaryTermViewModel (term));
      }

      /// <summary>
      /// Edits a glossary term.
      /// </summary>
      /// <param name="viewModel">The view model with data.</param>
      /// <returns></returns>
      [HttpPost]
      [ValidateAntiForgeryToken]
      [Authorize (Roles = "Editor")]
      public ActionResult Edit (GlossaryTermViewModel viewModel)
      {
         if (ModelState.IsValid)
         {
            var term = this.m_Db.GlossaryTerms.Get (viewModel.Id);
            this.m_Db.SetValues (term, viewModel);
            this.m_Db.Save ();
            return RedirectToAction ("Index", new { id = viewModel.Id });
         }
         return View (viewModel);
      }

      /// <summary>
      /// Adds the given tag to the given term.
      /// </summary>
      /// <param name="tagId">Id of tag to add.</param>
      /// <param name="id">Id of term to add tag to.</param>
      /// <returns></returns>
      [HttpPost]
      [ValidateAntiForgeryToken]
      [Authorize (Roles = "Editor")]
      public ActionResult AddTag (int tagId, int id)
      {
         var term = this.m_Db.GlossaryTerms.Get (id);
         var foundTag = term.Tags.FirstOrDefault (bt => bt.Tag.Id == tagId);
         if (foundTag != null)
         {
            Response.StatusCode = 500;
            return Json ("That tag is already assigned to the term.");
         }
         var tag = this.m_Db.Tags.Get (tagId);
         term.Tags.Add (new GlossaryTermTag { Term = term, Tag = tag });
         this.m_Db.Save ();
         return Json ("success");
      }

      /// <summary>
      /// Removes the tag with the given id from the given term.
      /// </summary>
      /// <param name="tagId">Id of tag to remove.</param>
      /// <param name="id">Id of term to remove tag from.</param>
      /// <returns></returns>
      [HttpPost]
      [ValidateAntiForgeryToken]
      [Authorize (Roles = "Editor")]
      public ActionResult RemoveTag (int tagId, int id)
      {
         var term = this.m_Db.GlossaryTerms.Get (id);
         if (term == null)
         {
            Response.StatusCode = 500;
            return Json ("Unable to get term - " + id);
         }
         var termTag = term.Tags.FirstOrDefault (bt => bt.Id == tagId);
         if (termTag == null)
         {
            Response.StatusCode = 500;
            return Json ("Unable to find the given tag.");
         }
         term.Tags.Remove (termTag);
         this.m_Db.Save ();
         return Json ("success");
      }

      /// <summary>
      /// Gets auto complete items for the given term.
      /// </summary>
      /// <param name="term">Term to get auto complete items for.</param>
      /// <returns>The list of possible terms for the given item.</returns>
      public ActionResult AutoComplete (string term)
      {
         var result = new
         {
            suggestions = this.m_Db.GlossaryTerms.Get (t => t.Name.Contains (term)).Select (t => new { value = t.Name, data = t.Id })
         };
         return Json (result, JsonRequestBehavior.AllowGet);
      }
   }
}
