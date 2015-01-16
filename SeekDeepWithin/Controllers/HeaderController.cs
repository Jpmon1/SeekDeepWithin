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
         return PartialView (new HeaderFooterViewModel { ItemId = itemId, For = type });
      }

      /// <summary>
      /// Creates a header for a passage.
      /// </summary>
      /// <param name="viewModel">View model with header information.</param>
      /// <returns>Result</returns>
      [HttpPost]
      [ValidateAntiForgeryToken]
      [Authorize (Roles = "Editor")]
      public ActionResult CreatePassage (HeaderFooterViewModel viewModel)
      {
         if (ModelState.IsValid)
         {
            var entry = this.m_Db.PassageEntries.Get (viewModel.ItemId);
            var pHeader = new PassageHeader
            {
               Passage = entry,
               Text = viewModel.Text,
               Justify = viewModel.Justify,
               IsBold = viewModel.IsBold,
               IsItalic = viewModel.IsItalic
            };
            entry.Headers.Add (pHeader);
            this.m_Db.Save ();
            return Json (new { message = "success", type = "passage", id = pHeader.Id, text = viewModel.Text });
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
      public ActionResult CreateChapter (HeaderFooterViewModel viewModel)
      {
         if (ModelState.IsValid)
         {
            var chapter = this.m_Db.Chapters.Get (viewModel.ItemId);
            var chHeader = new ChapterHeader
            {
               Chapter = chapter,
               Text = viewModel.Text,
               Justify = viewModel.Justify,
               IsBold = viewModel.IsBold,
               IsItalic = viewModel.IsItalic
            };
            chapter.Headers.Add (chHeader);
            this.m_Db.Save ();
            return PartialView ("EditorTemplates/ChapterHeaderViewModel", new ChapterHeaderViewModel (chHeader) { ItemId = viewModel.ItemId });
         }
         Response.StatusCode = 500;
         return Json ("Data is not valid.");
      }

      /// <summary>
      /// Gets the view to create a new header for the given entry.
      /// </summary>
      /// <param name="id">The id of the header to edit.</param>
      /// <param name="type">The type of header to create.</param>
      /// <returns>The partial header edit view.</returns>
      public ActionResult Edit (int id, string type)
      {
         IHeader header;
         if (type == "chapter")
            header = this.m_Db.ChapterHeaders.Get (id);
         else
            header = this.m_Db.PassageHeaders.Get (id);
         return PartialView (new HeaderFooterViewModel (header) { For = type });
      }

      /// <summary>
      /// Edits a header for a passage.
      /// </summary>
      /// <param name="viewModel">View model with header information.</param>
      /// <returns>Result</returns>
      [HttpPost]
      [ValidateAntiForgeryToken]
      [Authorize (Roles = "Editor")]
      public ActionResult Edit (HeaderFooterViewModel viewModel)
      {
         if (ModelState.IsValid)
         {
            IHeader header;
            if (viewModel.For == "chapter")
               header = this.m_Db.ChapterHeaders.Get (viewModel.Id);
            else
               header = this.m_Db.PassageHeaders.Get (viewModel.Id);
            this.m_Db.SetValues (header, viewModel);
            this.m_Db.Save ();
            return Json (new { message = "success", type = viewModel.For, id = header.Id, text = viewModel.Text });
         }
         Response.StatusCode = 500;
         return Json ("Data is not valid.");
      }

      /// <summary>
      /// Deletes a header from a chapter.
      /// </summary>
      /// <param name="id">The id of the header to delete.</param>
      /// <param name="itemId">Id of the header's parent to delete from.</param>
      /// <param name="type">The type the header is for.</param>
      /// <returns>Result</returns>
      [HttpPost]
      [ValidateAntiForgeryToken]
      [Authorize (Roles = "Deleter")]
      public ActionResult Delete (int id, int itemId, string type)
      {
         if (type == "chapter")
         {
            var chapter = this.m_Db.Chapters.Get (itemId);
            var header = chapter.Headers.FirstOrDefault (f => f.Id == id);
            chapter.Headers.Remove (header);
            this.m_Db.ChapterHeaders.Delete (id);
            this.m_Db.Save ();
            return Json ("Success");
         }
         if (type == "passage")
         {
            var passage = this.m_Db.PassageEntries.Get (itemId);
            var header = passage.Headers.FirstOrDefault (f => f.Id == id);
            passage.Headers.Remove (header);
            this.m_Db.PassageHeaders.Delete (id);
            this.m_Db.Save ();
            return Json ("Success");
         }
         Response.StatusCode = 500;
         return Json ("Invalid Data.");
      }
   }
}
