using System.Collections.ObjectModel;
using System.Linq;
using System.Web.Mvc;
using SeekDeepWithin.DataAccess;
using SeekDeepWithin.Pocos;
using SeekDeepWithin.Models;

namespace SeekDeepWithin.Controllers
{
   public class SubBookController : Controller
   {
      private readonly ISdwDatabase m_Db;

      /// <summary>
      /// Initializes a new book controller.
      /// </summary>
      public SubBookController ()
      {
         this.m_Db = new SdwDatabase ();
      }

      /// <summary>
      /// Initializes a new book controller with the given db info.
      /// </summary>
      /// <param name="db">Database object.</param>
      public SubBookController (ISdwDatabase db)
      {
         this.m_Db = db;
      }

      /// <summary>
      /// Gets the about page.
      /// </summary>
      /// <param name="id">Id of sub book.</param>
      /// <returns>The about view.</returns>
      public ActionResult About (int id)
      {
         var subBook = this.m_Db.VersionSubBooks.Get (id);
         return View (new SubBookViewModel (subBook));
      }

      /// <summary>
      /// Creates a new sub book.
      /// </summary>
      /// <returns>The results.</returns>
      [HttpPost]
      [ValidateAntiForgeryToken]
      [Authorize (Roles = "Creator")]
      public ActionResult Create (SubBookViewModel viewModel)
      {
         var version = this.m_Db.Versions.Get (viewModel.VersionId);
         var subBook = DbHelper.GetSubBook (this.m_Db, viewModel.Name, viewModel.BookId);
         var vSubBook = new VersionSubBook {Version = version, SubBook = subBook};
         version.SubBooks.Add (vSubBook);
         this.m_Db.Save ();
         this.AddAbbreviation (subBook.Id, viewModel.Name);
         return Json (new { id = vSubBook.Id, name = subBook.Name, itemId = subBook.Id });
      }

      /// <summary>
      /// Gets the edit page for the given sub book.
      /// </summary>
      /// <param name="id"></param>
      /// <returns></returns>
      [Authorize (Roles = "Editor")]
      public ActionResult Edit (int id)
      {
         var subBook = this.m_Db.VersionSubBooks.Get (id);
         ViewBag.Writers = new SelectList (this.m_Db.Writers.All (), "Id", "Name");
         return View (new SubBookViewModel (subBook));
      }

      /// <summary>
      /// Renames a sub book.
      /// </summary>
      /// <returns>The results.</returns>
      [HttpPost]
      [ValidateAntiForgeryToken]
      [Authorize (Roles = "Creator")]
      public ActionResult Rename (int id, string name)
      {
         var version = this.m_Db.VersionSubBooks.Get (id);
         var subBook = version.SubBook;
         subBook.Name = name;
         this.m_Db.Save ();
         return Json ("success");
      }

      /// <summary>
      /// Gets the edit page for the given sub book.
      /// </summary>
      /// <param name="id">The id of the sub book.</param>
      /// <param name="hide">True to hide sub book, otherwise false.</param>
      /// <returns></returns>
      [HttpPost]
      [ValidateAntiForgeryToken]
      [Authorize (Roles = "Editor")]
      public ActionResult Hide (int id, bool hide)
      {
         var subBook = this.m_Db.VersionSubBooks.Get (id);
         if (subBook == null)
         {
            Response.StatusCode = 500;
            return Json ("Unknown Sub Book");
         }
         subBook.Hide = hide;
         this.m_Db.Save();
         return Json ("success");
      }

      /// <summary>
      /// Gets the assign writer view.
      /// </summary>
      /// <param name="id">Id of sub book to assign a writer to.</param>
      /// <returns>The assign writer view.</returns>
      [Authorize (Roles = "Editor")]
      public ActionResult AssignWriter (int id)
      {
         ViewBag.SubBookId = id;
         var subBook = this.m_Db.SubBooks.Get (id);
         ViewBag.Title = subBook.Name;
         ViewBag.Writers = new SelectList (this.m_Db.Writers.All (), "Id", "Name");
         if (Request.UrlReferrer != null) TempData["RefUrl"] = Request.UrlReferrer.ToString ();
         return View ();
      }

      /// <summary>
      /// Assigns a writer to a sub book.
      /// </summary>
      /// <returns>The result.</returns>
      [HttpPost]
      [ValidateAntiForgeryToken]
      [Authorize (Roles = "Editor")]
      public ActionResult AssignWriter (int subBookId, int writerId, bool isTranslator)
      {
         var subBook = this.m_Db.SubBooks.Get (subBookId);
         var author = this.m_Db.Writers.Get (writerId);
         var writer = new SubBookWriter
         {
            SubBook = subBook,
            IsTranslator = isTranslator,
            Writer = author
         };
         subBook.Writers.Add (writer);
         this.m_Db.Save ();
         return Json (new { writerId = writer.Id, subBookId, writer = author.Name });
         //return RedirectToAction ("Details", new { id = subBook.Id });
      }

      /// <summary>
      /// Removes a writer from a sub book.
      /// </summary>
      /// <returns>The result.</returns>
      [HttpPost]
      [ValidateAntiForgeryToken]
      [Authorize (Roles = "Editor")]
      public ActionResult RemoveWriter (int subBookId, int writerId)
      {
         var subBook = this.m_Db.SubBooks.Get (subBookId);
         if (subBook == null)
         {
            Response.StatusCode = 500;
            return Json ("Unknown sub book");
         }
         var writer = subBook.Writers.FirstOrDefault (w => w.Id == writerId);
         if (writer == null)
         {
            Response.StatusCode = 500;
            return Json ("Unknown writer");
         }
         subBook.Writers.Remove (writer);
         this.m_Db.Save ();
         return Json ("success");
      }

      /// <summary>
      /// Assigns an abbreviation to a sub book.
      /// </summary>
      /// <returns>The result.</returns>
      [HttpPost]
      [ValidateAntiForgeryToken]
      [Authorize (Roles = "Editor")]
      public ActionResult AddAbbreviation (int subBookId, string text)
      {
         text = text.ToLower ().Replace (" ", "");
         var abbreviation = this.m_Db.Abbreviations.Get (c => c.Text == text).FirstOrDefault ();
         if (abbreviation != null)
         {
            if (abbreviation.SubBook.Id == subBookId)
               return Json ("Abbreviation already exists");
            Response.StatusCode = 500;
            return Json ("Abbreviation is already assigned to a different book - " + abbreviation.SubBook.Name);
         }

         var subBook = this.m_Db.SubBooks.Get (subBookId);
         if (subBook.Abbreviations == null)
            subBook.Abbreviations = new Collection <Abbreviation> ();
         abbreviation = new Abbreviation { SubBook = subBook, Text = text };
         subBook.Abbreviations.Add(abbreviation);
         this.m_Db.Save ();
         return Json (new { id = abbreviation.Id, text });
      }

      /// <summary>
      /// Removes an abbreviation from a sub book.
      /// </summary>
      /// <returns>The result.</returns>
      [HttpPost]
      [ValidateAntiForgeryToken]
      [Authorize (Roles = "Editor")]
      public ActionResult RemoveAbbreviation (int id)
      {
         var abbreviation = this.m_Db.Abbreviations.Get (id);
         if (abbreviation == null)
         {
            Response.StatusCode = 500;
            return Json ("Unknown abbreviation");
         }
         var subBook = abbreviation.SubBook;
         subBook.Abbreviations.Remove (abbreviation);
         this.m_Db.Abbreviations.Delete(abbreviation);
         this.m_Db.Save();
         return Json ("success");
      }

      /// <summary>
      /// Gets auto complete items.
      /// </summary>
      /// <param name="name">Name to get auto complete items for.</param>
      /// <param name="versionId">The id of the version to look up sub books for.</param>
      /// <returns>The list of possible items.</returns>
      public ActionResult AutoComplete (string name, int versionId)
      {
         var result = new
         {
            suggestions = this.m_Db.VersionSubBooks.Get (vs => vs.Version.Id == versionId && vs.SubBook.Name.Contains (name))
                                                 .Select (sb => new { value = sb.SubBook.Name, data = sb.Id })
         };
         return Json (result, JsonRequestBehavior.AllowGet);
      }
   }
}
