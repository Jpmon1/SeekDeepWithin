using System.Linq;
using System.Web.Mvc;
using SeekDeepWithin.DataAccess;
using SeekDeepWithin.Domain;
using SeekDeepWithin.Models;

namespace SeekDeepWithin.Controllers
{
   /// <summary>
   /// Controller for passages.
   /// </summary>
   public class PassageController : Controller
   {
      private readonly ISdwDatabase m_Db;

      /// <summary>
      /// Initializes a new controller.
      /// </summary>
      public PassageController ()
      {
         this.m_Db = new SdwDatabase ();
      }

      /// <summary>
      /// Initializes a new controller with the given db info.
      /// </summary>
      /// <param name="db">Database object.</param>
      public PassageController (ISdwDatabase db)
      {
         this.m_Db = db;
      }

      /// <summary>
      /// Gets the details page for the given item.
      /// </summary>
      /// <param name="entryId">The id of the entry we are getting details for.</param>
      /// <returns>The details page.</returns>
      public ActionResult Details (int entryId)
      {
         var entry = this.m_Db.PassageEntries.Get (entryId);
         return View (GetViewModel (entry.Passage, entry));
      }

      /// <summary>
      /// Gets a passage view model for the given passage and entry.
      /// </summary>
      /// <param name="passage">Passage.</param>
      /// <param name="entry">Entry passage belongs to.</param>
      /// <returns>Passage view model.</returns>
      public static PassageViewModel GetViewModel (Passage passage, PassageEntry entry = null)
      {
         var viewModel = new PassageViewModel { Text = passage.Text, Id = passage.Id };
         foreach (var link in passage.PassageLinks)
         {
            viewModel.Links.Add (new LinkViewModel
            {
               StartIndex = link.StartIndex,
               EndIndex = link.EndIndex,
               Url = link.Link.Url,
               OpenInNewWindow = link.OpenInNewWindow
            });
         }
         if (entry != null)
         {
            viewModel.EntryId = entry.Id;
            viewModel.Number = entry.Number;
            viewModel.ChapterId = entry.Chapter.Id;
            viewModel.ChapterName = entry.Chapter.Name;
            viewModel.SubBookId = entry.Chapter.SubBook.Id;
            viewModel.SubBookName = entry.Chapter.SubBook.Name;
            viewModel.VersionId = entry.Chapter.SubBook.Version.Id;
            viewModel.VersionName = entry.Chapter.SubBook.Version.Title;

            foreach (var style in entry.Styles)
            {
               viewModel.Styles.Add (new StyleViewModel
               {
                  StartIndex = style.StartIndex,
                  EndIndex = style.EndIndex,
                  Start = style.Style.Start,
                  End = style.Style.End
               });
            }

            foreach (var header in entry.Headers)
            {
               viewModel.Headers.Add (new HeaderFooterViewModel
               {
                  Text = header.Header.Text,
                  IsBold = header.IsBold,
                  IsItalic = header.IsItalic,
                  Justify = header.Justify
               });
            }

            foreach (var footer in entry
               .Footers)
            {
               viewModel.Footers.Add (new HeaderFooterViewModel
               {
                  IsBold = footer.IsBold,
                  IsItalic = footer.IsItalic,
                  Justify = footer.Justify,
                  Text = footer.Footer.Text
               });
            }
         }
         return viewModel;
      }

      /// <summary>
      /// Gets the edit Page.
      /// </summary>
      /// <param name="id">The id of the item to edit.</param>
      /// <returns>The edit page.</returns>
      [Authorize (Roles = "Editor")]
      public ActionResult Edit (int id)
      {
         if (Request.UrlReferrer != null) TempData["RefUrl"] = Request.UrlReferrer.ToString();
         var passage = this.m_Db.Passages.Get (id);
         return View (GetViewModel (passage));
      }

      /// <summary>
      /// Performs an edit for the given model.
      /// </summary>
      /// <param name="viewModel">The view model with edits.</param>
      /// <returns>The edit page.</returns>
      [HttpPost]
      [ValidateAntiForgeryToken]
      [Authorize (Roles = "Editor")]
      public ActionResult Edit (PassageViewModel viewModel)
      {
         if (ModelState.IsValid)
         {
            var passage = this.m_Db.Passages.Get (viewModel.Id);
            this.m_Db.SetValues (passage, viewModel);
            this.m_Db.Save ();
            return Json ("Success");
         }
         Response.StatusCode = 500;
         return Json ("Data is not valid.");
      }

      /// <summary>
      /// Creates a passage for the given chapter.
      /// </summary>
      /// <param name="viewModel">View model with data.</param>
      /// <returns>Create results.</returns>
      [HttpPost]
      [ValidateAntiForgeryToken]
      [Authorize (Roles = "Creator")]
      public ActionResult Create (AddPassageViewModel viewModel)
      {
         if (ModelState.IsValid)
         {
            var chapter = this.m_Db.Chapters.Get (viewModel.ChapterId);
            var passageEntry = new PassageEntry
            {
               Chapter = chapter,
               ChapterId = viewModel.ChapterId,
               Number = viewModel.Number,
               Order = viewModel.Order,
               Passage = this.GetPassage (viewModel.Text)
            };
            this.m_Db.Passages.Insert (passageEntry.Passage);
            chapter.Passages.Add (passageEntry);
            this.m_Db.Save ();
            return Json ("Success");
         }
         Response.StatusCode = 500;
         return Json ("Data is not valid.");
      }

      /// <summary>
      /// Gets the passage in the database with the given text.
      /// </summary>
      /// <param name="text">The text of the passage to get.</param>
      /// <returns>The requested passage, a new passage if does not exist.</returns>
      private Passage GetPassage (string text)
      {
         var passages = this.m_Db.Passages.Get (h => h.Text == text);
         var passage = passages.FirstOrDefault ();
         if (passage == null)
         {
            passage = new Passage { Text = text };
            this.m_Db.Passages.Insert (passage);
            this.m_Db.Save ();
         }
         return passage;
      }

      /// <summary>
      /// Gets the entry data for the given entry id.
      /// </summary>
      /// <param name="id">Id of entry to get text for.</param>
      /// <returns>The text of the entry.</returns>
      [AllowAnonymous]
      public ActionResult GetEntry (int id)
      {
         var entry = this.m_Db.PassageEntries.Get (id);
         var result = new
         {
            entryId = id,
            order = entry.Order,
            passageId = entry.PassageId,
            passageNumber = entry.Number,
            passageText = entry.Passage.Text,
            headers = entry.Headers.Select(h => new { text = h.Header.Text, id = h.Id }),
            footers = entry.Footers.Select(f => new { text = f.Footer.Text, index = f.Index, id = f.Id })
         };
         return Json (result, JsonRequestBehavior.AllowGet);
      }
   }
}
