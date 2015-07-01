using System.Collections.Generic;

namespace SeekDeepWithin.Pocos
{
   public class TermItemEntry : IDbTable
   {
      /// <summary>
      /// Gets or Sets the id of this item.
      /// </summary>
      public int Id { get; set; }

      /// <summary>
      /// Gets or Sets the order of the entry.
      /// </summary>
      public int Order { get; set; }

      /// <summary>
      /// Gets or Sets the text of the entry.
      /// </summary>
      public string Text { get; set; }

      /// <summary>
      /// Gets or Sets the parent item.
      /// </summary>
      public virtual TermItem Item { get; set; }

      /// <summary>
      /// Gets the header for this entry.
      /// </summary>
      public virtual TermItemEntryHeader Header { get; set; }

      /// <summary>
      /// Gets the list of footers for this entry.
      /// </summary>
      public virtual ICollection<TermItemEntryFooter> Footers { get; set; }

      /// <summary>
      /// Gets the list of links for this entry.
      /// </summary>
      public virtual ICollection<TermItemEntryLink> Links { get; set; }

      /// <summary>
      /// Gets the list of styles for this entry.
      /// </summary>
      public virtual ICollection<TermItemEntryStyle> Styles { get; set; }
   }
}