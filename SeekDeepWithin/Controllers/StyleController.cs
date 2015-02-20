using System.Linq;
using System.Web.Mvc;
using SeekDeepWithin.DataAccess;
using SeekDeepWithin.Pocos;
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
      /// Posts a new style to the given item.
      /// </summary>
      /// <param name="viewModel">Editing view model.</param>
      /// <returns>Results.</returns>
      [HttpPost]
      [ValidateAntiForgeryToken]
      [Authorize (Roles = "Editor")]
      public ActionResult CreatePassage (EditStyleViewModel viewModel)
      {
         if (!ModelState.IsValid)
            return Json ("Invalid Data.");

         var styles = this.m_Db.Styles.Get (s => s.Start == viewModel.StartStyle && s.End == viewModel.EndStyle);
         var style = styles.FirstOrDefault ();
         if (style == null)
         {
            style = new Style { Start = viewModel.StartStyle, End = viewModel.EndStyle };
            this.m_Db.Styles.Insert (style);
            this.m_Db.Save ();
         }

         var passage = this.m_Db.PassageEntries.Get (viewModel.ParentId);
         var pStyle = new PassageStyle
         {
            PassageEntry = passage,
            Style = style,
            StartIndex = viewModel.StartIndex,
            EndIndex = viewModel.EndIndex
         };
         passage.Styles.Add (pStyle);
         this.m_Db.Save ();
         return Json (new { id = pStyle.Id, startIndex = pStyle.StartIndex, endIndex = pStyle.EndIndex });
      }

      /// <summary>
      /// Posts a new style to the given item.
      /// </summary>
      /// <param name="viewModel">Editing view model.</param>
      /// <returns>Results.</returns>
      [HttpPost]
      [ValidateAntiForgeryToken]
      [Authorize (Roles = "Editor")]
      public ActionResult CreateEntry (EditStyleViewModel viewModel)
      {
         if (!ModelState.IsValid)
            return Json ("Invalid Data.");

         var styles = this.m_Db.Styles.Get (s => s.Start == viewModel.StartStyle && s.End == viewModel.EndStyle);
         var style = styles.FirstOrDefault ();
         if (style == null)
         {
            style = new Style { Start = viewModel.StartStyle, End = viewModel.EndStyle };
            this.m_Db.Styles.Insert (style);
            this.m_Db.Save ();
         }

         var entry = this.m_Db.GlossaryEntries.Get (viewModel.ParentId);
         var pStyle = new GlossaryEntryStyle
         {
            Entry = entry,
            Style = style,
            StartIndex = viewModel.StartIndex,
            EndIndex = viewModel.EndIndex
         };
         entry.Styles.Add (pStyle);
         this.m_Db.Save ();
         return Json (new { id = pStyle.Id, startIndex = pStyle.StartIndex, endIndex = pStyle.EndIndex });
      }
   }
}
