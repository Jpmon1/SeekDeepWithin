using System.Linq;
using System.Web.Mvc;
using SeekDeepWithin.DataAccess;
using SeekDeepWithin.Pocos;
using SeekDeepWithin.Models;

namespace SeekDeepWithin.Controllers
{
   /// <summary>
   /// Controller for links.
   /// </summary>
   public class LinkController : SdwController
   {
      /// <summary>
      /// Initializes a new controller.
      /// </summary>
      public LinkController () : base (new SdwDatabase ()) { }

      /// <summary>
      /// Initializes a new controller with the given db info.
      /// </summary>
      /// <param name="db">Database object.</param>
      public LinkController (ISdwDatabase db) : base (db) { }

      /// <summary>
      /// Posts a new link for the given Item.
      /// </summary>
      /// <param name="itemId">The id of the link's parent item.</param>
      /// <param name="startIndex">The start index of the link.</param>
      /// <param name="endIndex">The end index of the link.</param>
      /// <param name="openInNewWindow">True to open in new window.</param>
      /// <param name="linkUrl">The url of the link.</param>
      /// <returns>Results</returns>
      [HttpPost]
      [ValidateAntiForgeryToken]
      [Authorize (Roles = "Editor")]
      public ActionResult CreatePassage (int itemId, int startIndex, int endIndex, bool openInNewWindow, string linkUrl)
      {
         var passage = this.Database.PassageEntries.Get (itemId);
         if (passage == null) return this.Fail ("Unable to determine the passage");
         var link = GetLink (linkUrl);
         var rtnLink = new PassageLink
         {
            Link = link,
            StartIndex = startIndex,
            EndIndex = endIndex,
            OpenInNewWindow = openInNewWindow
         };
         passage.Passage.Links.Add (rtnLink);
         this.Database.Save ();
         return Json (new { status = SUCCESS, id = rtnLink.Id, startIndex, endIndex, linkUrl });
      }

      /// <summary>
      /// Posts a new link for the given Item.
      /// </summary>
      /// <param name="itemId">The id of the link's parent item.</param>
      /// <param name="startIndex">The start index of the link.</param>
      /// <param name="endIndex">The end index of the link.</param>
      /// <param name="openInNewWindow">True to open in new window.</param>
      /// <param name="linkUrl">The url of the link.</param>
      /// <returns>Results</returns>
      [HttpPost]
      [ValidateAntiForgeryToken]
      [Authorize (Roles = "Editor")]
      public ActionResult CreateItemEntry (int itemId, int startIndex, int endIndex, bool openInNewWindow, string linkUrl)
      {
         var entry = this.Database.TermItemEntries.Get (itemId);
         if (entry == null) return this.Fail ("Unable to determine the entry");
         var link = GetLink (linkUrl);
         var rtnLink = new TermItemEntryLink
         {
            Link = link,
            StartIndex = startIndex,
            EndIndex = endIndex,
            OpenInNewWindow = openInNewWindow
         };
         entry.Links.Add (rtnLink);
         this.Database.Save ();
         return Json (new { status = SUCCESS, id = rtnLink.Id, startIndex, endIndex, linkUrl });
      }

      /// <summary>
      /// Posts a new link for the given Item.
      /// </summary>
      /// <param name="itemId">The id of the link's parent item.</param>
      /// <param name="startIndex">The start index of the link.</param>
      /// <param name="endIndex">The end index of the link.</param>
      /// <param name="openInNewWindow">True to open in new window.</param>
      /// <param name="linkUrl">The url of the link.</param>
      /// <param name="footerId">The parent id of the item.</param>
      /// <returns>Results</returns>
      [HttpPost]
      [ValidateAntiForgeryToken]
      [Authorize (Roles = "Editor")]
      public ActionResult CreatePassageFooter (int itemId, int startIndex, int endIndex, bool openInNewWindow, string linkUrl, int footerId)
      {
         var passage = this.Database.PassageEntries.Get (itemId);
         if (passage == null) return this.Fail ("Unable to determine the passage");
         var footer = passage.Footers.FirstOrDefault (f => f.Id == footerId);
         if (footer == null) return this.Fail ("Unable to determine the footer");
         var link = GetLink (linkUrl);
         var rtnLink = new PassageFooterLink
         {
            Link = link,
            StartIndex = startIndex,
            EndIndex = endIndex,
            OpenInNewWindow = openInNewWindow
         };
         footer.Links.Add (rtnLink);
         this.Database.Save ();
         return Json (new { status = SUCCESS, id = rtnLink.Id, startIndex, endIndex, linkUrl });
      }

      /// <summary>
      /// Posts a new link for the given Item.
      /// </summary>
      /// <param name="itemId">The id of the link's parent item.</param>
      /// <param name="startIndex">The start index of the link.</param>
      /// <param name="endIndex">The end index of the link.</param>
      /// <param name="openInNewWindow">True to open in new window.</param>
      /// <param name="linkUrl">The url of the link.</param>
      /// <param name="footerId">The parent id of the item.</param>
      /// <returns>Results</returns>
      [HttpPost]
      [ValidateAntiForgeryToken]
      [Authorize (Roles = "Editor")]
      public ActionResult CreateChapterFooter (int itemId, int startIndex, int endIndex, bool openInNewWindow, string linkUrl, int footerId)
      {
         var chapter = this.Database.SubBookChapters.Get (itemId);
         if (chapter == null) return this.Fail ("Unable to determine the passage");
         if (chapter.Footer == null) return this.Fail ("Unable to determine the footer");
         var link = GetLink (linkUrl);
         var rtnLink = new ChapterFooterLink
         {
            Link = link,
            StartIndex = startIndex,
            EndIndex = endIndex,
            OpenInNewWindow = openInNewWindow
         };
         chapter.Footer.Links.Add (rtnLink);
         this.Database.Save ();
         return Json (new { status = SUCCESS, id = rtnLink.Id, startIndex, endIndex, linkUrl });
      }

      /// <summary>
      /// Posts a new link for the given Item.
      /// </summary>
      /// <param name="itemId">The id of the link's parent item.</param>
      /// <param name="startIndex">The start index of the link.</param>
      /// <param name="endIndex">The end index of the link.</param>
      /// <param name="openInNewWindow">True to open in new window.</param>
      /// <param name="linkUrl">The url of the link.</param>
      /// <param name="footerId">The parent id of the item.</param>
      /// <returns>Results</returns>
      [HttpPost]
      [ValidateAntiForgeryToken]
      [Authorize (Roles = "Editor")]
      public ActionResult CreateItemEntryFooter (int itemId, int startIndex, int endIndex, bool openInNewWindow, string linkUrl, int footerId)
      {
         var entry = this.Database.TermItemEntries.Get (itemId);
         if (entry == null) return this.Fail ("Unable to determine the entry");
         var footer = entry.Footers.FirstOrDefault (f => f.Id == footerId);
         if (footer == null) return this.Fail ("Unable to determine the footer");
         var link = GetLink (linkUrl);
         var rtnLink = new TermItemEntryFooterLink
         {
            Link = link,
            StartIndex = startIndex,
            EndIndex = endIndex,
            OpenInNewWindow = openInNewWindow
         };
         footer.Links.Add (rtnLink);
         this.Database.Save ();
         return Json (new { status = SUCCESS, id = rtnLink.Id, startIndex, endIndex, linkUrl });
      }

      /// <summary>
      /// Updates a link for the given Item.
      /// </summary>
      /// <param name="id">The id of the link to update.</param>
      /// <param name="itemId">The id of the link's parent entry.</param>
      /// <param name="startIndex">The start index of the link.</param>
      /// <param name="endIndex">The end index of the link.</param>
      /// <param name="openInNewWindow">True to open in new window.</param>
      /// <returns>Results</returns>
      [HttpPost]
      [ValidateAntiForgeryToken]
      [Authorize (Roles = "Editor")]
      public ActionResult UpdatePassage (int id, int itemId, int startIndex, int endIndex, bool openInNewWindow)
      {
         var passage = this.Database.PassageEntries.Get (itemId);
         if (passage == null) return this.Fail ("Unable to determine the passage");
         var link = passage.Passage.Links.FirstOrDefault (l => l.Id == id);
         if (link == null) return this.Fail ("Unable to determine the link");
         link.EndIndex = endIndex;
         link.StartIndex = startIndex;
         link.OpenInNewWindow = openInNewWindow;
         this.Database.Save ();
         return Json (new { status = SUCCESS, id, startIndex, endIndex, openInNewWindow, linkUrl = link.Link.Url });
      }

      /// <summary>
      /// Updates a link for the given Item.
      /// </summary>
      /// <param name="id">The id of the link to update.</param>
      /// <param name="itemId">The id of the link's parent entry.</param>
      /// <param name="startIndex">The start index of the link.</param>
      /// <param name="endIndex">The end index of the link.</param>
      /// <param name="openInNewWindow">True to open in new window.</param>
      /// <returns>Results</returns>
      [HttpPost]
      [ValidateAntiForgeryToken]
      [Authorize (Roles = "Editor")]
      public ActionResult UpdateItemEntry (int id, int itemId, int startIndex, int endIndex, bool openInNewWindow)
      {
         var entry = this.Database.TermItemEntries.Get (itemId);
         if (entry == null) return this.Fail ("Unable to determine the entry");
         var link = entry.Links.FirstOrDefault (l => l.Id == id);
         if (link == null) return this.Fail ("Unable to determine the link");
         link.EndIndex = endIndex;
         link.StartIndex = startIndex;
         link.OpenInNewWindow = openInNewWindow;
         this.Database.Save ();
         return Json (new { status = SUCCESS, id, startIndex, endIndex, openInNewWindow, linkUrl = link.Link.Url });
      }

      /// <summary>
      /// Updates a link for the given Item.
      /// </summary>
      /// <param name="id">The id of the link to update.</param>
      /// <param name="itemId">The id of the link's parent entry.</param>
      /// <param name="footerId">The parent id of the item.</param>
      /// <param name="startIndex">The start index of the link.</param>
      /// <param name="endIndex">The end index of the link.</param>
      /// <param name="openInNewWindow">True to open in new window.</param>
      /// <returns>Results</returns>
      [HttpPost]
      [ValidateAntiForgeryToken]
      [Authorize (Roles = "Editor")]
      public ActionResult UpdatePassageFooter (int id, int itemId, int footerId, int startIndex, int endIndex, bool openInNewWindow)
      {
         var passage = this.Database.PassageEntries.Get (itemId);
         if (passage == null) return this.Fail ("Unable to determine the passage");
         var footer = passage.Footers.FirstOrDefault (f => f.Id == footerId);
         if (footer == null) return this.Fail ("Unable to determine the footer");
         var link = footer.Links.FirstOrDefault (l => l.Id == id);
         if (link == null) return this.Fail ("Unable to determine the link");
         link.EndIndex = endIndex;
         link.StartIndex = startIndex;
         link.OpenInNewWindow = openInNewWindow;
         this.Database.Save ();
         return Json (new { status = SUCCESS, id, startIndex, endIndex, openInNewWindow, linkUrl = link.Link.Url });
      }

      /// <summary>
      /// Updates a link for the given Item.
      /// </summary>
      /// <param name="id">The id of the link to update.</param>
      /// <param name="itemId">The id of the link's parent entry.</param>
      /// <param name="footerId">The parent id of the item.</param>
      /// <param name="startIndex">The start index of the link.</param>
      /// <param name="endIndex">The end index of the link.</param>
      /// <param name="openInNewWindow">True to open in new window.</param>
      /// <returns>Results</returns>
      [HttpPost]
      [ValidateAntiForgeryToken]
      [Authorize (Roles = "Editor")]
      public ActionResult UpdateChapterFooter (int id, int itemId, int footerId, int startIndex, int endIndex, bool openInNewWindow)
      {
         var chapter = this.Database.SubBookChapters.Get (itemId);
         if (chapter == null) return this.Fail ("Unable to determine the passage");
         if (chapter.Footer == null) return this.Fail ("Unable to determine the footer");
         var link = chapter.Footer.Links.FirstOrDefault (l => l.Id == id);
         if (link == null) return this.Fail ("Unable to determine the link");
         link.EndIndex = endIndex;
         link.StartIndex = startIndex;
         link.OpenInNewWindow = openInNewWindow;
         this.Database.Save ();
         return Json (new { status = SUCCESS, id, startIndex, endIndex, openInNewWindow, linkUrl = link.Link.Url });
      }

      /// <summary>
      /// Updates a link for the given Item.
      /// </summary>
      /// <param name="id">The id of the link to update.</param>
      /// <param name="itemId">The id of the link's parent entry.</param>
      /// <param name="footerId">The parent id of the item.</param>
      /// <param name="startIndex">The start index of the link.</param>
      /// <param name="endIndex">The end index of the link.</param>
      /// <param name="openInNewWindow">True to open in new window.</param>
      /// <returns>Results</returns>
      [HttpPost]
      [ValidateAntiForgeryToken]
      [Authorize (Roles = "Editor")]
      public ActionResult UpdateItemEntryFooter (int id, int itemId, int footerId, int startIndex, int endIndex, bool openInNewWindow)
      {
         var entry = this.Database.TermItemEntries.Get (itemId);
         if (entry == null) return this.Fail ("Unable to determine the entry");
         var footer = entry.Footers.FirstOrDefault (f => f.Id == footerId);
         if (footer == null) return this.Fail ("Unable to determine the footer");
         var link = footer.Links.FirstOrDefault (l => l.Id == id);
         if (link == null) return this.Fail ("Unable to determine the link");
         link.EndIndex = endIndex;
         link.StartIndex = startIndex;
         link.OpenInNewWindow = openInNewWindow;
         this.Database.Save ();
         return Json (new { status = SUCCESS, id, startIndex, endIndex, openInNewWindow, linkUrl = link.Link.Url });
      }

      /// <summary>
      /// Deletes a link for the given item.
      /// </summary>
      /// <param name="id">Id of link to get.</param>
      /// <param name="itemId">Id of parent item.</param>
      /// <returns>Results.</returns>
      [HttpPost]
      [ValidateAntiForgeryToken]
      [Authorize (Roles = "Editor")]
      public ActionResult DeletePassage (int id, int itemId)
      {
         var passage = this.Database.PassageEntries.Get (itemId);
         if (passage == null) return this.Fail ("Unable to determine the passage");
         var link = passage.Passage.Links.FirstOrDefault (s => s.Id == id);
         if (link == null) return this.Fail ("Unable to determine the link");
         passage.Passage.Links.Remove (link);
         this.Database.Save ();
         return this.Success ();
      }

      /// <summary>
      /// Deletes a link for the given item.
      /// </summary>
      /// <param name="id">Id of link to get.</param>
      /// <param name="itemId">Id of parent item.</param>
      /// <returns>Results.</returns>
      [HttpPost]
      [ValidateAntiForgeryToken]
      [Authorize (Roles = "Editor")]
      public ActionResult DeleteItemEntry (int id, int itemId)
      {
         var entry = this.Database.TermItemEntries.Get (itemId);
         if (entry == null) return this.Fail ("Unable to determine the entry");
         var link = entry.Links.FirstOrDefault (s => s.Id == id);
         if (link == null) return this.Fail ("Unable to determine the link");
         entry.Links.Remove (link);
         this.Database.Save ();
         return this.Success ();
      }

      /// <summary>
      /// Deletes a link for the given item.
      /// </summary>
      /// <param name="id">Id of link to get.</param>
      /// <param name="itemId">Id of parent item.</param>
      /// <param name="footerId">The parent id of the item.</param>
      /// <returns>Results.</returns>
      [HttpPost]
      [ValidateAntiForgeryToken]
      [Authorize (Roles = "Editor")]
      public ActionResult DeletePassageFooter (int id, int itemId, int footerId)
      {
         var passage = this.Database.PassageEntries.Get (itemId);
         if (passage == null) return this.Fail ("Unable to determine the passage");
         var footer = passage.Footers.FirstOrDefault (f => f.Id == footerId);
         if (footer == null) return this.Fail ("Unable to determine the footer");
         var link = footer.Links.FirstOrDefault (s => s.Id == id);
         if (link == null) return this.Fail ("Unable to determine the link");
         footer.Links.Remove (link);
         this.Database.Save ();
         return this.Success ();
      }

      /// <summary>
      /// Deletes a link for the given item.
      /// </summary>
      /// <param name="id">Id of link to get.</param>
      /// <param name="itemId">Id of parent item.</param>
      /// <param name="footerId">The parent id of the item.</param>
      /// <returns>Results.</returns>
      [HttpPost]
      [ValidateAntiForgeryToken]
      [Authorize (Roles = "Editor")]
      public ActionResult DeleteChapterFooter (int id, int itemId, int footerId)
      {
         var chapter = this.Database.SubBookChapters.Get (itemId);
         if (chapter == null) return this.Fail ("Unable to determine the passage");
         if (chapter.Footer == null) return this.Fail ("Unable to determine the footer");
         var link = chapter.Footer.Links.FirstOrDefault (s => s.Id == id);
         if (link == null) return this.Fail ("Unable to determine the link");
         chapter.Footer.Links.Remove (link);
         this.Database.Save ();
         return this.Success ();
      }

      /// <summary>
      /// Deletes a link for the given item.
      /// </summary>
      /// <param name="id">Id of link to get.</param>
      /// <param name="itemId">Id of parent item.</param>
      /// <param name="footerId">The parent id of the item.</param>
      /// <returns>Results.</returns>
      [HttpPost]
      [ValidateAntiForgeryToken]
      [Authorize (Roles = "Editor")]
      public ActionResult DeleteItemEntryFooter (int id, int itemId, int footerId)
      {
         var entry = this.Database.TermItemEntries.Get (itemId);
         if (entry == null) return this.Fail ("Unable to determine the entry");
         var footer = entry.Footers.FirstOrDefault (f => f.Id == footerId);
         if (footer == null) return this.Fail ("Unable to determine the footer");
         var link = footer.Links.FirstOrDefault (s => s.Id == id);
         if (link == null) return this.Fail ("Unable to determine the link");
         footer.Links.Remove (link);
         this.Database.Save ();
         return this.Success ();
      }

      /// <summary>
      /// Gets a link for the given item.
      /// </summary>
      /// <param name="id">Id of link to get.</param>
      /// <param name="itemId">Id of parent item.</param>
      /// <returns>Results.</returns>
      public ActionResult GetPassage (int id, int itemId)
      {
         var passage = this.Database.PassageEntries.Get (itemId);
         if (passage == null) return this.Fail ("Unable to determine the passage");
         var link = passage.Passage.Links.FirstOrDefault (s => s.Id == id);
         if (link == null) return this.Fail ("Unable to determine the link");
         return Json (new
         {
            status = SUCCESS,
            id = link.Id,
            startIndex = link.StartIndex,
            endIndex = link.EndIndex,
            url = link.Link.Url,
            openInNewWindow = link.OpenInNewWindow
         }, JsonRequestBehavior.AllowGet);
      }

      /// <summary>
      /// Gets a link for the given item.
      /// </summary>
      /// <param name="id">Id of link to get.</param>
      /// <param name="itemId">Id of parent item.</param>
      /// <returns>Results.</returns>
      public ActionResult GetItemEntry (int id, int itemId)
      {
         var entry = this.Database.TermItemEntries.Get (itemId);
         if (entry == null) return this.Fail ("Unable to determine the entry");
         var link = entry.Links.FirstOrDefault (s => s.Id == id);
         if (link == null) return this.Fail ("Unable to determine the link");
         return Json (new
         {
            status = SUCCESS,
            id = link.Id,
            startIndex = link.StartIndex,
            endIndex = link.EndIndex,
            url = link.Link.Url,
            openInNewWindow = link.OpenInNewWindow
         }, JsonRequestBehavior.AllowGet);
      }

      /// <summary>
      /// Gets a link for the given item.
      /// </summary>
      /// <param name="id">Id of link to get.</param>
      /// <param name="itemId">Id of parent item.</param>
      /// <param name="footerId">The parent id of the item.</param>
      /// <returns>Results.</returns>
      public ActionResult GetPassageFooter (int id, int itemId, int footerId)
      {
         var passage = this.Database.PassageEntries.Get (itemId);
         if (passage == null) return this.Fail ("Unable to determine the passage");
         var footer = passage.Footers.FirstOrDefault (f => f.Id == footerId);
         if (footer == null) return this.Fail ("Unable to determine the footer");
         var link = footer.Links.FirstOrDefault (s => s.Id == id);
         if (link == null) return this.Fail ("Unable to determine the link");
         return Json (new
         {
            status = SUCCESS,
            id = link.Id,
            startIndex = link.StartIndex,
            endIndex = link.EndIndex,
            url = link.Link.Url,
            openInNewWindow = link.OpenInNewWindow
         }, JsonRequestBehavior.AllowGet);
      }

      /// <summary>
      /// Gets a link for the given item.
      /// </summary>
      /// <param name="id">Id of link to get.</param>
      /// <param name="itemId">Id of parent item.</param>
      /// <param name="footerId">The parent id of the item.</param>
      /// <returns>Results.</returns>
      public ActionResult GetChapterFooter (int id, int itemId, int footerId)
      {
         var chapter = this.Database.SubBookChapters.Get (itemId);
         if (chapter == null) return this.Fail ("Unable to determine the passage");
         if (chapter.Footer == null) return this.Fail ("Unable to determine the footer");
         var link = chapter.Footer.Links.FirstOrDefault (s => s.Id == id);
         if (link == null) return this.Fail ("Unable to determine the link");
         return Json (new
         {
            status = SUCCESS,
            id = link.Id,
            startIndex = link.StartIndex,
            endIndex = link.EndIndex,
            url = link.Link.Url,
            openInNewWindow = link.OpenInNewWindow
         }, JsonRequestBehavior.AllowGet);
      }

      /// <summary>
      /// Gets a link for the given item.
      /// </summary>
      /// <param name="id">Id of link to get.</param>
      /// <param name="itemId">Id of parent item.</param>
      /// <param name="footerId">The parent id of the item.</param>
      /// <returns>Results.</returns>
      public ActionResult GetItemEntryFooter (int id, int itemId, int footerId)
      {
         var entry = this.Database.TermItemEntries.Get (itemId);
         if (entry == null) return this.Fail ("Unable to determine the entry");
         var footer = entry.Footers.FirstOrDefault (f => f.Id == footerId);
         if (footer == null) return this.Fail ("Unable to determine the footer");
         var link = footer.Links.FirstOrDefault (s => s.Id == id);
         if (link == null) return this.Fail ("Unable to determine the link");
         return Json (new
         {
            status = SUCCESS,
            id = link.Id,
            startIndex = link.StartIndex,
            endIndex = link.EndIndex,
            url = link.Link.Url,
            openInNewWindow = link.OpenInNewWindow
         }, JsonRequestBehavior.AllowGet);
      }

      /// <summary>
      /// Renders the link for the given item.
      /// </summary>
      /// <param name="itemId">Id of parent item.</param>
      /// <param name="itemType">Type of parent item.</param>
      /// <param name="parentId">The parent id of the item.</param>
      /// <returns>Results.</returns>
      public ActionResult Render (int itemId, string itemType, int? parentId)
      {
         IRenderable renderable = null;
         if (itemType.ToLower () == "passage")
         {
            var passage = this.Database.PassageEntries.Get (itemId);
            renderable = new PassageViewModel (passage);
         }
         else if (itemType.ToLower () == "entry")
         {
            var entry = this.Database.TermItemEntries.Get (itemId);
            renderable = new TermItemEntryViewModel (entry, null);
         }
         else if (itemType.ToLower () == "passagefooter" && parentId.HasValue)
         {
            var passage = this.Database.PassageEntries.Get (parentId.Value);
            var footer = passage.Footers.FirstOrDefault (f => f.Id == itemId);
            renderable = new HeaderFooterViewModel (footer);
         }
         else if (itemType.ToLower () == "entryfooter" && parentId.HasValue)
         {
            var entry = this.Database.TermItemEntries.Get (parentId.Value);
            var footer = entry.Footers.FirstOrDefault (f => f.Id == itemId);
            renderable = new HeaderFooterViewModel (footer);
         }

         if (renderable == null)
         {
            Response.StatusCode = 500;
            return this.Fail ("Invalid Data.");
         }

         renderable.Styles.Clear ();
         renderable.Footers.Clear ();
         var html = new SdwRenderer ().Render (renderable);
         return Json (new { html }, JsonRequestBehavior.AllowGet);
      }

      /// <summary>
      /// Gets the link in the database with the given url.
      /// </summary>
      /// <param name="linkUrl">Url of link to get.</param>
      /// <returns>The requested link, a new link if does not exist.</returns>
      private Link GetLink (string linkUrl)
      {
         var links = this.Database.Links.Get (l => l.Url == linkUrl);
         var link = links.FirstOrDefault ();
         if (link == null)
         {
            link = new Link { Url = linkUrl };
            this.Database.Links.Insert (link);
            this.Database.Save ();
         }
         return link;
      }
   }
}
