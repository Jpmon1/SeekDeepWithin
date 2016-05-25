using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using SeekDeepWithin.DataAccess;
using SeekDeepWithin.Filters;
using SeekDeepWithin.Models;
using SeekDeepWithin.Pocos;
using SeekDeepWithin.SdwSearch;

namespace SeekDeepWithin.Controllers
{
   [InitializeSimpleMembership]
    public class AddController : SdwController
    {
      /// <summary>
      /// Initializes a new controller.
      /// </summary>
      public AddController () : base (new SdwDatabase ()) { }

      /// <summary>
      /// Initializes a new controller with the given db info.
      /// </summary>
      /// <param name="db">Database object.</param>
      public AddController (ISdwDatabase db) : base (db) { }

      /// <summary>
      /// Gets the add index page.
      /// </summary>
      /// <returns>The add view.</returns>
      [Authorize (Roles = "Creator")]
      public ActionResult Index ()
      {
         if (User.IsInRole ("Creator")) {
            ViewBag.Regexs = this.Database.RegexFormats.All ();
         }
         return View ();
      }

      /// <summary>
      /// Creates light.
      /// </summary>
      /// <param name="text">The text of the light.</param>
      /// <returns>Fails if the a light exists with the given name.
      /// Otherwise returns a json success with the new light's id.</returns>
      [HttpPost]
      [Authorize (Roles = "Creator")]
      public ActionResult Illuminate (string text)
      {
         if (string.IsNullOrWhiteSpace (text)) return this.Fail ("Text was not given and must be supplied.");
         var existingLight = this.Database.Light.Get (l => l.Text == text).FirstOrDefault ();
         if (existingLight != null) return this.Fail ("That light has already been illuminated.");
         var light = new Light { Text = text, Modified = DateTime.Now };
         this.Database.Light.Insert (light);
         this.Database.Save ();
         LightSearch.AddOrUpdateIndex (light);
         return Json (new { status = SUCCESS, id = light.Id, text });
      }

      /// <summary>
      /// Creates truth.
      /// </summary>
      /// <param name="data">The data to import.</param>
      /// <returns>Result</returns>
      [HttpPost]
      [Authorize (Roles = "Creator")]
      public ActionResult Love (string data)
      {
         if (string.IsNullOrEmpty (data)) return this.Fail ("No data given to import!");
         var hash = new Hashids ("GodisLove");
         var import = JsonConvert.DeserializeObject<ImportList> (data);
         foreach (var group in import.Groups) {
            if (group.Lights.Count <= 0) return this.Fail ("The light was not given.");
            var love = Helper.FindLove (this.Database, hash.Encode (Helper.GetLightIds (this.Database, group.Lights)));
            foreach (var t in group.Truths) {
               var truth = t;
               if (t.Parent.HasValue) {
                  if (t.Id.HasValue) {
                     if (!love.Truths.Any (tr => tr.ParentId.HasValue && tr.ParentId == t.Parent.Value && tr.Light != null && tr.Light.Id == truth.Id)) {
                        love.Truths.Add (new Truth {
                           Light = this.Database.Light.Get (t.Id.Value),
                           ParentId = t.Parent,
                           Number = t.Number,
                           Order = t.Order
                        });
                     }
                  } else {
                     if (!love.Truths.Any (tr => tr.ParentId.HasValue && tr.ParentId == t.Parent.Value && tr.Light == null)) {
                        love.Truths.Add (new Truth {
                           ParentId = t.Parent,
                           Number = t.Number,
                           Order = t.Order
                        });
                     }
                  }
               } else {
                  if (t.Id.HasValue) {
                     if (!love.Truths.Any (tr => !tr.ParentId.HasValue && tr.Light != null && tr.Light.Id == truth.Id)) {
                        love.Truths.Add (new Truth {
                           Light = this.Database.Light.Get (t.Id.Value),
                           Number = t.Number,
                           Order = t.Order
                        });
                     }
                  } else {
                     var light = this.Database.Light.Get (l => l.Text == truth.Text).FirstOrDefault () ??
                                 new Light { Text = truth.Text, Modified = DateTime.Now };
                     love.Truths.Add (new Truth { Light = light, Order = t.Order, Number = t.Number });
                  }
               }
            }
            this.Database.Save ();
            foreach (var t in love.Truths) {
               LightSearch.AddOrUpdateIndex (t.Light);
            }
            /*if (!string.IsNullOrEmpty (truthLinks)) {
               var links = truthLinks.Split (new [] { ',' }, StringSplitOptions.RemoveEmptyEntries);
               foreach (var t in love.Truths) {
                  var l = this.FindLove (hash.Encode (t.Light.Id));
                  foreach (var link in links) {
                     var lightIds = hash.Decode (link).ToList ();
                     if (lightIds [0] == 0) {
                        lightIds.RemoveAt (0);
                        var li = this.Database.Light.Get (lightIds [0]);
                        lightIds.RemoveAt (0);
                        var lov = this.FindLove (hash.Encode (lightIds));
                        if (lov.Truths.Any (tr => tr.Light != null && tr.Light.Id == li.Id && !tr.ParentId.HasValue))
                           continue;
                        lov.Truths.Add (new Truth { Light = li });
                     } else {
                        if (lightIds.Count == 1) {
                           if (l.Truths.Any (tr => tr.Light != null && tr.Light.Id == lightIds [0] && !tr.ParentId.HasValue))
                              continue;
                           l.Truths.Add (new Truth { Light = this.Database.Light.Get (lightIds [0]) });
                        } else {
                           var lov = this.FindLove (link);
                           if (l.Truths.Any (tr => tr.Light == null && tr.ParentId.HasValue && tr.ParentId == lov.Id))
                              continue;
                           l.Truths.Add (new Truth { ParentId = lov.Id });
                        }
                     }
                  }
               }
               this.Database.Save ();
            }*/
         }
         //if (string.IsNullOrWhiteSpace (truth)) return this.Fail ("No truth given");
         //var love = FindLove (light);
         //if (love == null) return this.Fail ("No light given");
         //var truths = truth.Trim ().Split (new [] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
         //foreach (var t in truths) {
         //   var truthData = t.Split ('|');
         //   var lightText = truthData [2].Trim ();
         //   var order = string.IsNullOrWhiteSpace (truthData [0]) ? null : (int?)Convert.ToInt32 (truthData [0]);
         //   var number = string.IsNullOrWhiteSpace (truthData [1]) ? null : (int?) Convert.ToInt32 (truthData [1]);
         //   var truthLight = this.Database.Light.Get (l => l.Text == lightText).FirstOrDefault () ??
         //                    new Light {Text = lightText, Modified = DateTime.Now};
         //   love.Truths.Add (new Truth { Light = truthLight, Order = order, Number = number });
         //}
         //var hash = new Hashids ("GodisLove");

         //if (!string.IsNullOrEmpty (versions)) {
         //   var loveIds = hash.Decode (versions);
         //   if (loveIds.Length > 0) {
         //      foreach (var loveId in loveIds) {
         //         var linkedLove = this.Database.Love.Get (loveId);
         //         foreach (var addTruth in love.Truths.Where (t => t.Number.HasValue)) {
         //            var lt = linkedLove.Truths.FirstOrDefault (tr => tr.Number == addTruth.Number);
         //            if (lt != null) {
         //               var truthLove = this.FindLove (hash.Encode (lt.Light.Id));
         //               truthLove.Truths.Add (new Truth {Light = addTruth.Light, ParentId = love.Id});
         //               truthLove = this.FindLove (hash.Encode (addTruth.Light.Id));
         //               truthLove.Truths.Add (new Truth {Light = lt.Light, ParentId = linkedLove.Id});
         //            }
         //         }
         //      }
         //      this.Database.Save ();
         //   }
         //}
         return this.Success ();
      }

      /// <summary>
      /// Gets possible version links for the given lights.
      /// </summary>
      /// <param name="lights">The hash of lights to get links for.</param>
      /// <returns>Results</returns>
      [Authorize (Roles = "Editor")]
      public ActionResult VersionLinks (string lights)
      {
         var loves = new List<Love> ();
         var hash = new Hashids ("GodisLove");
         var lightIds = hash.Decode (lights).ToList ();
         if (lightIds.Count > 1) {
            var first = lightIds.First ();
            lightIds.RemoveAt (0);
            var light = this.Database.Light.Get (first);
            var testLights = new List<int> ();
            foreach (var truth in light.Truths) {
               foreach (var test in truth.Love.Truths.Where (t => t.Light != null)) {
                  if (!testLights.Contains (test.Light.Id) && !lightIds.Contains (test.Light.Id) && test.Light.Id != first)
                     testLights.Add (test.Light.Id);
               }
            }
            hash.Order = true;
            if (testLights.Count > 0) {
               foreach (var testLight in testLights) {
                  lightIds.Add (testLight);
                  var peaceId = hash.Encode (lightIds);
                  var love = this.Database.Love.Get (l => l.PeaceId == peaceId).FirstOrDefault ();
                  if (love != null) loves.Add (love);
                  lightIds.Remove (testLight);
               }
            }
         }
         return PartialView (loves);
      }

      /// <summary>
      /// Formats the given text.
      /// </summary>
      /// <param name="regex">The regex to use for formatting.</param>
      /// <param name="text">The text to format.</param>
      /// <param name="startOrder">The starting order.</param>
      /// <param name="startNumber">The starting number.</param>
      /// <param name="light">The light for the import.</param>
      /// <returns>JSON result.</returns>
      [HttpPost]
      [Authorize (Roles = "Creator")]
      public ActionResult Format (string regex, string text, int? startOrder, int? startNumber, string light)
      {
         text = HttpUtility.UrlDecode (text);
         if (string.IsNullOrWhiteSpace (text)) return this.Fail ("Nothing given to format.");
         regex = HttpUtility.UrlDecode (regex);
         if (string.IsNullOrWhiteSpace (regex)) return this.Fail ("No regular expression supplied.");
         var matches = Regex.Matches (text, regex, RegexOptions.IgnoreCase);
         var dbRegex = this.Database.RegexFormats.Get (r => r.Regex == regex).FirstOrDefault ();
         if (dbRegex == null) {
            dbRegex = new FormatRegex { Regex = regex };
            this.Database.RegexFormats.Insert (dbRegex);
            this.Database.Save ();
         }
         var import = new ImportGroup { Lights = {} };
         foreach (Match match in matches) {
            var order = match.Groups ["o"];
            var number = match.Groups ["n"];
            var importTruth = new ImportTruth {
               Order = order.Success ? Convert.ToInt32 (order.Value) : startOrder,
               Number = number.Success ? Convert.ToInt32 (number.Value) : startNumber,
               Text = match.Groups ["t"].Value.Replace ("\n", " ").Trim ()
            };
            import.Truths.Add (importTruth);
            if (startOrder.HasValue) startOrder++;
            if (startNumber.HasValue) {
               if (startNumber < 0) startNumber--;
               else startNumber++;
            }
         }
         return Json (new { status = SUCCESS, items = import, regexId = dbRegex.Id, regexText = HttpUtility.UrlEncode (regex) });
      }
   }
}
