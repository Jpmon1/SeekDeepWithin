using System;
using System.Collections.ObjectModel;
using SeekDeepWithin.Controllers;
using SeekDeepWithin.DataAccess;
using SeekDeepWithin.Pocos;

namespace SeekDeepWithin.Models
{
   public class TermItemEntryViewModel : IRenderable
   {
      public TermItemEntryViewModel (TermItemEntry entry, SdwRenderer renderer)
      {
         this.Id = entry.Id;
         this.Text = entry.Text;
         this.Order = entry.Order;
         this.Renderer = renderer;
         if (entry.Item != null)
         {
            if (entry.Item.Term != null)
            {
               this.TermId = entry.Item.Term.Id;
               this.TermName = entry.Item.Term.Name;
            }
            else
            {
               this.TermId = -1;
               this.TermName = "Null Term";
            }
         }
         else
         {
            this.TermId = -1;
            this.TermName = "Null Item";
         }
         this.Footers = new Collection <HeaderFooterViewModel> ();
         this.Links = new Collection <LinkViewModel> ();
         this.Styles = new Collection <StyleViewModel> ();

         if (entry.Header != null)
            this.Header = new HeaderFooterViewModel (entry.Header);
         foreach (var link in entry.Links)
            this.Links.Add (new LinkViewModel (link));
         foreach (var style in entry.Styles)
            this.Styles.Add (new StyleViewModel (style));
         foreach (var footer in entry.Footers)
            this.Footers.Add (new HeaderFooterViewModel (footer));
      }

      /// <summary>
      /// Gets or Sets the term id.
      /// </summary>
      public int TermId { get; set; }

      /// <summary>
      /// Gets or Sets the term name.
      /// </summary>
      public string TermName { get; set; }

      /// <summary>
      /// Gets or Sets the order.
      /// </summary>
      public int Order { get; set; }

      /// <summary>
      /// Gets or Sets the id of the entry.
      /// </summary>
      public int Id { get; set; }

      /// <summary>
      /// Gets or Sets the text of the glossary entry.
      /// </summary>
      public string Text { get; set; }

      /// <summary>
      /// Gets the header for this entry.
      /// </summary>
      public HeaderFooterViewModel Header { get; set; }

      /// <summary>
      /// Gets the list of footers for this entry.
      /// </summary>
      public Collection<HeaderFooterViewModel> Footers { get; set; }

      /// <summary>
      /// Gets the list of links for this entry.
      /// </summary>
      public Collection<LinkViewModel> Links { get; set; }

      /// <summary>
      /// Gets the list of styles for this entry.
      /// </summary>
      public Collection<StyleViewModel> Styles { get; set; }

      /// <summary>
      /// Gets or Sets the renderer.
      /// </summary>
      public SdwRenderer Renderer { get; set; }

      /// <summary>
      /// Renders the passage.
      /// </summary>
      /// <returns>The html to display for the passage.</returns>
      public string Render (Uri url)
      {
         if (this.Text.StartsWith ("|PARSE|"))
         {
            var parser = new PassageParser (new SdwDatabase ());
            parser.Parse(this.Text.Substring(7));
            return parser.BuildHtmlOutput (url);
         }
         if (this.Renderer == null)
            this.Renderer = new SdwRenderer ();
         return Renderer.Render (this);
      }
   }
}