using System.Linq;
using System.Web.Mvc;
using SeekDeepWithin.DataAccess;
using SeekDeepWithin.Pocos;
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
      /// <param name="id">Id of the header to edit.</param>
      /// <param name="type">The type the header is for.</param>
      /// <returns>The partial header edit view.</returns>
      public ActionResult Edit (int id, string type)
      {
         var viewModel = new HeaderFooterEditViewModel
         {
            ItemType = type,
            ItemId = id,
            ItemText = string.Empty,
            NextEntryId = -1,
            PreviousEntryId = -1
         };
         if (type.ToLower () == "passage")
         {
            var passage = this.m_Db.PassageEntries.Get (id);
            viewModel.ParentId = passage.Chapter.Id;
            viewModel.ItemText = passage.Passage.Text;
            foreach (var header in passage.Headers)
               viewModel.Items.Add (new HeaderFooterViewModel (header));
            var prev = passage.Chapter.Passages.FirstOrDefault (p => p.Order == (passage.Order - 1));
            if (prev != null) viewModel.PreviousEntryId = prev.Id;
            var next = passage.Chapter.Passages.FirstOrDefault (p => p.Order == (passage.Order + 1));
            if (next != null) viewModel.NextEntryId = next.Id;
         }
         else if (type.ToLower () == "entry")
         {
            var entry = this.m_Db.GlossaryEntries.Get (id);
            viewModel.ParentId = entry.Item.Id;
            viewModel.ItemText = entry.Text;
            foreach (var header in entry.Headers)
               viewModel.Items.Add (new HeaderFooterViewModel (header));
            var prev = entry.Item.Entries.FirstOrDefault (e => e.Order == (entry.Order - 1));
            if (prev != null) viewModel.PreviousEntryId = prev.Id;
            var next = entry.Item.Entries.FirstOrDefault (e => e.Order == (entry.Order + 1));
            if (next != null) viewModel.NextEntryId = next.Id;
         }
         else if (type.ToLower () == "chapter")
         {
            var chapter = this.m_Db.SubBookChapters.Get (id);
            viewModel.ParentId = id;
            foreach (var header in chapter.Headers)
               viewModel.Items.Add (new HeaderFooterViewModel (header));
         }
         return View (viewModel);
      }

      /// <summary>
      /// Creates a header.
      /// </summary>
      /// <param name="viewModel">The veiw model with data.</param>
      /// <returns>Result</returns>
      [HttpPost]
      [ValidateAntiForgeryToken]
      [Authorize (Roles = "Editor")]
      public ActionResult Create (HeaderFooterViewModel viewModel)
      {
         IHeader header = null;
         if (viewModel.ItemType.ToLower () == "chapter")
         {
            var chapter = this.m_Db.SubBookChapters.Get (viewModel.ItemId);
            header = new ChapterHeader();
            chapter.Headers.Add((ChapterHeader)header);
         }
         if (viewModel.ItemType.ToLower () == "passage")
         {
            var passage = this.m_Db.PassageEntries.Get (viewModel.ItemId);
            header = new PassageHeader();
            passage.Headers.Add ((PassageHeader)header);
         }
         if (viewModel.ItemType.ToLower () == "entry")
         {
            var entry = this.m_Db.GlossaryEntries.Get (viewModel.ItemId);
            header = new GlossaryEntryHeader();
            entry.Headers.Add ((GlossaryEntryHeader)header);
         }
         if (header != null)
         {
            header.Text = viewModel.Text;
            header.IsBold = viewModel.IsBold;
            header.Justify = viewModel.Justify;
            header.IsItalic = viewModel.IsItalic;
            this.m_Db.Save ();
            return Json (new { id = header.Id });
         }
         Response.StatusCode = 500;
         return Json ("Invalid data.");
      }

      /// <summary>
      /// Updates a header.
      /// </summary>
      /// <param name="viewModel">The veiw model with data.</param>
      /// <returns>Result</returns>
      [HttpPost]
      [ValidateAntiForgeryToken]
      [Authorize (Roles = "Editor")]
      public ActionResult Update (HeaderFooterViewModel viewModel)
      {
         IHeader header = null;
         if (viewModel.ItemType.ToLower () == "chapter")
         {
            var chapter = this.m_Db.SubBookChapters.Get (viewModel.ItemId);
            header = chapter.Headers.FirstOrDefault (f => f.Id == viewModel.Id);
         }
         if (viewModel.ItemType.ToLower () == "passage")
         {
            var passage = this.m_Db.PassageEntries.Get (viewModel.ItemId);
            header = passage.Headers.FirstOrDefault (f => f.Id == viewModel.Id);
         }
         if (viewModel.ItemType.ToLower () == "entry")
         {
            var entry = this.m_Db.GlossaryEntries.Get (viewModel.ItemId);
            header = entry.Headers.FirstOrDefault (f => f.Id == viewModel.Id);
         }
         if (header != null)
         {
            header.Text = viewModel.Text;
            header.IsBold = viewModel.IsBold;
            header.Justify = viewModel.Justify;
            header.IsItalic = viewModel.IsItalic;
         }
         this.m_Db.Save ();
         return Json ("Success");
      }

      /// <summary>
      /// Deletes a header.
      /// </summary>
      /// <param name="id">The id of the header to delete.</param>
      /// <param name="itemId">Id of the header's parent to delete from.</param>
      /// <param name="itemType">The type the header is for.</param>
      /// <returns>Result</returns>
      [HttpPost]
      [ValidateAntiForgeryToken]
      [Authorize (Roles = "Editor")]
      public ActionResult Delete (int id, int itemId, string itemType)
      {
         if (itemType.ToLower () == "chapter")
         {
            var chapter = this.m_Db.SubBookChapters.Get (itemId);
            var header = chapter.Headers.FirstOrDefault (f => f.Id == id);
            chapter.Headers.Remove (header);
            this.m_Db.Save ();
            return Json ("Success");
         }
         if (itemType.ToLower () == "passage")
         {
            var passage = this.m_Db.PassageEntries.Get (itemId);
            var header = passage.Headers.FirstOrDefault (f => f.Id == id);
            passage.Headers.Remove (header);
            this.m_Db.Save ();
            return Json ("Success");
         }
         if (itemType.ToLower () == "entry")
         {
            var entry = this.m_Db.GlossaryEntries.Get (itemId);
            var header = entry.Headers.FirstOrDefault (f => f.Id == id);
            entry.Headers.Remove (header);
            this.m_Db.Save ();
            return Json ("Success");
         }
         Response.StatusCode = 500;
         return Json ("Invalid Data.");
      }

      /// <summary>
      /// Gets a header.
      /// </summary>
      /// <param name="id">The id of the header to get.</param>
      /// <param name="itemId">Id of the header's parent.</param>
      /// <param name="itemType">The type the header is for.</param>
      /// <returns>Result</returns>
      public ActionResult Get (int id, int itemId, string itemType)
      {
         if (itemType.ToLower () == "chapter")
         {
            var chapter = this.m_Db.SubBookChapters.Get (itemId);
            var header = chapter.Headers.FirstOrDefault (f => f.Id == id);
            if (header != null)
               return Json (new
               {
                  text = header.Text, justify = header.Justify, isBold = header.IsBold, isItalic = header.IsItalic
               }, JsonRequestBehavior.AllowGet);
         }
         if (itemType.ToLower () == "passage")
         {
            var passage = this.m_Db.PassageEntries.Get (itemId);
            var header = passage.Headers.FirstOrDefault (f => f.Id == id);
            if (header != null)
               return Json (new
               {
                  text = header.Text, justify = header.Justify, isBold = header.IsBold, isItalic = header.IsItalic
               }, JsonRequestBehavior.AllowGet);
         }
         if (itemType.ToLower () == "entry")
         {
            var entry = this.m_Db.GlossaryEntries.Get (itemId);
            var header = entry.Headers.FirstOrDefault (f => f.Id == id);
            if (header != null)
               return Json (new
               {
                  text = header.Text, justify = header.Justify, isBold = header.IsBold, isItalic = header.IsItalic
               }, JsonRequestBehavior.AllowGet);
         }
         Response.StatusCode = 500;
         return Json ("Invalid Data.", JsonRequestBehavior.AllowGet);
      }
   }
}
