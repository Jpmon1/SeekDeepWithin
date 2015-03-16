using System.Linq;
using System.Web.Mvc;
using SeekDeepWithin.DataAccess;
using SeekDeepWithin.Pocos;
using SeekDeepWithin.Models;

namespace SeekDeepWithin.Controllers
{
   /// <summary>
   /// Controller used for footers.
   /// </summary>
   public class FooterController : Controller
   {
      private readonly ISdwDatabase m_Db;

      /// <summary>
      /// Initializes a new controller.
      /// </summary>
      public FooterController ()
      {
         this.m_Db = new SdwDatabase ();
      }

      /// <summary>
      /// Initializes a new controller with the given db info.
      /// </summary>
      /// <param name="db">Database object.</param>
      public FooterController (ISdwDatabase db)
      {
         this.m_Db = db;
      }

      /// <summary>
      /// Gets the view to create a new footer for the given entry.
      /// </summary>
      /// <param name="itemId">Id of entry to create footer for.</param>
      /// <param name="type">The type the footer is for.</param>
      /// <param name="index">The index for the footer.</param>
      /// <returns>The partial footer create view.</returns>
      public ActionResult Create (int itemId, string type, int index)
      {
         return PartialView (new HeaderFooterViewModel { ItemId = itemId, Index = index, For = type, Type = "footer"});
      }

      /// <summary>
      /// Creates a footer for a passage.
      /// </summary>
      /// <param name="viewModel">View model with footer information.</param>
      /// <returns>Result</returns>
      [HttpPost]
      [ValidateAntiForgeryToken]
      [Authorize (Roles = "Editor")]
      public ActionResult CreatePassage (HeaderFooterViewModel viewModel)
      {
         if (ModelState.IsValid)
         {
            var entry = this.m_Db.PassageEntries.Get (viewModel.ItemId);
            var pFooter = new PassageFooter
            {
               Passage = entry,
               Text = viewModel.Text,
               Index = viewModel.Index,
               Justify = viewModel.Justify,
               IsBold = viewModel.IsBold,
               IsItalic = viewModel.IsItalic
            };
            entry.Footers.Add (pFooter);
            this.m_Db.Save ();
            return Json (new { message = "success", type = "passage", id = pFooter.Id, text = viewModel.Text });
         }
         Response.StatusCode = 500;
         return Json ("Data is not valid.");
      }

      /// <summary>
      /// Creates a footer for a glossary entry.
      /// </summary>
      /// <param name="viewModel">View model with footer information.</param>
      /// <returns>Result</returns>
      [HttpPost]
      [ValidateAntiForgeryToken]
      [Authorize (Roles = "Editor")]
      public ActionResult CreateEntry (HeaderFooterViewModel viewModel)
      {
         if (ModelState.IsValid)
         {
            var entry = this.m_Db.GlossaryEntries.Get (viewModel.ItemId);
            var entryFooter = new GlossaryEntryFooter
            {
               Entry = entry,
               Text = viewModel.Text,
               Index = viewModel.Index,
               Justify = viewModel.Justify,
               IsBold = viewModel.IsBold,
               IsItalic = viewModel.IsItalic
            };
            entry.Footers.Add (entryFooter);
            this.m_Db.Save ();
            return Json (new { message = "success", type = "passage", id = entryFooter.Id, text = viewModel.Text });
         }
         Response.StatusCode = 500;
         return Json ("Data is not valid.");
      }

      /// <summary>
      /// Creates a footer for a chapter.
      /// </summary>
      /// <param name="viewModel">View model with footer information.</param>
      /// <returns>Result</returns>
      [HttpPost]
      [ValidateAntiForgeryToken]
      [Authorize (Roles = "Editor")]
      public ActionResult CreateChapter (HeaderFooterViewModel viewModel)
      {
         if (ModelState.IsValid)
         {
            var chapter = this.m_Db.SubBookChapters.Get (viewModel.ItemId);
            var chFooter = new ChapterFooter
            {
               Chapter = chapter,
               Text = viewModel.Text,
               Justify = viewModel.Justify,
               IsBold = viewModel.IsBold,
               IsItalic = viewModel.IsItalic
            };
            chapter.Footers.Add (chFooter);
            this.m_Db.Save ();
            return PartialView ("EditorTemplates/ChapterFooterViewModel", new ChapterFooterViewModel (chFooter) {ItemId = viewModel.ItemId});
         }
         Response.StatusCode = 500;
         return Json ("Invalid data.");
      }

      /// <summary>
      /// Gets the view to create a new footer for the given entry.
      /// </summary>
      /// <param name="id">Id of the footer to edit.</param>
      /// <param name="type">The type the footer is for.</param>
      /// <returns>The partial footer edit view.</returns>
      public ActionResult Edit (int id, string type)
      {
         IFooter footer;
         if (type == "chapter")
            footer = this.m_Db.ChapterFooters.Get (id);
         else
            footer = this.m_Db.PassageFooters.Get (id);
         return PartialView (new HeaderFooterViewModel (footer) { For = type, });
      }

      /// <summary>
      /// Edits a footer for a passage.
      /// </summary>
      /// <param name="viewModel">View model with footer information.</param>
      /// <returns>Result</returns>
      [HttpPost]
      [ValidateAntiForgeryToken]
      [Authorize (Roles = "Editor")]
      public ActionResult Edit (HeaderFooterViewModel viewModel)
      {
         if (ModelState.IsValid)
         {
            IFooter footer;
            if (viewModel.For == "chapter")
               footer = this.m_Db.ChapterFooters.Get (viewModel.Id);
            else
               footer = this.m_Db.PassageFooters.Get (viewModel.Id);
            this.m_Db.SetValues (footer, viewModel);
            this.m_Db.Save ();
            return Json (new { message = "success", type = viewModel.For, id = footer.Id, text = viewModel.Text });
         }
         Response.StatusCode = 500;
         return Json ("Data is not valid.");
      }

      /// <summary>
      /// Deletes a footer from a chapter.
      /// </summary>
      /// <param name="id">The id of the footer to delete.</param>
      /// <param name="itemId">Id of the footer's parent to delete from.</param>
      /// <param name="type">The type the footer is for.</param>
      /// <returns>Result</returns>
      [HttpPost]
      [ValidateAntiForgeryToken]
      [Authorize (Roles = "Deleter")]
      public ActionResult Delete (int id, int itemId, string type)
      {
         if (type == "chapter")
         {
            var chapter = this.m_Db.SubBookChapters.Get (itemId);
            var footer = chapter.Footers.FirstOrDefault (f => f.Id == id);
            chapter.Footers.Remove (footer);
            this.m_Db.ChapterFooters.Delete (id);
            this.m_Db.Save ();
            return Json ("Success");
         }
         if (type == "passage")
         {
            var passage = this.m_Db.PassageEntries.Get (itemId);
            var footer = passage.Footers.FirstOrDefault (f => f.Id == id);
            passage.Footers.Remove (footer);
            this.m_Db.PassageFooters.Delete (id);
            this.m_Db.Save ();
            return Json ("Success");
         }
         Response.StatusCode = 500;
         return Json ("Invalid Data.");
      }
   }
}
