using System;
using System.Linq;
using System.Web.Mvc;
using SeekDeepWithin.DataAccess;
using SeekDeepWithin.Models;
using SeekDeepWithin.Pocos;
using SeekDeepWithin.SdwSearch;

namespace SeekDeepWithin.Controllers
{
   public class SubBookController : SdwController
   {
      /// <summary>
      /// Initializes a new book controller.
      /// </summary>
      public SubBookController () : base (new SdwDatabase()) { }

      /// <summary>
      /// Initializes a new book controller with the given db info.
      /// </summary>
      /// <param name="db">Database object.</param>
      public SubBookController (ISdwDatabase db) : base (db) { }

      /// <summary>
      /// Adds a sub book to the given version.
      /// </summary>
      /// <param name="id">Id of version to add sub book to.</param>
      /// <param name="list">List of sub book.</param>
      /// <returns></returns>
      [HttpPost]
      [ValidateAntiForgeryToken]
      [Authorize (Roles = "Creator")]
      public ActionResult CreateChapters (int id, string list)
      {
         var subBook = this.Database.VersionSubBooks.Get (id);
         if (subBook == null) return this.Fail ("Unable to determine the sub book.");
         var chapters = list.Split (new[] {'\n'}, StringSplitOptions.RemoveEmptyEntries);
         foreach (var chap in chapters)
         {
            if (string.IsNullOrWhiteSpace (chap)) continue;
            var dbChapter = DbHelper.GetChapter(this.Database, chap);
            var maxOrder = (subBook.Chapters.Count > 0 ? subBook.Chapters.Max (c => c.Order) : 0) + 1;
            var chapter = new SubBookChapter { Chapter = dbChapter, Order = maxOrder, SubBook = subBook };
            subBook.Chapters.Add (chapter);
         }
         this.Database.Save ();
         if (subBook.Version.DefaultReadChapter == 0 && subBook.Chapters.Count > 0)
            subBook.Version.DefaultReadChapter = subBook.Chapters.First ().Id;
         DbHelper.CreateToc (this.Database, subBook.Version);
         return Json ("success");
      }

      /// <summary>
      /// Gets the edit view for a sub book.
      /// </summary>
      /// <param name="id">Id of sub book to edit.</param>
      /// <returns>The Edit view.</returns>
      public ActionResult Edit (int id)
      {
         var subBook = this.Database.VersionSubBooks.Get (id);
         AbbrevSearch.AddOrUpdateIndex(subBook, subBook.Term.Name.ToLower());
         var viewModel = new SubBookViewModel (subBook, true);
         var abbrevations = AbbrevSearch.Get (viewModel.Term.Id);
         foreach (var abbrevation in abbrevations)
            viewModel.Abbreviations.Add(abbrevation);
         return View (viewModel);
      }

      /// <summary>
      /// Edits properties of the sub book.
      /// </summary>
      /// <param name="id">The id of the sub book.</param>
      /// <param name="alias">The alias of the sub book</param>
      /// <param name="visible">Visibility.</param>
      /// <param name="termId">The term id of the sub book.</param>
      /// <returns>The Json Resuts</returns>
      [HttpPost]
      [ValidateAntiForgeryToken]
      [Authorize (Roles = "Editor")]
      public ActionResult Edit (int id, int termId, string alias, bool visible)
      {
         var subBook = this.Database.VersionSubBooks.Get (id);
         if (subBook == null) return this.Fail ("Unable to determine the sub book.");
         subBook.Hide = !visible;
         subBook.Alias = alias;
         var term = subBook.Term;
         var link = term.Links.FirstOrDefault (l => l.LinkType == (int)TermLinkType.Book && l.RefId == id);
         if (link != null) term.Links.Remove (link);
         if (subBook.Term.Id != termId)
         {
            term = this.Database.Terms.Get (termId);
            subBook.Term = term;
         }
         term.Links.Add (new TermLink { LinkType = (int)TermLinkType.Book, RefId = subBook.Id });
         this.Database.Save ();
         DbHelper.CreateToc (this.Database, subBook.Version);
         return Json ("success");
      }

      /// <summary>
      /// Adds a new abbreviation for the given sub book.
      /// </summary>
      /// <param name="id">Id of sub book to add abbreviation for.</param>
      /// <param name="abbrev">Abbreviation to add.</param>
      /// <returns>Json results.</returns>
      [HttpPost]
      [ValidateAntiForgeryToken]
      [Authorize (Roles = "Editor")]
      public ActionResult AddAbbreviation (int id, string abbrev)
      {
         var subBook = this.Database.VersionSubBooks.Get (id);
         AbbrevSearch.AddOrUpdateIndex( subBook, abbrev);
         return Json ("success");
      }

      /// <summary>
      /// Removes the given abbreviation.
      /// </summary>
      /// <param name="abbrev">Abbreviation to remove.</param>
      /// <returns>Json results.</returns>
      [HttpPost]
      [ValidateAntiForgeryToken]
      [Authorize (Roles = "Editor")]
      public ActionResult RemoveAbbreviation (string abbrev)
      {
         AbbrevSearch.Delete(abbrev);
         return Json ("success");
      }

      /// <summary>
      /// Gets auto complete items for the given sub book.
      /// </summary>
      /// <param name="name">Term to get auto complete items for.</param>
      /// <param name="versionId">The id of the version to get sub book for.</param>
      /// <returns>The list of possible terms for the given item.</returns>
      public ActionResult AutoComplete (string name, int versionId)
      {
         var result = new
         {
            suggestions = this.Database.VersionSubBooks.Get (sb => sb.Version.Id == versionId && sb.Term.Name.Contains (name))
                                                 .Select (sb => new { value = sb.Term.Name, data = sb.Id })
         };
         return Json (result, JsonRequestBehavior.AllowGet);
      }
   }
}