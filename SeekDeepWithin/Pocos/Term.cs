using System;
using System.Collections.Generic;

namespace SeekDeepWithin.Pocos
{
   /// <summary>
   /// Represents a glossary term.
   /// </summary>
   public class Term : IDbTable
   {
      /// <summary>
      /// Gets or Sets the id of this item.
      /// </summary>
      public int Id { get; set; }

      /// <summary>
      /// Get or Sets name of this item.
      /// </summary>
      public string Name { get; set; }

      /// <summary>
      /// Gets or Sets if this should be hidden or not.
      /// </summary>
      public bool Hide { get; set; }

      /// <summary>
      /// Gets or Sets the last time this was modified.
      /// </summary>
      public DateTime Modified { get; set; }

      /// <summary>
      /// Get or Sets the list of linked items.
      /// </summary>
      public virtual ICollection<TermLink> Links { get; set; }

      /// <summary>
      /// Get or Sets the list of entries.
      /// </summary>
      public virtual ICollection<TermItem> Items { get; set; }

      /// <summary>
      /// Get or Sets the list of see also.
      /// </summary>
      public virtual ICollection<TermAmazonItem> AmazonItems { get; set; }
   }
}
