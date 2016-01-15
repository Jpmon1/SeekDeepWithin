using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
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
      /// <param name="data">Data of history to load.</param>
      /// <returns>The view for history.</returns>
      public ActionResult Load (string data)
      {
         var loveIds = new List<int> ();
         var truthIds = new List<int> ();
         var selected = ParseData (data);
         var rtnLevels = this.ParseLevels (selected);
         foreach (var level in rtnLevels) {
            foreach (var li in level.Items) {
               if (li.LoveId > 0 && !loveIds.Contains (li.LoveId))
                  loveIds.Add (li.LoveId);
               if (li.TruthId > 0 && !truthIds.Contains (li.TruthId))
                  truthIds.Add (li.TruthId);
            }
         }
         var love = this.Database.Love.Get (l => loveIds.Contains (l.Id)).ToList ();
         var truth = this.Database.Truth.Get (t => truthIds.Contains (t.Id)).ToList ();
         foreach (var level in rtnLevels) {
            foreach (var li in level.Items) {
               if (li.LoveId <= 0) {
                  var light = this.Database.Light.Get (li.Id);
                  if (light != null) li.Text = light.Text;
               } else {
                  li.Update (love.FirstOrDefault (l => l.Id == li.LoveId),
                     truth.FirstOrDefault (t => t.Id == li.TruthId));
               }
            }
         }
         return PartialView ("Get", rtnLevels.OrderBy (l => l.Index).ToList ());
      }

      /// <summary>
      /// Gets the love.
      /// </summary>
      [HttpPost]
      [AllowAnonymous]
      public ActionResult Get (string data, int clicked, int levelId)
      {
         var rtnLevels = new List<LevelModel> ();
         List<LevelModel> currentLevels;
         var light = this.Database.Light.Get (clicked);
         var selected = ParseData (data);
         if (selected.Count <= 0) {
            selected = new Dictionary <int, List <int>> ();
            currentLevels = new List<LevelModel> ();
            var m = new LevelModel {Index = 0};
            var root = new LevelItem { Id = light.Id, Text = light.Text, IsSelected = true, Level = m };
            m.Items.Add (root);
            rtnLevels.Add (m);
            currentLevels.Add (m);
            root.SetSelection ();
         } else {
            currentLevels = this.ParseLevels (selected);
         }
         var connections = new List <int> ();
         foreach (var peace in light.Peaces) {
            foreach (var love in peace.Loves) {
               foreach (var p in love.Peaces) {
                  if (connections.All (l => l != p.Light.Id))
                     connections.Add (p.Light.Id);
               }
            }
         }
         foreach (var levelModel in currentLevels) {
            var level = new LevelModel { Index = levelModel.Index + 1, Parent = levelModel.Key };
            LevelModel existingLevel = null;//currentLevels.FirstOrDefault (l => l.Parent == levelModel.Key);
            foreach (var levelItem in levelModel.Items) {
               if (levelItem.IsSelected && connections.Contains (levelItem.Id)) {
                  var possibleLove = new List <Love> ();
                  foreach (var peace in light.Peaces) {
                     possibleLove.AddRange (peace.Loves.Where (l => l.Peaces.All (p => levelItem.Selection.Contains (p.Light.Id))));
                  }
                  if (possibleLove.Count > 0) {
                     var toAdd = new List<LevelItem> ();
                     var max = possibleLove.Max (l => l.Peaces.Count);
                     foreach (var l in possibleLove.Where (lo => lo.Peaces.Count == max))
                        toAdd.AddRange (l.Truths.Select (t => new LevelItem (l, t)));
                     if (toAdd.Any (t => t.Id == levelItem.Id)) continue;
                     foreach (var truth in toAdd) {
                        if ((existingLevel == null || existingLevel.Items.All (i => i.Id != truth.Id)) &&
                           level.Items.All (li => li.Id != truth.Id)) {
                           if (connections.Any (l => l == truth.Id)) {
                              truth.OtherLights.Add (clicked);
                           }
                           level.Items.Add (truth);
                        }
                     }
                  }
               }
            }
            if (level.Items.Count > 0) {
               /*if (existingLevel != null) {
                  var carryOver = existingLevel.Items.Where (i => level.Items.All (li => li.TruthId != i.TruthId)).ToList ();
                  if (carryOver.Any ()) {
                     level.Replace = existingLevel.Key;
                     var truthIds = carryOver.Select (c => c.TruthId).ToList ();
                     var loveIds = carryOver.Select (c => c.LoveId).ToList ();
                     var truths = this.Database.Truth.Get (t => truthIds.Contains (t.Id));
                     var love = this.Database.Love.Get (l => loveIds.Contains (l.Id)).ToList();
                     foreach (var truth in truths) {
                        var item = carryOver.FirstOrDefault (c => c.TruthId == truth.Id);
                        level.Items.Add (new LevelItem (item == null ? null : love.FirstOrDefault (l => l.Id == item.LoveId), truth));
                     }
                  }
               }*/
               rtnLevels.Add (level);
            }
         }
         var replaceKey = LayoutClicked (clicked, currentLevels.FirstOrDefault (l => l.Key == levelId), rtnLevels);
         foreach (var level in rtnLevels) {
            var levelData = level.GetData ();
            var soul = this.Database.Soul.Get (s => s.Data == levelData).FirstOrDefault();
            if (soul == null) {
               soul = new Soul {Data = levelData};
               this.Database.Soul.Insert (soul);
               this.Database.Save ();
            }
            level.Key = soul.Id;
            var item = selected.FirstOrDefault (kvp => kvp.Key == soul.Id);
            if (item.Value == null)
               selected.Add (soul.Id, selected.Count == 0 ? new List<int> (new[]{clicked}) : new List <int> ());
         }
         /*if (replaceKey > 0) {
            var newKey = rtnLevels.FirstOrDefault (l => l.Replace == replaceKey);
            if (newKey != null) {
               foreach (var level in rtnLevels.Where (l => l.Parent == replaceKey && l.Index > 0)) { level.Parent = newKey.Key; }
            }
         }*/
         var selList = new List <int> ();
         foreach (var kvp in selected) {
            selList.Add (0);
            selList.Add (kvp.Key);
            selList.AddRange (kvp.Value);
         }
         var hash = new Hashids ("GodisLove");
         ViewBag.HistoryId = hash.Encode (selList);
         return PartialView (rtnLevels);
      }

      /// <summary>
      /// Re-layout the clicked level.
      /// </summary>
      /// <param name="clicked">The light that was clicked.</param>
      /// <param name="clickedLevel">The level that was clicked.</param>
      /// <param name="rtnLevels">The list of return levels.</param>
      /// <returns>The replace key.</returns>
      private int LayoutClicked (int clicked, LevelModel clickedLevel, IList <LevelModel> rtnLevels)
      {
         var replaceKey = -1;
         if (clickedLevel != null && clickedLevel.Items.Count > 1) {
            replaceKey = clickedLevel.Key;
            var level = new LevelModel {Index = clickedLevel.Index, Parent = clickedLevel.Parent, Replace = clickedLevel.Key};
            var subLevel = new LevelModel {Parent = clickedLevel.Key, Index = -1};
            var loveIds = clickedLevel.Items.Select (i => i.LoveId).ToList ();
            var truthIds = clickedLevel.Items.Select (i => i.TruthId).ToList ();
            var loves = this.Database.Love.Get (t => loveIds.Contains (t.Id));
            var truths = this.Database.Truth.Get (t => truthIds.Contains (t.Id));
            var items = clickedLevel.Items.Select ( i => new LevelItem (loves.FirstOrDefault (l => l.Id == i.LoveId),
                        truths.FirstOrDefault (t => t.Id == i.TruthId)));
            foreach (var item in items) {
               item.IsSelected = item.Id == clicked;
               if (item.Id == clicked || item.Type == SdwType.Summary ||
                   item.Type == SdwType.SubTitle || item.Type == SdwType.Version ||
                   item.Type == SdwType.Date) {
                  level.Items.Add (item);
               } else {
                  subLevel.Items.Add (item);
               }
            }
            if (subLevel.Items.Count > 0 && level.Items.Count > 0) {
               rtnLevels.Insert (0, subLevel);
               rtnLevels.Add (level);
            } else {
               replaceKey = -1;
            }
         }
         return replaceKey;
      }

      /// <summary>
      /// Parses the list of selected ids.
      /// </summary>
      /// <param name="data">The list of selected ids.</param>
      /// <returns>A dictionary with selected ids.</returns>
      private static Dictionary<int, List<int>> ParseData (string data)
      {
         var hash = new Hashids ("GodisLove");
         var dataList = hash.Decode (data);
         var selected = new Dictionary <int, List <int>> ();
         List <int> currSel = null;
         for (int i = 0; i < dataList.Length; i++) {
            if (dataList [i] == 0) {
               currSel = new List <int> ();
               selected.Add (dataList [++i], currSel);
            } else {
               if (currSel != null) { currSel.Add (dataList [i]); }
            }
         }
         return selected;
      }

      /// <summary>
      /// Parses the data to get levels.
      /// </summary>
      /// <param name="data">The current data.</param>
      /// <returns>The list of levels.</returns>
      private List<LevelModel> ParseLevels (Dictionary<int, List<int>> data)
      {
         var currentLevels = new List <LevelModel> ();
         var soulIds = data.Select (kvp => kvp.Key).ToList();
         var souls = this.Database.Soul.Get (s => soulIds.Contains (s.Id)).OrderBy (s => soulIds.IndexOf (s.Id));
         LevelModel model = null;
         foreach (var soul in souls) {
            var selLevel = data.FirstOrDefault (s => s.Key == soul.Id);
            var newModel = new LevelModel {Previous = model, Index = currentLevels.Count, Key = soul.Id};
            if (model != null) { model.Next = newModel; }
            var items = soul.Data.Split ('|');
            foreach (var item in items) {
               var ids = item.Split ('.');
               var id = Convert.ToInt32 (ids [0]);
               var levelItem = new LevelItem {
                  Id = id,
                  Level = newModel,
                  IsSelected = selLevel.Value != null && selLevel.Value.Any (i => i == id),
                  LoveId = Convert.ToInt32 (ids [1]),
                  TruthId = Convert.ToInt32 (ids [2])
               };
               newModel.Items.Add (levelItem);
               levelItem.SetSelection ();
            }
            currentLevels.Add (newModel);
            model = newModel;
         }
         return currentLevels;
      }
   }
}
