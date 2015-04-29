using System.ComponentModel.DataAnnotations;

namespace SeekDeepWithin.Models
{
   public class SourceViewModel
   {
      /// <summary>
      /// Gets or Sets the id of the source.
      /// </summary>
      public int Id { get; set; }

      /// <summary>
      /// Gets or Sets the type of source (entry, version, etc...)
      /// </summary>
      public string Type { get; set; }

      /// <summary>
      /// Gets or Sets the name of this source.
      /// </summary>
      [Required]
      [Display (Name = "Source Name")]
      public string Name { get; set; }

      /// <summary>
      /// Gets or Sets the url for the source.
      /// </summary>
      [Display (Name = "Source Url")]
      public string Url { get; set; }

      /// <summary>
      /// Gets or Sets any additional data for the source.
      /// </summary>
      [Display (Name = "Additional Information")]
      public string Data { get; set; }

      /// <summary>
      /// Gets or Sets the parent id, if needed.
      /// </summary>
      public int ParentId { get; set; }
   }
}