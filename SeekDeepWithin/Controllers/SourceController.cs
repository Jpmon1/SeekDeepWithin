using System.Linq;
using System.Web.Mvc;
using SeekDeepWithin.DataAccess;
using SeekDeepWithin.Domain;
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
         return View ();
      }

      /// <summary>
      /// Gets the edit source view.
      /// </summary>
      /// <returns>Edit source view.</returns>
      [Authorize (Roles = "Editor")]
      public ActionResult EditSource (int id, string type)
      {
         if (Request.UrlReferrer != null) TempData["RefUrl"] = Request.UrlReferrer.ToString ();

         var version = this.m_Db.Versions.Get (id);
         var source = version.VersionSources.FirstOrDefault ();

         return View (new SourceViewModel
         {
            Name = source == null ? string.Empty : source.Source.Name,
            Url = source == null ? string.Empty : source.Source.Url,
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
            var version = this.m_Db.Versions.Get (viewModel.VersionId);
            var source = this.GetSource (viewModel.SourceName, viewModel.SourceUrl);
            if (version.VersionSources.Count == 1 && version.VersionSources.First ().Source.Id == source.Id)
               this.m_Db.Save ();
            else
            {
               version.VersionSources.Clear ();
               version.VersionSources.Add (new VersionSource { Source = source, Version = version });
               this.m_Db.Save ();
            }
            return RedirectToAction ("About", "Version", new { id = viewModel.VersionId });
         }
         return View (viewModel);
      }

      /// <summary>
      /// Gets the source for the given view model.
      /// </summary>
      /// <param name="name">The name of the source.</param>
      /// <param name="url">The url of the source.</param>
      /// <returns>The requested source.</returns>
      private Source GetSource (string name, string url)
      {
         var source = this.m_Db.Sources.Get (s => s.Name == name && s.Url == url).FirstOrDefault ();
         if (source == null)
         {
            source = new Source { Name = name, Url = url };
            this.m_Db.Sources.Insert (source);
            this.m_Db.Save ();
         }
         return source;
      }
   }
}
