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
      /// Posts a new link for the given passage.
      /// </summary>
      /// <param name="viewModel"></param>
      /// <returns></returns>
      [HttpPost]
      [ValidateAntiForgeryToken]
      [Authorize (Roles = "Editor")]
      public ActionResult CreatePassage (EditLinkViewModel viewModel)
      {
         var linkUrl = this.GetLinkUrl (viewModel);
         if (string.IsNullOrEmpty (linkUrl))
         {
            Response.StatusCode = 500;
            return Json ("Unable to determine the links url.");
         }

         if (!string.IsNullOrWhiteSpace (viewModel.Anchor))
            linkUrl += "#" + viewModel.Anchor;

         var link = GetLink (linkUrl);
         var passage = this.m_Db.Passages.Get (viewModel.LinkItemId);

         passage.PassageLinks.Add (new PassageLink
         {
            Passage = passage,
            Link = link,
            OpenInNewWindow = viewModel.OpenInNewWindow,
            StartIndex = viewModel.StartIndex,
            EndIndex = viewModel.EndIndex
         });
         this.m_Db.Save ();
         return Json ("Success!");
      }

      /// <summary>
      /// Posts a new link for the given passage.
      /// </summary>
      /// <param name="viewModel"></param>
      /// <returns></returns>
      [HttpPost]
      [ValidateAntiForgeryToken]
      [Authorize (Roles = "Editor")]
      public ActionResult CreateEntry (EditLinkViewModel viewModel)
      {
         if (!ModelState.IsValid)
         {
            Response.StatusCode = 500;
            return Json ("Data is not Valid");
         }

         var linkUrl = this.GetLinkUrl (viewModel);
         if (string.IsNullOrEmpty (linkUrl))
         {
            Response.StatusCode = 500;
            return Json ("Unable to determine the links url.");
         }

         if (!string.IsNullOrWhiteSpace (viewModel.Anchor))
            linkUrl += "#" + viewModel.Anchor;

         var link = GetLink (linkUrl);
         var glossaryEntry = this.m_Db.GlossaryEntries.Get (viewModel.LinkItemId);

         glossaryEntry.Links.Add (new GlossaryEntryLink
         {
            Entry = glossaryEntry,
            Link = link,
            OpenInNewWindow = viewModel.OpenInNewWindow,
            StartIndex = viewModel.StartIndex,
            EndIndex = viewModel.EndIndex
         });
         this.m_Db.Save ();
         return Json ("Success!");
      }

      /// <summary>
      /// Posts a new link for the given passage.
      /// </summary>
      /// <param name="viewModel"></param>
      /// <returns></returns>
      [HttpPost]
      [ValidateAntiForgeryToken]
      [Authorize (Roles = "Editor")]
      public ActionResult CreateSeeAlso (EditLinkViewModel viewModel)
      {
         if (!ModelState.IsValid)
         {
            Response.StatusCode = 500;
            return Json ("Data is not Valid");
         }

         var linkUrl = this.GetLinkUrl (viewModel);
         if (string.IsNullOrEmpty (linkUrl))
         {
            Response.StatusCode = 500;
            return Json ("Unable to determine the links url.");
         }

         if (!string.IsNullOrWhiteSpace (viewModel.Anchor))
            linkUrl += "#" + viewModel.Anchor;

         var link = GetLink (linkUrl);
         var glossaryTerm = this.m_Db.GlossaryTerms.Get (viewModel.LinkItemId);

         glossaryTerm.SeeAlsos.Add (new GlossarySeeAlso
         {
            Term = glossaryTerm,
            Link = link,
            Name = GetLinkName(viewModel)
         });
         this.m_Db.Save ();
         return Json ("Success!");
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
            link = new Link {Url = linkUrl};
            this.m_Db.Links.Insert (link);
            this.m_Db.Save ();
         }
         return link;
      }

      private static string GetLinkName (EditLinkViewModel viewModel)
      {
         var linkName = string.Empty;
         if (!string.IsNullOrWhiteSpace (viewModel.Search))
            linkName = viewModel.Link;
         else if (!string.IsNullOrWhiteSpace (viewModel.Link))
            linkName = viewModel.Link;
         else if (!string.IsNullOrWhiteSpace (viewModel.GlossaryTerm))
            linkName = viewModel.GlossaryTerm;
         else if (!string.IsNullOrWhiteSpace (viewModel.Book) && !string.IsNullOrWhiteSpace (viewModel.Version))
         {
            linkName = viewModel.Version;
            if (!string.IsNullOrEmpty (viewModel.SubBook) && viewModel.SubBook != viewModel.Version)
               linkName += " " + viewModel.SubBook;
            if (!string.IsNullOrEmpty (viewModel.Chapter) && viewModel.Chapter != viewModel.SubBook)
               linkName += " " + viewModel.Chapter;
         }
         return linkName;
      }

      private string GetLinkUrl (EditLinkViewModel viewModel)
      {
         var host = Request.Url == null ? string.Empty : Request.Url.AbsoluteUri.Replace (Request.Url.AbsolutePath, "");
         var linkUrl = string.Empty;
         if (!string.IsNullOrWhiteSpace (viewModel.Search))
            linkUrl = viewModel.Link;
         else if (!string.IsNullOrWhiteSpace (viewModel.Link))
         {
            linkUrl = viewModel.Link;
            viewModel.OpenInNewWindow = true;
         }
         else if (!string.IsNullOrWhiteSpace (viewModel.GlossaryTerm))
         {
            var term = this.m_Db.GlossaryTerms.Get (t => t.Name == viewModel.GlossaryTerm).FirstOrDefault ();
            if (term == null)
            {
               term = new GlossaryTerm {Name = viewModel.GlossaryTerm};
               this.m_Db.GlossaryTerms.Insert (term);
               this.m_Db.Save ();
            }
            linkUrl = "/Glossary/Term/" + term.Id;
         }
         else if (viewModel.ChapterId != 0)
         {
            linkUrl = "/Chapter/Read/" + viewModel.ChapterId;
         }
         return host + linkUrl;
      }

      /// <summary>
      /// Gets the links for the given entry id.
      /// </summary>
      /// <param name="id">Id of entry to get links for.</param>
      /// <returns>JSON object with links.</returns>
      [AllowAnonymous]
      public ActionResult GetLinksForEntry (int id)
      {
         var entry = this.m_Db.PassageEntries.Get (id);
         var result = new
         {
            entryId = id,
            passageText = entry.Passage.Text,
            links = entry.Passage.PassageLinks.Select(l => new
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
