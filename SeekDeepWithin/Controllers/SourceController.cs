using System.Linq;
using System.Web.Mvc;
using SeekDeepWithin.DataAccess;
using SeekDeepWithin.Pocos;
using SeekDeepWithin.Models;

namespace SeekDeepWithin.Controllers
{
   /// <summary>
   /// Controller for sources.
   /// </summary>
   public class SourceController : Controller
   {
      private readonly ISdwDatabase m_Db;

      /// <summary>
      /// Initializes a new controller.
      /// </summary>
      public SourceController ()
      {
         this.m_Db = new SdwDatabase ();
      }

      /// <summary>
      /// Initializes a new controller with the given db info.
      /// </summary>
      /// <param name="db">Database object.</param>
      public SourceController (ISdwDatabase db)
      {
         this.m_Db = db;
      }

      /// <summary>
      /// Gets the view for the list of sources.
      /// </summary>
      /// <returns>List of sources view.</returns>
      public ActionResult Index ()
      {
         return View (this.m_Db.Sources.All().Select(s => new SourceViewModel { Id = s.Id, Name = s.Name, Url = s.Url }));
      }

      /// <summary>
      /// Gets the source edit view.
      /// </summary>
      /// <param name="id">Id of source to edit.</param>
      /// <returns>The source edit view.</returns>
      [Authorize (Roles = "Editor")]
      public ActionResult Edit (int id)
      {
         var source = this.m_Db.Sources.Get (id);
         if (Request.UrlReferrer != null) TempData["RefUrl"] = Request.UrlReferrer.ToString ();
         return View (new SourceViewModel { Id = source.Id, Name = source.Name, Url = source.Url });
      }

      /// <summary>
      /// Posts the source edits.
      /// </summary>
      /// <param name="viewModel">Source view model with edits.</param>
      /// <returns>The source index page.</returns>
      [HttpPost]
      [ValidateAntiForgeryToken]
      [Authorize (Roles = "Editor")]
      public ActionResult Edit (SourceViewModel viewModel)
      {
         if (ModelState.IsValid)
         {
            var source = this.m_Db.Sources.Get (viewModel.Id);
            this.m_Db.SetValues (source, viewModel);
            this.m_Db.Save ();
            return RedirectToAction ("Index");
         }
         return View (viewModel);
      }

      /// <summary>
      /// Gets the edit source view.
      /// </summary>
      /// <returns>Edit source view.</returns>
      [Authorize (Roles = "Editor")]
      public ActionResult EditSource (int id, string type, int? parentId)
      {
         if (Request.UrlReferrer != null) TempData["RefUrl"] = Request.UrlReferrer.ToString ();

         Source source = null;
         if (type == "version")
         {
            var version = this.m_Db.Versions.Get (id);
            var s = version.VersionSources.FirstOrDefault ();
            if (s != null)
               source = s.Source;
         }
         else if (type == "entry")
         {
            var version = this.m_Db.GlossaryItems.Get (id);
            var s = version.Sources.FirstOrDefault ();
            if (s != null)
               source = s.Source;
         }

         return View (new SourceViewModel
         {
            Name = source == null ? string.Empty : source.Name,
            Url = source == null ? string.Empty : source.Url,
            ParentId = parentId ?? -1,
            Type = type,
            Id = id
         });
      }

      /// <summary>
      /// Posts the edit source updates.
      /// </summary>
      /// <returns>Version about view.</returns>
      [HttpPost]
      [ValidateAntiForgeryToken]
      [Authorize (Roles = "Editor")]
      public ActionResult EditSource (SourceViewModel viewModel)
      {
         if (ModelState.IsValid)
         {
            var source = GetSource (viewModel.Name, viewModel.Url, this.m_Db);
            if (viewModel.Type == "version")
            {
               var version = this.m_Db.Versions.Get (viewModel.Id);
               if (version.VersionSources.Count == 1 && version.VersionSources.First ().Source.Id == source.Id)
                  this.m_Db.Save ();
               else
               {
                  if (version.VersionSources.Count == 0)
                     version.VersionSources.Add (new VersionSource { Version = version });
                  version.VersionSources.First ().Source = source;
                  this.m_Db.Save ();
               }
               return RedirectToAction ("Index", "Read", new {id = version.DefaultReadChapter});
            }
            if (viewModel.Type == "entry")
            {
               var entry = this.m_Db.GlossaryItems.Get (viewModel.Id);
               if (entry.Sources.Count == 1 && entry.Sources.First ().Source.Id == source.Id)
                  this.m_Db.Save ();
               else
               {
                  if (entry.Sources.Count == 0)
                     entry.Sources.Add (new GlossaryItemSource { GlossaryItem = entry });
                  entry.Sources.First ().Source = source;
                  this.m_Db.Save ();
               }
               return RedirectToAction ("Term", "Glossary", new { id = viewModel.ParentId });
            }
         }
         return View (viewModel);
      }

      /// <summary>
      /// Gets the source for the given view model.
      /// </summary>
      /// <param name="name">The name of the source.</param>
      /// <param name="url">The url of the source.</param>
      /// <param name="db">The database to use for getting the source.</param>
      /// <returns>The requested source.</returns>
      public static Source GetSource (string name, string url, ISdwDatabase db)
      {
         var source = db.Sources.Get (s => s.Url == url).FirstOrDefault ();
         if (source == null)
         {
            source = new Source { Name = name, Url = url };
            db.Sources.Insert (source);
            db.Save ();
         }
         else if (source.Name != name)
         {
            source.Name = name;
            db.Save ();
         }
         return source;
      }
   }
}
