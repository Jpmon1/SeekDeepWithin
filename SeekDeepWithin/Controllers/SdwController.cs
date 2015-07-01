using System.Web.Mvc;
using SeekDeepWithin.DataAccess;

namespace SeekDeepWithin.Controllers
{
   public abstract class SdwController : Controller
   {
      private readonly ISdwDatabase m_Db;

      /// <summary>
      /// Initializes a new sdw controller.
      /// </summary>
      /// <param name="db"></param>
      protected SdwController (ISdwDatabase db)
      {
         this.m_Db = db;
      }

      /// <summary>
      /// Gets the database connection.
      /// </summary>
      protected ISdwDatabase Database { get { return this.m_Db; } }

      /// <summary>
      /// Returns a failed JSON response with the given message.
      /// </summary>
      /// <param name="message">Message to return.</param>
      /// <returns>JSON response.</returns>
      protected ActionResult Fail (string message = "Error")
      {
         Response.StatusCode = 500;
         return Json (message, JsonRequestBehavior.AllowGet);
      }
   }
}