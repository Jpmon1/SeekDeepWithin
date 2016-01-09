using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using SeekDeepWithin.DataAccess;
using SeekDeepWithin.Pocos;

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
            if (love.Truths == null) love.Truths = new HashSet<Truth>();
            var truthLight = this.Database.Light.Get (l => l.Text == lightText).FirstOrDefault () ??
                             new Light { Text = lightText, Modified = DateTime.Now };
            var currTruth = love.Truths.FirstOrDefault (temp => temp.Light.Id == truthLight.Id);
            int? order = string.IsNullOrWhiteSpace (truthData [1]) ? null : (int?)Convert.ToInt32 (truthData [1]);
            int? number = string.IsNullOrWhiteSpace (truthData [2]) ? null : (int?) Convert.ToInt32 (truthData [2]);
            if (currTruth == null) {
               currTruth = this.Database.Truth.Get (tr => tr.Light.Id == truthLight.Id && tr.Order == order && tr.Number == number).FirstOrDefault () ??
                           new Truth {Type = Convert.ToInt32 (truthData [0]), Light = truthLight, Order = order, Number = number};
               love.Truths.Add (currTruth);
            }
         }
         this.Database.Save ();
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
         truth.Order = order;
         truth.Number = number;
         truth.Type = type;
         if (truth.Light.Text != text) {
            if (all)
               truth.Light.Text = text;
            else {
               var light = this.Database.Light.Get (l => l.Text == text).FirstOrDefault() ??
                  new Light {Text = text};
               truth.Light = light;
            }
         }
         this.Database.Save ();
         return this.Success ();
      }

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

      [HttpPost]
      [Authorize (Roles = "Creator")]
      public ActionResult Format (string regex, string list)
      {
         return this.Success ();
      }

      /// <summary>
      /// Gets the HTML for a new truth.
      /// </summary>
      /// <returns>The HTML used for adding truth.</returns>
      [Authorize (Roles = "Creator")]
      public ActionResult New (string id)
      {
         ViewBag.NewId = id;
         return PartialView ();
      }
   }
}