using System;
using System.Web.Mvc;
using SeekDeepWithin.Models;

namespace SeekDeepWithin.Controllers
{
   public class ErrorController : Controller
   {
      /// <summary>
      /// Gets the error page.
      /// </summary>
      /// <param name="statusCode">Error code.</param>
      /// <param name="exception">Error exception.</param>
      /// <param name="isAjaxRequet">True if ajax error.</param>
      /// <returns>The error view.</returns>
      public ActionResult Index (int statusCode, Exception exception, bool isAjaxRequet)
      {
         Response.StatusCode = statusCode;

         // If it's not an AJAX request that triggered this action then just retun the view
         if (!isAjaxRequet)
         {
            var model = new ErrorViewModel { HttpStatusCode = statusCode, Exception = exception };
            return View (model);
         }

         // Otherwise, if it was an AJAX request, return an anon type with the message from the exception
         var errorObjet = new { message = exception.Message };
         return Json (errorObjet, JsonRequestBehavior.AllowGet);
      }

   }
}
