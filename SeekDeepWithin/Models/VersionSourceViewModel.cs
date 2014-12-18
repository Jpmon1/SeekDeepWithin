using System.ComponentModel.DataAnnotations;

namespace SeekDeepWithin.Models
{
   public class VersionSourceViewModel
   {
      /// <summary>
      /// Gets or Sets the source name.
      /// </summary>
      [Required]
      [Display (Name = "Source Name")]
      public string SourceName { get; set; }

      /// <summary>
      /// Gets or Sets the source url.
      /// </summary>
      [Required]
      [Display (Name = "Source Url")]
      public string SourceUrl { get; set; }

      /// <summary>
      /// Gets or Sets the id of the version.
      /// </summary>
      public int VersionId { get; set; }
   }
}