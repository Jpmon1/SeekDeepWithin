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
         var title = passage.Chapter.SubBook.Version.Title + " | ";
         if (!passage.Chapter.SubBook.Hide)
            title += passage.Chapter.SubBook.SubBook.Name + " | ";
         if (!passage.Chapter.Hide)
            title += passage.Chapter.Chapter.Name + ":";
         title += passage.Number;
         viewModel.Title = title;
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
            viewModel.Styles.Add(new StyleViewModel (style));
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
         viewModel.Title = entry.Item.Term.Name;
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
            viewModel.Styles.Add (new StyleViewModel (style));
         return View ("Edit", viewModel);
      }

      /// <summary>
      /// Gets the style edit page for a header.
      /// </summary>
      /// <param name="id">Id of header.</param>
      /// <param name="itemId">Parent item id.</param>
      /// <param name="itemType">Type of parent item.</param>
      /// <returns>The edit view.</returns>
      public ActionResult EditHeader (int id, int itemId, string itemType)
      {
         var viewModel = new StyleEditViewModel
         {
            ItemId = id,
            ParentId = itemId,
            ItemType = itemType + "Header",
            NextEntryId = -1,
            PreviousEntryId = -1,
            Title = "Header"
         };
         IHeader header = null;
         if (itemType.ToLower () == "chapter")
         {
            var chapter = this.m_Db.SubBookChapters.Get (itemId);
            viewModel.Title = chapter.Chapter.Name;
            header = chapter.Headers.FirstOrDefault (f => f.Id == id);
         }
         if (itemType.ToLower () == "passage")
         {
            var passage = this.m_Db.PassageEntries.Get (itemId);
            header = passage.Headers.FirstOrDefault (f => f.Id == id);
         }
         if (itemType.ToLower () == "entry")
         {
            var entry = this.m_Db.GlossaryEntries.Get (itemId);
            viewModel.Title = entry.Item.Term.Name;
            header = entry.Headers.FirstOrDefault (f => f.Id == id);
         }
         if (header == null)
         {
            Response.StatusCode = 500;
            return Json ("Invalid Data.", JsonRequestBehavior.AllowGet);
         }
         viewModel.ItemText = header.Text;
         foreach (var style in header.StyleList)
            viewModel.Styles.Add(new StyleViewModel(style));
         var renderable = new HeaderFooterViewModel (header);
         viewModel.RenderedText = new SdwRenderer ().Render (renderable);
         return View ("Edit", viewModel);
      }

      /// <summary>
      /// Gets the style edit page for a footer.
      /// </summary>
      /// <param name="id">Id of footer.</param>
      /// <param name="itemId">Parent item id.</param>
      /// <param name="itemType">Type of parent item.</param>
      /// <returns>The edit view.</returns>
      public ActionResult EditFooter (int id, int itemId, string itemType)
      {
         var viewModel = new StyleEditViewModel
         {
            ItemId = id,
            ParentId = itemId,
            ItemType = itemType + "Footer",
            NextEntryId = -1,
            PreviousEntryId = -1,
            Title = "Footer"
         };
         IFooter footer = null;
         if (itemType.ToLower () == "chapter")
         {
            var chapter = this.m_Db.SubBookChapters.Get (itemId);
            viewModel.Title = chapter.Chapter.Name;
            footer = chapter.Footers.FirstOrDefault (f => f.Id == id);
         }
         if (itemType.ToLower () == "passage")
         {
            var passage = this.m_Db.PassageEntries.Get (itemId);
            footer = passage.Footers.FirstOrDefault (f => f.Id == id);
         }
         if (itemType.ToLower () == "entry")
         {
            var entry = this.m_Db.GlossaryEntries.Get (itemId);
            viewModel.Title = entry.Item.Term.Name;
            footer = entry.Footers.FirstOrDefault (f => f.Id == id);
         }
         if (footer == null)
         {
            Response.StatusCode = 500;
            return Json ("Invalid Data.", JsonRequestBehavior.AllowGet);
         }
         viewModel.ItemText = footer.Text;
         foreach (var style in footer.StyleList)
            viewModel.Styles.Add (new StyleViewModel (style));
         var renderable = new HeaderFooterViewModel (footer);
         viewModel.RenderedText = new SdwRenderer ().Render (renderable);
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
      public ActionResult CreatePassageHeader (EditStyleViewModel viewModel)
      {
         if (!ModelState.IsValid)
         {
            Response.StatusCode = 500;
            return Json ("Invalid Data.");
         }

         var style = GetStyle (viewModel);
         var passage = this.m_Db.PassageEntries.Get (viewModel.ParentId);
         var header = passage.Headers.FirstOrDefault (f => f.Id == viewModel.Id);
         if (header == null)
         {
            Response.StatusCode = 500;
            return Json ("Unable to get header.");
         }
         var fStyle = new PassageHeaderStyle
         {
            Header = header,
            Style = style,
            StartIndex = viewModel.StartIndex,
            EndIndex = viewModel.EndIndex
         };
         header.Styles.Add (fStyle);
         this.m_Db.Save ();
         return Json (new { id = fStyle.Id, startIndex = fStyle.StartIndex, endIndex = fStyle.EndIndex });
      }

      /// <summary>
      /// Posts a new style to the given item.
      /// </summary>
      /// <param name="viewModel">Editing view model.</param>
      /// <returns>Results.</returns>
      [HttpPost]
      [ValidateAntiForgeryToken]
      [Authorize (Roles = "Editor")]
      public ActionResult CreatePassageFooter (EditStyleViewModel viewModel)
      {
         if (!ModelState.IsValid)
         {
            Response.StatusCode = 500;
            return Json ("Invalid Data.");
         }

         var style = GetStyle (viewModel);
         var passage = this.m_Db.PassageEntries.Get (viewModel.ParentId);
         var footer = passage.Footers.FirstOrDefault (f => f.Id == viewModel.Id);
         if (footer == null)
         {
            Response.StatusCode = 500;
            return Json ("Unable to get footer.");
         }
         var fStyle = new PassageFooterStyle
         {
            Footer = footer,
            Style = style,
            StartIndex = viewModel.StartIndex,
            EndIndex = viewModel.EndIndex
         };
         footer.Styles.Add (fStyle);
         this.m_Db.Save ();
         return Json (new { id = fStyle.Id, startIndex = fStyle.StartIndex, endIndex = fStyle.EndIndex });
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
      /// Posts a new style to the given item.
      /// </summary>
      /// <param name="viewModel">Editing view model.</param>
      /// <returns>Results.</returns>
      [HttpPost]
      [ValidateAntiForgeryToken]
      [Authorize (Roles = "Editor")]
      public ActionResult CreateEntryHeader (EditStyleViewModel viewModel)
      {
         if (!ModelState.IsValid)
         {
            Response.StatusCode = 500;
            return Json ("Invalid Data.");
         }

         var style = GetStyle (viewModel);
         var entry = this.m_Db.GlossaryEntries.Get (viewModel.ParentId);
         var header = entry.Headers.FirstOrDefault (f => f.Id == viewModel.Id);
         if (header == null)
         {
            Response.StatusCode = 500;
            return Json ("Unable to get header.");
         }
         var hStyle = new GlossaryHeaderStyle
         {
            Header = header,
            Style = style,
            StartIndex = viewModel.StartIndex,
            EndIndex = viewModel.EndIndex
         };
         header.Styles.Add (hStyle);
         this.m_Db.Save ();
         return Json (new { id = hStyle.Id, startIndex = hStyle.StartIndex, endIndex = hStyle.EndIndex });
      }

      /// <summary>
      /// Posts a new style to the given item.
      /// </summary>
      /// <param name="viewModel">Editing view model.</param>
      /// <returns>Results.</returns>
      [HttpPost]
      [ValidateAntiForgeryToken]
      [Authorize (Roles = "Editor")]
      public ActionResult CreateEntryFooter (EditStyleViewModel viewModel)
      {
         if (!ModelState.IsValid)
         {
            Response.StatusCode = 500;
            return Json ("Invalid Data.");
         }

         var style = GetStyle (viewModel);
         var entry = this.m_Db.GlossaryEntries.Get (viewModel.ParentId);
         var footer = entry.Footers.FirstOrDefault (f => f.Id == viewModel.Id);
         if (footer == null)
         {
            Response.StatusCode = 500;
            return Json ("Unable to get footer.");
         }
         var fStyle = new GlossaryFooterStyle
         {
            Footer = footer,
            Style = style,
            StartIndex = viewModel.StartIndex,
            EndIndex = viewModel.EndIndex
         };
         footer.Styles.Add (fStyle);
         this.m_Db.Save ();
         return Json (new { id = fStyle.Id, startIndex = fStyle.StartIndex, endIndex = fStyle.EndIndex });
      }

      /// <summary>
      /// Posts a new style to the given item.
      /// </summary>
      /// <param name="viewModel">Editing view model.</param>
      /// <returns>Results.</returns>
      [HttpPost]
      [ValidateAntiForgeryToken]
      [Authorize (Roles = "Editor")]
      public ActionResult CreateChapterHeader (EditStyleViewModel viewModel)
      {
         if (!ModelState.IsValid)
         {
            Response.StatusCode = 500;
            return Json ("Invalid Data.");
         }

         var style = GetStyle (viewModel);
         var chapter = this.m_Db.SubBookChapters.Get (viewModel.ParentId);
         var header = chapter.Headers.FirstOrDefault (f => f.Id == viewModel.Id);
         if (header == null)
         {
            Response.StatusCode = 500;
            return Json ("Unable to get header.");
         }
         var hStyle = new ChapterHeaderStyle
         {
            Header = header,
            Style = style,
            StartIndex = viewModel.StartIndex,
            EndIndex = viewModel.EndIndex
         };
         header.Styles.Add (hStyle);
         this.m_Db.Save ();
         return Json (new { id = hStyle.Id, startIndex = hStyle.StartIndex, endIndex = hStyle.EndIndex });
      }

      /// <summary>
      /// Posts a new style to the given item.
      /// </summary>
      /// <param name="viewModel">Editing view model.</param>
      /// <returns>Results.</returns>
      [HttpPost]
      [ValidateAntiForgeryToken]
      [Authorize (Roles = "Editor")]
      public ActionResult CreateChapterFooter (EditStyleViewModel viewModel)
      {
         if (!ModelState.IsValid)
         {
            Response.StatusCode = 500;
            return Json ("Invalid Data.");
         }

         var style = GetStyle (viewModel);
         var chapter = this.m_Db.SubBookChapters.Get (viewModel.ParentId);
         var footer = chapter.Footers.FirstOrDefault (f => f.Id == viewModel.Id);
         if (footer == null)
         {
            Response.StatusCode = 500;
            return Json ("Unable to get footer.");
         }
         var fStyle = new ChapterFooterStyle
         {
            Footer = footer,
            Style = style,
            StartIndex = viewModel.StartIndex,
            EndIndex = viewModel.EndIndex
         };
         footer.Styles.Add (fStyle);
         this.m_Db.Save ();
         return Json (new { id = fStyle.Id, startIndex = fStyle.StartIndex, endIndex = fStyle.EndIndex });
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
      /// <param name="parentId">The parent id of the item.</param>
      /// <returns>Results.</returns>
      [HttpPost]
      [ValidateAntiForgeryToken]
      [Authorize (Roles = "Editor")]
      public ActionResult DeletePassageHeader (int id, int itemId, int parentId)
      {
         var passage = this.m_Db.PassageEntries.Get (parentId);
         var header = passage.Headers.FirstOrDefault (h => h.Id == itemId);
         if (header == null)
            return Json ("No header to delete from.");
         var style = header.Styles.FirstOrDefault (s => s.Id == id);
         if (style == null)
            return Json ("No style to delete.");
         header.Styles.Remove (style);
         this.m_Db.Save ();
         return Json ("success");
      }

      /// <summary>
      /// Deletes a style for the given item.
      /// </summary>
      /// <param name="id">Id of style to get.</param>
      /// <param name="itemId">Id of parent entry.</param>
      /// <param name="parentId">The parent id of the item.</param>
      /// <returns>Results.</returns>
      [HttpPost]
      [ValidateAntiForgeryToken]
      [Authorize (Roles = "Editor")]
      public ActionResult DeletePassageFooter (int id, int itemId, int parentId)
      {
         var passage = this.m_Db.PassageEntries.Get (parentId);
         var footer = passage.Footers.FirstOrDefault (h => h.Id == itemId);
         if (footer == null)
            return Json ("No footer to delete from.");
         var style = footer.Styles.FirstOrDefault (s => s.Id == id);
         if (style == null)
            return Json ("No style to delete.");
         footer.Styles.Remove (style);
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
      /// Deletes a style for the given item.
      /// </summary>
      /// <param name="id">Id of style to get.</param>
      /// <param name="itemId">Id of parent entry.</param>
      /// <param name="parentId">The parent id of the item.</param>
      /// <returns>Results.</returns>
      [HttpPost]
      [ValidateAntiForgeryToken]
      [Authorize (Roles = "Editor")]
      public ActionResult DeleteEntryHeader (int id, int itemId, int parentId)
      {
         var entry = this.m_Db.GlossaryEntries.Get (parentId);
         var header = entry.Headers.FirstOrDefault (h => h.Id == itemId);
         if (header == null)
            return Json ("No header to delete from.");
         var style = header.Styles.FirstOrDefault (s => s.Id == id);
         if (style == null)
            return Json ("No style to delete.");
         header.Styles.Remove (style);
         this.m_Db.Save ();
         return Json ("success");
      }

      /// <summary>
      /// Deletes a style for the given item.
      /// </summary>
      /// <param name="id">Id of style to get.</param>
      /// <param name="itemId">Id of parent entry.</param>
      /// <param name="parentId">The parent id of the item.</param>
      /// <returns>Results.</returns>
      [HttpPost]
      [ValidateAntiForgeryToken]
      [Authorize (Roles = "Editor")]
      public ActionResult DeleteEntryFooter (int id, int itemId, int parentId)
      {
         var entry = this.m_Db.GlossaryEntries.Get (parentId);
         var footer = entry.Footers.FirstOrDefault (h => h.Id == itemId);
         if (footer == null)
            return Json ("No footer to delete from.");
         var style = footer.Styles.FirstOrDefault (s => s.Id == id);
         if (style == null)
            return Json ("No style to delete.");
         footer.Styles.Remove (style);
         this.m_Db.Save ();
         return Json ("success");
      }

      /// <summary>
      /// Deletes a style for the given item.
      /// </summary>
      /// <param name="id">Id of style to get.</param>
      /// <param name="itemId">Id of parent entry.</param>
      /// <param name="parentId">The parent id of the item.</param>
      /// <returns>Results.</returns>
      [HttpPost]
      [ValidateAntiForgeryToken]
      [Authorize (Roles = "Editor")]
      public ActionResult DeleteChapterHeader (int id, int itemId, int parentId)
      {
         var chapter = this.m_Db.SubBookChapters.Get (parentId);
         var header = chapter.Headers.FirstOrDefault (h => h.Id == itemId);
         if (header == null)
            return Json ("No header to delete from.");
         var style = header.Styles.FirstOrDefault (s => s.Id == id);
         if (style == null)
            return Json ("No style to delete.");
         header.Styles.Remove (style);
         this.m_Db.Save ();
         return Json ("success");
      }

      /// <summary>
      /// Deletes a style for the given item.
      /// </summary>
      /// <param name="id">Id of style to get.</param>
      /// <param name="itemId">Id of parent entry.</param>
      /// <param name="parentId">The parent id of the item.</param>
      /// <returns>Results.</returns>
      [HttpPost]
      [ValidateAntiForgeryToken]
      [Authorize (Roles = "Editor")]
      public ActionResult DeleteChapterFooter (int id, int itemId, int parentId)
      {
         var chapter = this.m_Db.SubBookChapters.Get (parentId);
         var footer = chapter.Footers.FirstOrDefault (h => h.Id == itemId);
         if (footer == null)
            return Json ("No footer to delete from.");
         var style = footer.Styles.FirstOrDefault (s => s.Id == id);
         if (style == null)
            return Json ("No style to delete.");
         footer.Styles.Remove (style);
         this.m_Db.Save ();
         return Json ("success");
      }

      /// <summary>
      /// Gets a style for the given item.
      /// </summary>
      /// <param name="id">Id of style to get.</param>
      /// <param name="itemId">Id of parent entry.</param>
      /// <param name="itemType">Type of parent entry.</param>
      /// <param name="parentId">The parent id of the item.</param>
      /// <returns>Results.</returns>
      public ActionResult Get (int id, int itemId, string itemType, int? parentId)
      {
         IStyle style = null;
         if (itemType.ToLower() == "passage")
         {
            var passage = this.m_Db.PassageEntries.Get (itemId);
            style = passage.Styles.FirstOrDefault (s => s.Id == id);
         }
         else if (itemType.ToLower () == "entry")
         {
            var entry = this.m_Db.GlossaryEntries.Get (itemId);
            style = entry.Styles.FirstOrDefault (s => s.Id == id);
         }
         else if (itemType.ToLower () == "passageheader" && parentId.HasValue)
         {
            var passage = this.m_Db.PassageEntries.Get (parentId.Value);
            var header = passage.Headers.FirstOrDefault (h => h.Id == itemId);
            if (header != null)
               style = header.Styles.FirstOrDefault (s => s.Id == id);
         }
         else if (itemType.ToLower () == "passagefooter" && parentId.HasValue)
         {
            var passage = this.m_Db.PassageEntries.Get (parentId.Value);
            var footer = passage.Footers.FirstOrDefault (f => f.Id == itemId);
            if (footer != null)
               style = footer.Styles.FirstOrDefault (s => s.Id == id);
         }
         else if (itemType.ToLower () == "entryheader" && parentId.HasValue)
         {
            var entry = this.m_Db.GlossaryEntries.Get (parentId.Value);
            var header = entry.Headers.FirstOrDefault (h => h.Id == itemId);
            if (header != null)
               style = header.Styles.FirstOrDefault (s => s.Id == id);
         }
         else if (itemType.ToLower () == "entryfooter" && parentId.HasValue)
         {
            var entry = this.m_Db.GlossaryEntries.Get (parentId.Value);
            var footer = entry.Footers.FirstOrDefault (f => f.Id == itemId);
            if (footer != null)
               style = footer.Styles.FirstOrDefault (s => s.Id == id);
         }
         else if (itemType.ToLower () == "chapterheader" && parentId.HasValue)
         {
            var chapter = this.m_Db.SubBookChapters.Get (parentId.Value);
            var header = chapter.Headers.FirstOrDefault (h => h.Id == itemId);
            if (header != null)
               style = header.Styles.FirstOrDefault (s => s.Id == id);
         }
         else if (itemType.ToLower () == "chapterfooter" && parentId.HasValue)
         {
            var chapter = this.m_Db.SubBookChapters.Get (parentId.Value);
            var footer = chapter.Footers.FirstOrDefault (f => f.Id == itemId);
            if (footer != null)
               style = footer.Styles.FirstOrDefault (s => s.Id == id);
         }

         if (style != null)
         {
            return Json (new
            {
               id = style.Id,
               startIndex = style.StartIndex,
               endIndex = style.EndIndex,
               start = style.Style.Start,
               end = style.Style.End
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
      /// <param name="parentId">The parent id of the item.</param>
      /// <returns>Results.</returns>
      public ActionResult Render (int itemId, string itemType, int? parentId)
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
         else if (itemType.ToLower () == "passageheader" && parentId.HasValue)
         {
            var passage = this.m_Db.PassageEntries.Get (parentId.Value);
            var header = passage.Headers.FirstOrDefault (h => h.Id == itemId);
            renderable = new HeaderFooterViewModel (header);
         }
         else if (itemType.ToLower () == "passagefooter" && parentId.HasValue)
         {
            var passage = this.m_Db.PassageEntries.Get (parentId.Value);
            var footer = passage.Footers.FirstOrDefault (f => f.Id == itemId);
            renderable = new HeaderFooterViewModel (footer);
         }
         else if (itemType.ToLower () == "entryheader" && parentId.HasValue)
         {
            var entry = this.m_Db.GlossaryEntries.Get (parentId.Value);
            var header = entry.Headers.FirstOrDefault (h => h.Id == itemId);
            renderable = new HeaderFooterViewModel (header);
         }
         else if (itemType.ToLower () == "entryfooter" && parentId.HasValue)
         {
            var entry = this.m_Db.GlossaryEntries.Get (parentId.Value);
            var footer = entry.Footers.FirstOrDefault (f => f.Id == itemId);
            renderable = new HeaderFooterViewModel (footer);
         }
         else if (itemType.ToLower () == "chapterheader" && parentId.HasValue)
         {
            var chapter = this.m_Db.SubBookChapters.Get (parentId.Value);
            var header = chapter.Headers.FirstOrDefault (h => h.Id == itemId);
            renderable = new HeaderFooterViewModel (header);
         }
         else if (itemType.ToLower () == "chapterfooter" && parentId.HasValue)
         {
            var chapter = this.m_Db.SubBookChapters.Get (parentId.Value);
            var footer = chapter.Footers.FirstOrDefault (f => f.Id == itemId);
            renderable = new HeaderFooterViewModel (footer);
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
