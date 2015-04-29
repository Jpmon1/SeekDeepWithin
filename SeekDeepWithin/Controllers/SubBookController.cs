using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
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
         ViewBag.Tags = new SelectList (this.m_Db.Tags.All (q => q.OrderBy (t => t.Name)), "Id", "Name");
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
      /// Adds passages to the sub book, based on the given regex.
      /// </summary>
      /// <returns>The result.</returns>
      [HttpPost]
      [ValidateAntiForgeryToken]
      [Authorize (Roles = "Editor")]
      public ActionResult AddPassages (int id, string text, string regex)
      {
         if (string.IsNullOrWhiteSpace (text))
         {
            Response.StatusCode = 500;
            return Json ("The text cannot be empty.", JsonRequestBehavior.AllowGet);
         }

         regex = HttpUtility.UrlDecode (regex);
         if (string.IsNullOrWhiteSpace (regex))
         {
            Response.StatusCode = 500;
            return Json ("The regex cannot be empty.", JsonRequestBehavior.AllowGet);
         }

         var startOrder = 1;
         var startNumber = 1;
         var chapterId = -1;
         var lastChapter = -1;
         var subBook = this.m_Db.VersionSubBooks.Get (id);
         var passageController = new PassageController (this.m_Db);
         var matches = Regex.Matches (text, regex, RegexOptions.IgnoreCase);
         foreach (Match match in matches)
         {
            var order = match.Groups["order"];
            var number = match.Groups["number"];
            var chapter = match.Groups["chapter"];
            var cInt = Convert.ToInt32 (chapter.Value);
            if (cInt != lastChapter)
            {
               startOrder = 1;
               startNumber = 1;
               lastChapter = cInt;
               var subBookChapter = subBook.Chapters.FirstOrDefault (c => c.Order == cInt);
               if (subBookChapter != null)
                  chapterId = subBookChapter.Id;
               else
               {
                  Response.StatusCode = 500;
                  return Json ("Unable to determine the correct chapter to add passages to: " + chapter.Value);
               }
            }
            passageController.Create (new AddItemViewModel
            {
               ItemType = ItemType.Passage,
               Number = number.Success ? Convert.ToInt32 (number.Value) : startNumber,
               Order = order.Success ? Convert.ToInt32 (order.Value) : startOrder,
               ParentId = chapterId,
               Text = match.Groups["text"].Value.Replace ("\"", "&quot;").Trim ()
            });
            startOrder++;
            startNumber++;
         }
         return Json ("success", JsonRequestBehavior.AllowGet);
      }

      /// <summary>
      /// Adds the given tag to the given sub book.
      /// </summary>
      /// <param name="tagId">Id of tag to add.</param>
      /// <param name="id">Id of sub book to add tag to.</param>
      /// <returns></returns>
      [HttpPost]
      [ValidateAntiForgeryToken]
      [Authorize (Roles = "Editor")]
      public ActionResult AddTag (int tagId, int id)
      {
         var subBook = this.m_Db.SubBooks.Get (id);
         var foundTag = subBook.Tags.FirstOrDefault (bt => bt.Tag.Id == tagId);
         if (foundTag != null)
         {
            Response.StatusCode = 500;
            return Json ("That tag is already assigned to the sub book.");
         }
         var tag = this.m_Db.Tags.Get (tagId);
         subBook.Tags.Add (new SubBookTag { SubBook = subBook, Tag = tag });
         this.m_Db.Save ();
         return Json ("success");
      }

      /// <summary>
      /// Removes the tag with the given id from the given term.
      /// </summary>
      /// <param name="tagId">Id of tag to remove.</param>
      /// <param name="id">Id of term to remove tag from.</param>
      /// <returns></returns>
      [HttpPost]
      [ValidateAntiForgeryToken]
      [Authorize (Roles = "Editor")]
      public ActionResult RemoveTag (int tagId, int id)
      {
         var subBook = this.m_Db.SubBooks.Get (id);
         if (subBook == null)
         {
            Response.StatusCode = 500;
            return Json ("Unable to get sub book - " + id);
         }
         var subBookTag = subBook.Tags.FirstOrDefault (bt => bt.Id == tagId);
         if (subBookTag == null)
         {
            Response.StatusCode = 500;
            return Json ("Unable to find the given tag.");
         }
         subBook.Tags.Remove (subBookTag);
         this.m_Db.Save ();
         return Json ("success");
      }

      /// <summary>
      /// Creates an alias for a given sub book.
      /// </summary>
      /// <returns></returns>
      [HttpPost]
      [ValidateAntiForgeryToken]
      [Authorize (Roles = "Editor")]
      public ActionResult SetAlias (int id, string alias)
      {
         var subBook = this.m_Db.VersionSubBooks.Get (id);
         subBook.Alias = alias;
         this.m_Db.Save ();
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
