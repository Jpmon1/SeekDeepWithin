using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using SeekDeepWithin.DataAccess;
using SeekDeepWithin.Pocos;
using SeekDeepWithin.Models;
using SeekDeepWithin.SdwSearch;
using Version = SeekDeepWithin.Pocos.Version;

namespace SeekDeepWithin.Controllers
{
   /// <summary>
   /// Controller for versions.
   /// </summary>
   public class VersionController : SdwController
   {
      /// <summary>
      /// Initializes a new book controller.
      /// </summary>
      public VersionController () : base (new SdwDatabase ()) { }

      /// <summary>
      /// Initializes a new book controller with the given db info.
      /// </summary>
      /// <param name="db">Database object.</param>
      public VersionController (ISdwDatabase db) : base (db) { }

      /// <summary>
      /// Gets the create new version view.
      /// </summary>
      /// <param name="bookId">Id of book to add version for.</param>
      /// <param name="bookTitle">The title of the book.</param>
      /// <returns>Create version view.</returns>
      [Authorize (Roles = "Creator")]
      public ActionResult Create (int bookId, string bookTitle)
      {
         ViewBag.BookId = bookId;
         ViewBag.BookTitle = bookTitle;
         return View ();
      }

      /// <summary>
      /// Gets the create new version view.
      /// </summary>
      /// <param name="title">The version's title.</param>
      /// <param name="date">The date of the version.</param>
      /// <param name="bookId">The id of the version's book.</param>
      /// <param name="termId">The id of the associated term.</param>
      /// <returns>Json results.</returns>
      [HttpPost]
      [ValidateAntiForgeryToken]
      [Authorize (Roles = "Creator")]
      public ActionResult Create (string title, string date, int bookId, int termId)
      {
         if (string.IsNullOrWhiteSpace (title))
            return this.Fail ("A title must be specified.");
         var book = this.Database.Books.Get (bookId);
         var term = this.Database.Terms.Get (termId);
         if (book == null)
            return this.Fail ("Unable to determine the associated book.");
         if (term == null)
            return this.Fail ("Unable to determine the associated term.");

         var version = new Version {
            Book = book,
            Title = title,
            PublishDate = date,
            Term = term,
            Modified = DateTime.Now
         };
         if (book.Versions.Count <= 0)
            book.DefaultVersion = version;
         book.Versions.Add (version);
         this.Database.Versions.Insert (version);
         this.Database.Save ();
         if (term.Links == null) term.Links = new Collection<TermLink> ();
         term.Links.Add (new TermLink { LinkType = (int) TermLinkType.Version, RefId = version.Id });
         this.Database.Save ();
         BookSearch.AddOrUpdateIndex (book);
         return Json (new { status = "success", title, id = version.Id, bookId, termId });
      }

      /// <summary>
      /// Gets the edit version page.
      /// </summary>
      /// <param name="id">The id of the version to edit.</param>
      /// <returns>The edit page.</returns>
      [Authorize (Roles = "Editor")]
      public ActionResult Edit (int id)
      {
         return View (new VersionViewModel (this.Database.Versions.Get (id), true));
      }

      /// <summary>
      /// Gets the edit version Page.
      /// </summary>
      /// <param name="id">The version's id.</param>
      /// <param name="title">The version's title.</param>
      /// <param name="date">The date of the version.</param>
      /// <param name="termId">The id of the associated term.</param>
      /// <param name="sourceName">The name of the source.</param>
      /// <param name="sourceUrl">The url of the source.</param>
      /// <returns>The edit version page.</returns>
      [HttpPost]
      [ValidateAntiForgeryToken]
      [Authorize (Roles = "Editor")]
      public ActionResult Edit (int id, string title, string date, int termId, string sourceName, string sourceUrl)
      {
         var version = this.Database.Versions.Get (id);
         version.Title = title;
         version.PublishDate = date;
         version.SourceUrl = sourceUrl;
         version.SourceName = sourceName;
         version.Modified = DateTime.Now;
         if (version.Term.Id != termId) {
            var term = version.Term;
            var link = term.Links.FirstOrDefault (l => l.LinkType == (int) TermLinkType.Version && l.RefId == id);
            if (link != null) term.Links.Remove (link);
            term = this.Database.Terms.Get (termId);
            version.Term = term;
            term.Links.Add (new TermLink { LinkType = (int) TermLinkType.Version, RefId = version.Id });
         }
         this.Database.Save ();
         BookSearch.AddOrUpdateIndex (version.Book);
         return this.Success ();
      }

      /// <summary>
      /// Adds a sub book to the given version.
      /// </summary>
      /// <param name="id">Id of version to add sub book to.</param>
      /// <param name="list">List of sub book.</param>
      /// <returns></returns>
      [HttpPost]
      [ValidateAntiForgeryToken]
      [Authorize (Roles = "Creator")]
      public ActionResult CreateSubBooks (int id, string list)
      {
         var version = this.Database.Versions.Get (id);
         if (version == null) return this.Fail ("Unable to determine the version.");
         var subBooks = list.Split (new [] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
         foreach (var sb in subBooks) {
            var sbName = sb.Trim ();
            string alias = null;
            if (sbName.Contains ("|")) {
               var nameSplit = sbName.Split (new [] { '|' }, StringSplitOptions.RemoveEmptyEntries);
               sbName = nameSplit [0].Trim();
               alias = nameSplit [1].Trim();
            }
            if (string.IsNullOrWhiteSpace (sbName)) continue;
            var term = this.Database.Terms.Get (t => t.Name == sbName).FirstOrDefault ();
            if (term == null) {
               term = new Term { Name = sbName };
               this.Database.Terms.Insert (term);
               this.Database.Save ();
               TermSearch.AddOrUpdateIndex (term);
            }
            var maxOrder = (version.SubBooks.Count > 0 ? version.SubBooks.Max (c => c.Order) : 0) + 1;
            var subBook = new VersionSubBook {
               Term = term,
               Order = maxOrder,
               Version = version,
               Alias = alias,
               Modified = DateTime.Now
            };
            AbbrevSearch.AddOrUpdateIndex (subBook, subBook.Term.Name.Replace (" ", string.Empty).ToLower ());
            version.SubBooks.Add (subBook);
         }
         this.Database.Save ();
         foreach (var subBook in version.SubBooks) {
            if (subBook.Term.Links == null) subBook.Term.Links = new Collection<TermLink> ();
            subBook.Term.Links.Add (new TermLink { LinkType = (int) TermLinkType.SubBook, RefId = subBook.Id, Name = subBook.Term.Name });
         }
         this.Database.Save ();
         DbHelper.CreateToc (this.Database, version);
         return
            Json (new {
               status = SUCCESS,
               message = "Sub Books created!",
               subbooks =
                  version.SubBooks.Select (s => new {
                     id = s.Id,
                     hide = false,
                     order = s.Order,
                     termname = s.Term.Name,
                     termid = s.Term.Id,
                     alias = s.Alias ?? string.Empty
                  })
            });
      }

      /// <summary>
      /// Sets the default read chapter for the given version..
      /// </summary>
      /// <param name="id">The version id.</param>
      /// <param name="chapterId">The chapter id.</param>
      /// <returns>Create version view.</returns>
      [HttpPost]
      [ValidateAntiForgeryToken]
      [Authorize (Roles = "Creator")]
      public ActionResult DefaultReadChapter (int id, int chapterId)
      {
         var version = this.Database.Versions.Get (id);
         if (version == null) return this.Fail ("Unknown version");
         version.DefaultReadChapter = chapterId;
         this.Database.Save ();
         return this.Success ();
      }

      /// <summary>
      /// Gets the list of versions for the given book.
      /// </summary>
      /// <param name="id">Id of book to get versions for.</param>
      /// <returns>A JSON result.</returns>
      public ActionResult List (int id)
      {
         var book = this.Database.Books.Get (id);
         if (book == null)
            return this.Fail ("Unable to determine the book");

         var result = new {
            status = SUCCESS,
            count = book.Versions.Count,
            versions = book.Versions.Select (v => new {
               id = v.Id,
               bookid = id,
               title = v.Title,
               contents = v.Contents,
               modified = v.Modified.ToString (CultureInfo.InvariantCulture),
               defaultread = v.DefaultReadChapter,
               publishdate = v.PublishDate,
               termid = v.Term.Id,
               termname = v.Term.Name,
               sourcename = v.SourceName,
               sourceurl = v.SourceUrl
            })
         };
         return Json (result, JsonRequestBehavior.AllowGet);
      }

      /// <summary>
      /// Gets auto complete items.
      /// </summary>
      /// <param name="title">Title to get auto complete items for.</param>
      /// <param name="bookId">The id of the book to look up versions for.</param>
      /// <returns>The list of possible items.</returns>
      public ActionResult AutoComplete (string title, int bookId)
      {
         var result = new {
            suggestions = this.Database.Versions.Get (v => v.Book.Id == bookId && v.Title.Contains (title))
                                                 .Select (v => new { value = v.Title, data = v.Id })
         };
         return Json (result, JsonRequestBehavior.AllowGet);
      }
   }
}
