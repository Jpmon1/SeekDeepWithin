using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using SeekDeepWithin.DataAccess;
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
      /// Creates love.
      /// </summary>
      [HttpPost]
      [Authorize (Roles = "Creator")]
      public ActionResult Create (string parents)
      {
         if (string.IsNullOrWhiteSpace (parents)) return this.Fail ("You must specify some light for love to bind.");
         var parentIds = parents.Split (new [] { '|' }, StringSplitOptions.RemoveEmptyEntries).Select(id => Convert.ToInt32 (id)).ToArray ();
         var byteArray = new byte [parentIds.Length * sizeof (int)];
         Buffer.BlockCopy (parentIds, 0, byteArray, 0, byteArray.Length);
         var base64 = Convert.ToBase64String (byteArray);
         var lights = parentIds.Select (parentId => this.Database.Light.Get (parentId)).ToList ();
         var love = new Love { Lights = lights, Modified = DateTime.Now, ParentId = base64 };
         this.Database.Love.Insert (love);
         /*int[] result = new int[byteArray.Length / sizeof(int)];
         Buffer.BlockCopy(byteArray, 0, result, 0, result.Length);*/
         this.Database.Save ();
         return Json (new { status = SUCCESS, id = love.Id });
      }

      /// <summary>
      /// Edits love.
      /// </summary>
      [HttpPost]
      [Authorize (Roles = "Editor")]
      public ActionResult Edit (int id, int childType, int number)
      {
         var love = this.Database.Love.Get (id);
         if (love == null) return this.Fail ("Unable to find the love.");
         /*love.ChildType = childType;
         love.Number = number <= 0 ? null : (int?) number;
         love.Modified = DateTime.Now;*/
         this.Database.Save ();
         return this.Success ();
      }

      /// <summary>
      /// Reorders love.
      /// </summary>
      [HttpPost]
      [Authorize (Roles = "Editor")]
      public ActionResult Reorder (string order)
      {
         var loveAndOrders = order.Split (new []{'|'}, StringSplitOptions.RemoveEmptyEntries);
         foreach (var loveAndOrder in loveAndOrders) {
            if (string.IsNullOrWhiteSpace (loveAndOrder)) continue;
            var loveSplit = loveAndOrder.Split ('-');
            var loveId = Convert.ToInt32 (loveSplit [0]);
            var loveOrder = Convert.ToInt32 (loveSplit [1]);
            var love = this.Database.Love.Get (loveId);
            if (love == null) return this.Fail ("Unable to find the love - " + loveId);
            /*if (love.Order != loveOrder) {
               love.Order = loveOrder == 0 ? null : (int?)loveOrder;
               love.Modified = DateTime.Now;
            }*/
         }
         this.Database.Save ();
         return this.Success ();
      }

      /// <summary>
      /// Gets the list of truth types.
      /// </summary>
      /// <returns>A JSON reslut.</returns>
      public ActionResult GetTypes ()
      {
         var values = Enum.GetValues (typeof (TruthType)).Cast<TruthType> ();
         return Json (new {
            status = SUCCESS,
            types = values.Select (t => new {
               name = t.ToString (),
               value = (int) t
            })
         }, JsonRequestBehavior.AllowGet);
      }

      /// <summary>
      /// Gets love for the given light.
      /// </summary>
      /// <returns>A JSON result.</returns>
      public ActionResult Get (int lightId)
      {
         var lights = new List <Light> ();
         var loves = this.Database.Love.Get (love => love.Lights.Any (l => l.Id == lightId)) .ToList ();
         foreach (var love in loves) {
            foreach (var light in love.Lights) {
               if (lights.All (l => l.Id != light.Id)) {
                  lights.Add (light);
               }
            }
         }
         return Json (new {
            status = SUCCESS,
            count = lights.Count,
            light = lights.Select (l => new {
               id = l.Id,
               text = l.Text,
               modified = l.Modified.ToString (CultureInfo.InvariantCulture),
               key = Helper.Base64Encode (l.Id.ToString (CultureInfo.InvariantCulture))
            })
         }, JsonRequestBehavior.AllowGet);
      }
   }
}
