using System.Linq;
using System.Web;
using System.Web.Mvc;
using SeekDeepWithin.DataAccess;
using SeekDeepWithin.Pocos;

namespace SeekDeepWithin.Controllers
{
   public class StyleController : SdwController
   {
      /// <summary>
      /// Initializes a new controller.
      /// </summary>
      public StyleController () : base (new SdwDatabase ()) { }

      /// <summary>
      /// Initializes a new controller with the given db info.
      /// </summary>
      /// <param name="db">Database object.</param>
      public StyleController (ISdwDatabase db) : base (db) { }

      /// <summary>
      /// Adds a new style to a light.
      /// </summary>
      /// <param name="truthId">Id of light to add style to.</param>
      /// <param name="startStyle">The start tag of the style.</param>
      /// <param name="endStyle">The end tag of the style.</param>
      /// <param name="startIndex">The start index of the style.</param>
      /// <param name="endIndex">The end index of the style.</param>
      /// <returns>Json results</returns>
      [HttpPost]
      [Authorize (Roles = "Editor")]
      public ActionResult Create (int truthId, string startStyle, string endStyle, int startIndex, int endIndex)
      {
         var truth = this.Database.Truth.Get (truthId);
         if (truth == null) return this.Fail ("That truth is unknown.");
         var style = GetStyle (startStyle, endStyle);
         var lightStyle = new TruthStyle {
            Truth = truth,
            Style = style,
            StartIndex = startIndex,
            EndIndex = endIndex
         };
         truth.Styles.Add (lightStyle);
         this.Database.Save ();
         return Json (new { status = SUCCESS, id = lightStyle.Id });
      }

      /// <summary>
      /// Edits the given style in the given light.
      /// </summary>
      /// <param name="id">The id of the style to edit.</param>
      /// <param name="truthId">Id of light with style to edit.</param>
      /// <param name="startStyle">The start tag of the style.</param>
      /// <param name="endStyle">The end tag of the style.</param>
      /// <param name="startIndex">The start index of the style.</param>
      /// <param name="endIndex">The end index of the style.</param>
      /// <returns>Json results</returns>
      [HttpPost]
      [Authorize (Roles = "Editor")]
      public ActionResult Edit (int id, int truthId, string startStyle, string endStyle, int startIndex, int endIndex)
      {
         var truth = this.Database.Truth.Get (truthId);
         if (truth == null) return this.Fail ("That truth is unknown.");
         var style = GetStyle (startStyle, endStyle);
         var truthStyle = truth.Styles.FirstOrDefault (s => s.Id == id);
         if (truthStyle == null) return this.Fail ("Unable to find the style.");
         if (truthStyle.Style.Id != style.Id) truthStyle.Style = style;
         truthStyle.EndIndex = endIndex;
         truthStyle.StartIndex = startIndex;
         this.Database.Save ();
         return this.Success ();
      }

      /// <summary>
      /// Deletes a style from a light.
      /// </summary>
      /// <param name="id">The id of the style to delete.</param>
      /// <param name="truthId">The id of the light to delete the style from.</param>
      /// <returns>Json results</returns>
      [HttpPost]
      [Authorize (Roles = "Editor")]
      public ActionResult Delete (int id, int truthId)
      {
         var truth = this.Database.Truth.Get (truthId);
         if (truth == null) return this.Fail ("That truth is unknown.");
         var truthStyle = truth.Styles.FirstOrDefault (s => s.Id == id);
         if (truthStyle == null) return this.Fail ("Unable to find the style.");
         truth.Styles.Remove (truthStyle);
         this.Database.Save ();
         return this.Success ();
      }

      /// <summary>
      /// Gets the styles for a light.
      /// </summary>
      /// <param name="truthId">The id of the light to get styles for.</param>
      /// <returns>Json results</returns>
      public ActionResult Get (int truthId)
      {
         var truth = this.Database.Truth.Get (truthId);
         if (truth == null) return this.Fail ("That truth is unknown.");
         return Json (
            new {
               status = SUCCESS,
               count = truth.Styles.Count,
               light = truth.Styles.Select (l => new {
                  id = l.Id,
                  endIndex = l.EndIndex,
                  endStyle = l.Style.End,
                  startIndex = l.StartIndex,
                  startStyle = l.Style.Start
               })
            }, JsonRequestBehavior.AllowGet);
      }

      /// <summary>
      /// Gets the style, or creates if not found.
      /// </summary>
      /// <param name="startStyle">The starting style.</param>
      /// <param name="endStyle">The ending style.</param>
      /// <returns>The requested style.</returns>
      private Style GetStyle (string startStyle, string endStyle)
      {
         var start = HttpUtility.UrlDecode (startStyle) ?? "<span>";
         var end = HttpUtility.UrlDecode (endStyle) ?? "</span>";
         var styles = this.Database.Styles.Get (s => s.Start == start  && s.End == end);
         var style = styles.FirstOrDefault ();
         if (style == null)
         {
            style = new Style {Start = start, End = end};
            this.Database.Styles.Insert (style);
            this.Database.Save ();
         }
         return style;
      }
   }
}
