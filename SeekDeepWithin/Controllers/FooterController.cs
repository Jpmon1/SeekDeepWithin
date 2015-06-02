using System.Linq;
using System.Web.Mvc;
using SeekDeepWithin.DataAccess;
using SeekDeepWithin.Pocos;
using SeekDeepWithin.Models;
using SeekDeepWithin.SdwSearch;

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
      /// <param name="id">Id of the footer to edit.</param>
      /// <param name="type">The type the footer is for.</param>
      /// <returns>The partial footer edit view.</returns>
      public ActionResult Edit (int id, string type)
      {
         var viewModel = new HeaderFooterEditViewModel
         {
            ItemType = type,
            ItemId = id,
            NextEntryId = -1,
            PreviousEntryId = -1
         };
         if (type.ToLower () == "passage")
         {
            var passage = this.m_Db.PassageEntries.Get (id);
            var title = passage.Chapter.SubBook.Version.Title + " | ";
            if (!passage.Chapter.SubBook.Hide)
               title += passage.Chapter.SubBook.SubBook.Name + " | ";
            if (!passage.Chapter.Hide)
               title += passage.Chapter.Chapter.Name + ":";
            title += passage.Number;
            viewModel.Title = title;
            viewModel.ItemText = passage.Passage.Text;
            viewModel.ParentId = passage.Chapter.Id;
            foreach (var footer in passage.Footers)
               viewModel.Items.Add (new HeaderFooterViewModel (footer));
            var prev = passage.Chapter.Passages.FirstOrDefault (p => p.Order == (passage.Order - 1));
            if (prev != null) viewModel.PreviousEntryId = prev.Id;
            var next = passage.Chapter.Passages.FirstOrDefault (p => p.Order == (passage.Order + 1));
            if (next != null) viewModel.NextEntryId = next.Id;
         }
         else if (type.ToLower () == "entry")
         {
            var entry = this.m_Db.GlossaryEntries.Get (id);
            viewModel.Title = entry.Item.Term.Name;
            viewModel.ParentId = entry.Item.Id;
            viewModel.ItemText = entry.Text;
            foreach (var footer in entry.Footers)
               viewModel.Items.Add (new HeaderFooterViewModel (footer));
            var prev = entry.Item.Entries.FirstOrDefault (e => e.Order == (entry.Order - 1));
            if (prev != null) viewModel.PreviousEntryId = prev.Id;
            var next = entry.Item.Entries.FirstOrDefault (e => e.Order == (entry.Order + 1));
            if (next != null) viewModel.NextEntryId = next.Id;
         }
         else if (type.ToLower () == "chapter")
         {
            var chapter = this.m_Db.SubBookChapters.Get (id);
            var title = chapter.SubBook.Version.Title + " | ";
            if (!chapter.SubBook.Hide)
               title += chapter.SubBook.SubBook.Name + " | ";
            if (!chapter.Hide)
               title += chapter.Chapter.Name;
            viewModel.Title = title;
            viewModel.ParentId = id;
            foreach (var footer in chapter.Footers)
               viewModel.Items.Add(new HeaderFooterViewModel (footer));
         }
         return View (viewModel);
      }

      /// <summary>
      /// Creates a footer.
      /// </summary>
      /// <param name="viewModel">The veiw model with data.</param>
      /// <returns>Result</returns>
      [HttpPost]
      [ValidateAntiForgeryToken]
      [Authorize (Roles = "Editor")]
      public ActionResult Create (HeaderFooterViewModel viewModel)
      {
         IFooter footer = null;
         if (viewModel.ItemType.ToLower () == "chapter")
         {
            var chapter = this.m_Db.SubBookChapters.Get (viewModel.ItemId);
            footer = new ChapterFooter{Text = viewModel.Text, Index = viewModel.Index};
            chapter.Footers.Add ((ChapterFooter)footer);
            this.m_Db.Save ();
         }
         else if (viewModel.ItemType.ToLower () == "passage")
         {
            var passage = this.m_Db.PassageEntries.Get (viewModel.ItemId);
            footer = new PassageFooter { Text = viewModel.Text, Index = viewModel.Index };
            passage.Footers.Add ((PassageFooter)footer);
            this.m_Db.Save ();
            PassageSearch.AddOrUpdateIndex (passage);
         }
         else if (viewModel.ItemType.ToLower () == "entry")
         {
            var entry = this.m_Db.GlossaryEntries.Get (viewModel.ItemId);
            footer = new GlossaryEntryFooter { Text = viewModel.Text, Index = viewModel.Index };
            entry.Footers.Add ((GlossaryEntryFooter)footer);
            this.m_Db.Save ();
            GlossarySearch.AddOrUpdateIndex (entry);
         }
         if (footer != null)
            return Json (new {id=footer.Id, index=footer.Index});
         return Fail ("Invalid data.");
      }

      /// <summary>
      /// Updates a footer.
      /// </summary>
      /// <param name="viewModel">The veiw model with data.</param>
      /// <returns>Result</returns>
      [HttpPost]
      [ValidateAntiForgeryToken]
      [Authorize (Roles = "Editor")]
      public ActionResult Update (HeaderFooterViewModel viewModel)
      {
         IFooter footer;
         if (viewModel.ItemType.ToLower () == "chapter")
         {
            var chapter = this.m_Db.SubBookChapters.Get (viewModel.ItemId);
            footer = chapter.Footers.FirstOrDefault (f => f.Id == viewModel.Id);
            if (footer == null) return Fail ("Unable to determine footer");
            footer.Text = viewModel.Text;
            this.m_Db.Save ();
         }
         else if (viewModel.ItemType.ToLower () == "passage")
         {
            var passage = this.m_Db.PassageEntries.Get (viewModel.ItemId);
            footer = passage.Footers.FirstOrDefault (f => f.Id == viewModel.Id);
            if (footer == null) return Fail ("Unable to determine footer");
            footer.Text = viewModel.Text;
            this.m_Db.Save ();
         }
         else if (viewModel.ItemType.ToLower () == "entry")
         {
            var entry = this.m_Db.GlossaryEntries.Get (viewModel.ItemId);
            footer = entry.Footers.FirstOrDefault (f => f.Id == viewModel.Id);
            if (footer == null) return Fail ("Unable to determine footer");
            footer.Text = viewModel.Text;
            this.m_Db.Save ();
         }
         return Json ("Success");
      }

      /// <summary>
      /// Returns a failed JSON response with the given message.
      /// </summary>
      /// <param name="message">Message to return.</param>
      /// <returns>JSON response.</returns>
      public ActionResult Fail (string message = "Error")
      {
         Response.StatusCode = 500;
         return Json (message);
      }

      /// <summary>
      /// Deletes a footer.
      /// </summary>
      /// <param name="id">The id of the footer to delete.</param>
      /// <param name="itemId">Id of the footer's parent to delete from.</param>
      /// <param name="itemType">The type the footer is for.</param>
      /// <returns>Result</returns>
      [HttpPost]
      [ValidateAntiForgeryToken]
      [Authorize (Roles = "Editor")]
      public ActionResult Delete (int id, int itemId, string itemType)
      {
         if (itemType.ToLower () == "chapter")
         {
            var chapter = this.m_Db.SubBookChapters.Get (itemId);
            var footer = chapter.Footers.FirstOrDefault (f => f.Id == id);
            chapter.Footers.Remove (footer);
            this.m_Db.Save ();
            return Json ("Success");
         }
         if (itemType.ToLower () == "passage")
         {
            var passage = this.m_Db.PassageEntries.Get (itemId);
            var footer = passage.Footers.FirstOrDefault (f => f.Id == id);
            passage.Footers.Remove (footer);
            this.m_Db.Save ();
            PassageSearch.AddOrUpdateIndex (passage);
            return Json ("Success");
         }
         if (itemType.ToLower () == "entry")
         {
            var entry = this.m_Db.GlossaryEntries.Get (itemId);
            var footer = entry.Footers.FirstOrDefault (f => f.Id == id);
            entry.Footers.Remove (footer);
            this.m_Db.Save ();
            GlossarySearch.AddOrUpdateIndex (entry);
            return Json ("Success");
         }
         Response.StatusCode = 500;
         return Json ("Invalid Data.");
      }

      /// <summary>
      /// Gets a footer.
      /// </summary>
      /// <param name="id">The id of the footer to get.</param>
      /// <param name="itemId">Id of the footer's parent.</param>
      /// <param name="itemType">The type the footer is for.</param>
      /// <returns>Result</returns>
      public ActionResult Get (int id, int itemId, string itemType)
      {
         if (itemType.ToLower () == "chapter")
         {
            var chapter = this.m_Db.SubBookChapters.Get (itemId);
            var footer = chapter.Footers.FirstOrDefault (f => f.Id == id);
            if (footer != null)
               return Json (new {text = footer.Text}, JsonRequestBehavior.AllowGet);
         }
         if (itemType.ToLower () == "passage")
         {
            var passage = this.m_Db.PassageEntries.Get (itemId);
            var footer = passage.Footers.FirstOrDefault (f => f.Id == id);
            if (footer != null)
               return Json (new {text = footer.Text}, JsonRequestBehavior.AllowGet);
         }
         if (itemType.ToLower () == "entry")
         {
            var entry = this.m_Db.GlossaryEntries.Get (itemId);
            var footer = entry.Footers.FirstOrDefault (f => f.Id == id);
            if (footer != null)
               return Json (new {text = footer.Text}, JsonRequestBehavior.AllowGet);
         }
         Response.StatusCode = 500;
         return Json ("Invalid Data.", JsonRequestBehavior.AllowGet);
      }
   }
}
