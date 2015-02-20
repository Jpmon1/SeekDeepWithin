using System.Collections.Generic;

namespace SeekDeepWithin.Pocos
{
   public class GlossaryEntry : IDbTable
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
      /// Gets the list of headers for this entry.
      /// </summary>
      public virtual ICollection<GlossaryEntryHeader> Headers { get; set; }

      /// <summary>
      /// Gets the list of footers for this entry.
      /// </summary>
      public virtual ICollection<GlossaryEntryFooter> Footers { get; set; }

      /// <summary>
      /// Gets the list of links for this entry.
      /// </summary>
      public virtual ICollection<GlossaryEntryLink> Links { get; set; }

      /// <summary>
      /// Gets the list of styles for this entry.
      /// </summary>
      public virtual ICollection<GlossaryEntryStyle> Styles { get; set; }
   }
}