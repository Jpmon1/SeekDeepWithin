using System;
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
               model.ToAdd.Add (new SdwItem (light) { Key = hash.Encode(light.Id)});
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
               var max = loves.Max (l => l.Peaces.Count);
               foreach (var love in loves.Where (l => l.Peaces.Count == max)) {
                  foreach (var truth in love.Truths) {
                     if (ids.Contains (truth.Light.Id) || model.ToAdd.Any (i=> i.Id == truth.Light.Id) ||
                        (histIds.Contains (truth.Light.Id) && !truth.Number.HasValue))
                        continue;
                     var item = new SdwItem (truth) { History = history };
                     if (truth.ParentId != null) {
                        var l = this.Database.Love.Get (truth.ParentId.Value);
                        item.Title = GetTitle (l.Peaces);
                        item.Parents = hash.Encode (l.Peaces.Select (p => p.Light.Id));
                        var parentIds = l.Peaces.Select (p => p.Light.Id).ToList ();
                        parentIds.Add (item.Id);
                        item.Key = hash.Encode (parentIds);
                     } else {
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
                        parentIds.Add (item.Id);
                        item.Key = hash.Encode (parentIds);
                        item.IsSelected = histIds.Contains (truth.Light.Id);
                        item.Title = GetTitle ((parents.Count > 0) ? parents : truth.Number.HasValue ? love.Peaces : new List <Peace> ());
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
      /// Gets the truth with the given id.
      /// </summary>
      /// <param name="id">Id of truth to get.</param>
      /// <returns>JSON results.</returns>
      public ActionResult Truth (int id)
      {
         var truth = this.Database.Truth.Get (id);
         if (truth == null) return this.Fail ("That is an unknown truth!");
         return Json (new {
            id,
            status = SUCCESS,
            order = truth.Order,
            number = truth.Number,
            text = truth.Light.Text
         }, JsonRequestBehavior.AllowGet);
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
         var hash = new Hashids ("GodisLove") { Order = true };
         var lights = this.Database.Light.Get (l => lightIds.Contains (l.Id));
         foreach (var light in lights) {
            model.ToAdd.Add (new SdwItem (light) { Key = hash.Encode (light.Id) });
         }
         ViewBag.Search = text.GetWords ();
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
         var query = LightSearch.AutoComplete (text);
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
                  title += "|";
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
         foreach (var header in model.ToAdd.Where (li => li.Order.HasValue && li.Order == 0 && li.Number.HasValue)) {
            var passage = model.ToAdd.FirstOrDefault (li => li.Number == header.Number && li.Id != header.Id);
            if (passage != null) {
               passage.Headers.Add (header);
               remove.Add (header);
            }
         }
         foreach (var footer in model.ToAdd.Where (li => li.Order.HasValue && li.Order < 0 && li.Number.HasValue)) {
            var passage = model.ToAdd.FirstOrDefault (li => li.Number == footer.Number);
            if (passage != null) {
               passage.Footers.Add (footer);
               remove.Add (footer);
            }
         }
         foreach (var levelItem in remove) {
            model.ToAdd.Remove (levelItem);
         }
      }
   }
}
