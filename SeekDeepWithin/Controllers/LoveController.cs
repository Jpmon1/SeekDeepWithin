using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SeekDeepWithin.DataAccess;
using SeekDeepWithin.Models;
using SeekDeepWithin.Pocos;

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
      /// <returns>The view for history.</returns>
      public ActionResult Load (int id)
      {
         var rtnLevels = new List<LevelModel> ();
         var history = this.Database.Histories.Get (id);
         if (history != null) {
            var loveIds = new List<int> ();
            var truthIds = new List<int> ();
            var levels = JArray.Parse (history.Json);
            foreach (dynamic l in levels) {
               var level = new LevelModel { Index = rtnLevels.Count };
               foreach (var i in l) {
                  var levelItem = new LevelItem ();
                  if (i.l == null || i.t == null) {
                     levelItem.Id = (int) i.i;
                  } else {
                     levelItem.LoveId = (int) i.l;
                     levelItem.TruthId = (int) i.t;
                     if (!loveIds.Contains (levelItem.LoveId))
                        loveIds.Add (levelItem.LoveId);
                     if (!truthIds.Contains (levelItem.TruthId))
                        truthIds.Add (levelItem.TruthId);
                  }
                  level.Items.Add (levelItem);
               }
               rtnLevels.Add (level);
            }
            levels = JArray.Parse (history.LevelItems);
            foreach (dynamic l in levels) {
               var level = rtnLevels.FirstOrDefault (lm => lm.Index == (int) l.n);
               if (level == null) {
                  level = new LevelModel {Index = (int) l.n};
                  rtnLevels.Add (level);
               }
               foreach (var li in l.i) {
                  var levelItem = new LevelItem {LoveId = (int) li.l, TruthId = (int) li.t};
                  if (!loveIds.Contains (levelItem.LoveId))
                     loveIds.Add (levelItem.LoveId);
                  if (!truthIds.Contains (levelItem.TruthId))
                     truthIds.Add (levelItem.TruthId);
                  level.Items.Add (levelItem);
               }
            }
            var love = this.Database.Love.Get (l => loveIds.Contains (l.Id)).ToList ();
            var truth = this.Database.Truth.Get (t => truthIds.Contains (t.Id)).ToList ();
            foreach (var level in rtnLevels) {
               foreach (var li in level.Items) {
                  if (li.LoveId <= 0) {
                     var light = this.Database.Light.Get (li.Id);
                     li.Text = light.Text;
                  } else {
                     li.Update (love.FirstOrDefault (l => l.Id == li.LoveId),
                        truth.FirstOrDefault (t => t.Id == li.TruthId));
                  }
               }
            }
         }
         return PartialView ("Get", rtnLevels.OrderBy (l => l.Index).ToList ());
      }

      /// <summary>
      /// Gets the love.
      /// </summary>
      /// <returns></returns>
      public ActionResult Get (string data, int lastId)
      {
         var history = this.Database.Histories.Get (h => h.Json == data).FirstOrDefault ();
         if (history == null) {
            history = new History { Json = data };
            this.Database.Histories.Insert (history);
         }
         LevelModel model = null;
         var levels = JArray.Parse (data);
         var light = this.Database.Light.Get (lastId);
         var currentLevels = new List <LevelModel> ();
         foreach (var level in levels) {
            var newModel = new LevelModel { Previous = model, Index = currentLevels.Count };
            if (model != null) model.Next = newModel;
            foreach (dynamic item in level) {
               var levelItem = new LevelItem {
                  Id = (int) item.i,
                  Level = newModel,
                  ShowAll = item.a != null,
                  IsSelected = item.s != null,
                  LoveId = item.l == null ? 0 : (int) item.l,
                  TruthId = item.t == null ? 0 : (int) item.t
               };
               newModel.Items.Add (levelItem);
               levelItem.SetSelection ();
            }
            currentLevels.Add (newModel);
            model = newModel;
         }
         var loves = new List <int> ();
         foreach (var love in light.Loves) {
            foreach (var li in love.Lights) {
               if (loves.All (l => l != li.Id))
                  loves.Add (li.Id);
            }
         }
         var rtnLevels = new List <LevelModel> ();
         foreach (var levelModel in currentLevels) {
            var level = new LevelModel { Index = levelModel.Index + 1 };
            var existingLevel = currentLevels.FirstOrDefault (l => l.Index == levelModel.Index + 1);
            foreach (var levelItem in levelModel.Items) {
               if (levelItem.IsSelected && loves.Contains (levelItem.Id)) {
                  var possibleLove = light.Loves.Where (l => l.Lights.All (li => levelItem.Selection.Contains (li.Id))).ToList ();
                  if (possibleLove.Count > 0) {
                     var toAdd = new List<LevelItem> ();
                     var max = possibleLove.Max (l => l.Lights.Count);
                     foreach (var l in possibleLove.Where (lo => lo.Lights.Count == max))
                        toAdd.AddRange (l.Truths.OrderBy (t => t.Type).ThenBy (t => t.Order ?? 0).Select (t => new LevelItem (l, t)));
                     if (toAdd.Any (t => t.Id == levelItem.Id)) continue;
                     foreach (var truth in toAdd) {
                        if ((existingLevel == null || existingLevel.Items.All (i => i.Id != truth.Id)) &&
                           level.Items.All(li => li.Id != truth.Id)) {
                           level.Items.Add (truth);
                        }
                     }
                  }
               }
            }
            if (level.Items.Count > 0) {
               if (existingLevel != null) {
                  var carryOver = existingLevel.Items.Where (i => level.Items.All (li => li.TruthId != i.TruthId)).ToList ();
                  if (carryOver.Any ()) {
                     var truthIds = carryOver.Select (c => c.TruthId).ToList ();
                     var loveIds = carryOver.Select (c => c.LoveId).ToList ();
                     var truths = this.Database.Truth.Get (t => truthIds.Contains (t.Id));
                     var love = this.Database.Love.Get (l => loveIds.Contains (l.Id)).ToList();
                     foreach (var truth in truths) {
                        var item = carryOver.FirstOrDefault (c => c.TruthId == truth.Id);
                        level.Items.Add (new LevelItem (item == null ? null : love.FirstOrDefault (l => l.Id == item.LoveId), truth));
                     }
                  }
               }
               rtnLevels.Add (level);
            }
         }
         history.LevelItems = JsonConvert.SerializeObject (rtnLevels.Select (l => new {
            n = l.Index,
            i = l.Items.Select (i => new {
               l = i.LoveId,
               t = i.TruthId
            })
         }), Formatting.None);
         this.Database.Save();
         ViewBag.HistoryId = history.Id;
         return PartialView (rtnLevels);
      }
   }
}
