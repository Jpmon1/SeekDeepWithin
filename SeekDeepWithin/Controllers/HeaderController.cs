using System.Linq;
using System.Web.Mvc;
using SeekDeepWithin.DataAccess;
using SeekDeepWithin.Domain;
using SeekDeepWithin.Models;

namespace SeekDeepWithin.Controllers
{
   /// <summary>
   /// Controller used for headers.
   /// </summary>
   public class HeaderController : Controller
   {
      private readonly ISdwDatabase m_Db;

      /// <summary>
      /// Initializes a new controller.
      /// </summary>
      public HeaderController ()
      {
         this.m_Db = new SdwDatabase ();
      }

      /// <summary>
      /// Initializes a new controller with the given db info.
      /// </summary>
      /// <param name="db">Database object.</param>
      public HeaderController (ISdwDatabase db)
      {
         this.m_Db = db;
      }

      /// <summary>
      /// Gets the view to create a new header for the given entry.
      /// </summary>
      /// <param name="itemId">The id of the item the header is for.</param>
      /// <param name="type">The type of header to create.</param>
      /// <returns>The partial header create view.</returns>
      public ActionResult Create (int itemId, string type)
      {
         return PartialView (new HeaderFooterViewModel { ItemId = itemId, Type = type });
      }

      /// <summary>
      /// Creates a header for a passage.
      /// </summary>
      /// <param name="viewModel">View model with header information.</param>
      /// <returns>Result</returns>
      [HttpPost]
      [ValidateAntiForgeryToken]
      [Authorize (Roles = "Editor")]
      public ActionResult CreateForPassage (HeaderFooterViewModel viewModel)
      {
         if (ModelState.IsValid)
         {
            var entry = this.m_Db.PassageEntries.Get (viewModel.ItemId);
            var header = this.GetHeader (viewModel.Text);
            entry.Headers.Add (new PassageHeader
            {
               Passage = entry,
               Header = header,
               Justify = viewModel.Justify,
               IsBold = viewModel.IsBold,
               IsItalic = viewModel.IsItalic
            });
            this.m_Db.Save ();
            return Json ("Success");
         }
         Response.StatusCode = 500;
         return Json ("Data is not valid.");
      }

      /// <summary>
      /// Creates a header for a chapter.
      /// </summary>
      /// <param name="viewModel">View model with header information.</param>
      /// <returns>Result</returns>
      [HttpPost]
      [ValidateAntiForgeryToken]
      [Authorize (Roles = "Editor")]
      public ActionResult CreateForChapter (HeaderFooterViewModel viewModel)
      {
         if (ModelState.IsValid)
         {
            var chapter = this.m_Db.Chapters.Get (viewModel.ItemId);
            var header = this.GetHeader (viewModel.Text);
            chapter.Headers.Add (new ChapterHeader
            {
               Chapter = chapter,
               Header = header,
               Justify = viewModel.Justify,
               IsBold = viewModel.IsBold,
               IsItalic = viewModel.IsItalic
            });
            this.m_Db.Save ();
            return Json ("Success");
         }
         Response.StatusCode = 500;
         return Json ("Data is not valid.");
      }

      /// <summary>
      /// Gets the header in the database with the given text.
      /// </summary>
      /// <param name="text">The text of the header to get.</param>
      /// <returns>The requested header, a new header if does not exist.</returns>
      private Header GetHeader (string text)
      {
         var headers = this.m_Db.Headers.Get (h => h.Text == text);
         var header = headers.FirstOrDefault ();
         if (header == null)
         {
            header = new Header { Text = text };
            this.m_Db.Headers.Insert (header);
            this.m_Db.Save ();
         }
         return header;
      }
   }
}
