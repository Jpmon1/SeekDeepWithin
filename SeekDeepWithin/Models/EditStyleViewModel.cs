using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SeekDeepWithin.Models
{
   /// <summary>
   /// Editing view model for a style.
   /// </summary>
   public class EditStyleViewModel
   {
      /// <summary>
      /// Gets or Sets the item id of the style.
      /// </summary>
      public int Id { get; set; }

      /// <summary>
      /// Gets or Sets beginning style tag(s).
      /// </summary>
      [Required]
      [AllowHtml]
      [Display (Name = "Beginning Style Tag(s)")]
      public string StartStyle { get; set; }

      /// <summary>
      /// Gets or Sets the ending style tag(s).
      /// </summary>
      [Required]
      [AllowHtml]
      [Display (Name = "Ending Style Tag(s)")]
      public string EndStyle { get; set; }

      /// <summary>
      /// Gets or Sets the start index of the style.
      /// </summary>
      [Required]
      [Display (Name = "Start Index")]
      public int StartIndex { get; set; }

      /// <summary>
      /// Gets or Sets the end index of the style.
      /// </summary>
      [Required]
      [Display (Name = "End Index")]
      public int EndIndex { get; set; }

      /// <summary>
      /// Gets or Sets if this style spans multiple items (entries/passages).
      /// </summary>
      public bool SpansMultiple { get; set; }

      /// <summary>
      /// Gets or Sets the parent id, if needed.
      /// </summary>
      public int ParentId { get; set; }
   }
}