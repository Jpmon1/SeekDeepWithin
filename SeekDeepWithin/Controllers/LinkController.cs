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
      /// Shows the edit page for styles.
      /// </summary>
      /// <param name="id">Id of item to edit.</param>
      /// <returns>The edit page.</returns>
      [Authorize (Roles = "Editor")]
      public ActionResult EditPassage (int id)
      {
         var viewModel = new LinkEditViewModel { ItemId = id, ItemType = "Passage", NextEntryId = -1, PreviousEntryId = -1 };
         var passage = this.m_Db.PassageEntries.Get (id);
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
         {
            viewModel.Links.Add (new LinkViewModel
            {
               ItemId = link.Id,
               LinkId = link.Link.Id,
               Url = link.Link.Url,
               StartIndex = link.StartIndex,
               EndIndex = link.EndIndex
            });
         }
         return View ("Edit", viewModel);
      }

      /// <summary>
      /// Shows the edit page for styles.
      /// </summary>
      /// <param name="id">Id of item to edit.</param>
      /// <returns>The edit page.</returns>
      [Authorize (Roles = "Editor")]
      public ActionResult EditEntry (int id)
      {
         var viewModel = new LinkEditViewModel { ItemId = id, ItemType = "Entry", NextEntryId = -1, PreviousEntryId = -1 };
         var entry = this.m_Db.GlossaryEntries.Get (id);
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
         {
            viewModel.Links.Add (new LinkViewModel
            {
               ItemId = link.Id,
               LinkId = link.Link.Id,
               Url = link.Link.Url,
               StartIndex = link.StartIndex,
               EndIndex = link.EndIndex
            });
         }
         return View ("Edit", viewModel);
      }

      /// <summary>
      /// Shows the edit page for styles.
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
            viewModel.Links.Add (new LinkViewModel
            {
               ItemId = link.Id,
               LinkId = link.Link.Id,
               Url = link.Link.Url,
               Name = link.Name
            });
         }
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
      /// <returns>Results</returns>
      [HttpPost]
      [ValidateAntiForgeryToken]
      [Authorize (Roles = "Editor")]
      public ActionResult Create (int itemId, string itemType, int startIndex, int endIndex, bool openInNewWindow,
         string linkUrl, string linkName)
      {
         var link = GetLink (linkUrl);
         if (itemType.ToLower () == "passage")
         {
            var passage = this.m_Db.PassageEntries.Get (itemId);
            var pLink = new PassageLink { Link = link, StartIndex = startIndex, EndIndex = endIndex, OpenInNewWindow = openInNewWindow };
            passage.Passage.Links.Add (pLink);
            this.m_Db.Save ();
            return Json (new { id = pLink.Id, startIndex, endIndex, linkUrl });
         }
         if (itemType.ToLower () == "entry")
         {
            var entry = this.m_Db.GlossaryEntries.Get (itemId);
            var eLink = new GlossaryEntryLink { Link = link, StartIndex = startIndex, EndIndex = endIndex, OpenInNewWindow = openInNewWindow };
            entry.Links.Add (eLink);
            this.m_Db.Save ();
            return Json (new { id = eLink.Id, startIndex, endIndex, linkUrl });
         }
         if (itemType.ToLower () == "seealso")
         {
            var term = this.m_Db.GlossaryTerms.Get (itemId);
            var tLink = new GlossarySeeAlso { Link = link, Term = term, Name = linkName };
            term.SeeAlsos.Add (tLink);
            this.m_Db.Save ();
            return Json (new { id = tLink.Id, startIndex, endIndex, linkUrl });
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
      /// Deletes a style for the given item.
      /// </summary>
      /// <param name="id">Id of style to get.</param>
      /// <param name="itemId">Id of parent item.</param>
      /// <param name="itemType">Type of parent item.</param>
      /// <returns>Results.</returns>
      [HttpPost]
      [ValidateAntiForgeryToken]
      [Authorize (Roles = "Editor")]
      public ActionResult Delete (int id, int itemId, string itemType)
      {
         if (itemType.ToLower () == "passage")
         {
            var passage = this.m_Db.PassageEntries.Get (itemId);
            var pLink = passage.Passage.Links.FirstOrDefault (s => s.Id == id);
            if (pLink == null)
               return Json ("No style to delete.");
            passage.Passage.Links.Remove (pLink);
         }
         else if (itemType.ToLower () == "entry")
         {
            var entry = this.m_Db.GlossaryEntries.Get (itemId);
            var eLink = entry.Links.FirstOrDefault (s => s.Id == id);
            if (eLink == null)
               return Json ("No style to delete.");
            entry.Links.Remove (eLink);
         }
         else if (itemType.ToLower () == "seealso")
         {
            var term = this.m_Db.GlossaryTerms.Get (itemId);
            var tLink = term.SeeAlsos.FirstOrDefault (s => s.Id == id);
            if (tLink == null)
               return Json ("No style to delete.");
            term.SeeAlsos.Remove (tLink);
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
      /// Gets a style for the given item.
      /// </summary>
      /// <param name="id">Id of style to get.</param>
      /// <param name="itemId">Id of parent item.</param>
      /// <param name="itemType">Type of parent item.</param>
      /// <returns>Results.</returns>
      public ActionResult Get (int id, int itemId, string itemType)
      {
         if (itemType.ToLower () == "passage")
         {
            var passage = this.m_Db.PassageEntries.Get (itemId);
            var pLink = passage.Passage.Links.FirstOrDefault (s => s.Id == id);
            if (pLink != null)
               return Json (new
               {
                  id = pLink.Id,
                  startIndex = pLink.StartIndex,
                  endIndex = pLink.EndIndex,
                  url = pLink.Link.Url
               }, JsonRequestBehavior.AllowGet);
         }
         else if (itemType.ToLower () == "entry")
         {
            var entry = this.m_Db.GlossaryEntries.Get (itemId);
            var eLink = entry.Links.FirstOrDefault (s => s.Id == id);
            if (eLink != null)
               return Json (new
               {
                  id = eLink.Id,
                  startIndex = eLink.StartIndex,
                  endIndex = eLink.EndIndex,
                  url = eLink.Link.Url
               }, JsonRequestBehavior.AllowGet);
         }
         else if (itemType.ToLower () == "seealso")
         {
            var term = this.m_Db.GlossaryTerms.Get (itemId);
            var tLink = term.SeeAlsos.FirstOrDefault (s => s.Id == id);
            if (tLink != null)
               return Json (new
               {
                  id = tLink.Id,
                  name = tLink.Name,
                  url = tLink.Link.Url
               }, JsonRequestBehavior.AllowGet);
         }

         Response.StatusCode = 500;
         return Json ("Invalid Data.", JsonRequestBehavior.AllowGet);
      }

      /// <summary>
      /// Renders the style for the given item.
      /// </summary>
      /// <param name="itemId">Id of parent item.</param>
      /// <param name="itemType">Type of parent item.</param>
      /// <returns>Results.</returns>
      public ActionResult Render (int itemId, string itemType)
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

      /// <summary>
      /// Gets the links for the given item id.
      /// </summary>
      /// <param name="id">Id of item to get links for.</param>
      /// <returns>JSON object with links.</returns>
      [AllowAnonymous]
      public ActionResult GetLinksForEntry (int id)
      {
         var entry = this.m_Db.PassageEntries.Get (id);
         var result = new
         {
            entryId = id,
            passageText = entry.Passage.Text,
            links = entry.Passage.Links.Select (l => new
            {
               id = l.Id,
               url = l.Link.Url,
               linkId = l.Link.Id,
               endIndex = l.EndIndex,
               startIndex = l.StartIndex,
               openInNewWindow = l.OpenInNewWindow
            })
         };
         return Json (result, JsonRequestBehavior.AllowGet);
      }
   }
}
