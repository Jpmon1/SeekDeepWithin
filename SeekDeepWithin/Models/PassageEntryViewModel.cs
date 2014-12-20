using System.Collections.Generic;
using DotNetOpenAuth.Messaging;
using SeekDeepWithin.Controllers;

namespace SeekDeepWithin.Models
{
   public class PassageEntryViewModel
   {
      /// <summary>
      /// Gets or Sets the id of the passage entry.
      /// </summary>
      public int Id { get; set; }

      /// <summary>
      /// Gets or Sets the passage id.
      /// </summary>
      public int PassageId { get; set; }

      /// <summary>
      /// Gets or Sets the chapter id.
      /// </summary>
      public int ChapterId { get; set; }

      /// <summary>
      /// Gets or Sets the number of the passage entry.
      /// </summary>
      public int Number { get; set; }

      /// <summary>
      /// Gets or Sets the order of the passage entry.
      /// </summary>
      public int Order { get; set; }

      /// <summary>
      /// Gets or Sets the chapter of this entry.
      /// </summary>
      public ChapterViewModel Chapter { get; set; }

      /// <summary>
      /// Gets or Sets the passage of this entry.
      /// </summary>
      public PassageViewModel Passage { get; set; }

      /// <summary>
      /// Get or Sets the styles for this entry.
      /// </summary>
      public ICollection<StyleViewModel> Styles { get; set; }

      /// <summary>
      /// Renders the passage entry.
      /// </summary>
      /// <returns>The rendered html of the passage entry.</returns>
      public string Render ()
      {
         var renderer = new SdwRenderer {Text = this.Passage.Text};
         renderer.Links.AddRange (this.Passage.PassageLinks);
         renderer.Styles.AddRange (this.Styles);
         return renderer.Render();
      }
   }
}