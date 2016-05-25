using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using SeekDeepWithin.DataAccess;
using SeekDeepWithin.Filters;
using SeekDeepWithin.Pocos;
using SeekDeepWithin.SdwSearch;

namespace SeekDeepWithin.Controllers
{
   [InitializeSimpleMembership]
   public class EditController : SdwController
   {
      /// <summary>
      /// Initializes a new controller.
      /// </summary>
      public EditController () : base (new SdwDatabase ()) { }

      /// <summary>
      /// Initializes a new controller with the given db info.
      /// </summary>
      /// <param name="db">Database object.</param>
      public EditController (ISdwDatabase db) : base (db) { }

      /// <summary>
      /// Gets the edit index page.
      /// </summary>
      /// <returns></returns>
      public ActionResult Index (string edit, string link)
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
      /// <param name="light">The light ids for the love.</param>
      /// <param name="truth">The truth to add.</param>
      /// <param name="versions">List of version loves to link truths to.</param>
      /// <param name="truthLinks">List of truth links.</param>
      /// <returns>Result</returns>
      [HttpPost]
      [Authorize (Roles = "Creator")]
      public ActionResult Love (string light, string truth, string versions, string truthLinks)
      {
         if (string.IsNullOrWhiteSpace (truth)) return this.Fail ("No truth given");
         var love = Helper.FindLove (this.Database, light);
         if (love == null) return this.Fail ("No light given");
         var truths = truth.Trim ().Split (new [] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
         foreach (var t in truths) {
            var truthData = t.Split ('|');
            var lightText = truthData [2].Trim ();
            var order = string.IsNullOrWhiteSpace (truthData [0]) ? null : (int?)Convert.ToInt32 (truthData [0]);
            var number = string.IsNullOrWhiteSpace (truthData [1]) ? null : (int?) Convert.ToInt32 (truthData [1]);
            var truthLight = this.Database.Light.Get (l => l.Text == lightText).FirstOrDefault () ??
                             new Light {Text = lightText, Modified = DateTime.Now};
            love.Truths.Add (new Truth { Light = truthLight, Order = order, Number = number });
         }
         this.Database.Save ();
         foreach (var t in love.Truths) {
            LightSearch.AddOrUpdateIndex (t.Light);
         }
         var hash = new Hashids ("GodisLove");
         if (!string.IsNullOrEmpty (truthLinks)) {
            var links = truthLinks.Split (new[] {','}, StringSplitOptions.RemoveEmptyEntries);
            foreach (var t in love.Truths) {
               var l = Helper.FindLove (this.Database, t.Light.Id);
               foreach (var link in links) {
                  var lightIds = hash.Decode (link).ToList ();
                  if (lightIds [0] == 0) {
                     lightIds.RemoveAt (0);
                     var li = this.Database.Light.Get (lightIds [0]);
                     lightIds.RemoveAt (0);
                     var lov = Helper.FindLove (this.Database, lightIds);
                     if (lov.Truths.Any (tr => tr.Light != null && tr.Light.Id == li.Id && !tr.ParentId.HasValue))
                        continue;
                     lov.Truths.Add(new Truth { Light = li });
                  } else {
                     if (lightIds.Count == 1) {
                        if (l.Truths.Any (tr => tr.Light != null && tr.Light.Id == lightIds [0] && !tr.ParentId.HasValue))
                           continue;
                        l.Truths.Add (new Truth {Light = this.Database.Light.Get (lightIds [0])});
                     } else {
                        var lov = Helper.FindLove (this.Database, link);
                        if (l.Truths.Any (tr => tr.Light == null && tr.ParentId.HasValue && tr.ParentId == lov.Id))
                           continue;
                        l.Truths.Add (new Truth { ParentId = lov.Id });
                     }
                  }
               }
            }
            this.Database.Save ();
         }
         if (!string.IsNullOrEmpty (versions)) {
            var loveIds = hash.Decode (versions);
            if (loveIds.Length > 0) {
               foreach (var loveId in loveIds) {
                  var linkedLove = this.Database.Love.Get (loveId);
                  foreach (var addTruth in love.Truths.Where (t => t.Number.HasValue)) {
                     var lt = linkedLove.Truths.FirstOrDefault (tr => tr.Number == addTruth.Number);
                     if (lt != null) {
                        var truthLove = Helper.FindLove (this.Database, lt.Light.Id);
                        truthLove.Truths.Add (new Truth {Light = addTruth.Light, ParentId = love.Id});
                        truthLove = Helper.FindLove (this.Database, addTruth.Light.Id);
                        truthLove.Truths.Add (new Truth {Light = lt.Light, ParentId = linkedLove.Id});
                     }
                  }
               }
               this.Database.Save ();
            }
         }
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
         var loves = new List <Love> ();
         var hash = new Hashids ("GodisLove");
         var lightIds = hash.Decode (lights).ToList ();
         if (lightIds.Count > 1) {
            var first = lightIds.First ();
            lightIds.RemoveAt (0);
            var light = this.Database.Light.Get (first);
            var testLights = new List <int> ();
            foreach (var truth in light.Truths) {
               foreach (var test in truth.Love.Truths.Where(t => t.Light != null)) {
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
      /// Adds a specific truth as a link.
      /// </summary>
      /// <param name="id">Id of truth to add.</param>
      /// <param name="light">The light of the love.</param>
      /// <returns>JSON results</returns>
      public ActionResult TruthAddTruth (int id, string light)
      {
         var truth = this.Database.Truth.Get (id);
         if (truth == null) return this.Fail ("Unable to understand the truth.");
         var love = Helper.FindLove (this.Database, light);
         var order = love.Truths.Max (t => t.Order ?? 0) + 1;
         if (!love.Truths.Any (t => t.ParentId.HasValue && t.ParentId == truth.Love.Id &&
                                    t.Light != null && t.Light.Id == truth.Light.Id &&
                                    t.Order.HasValue && t.Order == order)) {
            love.Truths.Add (new Truth {
               Light = truth.Light,
               ParentId = truth.Love.Id,
               Number = truth.Number,
               Order = order
            });
         }
         else
            return this.Fail("Truth is already known.");
         this.Database.Save ();
         return this.Success ();
      }

      /// <summary>
      /// Adds a specific truth as a link.
      /// </summary>
      /// <param name="link">Id of love to add.</param>
      /// <param name="light">The light of the love.</param>
      /// <param name="toTruth">Add the link to the truths of the love.</param>
      /// <returns>JSON results</returns>
      public ActionResult TruthAddLove (string link, string light, int toTruth)
      {
         var loveLink = Helper.FindLove (this.Database, link);
         var love = Helper.FindLove (this.Database, light);
         if (toTruth == 1) {
            foreach (var truth in love.Truths) {
               var truthLove = Helper.FindLove (this.Database, truth.Light.Id);
               if (!truthLove.Truths.Any (t => t.ParentId.HasValue && t.ParentId.Value == loveLink.Id))
                  truthLove.Truths.Add (new Truth { ParentId = loveLink.Id });
            }
         } else {
            if (love.Truths.Any(t => t.ParentId.HasValue && t.ParentId.Value == loveLink.Id))
               love.Truths.Add (new Truth {ParentId = loveLink.Id});
         }
         this.Database.Save ();
         return this.Success ();
      }

      /// <summary>
      /// Adds a light as a truth to another light.
      /// </summary>
      /// <param name="id">Id of light to add truth to.</param>
      /// <param name="truth">Id of light to as a truth.</param>
      /// <returns>JSON results</returns>
      [HttpPost]
      public ActionResult LightAddLight (int id, string truth)
      {
         var light = this.Database.Light.Get (l => l.Text == truth).FirstOrDefault() ?? new Light {Text = truth, Modified = DateTime.Now};
         var love = Helper.FindLove (this.Database, id);
         if (!love.Truths.Any (t => t.Light != null && t.Light.Id == id))
            love.Truths.Add(new Truth { Light = light});
         this.Database.Save ();
         LightSearch.AddOrUpdateIndex (light);
         return this.Success ();
      }

      /// <summary>
      /// Adds a header to the given truth.
      /// </summary>
      /// <param name="id">The id of the truth to add a header to.</param>
      /// <param name="text">The header's text.</param>
      /// <param name="index">The index to use for footers.</param>
      /// <returns>The result.</returns>
      public ActionResult CreateHeaderFooter (int id, string text, int? index)
      {
         var truth = this.Database.Truth.Get (id);
         if (truth == null) return this.Fail ("Unable to understand the truth.");
         var light = this.Database.Light.Get (l => l.Text == text).FirstOrDefault ();
         if (light == null) {
            light = new Light {Text = text, Modified = DateTime.Now};
            this.Database.Save ();
            LightSearch.AddOrUpdateIndex (light);
         }
         if (!truth.Love.Truths.Any (t => t.ParentId.HasValue && t.ParentId == id && t.Light != null && t.Light.Id == light.Id)) {
            var headerFooter = new Truth {Light = light, ParentId = id, Order = index ?? 0};
            truth.Love.Truths.Add (headerFooter);
            var hLove = Helper.FindLove (this.Database, light.Id);
            hLove.Truths.Add (new Truth { ParentId = truth.Love.Id });
         }
         this.Database.Save ();
         return this.Success ();
      }

      /// <summary>
      /// Adds a specific truth as a link.
      /// </summary>
      /// <param name="id">Id of light to add.</param>
      /// <param name="light">The light of the love.</param>
      /// <param name="toTruth">Add the link to the truths of the love.</param>
      /// <returns>JSON results</returns>
      public ActionResult TruthAddLight (int id, string light, int toTruth)
      {
         var l = this.Database.Light.Get (id);
         if (l == null) return this.Fail ("That light has not yet been illuminated.");
         var love = Helper.FindLove (this.Database, light);
         if (toTruth == 1) {
            foreach (var truth in love.Truths) {
               var truthLove = Helper.FindLove (this.Database, truth.Light.Id);
               if (truthLove.Truths.Any (t => t.Light != null && t.Light.Id == id))
                  continue;
               truthLove.Truths.Add (new Truth { Light = l});
            }
         } else {
            if (!love.Truths.Any (t => t.Light != null && t.Light.Id == id))
               love.Truths.Add (new Truth { Light = l});
         }
         this.Database.Save ();
         return this.Success ();
      }

      /// <summary>
      /// Gets any possible truth links.
      /// </summary>
      /// <param name="lights">The hash of lights to get links for.</param>
      /// <returns>The different combinations of truth.</returns>
      [Authorize (Roles = "Editor")]
      public ActionResult TruthLinks (string lights)
      {
         var hash = new Hashids ("GodisLove");
         var lightIds = hash.Decode (lights).ToList ();
         var combos = GetCombinations (lightIds);
         var light = this.Database.Light.Get (l => lightIds.Contains (l.Id)).ToList ();
         var items = new Dictionary <string, string> ();
         foreach (var combo in combos) {
            var hashId = hash.Encode (combo);
            var text = string.Empty;
            foreach (var id in combo.OrderBy (lightIds.IndexOf)) {
               var li = light.FirstOrDefault (l => l.Id == id);
               if (li != null) {
                  if (!string.IsNullOrEmpty (text))
                     text += " | ";
                  text += li.Text;
               }
            }
            items.Add(hashId, text);
         }
         if (lightIds.Count > 2) {
            var permus = new List <List <int>> ();
            foreach (var lId in lightIds) {
               int id = lId;
               var perm = new List <int> { id };
               perm.AddRange (lightIds.Where (i => i != id));
               permus.Add (perm);
            }
            foreach (var perm in permus) {
               perm.Insert (0, 0);
               var hashId = hash.Encode (perm);
               var text = "P";
               foreach (var id in perm) {
                  var li = light.FirstOrDefault (l => l.Id == id);
                  if (li != null) {
                     text += " | " + li.Text;
                  }
               }
               items.Add (hashId, text);
            }
         }
         return PartialView (items);
      }

      /// <summary>
      /// Gets all of the combinations of the given list.
      /// </summary>
      /// <param name="list">The list of integers.</param>
      /// <returns>A list of combinations.</returns>
      static IEnumerable <List <int>> GetCombinations (IList <int> list)
      {
         var combos = new List <List <int>> ();
         double count = Math.Pow (2, list.Count);
         for (int i = 1; i <= count - 1; i++) {
            var combo = new List <int> ();
            string str = Convert.ToString (i, 2).PadLeft (list.Count, '0');
            for (int j = 0; j < str.Length; j++) {
               if (str [j] == '1') {
                  combo.Add (list [j]);
               }
            }
            combos.Add (combo);
         }
         return combos;
      }

      /// <summary>
      /// Gets the edit view for the given light.
      /// </summary>
      /// <param name="id">Id of light to get view for.</param>
      /// <param name="link">Gets or Sets if the this is a link edit.</param>
      /// <returns>The edit view or json error.</returns>
      public ActionResult GetLightItem (int id, int link)
      {
         ViewBag.IsLink = link == 1;
         var light = this.Database.Light.Get (id);
         if (light == null) return this.Fail ("That light has not yet been illuminated.");
         return this.PartialView (light);
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
      public ActionResult LightEdit (int id, string text)
      {
         if (string.IsNullOrWhiteSpace (text)) return this.Fail ("Text was not given and must be supplied.");
         var light = this.Database.Light.Get (id);
         if (light == null) return this.Fail ("That light has not yet been illuminated.");
         light.Text = text;
         light.Modified = DateTime.Now;
         this.Database.Save ();
         LightSearch.AddOrUpdateIndex (light);
         return this.Success ();
      }

      /// <summary>
      /// Gets the truths for the given light.
      /// </summary>
      /// <param name="lights">Light to get truth for.</param>
      /// <param name="link">Gets or Sets if the this is a link edit.</param>
      /// <returns>Results.</returns>
      [Authorize (Roles = "Editor")]
      public ActionResult Truths (string lights, int link)
      {
         ViewBag.IsLink = link == 1;
         var love = Helper.FindLove (this.Database, lights, false);
         var truths = love == null ? new List <Truth> () : love.Truths.ToList ();
         if (love != null)
            GetOtherLoves (love);
         return PartialView (truths);
      }

      /// <summary>
      /// Gets the edit page for a truth.
      /// </summary>
      /// <param name="id">Id of truth to edit.</param>
      /// <returns>The edit page for the truth.</returns>
      [Authorize (Roles = "Editor")]
      public ActionResult TruthEdit (int id)
      {
         var truth = this.Database.Truth.Get (id);
         if (truth == null) return this.Fail ("Unable to understand the truth.");
         if (truth.Light != null) {
            var love = Helper.FindLove (this.Database, truth.Light.Id, false);
            if (love != null) {
               ViewBag.Love = love;
               GetOtherLoves (love);
            }
         }
         return this.PartialView (truth);
      }

      /// <summary>
      /// Gets the list of other loves in the given truths.
      /// </summary>
      /// <param name="love">Love with truths to check.</param>
      private void GetOtherLoves (Love love)
      {
         var otherLoves = new Dictionary <int, Love> ();
         foreach (var t in love.Truths) {
            if (t.ParentId.HasValue && !otherLoves.ContainsKey (t.ParentId.Value)) {
               otherLoves.Add (t.ParentId.Value, this.Database.Love.Get (t.ParentId.Value));
            }
         }
         ViewBag.OtherLoves = otherLoves;
      }

      /// <summary>
      /// Performs an edit on the given truth.
      /// </summary>
      /// <param name="id">Id of truth to edit.</param>
      /// <param name="order">Edited truth order.</param>
      /// <param name="number">Edited truth number.</param>
      /// <returns>JSON response.</returns>
      [HttpPost]
      [Authorize (Roles = "Editor")]
      public ActionResult TruthEdit (int id, int? order, int? number)
      {
         var truth = this.Database.Truth.Get (id);
         if (truth == null) return this.Fail ("Unable to understand the truth.");
         truth.Order = order;
         truth.Number = number;
         this.Database.Save ();
         return this.Success ();
      }

      /// <summary>
      /// Removes a truth from the parent love.
      /// </summary>
      /// <param name="id">Id of truth to remove.</param>
      /// <returns>The results.</returns>
      [HttpPost]
      [Authorize (Roles = "Creator")]
      public ActionResult TruthRemove (int id)
      {
         var truth = this.Database.Truth.Get (id);
         if (truth == null) return this.Fail ("Unable to understand the truth.");
         var love = truth.Love;
         love.Truths.Remove (truth);
         var count = truth.Styles.Count;
         if (count > 0) {
            var list = truth.Styles.ToList ();
            for (int i = count - 1; i >= 0; i--) {
               this.Database.TruthStyles.Delete (list [i]);
            }
         }
         this.Database.Truth.Delete (truth);
         if (love.Truths.Count == 0) {
            count = love.Peaces.Count;
            var peaceList = love.Peaces.ToList ();
            for (int i = count - 1; i >= 0; i--) {
               this.Database.Peace.Delete (peaceList[i]);
            }
            this.Database.Love.Delete (love);
         }
         this.Database.Save ();
         return this.Success ();
      }

      /// <summary>
      /// Adds the given style to the given truth.
      /// </summary>
      /// <param name="id">Id of truth.</param>
      /// <param name="startIndex">The start index of the style</param>
      /// <param name="endIndex">The end index of the style</param>
      /// <param name="start">The start of the style</param>
      /// <param name="end">The end of the style</param>
      /// <returns>The results.</returns>
      [HttpPost]
      [Authorize (Roles = "Editor")]
      public ActionResult TruthAddStyle (int id, int startIndex, int endIndex, string start, string end)
      {
         var truth = this.Database.Truth.Get (id);
         if (truth == null) return this.Fail ("Unable to understand the truth.");
         start = HttpUtility.UrlDecode (start);
         end = HttpUtility.UrlDecode (end);
         var style = this.Database.Styles.Get (s => s.Start == start && s.End == end).FirstOrDefault();
         if (style == null) {
            style = new Style {Start = start, End = end};
            this.Database.Styles.Insert (style);
         }
         truth.Styles.Add (new TruthStyle { StartIndex = startIndex, EndIndex = endIndex, Style = style });
         this.Database.Save ();
         return this.Success ();
      }

      /// <summary>
      /// Removes the given style from the given truth.
      /// </summary>
      /// <param name="id">Id of truth style.</param>
      /// <returns>The results.</returns>
      [HttpPost]
      [Authorize (Roles = "Editor")]
      public ActionResult TruthRemoveStyle (int id)
      {
         var style = this.Database.TruthStyles.Get (id);
         if (style == null) return this.Fail ("Unable to find the style.");
         if (style.Truth != null)
            style.Truth.Styles.Remove (style);
         this.Database.TruthStyles.Delete (style);
         this.Database.Save ();
         return this.Success ();
      }

      /// <summary>
      /// Re indexes the given light.
      /// </summary>
      /// <param name="id">Id of light to index.</param>
      /// <returns>JSON results.</returns>
      [HttpPost]
      [Authorize (Roles = "Editor")]
      public ActionResult ReIndex (int? id)
      {
         if (id == null) {
            LightSearch.AddOrUpdateIndex (this.Database.Light.All ());
         } else {
            var light = this.Database.Light.Get (id.Value);
            if (light == null) return this.Fail ("That light has not been illuminated.");
            LightSearch.AddOrUpdateIndex (light);
         }
         return this.Success ();
      }

      /// <summary>
      /// Formats the given text.
      /// </summary>
      /// <param name="regex">The regex to use for formatting.</param>
      /// <param name="text">The text to format.</param>
      /// <param name="startOrder">The starting order.</param>
      /// <param name="startNumber">The starting number.</param>
      /// <returns>JSON result.</returns>
      [HttpPost]
      [Authorize (Roles = "Creator")]
      public ActionResult Format (string regex, string text, int? startOrder, int? startNumber)
      {
         text = HttpUtility.UrlDecode (text);
         if (string.IsNullOrWhiteSpace (text)) return this.Fail ("Nothing given to format.");
         regex = HttpUtility.UrlDecode (regex);
         if (string.IsNullOrWhiteSpace (regex)) return this.Fail ("No regular expression supplied.");
         var matches = Regex.Matches (text, regex, RegexOptions.IgnoreCase);
         var dbRegex = this.Database.RegexFormats.Get (r => r.Regex == regex).FirstOrDefault ();
         if (dbRegex == null) {
            dbRegex = new FormatRegex {Regex = regex};
            this.Database.RegexFormats.Insert (dbRegex);
            this.Database.Save ();
         }
         /*type|order|number|text...*/
         var itemList = string.Empty;
         foreach (Match match in matches) {
            var order = match.Groups ["o"];
            var number = match.Groups ["n"];
            itemList += string.Format ("{0}|{1}|{2}",
               order.Success ? Convert.ToInt32 (order.Value) : startOrder,
               number.Success ? Convert.ToInt32 (number.Value) : startNumber,
               match.Groups ["t"].Value.Replace ("\"", "&quot;").Trim ()
            );
            itemList += Environment.NewLine;
            if (startOrder.HasValue) startOrder++;
            if (startNumber.HasValue) {
               if (startNumber < 0) startNumber--;
               else startNumber++;
            }
         }

         return Json (new { status = SUCCESS, items = itemList.Trim (), regexId = dbRegex.Id, regexText = HttpUtility.UrlEncode (regex) });
      }

      /// <summary>
      /// Updates something.
      /// </summary>
      /// <returns>JSON result.</returns>
      [HttpPost]
      public ActionResult Update (string key)
      {
         if (string.IsNullOrWhiteSpace (key)) return this.Fail ();
         var k = Helper.Base64Decode (key);
         if (k == "GodisLove") {
         }
         return this.Fail ();
      }
   }
}