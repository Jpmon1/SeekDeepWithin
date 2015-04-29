﻿using System.Collections.Generic;

namespace SeekDeepWithin.Pocos
{
   public class GlossaryItem : IDbTable
   {
      /// <summary>
      /// Gets or Sets the id of this item.
      /// </summary>
      public int Id { get; set; }

      /// <summary>
      /// Gets or Sets the number of votes this entry has.
      /// </summary>
      public int Votes { get; set; }

      /// <summary>
      /// Get or Sets the parent glossary term.
      /// </summary>
      public virtual GlossaryTerm Term { get; set; }

      /// <summary>
      /// Get or Sets the source of this entry.
      /// </summary>
      public virtual GlossaryItemSource Source { get; set; }

      /// <summary>
      /// Gets or Sets the list of entries.
      /// </summary>
      public virtual ICollection<GlossaryEntry> Entries { get; set; }
   }
}
