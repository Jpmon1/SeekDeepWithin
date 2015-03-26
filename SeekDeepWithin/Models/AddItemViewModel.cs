using System.ComponentModel.DataAnnotations;

namespace SeekDeepWithin.Models
{
   /// <summary>
   /// View model for adding passages.
   /// </summary>
   public class AddItemViewModel
   {
      /// <summary>
      /// Gets or Sets the item type.
      /// </summary>
      public ItemType ItemType { get; set; }

      /// <summary>
      /// Gets or Sets the add operation is an insert operation.
      /// </summary>
      public bool IsInsert { get; set; }

      /// <summary>
      /// Gets or Sets the text of the passage.
      /// </summary>
      [Required]
      [Display (Name = "Passage Text")]
      public string Text { get; set; }

      /// <summary>
      /// Gets or Sets the order for the passage.
      /// </summary>
      [Required]
      public int Order { get; set; }

      /// <summary>
      /// Gets or Sets the passage number.
      /// </summary>
      [Required]
      public int Number { get; set; }

      /// <summary>
      /// Gets or Sets the ID of the chapter this passage belongs to.
      /// </summary>
      [Required]
      public int ParentId { get; set; }
   }
}