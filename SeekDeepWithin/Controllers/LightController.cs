using System;
using System.Collections.Generic;
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
      /// Gets the edit view for the given light.
      /// </summary>
      /// <param name="id">Id of light to get view for.</param>
      /// <returns>The edit view or json error.</returns>
      public ActionResult Edit (int id)
      {
         var light = this.Database.Light.Get (id);
         return light == null ? this.Fail ("That light has not yet been illuminated.") : PartialView (light);
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
      public ActionResult Get (int? start, int? seed)
      {
         if (seed == null)
            seed = Environment.TickCount;
         if (start == null)
            start = 1;
         var total = start + 100;
         var ids = new List <int> ();
         var random = new Random (seed.Value);
         var maxId = this.Database.Light.All ().Max (l => l.Id);
         for (int i = 0; i < total; i++) {
            var id = random.Next (1, maxId + 1);
            if (i >= start) {
               ids.Add (id);
            }
         }
         ViewBag.Seed = seed.Value;
         var lights = this.Database.Light.Get (l => ids.Contains (l.Id)).OrderBy(l => ids.IndexOf(l.Id));
         return PartialView (lights);
      }

      /// <summary>
      /// Gets auto complete items for the given text.
      /// </summary>
      /// <param name="text">Text to search for.</param>
      /// <returns>The list of possible items for the given text.</returns>
      [AllowAnonymous]
      public ActionResult AutoComplete (string text)
      {
         var query = LightSearch.AutoComplete (text);
         //var lights = this.Database.Light.Get (l => l.Text.Contains (text));
         var result = new { suggestions = query.Select (kvp => new { value = kvp.Value, data = kvp.Key }) };
         return Json (result, JsonRequestBehavior.AllowGet);
      }
   }
}
