using System.Linq;
using System.Web.Mvc;
using SeekDeepWithin.DataAccess;
using SeekDeepWithin.Domain;
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
      /// Gets the add link view.
      /// </summary>
      /// <param name="id">Id of the passage to add a link for.</param>
      /// <returns>The add link view.</returns>
      [Authorize (Roles = "Editor")]
      public ActionResult CreateForPassage (int id)
      {
         if (Request.UrlReferrer != null) TempData["RefUrl"] = Request.UrlReferrer.ToString ();
         var passage = this.m_Db.Passages.Get (id);
         return View (new EditLinkViewModel { Id = id, Text = passage.Text });
      }

      /// <summary>
      /// Gets the add link view.
      /// </summary>
      /// <param name="id">Id of the item to add a link for.</param>
      /// <param name="type">The type of object we are adding links for.</param>
      /// <returns>The add link view.</returns>
      [Authorize (Roles = "Editor")]
      public ActionResult CreateForVersion (int id, string type)
      {
         if (Request.UrlReferrer != null) TempData["RefUrl"] = Request.UrlReferrer.ToString ();
         var version = this.m_Db.Versions.Get (id);
         return View (new EditLinkViewModel { Id = id, Text = version.About });
      }

      /// <summary>
      /// Posts a new link for the given passage.
      /// </summary>
      /// <param name="viewModel"></param>
      /// <returns></returns>
      [HttpPost]
      [ValidateAntiForgeryToken]
      [Authorize (Roles = "Editor")]
      public ActionResult CreateForPassage (EditLinkViewModel viewModel)
      {
         if (!ModelState.IsValid)
            return View (viewModel);

         var linkUrl = this.GetLinkUrl (viewModel);
         if (string.IsNullOrEmpty (linkUrl))
            return View (viewModel);

         if (!string.IsNullOrWhiteSpace (viewModel.Anchor))
            linkUrl += "#" + viewModel.Anchor;

         var link = GetLink (linkUrl);
         var passage = this.m_Db.Passages.Get (viewModel.Id);

         passage.PassageLinks.Add (new PassageLink
         {
            Passage = passage,
            Link = link,
            OpenInNewWindow = viewModel.OpenInNewWindow,
            StartIndex = viewModel.StartIndex,
            EndIndex = viewModel.EndIndex
         });
         this.m_Db.Save ();
         return RedirectToAction ("Details", "Passage", new { id = viewModel.Id });
      }

      /// <summary>
      /// Posts a new link for the given version.
      /// </summary>
      /// <param name="viewModel"></param>
      /// <returns></returns>
      [HttpPost]
      [ValidateAntiForgeryToken]
      [Authorize (Roles = "Editor")]
      public ActionResult CreateForVersion (EditLinkViewModel viewModel)
      {
         if (!ModelState.IsValid)
            return View (viewModel);

         var linkUrl = this.GetLinkUrl (viewModel);
         if (string.IsNullOrEmpty (linkUrl))
            return View (viewModel);

         if (!string.IsNullOrWhiteSpace (viewModel.Anchor))
            linkUrl += "#" + viewModel.Anchor;

         var link = GetLink (linkUrl);
         var version = this.m_Db.Versions.Get (viewModel.Id);
         version.VersionAboutLinks.Add (new VersionAboutLink
         {
            Vesion = version,
            Link = link,
            OpenInNewWindow = viewModel.OpenInNewWindow,
            StartIndex = viewModel.StartIndex,
            EndIndex = viewModel.EndIndex
         });
         this.m_Db.Save ();
         return RedirectToAction ("About", "Version", new { id = viewModel.Id });
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

      private string GetLinkUrl (EditLinkViewModel viewModel)
      {
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
         else if (!string.IsNullOrWhiteSpace (viewModel.Book) && !string.IsNullOrWhiteSpace (viewModel.Version)) {}
         return linkUrl;
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
