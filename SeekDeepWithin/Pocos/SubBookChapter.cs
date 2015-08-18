using System;
using System.Collections.Generic;

namespace SeekDeepWithin.Pocos
{
   public class SubBookChapter : IDbTable
   {
      /// <summary>
      /// Gets the id of the item.
      /// </summary>
      public int Id { get; set; }

      /// <summary>
      /// Gets or Sets the order of the chapter.
      /// </summary>
      public int Order { get; set; }

      /// <summary>
      /// Gets or Sets if this chapter should be hidden or not.
      /// </summary>
      public bool Hide { get; set; }

      /// <summary>
      /// Gets or Sets the last time this was modified.
      /// </summary>
      public DateTime Modified { get; set; }

      /// <summary>
      /// Gets or Sets the default reading style.
      /// </summary>
      public bool DefaultToParagraph { get; set; }

      /// <summary>
      /// Gets or Sets the associated sub book.
      /// </summary>
      public virtual VersionSubBook SubBook { get; set; }

      /// <summary>
      /// Gets or Sets the associated chapter.
      /// </summary>
      public virtual Chapter Chapter { get; set; }

      /// <summary>
      /// Gets the header for this chapter.
      /// </summary>
      public virtual ChapterHeader Header { get; set; }

      /// <summary>
      /// Gets or Sets the list of passages.
      /// </summary>
      public virtual ICollection<PassageEntry> Passages { get; set; }

      /// <summary>
      /// Gets the footer for this chapter.
      /// </summary>
      public virtual ChapterFooter Footer { get; set; }
   }
}