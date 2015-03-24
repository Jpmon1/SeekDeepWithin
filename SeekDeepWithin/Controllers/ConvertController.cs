using System;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace SeekDeepWithin.Controllers
{
   /// <summary>
   /// Controller used to import information.
   /// </summary>
   public class ConvertController : Controller
   {
      /// <summary>
      /// Converts the new lines in the given text to spaces.
      /// </summary>
      /// <param name="text">Text to convert.</param>
      /// <returns>Converted text.</returns>
      [HttpPost]
      [AllowAnonymous]
      public ActionResult LinesToSpaces (string text)
      {
         var newText = string.Empty;
         if (!string.IsNullOrWhiteSpace(text))
            newText = text.Replace ("\n", " ").Replace ("  ", " ").Trim();
         return Json (new { text = newText });
      }

      /// <summary>
      /// Converts the new lines in the given text to html breaks.
      /// </summary>
      /// <param name="text">Text to convert.</param>
      /// <returns>Converted text.</returns>
      [HttpPost]
      [AllowAnonymous]
      public ActionResult LinesToBreaks (string text)
      {
         var newText = string.Empty;
         if (!string.IsNullOrWhiteSpace (text))
            newText = text.Replace ("\n", "<br />").Trim();
         return Json (new { text = newText });
      }

      /// <summary>
      /// Removes the regex out of the given text.
      /// </summary>
      /// <param name="text">Text to remove from.</param>
      /// <param name="regex">Regex to remove.</param>
      /// <returns>The upated text.</returns>
      [HttpPost]
      [AllowAnonymous]
      public ActionResult RemoveRegex (string text, string regex)
      {
         return ReplaceRegex (text, regex, "");
      }

      /// <summary>
      /// Replaces the regex in the given text.
      /// </summary>
      /// <param name="text">Text to replace in.</param>
      /// <param name="regex">Regex to replace.</param>
      /// <param name="replace">The replacement.</param>
      /// <returns>The upated text.</returns>
      [HttpPost]
      [AllowAnonymous]
      public ActionResult ReplaceRegex (string text, string regex, string replace)
      {
         var newText = string.Empty;
         regex = HttpUtility.UrlDecode (regex);
         if (string.IsNullOrWhiteSpace (regex))
         {
            Response.StatusCode = 500;
            return Json ("The regex cannot be empty.", JsonRequestBehavior.AllowGet);
         }
         if (!string.IsNullOrWhiteSpace (text))
         {
            var r = new Regex (regex);
            newText = r.Replace (text, replace).Trim();
         }
         return Json (new { text = newText });
      }

      /// <summary>
      /// Converts the new lines in the given text to new passages.
      /// </summary>
      /// <param name="text">Text to convert.</param>
      /// <param name="startOrder">The starting order index.</param>
      /// <param name="startNumber">The starting passage number.</param>
      /// <returns>Converted text.</returns>
      [HttpPost]
      [AllowAnonymous]
      public ActionResult LinesToPassages (string text, int? startOrder, int? startNumber)
      {
         if (string.IsNullOrWhiteSpace (text))
         {
            Response.StatusCode = 500;
            return Json ("The text cannot be empty.", JsonRequestBehavior.AllowGet);
         }

         var passages = text.Split (new [] {'\n'}, StringSplitOptions.RemoveEmptyEntries);
         var passageList = new Collection <object> ();
         foreach (var passage in passages)
         {
            passageList.Add (new
            {
               text = passage.Replace ("\"", "&quot;").Trim (),
               number = startNumber,
               order = startOrder
            });
            if (startNumber != null)
               startNumber++;
            if (startOrder != null)
               startOrder++;
         }

         return Json (new { passages = passageList });
      }

      /// <summary>
      /// Converts the the given text to new passages, based on the given regex.
      /// </summary>
      /// <param name="text">Text to convert.</param>
      /// <param name="regex">The regex to use.</param>
      /// <param name="startOrder">The starting order index.</param>
      /// <param name="startNumber">The starting passage number.</param>
      /// <returns>Converted text.</returns>
      [HttpPost]
      [AllowAnonymous]
      public ActionResult RegexToPassages (string text, string regex, int? startOrder, int? startNumber)
      {
         if (string.IsNullOrWhiteSpace (text))
         {
            Response.StatusCode = 500;
            return Json ("The text cannot be empty.", JsonRequestBehavior.AllowGet);
         }

         regex = HttpUtility.UrlDecode (regex);
         if (string.IsNullOrWhiteSpace (regex))
         {
            Response.StatusCode = 500;
            return Json ("The regex cannot be empty.", JsonRequestBehavior.AllowGet);
         }
         var matches = Regex.Matches (text, regex, RegexOptions.IgnoreCase);
         var passageList = new Collection <object> ();
         foreach (Match match in matches)
         {
            var order = match.Groups ["order"];
            var number = match.Groups["number"];
            var chapter = match.Groups["chapter"];
            passageList.Add (new
            {
               text = match.Groups["text"].Value.Replace ("\"", "&quot;").Trim (),
               chapter = chapter.Success ? chapter.Value : "unknown",
               number = number.Success ? Convert.ToInt32(number.Value) : startNumber,
               order = order.Success ? Convert.ToInt32 (order.Value) : startOrder
            });
            if (startNumber != null)
               startNumber++;
            if (startOrder != null)
               startOrder++;
         }

         return Json (new { passages = passageList });
      }
   }
}
