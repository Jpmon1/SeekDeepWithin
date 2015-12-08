using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using SeekDeepWithin.DataAccess;
using SeekDeepWithin.Models;
using SeekDeepWithin.Pocos;
using SeekDeepWithin.SdwSearch;

namespace SeekDeepWithin.Controllers
{
   public class ItemEntryController : SdwController
   {
      /// <summary>
      /// Initializes a new controller.
      /// </summary>
      public ItemEntryController () : base (new SdwDatabase ()) { }

      /// <summary>
      /// Initializes a new controller with the given db info.
      /// </summary>
      /// <param name="db">Database object.</param>
      public ItemEntryController (ISdwDatabase db) : base (db) { }

      /// <summary>
      /// Creates a passage for the given chapter.
      /// </summary>
      /// <param name="entryList">The list of passages to add.</param>
      /// <param name="itemId">The id of the item.</param>
      /// <returns>Create results.</returns>
      [HttpPost]
      [ValidateAntiForgeryToken]
      [Authorize (Roles = "Creator")]
      public ActionResult Create (string entryList, int itemId)
      {
         if (string.IsNullOrWhiteSpace (entryList)) return this.Fail ("No entries were given to add.");
         var item = this.Database.TermItems.Get (itemId);
         if (item == null) return this.Fail ("Unable to determine the item.");
         var entries = entryList.Split (new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
         foreach (var entry in entries)
         {
            var entryData = entry.Split ('|');
            var order = entryData.FirstOrDefault (pd => pd.StartsWith ("[o]"));
            if (string.IsNullOrWhiteSpace (order)) return this.Fail ("Passage order was not supplied.");
            var text = entryData.FirstOrDefault (pd => pd.StartsWith ("[t]"));
            if (string.IsNullOrWhiteSpace (text)) return this.Fail ("Passage text was not supplied.");
            var header = entryData.FirstOrDefault (pd => pd.StartsWith ("[h]"));
            var termEntry = new TermItemEntry
            {
               Order = Convert.ToInt32 (order.Substring (3)),
               Text = text.Substring (3)
            };
            if (!string.IsNullOrWhiteSpace (header))
               termEntry.Header = new TermItemEntryHeader { Text = header.Substring (3) };
            var reg = new Regex ("\\[f@(\\d+)\\](.+)");
            foreach (var footer in entryData.Where (pd => pd.StartsWith ("[f@")))
            {
               var match = reg.Match (footer);
               if (match.Success)
               {
                  var index = Convert.ToInt32 (match.Groups[1].Value);
                  var fText = match.Groups[2].Value;
                  if (termEntry.Footers == null) termEntry.Footers = new Collection<TermItemEntryFooter> ();
                  termEntry.Footers.Add (new TermItemEntryFooter { Index = index, Text = fText });
               }
            }
            item.Entries.Add (termEntry);
         }
         this.Database.Save ();
         GlossarySearch.AddOrUpdateIndex (item.Entries);
         GlossarySearch.Optimize ();
         return Json ("Success");
      }

      /// <summary>
      /// Performs an edit for the given entry.
      /// </summary>
      /// <param name="entryId">The entry to edit.</param>
      /// <param name="text">The entry text.</param>
      /// <param name="order">The entry order.</param>
      /// <param name="header">A header for the entry.</param>
      /// <returns>Results.</returns>
      [HttpPost]
      [ValidateAntiForgeryToken]
      [Authorize (Roles = "Editor")]
      public ActionResult Update (int entryId, string text, int? order, string header)
      {
         var entry = this.Database.TermItemEntries.Get (entryId);
         if (entry == null) return this.Fail ("Unable to determine the item entry.");
         if (order != null) entry.Order = order.Value;
         entry.Text = text;
         if (!string.IsNullOrWhiteSpace (header))
         {
            if (entry.Header == null)
               entry.Header = new TermItemEntryHeader { Text = header };
            else
               entry.Header.Text = header;
         }
         else if (entry.Header != null)
         {
            entry.Header.Styles.Clear();
            entry.Header = null;
         }
         this.Database.Save ();
         GlossarySearch.AddOrUpdateIndex (entry);
         return Json ("Success");
      }

      /// <summary>
      /// Gets the edit view for the given entry.
      /// </summary>
      /// <param name="id">The id of the item we editing.</param>
      /// <returns></returns>
      [Authorize (Roles = "Editor")]
      public ActionResult Edit (int id)
      {
         var entry = this.Database.TermItemEntries.Get (id);
         var viewModel = new EditItemViewModel (id, EditItemType.ItemEntry) { Text = entry.Text };
         foreach (var style in entry.Styles)
            viewModel.Styles.Add (new StyleViewModel (style));
         foreach (var link in entry.Links)
            viewModel.Links.Add (new LinkViewModel (link));
         foreach (var footer in entry.Footers)
            viewModel.Footers.Add (new HeaderFooterViewModel (footer));
         return PartialView ("_EditItem", viewModel);
      }

      /// <summary>
      /// Gets the edit view for the given entry.
      /// </summary>
      /// <param name="id">The id of the item we editing.</param>
      /// <returns></returns>
      [Authorize (Roles = "Editor")]
      public ActionResult EditHeader (int id)
      {
         var entry = this.Database.TermItemEntries.Get (id);
         var viewModel = new EditItemViewModel (id, EditItemType.ItemEntryHeader)
         {
            HasLinks = false,
            HasFooters = false
         };
         if (entry.Header != null)
         {
            viewModel.Text = entry.Header.Text;
            foreach (var style in entry.Header.Styles)
               viewModel.Styles.Add (new StyleViewModel (style));
         }
         return PartialView ("_EditItem", viewModel);
      }

      /// <summary>
      /// Gets the edit view for the given entry.
      /// </summary>
      /// <param name="id">The id of the item we editing.</param>
      /// <param name="footerId">The id of the footer we are editing.</param>
      /// <returns></returns>
      [Authorize (Roles = "Editor")]
      public ActionResult EditFooter (int id, int footerId)
      {
         var entry = this.Database.TermItemEntries.Get (id);
         if (entry == null) return this.Fail ("Unable to determine the entry");
         var footer = entry.Footers.FirstOrDefault (f => f.Id == footerId);
         if (footer == null) return this.Fail ("Unable to determine the footer");
         var viewModel = new EditItemViewModel (id, EditItemType.ItemEntryFooter)
         {
            Text = footer.Text,
            HasFooters = false,
            FooterId = footerId
         };
         foreach (var style in footer.Styles)
            viewModel.Styles.Add (new StyleViewModel (style));
         foreach (var link in footer.Links)
            viewModel.Links.Add (new LinkViewModel (link));
         return PartialView ("_EditItem", viewModel);
      }

      /// <summary>
      /// Deletes the given entry.
      /// </summary>
      /// <param name="entryId">The entry to delete.</param>
      /// <returns>Results.</returns>
      [HttpPost]
      [ValidateAntiForgeryToken]
      [Authorize (Roles = "Creator")]
      public ActionResult Delete (int entryId)
      {
         var entry = this.Database.TermItemEntries.Get (entryId);
         if (entry == null) return this.Fail ("Unable to determine the item entry.");
         entry.Item.Entries.Remove (entry);
         this.Database.TermItemEntries.Delete (entry);
         this.Database.Save ();
         GlossarySearch.Delete (entry.Id);
         return Json ("Success");
      }

      /// <summary>
      /// Gets the header data for the given chapter.
      /// </summary>
      /// <param name="id">The id of the chapter to get header data for.</param>
      /// <returns>JSON results</returns>
      public ActionResult GetHeader (int id)
      {
         var entry = this.Database.TermItemEntries.Get (id);
         if (entry.Header == null)
            return Json (new { status = SUCCESS, text = string.Empty }, JsonRequestBehavior.AllowGet);
         var result = new {
            status = SUCCESS,
            text = entry.Header.Text,
            styles = entry.Header.Styles.Select (s => new {
               id = s.Id,
               start = s.Style.Start,
               end = s.Style.End,
               startindex = s.StartIndex,
               endindex = s.EndIndex,
               multispan = s.Style.SpansMultiple
            })
         };
         return Json (result, JsonRequestBehavior.AllowGet);
      }

      /// <summary>
      /// Gets the footer data for the given chapter.
      /// </summary>
      /// <param name="id">The id of the chapter to get footer data for.</param>
      /// <returns>JSON results</returns>
      public ActionResult GetFooter (int id)
      {
         var entry = this.Database.TermItemEntries.Get (id);
         var result = new {
            status = SUCCESS,
            footers = entry.Footers.Select (f => new {
               text = f.Text,
               styles = f.Styles.Select (s => new {
                  id = s.Id,
                  start = s.Style.Start,
                  end = s.Style.End,
                  startindex = s.StartIndex,
                  endindex = s.EndIndex,
                  multispan = s.Style.SpansMultiple
               }),
               links = f.Links.Select (l => new {
                  id = l.Id,
                  url = l.Link.Url,
                  endindex = l.EndIndex,
                  startindex = l.StartIndex,
                  newwindow = l.OpenInNewWindow
               })
            })
         };
         return Json (result, JsonRequestBehavior.AllowGet);
      }

      /// <summary>
      /// Gets details about the entry with the given id.
      /// </summary>
      /// <param name="id"></param>
      /// <returns></returns>
      [AllowAnonymous]
      public ActionResult Get (int id)
      {
         var entry = this.Database.TermItemEntries.Get (id);
         if (entry == null) return this.Fail ("Unable to determine the item entry.");
         var result = new
         {
            entryId = id,
            text = entry.Text,
            order = entry.Order,
            header = entry.Header == null ? string.Empty : entry.Header.Text
         };
         return Json (result, JsonRequestBehavior.AllowGet);
      }
   }
}