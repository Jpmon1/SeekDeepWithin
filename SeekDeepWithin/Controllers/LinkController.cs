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
   public class LinkController : Controller
   {
      private readonly ISdwDatabase m_Db;

      /// <summary>
      /// Initializes a new controller.
      /// </summary>
      public LinkController ()
      {
         this.m_Db = new SdwDatabase ();
      }

      /// <summary>
      /// Initializes a new controller with the given db info.
      /// </summary>
      /// <param name="db">Database object.</param>
      public LinkController (ISdwDatabase db)
      {
         this.m_Db = db;
      }

      /// <summary>
      /// Shows the edit page for links.
      /// </summary>
      /// <param name="id">Id of item to edit.</param>
      /// <returns>The edit page.</returns>
      [Authorize (Roles = "Editor")]
      public ActionResult EditPassage (int id)
      {
         var viewModel = new LinkEditViewModel { ItemId = id, ItemType = "Passage", NextEntryId = -1, PreviousEntryId = -1 };
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
         var prev = passage.Chapter.Passages.FirstOrDefault (p => p.Order == (passage.Order - 1));
         if (prev != null) viewModel.PreviousEntryId = prev.Id;
         var next = passage.Chapter.Passages.FirstOrDefault (p => p.Order == (passage.Order + 1));
         if (next != null) viewModel.NextEntryId = next.Id;
         var renderable = new PassageViewModel (passage);
         renderable.Styles.Clear ();
         renderable.Footers.Clear ();
         viewModel.RenderedText = new SdwRenderer ().Render (renderable);
         foreach (var link in passage.Passage.Links)
            viewModel.Links.Add (new LinkViewModel (link));
         return View ("Edit", viewModel);
      }

      /// <summary>
      /// Shows the edit page for links.
      /// </summary>
      /// <param name="id">Id of item to edit.</param>
      /// <returns>The edit page.</returns>
      [Authorize (Roles = "Editor")]
      public ActionResult EditEntry (int id)
      {
         var viewModel = new LinkEditViewModel { ItemId = id, ItemType = "Entry", NextEntryId = -1, PreviousEntryId = -1 };
         var entry = this.m_Db.GlossaryEntries.Get (id);
         viewModel.Title = entry.Item.Term.Name;
         viewModel.ItemText = entry.Text;
         viewModel.ParentId = entry.Item.Id;
         var prev = entry.Item.Entries.FirstOrDefault (e => e.Order == (entry.Order - 1));
         if (prev != null) viewModel.PreviousEntryId = prev.Id;
         var next = entry.Item.Entries.FirstOrDefault (e => e.Order == (entry.Order + 1));
         if (next != null) viewModel.NextEntryId = next.Id;
         var renderable = new GlossaryEntryViewModel (entry, null);
         renderable.Styles.Clear ();
         renderable.Footers.Clear ();
         viewModel.RenderedText = new SdwRenderer ().Render (renderable);
         foreach (var link in entry.Links)
            viewModel.Links.Add (new LinkViewModel (link));
         return View ("Edit", viewModel);
      }

      /// <summary>
      /// Shows the edit page for links.
      /// </summary>
      /// <param name="id">Id of item to edit.</param>
      /// <returns>The edit page.</returns>
      [Authorize (Roles = "Editor")]
      public ActionResult EditSeeAlso (int id)
      {
         var viewModel = new LinkEditViewModel { ItemId = id, ItemType = "SeeAlso", NextEntryId = -1, PreviousEntryId = -1 };
         var glossaryTerm = this.m_Db.GlossaryTerms.Get (id);
         viewModel.ParentId = glossaryTerm.Id;
         foreach (var link in glossaryTerm.SeeAlsos)
         {
            var vm = new LinkViewModel (link) {Name = link.Name};
            viewModel.Links.Add (vm);
         }
         return View ("Edit", viewModel);
      }

      /// <summary>
      /// Gets the link edit page for a footer.
      /// </summary>
      /// <param name="id">Id of footer.</param>
      /// <param name="itemId">Parent item id.</param>
      /// <param name="itemType">Type of parent item.</param>
      /// <returns>The edit view.</returns>
      public ActionResult EditFooter (int id, int itemId, string itemType)
      {
         var viewModel = new LinkEditViewModel
         {
            ItemId = id,
            ParentId = itemId,
            ItemType = itemType + "Footer",
            NextEntryId = -1,
            PreviousEntryId = -1
         };
         IFooter footer = null;
         ViewBag.RefUrl = (Request.UrlReferrer != null) ? Request.UrlReferrer.AbsoluteUri : string.Empty;
         if (itemType.ToLower () == "chapter")
         {
            var chapter = this.m_Db.SubBookChapters.Get (itemId);
            viewModel.Title = chapter.Chapter.Name;
            footer = chapter.Footers.FirstOrDefault (f => f.Id == id);
         }
         if (itemType.ToLower () == "passage")
         {
            var passage = this.m_Db.PassageEntries.Get (itemId);
            footer = passage.Footers.FirstOrDefault (f => f.Id == id);
         }
         if (itemType.ToLower () == "entry")
         {
            var entry = this.m_Db.GlossaryEntries.Get (itemId);
            viewModel.Title = entry.Item.Term.Name;
            footer = entry.Footers.FirstOrDefault (f => f.Id == id);
         }
         if (footer == null)
         {
            Response.StatusCode = 500;
            return Json ("Invalid Data.", JsonRequestBehavior.AllowGet);
         }
         viewModel.ItemText = footer.Text;
         foreach (var link in footer.LinkList)
            viewModel.Links.Add (new LinkViewModel (link));
         var renderable = new HeaderFooterViewModel (footer);
         renderable.Styles.Clear();
         viewModel.RenderedText = new SdwRenderer ().Render (renderable);
         return View ("Edit", viewModel);
      }

      /// <summary>
      /// Posts a new link for the given Item.
      /// </summary>
      /// <param name="itemId">The id of the link's parent item.</param>
      /// <param name="itemType">The type of the link's parent item.</param>
      /// <param name="startIndex">The start index of the link.</param>
      /// <param name="endIndex">The end index of the link.</param>
      /// <param name="openInNewWindow">True to open in new window.</param>
      /// <param name="linkUrl">The url of the link.</param>
      /// <param name="linkName">The name of the link .</param>
      /// <param name="parentId">The parent id of the item.</param>
      /// <returns>Results</returns>
      [HttpPost]
      [ValidateAntiForgeryToken]
      [Authorize (Roles = "Editor")]
      public ActionResult Create (int itemId, string itemType, int startIndex, int endIndex, bool openInNewWindow,
         string linkUrl, string linkName, int? parentId)
      {
         ILink rtnLink = null;
         var link = GetLink (linkUrl);
         if (itemType.ToLower () == "passage")
         {
            var passage = this.m_Db.PassageEntries.Get (itemId);
            rtnLink = new PassageLink { Link = link, StartIndex = startIndex, EndIndex = endIndex, OpenInNewWindow = openInNewWindow };
            passage.Passage.Links.Add ((PassageLink)rtnLink);
         }
         else if (itemType.ToLower () == "entry")
         {
            var entry = this.m_Db.GlossaryEntries.Get (itemId);
            rtnLink = new GlossaryEntryLink { Link = link, StartIndex = startIndex, EndIndex = endIndex, OpenInNewWindow = openInNewWindow };
            entry.Links.Add ((GlossaryEntryLink)rtnLink);
         }
         else if (itemType.ToLower () == "seealso")
         {
            var term = this.m_Db.GlossaryTerms.Get (itemId);
            rtnLink = new GlossarySeeAlso { Link = link, Term = term, Name = linkName };
            term.SeeAlsos.Add ((GlossarySeeAlso)rtnLink);
         }
         else if (itemType.ToLower () == "passagefooter" && parentId.HasValue)
         {
            var passage = this.m_Db.PassageEntries.Get (parentId.Value);
            var footer = passage.Footers.FirstOrDefault (f => f.Id == itemId);
            if (footer != null)
            {
               rtnLink = new PassageFooterLink { Link = link, StartIndex = startIndex, EndIndex = endIndex, OpenInNewWindow = openInNewWindow };
               footer.Links.Add ((PassageFooterLink)rtnLink);
            }
         }
         else if (itemType.ToLower () == "entryfooter" && parentId.HasValue)
         {
            var entry = this.m_Db.GlossaryEntries.Get (parentId.Value);
            var footer = entry.Footers.FirstOrDefault (f => f.Id == itemId);
            if (footer != null)
            {
               rtnLink = new GlossaryFooterLink { Link = link, StartIndex = startIndex, EndIndex = endIndex, OpenInNewWindow = openInNewWindow };
               footer.Links.Add ((GlossaryFooterLink)rtnLink);
            }
         }
         else if (itemType.ToLower () == "chapterfooter" && parentId.HasValue)
         {
            var chapter = this.m_Db.SubBookChapters.Get (parentId.Value);
            var footer = chapter.Footers.FirstOrDefault (f => f.Id == itemId);
            if (footer != null)
            {
               rtnLink = new ChapterFooterLink { Link = link, StartIndex = startIndex, EndIndex = endIndex, OpenInNewWindow = openInNewWindow };
               footer.Links.Add ((ChapterFooterLink)rtnLink);
            }
         }

         if (rtnLink != null)
         {
            this.m_Db.Save ();
            return Json (new { id = rtnLink.Id, startIndex, endIndex, linkUrl });
         }

         Response.StatusCode = 500;
         return Json ("Unknown link item type.");
      }
      
      /// <summary>
      /// Gets the link in the database with the given url.
      /// </summary>
      /// <param name="linkUrl">Url of link to get.</param>
      /// <returns>The requested link, a new link if does not exist.</returns>
      private Link GetLink (string linkUrl)
      {
         var links = this.m_Db.Links.Get (l => l.Url == linkUrl);
         var link = links.FirstOrDefault ();
         if (link == null)
         {
            link = new Link { Url = linkUrl };
            this.m_Db.Links.Insert (link);
            this.m_Db.Save ();
         }
         return link;
      }

      /// <summary>
      /// Updates a link for the given Item.
      /// </summary>
      /// <param name="linkId">The id of the link to update.</param>
      /// <param name="itemId">The id of the link's parent entry.</param>
      /// <param name="itemType">The type of the link's parent entry.</param>
      /// <param name="startIndex">The start index of the link.</param>
      /// <param name="endIndex">The end index of the link.</param>
      /// <param name="openInNewWindow">True to open in new window.</param>
      /// <param name="linkName">The name of the link .</param>
      /// <returns>Results</returns>
      [HttpPost]
      [ValidateAntiForgeryToken]
      [Authorize (Roles = "Editor")]
      public ActionResult Update (int linkId, int itemId, string itemType, int startIndex, int endIndex, bool openInNewWindow, string linkName)
      {
         if (itemType.ToLower () == "passage")
         {
            var passage = this.m_Db.PassageEntries.Get (itemId);
            var pLink = passage.Passage.Links.FirstOrDefault (l => l.Id == linkId);
            if (pLink != null)
            {
               pLink.EndIndex = endIndex;
               pLink.StartIndex = startIndex;
               pLink.OpenInNewWindow = openInNewWindow;
            }
         }
         else if (itemType.ToLower () == "entry")
         {
            var entry = this.m_Db.GlossaryEntries.Get (itemId);
            var eLink = entry.Links.FirstOrDefault (l => l.Id == linkId);
            if (eLink != null)
            {
               eLink.EndIndex = endIndex;
               eLink.StartIndex = startIndex;
               eLink.OpenInNewWindow = openInNewWindow;
            }
         }
         else if (itemType.ToLower () == "seealso")
         {
            var term = this.m_Db.GlossaryTerms.Get (itemId);
            var tLink = term.SeeAlsos.FirstOrDefault (l => l.Id == linkId);
            if (tLink != null)
               tLink.Name = linkName;
         }
         else
         {
            Response.StatusCode = 500;
            return Json ("Unknown link item type.");
         }
         this.m_Db.Save ();
         return Json ("Success!");
      }

      /// <summary>
      /// Deletes a link for the given item.
      /// </summary>
      /// <param name="id">Id of link to get.</param>
      /// <param name="itemId">Id of parent item.</param>
      /// <param name="itemType">Type of parent item.</param>
      /// <param name="parentId">The parent id of the item.</param>
      /// <returns>Results.</returns>
      [HttpPost]
      [ValidateAntiForgeryToken]
      [Authorize (Roles = "Editor")]
      public ActionResult Delete (int id, int itemId, string itemType, int? parentId)
      {
         if (itemType.ToLower () == "passage")
         {
            var passage = this.m_Db.PassageEntries.Get (itemId);
            var link = passage.Passage.Links.FirstOrDefault (s => s.Id == id);
            if (link != null)
               passage.Passage.Links.Remove (link);
         }
         else if (itemType.ToLower () == "entry")
         {
            var entry = this.m_Db.GlossaryEntries.Get (itemId);
            var link = entry.Links.FirstOrDefault (s => s.Id == id);
            if (link != null)
               entry.Links.Remove (link);
         }
         else if (itemType.ToLower () == "seealso")
         {
            var term = this.m_Db.GlossaryTerms.Get (itemId);
            var link = term.SeeAlsos.FirstOrDefault (s => s.Id == id);
            if (link != null)
               term.SeeAlsos.Remove (link);
         }
         else if (itemType.ToLower () == "passagefooter" && parentId.HasValue)
         {
            var passage = this.m_Db.PassageEntries.Get (parentId.Value);
            var footer = passage.Footers.FirstOrDefault (f => f.Id == itemId);
            if (footer != null)
            {
               var link = footer.Links.FirstOrDefault (s => s.Id == id);
               if (link != null)
                  footer.Links.Remove (link);
            }
         }
         else if (itemType.ToLower () == "entryfooter" && parentId.HasValue)
         {
            var entry = this.m_Db.GlossaryEntries.Get (parentId.Value);
            var footer = entry.Footers.FirstOrDefault (f => f.Id == itemId);
            if (footer != null)
            {
               var link = footer.Links.FirstOrDefault (s => s.Id == id);
               if (link != null)
                  footer.Links.Remove (link);
            }
         }
         else if (itemType.ToLower () == "chapterfooter" && parentId.HasValue)
         {
            var chapter = this.m_Db.SubBookChapters.Get (parentId.Value);
            var footer = chapter.Footers.FirstOrDefault (f => f.Id == itemId);
            if (footer != null)
            {
               var link = footer.Links.FirstOrDefault (s => s.Id == id);
               if (link != null)
                  footer.Links.Remove (link);
            }
         }
         else
         {
            Response.StatusCode = 500;
            return Json ("Unknown link item type.");
         }
         this.m_Db.Save ();
         return Json ("Success!");
      }

      /// <summary>
      /// Gets a link for the given item.
      /// </summary>
      /// <param name="id">Id of link to get.</param>
      /// <param name="itemId">Id of parent item.</param>
      /// <param name="itemType">Type of parent item.</param>
      /// <param name="parentId">The parent id of the item.</param>
      /// <returns>Results.</returns>
      public ActionResult Get (int id, int itemId, string itemType, int? parentId)
      {
         ILink link = null;
         var name = string.Empty;
         if (itemType.ToLower () == "passage")
         {
            var passage = this.m_Db.PassageEntries.Get (itemId);
            link = passage.Passage.Links.FirstOrDefault (s => s.Id == id);
         }
         else if (itemType.ToLower () == "entry")
         {
            var entry = this.m_Db.GlossaryEntries.Get (itemId);
            link = entry.Links.FirstOrDefault (s => s.Id == id);
         }
         else if (itemType.ToLower () == "seealso")
         {
            var term = this.m_Db.GlossaryTerms.Get (itemId);
            var seeAlso = term.SeeAlsos.FirstOrDefault (s => s.Id == id);
            if (seeAlso != null)
            {
               name = seeAlso.Name;
               link = seeAlso;
            }
         }
         else if (itemType.ToLower () == "passagefooter" && parentId.HasValue)
         {
            var passage = this.m_Db.PassageEntries.Get (parentId.Value);
            var footer = passage.Footers.FirstOrDefault (f => f.Id == itemId);
            if (footer != null)
               link = footer.Links.FirstOrDefault (s => s.Id == id);
         }
         else if (itemType.ToLower () == "entryfooter" && parentId.HasValue)
         {
            var entry = this.m_Db.GlossaryEntries.Get (parentId.Value);
            var footer = entry.Footers.FirstOrDefault (f => f.Id == itemId);
            if (footer != null)
               link = footer.Links.FirstOrDefault (s => s.Id == id);
         }
         else if (itemType.ToLower () == "chapterfooter" && parentId.HasValue)
         {
            var chapter = this.m_Db.SubBookChapters.Get (parentId.Value);
            var footer = chapter.Footers.FirstOrDefault (f => f.Id == itemId);
            if (footer != null)
               link = footer.Links.FirstOrDefault (s => s.Id == id);
         }

         if (link != null)
         {
            return Json (new
            {
               name,
               id = link.Id,
               startIndex = link.StartIndex,
               endIndex = link.EndIndex,
               url = link.Link.Url
            }, JsonRequestBehavior.AllowGet);
         }
         Response.StatusCode = 500;
         return Json ("Invalid Data.", JsonRequestBehavior.AllowGet);
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
            var passage = this.m_Db.PassageEntries.Get (itemId);
            renderable = new PassageViewModel (passage);
         }
         else if (itemType.ToLower () == "entry")
         {
            var entry = this.m_Db.GlossaryEntries.Get (itemId);
            renderable = new GlossaryEntryViewModel (entry, null);
         }
         else if (itemType.ToLower () == "passagefooter" && parentId.HasValue)
         {
            var passage = this.m_Db.PassageEntries.Get (parentId.Value);
            var footer = passage.Footers.FirstOrDefault (f => f.Id == itemId);
            renderable = new HeaderFooterViewModel (footer);
         }
         else if (itemType.ToLower () == "entryfooter" && parentId.HasValue)
         {
            var entry = this.m_Db.GlossaryEntries.Get (parentId.Value);
            var footer = entry.Footers.FirstOrDefault (f => f.Id == itemId);
            renderable = new HeaderFooterViewModel (footer);
         }
         else if (itemType.ToLower () == "chapterfooter" && parentId.HasValue)
         {
            var chapter = this.m_Db.SubBookChapters.Get (parentId.Value);
            var footer = chapter.Footers.FirstOrDefault (f => f.Id == itemId);
            renderable = new HeaderFooterViewModel (footer);
         }

         if (renderable == null)
         {
            Response.StatusCode = 500;
            return Json ("Invalid Data.", JsonRequestBehavior.AllowGet);
         }

         renderable.Styles.Clear ();
         renderable.Footers.Clear ();
         var html = new SdwRenderer ().Render (renderable);
         return Json (new { html }, JsonRequestBehavior.AllowGet);
      }
   }
}
