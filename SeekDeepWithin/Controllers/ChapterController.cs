using System.Linq;
using System.Web.Mvc;
using SeekDeepWithin.DataAccess;
using SeekDeepWithin.Domain;
using SeekDeepWithin.Models;

namespace SeekDeepWithin.Controllers
{
   public class ChapterController : Controller
   {
      private readonly ISdwDatabase m_Db;

      /// <summary>
      /// Initializes a new chapter controller.
      /// </summary>
      public ChapterController ()
      {
         this.m_Db = new SdwDatabase ();
      }

      /// <summary>
      /// Initializes a new chapter controller with the given db info.
      /// </summary>
      /// <param name="db">Database object.</param>
      public ChapterController (ISdwDatabase db)
      {
         this.m_Db = db;
      }

      /// <summary>
      /// Gets the read chapter Page.
      /// </summary>
      /// <param name="id">The id of the chapter to read.</param>
      /// <returns>The read page.</returns>
      public ActionResult Read (int id)
      {
         var chapter = this.m_Db.Chapters.Get (id);
         return View (GetViewModel (chapter, true));
      }

      /// <summary>
      /// Gets the edit chapter Page.
      /// </summary>
      /// <param name="id">The id of the chapter to edit.</param>
      /// <returns>The edit page.</returns>
      [Authorize (Roles = "Editor")]
      public ActionResult Edit (int id)
      {
         if (Request.UrlReferrer != null) TempData["RefUrl"] = Request.UrlReferrer.ToString ();
         var chapter = this.m_Db.Chapters.Get (id);
         return View (GetViewModel (chapter));
      }

      /// <summary>
      /// Performs an edit for the given model.
      /// </summary>
      /// <param name="viewModel">The view model with edits.</param>
      /// <returns>The edit page.</returns>
      [HttpPost]
      [ValidateAntiForgeryToken]
      [Authorize (Roles = "Editor")]
      public ActionResult Edit (ChapterViewModel viewModel)
      {
         if (ModelState.IsValid)
         {
            var chapter = this.m_Db.Chapters.Get (viewModel.Id);
            this.m_Db.SetValues (chapter, viewModel);
            this.m_Db.Save ();
            return RedirectToAction ("Read", new { id = viewModel.Id });
         }
         return View (viewModel);
      }

      /// <summary>
      /// Gets the add passage for chapter Page.
      /// </summary>
      /// <param name="id">The id of the chapter to add passages for.</param>
      /// <returns>The add page.</returns>
      [Authorize (Roles = "Creator")]
      public ActionResult Add (int id)
      {
         if (Request.UrlReferrer != null) TempData["RefUrl"] = Request.UrlReferrer.ToString ();
         var viewModel = new AddPassageViewModel {ChapterId = id};
         var chapter = this.m_Db.Chapters.Get (id);
         if (chapter.Passages.Count > 0)
         {
            viewModel.Order = chapter.Passages.Max (p => p.Order) + 1;
            viewModel.Number = chapter.Passages.Max (p => p.Number) + 1;
         }
         else
         {
            viewModel.Order = 1;
            viewModel.Number = 1;
         }
         return View (viewModel);
      }

      /// <summary>
      /// Gets the create new chapter view.
      /// </summary>
      /// <param name="versionId">Version id to create the chapter for.</param>
      /// <param name="subBookId">The id of the of the sub book to select.</param>
      /// <returns>The create chapter view.</returns>
      [Authorize (Roles = "Creator")]
      public ActionResult Create (int versionId, int? subBookId)
      {
         var subBooks = this.m_Db.SubBooks.Get (sb => sb.Version.Id == versionId).ToList();
         if (subBooks.Count == 0)
         {
            TempData ["ErrorMessage"] = "You must have at least one sub book to add a chapter.";
            return RedirectToAction ("Edit", "Version", new {id = versionId});
         }
         ViewBag.VersionId = versionId;
         ViewBag.SubBooks = subBookId.HasValue ? new SelectList (subBooks, "Id", "Name", subBookId.Value)
            : new SelectList (subBooks, "Id", "Name");
         if (Request.UrlReferrer != null) TempData["RefUrl"] = Request.UrlReferrer.ToString ();
         return View ();
      }

      /// <summary>
      /// Creates a new chapter.
      /// </summary>
      /// <returns>The results.</returns>
      [HttpPost]
      [ValidateAntiForgeryToken]
      [Authorize (Roles = "Creator")]
      public ActionResult Create (int subBookId, string name, int order, int versionId)
      {
         var subBook = this.m_Db.SubBooks.Get (subBookId);
         var chapter = new Chapter
         {
            Name = name,
            Order = order,
            SubBook = subBook
         };
         subBook.Chapters.Add (chapter);
         this.m_Db.Chapters.Insert (chapter);
         this.m_Db.Save ();
         return RedirectToAction ("Read", "Chapter", new {id = chapter.Id});
      }

      /// <summary>
      /// Converts the given chapter to a view model.
      /// </summary>
      /// <param name="chapter">Chapter to convert to view model.</param>
      /// <param name="deepCopy">True to perform a deep copy, other wise false.</param>
      /// <param name="subBook">The sub book for the chapter.</param>
      /// <returns>The view model representation of the chapter.</returns>
      public static ChapterViewModel GetViewModel (Chapter chapter, bool deepCopy = false, SubBookViewModel subBook = null)
      {
         var viewModel = new ChapterViewModel
         {
            Id = chapter.Id,
            Name = chapter.Name,
            Order = chapter.Order,
            SubBookId = chapter.SubBook.Id,
            DefaultToParagraph = chapter.DefaultToParagraph,
            SubBook = subBook
         };
         if (subBook == null)
            viewModel.SubBook = SubBookController.GetViewModel (chapter.SubBook, deepCopy ? viewModel : null);
         foreach (var header in chapter.Headers)
         {
            viewModel.Headers.Add(new HeaderFooterViewModel
            {
               IsBold = header.IsBold,
               IsItalic = header.IsItalic,
               Justify = header.Justify,
               Text = header.Header.Text
            });
         }

         foreach (var footer in chapter.Footers)
         {
            viewModel.Footers.Add (new HeaderFooterViewModel
            {
               IsBold = footer.IsBold,
               IsItalic = footer.IsItalic,
               Justify = footer.Justify,
               Text = footer.Footer.Text
            });
         }

         foreach (var entry in chapter.Passages.OrderBy (pe => pe.Order))
            viewModel.Passages.Add (PassageController.GetViewModel (entry.Passage, entry));
         return viewModel;
      }
   }
}
