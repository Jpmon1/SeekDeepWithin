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
   public class LoveController : SdwController
   {
      /// <summary>
      /// Initializes a new controller.
      /// </summary>
      public LoveController () : base (new SdwDatabase ()) { }

      /// <summary>
      /// Initializes a new controller with the given db info.
      /// </summary>
      /// <param name="db">Database object.</param>
      public LoveController (ISdwDatabase db) : base (db) { }

      /// <summary>
      /// Loads the given history item.
      /// </summary>
      /// <param name="id">Id of history to load.</param>
      /// <param name="prevId">Id of previous history.</param>
      /// <returns>The view for history.</returns>
      public ActionResult Load (int id, int? prevId)
      {
         var soul = this.Database.Soul.Get (id);
         var items = soul.Data.Split ('|').ToList ();
         var hash = new Hashids ("GodisLove") { Order = true };
         var soulItems = items.Select (i => new SoulItem (i, hash)).ToList ();
         var truthIds = new List <int> ();
         var lightIds = soulItems.Where (s => s.IsLight).Select (s => s.Id).ToList ();
         foreach (var item in soulItems.Where(s => !s.IsLight)) {
            lightIds.AddRange (item.ParentLights);
            truthIds.Add (item.Id);
         }
         var lights = this.Database.Light.Get (l => lightIds.Contains (l.Id)).ToList ();
         var truths = this.Database.Truth.Get (t => truthIds.Contains (t.Id)).ToList ();
         var model = new ItemModel ();
         foreach (var item in soulItems) {
            if (item.IsLight) {
               model.ToAdd.Add (new SdwItem (lights.FirstOrDefault (l => l.Id == item.Id)) {
                  Key = item.Key,
                  IsSelected = item.IsSelected
               });
            } else {
               var truth = truths.FirstOrDefault (t => t.Id == item.Id);
               if (truth != null) {
                  var sdwItem = new SdwItem (truth) { Key = item.Key, IsSelected = item.IsSelected };
                  var ids = hash.Decode (item.Key).ToList ();
                  ids.Remove (truth.Light.Id);
                  sdwItem.Parents = hash.Encode (ids);
                  foreach (var i in ids) {
                     var light = lights.FirstOrDefault (l => l.Id == i);
                     if (light != null) {
                        if (!string.IsNullOrEmpty (sdwItem.Title))
                           sdwItem.Title += "|";
                        sdwItem.Title += light.Text;
                     }
                  }
                  model.ToAdd.Add (sdwItem);
               }
            }
         }
         return PartialView ("Get", model);
      }

      /// <summary>
      /// Gets the love.
      /// </summary>
      [AllowAnonymous]
      public ActionResult Get (int? id, string items, int? current)
      {
         var model = new ItemModel ();
         var hash = new Hashids ("GodisLove") { Order = true };
         var ids = hash.Decode (items).ToList ();
         if (id == null) {
            var lights = this.Database.Light.All (q => q.OrderBy(l => Guid.NewGuid())).Take (25);
            foreach (var light in lights) {
               model.ToAdd.Add (new SdwItem (light) { Key = hash.Encode(light.Id)});
            }
         } else {
            var loves = new List <Love> ();
            var light = this.Database.Light.Get (id.Value);
            ids.Add (id.Value);
            foreach (var peace in light.Peaces) {
               loves.AddRange (peace.Loves.Where (l => l.Peaces.All (p => ids.Contains (p.Light.Id))));
            }
            if (loves.Count > 0) {
               var max = loves.Max (l => l.Peaces.Count);
               foreach (var love in loves.Where (l => l.Peaces.Count == max)) {
                  foreach (var truth in love.Truths) {
                     if (ids.Contains (truth.Light.Id)) continue;
                     var item = new SdwItem (truth);
                     if (truth.ParentId != null) {
                        var l = this.Database.Love.Get (truth.ParentId.Value);
                        item.Title = GetTitle ((int) item.Type, l.Peaces);
                        item.Parents = hash.Encode (l.Peaces.Select (p => p.Light.Id));
                        var parentIds = l.Peaces.Select (p => p.Light.Id).ToList ();
                        parentIds.Add (item.Id);
                        item.Key = hash.Encode (parentIds);
                     } else {
                        var parents = new List <Peace> ();
                        var tempParents = (from peace in truth.Light.Peaces
                           from peaceLove in peace.Loves
                           from p in peaceLove.Peaces
                           where ids.Contains (p.Light.Id)
                           select p);
                        foreach (var tp in tempParents) {
                           if (parents.Any (p => p.Light.Id == tp.Light.Id)) continue;
                           parents.Add (tp);
                        }
                        var parentIds = parents.Select (p => p.Light.Id).ToList ();
                        item.Parents = hash.Encode (parentIds);
                        parentIds.Add (item.Id);
                        item.Key = hash.Encode (parentIds);
                        item.Title = GetTitle ((int) item.Type, (parents.Count > 0) ? parents : love.Peaces);
                     }
                     model.ToAdd.Add (item);
                  }
               }
               SetHeadersAndFooters (model);
            }
         }
         ViewBag.HistoryId = this.UpdateHistory (hash.Encode (ids), current, model);
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
         var model = new ItemModel ();
         var lightIds = query.Select (kvp => kvp.Key);
         var hash = new Hashids ("GodisLove") { Order = true };
         var lights = this.Database.Light.Get (l => lightIds.Contains (l.Id));
         foreach (var light in lights) {
            model.ToAdd.Add (new SdwItem (light) { Key = hash.Encode (light.Id) });
         }
         return PartialView ("Get", model);
      }

      /// <summary>
      /// Gets the title of the given list.
      /// </summary>
      /// <param name="currType">The current type.</param>
      /// <param name="peaces">The list of peace to get title for.</param>
      private static string GetTitle (int currType, IEnumerable <Peace> peaces)
      {
         var title = string.Empty;
         foreach (var peace in peaces.OrderBy (p => p.Type)) {
            if (peace.Type == currType) continue;
            if (!string.IsNullOrEmpty (title))
               title += "|";
            title += peace.Light.Text;
         }
         return title;
      }

      /// <summary>
      /// Sets the headers and footers.
      /// </summary>
      /// <param name="model">Sets the headers and footers for the model..</param>
      private static void SetHeadersAndFooters (ItemModel model)
      {
         var remove = new List<SdwItem> ();
         foreach (var header in model.ToAdd.Where (li => li.Type == SdwType.Header)) {
            var passage = model.ToAdd.FirstOrDefault (li => li.Number == header.Number && li.Id != header.Id);
            if (passage != null) {
               passage.Headers.Add (header);
               remove.Add (header);
            }
         }
         foreach (var footer in model.ToAdd.Where (li => li.Type == SdwType.Footer)) {
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

      /// <summary>
      /// Updates the history.
      /// </summary>
      /// <param name="insertKey"></param>
      /// <param name="current"></param>
      /// <param name="model"></param>
      /// <returns></returns>
      private int UpdateHistory (string insertKey, int? current, ItemModel model)
      {
         List <string> items;
         if (!current.HasValue || string.IsNullOrEmpty (insertKey)) {
            items = model.ToAdd.Select (a => a.GetData ()).ToList ();
         } else {
            var soul = this.Database.Soul.Get (current.Value);
            items = soul.Data.Split ('|').ToList ();
            var insert = items.FirstOrDefault (i => i.EndsWith (insertKey));
            var toRemove = items.Where (i => model.ToAdd.Any (a => i.EndsWith (a.Key))).ToList ();
            for (int i = 0; i < toRemove.Count; i++) {
               var r = toRemove [i];
               if (r == insert) continue;
               items.Remove (r);
               model.ToRemove.Add (r.Substring (r.LastIndexOf (',') + 1));
            }
            var index = items.IndexOf (insert);
            if (insert!= null && !insert.StartsWith("S,"))
               items [index] = "S," + insert;
            if (index == -1)
               items.AddRange (model.ToAdd.Select (a => a.GetData ()));
            else
               items.InsertRange (index + 1, model.ToAdd.Select (a => a.GetData ()));
         }
         var data = string.Empty;
         foreach (var item in items) {
            if (!string.IsNullOrEmpty (data))
               data += "|";
            data += item;
         }
         var newSoul = this.Database.Soul.Get (s => s.Data == data).FirstOrDefault ();
         if (newSoul == null) {
            newSoul = new Soul {Data = data};
            this.Database.Soul.Insert (newSoul);
            this.Database.Save ();
         }
         return newSoul.Id;
      }
   }
}
