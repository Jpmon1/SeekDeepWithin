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
         return PartialView ("Get", new LevelModel());
      }

      /// <summary>
      /// Gets the love.
      /// </summary>
      [HttpPost]
      [AllowAnonymous]
      public ActionResult Get (string data, int clicked, string items, bool? root)
      {
         var model = new LevelModel ();
         var loves = new List <Love> ();
         var hash = new Hashids ("GodisLove");
         var ids = hash.Decode (items).ToList ();
         var light = this.Database.Light.Get (clicked);
         ids.Add(clicked);
         foreach (var peace in light.Peaces) {
            loves.AddRange (peace.Loves.Where (l => l.Peaces.All (p => ids.Contains (p.Light.Id))));
         }
         if (loves.Count > 0) {
            var max = loves.Max (l => l.Peaces.Count);
            if (root.HasValue && root.Value)
               model.Items.Add (new LevelItem (light));
            foreach (var love in loves.Where (l => l.Peaces.Count == max)) {
               foreach (var truth in love.Truths) {
                  var item = new LevelItem (truth) { LoveId = love.Id};
                  var tempParents = (from peace in truth.Light.Peaces
                     from peaceLove in peace.Loves
                     from p in peaceLove.Peaces
                     where ids.Contains (p.Light.Id)
                     select p);
                  var parents = new List <Peace> ();
                  foreach (var tp in tempParents) {
                     if (parents.Any (p => p.Light.Id == tp.Light.Id)) continue;
                     parents.Add (tp);
                  }
                  item.Title = GetTitle ((int) item.Type, (item.Type == SdwType.Passage) ? love.Peaces : parents);
                  item.Parents = hash.Encode (parents.Select (p => p.Light.Id));
                  model.Items.Add (item);
               }
            }
            SetHeadersAndFooters (model);
         }
         return PartialView (model);
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
      private static void SetHeadersAndFooters (LevelModel model)
      {
         var remove = new List<LevelItem> ();
         foreach (var header in model.Items.Where (li => li.Type == SdwType.Header)) {
            var passage = model.Items.FirstOrDefault (li => li.Number == header.Number && li.Id != header.Id);
            if (passage != null) {
               passage.Headers.Add (header);
               remove.Add (header);
            }
         }
         foreach (var footer in model.Items.Where (li => li.Type == SdwType.Footer)) {
            var passage = model.Items.FirstOrDefault (li => li.Number == footer.Number);
            if (passage != null) {
               passage.Footers.Add (footer);
               remove.Add (footer);
            }
         }
         foreach (var levelItem in remove) {
            model.Items.Remove (levelItem);
         }
      }
   }
}
