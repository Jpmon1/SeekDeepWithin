using System.Collections.ObjectModel;
using DotNetOpenAuth.Messaging;
using SeekDeepWithin.Controllers;

namespace SeekDeepWithin.Models
{
   public class PassageViewModel
   {
      /// <summary>
      /// Initializes a new passage view model.
      /// </summary>
      public PassageViewModel ()
      {
         this.Links = new Collection <LinkViewModel> ();
         this.Styles = new Collection <StyleViewModel> ();
         this.Headers = new Collection <HeaderFooterViewModel> ();
         this.Footers = new Collection <HeaderFooterViewModel> ();
      }

      /// <summary>
      /// Gets or Sets the id of the passage.
      /// </summary>
      public int Id { get; set; }

      /// <summary>
      /// Gets or Sets the entry id.
      /// </summary>
      public int EntryId { get; set; }

      /// <summary>
      /// Gets or Sets the text of the passage.
      /// </summary>
      public string Text { get; set; }

      /// <summary>
      /// Gets or Sets the number of the passage.
      /// </summary>
      public int Number { get; set; }

      /// <summary>
      /// Gets or Sets the id of the chapter.
      /// </summary>
      public int ChapterId { get; set; }

      /// <summary>
      /// Gets or Sets the name of the chapter.
      /// </summary>
      public string ChapterName { get; set; }

      /// <summary>
      /// Gets or Sets the id of the sub book.
      /// </summary>
      public int SubBookId { get; set; }

      /// <summary>
      /// Gets or Sets the name of the sub book.
      /// </summary>
      public string SubBookName { get; set; }

      /// <summary>
      /// Gets or Sets the id of the version.
      /// </summary>
      public int VersionId { get; set; }

      /// <summary>
      /// Gets or Sets the name of the version.
      /// </summary>
      public string VersionName { get; set; }

      /// <summary>
      /// Gets the list of links for this passage.
      /// </summary>
      public Collection<LinkViewModel> Links { get; set; }

      /// <summary>
      /// Get or Sets the styles for this passage.
      /// </summary>
      public Collection<StyleViewModel> Styles { get; set; }

      /// <summary>
      /// Get or Sets the footers for this passage.
      /// </summary>
      public Collection<HeaderFooterViewModel> Footers { get; set; }

      /// <summary>
      /// Get or Sets the headers for this passage.
      /// </summary>
      public Collection<HeaderFooterViewModel> Headers { get; set; }

      /// <summary>
      /// Renders the passage.
      /// </summary>
      /// <returns>The html to display for the passage.</returns>
      public string Render ()
      {
         var renderer = new SdwRenderer { Text = this.Text };
         renderer.Links.AddRange (this.Links);
         renderer.Styles.AddRange (this.Styles);
         return renderer.Render ();
      }
   }
}