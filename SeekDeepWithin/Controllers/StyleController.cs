using System.Linq;
using System.Web.Mvc;
using SeekDeepWithin.DataAccess;
using SeekDeepWithin.Domain;
using SeekDeepWithin.Models;

namespace SeekDeepWithin.Controllers
{
   public class StyleController : Controller
   {
      private readonly ISdwDatabase m_Db;

      /// <summary>
      /// Initializes a new controller.
      /// </summary>
      public StyleController ()
      {
         this.m_Db = new SdwDatabase ();
      }

      /// <summary>
      /// Initializes a new controller with the given db info.
      /// </summary>
      /// <param name="db">Database object.</param>
      public StyleController (ISdwDatabase db)
      {
         this.m_Db = db;
      }

      /// <summary>
      /// Gets the add style view.
      /// </summary>
      /// <param name="id">Id of the item to add a style for.</param>
      /// <param name="type">The type of object we are adding style for.</param>
      /// <param name="parentId">The id of the parent object.</param>
      /// <returns>The add style view.</returns>
      [Authorize (Roles = "Editor")]
      public ActionResult Create (int id, string type, int parentId)
      {
         if (Request.UrlReferrer != null) TempData["RefUrl"] = Request.UrlReferrer.ToString ();
         var passage = this.m_Db.Passages.Get (id);
         return View (new EditStyleViewModel { Id = id, Text = passage.Text, ParentId = parentId });
      }

      /// <summary>
      /// Posts a new style to the given item.
      /// </summary>
      /// <param name="viewModel">Editing view model.</param>
      /// <returns>Results.</returns>
      [HttpPost]
      [ValidateAntiForgeryToken]
      [Authorize (Roles = "Editor")]
      public ActionResult Create (EditStyleViewModel viewModel)
      {
         if (!ModelState.IsValid)
            return View (viewModel);

         var styles = this.m_Db.Styles.Get (s => s.Start == viewModel.StartStyle && s.End == viewModel.EndStyle);
         var style = styles.FirstOrDefault ();
         if (style == null)
         {
            style = new Style { Start = viewModel.StartStyle, End = viewModel.EndStyle };
            this.m_Db.Styles.Insert (style);
            this.m_Db.Save ();
         }

         var passage = this.m_Db.PassageEntries.Get (viewModel.ParentId);
         passage.Styles.Add (new PassageStyle
         {
            PassageEntry = passage,
            Style = style,
            StartIndex = viewModel.StartIndex,
            EndIndex = viewModel.EndIndex
         });
         this.m_Db.Save ();
         return RedirectToAction ("Details", "Passage", new { id = viewModel.Id });
      }
   }
}
