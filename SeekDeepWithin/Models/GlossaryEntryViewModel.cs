using System;
using System.Collections.ObjectModel;
using SeekDeepWithin.Controllers;
using SeekDeepWithin.DataAccess;
using SeekDeepWithin.Pocos;

namespace SeekDeepWithin.Models
{
   public class GlossaryEntryViewModel : IRenderable
   {
      public GlossaryEntryViewModel (GlossaryEntry entry, SdwRenderer renderer)
      {
         this.Id = entry.Id;
         this.Text = entry.Text;
         this.Renderer = renderer;
         this.Headers = new Collection<HeaderFooterViewModel> ();
         this.Footers = new Collection <HeaderFooterViewModel> ();
         this.Links = new Collection <LinkViewModel> ();
         this.Styles = new Collection <StyleViewModel> ();

         foreach (var link in entry.Links)
            this.Links.Add (new LinkViewModel
            {
               StartIndex = link.StartIndex,
               EndIndex = link.EndIndex,
               Url = link.Link.Url,
               OpenInNewWindow = link.OpenInNewWindow
            });

         foreach (var style in entry.Styles)
            this.Styles.Add (new StyleViewModel
            {
               StartIndex = style.StartIndex,
               EndIndex = style.EndIndex,
               Start = style.Style.Start,
               End = style.Style.End,
               SpansMultiple = style.Style.SpansMultiple
            });

         foreach (var header in entry.Headers)
            this.Headers.Add (new HeaderFooterViewModel (header));
         foreach (var footer in entry.Footers)
            this.Footers.Add (new HeaderFooterViewModel (footer));
      }

      /// <summary>
      /// Gets or Sets the id of the entry.
      /// </summary>
      public int Id { get; set; }

      /// <summary>
      /// Gets or Sets the text of the glossary entry.
      /// </summary>
      public string Text { get; set; }

      /// <summary>
      /// Gets the list of headers for this entry.
      /// </summary>
      public Collection<HeaderFooterViewModel> Headers { get; set; }

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