using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DotNetOpenAuth.Messaging;
using SeekDeepWithin.Controllers;

namespace SeekDeepWithin.Models
{
   /// <summary>
   /// View model for a version.
   /// </summary>
   public class VersionViewModel
   {
      /// <summary>
      /// Gets the id of the table.
      /// </summary>
      public int Id { get; set; }

      /// <summary>
      /// Gets or Sets an abbreviation to use for this version.
      /// </summary>
      public string Abbreviation { get; set; }

      /// <summary>
      /// Gets or Sets the book id.
      /// </summary>
      public int BookId { get; set; }

      /// <summary>
      /// Gets or Sets the format the title should be displayed in.
      /// </summary>
      [Display (Name = "Title Format")]
      public string TitleFormat { get; set; }

      /// <summary>
      /// Gets the formatted title of the version.
      /// </summary>
      public string Title
      {
         get
         {
            if (string.IsNullOrEmpty (this.TitleFormat))
               return this.Book.Title;
            return this.TitleFormat.Replace ("{B}", this.Book.Title).Replace ("{V}", this.Name);
         }
      }

      /// <summary>
      /// Gets or Sets the name of the table.
      /// </summary>
      public string Name { get; set; }

      /// <summary>
      /// Gets or Sets the date the version was published.
      /// </summary>
      [Display (Name = "Published Date")]
      public string PublishDate { get; set; }

      /// <summary>
      /// Gets or Sets the summary.
      /// </summary>
      public string About { get; set; }

      /// <summary>
      /// Gets or Sets the source name.
      /// </summary>
      public string SourceName { get; set; }

      /// <summary>
      /// Gets or Sets the source url.
      /// </summary>
      public string SourceUrl { get; set; }

      /// <summary>
      /// Gets or Sets the book of the version
      /// </summary>
      public BookViewModel Book { get; set; }

      /// <summary>
      /// Gets or Sets the links of this version.
      /// </summary>
      public virtual ICollection<LinkViewModel> VersionAboutLinks { get; set; }

      /// <summary>
      /// Gets or Sets the links of this version.
      /// </summary>
      public virtual ICollection<StyleViewModel> VersionAboutStyles { get; set; }

      /// <summary>
      /// Gets or Sets the list of authors.
      /// </summary>
      public ICollection<WriterLink> Writers { get; set; }

      /// <summary>
      /// Gets or Sets the list of sub books.
      /// </summary>
      public ICollection<SubBookViewModel> SubBooks { get; set; }

      /// <summary>
      /// Renders the about content.
      /// </summary>
      /// <returns>Html for about content.</returns>
      public string RenderAbout ()
      {
         var renderer = new SdwRenderer { Text = this.About };
         renderer.Links.AddRange (this.VersionAboutLinks);
         renderer.Styles.AddRange (this.VersionAboutStyles);
         return renderer.Render ();
      }
   }
}