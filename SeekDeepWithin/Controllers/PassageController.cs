using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using SeekDeepWithin.DataAccess;
using SeekDeepWithin.Pocos;
using SeekDeepWithin.Models;
using SeekDeepWithin.SdwSearch;

namespace SeekDeepWithin.Controllers
{
   /// <summary>
   /// Controller for passages.
   /// </summary>
   public class PassageController : SdwController
   {
      /// <summary>
      /// Initializes a new controller.
      /// </summary>
      public PassageController () : base (new SdwDatabase ()) { }

      /// <summary>
      /// Initializes a new controller with the given db info.
      /// </summary>
      /// <param name="db">Database object.</param>
      public PassageController (ISdwDatabase db) : base (db) { }

      /// <summary>
      /// Gets the details page for the given item.
      /// </summary>
      /// <param name="entryId">The id of the entry we are getting details for.</param>
      /// <returns>The details page.</returns>
      public ActionResult Index (int entryId)
      {
         var entry = this.Database.PassageEntries.Get (entryId);
         return View (new PassageViewModel (entry));
      }

      /// <summary>
      /// Creates a passage for the given chapter.
      /// </summary>
      /// <param name="passageList">The list of passages to add.</param>
      /// <param name="subBookId">The id of the sub book.</param>
      /// <returns>Create results.</returns>
      [HttpPost]
      [ValidateAntiForgeryToken]
      [Authorize (Roles = "Creator")]
      public ActionResult Create (string passageList, int subBookId)
      {
         if (string.IsNullOrWhiteSpace (passageList)) return this.Fail ("No passages were given to add.");
         var subBook = this.Database.VersionSubBooks.Get (subBookId);
         if (subBook == null) return this.Fail ("Unable to determine the sub book.");
         var passages = passageList.Split (new [] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
         var lastChapter = -1;
         SubBookChapter sbChapter = null;
         foreach (var passage in passages) {
            var passageData = passage.Split ('|');
            var chapterOrder = passageData.FirstOrDefault (pd => pd.StartsWith ("[c]"));
            if (string.IsNullOrWhiteSpace (chapterOrder)) return this.Fail ("Unable to determine the chapter.");
            var cInt = Convert.ToInt32 (chapterOrder.Substring (3));
            if (lastChapter != cInt) {
               lastChapter = cInt;
               if (sbChapter != null) {
                  this.Database.Save ();
                  PassageSearch.AddOrUpdateIndex (sbChapter.Passages);
               }
               sbChapter = subBook.Chapters.FirstOrDefault (c => c.Order == lastChapter);
            }
            if (sbChapter == null)
               return this.Fail ("Unable to determine the correct chapter to add passages to: " + chapterOrder);
            var cHeader = passageData.FirstOrDefault (pd => pd.StartsWith ("[ch]"));
            if (!string.IsNullOrWhiteSpace (cHeader))
               sbChapter.Header = new ChapterHeader { Text = cHeader.Substring (4) };
            var number = passageData.FirstOrDefault (pd => pd.StartsWith ("[n]"));
            if (string.IsNullOrWhiteSpace (number)) return this.Fail ("Passage number was not supplied.");
            var order = passageData.FirstOrDefault (pd => pd.StartsWith ("[o]"));
            if (string.IsNullOrWhiteSpace (order)) return this.Fail ("Passage order was not supplied.");
            var text = passageData.FirstOrDefault (pd => pd.StartsWith ("[t]"));
            if (string.IsNullOrWhiteSpace (text)) return this.Fail ("Passage text was not supplied.");
            var header = passageData.FirstOrDefault (pd => pd.StartsWith ("[h]"));
            var passageEntry = new PassageEntry {
               Chapter = sbChapter,
               Number = Convert.ToInt32 (number.Substring (3)),
               Order = Convert.ToInt32 (order.Substring (3)),
               Passage = this.GetPassage (text.Substring (3))
            };
            if (!string.IsNullOrWhiteSpace (header))
               passageEntry.Header = new PassageHeader { Text = header.Substring (3) };
            var reg = new Regex ("\\[f@(\\d+)\\](.+)");
            foreach (var footer in passageData.Where (pd => pd.StartsWith ("[f@"))) {
               var match = reg.Match (footer);
               if (match.Success) {
                  var index = Convert.ToInt32 (match.Groups [1].Value);
                  var fText = match.Groups [2].Value;
                  if (passageEntry.Footers == null) passageEntry.Footers = new Collection<PassageFooter> ();
                  passageEntry.Footers.Add (new PassageFooter { Index = index, Text = fText });
               }
            }
            sbChapter.Passages.Add (passageEntry);
         }
         this.Database.Save ();
         if (sbChapter != null)
            PassageSearch.AddOrUpdateIndex (sbChapter.Passages);
         PassageSearch.Optimize ();
         return this.Success ();
      }

      /// <summary>
      /// Performs an edit for the given passage.
      /// </summary>
      /// <param name="entryId">The passage entry to edit.</param>
      /// <param name="text">The passage text.</param>
      /// <param name="order">The passage order.</param>
      /// <param name="number">The passage number.</param>
      /// <param name="header">A header for the passage.</param>
      /// <returns>The edit page.</returns>
      [HttpPost]
      [ValidateAntiForgeryToken]
      [Authorize (Roles = "Editor")]
      public ActionResult Update (int entryId, string text, int? order, int? number, string header)
      {
         var passage = this.Database.PassageEntries.Get (entryId);
         if (passage == null) return this.Fail ("Unable to determine the passage");
         passage.Passage.Text = text;
         if (!string.IsNullOrWhiteSpace (header)) {
            if (passage.Header == null)
               passage.Header = new PassageHeader { Text = header };
            else
               passage.Header.Text = header;
         } else if (passage.Header != null) {
            passage.Header.Styles.Clear ();
            passage.Header = null;
         }
         if (order != null)
            passage.Order = order.Value;
         if (number != null)
            passage.Number = number.Value;
         this.Database.Save ();
         PassageSearch.AddOrUpdateIndex (passage);
         return this.Success ();
      }

      /// <summary>
      /// Deletes the given passage.
      /// </summary>
      /// <param name="entryId">The passage entry to delete.</param>
      /// <returns>The edit page.</returns>
      [HttpPost]
      [ValidateAntiForgeryToken]
      [Authorize (Roles = "Creator")]
      public ActionResult Delete (int entryId)
      {
         var passage = this.Database.PassageEntries.Get (entryId);
         if (passage == null) return this.Fail ("Unable to determine the passage");
         passage.Chapter.Passages.Remove (passage);
         if (passage.Passage.Entries.Count == 1)
            this.Database.Passages.Delete (passage.Passage);
         this.Database.Save ();
         PassageSearch.Delete (passage.Id);
         return this.Success ();
      }

      /// <summary>
      /// Gets the edit view for the given entry.
      /// </summary>
      /// <param name="id">The id of the item we editing.</param>
      /// <returns></returns>
      [Authorize (Roles = "Editor")]
      public ActionResult Edit (int id)
      {
         var entry = this.Database.PassageEntries.Get (id);
         if (entry == null) return this.Fail ("Unable to determine the passage");
         var viewModel = new EditItemViewModel (id, EditItemType.Passage) { Text = entry.Passage.Text };
         foreach (var style in entry.Styles)
            viewModel.Styles.Add (new StyleViewModel (style));
         foreach (var link in entry.Passage.Links)
            viewModel.Links.Add (new LinkViewModel (link));
         foreach (var footer in entry.Footers)
            viewModel.Footers.Add (new HeaderFooterViewModel (footer));
         return PartialView ("_EditItem", viewModel);
      }

      /// <summary>
      /// Gets the edit view for the given entry.
      /// </summary>
      /// <param name="id">The id of the item we editing.</param>
      /// <returns></returns>
      [Authorize (Roles = "Editor")]
      public ActionResult EditHeader (int id)
      {
         var entry = this.Database.PassageEntries.Get (id);
         if (entry == null) return this.Fail ("Unable to determine the passage");
         var viewModel = new EditItemViewModel (id, EditItemType.PassageHeader) {
            HasLinks = false,
            HasFooters = false
         };
         if (entry.Header != null) {
            viewModel.Text = entry.Header.Text;
            foreach (var style in entry.Header.Styles)
               viewModel.Styles.Add (new StyleViewModel (style));
         }
         return PartialView ("_EditItem", viewModel);
      }

      /// <summary>
      /// Gets the edit view for the given entry.
      /// </summary>
      /// <param name="id">The id of the item we editing.</param>
      /// <param name="footerId">The id of the footer we are editing.</param>
      /// <returns></returns>
      [Authorize (Roles = "Editor")]
      public ActionResult EditFooter (int id, int footerId)
      {
         var entry = this.Database.PassageEntries.Get (id);
         if (entry == null) return this.Fail ("Unable to determine the passage");
         var footer = entry.Footers.FirstOrDefault (f => f.Id == footerId);
         if (footer == null) return this.Fail ("Unable to determine the footer");
         var viewModel = new EditItemViewModel (id, EditItemType.PassageFooter) {
            Text = footer.Text,
            HasFooters = false,
            FooterId = footerId
         };
         foreach (var style in footer.Styles)
            viewModel.Styles.Add (new StyleViewModel (style));
         foreach (var link in footer.Links)
            viewModel.Links.Add (new LinkViewModel (link));
         return PartialView ("_EditItem", viewModel);
      }

      /// <summary>
      /// Gets the entry data for the given entry id.
      /// </summary>
      /// <param name="id">Id of entry to get text for.</param>
      /// <returns>The text of the entry.</returns>
      [AllowAnonymous]
      public ActionResult Get (int id)
      {
         var entry = this.Database.PassageEntries.Get (id);
         if (entry == null) return this.Fail ("Unable to determine the passage");
         var result = new {
            status = SUCCESS,
            entryId = id,
            order = entry.Order,
            passageId = entry.Passage.Id,
            passageNumber = entry.Number,
            passageText = entry.Passage.Text,
            header = entry.Header == null ? string.Empty : entry.Header.Text
         };
         return Json (result, JsonRequestBehavior.AllowGet);
      }

      /// <summary>
      /// Gets the passage in the database with the given text.
      /// </summary>
      /// <param name="text">The text of the passage to get.</param>
      /// <returns>The requested passage, a new passage if does not exist.</returns>
      private Passage GetPassage (string text)
      {
         var passages = this.Database.Passages.Get (h => h.Text == text);
         var passage = passages.FirstOrDefault ();
         if (passage == null) {
            passage = new Passage { Text = text };
            this.Database.Passages.Insert (passage);
            this.Database.Save ();
         }
         return passage;
      }

      /// <summary>
      /// Gets a random passage.
      /// </summary>
      /// <returns></returns>
      public PassageViewModel GetRandomPassage ()
      {
         return new PassageViewModel (this.Database.PassageEntries.All (q => q.OrderBy (r => Guid.NewGuid ())).Take (1).FirstOrDefault ());
      }

      /// <summary>
      /// Gets the list of passages for the given chapter.
      /// </summary>
      /// <param name="id">Id of chapter to get passages for.</param>
      /// <returns>A JSON result.</returns>
      public ActionResult List (int id)
      {
         var chapter = this.Database.SubBookChapters.Get (id);
         if (chapter == null)
            return this.Fail ("Unable to determine the chapter.");

         var result = new {
            status = SUCCESS,
            count = chapter.Passages.Count,
            passages = chapter.Passages.Select (p => new {
               id = p.Id,
               order = p.Order,
               number = p.Number,
               text = p.Passage.Text,
               passageid = p.Passage.Id
            })
         };
         return Json (result, JsonRequestBehavior.AllowGet);
      }
   }
}
