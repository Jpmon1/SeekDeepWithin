using System.Linq;
using System.Web;
using System.Web.Mvc;
using SeekDeepWithin.DataAccess;
using SeekDeepWithin.Pocos;

namespace SeekDeepWithin.Controllers
{
   public class StyleController : SdwController
   {
      /// <summary>
      /// Initializes a new controller.
      /// </summary>
      public StyleController () : base (new SdwDatabase ()) { }

      /// <summary>
      /// Initializes a new controller with the given db info.
      /// </summary>
      /// <param name="db">Database object.</param>
      public StyleController (ISdwDatabase db) : base (db) { }

      #region Passage

      /// <summary>
      /// Gets information about the given style.
      /// </summary>
      /// <param name="id">Id of style to get.</param>
      /// <param name="itemId">Item style belongs to.</param>
      /// <returns>Json Results.</returns>
      public ActionResult GetPassage (int id, int itemId)
      {
         var passage = this.Database.PassageEntries.Get (itemId);
         if (passage == null) return this.Fail ("Unable to determine the passage");
         var style = passage.Styles.FirstOrDefault (s => s.Id == id);
         if (style == null) return this.Fail ("Unable to determine the style to get");
         return Json (new
         {
            itemId,
            id = style.Id,
            startIndex = style.StartIndex,
            endIndex = style.EndIndex,
            start = style.Style.Start,
            end = style.Style.End
         }, JsonRequestBehavior.AllowGet);
      }

      /// <summary>
      /// Posts a new style to the given item.
      /// </summary>
      /// <returns>Results.</returns>
      [HttpPost]
      [ValidateAntiForgeryToken]
      [Authorize (Roles = "Editor")]
      public ActionResult CreatePassage (int itemId, string startStyle, string endStyle, int startIndex, int endIndex, bool spansMultiple)
      {
         var style = GetStyle (startStyle, endStyle, spansMultiple);
         var passage = this.Database.PassageEntries.Get (itemId);
         if (passage == null) return this.Fail ("Unable to determine the passage");
         var pStyle = new PassageStyle
         {
            PassageEntry = passage,
            Style = style,
            StartIndex = startIndex,
            EndIndex = endIndex
         };
         passage.Styles.Add (pStyle);
         this.Database.Save ();
         return Json (new { id = pStyle.Id, startIndex = pStyle.StartIndex, endIndex = pStyle.EndIndex });
      }

      /// <summary>
      /// Updates a style for the given item.
      /// </summary>
      /// <returns>Results.</returns>
      [HttpPost]
      [ValidateAntiForgeryToken]
      [Authorize (Roles = "Editor")]
      public ActionResult UpdatePassage (int id, int itemId, string startStyle, string endStyle, int startIndex, int endIndex, bool spansMultiple)
      {
         var style = GetStyle (startStyle, endStyle, spansMultiple);
         var passage = this.Database.PassageEntries.Get (itemId);
         if (passage == null) return this.Fail ("Unable to determine the passage");
         var pStyle = passage.Styles.FirstOrDefault (s => s.Id == id);
         if (pStyle == null) return this.Fail ("Unable to determine the style to edit");
         if (pStyle.Style.Id != style.Id) pStyle.Style = style;
         pStyle.EndIndex = endIndex;
         pStyle.StartIndex = startIndex;
         this.Database.Save ();
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
         var passage = this.Database.PassageEntries.Get (itemId);
         if (passage == null) return this.Fail ("Unable to determine the passage");
         var pStyle = passage.Styles.FirstOrDefault (s => s.Id == id);
         if (pStyle == null) return this.Fail ("No style to delete");
         passage.Styles.Remove (pStyle);
         this.Database.Save ();
         return Json ("success");
      }

      /// <summary>
      /// Gets information about the given style.
      /// </summary>
      /// <param name="id">Id of style to get.</param>
      /// <param name="itemId">Item style belongs to.</param>
      /// <returns>Json Results.</returns>
      public ActionResult GetPassageHeader (int id, int itemId)
      {
         var passage = this.Database.PassageEntries.Get (itemId);
         if (passage == null) return this.Fail ("Unable to determine the passage");
         if (passage.Header == null) return this.Fail ("No information to get style for");
         var style = passage.Header.Styles.FirstOrDefault (s => s.Id == id);
         if (style == null) return this.Fail ("Unable to determine the style to get");
         return Json (new
         {
            itemId,
            id = style.Id,
            startIndex = style.StartIndex,
            endIndex = style.EndIndex,
            start = style.Style.Start,
            end = style.Style.End
         }, JsonRequestBehavior.AllowGet);
      }

      /// <summary>
      /// Posts a new style to the given item.
      /// </summary>
      /// <returns>Results.</returns>
      [HttpPost]
      [ValidateAntiForgeryToken]
      [Authorize (Roles = "Editor")]
      public ActionResult CreatePassageHeader (int itemId, string startStyle, string endStyle, int startIndex, int endIndex)
      {
         var style = GetStyle (startStyle, endStyle, false);
         var passage = this.Database.PassageEntries.Get (itemId);
         if (passage == null) return this.Fail ("Unable to determine the passage");
         if (passage.Header == null) return this.Fail ("No header to apply style to");
         var fStyle = new PassageHeaderStyle
         {
            Header = passage.Header,
            Style = style,
            StartIndex = startIndex,
            EndIndex = endIndex
         };
         passage.Header.Styles.Add (fStyle);
         this.Database.Save ();
         return Json (new { id = fStyle.Id, startIndex = fStyle.StartIndex, endIndex = fStyle.EndIndex });
      }

      /// <summary>
      /// Posts a new style to the given item.
      /// </summary>
      /// <returns>Results.</returns>
      [HttpPost]
      [ValidateAntiForgeryToken]
      [Authorize (Roles = "Editor")]
      public ActionResult UpdatePassageHeader (int id, int itemId, string startStyle, string endStyle, int startIndex, int endIndex)
      {
         var style = GetStyle (startStyle, endStyle, false);
         var passage = this.Database.PassageEntries.Get (itemId);
         if (passage == null) return this.Fail ("Unable to determine the passage");
         if (passage.Header == null) return this.Fail ("No header to apply style to");
         var hStyle = passage.Header.Styles.FirstOrDefault (s => s.Id == id);
         if (hStyle == null) return this.Fail ("Unable to determine the style to update");
         if (hStyle.Style.Id != style.Id) hStyle.Style = style;
         hStyle.EndIndex = endIndex;
         hStyle.StartIndex = startIndex;
         this.Database.Save ();
         return Json (new { id, startIndex, endIndex });
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
      public ActionResult DeletePassageHeader (int id, int itemId)
      {
         var passage = this.Database.PassageEntries.Get (itemId);
         if (passage == null) return this.Fail ("Unable to determine the passage");
         if (passage.Header == null) return this.Fail ("No header to delete from.");
         var style = passage.Header.Styles.FirstOrDefault (s => s.Id == id);
         if (style == null) return this.Fail ("No style to delete.");
         passage.Header.Styles.Remove (style);
         this.Database.Save ();
         return Json ("success");
      }

      /// <summary>
      /// Gets information about the given style.
      /// </summary>
      /// <param name="id">Id of style to get.</param>
      /// <param name="itemId">Item style belongs to.</param>
      /// <param name="footerId">Id of footer.</param>
      /// <returns>Json Results.</returns>
      public ActionResult GetPassageFooter (int id, int itemId, int footerId)
      {
         var passage = this.Database.PassageEntries.Get (itemId);
         if (passage == null) return this.Fail ("Unable to determine the passage");
         var footer = passage.Footers.FirstOrDefault (f => f.Id == footerId);
         if (footer == null) return this.Fail ("Unable to determine the footer");
         var style = footer.Styles.FirstOrDefault (s => s.Id == id);
         if (style == null) return this.Fail ("Unable to determine the style to get");
         return Json (new
         {
            itemId,
            id = style.Id,
            startIndex = style.StartIndex,
            endIndex = style.EndIndex,
            start = style.Style.Start,
            end = style.Style.End
         }, JsonRequestBehavior.AllowGet);
      }

      /// <summary>
      /// Posts a new style to the given item.
      /// </summary>
      /// <returns>Results.</returns>
      [HttpPost]
      [ValidateAntiForgeryToken]
      [Authorize (Roles = "Editor")]
      public ActionResult CreatePassageFooter (int itemId, int footerId, string startStyle, string endStyle, int startIndex, int endIndex)
      {
         var style = GetStyle (startStyle, endStyle, false);
         var passage = this.Database.PassageEntries.Get (itemId);
         if (passage == null) return this.Fail ("Unable to determine the passage");
         var footer = passage.Footers.FirstOrDefault (f => f.Id == footerId);
         if (footer == null) return this.Fail ("Unable to determine the footer");
         var fStyle = new PassageFooterStyle
         {
            Footer = footer,
            Style = style,
            StartIndex = startIndex,
            EndIndex = endIndex
         };
         footer.Styles.Add (fStyle);
         this.Database.Save ();
         return Json (new { id = fStyle.Id, startIndex = fStyle.StartIndex, endIndex = fStyle.EndIndex });
      }

      /// <summary>
      /// Posts a new style to the given item.
      /// </summary>
      /// <returns>Results.</returns>
      [HttpPost]
      [ValidateAntiForgeryToken]
      [Authorize (Roles = "Editor")]
      public ActionResult UpdatePassageFooter (int id, int itemId, int footerId, string startStyle, string endStyle, int startIndex, int endIndex)
      {
         var style = GetStyle (startStyle, endStyle, false);
         var passage = this.Database.PassageEntries.Get (itemId);
         if (passage == null) return this.Fail ("Unable to determine the passage");
         var footer = passage.Footers.FirstOrDefault (f => f.Id == footerId);
         if (footer == null) return this.Fail ("Unable to determine the footer");
         var fStyle = footer.Styles.FirstOrDefault (s => s.Id == id);
         if (fStyle == null) return this.Fail ("Unable to determine the style to update");
         if (fStyle.Style.Id != style.Id) fStyle.Style = style;
         fStyle.EndIndex = endIndex;
         fStyle.StartIndex = startIndex;
         this.Database.Save ();
         return Json (new { id, startIndex, endIndex });
      }

      /// <summary>
      /// Deletes a style for the given item.
      /// </summary>
      /// <param name="id">Id of style to get.</param>
      /// <param name="itemId">Id of parent entry.</param>
      /// <param name="footerId">The footer id of the item.</param>
      /// <returns>Results.</returns>
      [HttpPost]
      [ValidateAntiForgeryToken]
      [Authorize (Roles = "Editor")]
      public ActionResult DeletePassageFooter (int id, int itemId, int footerId)
      {
         var passage = this.Database.PassageEntries.Get (itemId);
         if (passage == null) return this.Fail ("Unable to determine the passage");
         var footer = passage.Footers.FirstOrDefault (f => f.Id == footerId);
         if (footer == null) return this.Fail ("No footer to delete from.");
         var style = footer.Styles.FirstOrDefault (s => s.Id == id);
         if (style == null) return this.Fail ("Unable to determine the style");
         footer.Styles.Remove (style);
         this.Database.Save ();
         return Json ("success");
      }

      #endregion

      #region Item Entry

      /// <summary>
      /// Gets information about the given style.
      /// </summary>
      /// <param name="id">Id of style to get.</param>
      /// <param name="itemId">Item style belongs to.</param>
      /// <returns>Json Results.</returns>
      public ActionResult GetItemEntry (int id, int itemId)
      {
         var entry = this.Database.TermItemEntries.Get (itemId);
         if (entry == null) return this.Fail ("Unable to determine the entry");
         var style = entry.Styles.FirstOrDefault (s => s.Id == id);
         if (style == null) return this.Fail ("Unable to determine the style to get");
         return Json (new
         {
            itemId,
            id = style.Id,
            startIndex = style.StartIndex,
            endIndex = style.EndIndex,
            start = style.Style.Start,
            end = style.Style.End
         }, JsonRequestBehavior.AllowGet);
      }

      /// <summary>
      /// Posts a new style to the given item.
      /// </summary>
      /// <returns>Results.</returns>
      [HttpPost]
      [ValidateAntiForgeryToken]
      [Authorize (Roles = "Editor")]
      public ActionResult CreateItemEntry (int itemId, string startStyle, string endStyle, int startIndex, int endIndex, bool spansMultiple)
      {
         var entry = this.Database.TermItemEntries.Get (itemId);
         if (entry == null) return this.Fail ("Unable to determine the entry");
         var style = GetStyle (startStyle, endStyle, spansMultiple);
         var pStyle = new TermItemEntryStyle
         {
            Entry = entry,
            Style = style,
            StartIndex = startIndex,
            EndIndex = endIndex
         };
         entry.Styles.Add (pStyle);
         this.Database.Save ();
         return Json (new { id = pStyle.Id, startIndex = pStyle.StartIndex, endIndex = pStyle.EndIndex });
      }

      /// <summary>
      /// Updates a style for the given item.
      /// </summary>
      /// <returns>Results.</returns>
      [HttpPost]
      [ValidateAntiForgeryToken]
      [Authorize (Roles = "Editor")]
      public ActionResult UpdateItemEntry (int id, int itemId, string startStyle, string endStyle, int startIndex, int endIndex, bool spansMultiple)
      {
         var entry = this.Database.TermItemEntries.Get (itemId);
         if (entry == null) return this.Fail ("Unable to determine the entry");
         var style = GetStyle (startStyle, endStyle, spansMultiple);
         var eStyle = entry.Styles.FirstOrDefault (s => s.Id == id);
         if (eStyle == null) return this.Fail ("Unable to determine the style to update");
         eStyle.EndIndex = endIndex;
         eStyle.StartIndex = startIndex;
         if (eStyle.Style.Id != style.Id)
            eStyle.Style = style;
         this.Database.Save ();
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
      public ActionResult DeleteItemEntry (int id, int itemId)
      {
         var entry = this.Database.TermItemEntries.Get (itemId);
         if (entry == null) return this.Fail ("Unable to determine the entry");
         var eStyle = entry.Styles.FirstOrDefault (s => s.Id == id);
         if (eStyle == null)return this.Fail ("No style to delete.");
         entry.Styles.Remove (eStyle);
         this.Database.Save ();
         return Json ("success");
      }

      /// <summary>
      /// Gets information about the given style.
      /// </summary>
      /// <param name="id">Id of style to get.</param>
      /// <param name="itemId">Item style belongs to.</param>
      /// <returns>Json Results.</returns>
      public ActionResult GetItemEntryHeader (int id, int itemId)
      {
         var entry = this.Database.TermItemEntries.Get (itemId);
         if (entry == null) return this.Fail ("Unable to determine the entry");
         if (entry.Header == null) return this.Fail ("No information to get style for");
         var style = entry.Header.Styles.FirstOrDefault (s => s.Id == id);
         if (style == null) return this.Fail ("Unable to determine the style to get");
         return Json (new
         {
            itemId,
            id = style.Id,
            startIndex = style.StartIndex,
            endIndex = style.EndIndex,
            start = style.Style.Start,
            end = style.Style.End
         }, JsonRequestBehavior.AllowGet);
      }

      /// <summary>
      /// Posts a new style to the given item.
      /// </summary>
      /// <returns>Results.</returns>
      [HttpPost]
      [ValidateAntiForgeryToken]
      [Authorize (Roles = "Editor")]
      public ActionResult CreateItemEntryHeader (int itemId, string startStyle, string endStyle, int startIndex, int endIndex)
      {
         var style = GetStyle (startStyle, endStyle, false);
         var entry = this.Database.TermItemEntries.Get (itemId);
         if (entry == null) return this.Fail ("Unable to determine the entry");
         if (entry.Header == null) return this.Fail ("No header to apply style to.");
         var hStyle = new TermItemEntryHeaderStyle
         {
            Header = entry.Header,
            Style = style,
            StartIndex = startIndex,
            EndIndex = endIndex
         };
         entry.Header.Styles.Add (hStyle);
         this.Database.Save ();
         return Json (new { id = hStyle.Id, startIndex = hStyle.StartIndex, endIndex = hStyle.EndIndex });
      }

      /// <summary>
      /// Posts a new style to the given item.
      /// </summary>
      /// <returns>Results.</returns>
      [HttpPost]
      [ValidateAntiForgeryToken]
      [Authorize (Roles = "Editor")]
      public ActionResult UpdateItemEntryHeader (int id, int itemId, string startStyle, string endStyle, int startIndex, int endIndex)
      {
         var style = GetStyle (startStyle, endStyle, false);
         var entry = this.Database.TermItemEntries.Get (itemId);
         if (entry == null) return this.Fail ("Unable to determine the item entry");
         if (entry.Header == null) return this.Fail ("No header to apply style to");
         var hStyle = entry.Header.Styles.FirstOrDefault (s => s.Id == id);
         if (hStyle == null) return this.Fail ("Unable to determine the style to update");
         if (hStyle.Style.Id != style.Id) hStyle.Style = style;
         hStyle.EndIndex = endIndex;
         hStyle.StartIndex = startIndex;
         this.Database.Save ();
         return Json (new { id, startIndex, endIndex });
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
      public ActionResult DeleteItemEntryHeader (int id, int itemId)
      {
         var entry = this.Database.TermItemEntries.Get (itemId);
         if (entry == null) return this.Fail ("Unable to determine the entry");
         var header = entry.Header;
         if (header == null)return this.Fail ("No header to delete from.");
         var style = header.Styles.FirstOrDefault (s => s.Id == id);
         if (style == null) return this.Fail ("No style to delete.");
         header.Styles.Remove (style);
         this.Database.Save ();
         return Json ("success");
      }

      /// <summary>
      /// Gets information about the given style.
      /// </summary>
      /// <param name="id">Id of style to get.</param>
      /// <param name="itemId">Item style belongs to.</param>
      /// <param name="footerId">Id of footer.</param>
      /// <returns>Json Results.</returns>
      public ActionResult GetItemEntryFooter (int id, int itemId, int footerId)
      {
         var entry = this.Database.TermItemEntries.Get (itemId);
         if (entry == null) return this.Fail ("Unable to determine the entry");
         var footer = entry.Footers.FirstOrDefault (f => f.Id == footerId);
         if (footer == null) return this.Fail ("Unable to determine the footer");
         var style = footer.Styles.FirstOrDefault (s => s.Id == id);
         if (style == null) return this.Fail ("Unable to determine the style");
         return Json (new
         {
            id,
            itemId,
            startIndex = style.StartIndex,
            endIndex = style.EndIndex,
            start = style.Style.Start,
            end = style.Style.End
         }, JsonRequestBehavior.AllowGet);
      }

      /// <summary>
      /// Posts a new style to the given item.
      /// </summary>
      /// <returns>Results.</returns>
      [HttpPost]
      [ValidateAntiForgeryToken]
      [Authorize (Roles = "Editor")]
      public ActionResult CreateItemEntryFooter (int footerId, int itemId, string startStyle, string endStyle, int startIndex, int endIndex)
      {
         var style = GetStyle (startStyle, endStyle, false);
         var entry = this.Database.TermItemEntries.Get (itemId);
         if (entry == null) return this.Fail ("Unable to determine the entry");
         var footer = entry.Footers.FirstOrDefault (f => f.Id == footerId);
         if (footer == null) return this.Fail ("Unable to determine the footer");
         var fStyle = new TermItemEntryFooterStyle
         {
            Footer = footer,
            Style = style,
            StartIndex = startIndex,
            EndIndex = endIndex
         };
         footer.Styles.Add (fStyle);
         this.Database.Save ();
         return Json (new { id = fStyle.Id, startIndex = fStyle.StartIndex, endIndex = fStyle.EndIndex });
      }

      /// <summary>
      /// Posts a new style to the given item.
      /// </summary>
      /// <returns>Results.</returns>
      [HttpPost]
      [ValidateAntiForgeryToken]
      [Authorize (Roles = "Editor")]
      public ActionResult UpdateItemEntryFooter (int id, int itemId, int footerId, string startStyle, string endStyle, int startIndex, int endIndex)
      {
         var style = GetStyle (startStyle, endStyle, false);
         var entry = this.Database.TermItemEntries.Get (itemId);
         if (entry == null) return this.Fail ("Unable to determine the item entry");
         var footer = entry.Footers.FirstOrDefault (f => f.Id == footerId);
         if (footer == null) return this.Fail ("Unable to determine the footer");
         var fStyle = footer.Styles.FirstOrDefault (s => s.Id == id);
         if (fStyle == null) return this.Fail ("Unable to determine the style to update");
         if (fStyle.Style.Id != style.Id) fStyle.Style = style;
         fStyle.EndIndex = endIndex;
         fStyle.StartIndex = startIndex;
         this.Database.Save ();
         return Json (new { id, startIndex, endIndex });
      }

      /// <summary>
      /// Deletes a style for the given item.
      /// </summary>
      /// <param name="id">Id of style to get.</param>
      /// <param name="itemId">Id of parent entry.</param>
      /// <param name="footerId">The parent id of the item.</param>
      /// <returns>Results.</returns>
      [HttpPost]
      [ValidateAntiForgeryToken]
      [Authorize (Roles = "Editor")]
      public ActionResult DeleteItemEntryFooter (int id, int itemId, int footerId)
      {
         var entry = this.Database.TermItemEntries.Get (itemId);
         if (entry == null) return this.Fail ("Unable to determine the entry");
         var footer = entry.Footers.FirstOrDefault (h => h.Id == footerId);
         if (footer == null) return this.Fail ("No footer to delete from.");
         var style = footer.Styles.FirstOrDefault (s => s.Id == id);
         if (style == null) return this.Fail ("No style to delete.");
         footer.Styles.Remove (style);
         this.Database.Save ();
         return Json ("success");
      }

      #endregion

      #region Chapter

      /// <summary>
      /// Gets information about the given style.
      /// </summary>
      /// <param name="id">Id of style to get.</param>
      /// <param name="itemId">Item style belongs to.</param>
      /// <returns>Json Results.</returns>
      public ActionResult GetChapterHeader (int id, int itemId)
      {
         var chapter = this.Database.SubBookChapters.Get (itemId);
         if (chapter == null) return this.Fail ("Unable to determine the chapter");
         if (chapter.Header == null) return this.Fail ("Unable to determine the chapter footer");
         var style = chapter.Header.Styles.FirstOrDefault (s => s.Id == id);
         if (style == null) return this.Fail ("Unable to determine the style to get");
         return Json (new
         {
            itemId,
            id = style.Id,
            startIndex = style.StartIndex,
            endIndex = style.EndIndex,
            start = style.Style.Start,
            end = style.Style.End
         }, JsonRequestBehavior.AllowGet);
      }

      /// <summary>
      /// Posts a new style to the given item.
      /// </summary>
      /// <returns>Results.</returns>
      [HttpPost]
      [ValidateAntiForgeryToken]
      [Authorize (Roles = "Editor")]
      public ActionResult CreateChapterHeader (int itemId, string startStyle, string endStyle, int startIndex, int endIndex)
      {
         var style = GetStyle (startStyle, endStyle, false);
         var chapter = this.Database.SubBookChapters.Get (itemId);
         if (chapter == null) return this.Fail ("Unable to determine the chapter");
         if (chapter.Header == null) return this.Fail ("No header to apply style to.");
         var hStyle = new ChapterHeaderStyle
         {
            Header = chapter.Header,
            Style = style,
            StartIndex = startIndex,
            EndIndex = endIndex
         };
         chapter.Header.Styles.Add (hStyle);
         this.Database.Save ();
         return Json (new { id = hStyle.Id, startIndex = hStyle.StartIndex, endIndex = hStyle.EndIndex });
      }

      /// <summary>
      /// Posts a new style to the given item.
      /// </summary>
      /// <returns>Results.</returns>
      [HttpPost]
      [ValidateAntiForgeryToken]
      [Authorize (Roles = "Editor")]
      public ActionResult UpdateChapterHeader (int id, int itemId, string startStyle, string endStyle, int startIndex, int endIndex)
      {
         var style = GetStyle (startStyle, endStyle, false);
         var chapter = this.Database.SubBookChapters.Get (itemId);
         if (chapter == null) return this.Fail ("Unable to determine the chapter");
         if (chapter.Header == null) return this.Fail ("No header to apply style to.");
         var hStyle = chapter.Header.Styles.FirstOrDefault (s => s.Id == id);
         if (hStyle == null) return this.Fail ("Unable to determine the style to update");
         if (hStyle.Style.Id != style.Id) hStyle.Style = style;
         hStyle.EndIndex = endIndex;
         hStyle.StartIndex = startIndex;
         this.Database.Save ();
         return Json (new { id = hStyle.Id, startIndex = hStyle.StartIndex, endIndex = hStyle.EndIndex });
      }

      /// <summary>
      /// Deletes a style for the given item.
      /// </summary>
      /// <param name="id">Id of style to delete.</param>
      /// <param name="itemId">Id of parent entry.</param>
      /// <returns>Results.</returns>
      [HttpPost]
      [ValidateAntiForgeryToken]
      [Authorize (Roles = "Editor")]
      public ActionResult DeleteChapterHeader (int id, int itemId)
      {
         var chapter = this.Database.SubBookChapters.Get (itemId);
         if (chapter == null) return this.Fail ("Unable to determine the chapter");
         if (chapter.Header == null) return this.Fail ("No header to delete from.");
         var style = chapter.Header.Styles.FirstOrDefault (s => s.Id == id);
         if (style == null) return this.Fail ("No style to delete.");
         chapter.Header.Styles.Remove (style);
         this.Database.Save ();
         return Json ("success");
      }

      /// <summary>
      /// Gets information about the given style.
      /// </summary>
      /// <param name="id">Id of style to get.</param>
      /// <param name="itemId">Item style belongs to.</param>
      /// <returns>Json Results.</returns>
      public ActionResult GetChapterFooter (int id, int itemId)
      {
         var chapter = this.Database.SubBookChapters.Get (itemId);
         if (chapter == null) return this.Fail ("Unable to determine the chapter");
         if (chapter.Footer == null) return this.Fail ("Unable to determine the chapter footer");
         var style = chapter.Footer.Styles.FirstOrDefault (s => s.Id == id);
         if (style == null) return this.Fail ("Unable to determine the style to get");
         return Json (new
         {
            itemId,
            id = style.Id,
            startIndex = style.StartIndex,
            endIndex = style.EndIndex,
            start = style.Style.Start,
            end = style.Style.End
         }, JsonRequestBehavior.AllowGet);
      }

      /// <summary>
      /// Posts a new style to the given item.
      /// </summary>
      /// <returns>Results.</returns>
      [HttpPost]
      [ValidateAntiForgeryToken]
      [Authorize (Roles = "Editor")]
      public ActionResult CreateChapterFooter (int itemId, string startStyle, string endStyle, int startIndex, int endIndex)
      {
         var style = GetStyle (startStyle, endStyle, false);
         var chapter = this.Database.SubBookChapters.Get (itemId);
         if (chapter == null) return this.Fail ("Unable to determine the chapter");
         if (chapter.Footer == null) return this.Fail ("No footer to apply style to.");
         var hStyle = new ChapterFooterStyle
         {
            Footer = chapter.Footer,
            Style = style,
            StartIndex = startIndex,
            EndIndex = endIndex
         };
         chapter.Footer.Styles.Add (hStyle);
         this.Database.Save ();
         return Json (new { id = hStyle.Id, startIndex = hStyle.StartIndex, endIndex = hStyle.EndIndex });
      }

      /// <summary>
      /// Posts a new style to the given item.
      /// </summary>
      /// <returns>Results.</returns>
      [HttpPost]
      [ValidateAntiForgeryToken]
      [Authorize (Roles = "Editor")]
      public ActionResult UpdateChapterFooter (int id, int itemId, int footerId, string startStyle, string endStyle, int startIndex, int endIndex)
      {
         var style = GetStyle (startStyle, endStyle, false);
         var chapter = this.Database.SubBookChapters.Get (itemId);
         if (chapter == null) return this.Fail ("Unable to determine the passage");
         if (chapter.Footer == null) return this.Fail ("Unable to determine the footer");
         var fStyle = chapter.Footer.Styles.FirstOrDefault (s => s.Id == id);
         if (fStyle == null) return this.Fail ("Unable to determine the style to update");
         if (fStyle.Style.Id != style.Id) fStyle.Style = style;
         fStyle.EndIndex = endIndex;
         fStyle.StartIndex = startIndex;
         this.Database.Save ();
         return Json (new { id, startIndex, endIndex });
      }

      /// <summary>
      /// Deletes a style for the given item.
      /// </summary>
      /// <param name="id">Id of style to delete.</param>
      /// <param name="itemId">Id of parent entry.</param>
      /// <returns>Results.</returns>
      [HttpPost]
      [ValidateAntiForgeryToken]
      [Authorize (Roles = "Editor")]
      public ActionResult DeleteChapterFooter (int id, int itemId)
      {
         var chapter = this.Database.SubBookChapters.Get (itemId);
         if (chapter == null) return this.Fail ("Unable to determine the chapter");
         if (chapter.Footer == null) return this.Fail ("No header to delete from.");
         var style = chapter.Footer.Styles.FirstOrDefault (s => s.Id == id);
         if (style == null) return this.Fail ("No style to delete.");
         chapter.Footer.Styles.Remove (style);
         this.Database.Save ();
         return Json ("success");
      }

      #endregion

      /// <summary>
      /// Gets the style, or creates if not found.
      /// </summary>
      /// <param name="startStyle">The starting style.</param>
      /// <param name="endStyle">The ending style.</param>
      /// <param name="spansMultiple">Whether or not the style spans multiple entries.</param>
      /// <returns>The requested style.</returns>
      private Style GetStyle (string startStyle, string endStyle, bool spansMultiple)
      {
         var start = HttpUtility.UrlDecode (startStyle) ?? "<span>";
         var end = HttpUtility.UrlDecode (endStyle) ?? "</span>";
         var styles = this.Database.Styles.Get (s => s.Start == start  && s.End == end && s.SpansMultiple == spansMultiple);
         var style = styles.FirstOrDefault ();
         if (style == null)
         {
            style = new Style {Start = start, End = end, SpansMultiple = spansMultiple};
            this.Database.Styles.Insert (style);
            this.Database.Save ();
         }
         return style;
      }
   }
}
