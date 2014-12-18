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
