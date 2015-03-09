using System.ComponentModel.DataAnnotations;

namespace SeekDeepWithin.Models
{
   /// <summary>
   /// Editing view model for a link.
   /// </summary>
   public class EditLinkViewModel
   {
      /// <summary>
      /// Initializes a new edit link view model.
      /// </summary>
      public EditLinkViewModel ()
      {
         this.LinkItemId = -1;
      }

      /// <summary>
      /// Gets or Sets the item id of the link.
      /// </summary>
      public int LinkItemId { get; set; }

      /// <summary>
      /// Gets or Sets if the link should open in a new window.
      /// </summary>
      public bool OpenInNewWindow { get; set; }

      /// <summary>
      /// Gets or Sets the glossary term of the link.
      /// </summary>
      public string GlossaryTerm { get; set; }

      /// <summary>
      /// Gets or Sets and anchor for the link.
      /// </summary>
      public string Anchor { get; set; }

      /// <summary>
      /// Gets or Sets the book of the link.
      /// </summary>
      public string Book { get; set; }

      /// <summary>
      /// Gets or Sets the version of the link.
      /// </summary>
      public string Version { get; set; }

      /// <summary>
      /// Gets or Sets the sub book of the link.
      /// </summary>
      public string SubBook { get; set; }

      /// <summary>
      /// Gets or Sets the chapter of the link.
      /// </summary>
      public string Chapter { get; set; }

      /// <summary>
      /// Gets or Sets the id of the chapter.
      /// </summary>
      public int ChapterId { get; set; }

      /// <summary>
      /// Gets or Sets the search text of the link.
      /// </summary>
      public string Search { get; set; }

      /// <summary>
      /// Gets or Sets the external url of the link.
      /// </summary>
      public string Link { get; set; }

      /// <summary>
      /// Gets or Sets the type of link to add.
      /// </summary>
      public string Type { get; set; }

      /// <summary>
      /// Gets or Sets the start index of the link.
      /// </summary>
      [Required]
      public int StartIndex { get; set; }

      /// <summary>
      /// Gets or Sets the end index of the link.
      /// </summary>
      [Required]
      public int EndIndex { get; set; }
   }
}