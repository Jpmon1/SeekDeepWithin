using System.Collections.Generic;

namespace SeekDeepWithin.Pocos
{
   public class Chapter : IDbTable
   {
      /// <summary>
      /// Gets or Sets the id of this chapter.
      /// </summary>
      public int Id { get; set; }

      /// <summary>
      /// Gets or Sets the name of this chapter.
      /// </summary>
      public string Name { get; set; }

      /// <summary>
      /// Gets or Sets the default reading style.
      /// </summary>
      public bool DefaultToParagraph { get; set; }

      /// <summary>
      /// Gets or Sets the sub book this chapter belongs to.
      /// </summary>
      public virtual SubBook SubBook { get; set; }

      /// <summary>
      /// Gets or Sets the list of passages.
      /// </summary>
      public virtual ICollection<PassageEntry> Passages { get; set; }

      /// <summary>
      /// Gets the list of headers for this chapter.
      /// </summary>
      public virtual ICollection<ChapterHeader> Headers { get; set; }

      /// <summary>
      /// Gets the list of footers for this chapter.
      /// </summary>
      public virtual ICollection<ChapterFooter> Footers { get; set; }
   }
}
