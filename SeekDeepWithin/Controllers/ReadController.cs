using System.Web.Mvc;
using SeekDeepWithin.DataAccess;
using SeekDeepWithin.Models;

namespace SeekDeepWithin.Controllers
{
   public class ReadController : Controller
   {
      private readonly ISdwDatabase m_Db;

      /// <summary>
      /// Initializes a new controller.
      /// </summary>
      public ReadController ()
      {
         this.m_Db = new SdwDatabase ();
      }

      /// <summary>
      /// Initializes a new controller with the given db info.
      /// </summary>
      /// <param name="db">Database object.</param>
      public ReadController (ISdwDatabase db)
      {
         this.m_Db = db;
      }

      /// <summary>
      /// Gets a read page.
      /// </summary>
      /// <param name="id">ID of chapter to read.</param>
      /// <returns>The read view.</returns>
      public ActionResult Index (int id)
      {
         var chapter = this.m_Db.SubBookChapters.Get (id);
         return View (new ChapterViewModel (chapter));
      }
   }
}
