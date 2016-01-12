using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using SeekDeepWithin.DataAccess;
using SeekDeepWithin.Pocos;
using SeekDeepWithin.SdwSearch;

namespace SeekDeepWithin.Controllers
{
   public class TruthController : SdwController
   {
      /// <summary>
      /// Initializes a new controller.
      /// </summary>
      public TruthController () : base (new SdwDatabase ()) { }

      /// <summary>
      /// Initializes a new controller with the given db info.
      /// </summary>
      /// <param name="db">Database object.</param>
      public TruthController (ISdwDatabase db) : base (db) { }

      /// <summary>
      /// Creates truth.
      /// </summary>
      /// <returns></returns>
      [HttpPost]
      [Authorize (Roles = "Creator")]
      public ActionResult Create (string light, string truth)
      {
         if (string.IsNullOrWhiteSpace (truth)) return this.Fail ("No truth given");
         if (string.IsNullOrWhiteSpace (light)) return this.Fail ("No light supplied for the truth");
         var truths = truth.Split (new [] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
         var hashIds = new Hashids ("GodisLove") { Order = false };
         var lightIds = hashIds.Decode (light).ToList();
         var love = this.Database.Love.Get (l => l.Lights.Count == lightIds.Count && l.Lights.All (li => lightIds.Contains (li.Id))).FirstOrDefault ();

         if (love == null) {
            hashIds.Order = true;
            love = new Love { Modified = DateTime.Now, Lights = new HashSet<Light> () };
            foreach (var lId in lightIds) {
               var l = this.Database.Light.Get (lId);
               if (l == null) return this.Fail ("The given light was has not yet been illuminated - " + lId);
               love.Lights.Add(l);
            }
            this.Database.Love.Insert (love);
         }

         foreach (var t in truths) {
            var truthData = t.Split ('|');
            var lightText = truthData [3];
            Truth currTruth = null;
            if (love.Truths == null) love.Truths = new HashSet<Truth>();
            var order = string.IsNullOrWhiteSpace (truthData [1]) ? null : (int?)Convert.ToInt32 (truthData [1]);
            var number = string.IsNullOrWhiteSpace (truthData [2]) ? null : (int?) Convert.ToInt32 (truthData [2]);
            var truthLight = this.Database.Light.Get (l => l.Text == lightText).FirstOrDefault ();
            if (truthLight == null)
               truthLight = new Light { Text = lightText, Modified = DateTime.Now };
            else
               currTruth = love.Truths.FirstOrDefault (temp => temp.Light.Id == truthLight.Id);
            if (currTruth == null) {
               currTruth = this.Database.Truth.Get (tr => tr.Light.Id == truthLight.Id && tr.Order == order && tr.Number == number).FirstOrDefault () ??
                           new Truth {Type = Convert.ToInt32 (truthData [0]), Light = truthLight, Order = order, Number = number};
               love.Truths.Add (currTruth);
            }
         }
         this.Database.Save ();
         foreach (var t in love.Truths) {
            LightSearch.AddOrUpdateIndex (t.Light);
         }
         return this.Success ();
      }

      /// <summary>
      /// Performs an edit on the given truth.
      /// </summary>
      /// <param name="id">Id of truth to edit.</param>
      /// <param name="type">Edited truth type.</param>
      /// <param name="order">Edited truth order.</param>
      /// <param name="number">Edited truth number.</param>
      /// <param name="text">Edited truth text.</param>
      /// <param name="all">If text has changed and all is true, all truths with the old text will be updated to the given text.</param>
      /// <returns>JSON response.</returns>
      [HttpPost]
      [Authorize (Roles = "Editor")]
      public ActionResult Edit (int id, int type, int? order, int? number, string text, bool all)
      {
         var truth = this.Database.Truth.Get (id);
         if (truth == null) return this.Fail ("That is an unknown truth!");
         var lightChanged = false;
         truth.Order = order;
         truth.Number = number;
         truth.Type = type;
         if (truth.Light.Text != text) {
            lightChanged = true;
            if (all) {
               truth.Light.Text = text;
               truth.Light.Modified = DateTime.Now;
            } else {
               var light = this.Database.Light.Get (l => l.Text == text).FirstOrDefault () ??
                           new Light {Text = text, Modified = DateTime.Now};
               truth.Light = light;
            }
         }
         this.Database.Save ();
         if (lightChanged) LightSearch.AddOrUpdateIndex (truth.Light);
         return this.Success ();
      }

      /// <summary>
      /// Gets the truth with the given id.
      /// </summary>
      /// <param name="id">Id of truth to get.</param>
      /// <returns>JSON results.</returns>
      public ActionResult Get (int id)
      {
         var truth = this.Database.Truth.Get (id);
         if (truth == null) return this.Fail ("That is an unknown truth!");
         return Json (new { id,
            status = SUCCESS,
            type = truth.Type,
            order = truth.Order,
            number = truth.Number,
            text = truth.Light.Text
         }, JsonRequestBehavior.AllowGet);
      }

      /// <summary>
      /// Formats the given text.
      /// </summary>
      /// <param name="regex">The regex to use for formatting.</param>
      /// <param name="text">The text to format.</param>
      /// <param name="type">The default type.</param>
      /// <param name="startOrder">The starting order.</param>
      /// <returns>JSON result.</returns>
      [HttpPost]
      [Authorize (Roles = "Creator")]
      public ActionResult Format (string regex, string text, int type, int startOrder)
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
         var startNumber = 1;
         var itemList = string.Empty;
         foreach (Match match in matches) {
            var order = match.Groups ["o"];
            var number = match.Groups ["n"];
            itemList += string.Format ("{0}|{1}|{2}|{3}", type,
               order.Success ? Convert.ToInt32 (order.Value) : startOrder,
               number.Success ? Convert.ToInt32 (number.Value) : startNumber,
               match.Groups ["t"].Value.Replace ("\"", "&quot;").Trim ()
            );
            itemList += Environment.NewLine;
            startOrder++;
            startNumber++;
         }

         return Json (new { status = SUCCESS, items = itemList, regexId = dbRegex.Id, regexText = HttpUtility.UrlEncode (regex) });
      }
   }
}