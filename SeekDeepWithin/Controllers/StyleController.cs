using System.Linq;
using System.Web;
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
      /// Shows the edit page for styles.
      /// </summary>
      /// <param name="id">Id of item to edit.</param>
      /// <returns>The edit page.</returns>
      [Authorize (Roles = "Editor")]
      public ActionResult EditPassage (int id)
      {
         var viewModel = new StyleEditViewModel { ItemId = id, ItemType = "Passage", NextEntryId = -1, PreviousEntryId = -1};
         var passage = this.m_Db.PassageEntries.Get (id);
         viewModel.ItemText = passage.Passage.Text;
         viewModel.ParentId = passage.Chapter.Id;
         var prev = passage.Chapter.Passages.FirstOrDefault (p => p.Order == (passage.Order - 1));
         if (prev != null) viewModel.PreviousEntryId = prev.Id;
         var next = passage.Chapter.Passages.FirstOrDefault (p => p.Order == (passage.Order + 1));
         if (next != null) viewModel.NextEntryId = next.Id;
         var renderable = new PassageViewModel (passage);
         renderable.Links.Clear();
         renderable.Footers.Clear();
         viewModel.RenderedText = new SdwRenderer ().Render (renderable);
         foreach (var style in passage.Styles)
         {
            viewModel.Styles.Add(new StyleViewModel
            {
               ItemId = style.Id,
               StyleId = style.Style.Id,
               Start = style.Style.Start,
               End = style.Style.End,
               StartIndex = style.StartIndex,
               EndIndex = style.EndIndex,
               SpansMultiple = style.Style.SpansMultiple
            });
         }
         return View ("Edit", viewModel);
      }

      /// <summary>
      /// Shows the edit page for styles.
      /// </summary>
      /// <param name="id">Id of item to edit.</param>
      /// <returns>The edit page.</returns>
      [Authorize (Roles = "Editor")]
      public ActionResult EditEntry (int id)
      {
         var viewModel = new StyleEditViewModel { ItemId = id, ItemType = "Entry", NextEntryId = -1, PreviousEntryId = -1 };
         var entry = this.m_Db.GlossaryEntries.Get (id);
         viewModel.ItemText = entry.Text;
         viewModel.ParentId = entry.Item.Id;
         var prev = entry.Item.Entries.FirstOrDefault (e => e.Order == (entry.Order - 1));
         if (prev != null) viewModel.PreviousEntryId = prev.Id;
         var next = entry.Item.Entries.FirstOrDefault (e => e.Order == (entry.Order + 1));
         if (next != null) viewModel.NextEntryId = next.Id;
         var renderable = new GlossaryEntryViewModel(entry, null);
         renderable.Links.Clear ();
         renderable.Footers.Clear ();
         viewModel.RenderedText = new SdwRenderer ().Render (renderable);
         foreach (var style in entry.Styles)
         {
            viewModel.Styles.Add (new StyleViewModel
            {
               ItemId = style.Id,
               StyleId = style.Style.Id,
               Start = style.Style.Start,
               End = style.Style.End,
               StartIndex = style.StartIndex,
               EndIndex = style.EndIndex,
               SpansMultiple = style.Style.SpansMultiple
            });
         }
         return View ("Edit", viewModel);
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
         {
            Response.StatusCode = 500;
            return Json ("Invalid Data.");
         }

         var style = GetStyle (viewModel);
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
         {
            Response.StatusCode = 500;
            return Json ("Invalid Data.");
         }

         var style = GetStyle (viewModel);
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

      /// <summary>
      /// Updates a style for the given item.
      /// </summary>
      /// <param name="viewModel">Editing view model.</param>
      /// <returns>Results.</returns>
      [HttpPost]
      [ValidateAntiForgeryToken]
      [Authorize (Roles = "Editor")]
      public ActionResult UpdatePassage (EditStyleViewModel viewModel)
      {
         if (!ModelState.IsValid)
         {
            Response.StatusCode = 500;
            return Json ("Invalid Data.");
         }

         var style = GetStyle (viewModel);
         var passage = this.m_Db.PassageEntries.Get (viewModel.ParentId);
         var pStyle = passage.Styles.FirstOrDefault (s => s.Id == viewModel.Id);
         if (pStyle == null)
         {
            Response.StatusCode = 500;
            return Json ("Invalid Data.");
         }

         pStyle.EndIndex = viewModel.EndIndex;
         pStyle.StartIndex = viewModel.StartIndex;
         if (pStyle.Style.Id != style.Id)
            pStyle.Style = style;
         this.m_Db.Save ();
         return Json ("success");
      }

      /// <summary>
      /// Updates a style for the given item.
      /// </summary>
      /// <param name="viewModel">Editing view model.</param>
      /// <returns>Results.</returns>
      [HttpPost]
      [ValidateAntiForgeryToken]
      [Authorize (Roles = "Editor")]
      public ActionResult UpdateEntry (EditStyleViewModel viewModel)
      {
         if (!ModelState.IsValid)
         {
            Response.StatusCode = 500;
            return Json ("Invalid Data.");
         }

         var style = GetStyle (viewModel);
         var entry = this.m_Db.GlossaryEntries.Get (viewModel.ParentId);
         var eStyle = entry.Styles.FirstOrDefault (s => s.Id == viewModel.Id);
         if (eStyle == null)
         {
            Response.StatusCode = 500;
            return Json ("Invalid Data.");
         }

         eStyle.EndIndex = viewModel.EndIndex;
         eStyle.StartIndex = viewModel.StartIndex;
         if (eStyle.Style.Id != style.Id)
            eStyle.Style = style;
         this.m_Db.Save ();
         return Json ("success");
      }

      /// <summary>
      /// Deletes a style for the given item.
      /// </summary>
      /// <param name="id">Id of style to get.</param>
      /// <param name="itemId">Id of parent entry.</param>
      /// <returns>Results.</returns>
      [HttpPost]
      [ValidateAntiForgeryToken]
      [Authorize (Roles = "Editor")]
      public ActionResult DeletePassage (int id, int itemId)
      {
         var passage = this.m_Db.PassageEntries.Get (itemId);
         var pStyle = passage.Styles.FirstOrDefault (s => s.Id == id);
         if (pStyle == null)
            return Json ("No style to delete.");
         passage.Styles.Remove (pStyle);
         this.m_Db.Save ();
         return Json ("success");
      }

      /// <summary>
      /// Deletes a style for the given item.
      /// </summary>
      /// <param name="id">Id of style to get.</param>
      /// <param name="itemId">Id of parent entry.</param>
      /// <returns>Results.</returns>
      [HttpPost]
      [ValidateAntiForgeryToken]
      [Authorize (Roles = "Editor")]
      public ActionResult DeleteEntry (int id, int itemId)
      {
         var entry = this.m_Db.GlossaryEntries.Get (itemId);
         var eStyle = entry.Styles.FirstOrDefault (s => s.Id == id);
         if (eStyle == null)
            return Json ("No style to delete.");
         entry.Styles.Remove (eStyle);
         this.m_Db.Save ();
         return Json ("success");
      }

      /// <summary>
      /// Gets a style for the given item.
      /// </summary>
      /// <param name="id">Id of style to get.</param>
      /// <param name="itemId">Id of parent entry.</param>
      /// <param name="itemType">Type of parent entry.</param>
      /// <returns>Results.</returns>
      public ActionResult Get (int id, int itemId, string itemType)
      {
         if (itemType.ToLower() == "passage")
         {
            var passage = this.m_Db.PassageEntries.Get (itemId);
            var pStyle = passage.Styles.FirstOrDefault (s => s.Id == id);
            if (pStyle != null)
               return Json (new
               {
                  id = pStyle.Id,
                  startIndex = pStyle.StartIndex,
                  endIndex = pStyle.EndIndex,
                  start = pStyle.Style.Start,
                  end = pStyle.Style.End
               }, JsonRequestBehavior.AllowGet);
         }
         else if (itemType.ToLower () == "entry")
         {
            var entry = this.m_Db.GlossaryEntries.Get (itemId);
            var eStyle = entry.Styles.FirstOrDefault (s => s.Id == id);
            if (eStyle != null)
               return Json (new
               {
                  id = eStyle.Id,
                  startIndex = eStyle.StartIndex,
                  endIndex = eStyle.EndIndex,
                  start = eStyle.Style.Start,
                  end = eStyle.Style.End
               }, JsonRequestBehavior.AllowGet);
         }

         Response.StatusCode = 500;
         return Json ("Invalid Data.", JsonRequestBehavior.AllowGet);
      }

      /// <summary>
      /// Renders the style for the given item.
      /// </summary>
      /// <param name="itemId">Id of parent entry.</param>
      /// <param name="itemType">Type of parent entry.</param>
      /// <returns>Results.</returns>
      public ActionResult Render (int itemId, string itemType)
      {
         IRenderable renderable = null;
         if (itemType.ToLower () == "passage")
         {
            var passage = this.m_Db.PassageEntries.Get (itemId);
            renderable = new PassageViewModel (passage);
         }
         else if (itemType.ToLower () == "entry")
         {
            var entry = this.m_Db.GlossaryEntries.Get (itemId);
            renderable = new GlossaryEntryViewModel (entry, null);
         }

         if (renderable == null)
         {
            Response.StatusCode = 500;
            return Json ("Invalid Data.", JsonRequestBehavior.AllowGet);
         }

         renderable.Links.Clear ();
         renderable.Footers.Clear ();
         var html = new SdwRenderer ().Render (renderable);
         return Json (new { html }, JsonRequestBehavior.AllowGet);
      }

      /// <summary>
      /// Gets the style, or creates if not found.
      /// </summary>
      /// <param name="viewModel">View model with style information.</param>
      /// <returns>The requested style.</returns>
      private Style GetStyle (EditStyleViewModel viewModel)
      {
         var start = HttpUtility.UrlDecode (viewModel.StartStyle) ?? "<span>";
         var end = HttpUtility.UrlDecode (viewModel.EndStyle) ?? "</span>";
         var styles = this.m_Db.Styles.Get (s => s.Start == start  && s.End == end && s.SpansMultiple == viewModel.SpansMultiple);
         var style = styles.FirstOrDefault ();
         if (style == null)
         {
            style = new Style {Start = start, End = end, SpansMultiple = viewModel.SpansMultiple};
            this.m_Db.Styles.Insert (style);
            this.m_Db.Save ();
         }
         return style;
      }
   }
}
