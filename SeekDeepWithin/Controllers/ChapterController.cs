using System.Linq;
using System.Web.Mvc;
using SeekDeepWithin.DataAccess;
using SeekDeepWithin.Models;
using SeekDeepWithin.Pocos;

namespace SeekDeepWithin.Controllers
{
   public class ChapterController : SdwController
   {
      /// <summary>
      /// Initializes a new chapter controller.
      /// </summary>
      public ChapterController () : base (new SdwDatabase ()) { }

      /// <summary>
      /// Initializes a new chapter controller with the given db info.
      /// </summary>
      /// <param name="db">Database object.</param>
      public ChapterController (ISdwDatabase db) : base (db) { }

      /// <summary>
      /// Gets the edit chapter Page.
      /// </summary>
      /// <param name="id">The id of the chapter to edit.</param>
      /// <returns>The edit page.</returns>
      [Authorize (Roles = "Editor")]
      public ActionResult Edit (int id)
      {
         var chapter = this.Database.SubBookChapters.Get (id);
         return View (new ChapterViewModel (chapter));
      }

      /// <summary>
      /// Performs edit actions on the given chapter.
      /// </summary>
      /// <param name="id">Id of chapter to edit.</param>
      /// <param name="name">New name of chapter.</param>
      /// <param name="order">New order of chapter.</param>
      /// <param name="visible">New visibility of chapter.</param>
      /// <param name="para">New default view of chapter.</param>
      /// <param name="header">The chapter's header.</param>
      /// <param name="footer">The chapter's footer.</param>
      /// <returns>Json Results</returns>
      [HttpPost]
      [ValidateAntiForgeryToken]
      [Authorize (Roles = "Editor")]
      public ActionResult Edit (int id, string name, int order, bool visible, bool para, string header, string footer)
      {
         var chapter = this.Database.SubBookChapters.Get (id);
         if (chapter == null) return this.Fail ("Unable to determine the chapter.");
         chapter.DefaultToParagraph = para;
         chapter.Chapter.Name = name;
         chapter.Hide = !visible;
         chapter.Order = order;
         if (!string.IsNullOrWhiteSpace (header)) {
            if (chapter.Header == null)
               chapter.Header = new ChapterHeader { Text = header };
            else
               chapter.Header.Text = header;
         } else if (chapter.Header != null) {
            chapter.Header.Styles.Clear ();
            chapter.Header = null;
         }
         if (!string.IsNullOrWhiteSpace (footer)) {
            if (chapter.Footer == null)
               chapter.Footer = new ChapterFooter { Text = footer };
            else
               chapter.Footer.Text = footer;
         } else if (chapter.Footer != null) {
            chapter.Footer.Links.Clear ();
            chapter.Footer.Styles.Clear ();
            chapter.Footer = null;
         }
         this.Database.Save ();
         DbHelper.CreateToc (this.Database, chapter.SubBook.Version);
         return this.Success ();
      }

      /// <summary>
      /// Gets the edit view for the given chapter.
      /// </summary>
      /// <param name="id"></param>
      /// <returns></returns>
      [Authorize (Roles = "Editor")]
      public ActionResult EditHeader (int id)
      {
         var chapter = this.Database.SubBookChapters.Get (id);
         var viewModel = new EditItemViewModel (id, EditItemType.ChapterHeader) { HasLinks = false, HasFooters = false };
         if (chapter.Header != null) {
            viewModel.Text = chapter.Header.Text;
            foreach (var style in chapter.Header.Styles)
               viewModel.Styles.Add (new StyleViewModel (style));
         }
         return PartialView ("_EditItem", viewModel);
      }

      /// <summary>
      /// Deletes the given chapter.
      /// </summary>
      /// <param name="id">Id of chapter to delete.</param>
      /// <returns>The JSON results.</returns>
      [HttpPost]
      [ValidateAntiForgeryToken]
      [Authorize (Roles = "Editor")]
      public ActionResult Delete (int id)
      {
         var chapter = this.Database.SubBookChapters.Get (id);
         if (chapter == null) return this.Fail ("Unable to determine the chapter.");
         var subBook = chapter.SubBook;
         var passages = chapter.Passages.ToList ();
         for (int a = passages.Count - 1; a >= 0; a--)
            chapter.Passages.Remove (passages [a]);
         subBook.Chapters.Remove (chapter);
         foreach (var passage in passages)
            this.Database.PassageEntries.Delete (passage);
         this.Database.SubBookChapters.Delete (chapter);
         this.Database.Save ();
         DbHelper.CreateToc (this.Database, subBook.Version);
         return this.Success ();
      }

      /// <summary>
      /// Gets the edit view for the given chapter.
      /// </summary>
      /// <param name="id"></param>
      /// <returns></returns>
      [Authorize (Roles = "Editor")]
      public ActionResult EditFooter (int id)
      {
         var chapter = this.Database.SubBookChapters.Get (id);
         var viewModel = new EditItemViewModel (id, EditItemType.ChapterFooter) { HasFooters = false };
         if (chapter.Footer != null) {
            viewModel.Text = chapter.Footer.Text;
            foreach (var style in chapter.Footer.Styles)
               viewModel.Styles.Add (new StyleViewModel (style));
            foreach (var link in chapter.Footer.Links)
               viewModel.Links.Add (new LinkViewModel (link));
         }
         return PartialView ("_EditItem", viewModel);
      }

      /// <summary>
      /// Gets the contents view.
      /// </summary>
      /// <param name="id">Id of chapter to get contents for.</param>
      /// <returns>The contents HTML.</returns>
      public ActionResult Contents (int id)
      {
         var chapter = this.Database.SubBookChapters.Get (id);
         var contents = new VersionContents (chapter.SubBook.Version.Title, chapter.SubBook.Version.Contents,
            chapter.SubBook.Version.Id, chapter.SubBook.Id, id);
         return PartialView (contents);
      }

      /// <summary>
      /// Gets the list of chapters for the given sub book.
      /// </summary>
      /// <param name="id">Id of sub book to get chapters for.</param>
      /// <returns>A JSON result.</returns>
      public ActionResult List (int id)
      {
         var subBook = this.Database.VersionSubBooks.Get (id);
         if (subBook == null)
            return this.Fail ("Unable to determine the sub book.");

         var result = new {
            status = SUCCESS,
            count = subBook.Chapters.Count,
            chapters = subBook.Chapters.Select (c => new {
               id = c.Id,
               hide = c.Hide,
               order = c.Order,
               name = c.Chapter.Name,
               chapterid = c.Chapter.Id,
               isparagraphs = c.DefaultToParagraph
            })
         };
         return Json (result, JsonRequestBehavior.AllowGet);
      }

      /// <summary>
      /// Gets auto complete items for the given item.
      /// </summary>
      /// <param name="name">Name to get auto complete items for.</param>
      /// <param name="subBookId">The id of the sub book to look up chapters for.</param>
      /// <returns>The list of possible items.</returns>
      public ActionResult AutoComplete (string name, int subBookId)
      {
         var result = new {
            suggestions = this.Database.SubBookChapters.Get (sc => sc.SubBook.Id == subBookId && sc.Chapter.Name.Contains (name))
                                                 .Select (c => new { value = c.Chapter.Name, data = c.Id })
         };
         return Json (result, JsonRequestBehavior.AllowGet);
      }
   }
}
