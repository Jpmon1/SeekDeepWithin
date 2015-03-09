using System.Linq;
using System.Web.Mvc;
using SeekDeepWithin.DataAccess;
using SeekDeepWithin.Pocos;
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
         var chapter = this.m_Db.SubBookChapters.Get (id);
         return View (new ChapterViewModel (chapter));
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
         var chapter = this.m_Db.SubBookChapters.Get (id);
         return View (new ChapterViewModel (chapter));
      }

      /// <summary>
      /// Gets the add passage view for a chapter.
      /// </summary>
      /// <param name="id">The id of the chapter to add passages for.</param>
      /// <returns>The add passge view.</returns>
      [Authorize (Roles = "Creator")]
      public ActionResult Add (int id)
      {
         if (Request.UrlReferrer != null) TempData["RefUrl"] = Request.UrlReferrer.ToString ();
         var viewModel = new AddItemViewModel { ParentId = id, ItemType = ItemType.Passage };
         var chapter = this.m_Db.SubBookChapters.Get (id);
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
      /// Gets the edit page for the given chapter.
      /// </summary>
      /// <param name="id">The id of the chapter.</param>
      /// <param name="hide">True to hide chapter, otherwise false.</param>
      /// <returns></returns>
      [HttpPost]
      [ValidateAntiForgeryToken]
      [Authorize (Roles = "Editor")]
      public ActionResult Hide (int id, bool hide)
      {
         var chapter = this.m_Db.SubBookChapters.Get (id);
         if (chapter == null)
         {
            Response.StatusCode = 500;
            return Json ("Unknown Chapter");
         }
         chapter.Hide = hide;
         this.m_Db.Save ();
         return Json ("success");
      }

      /// <summary>
      /// Creates a new chapter.
      /// </summary>
      /// <returns>The results.</returns>
      [HttpPost]
      [ValidateAntiForgeryToken]
      [Authorize (Roles = "Editor")]
      public ActionResult ReadStyle (int id, bool paragraph)
      {
         var chapter = this.m_Db.SubBookChapters.Get (id);
         if (chapter == null)
         {
            Response.StatusCode = 500;
            return Json ("Unknown chapter");
         }
         chapter.DefaultToParagraph = paragraph;
         this.m_Db.Save();
         return Json ("success");
      }

      /// <summary>
      /// Creates a new chapter.
      /// </summary>
      /// <returns>The results.</returns>
      [HttpPost]
      [ValidateAntiForgeryToken]
      [Authorize (Roles = "Creator")]
      public ActionResult Create (int subBookId, string name)
      {
         var subBook = this.m_Db.VersionSubBooks.Get (subBookId);
         var maxOrder = (subBook.Chapters.Count > 0 ? subBook.Chapters.Max (c => c.Order) : 0) + 1;
         var chapter = DbHelper.GetChapter (this.m_Db, name);
         var sChapter = new SubBookChapter {Chapter = chapter, Order = maxOrder, SubBook = subBook};
         subBook.Chapters.Add (sChapter);
         this.m_Db.Save ();

         if (subBook.Version.DefaultReadChapter == 0)
         {
            subBook.Version.DefaultReadChapter = sChapter.Id;
            this.m_Db.Save ();
         }

         return Json (new {id=sChapter.Id, name, itemId=chapter.Id});
      }

      /// <summary>
      /// Renames a chapter.
      /// </summary>
      /// <returns>The results.</returns>
      [HttpPost]
      [ValidateAntiForgeryToken]
      [Authorize (Roles = "Creator")]
      public ActionResult Rename (int id, string name)
      {
         var subBook = this.m_Db.SubBookChapters.Get (id);
         var chapter = subBook.Chapter;
         chapter.Name = name;
         this.m_Db.Save();
         return Json ("success");
      }

      /// <summary>
      /// Gets auto complete items for the given item.
      /// </summary>
      /// <param name="name">Name to get auto complete items for.</param>
      /// <param name="subBookId">The id of the sub book to look up chapters for.</param>
      /// <returns>The list of possible items.</returns>
      public ActionResult AutoComplete (string name, int subBookId)
      {
         var result = new
         {
            suggestions = this.m_Db.SubBookChapters.Get (sc => sc.SubBook.Id == subBookId && sc.Chapter.Name.Contains (name))
                                                 .Select (c => new { value = c.Chapter.Name, data = c.Id })
         };
         return Json (result, JsonRequestBehavior.AllowGet);
      }
   }
}
