using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using SeekDeepWithin.Domain;

namespace SeekDeepWithin.Models
{
   public class ChapterViewModel
   {
      /// <summary>
      /// Gets or Sets the id of this chapter.
      /// </summary>
      public int Id { get; set; }

      /// <summary>
      /// Gets or Sets the order of this chapter.
      /// </summary>
      [Required]
      public int Order { get; set; }

      /// <summary>
      /// Gets or Sets the name of this chapter.
      /// </summary>
      [Required]
      public string Name { get; set; }

      /// <summary>
      /// Gets or Sets the id of the parent sub book.
      /// </summary>
      public int SubBookId { get; set; }

      /// <summary>
      /// Gets or Sets the default reading style.
      /// </summary>
      public bool DefaultToParagraph { get; set; }

      /// <summary>
      /// Gets or Sets the sub book this chapter belongs to.
      /// </summary>
      public SubBookViewModel SubBook { get; set; }

      /// <summary>
      /// Gets or Sets the list of passages.
      /// </summary>
      public ICollection<PassageEntryViewModel> Passages { get; set; }
   }
}