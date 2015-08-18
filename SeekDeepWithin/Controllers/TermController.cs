using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using SeekDeepWithin.DataAccess;
using SeekDeepWithin.Models;
using SeekDeepWithin.Pocos;
using SeekDeepWithin.SdwSearch;

namespace SeekDeepWithin.Controllers
{
   public class TermController : SdwController
   {
      /// <summary>
      /// Initializes a new controller.
      /// </summary>
      public TermController () : base (new SdwDatabase ()) { }

      /// <summary>
      /// Initializes a new controller with the given db info.
      /// </summary>
      /// <param name="db">Database object.</param>
      public TermController (ISdwDatabase db) : base (db) { }

      /// <summary>
      /// 
      /// </summary>
      /// <returns>The term view</returns>
      public ActionResult Index (int id)
      {
         var term = this.Database.Terms.Get (id);
         var viewModel = new TermViewModel (term);
         foreach (var termLink in term.Links)
         {
            if (termLink.LinkType == (int)TermLinkType.Book)
            {
               var book = this.Database.Books.Get (termLink.RefId);
               viewModel.Book = new BookViewModel (book);
            }
         }
         return View (viewModel);
      }

      /// <summary>
      /// Gets the create new glossary item view.
      /// </summary>
      /// <returns>Create new glossary item view.</returns>
      [Authorize (Roles = "Creator")]
      public ActionResult Create ()
      {
         return View ();
      }

      /// <summary>
      /// Creates the given glossary item.
      /// </summary>
      /// <returns>Create new glossary item view.</returns>
      [HttpPost]
      [ValidateAntiForgeryToken]
      [Authorize (Roles = "Creator")]
      public ActionResult Create (string name)
      {
         if (string.IsNullOrWhiteSpace (name))
            this.Fail ("You must supply a name for the new term.");
         var foundTerm = this.Database.Terms.Get (t => t.Name == name).FirstOrDefault ();
         if (foundTerm != null)
            return this.Fail ("A term with that name already exists.");
         var term = new Term { Name = name, Modified = DateTime.Now };
         this.Database.Terms.Insert (term);
         this.Database.Save ();
         TermSearch.AddOrUpdateIndex(term);
         return Json (new { status = SUCCESS, id = term.Id, name });
      }

      /// <summary>
      /// Edits a glossary term.
      /// </summary>
      /// <param name="id">The id of the term to edit.</param>
      /// <returns></returns>
      [Authorize (Roles = "Editor")]
      public ActionResult Edit (int id)
      {
         return View (new TermViewModel (this.Database.Terms.Get (id)));
      }

      /// <summary>
      /// Edits a glossary term.
      /// </summary>
      /// <param name="id">The id of the term to edit.</param>
      /// <param name="name">The new name of the term.</param>
      /// <returns></returns>
      [HttpPost]
      [ValidateAntiForgeryToken]
      [Authorize (Roles = "Editor")]
      public ActionResult Edit (int id, string name)
      {
         var term = this.Database.Terms.Get (id);
         if (term == null) return this.Fail ("Unable to determine the term.");
         term.Name = name;
         this.Database.Save ();
         TermSearch.AddOrUpdateIndex (term);
         return this.Success();
      }

      /// <summary>
      /// Adds the given tag to the given term.
      /// </summary>
      /// <param name="linkId">Id of tag to add.</param>
      /// <param name="id">Id of term to add tag to.</param>
      /// <returns></returns>
      [HttpPost]
      [ValidateAntiForgeryToken]
      [Authorize (Roles = "Editor")]
      public ActionResult AddTag (int id, int linkId)
      {
         var term = this.Database.Terms.Get (id);
         if (term == null) return this.Fail ("Unable to determine the term.");
         var tag = this.Database.Terms.Get (linkId);
         if (tag == null) return this.Fail ("Unable to determine the tag.");
         var foundTag = term.Links.FirstOrDefault (l => l.RefId == linkId && l.LinkType == (int)TermLinkType.Tag);
         if (foundTag != null) return this.Fail ("That tag is already assigned to the term.");

         if (term.Links == null) term.Links = new Collection<TermLink> ();
         if (tag.Links == null) tag.Links = new Collection<TermLink> ();
         var link = new TermLink {LinkType = (int) TermLinkType.Tag, RefId = tag.Id, Name = tag.Name};
         term.Links.Add (link);
         var linked = new TermLink {LinkType = (int) TermLinkType.Tagged, RefId = term.Id, Name = term.Name};
         tag.Links.Add (linked);
         this.Database.Save ();
         return Json (new { status = SUCCESS, id = link.Id, linkedId = linked.Id, termId = id, refId = linkId, name = tag.Name });
      }

      /// <summary>
      /// Removes the tag with the given id from the given term.
      /// </summary>
      /// <param name="linkId">Id of tag to remove.</param>
      /// <param name="id">Id of term to remove tag from.</param>
      /// <returns></returns>
      [HttpPost]
      [ValidateAntiForgeryToken]
      [Authorize (Roles = "Editor")]
      public ActionResult RemoveTag (int id, int linkId)
      {
         var term = this.Database.Terms.Get (id);
         if (term == null) return this.Fail ("Unable to determine the term.");
         var tag = this.Database.Terms.Get (id);
         if (tag == null) return this.Fail ("Unable to determine the tag.");

         var tagLink = term.Links.FirstOrDefault (l => l.Id == linkId);
         if (tagLink == null) return this.Fail ("Unable to find the given tag.");
         var taggedLink = tag.Links.FirstOrDefault (l => l.RefId == id && l.LinkType == (int)TermLinkType.Tagged);
         if (taggedLink == null) return this.Fail ("Unable to find the given tag.");

         term.Links.Remove (tagLink);
         tag.Links.Remove (taggedLink);
         this.Database.Save ();
         return this.Success();
      }

      /// <summary>
      /// Assigns a writer to a term.
      /// </summary>
      /// <returns>The result.</returns>
      [HttpPost]
      [ValidateAntiForgeryToken]
      [Authorize (Roles = "Editor")]
      public ActionResult AddWriter (int id, int linkId)
      {
         var term = this.Database.Terms.Get (id);
         if (term == null) return this.Fail ("Unable to determine the term.");
         var writer = this.Database.Terms.Get (linkId);
         if (writer == null) return this.Fail ("Unable to determine the writer.");
         var foundWriter = term.Links.FirstOrDefault (l => l.RefId == linkId && l.LinkType == (int)TermLinkType.Writer);
         if (foundWriter != null) return this.Fail ("That writer is already assigned to the term.");

         if (term.Links == null) term.Links = new Collection<TermLink> ();
         if (writer.Links == null) writer.Links = new Collection<TermLink> ();
         var link = new TermLink {LinkType = (int) TermLinkType.Writer, RefId = linkId, Name = writer.Name};
         term.Links.Add (link);
         var linked = new TermLink {LinkType = (int) TermLinkType.Written, RefId = id, Name = term.Name};
         writer.Links.Add (linked);

         this.Database.Save ();
         return Json (new { status = SUCCESS, id = link.Id, linkedId = linked.Id, termId = id, refId = linkId, name = writer.Name });
      }

      /// <summary>
      /// Removes a writer from a tern.
      /// </summary>
      /// <returns>The result.</returns>
      [HttpPost]
      [ValidateAntiForgeryToken]
      [Authorize (Roles = "Editor")]
      public ActionResult RemoveWriter (int id, int linkId)
      {
         var term = this.Database.Terms.Get (id);
         if (term == null) return this.Fail ("Unable to determine the term.");
         var writer = this.Database.Terms.Get (id);
         if (writer == null) return this.Fail ("Unable to determine the writer.");

         var writerLink = term.Links.FirstOrDefault (l => l.Id == linkId);
         if (writerLink == null) return this.Fail ("Unable to find the given writer.");
         var writtenLink = writer.Links.FirstOrDefault (l => l.RefId == id && l.LinkType == (int)TermLinkType.Written);
         if (writtenLink == null) return this.Fail ("Unable to find the given writer.");

         term.Links.Remove (writerLink);
         writer.Links.Remove (writtenLink);
         this.Database.Save ();
         return this.Success();
      }

      /// <summary>
      /// Adds a see also to a term.
      /// </summary>
      /// <returns>The result.</returns>
      [HttpPost]
      [ValidateAntiForgeryToken]
      [Authorize (Roles = "Editor")]
      public ActionResult AddSeeAlso (int id, int linkId)
      {
         var term = this.Database.Terms.Get (id);
         if (term == null) return this.Fail ("Unable to determine the term.");
         var seeAlso = this.Database.Terms.Get (linkId);
         if (seeAlso == null) return this.Fail ("Unable to determine the writer.");
         var foundSeeAlso = term.Links.FirstOrDefault (l => l.RefId == linkId && l.LinkType == (int)TermLinkType.SeeAlso);
         if (foundSeeAlso != null) return this.Fail ("That see also is already assigned to the term.");

         if (term.Links == null) term.Links = new Collection<TermLink> ();
         var link = new TermLink {LinkType = (int) TermLinkType.SeeAlso, RefId = linkId, Name = seeAlso.Name};
         term.Links.Add (link);
         this.Database.Save ();
         return Json (new { status = SUCCESS, id = link.Id, termId = id, refId = linkId, name = seeAlso.Name });
      }

      /// <summary>
      /// Removes a see also from a term.
      /// </summary>
      /// <returns>The result.</returns>
      [HttpPost]
      [ValidateAntiForgeryToken]
      [Authorize (Roles = "Editor")]
      public ActionResult RemoveSeeAlso (int id, int linkId)
      {
         var term = this.Database.Terms.Get (id);
         if (term == null) return this.Fail ("Unable to determine the term.");
         var seeAlso = this.Database.Terms.Get (id);
         if (seeAlso == null) return this.Fail ("Unable to determine the writer.");

         var seeAlsoLink = term.Links.FirstOrDefault (l => l.Id == linkId);
         if (seeAlsoLink == null) return this.Fail ("Unable to find the given writer.");

         term.Links.Remove (seeAlsoLink);
         this.Database.Save ();
         return this.Success();
      }

      /// <summary>
      /// Gets the list of terms.
      /// </summary>
      /// <returns>A JSON result.</returns>
      public ActionResult List (int? start, string filter)
      {
         var terms = this.Database.Terms.All (q => q.OrderBy (t => t.Name))
            .Where(t => string.IsNullOrEmpty (filter) || t.Name.Contains(filter))
            .Skip(start ?? 0).Take(25).ToList ();
         return Json (
            new
            {
               status = SUCCESS,
               count = terms.Count,
               terms = terms.Select (t => new
               {
                  id = t.Id,
                  name = t.Name,
                  modified = t.Modified.ToString (CultureInfo.InvariantCulture)
               })
            }, JsonRequestBehavior.AllowGet);
      }

      /// <summary>
      /// Gets auto complete items for the given term.
      /// </summary>
      /// <param name="term">Term to get auto complete items for.</param>
      /// <returns>The list of possible terms for the given item.</returns>
      public ActionResult AutoComplete (string term)
      {
         var result = new
         {
            suggestions = this.Database.Terms.Get (t => t.Name.Contains (term)).Select (t => new { value = t.Name, data = t.Id })
         };
         return Json (result, JsonRequestBehavior.AllowGet);
      }
   }
}
