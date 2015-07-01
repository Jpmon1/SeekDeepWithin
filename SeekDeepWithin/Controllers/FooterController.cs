using System.Linq;
using System.Web.Mvc;
using SeekDeepWithin.DataAccess;
using SeekDeepWithin.Pocos;
using SeekDeepWithin.SdwSearch;

namespace SeekDeepWithin.Controllers
{
   /// <summary>
   /// Controller used for footers.
   /// </summary>
   public class FooterController : SdwController
   {
      /// <summary>
      /// Initializes a new controller.
      /// </summary>
      public FooterController () : base (new SdwDatabase ()) { }

      /// <summary>
      /// Initializes a new controller with the given db info.
      /// </summary>
      /// <param name="db">Database object.</param>
      public FooterController (ISdwDatabase db) : base (db) { }

      /// <summary>
      /// Creates a footer.
      /// </summary>
      /// <param name="itemId">The item id.</param>
      /// <param name="text">The text of the footer.</param>
      /// <param name="index">The index of the footer.</param>
      /// <returns>Result</returns>
      [HttpPost]
      [ValidateAntiForgeryToken]
      [Authorize (Roles = "Editor")]
      public ActionResult CreatePassage (int itemId, string text, int index)
      {
         if (itemId <= 0)
            return this.Fail ("Unable to determine the passage.");
         if (string.IsNullOrWhiteSpace (text))
            return this.Fail ("The footer must have some text.");

         var entry = this.Database.PassageEntries.Get (itemId);
         var footer = new PassageFooter { Text = text, Index = index };
         entry.Footers.Add (footer);
         this.Database.Save ();
         PassageSearch.AddOrUpdateIndex (entry);
         return Json (new { id = footer.Id, index = footer.Index });
      }

      /// <summary>
      /// Creates a footer.
      /// </summary>
      /// <param name="itemId">The item id.</param>
      /// <param name="text">The text of the footer.</param>
      /// <param name="index">The index of the footer.</param>
      /// <returns>Result</returns>
      [HttpPost]
      [ValidateAntiForgeryToken]
      [Authorize (Roles = "Editor")]
      public ActionResult CreateItemEntry (int itemId, string text, int index)
      {
         if (itemId <= 0)
            return this.Fail ("Unable to determine the item entry.");
         if (string.IsNullOrWhiteSpace (text))
            return this.Fail ("The footer must have some text.");

         var entry = this.Database.TermItemEntries.Get (itemId);
         var footer = new TermItemEntryFooter { Text = text, Index = index };
         entry.Footers.Add (footer);
         this.Database.Save ();
         GlossarySearch.AddOrUpdateIndex (entry);
         return Json (new { id = footer.Id, index = footer.Index });
      }

      /// <summary>
      /// Updates a footer.
      /// </summary>
      /// <param name="itemId">The item id.</param>
      /// <param name="id">The id of the footer to edit.</param>
      /// <param name="text">The text of the footer.</param>
      /// <param name="index">The index of the footer.</param>
      /// <returns>Result</returns>
      [HttpPost]
      [ValidateAntiForgeryToken]
      [Authorize (Roles = "Editor")]
      public ActionResult UpdatePassage (int itemId, int id, string text, int index)
      {
         var entry = this.Database.PassageEntries.Get (itemId);
         if (entry == null) return Fail ("Unable to determine passage.");
         var footer = entry.Footers.FirstOrDefault (f => f.Id == id);
         if (footer == null) return Fail ("Unable to determine footer.");
         footer.Text = text;
         footer.Index = index;
         this.Database.Save ();
         return Json ("Success");
      }

      /// <summary>
      /// Updates a footer.
      /// </summary>
      /// <param name="itemId">The item id.</param>
      /// <param name="id">The id of the footer to edit.</param>
      /// <param name="text">The text of the footer.</param>
      /// <param name="index">The index of the footer.</param>
      /// <returns>Result</returns>
      [HttpPost]
      [ValidateAntiForgeryToken]
      [Authorize (Roles = "Editor")]
      public ActionResult UpdateItemEntry (int itemId, int id, string text, int index)
      {
         var entry = this.Database.TermItemEntries.Get (itemId);
         if (entry == null) return Fail ("Unable to determine item entry.");
         var footer = entry.Footers.FirstOrDefault (f => f.Id == id);
         if (footer == null) return Fail ("Unable to determine footer.");
         footer.Text = text;
         footer.Index = index;
         this.Database.Save ();
         return Json ("Success");
      }

      /// <summary>
      /// Deletes a footer.
      /// </summary>
      /// <param name="id">The id of the footer to delete.</param>
      /// <param name="itemId">Id of the footer's parent to delete from.</param>
      /// <returns>Result</returns>
      [HttpPost]
      [ValidateAntiForgeryToken]
      [Authorize (Roles = "Editor")]
      public ActionResult DeletePassage (int id, int itemId)
      {
         var entry = this.Database.PassageEntries.Get (itemId);
         if (entry == null) return Fail ("Unable to determine passage.");
         var footer = entry.Footers.FirstOrDefault (f => f.Id == id);
         if (footer == null) return Fail ("Unable to determine footer.");
         entry.Footers.Remove (footer);
         this.Database.Save ();
         PassageSearch.AddOrUpdateIndex (entry);
         return Json ("Success");
      }

      /// <summary>
      /// Deletes a footer.
      /// </summary>
      /// <param name="id">The id of the footer to delete.</param>
      /// <param name="itemId">Id of the footer's parent to delete from.</param>
      /// <returns>Result</returns>
      [HttpPost]
      [ValidateAntiForgeryToken]
      [Authorize (Roles = "Editor")]
      public ActionResult DeleteItemEntry (int id, int itemId)
      {
         var entry = this.Database.TermItemEntries.Get (itemId);
         if (entry == null) return Fail ("Unable to determine item entry.");
         var footer = entry.Footers.FirstOrDefault (f => f.Id == id);
         if (footer == null) return Fail ("Unable to determine footer.");
         entry.Footers.Remove (footer);
         this.Database.Save ();
         GlossarySearch.AddOrUpdateIndex (entry);
         return Json ("Success");
      }

      /// <summary>
      /// Gets a footer.
      /// </summary>
      /// <param name="id">The id of the footer to get.</param>
      /// <param name="itemId">Id of the footer's parent.</param>
      /// <returns>Result</returns>
      public ActionResult GetPassage (int id, int itemId)
      {
         var entry = this.Database.PassageEntries.Get (itemId);
         if (entry == null) return Fail ("Unable to determine passage.");
         var footer = entry.Footers.FirstOrDefault (f => f.Id == id);
         if (footer == null) return Fail ("Unable to determine footer.");
         return Json (new {id, itemId, index = footer.Index, text = footer.Text}, JsonRequestBehavior.AllowGet);
      }

      /// <summary>
      /// Gets a footer.
      /// </summary>
      /// <param name="id">The id of the footer to get.</param>
      /// <param name="itemId">Id of the footer's parent.</param>
      /// <returns>Result</returns>
      public ActionResult GetItemEntry (int id, int itemId)
      {
         var entry = this.Database.TermItemEntries.Get (itemId);
         if (entry == null) return Fail ("Unable to determine item entry.");
         var footer = entry.Footers.FirstOrDefault (f => f.Id == id);
         if (footer == null) return Fail ("Unable to determine footer.");
         return Json (new { id, itemId, index = footer.Index, text = footer.Text }, JsonRequestBehavior.AllowGet);
      }
   }
}
