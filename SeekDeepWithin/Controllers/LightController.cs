using System;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using SeekDeepWithin.DataAccess;
using SeekDeepWithin.Pocos;
using SeekDeepWithin.SdwSearch;

namespace SeekDeepWithin.Controllers
{
   public class LightController : SdwController
   {
      /// <summary>
      /// Initializes a new controller.
      /// </summary>
      public LightController () : base (new SdwDatabase ()) { }

      /// <summary>
      /// Initializes a new controller with the given db info.
      /// </summary>
      /// <param name="db">Database object.</param>
      public LightController (ISdwDatabase db) : base (db) { }

      /// <summary>
      /// Creates light.
      /// </summary>
      /// <param name="text">The text of the light.</param>
      /// <returns>Fails if the a light exists with the given name.
      /// Otherwise returns a json success with the new light's id.</returns>
      [HttpPost]
      [Authorize (Roles = "Creator")]
      public ActionResult Create (string text)
      {
         if (string.IsNullOrWhiteSpace (text)) return this.Fail ("Text was not given and must be supplied.");
         var existingLight = this.Database.Light.Get (l => l.Text == text).FirstOrDefault ();
         if (existingLight != null) return this.Fail ("That light has already been illuminated.");
         var light = new Light { Text = text, Modified = DateTime.Now};
         this.Database.Light.Insert (light);
         this.Database.Save ();
         LightSearch.AddOrUpdateIndex (light);
         return Json (new { status = SUCCESS, id = light.Id, text });
      }

      /// <summary>
      /// Edits light.
      /// </summary>
      /// <param name="id">The id of the light to edit.</param>
      /// <param name="text">The text of the light.</param>
      /// <returns>Fails if the a light exists with the given name.
      /// Otherwise returns a json success with the new light's id.</returns>
      [HttpPost]
      [Authorize (Roles = "Editor")]
      public ActionResult Edit (int id, string text)
      {
         if (string.IsNullOrWhiteSpace (text)) return this.Fail ("Text was not given and must be supplied.");
         var light = this.Database.Light.Get (id);
         if (light == null) return this.Fail ("That light has not yet been illuminated.");
         light.Text = text;
         light.Modified = DateTime.Now;
         this.Database.Save ();
         LightSearch.AddOrUpdateIndex (light);
         return this.Success();
      }

      /// <summary>
      /// Gets light.
      /// </summary>
      /// <returns>A JSON result.</returns>
      public ActionResult Get (int? start, string filter)
      {
         var lights = this.Database.Light.All (q => q.OrderBy (b => b.Text))
            .Where (b => string.IsNullOrEmpty (filter) || b.Text.Contains (filter))
            .Skip (start ?? 0).Take (100).ToList ();
         return Json (
            new {
               status = SUCCESS,
               count = lights.Count,
               light = lights.Select (l => new {
                  id = l.Id,
                  text = l.Text,
                  modified = l.Modified.ToString (CultureInfo.InvariantCulture),
                  key = Helper.Base64Encode (l.Id.ToString (CultureInfo.InvariantCulture))
               })
            }, JsonRequestBehavior.AllowGet);
      }

      /// <summary>
      /// Gets auto complete items for the given text.
      /// </summary>
      /// <param name="text">Text to search for.</param>
      /// <returns>The list of possible items for the given text.</returns>
      public ActionResult AutoComplete (string text)
      {
         var result = new {
            suggestions = this.Database.Light.Get (t => t.Text.Contains (text)).Select (t => new { value = t.Text, data = t.Id })
         };
         return Json (result, JsonRequestBehavior.AllowGet);
      }
   }
}
