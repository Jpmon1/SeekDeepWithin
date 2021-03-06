﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using SeekDeepWithin.DataAccess;
using SeekDeepWithin.Models;
using SeekDeepWithin.Pocos;
using SeekDeepWithin.SdwSearch;

namespace SeekDeepWithin.Controllers
{
   public class SeekController : SdwController
   {
      /// <summary>
      /// Initializes a new controller.
      /// </summary>
      public SeekController () : base (new SdwDatabase ()) { }

      /// <summary>
      /// Initializes a new controller with the given db info.
      /// </summary>
      /// <param name="db">Database object.</param>
      public SeekController (ISdwDatabase db) : base (db) { }

      /// <summary>
      /// Gets the love.
      /// </summary>
      [AllowAnonymous]
      public ActionResult Love (int? id, string items, string history)
      {
         var model = new LoveModel ();
         var hash = new Hashids ("GodisLove") { Order = true };
         var ids = hash.Decode (items).ToList ();
         if (id == null) {
            var lights = this.Database.Light.All (q => q.OrderBy(l => Guid.NewGuid())).Take (25);
            foreach (var light in lights) {
               model.ToAdd.Add (new SdwItem (light));
            }
         } else {
            var histIds = hash.Decode (history).ToList ();
            histIds.Add (id.Value);
            history = hash.Encode (histIds);
            var light = this.Database.Light.Get (id.Value);
            ids.Add (id.Value);
            var loves = (from peace in light.Peaces
                         where peace.Love.Peaces.All (p => ids.Contains (p.Light.Id))
                         select peace.Love).ToList ();
            if (loves.Count > 0) {
               var max = loves.Where(l => l.Truths.Any ()).Max (l => l.Peaces.Count);
               foreach (var love in loves.Where (l => l.Peaces.Count == max)) {
                  foreach (var truth in love.Truths) {
                     Love truthLove = null;
                     var truthIds = new List <int> ();
                     if (truth.Light != null)
                        truthIds.Add(truth.Light.Id);
                     if (truth.ParentId.HasValue && (!truth.Order.HasValue || truth.Order.Value > 0)) {
                        truthLove = this.Database.Love.Get (truth.ParentId.Value);
                        truthIds.AddRange (truthLove.Peaces.Select (p => p.Light.Id));
                     }
                     if ((truthLove == null && truthIds.All (ids.Contains)) ||
                         (truthIds.All (histIds.Contains) && !truth.Number.HasValue && (!truth.Order.HasValue || truth.Order.Value > 0)) ||
                         (truth.Light != null && truth.Order == null && model.ToAdd.Any (i => i.Id == truth.Light.Id))) {
                        continue;
                     }

                     if (truthLove == null && truthIds.Count <= 0)
                        continue;
                     var item = new SdwItem (truth) { History = history };
                     if (truthLove != null) {
                        item.IsLink = true;
                        var text = GetTitle (truthLove.Peaces);
                        if (truth.Light != null)
                           item.Title = text;
                        else {
                           item.Text = text;
                           item.Id = truthLove.Peaces.OrderBy (p => p.Order).Last ().Light.Id;
                        }
                        item.Parents = hash.Encode (truthLove.Peaces.Select (p => p.Light.Id));
                     } else if (truth.Light != null) {
                        var parents = new List <Peace> ();
                        var tempParents = (from peace in truth.Light.Peaces
                                           from p in peace.Love.Peaces
                                           where ids.Contains (p.Light.Id)
                                           select p).ToList();
                        foreach (var tp in tempParents) {
                           if (parents.Any (p => p.Light.Id == tp.Light.Id)) continue;
                           parents.Add (tp);
                        }
                        var parentIds = parents.Select (p => p.Light.Id).ToList ();
                        item.Parents = hash.Encode (parentIds);
                        item.IsSelected = histIds.Contains (truth.Light.Id);
                        item.Title = GetTitle ((parents.Count > 0) ? parents : truth.Number.HasValue || love.Peaces.Count > 1 ? love.Peaces : new List <Peace> ());
                        if (string.IsNullOrEmpty (item.Title)) item.History = string.Empty;
                     }
                     model.ToAdd.Add (item);
                  }
               }
               SetHeadersAndFooters (model);
            }
         }
         return PartialView (model);
      }

      /// <summary>
      /// Searches the database for the given text.
      /// </summary>
      /// <param name="text">Text to search for.</param>
      /// <returns>The requested results.</returns>
      [AllowAnonymous]
      public ActionResult Search (string text)
      {
         var query = LightSearch.Query (0, 100, text);
         var model = new LoveModel ();
         var lightIds = query.Select (kvp => kvp.Key);
         var lights = this.Database.Light.Get (l => lightIds.Contains (l.Id));
         foreach (var light in lights) {
            model.ToAdd.Add (new SdwItem (light));
         }
         ViewBag.Search = text;
         return PartialView ("Love", model);
      }

      /// <summary>
      /// Gets auto complete items for the given text.
      /// </summary>
      /// <param name="text">Text to search for.</param>
      /// <returns>The list of possible items for the given text.</returns>
      [AllowAnonymous]
      public ActionResult AutoComplete (string text)
      {
         Dictionary <int, string> query;
         if (text.Contains ('|')) {
            query = new Dictionary<int, string> ();
            var items = text.Split ('|');
            var lights = new List <Light> ();
            for (int i = 0; i < items.Length - 1; i++) {
               var t = items [i];
               var light = this.Database.Light.Get (l => l.Text == t).FirstOrDefault ();
               if (light != null) lights.Add (light);
            }
            var ids = new List <int> ();
            var loves = new List <Love> ();
            foreach (var light in lights) {
               ids.Add (light.Id);
               loves.Add (Helper.FindLove (this.Database, ids, false));
            }
            var last = items [items.Length - 1];
            int number;
            if (Int32.TryParse (last, out number)) {
               foreach (var love in loves) {
                  var title = love.Peaces.Aggregate (string.Empty, (current, peace) => current + (peace.Light.Text + "|"));
                  foreach (var truth in love.Truths) {
                     if (truth.Number.HasValue && truth.Number == number) {
                        query.Add (truth.Id, title + truth.Light.Text);
                     }
                  }
               }
            } else {
               foreach (var love in loves) {
                  foreach (var truth in love.Truths.Where (t => t.Light.Text.Contains (last))) {
                     query.Add (truth.Id, truth.Light.Text);
                  }
               }
            }
         } else {
            query = LightSearch.AutoComplete (text);
         }
         var result = new { suggestions = query.Select (kvp => new { value = kvp.Value, data = kvp.Key }) };
         return Json (result, JsonRequestBehavior.AllowGet);
      }

      /// <summary>
      /// Gets the title of the given list.
      /// </summary>
      /// <param name="peaces">The list of peace to get title for.</param>
      private static string GetTitle (ICollection <Peace> peaces)
      {
         var title = string.Empty;
         if (peaces.Count > 0) {
            foreach (var peace in peaces.OrderBy (p => p.Order)) {
               if (!string.IsNullOrEmpty (title))
                  title += " | ";
               title += peace.Light.Text;
            }
         }
         return title;
      }

      /// <summary>
      /// Sets the headers and footers.
      /// </summary>
      /// <param name="model">Sets the headers and footers for the model..</param>
      private static void SetHeadersAndFooters (LoveModel model)
      {
         var remove = new List<SdwItem> ();
         // Headers => Order = 0; Number = The corresponding item.
         foreach (var header in model.ToAdd.Where (li => li.Order.HasValue && li.Order == 0 && li.ParentId.HasValue)) {
            var item = model.ToAdd.FirstOrDefault (li => li.TruthId == header.ParentId);
            if (item != null) {
               if (header.IsSelected) item.IsSelected = true;
               item.Headers.Add (header);
               remove.Add (header);
            }
         }
         // Footers => Order = the negative index of the footer; Number = The corresponding item.
         foreach (var footer in model.ToAdd.Where (li => li.Order.HasValue && li.Order < 0 && li.ParentId.HasValue)) {
            var item = model.ToAdd.FirstOrDefault (li => li.TruthId == footer.ParentId);
            if (item != null) {
               if (footer.IsSelected) item.IsSelected = true;
               item.Footers.Add (footer);
               remove.Add (footer);
            }
         }
         foreach (var levelItem in remove) {
            model.ToAdd.Remove (levelItem);
         }
      }
   }
}
