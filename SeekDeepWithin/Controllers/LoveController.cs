﻿using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Newtonsoft.Json.Linq;
using SeekDeepWithin.DataAccess;
using SeekDeepWithin.Models;

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
      /// Gets the love.
      /// </summary>
      /// <returns></returns>
      public ActionResult Get (string data, int lastId, bool? reload)
      {
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
         return PartialView (rtnLevels);
      }
   }
}
