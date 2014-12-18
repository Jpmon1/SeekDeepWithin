using System.Collections.Generic;
using SeekDeepWithin.Domain;

namespace SeekDeepWithin.Models
{
   public class SubBookViewModel
   {
      /// <summary>
      /// Gets or Sets the id of this sub book.
      /// </summary>
      public int Id { get; set; }

      /// <summary>
      /// Gets or Sets the id of the version this sub book belongs to.
      /// </summary>
      public int VersionId { get; set; }

      /// <summary>
      /// Gets or Sets the order of this sub book.
      /// </summary>
      public int Order { get; set; }

      /// <summary>
      /// Gets or Sets the name of this sub book.
      /// </summary>
      public string Name { get; set; }

      /// <summary>
      /// Gets or Sets the version of this sub book.
      /// </summary>
      public VersionViewModel Version { get; set; }

      /// <summary>
      /// Gets or Sets the list of chapters.
      /// </summary>
      public ICollection<ChapterViewModel> Chapters { get; set; }
   }
}