using System.Linq;
using System.Web.Mvc;
using SeekDeepWithin.DataAccess;
using SeekDeepWithin.Domain;
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
      /// Gets the view to create a new header for the given entry.
      /// </summary>
      /// <param name="itemId">Id of entry to create header for.</param>
      /// <param name="type">The type the footer is for.</param>
      /// <param name="index">The index for the footer.</param>
      /// <returns>The partial header create view.</returns>
      public ActionResult Create (int itemId, string type, int index)
      {
         return PartialView (new HeaderFooterViewModel { ItemId = itemId, Index = index, Type = type });
      }

      /// <summary>
      /// Creates a footer for a passage.
      /// </summary>
      /// <param name="viewModel">View model with footer information.</param>
      /// <returns>Result</returns>
      [HttpPost]
      [ValidateAntiForgeryToken]
      [Authorize (Roles = "Editor")]
      public ActionResult CreateForPassage (HeaderFooterViewModel viewModel)
      {
         if (ModelState.IsValid)
         {
            var entry = this.m_Db.PassageEntries.Get (viewModel.ItemId);
            var footer = this.GetFooter (viewModel.Text);
            entry.Footers.Add (new PassageFooter { Passage = entry, Footer = footer, Index = viewModel.Index, Justify = viewModel.Justify });
            this.m_Db.Save ();
            return Json ("Success");
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
      public ActionResult CreateForChapter (HeaderFooterViewModel viewModel)
      {
         if (ModelState.IsValid)
         {
            var chapter = this.m_Db.Chapters.Get (viewModel.ItemId);
            var footer = this.GetFooter (viewModel.Text);
            chapter.Footers.Add (new ChapterFooter { Chapter = chapter, Footer = footer, Justify = viewModel.Justify });
            this.m_Db.Save ();
            return Json ("Success");
         }
         Response.StatusCode = 500;
         return Json ("Data is not valid.");
      }

      /// <summary>
      /// Gets the footer in the database with the given text.
      /// </summary>
      /// <param name="text">The text of the footer to get.</param>
      /// <returns>The requested footer, a new footer if does not exist.</returns>
      private Footer GetFooter (string text)
      {
         var footers = this.m_Db.Footers.Get (f => f.Text == text);
         var footer = footers.FirstOrDefault ();
         if (footer == null)
         {
            footer = new Footer { Text = text };
            this.m_Db.Footers.Insert (footer);
            this.m_Db.Save ();
         }
         return footer;
      }
   }
}
